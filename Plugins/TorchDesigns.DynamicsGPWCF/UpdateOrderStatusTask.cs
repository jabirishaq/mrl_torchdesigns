using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Xml;
using Nop.Core.Domain.Logging;
using Nop.Core.Infrastructure;
using Nop.Core.Domain.Tasks;
using Nop.Core.Domain.Orders;
using Nop.Core.Data;
using System.Configuration;
using System.Data.SqlClient;
using Nop.Core.Domain.Payments;
using Nop.Plugin.TorchDesigns.DynamicsGP.DynamicsGP;
using Nop.Plugin.TorchDesigns.DynamicsGP;
using Nop.Core.Domain.Shipping;
using Nop.Core.Domain.Messages;
using Nop.Services.Tasks;
using Nop.Services.Orders;
using Nop.Services.Messages;
using Nop.Services.Shipping;
using System.Linq;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Logging;
using Nop.Services.Customers;
using Nop.Services.Tax;
using Nop.Core;
using Nop.Core.Domain.Common;


namespace Nop.Plugin.TorchDesigns.DynamicsGP
{
    class UpdateOrderStatusTask : ITask
    {
        #region Fields

        private readonly IOrderService _orderService;
        private readonly IMessageTemplateService _messageService;
        private readonly IOrderProcessingService _orderProcessingService;
        private readonly IShipmentService _shipmentService;
        private readonly IShippingService _shippingService;
        private readonly IEmailAccountService _emailAccountService;
        private readonly EmailAccountSettings _emailAccountSettings;
        private readonly IQueuedEmailService _queuedEmailService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ISettingService _settingService;
        private readonly ILogger _logger;
        private readonly ITaxService _taxService;
        private readonly IAddressService _addressService;
        private readonly ICustomerService _customerService;
        GreatPlains _greatPlains;
        
        #endregion


        public UpdateOrderStatusTask(IOrderService orderService, IMessageTemplateService messageService, IOrderProcessingService orderProcessingService, IShipmentService shipmentService, 
            IShippingService shippingService, IEmailAccountService emailAccountService, EmailAccountSettings emailAccountSettings, IQueuedEmailService queuedEmailService, 
            IGenericAttributeService genericAttributeService, ISettingService settingService, ICustomerService customerService, ILogger logger, ITaxService taxService, IAddressService addressService)
        {
            this._orderService = orderService;
            this._messageService = messageService;
            this._orderProcessingService = orderProcessingService;
            this._shipmentService = shipmentService;
            this._shippingService = shippingService;
            this._emailAccountService = emailAccountService;
            this._emailAccountSettings = emailAccountSettings;
            this._queuedEmailService = queuedEmailService;
            this._genericAttributeService = genericAttributeService;
            this._settingService = settingService;
            this._logger = logger;
            this._taxService = taxService;
            this._addressService = addressService;
            this._customerService = customerService;
        }

        public void Execute()
        {
            _greatPlains = new GreatPlains(_settingService, _logger, _orderService, _shippingService, _genericAttributeService, _customerService, _taxService, _addressService);

            // Update Shipped Orders
            UpdateShippedOrders();

            // Capture Credit Cards
            CaptureCreditCards();

            // Update Delivered Orders
            UpdateDeliveredOrders();
        }

        private void CaptureCreditCards()
        {
            // Get Orders that are shipped, but have not yet been captured
            IPagedList<Order> nopOrders = _orderService.SearchOrders(0, 0, 0, 0, 0, 0, null, null, OrderStatus.Transmitted, PaymentStatus.Authorized, ShippingStatus.Shipped, null, null, 0, int.MaxValue);

            // Process the Credit Card
            foreach (Order nopOrder in nopOrders)
            {
                IList<string> results = _orderProcessingService.Capture(nopOrder);
                if (results.Count > 0)
                {
                    foreach (string result in results)
                    {
                        if (result == "Capture Failed")
                        {
                            // Create the error message
                            string strMessage = "<p><strong>Error Message from MisterLandscaper.com</strong></p>"
                                                + "<p>The payment for Order #" + nopOrder.Id.ToString().Trim() + " could not be captured.  The most common cause of this issue is an expired Credit Card Authorization.  Please contact the customer"
                                                + "to make other payment arrangements.</p>"
                                                + "<p>The original PayFlowPro Transaction ID was: " + nopOrder.AuthorizationTransactionId.ToString().Trim() + "</p>";

                            // Add the error to the order notes
                            OrderNote note = new OrderNote();
                            note.CreatedOnUtc = DateTime.UtcNow;
                            note.DisplayToCustomer = false;
                            note.Note = strMessage;
                            nopOrder.OrderNotes.Add(note);

                            // Email the error to the store owner
                            QueuedEmail email = new QueuedEmail();
                            email.Body = strMessage;
                            email.CreatedOnUtc = DateTime.UtcNow;
                            email.EmailAccountId = 1;
                            email.From = "store@misterlandscaper.com";
                            email.FromName = "Mister Landscaper Store";
                            email.To = "sales@misterlandscaper.com";
                            email.ToName = "Mister Landscaper Support";
                            email.Priority = 5;
                            email.Subject = "Error:  Cannot capture payment on Order #" + nopOrder.Id.ToString().Trim();

                            _queuedEmailService.InsertQueuedEmail(email);

                            // Remove the Authorization from the card, so that this does not run on a loop
                            nopOrder.PaymentStatus = PaymentStatus.Pending;
                            _orderService.UpdateOrder(nopOrder);
                        }
                    }
                }
            }
        }
 
        private void UpdateShippedOrders()
        {
            // Get the list of orders
            IPagedList<Order> nopOrders = _orderService.SearchOrders(0, 0, 0, 0, 0, 0, null, null, OrderStatus.Transmitted, null, ShippingStatus.NotYetShipped, null, null, 0, int.MaxValue);

            // Iterate through the list of orders
            foreach (Order nopOrder in nopOrders)
            {
                try
                {
                    // Get the order from Great Plains
                    GPSalesOrder gpSalesOrder = new GPSalesOrder(_orderService, _shippingService, _genericAttributeService, _customerService, _logger, _taxService, _addressService, _greatPlains);
                    SalesOrder salesOrder = gpSalesOrder.GetSalesOrder(nopOrder);

                    // If the order was found...
                    if (salesOrder != null)
                    {
                        // Set the Tracking Number in nopCommerce.
                        if (salesOrder.TrackingNumbers != null)
                        {
                            if (salesOrder.TrackingNumbers.Length > 0)
                            {
                                if (salesOrder.TrackingNumbers[0] != null)
                                {
                                    if (!String.IsNullOrWhiteSpace(salesOrder.TrackingNumbers[0].Key.Id))
                                    {
                                        Shipment shipment = new Shipment()
                                        {
                                            OrderId = nopOrder.Id,
                                            TrackingNumber = salesOrder.TrackingNumbers[0].Key.Id.ToString().Trim(),
                                        };

                                        var orderItems = nopOrder.OrderItems;

                                        foreach (var orderItem in orderItems)
                                        {
                                            //we can ship only shippable products
                                            if (!orderItem.Product.IsShipEnabled)
                                                continue;

                                            var warehouse = _shippingService.GetWarehouseById(orderItem.Product.WarehouseId);

                                            ShipmentItem shipmentItem = new ShipmentItem()
                                            {
                                                OrderItemId = orderItem.Id,
                                                Quantity = orderItem.Quantity,
                                            };

                                            shipment.ShipmentItems.Add(shipmentItem);

                                        }
                                        shipment.CreatedOnUtc = DateTime.UtcNow;
                                        //shipment.ShippedDateUtc = DateTime.UtcNow;
                                        nopOrder.Shipments.Add(shipment);
                                        _orderService.UpdateOrder(nopOrder);
                                        _orderProcessingService.Ship(shipment, true);
                                        _genericAttributeService.SaveAttribute<string>(nopOrder, "gp_shipdate", DateTime.Now.ToString(), 0);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.InsertLog(Core.Domain.Logging.LogLevel.Error, ex.Message, ex.InnerException.ToString(), null);
                }
            }
        }

        /// <summary>
        /// Marks orders delivered and complete 2 weeks after delivery, which also triggers notification to the GPCustomer
        /// </summary>
        private void UpdateDeliveredOrders()
        {
            // Get the list of orders
            IPagedList<Order> nopOrders = _orderService.SearchOrders(0, 0, 0, 0, 0, 0, null, null, OrderStatus.Transmitted, null, ShippingStatus.Shipped, null, null, 0, int.MaxValue);

            // Iterate through the list of orders
            if (nopOrders.Count > 0)
            {
                foreach (Order order in nopOrders)
                {
                    IList<GenericAttribute> attributes = _genericAttributeService.GetAttributesForEntity(order.Id, "Order");
                    foreach (GenericAttribute attribute in attributes)
                    {
                        if (attribute.Key == "gp_shipdate")
                        {
                            DateTime oldDate = Convert.ToDateTime(attribute.Value.ToString());
                            int iCompare = DateTime.Now.CompareTo(oldDate.AddDays(14));

                            if(iCompare >= 0)
                            {
                                foreach(Shipment shipment in order.Shipments)
                                {
                                    _orderProcessingService.Deliver(shipment, true);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
