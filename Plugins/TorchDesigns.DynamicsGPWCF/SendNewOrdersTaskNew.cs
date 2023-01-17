using System;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Services.Tasks;
using Nop.Services.Orders;
using Nop.Services.Logging;
using Nop.Core;
using Nop.Services.Configuration;
using Nop.Services.Shipping;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Tax;

namespace Nop.Plugin.TorchDesigns.DynamicsGPWCF
{
    class SendNewOrdersTaskNew : ITask
    {

        private readonly IOrderService _orderService;
        private readonly ILogger _logger;
        private readonly ISettingService _settingService;
        private readonly IShippingService _shippingService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ICustomerService _customerService;
        private readonly ITaxService _taxService;
        private readonly IAddressService _addressService;

        public SendNewOrdersTaskNew(IOrderService orderService, ISettingService settingService, ILogger logger, IShippingService shippingService, IGenericAttributeService genericAttributeService, 
            ICustomerService customerService, ITaxService taxService, IAddressService addressService)
        {
            this._orderService = orderService;
            this._logger = logger;
            this._settingService = settingService;
            this._shippingService = shippingService;
            this._genericAttributeService = genericAttributeService;
            this._customerService = customerService;
            this._taxService = taxService;
            this._addressService = addressService;
        }
        
        public void Execute()
        {
            try
            {
                GreatPlains greatPlains = new GreatPlains(_settingService, _logger, _orderService, _shippingService, _genericAttributeService, _customerService, _taxService, _addressService);

                // Paid Orders
                IPagedList<Order> newOrders = _orderService.SearchOrders(0, 0, 0, 0, 0, 0, null, null, OrderStatus.Processing, PaymentStatus.Paid, null, null, null, 0, int.MaxValue);

                // Submit new Sales Orders
                foreach (Order o in newOrders)
                {
                    _logger.Information("Started submitting new sales order : " + o.Id.ToString());
                    greatPlains.SubmitSalesOrder(o);
                    _logger.Information("Ended submitting new sales order : " + o.Id.ToString());
                }

                // Authorized Orders
                newOrders = _orderService.SearchOrders(0, 0, 0, 0, 0, 0, null, null, OrderStatus.Processing, PaymentStatus.Authorized, null, null, null, 0, int.MaxValue);

                // Submit new Sales Orders
                foreach (Order o in newOrders)
                {
                    greatPlains.SubmitSalesOrder(o);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("GreatPlains.Task",ex ,null);
            }
            
        }
     
    }
}
