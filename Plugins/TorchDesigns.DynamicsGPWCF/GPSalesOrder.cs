using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core.Domain.Orders;
using Nop.Plugin.TorchDesigns.DynamicsGPWCF.DynamicsGP;
using Nop.Core.Infrastructure;
using Nop.Services.Tax;
using System.Xml.Serialization;
using System.IO;
using Nop.Services.Orders;
using Nop.Services.Shipping;
using Nop.Services.Common;
using Nop.Services.Logging;
using Nop.Core.Domain.Common;
using Nop.Services.Customers;
using Nop.Plugin.TorchDesign.Zip2Tax;

namespace Nop.Plugin.TorchDesigns.DynamicsGPWCF
{
    class GPSalesOrder
    {
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IOrderService _orderservice;
        private readonly IShippingService _shippingservice;
        private readonly ILogger _logger;
        private readonly ICustomerService _customerService;
        private readonly ITaxService _taxService;
        private readonly IAddressService _addressService;
        private readonly TorchDesignsDynamicsGPWCFSettings _torchDesignsDynamicsGPSettings;

        private GreatPlains _greatPlains; // Central Dynamics Control Object
        private Order _nopOrder; // nopCommerce Customer Order
        private SalesDocumentKey _oKey; // Great Plains Sales Order Key

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="orderservice">nopCommerce Order Service</param>
        /// <param name="shippingservice">nopCommerce Shipping Service</param>
        /// <param name="genericAttributeService">nopCommerce Generic Attribute Service</param>
        /// <param name="customerService">nopCommerce Customer Service</param>
        /// <param name="logger">nopCommerce Logger Service</param>
        /// <param name="taxService">nopCommerce Tax Service</param>
        /// <param name="_addressService">nopCommerce Address Service</param>
        public GPSalesOrder(IOrderService orderservice, IShippingService shippingservice, IGenericAttributeService genericAttributeService, ICustomerService customerService, ILogger logger,
            ITaxService taxService, IAddressService addressService, GreatPlains greatPlains)
        {
            this._orderservice = orderservice;
            this._shippingservice = shippingservice;
            this._genericAttributeService = genericAttributeService;
            this._logger = logger;
            this._customerService = customerService;
            this._addressService = addressService;
            this._taxService = taxService;
            this._greatPlains = greatPlains;
            this._torchDesignsDynamicsGPSettings = EngineContext.Current.Resolve<TorchDesignsDynamicsGPWCFSettings>();
        }

        /// <summary>
        /// Submit a sales order to Great Plains.  This will either find an existing order and update the reference, or it will create a new order.
        /// </summary>
        /// <param name="greatPlains">Great Plains Controller Object</param>
        /// <param name="oCustomerOrder">nopCommerce Customer</param>
        /// <param name="bCreateSalesOrder">If this is true and an existing order is not found, a new sales order will be created</param>
        public void SubmitSalesOrder(Order oCustomerOrder, bool bCreateSalesOrder)
        {
            // Set the Great Plains Controller Object
            _nopOrder = oCustomerOrder;

            // Create the Great Plains Sales Order Object
            _oKey = findOrCreateSalesOrder(bCreateSalesOrder, oCustomerOrder);
        }

        /// <summary>
        /// Submit a sales order to Great Plains.  This will either find an existing order and update the reference, or it will create a new order.  Order creation is defaulted to true.
        /// </summary>
        /// <param name="greatPlains">Great Plains Controller Object</param>
        /// <param name="oCustomerOrder">nopCommerce Customer</param>
        public void SubmitSalesOrder(Order oCustomerOrder)
        {
            this.SubmitSalesOrder(oCustomerOrder, true);
        }

        private SalesDocumentKey createSalesOrder()
        {
            // Init
            SalesDocumentKey oKey = null;
            SalesOrder oSalesOrder = new SalesOrder();

            // Create the Sales Order
            try
            {
                // Batch
                oSalesOrder.BatchKey = GPBatchKey.GetBatchKey();

                // Customer
                GPCustomer gpCustomer = new GPCustomer(_greatPlains, _nopOrder, _genericAttributeService, _customerService, _logger, _addressService, _taxService);
                CustomerKey oCustomerKey = gpCustomer.CustKey;
                oSalesOrder.CustomerKey = oCustomerKey;

                // Currency
                oSalesOrder.CurrencyKey = new CurrencyKey();
                var isoCode = _torchDesignsDynamicsGPSettings.CurrencyKeyISOCode;
                oSalesOrder.CurrencyKey.ISOCode = !string.IsNullOrEmpty(isoCode) ? isoCode.ToUpper() : string.Empty; // TODO: Configurable

                // PO Number
                oSalesOrder.CustomerPONumber = _torchDesignsDynamicsGPSettings.PONumberPrefix + _nopOrder.Id.ToString().Trim(); // TODO Configurable

                // Document Type
                oSalesOrder.DocumentTypeKey = new SalesDocumentTypeKey();
                oSalesOrder.DocumentTypeKey.Type = SalesDocumentType.Order;

                // Date
                oSalesOrder.Date = DateTime.Now.Date;

                // Shipping
                oSalesOrder.FreightAmount = new MoneyAmount();
                oSalesOrder.FreightAmount.Value = Decimal.Round(_nopOrder.OrderShippingExclTax, 2);
                oSalesOrder.FreightAmount.Currency = "USD";
                oSalesOrder.FreightAmount.DecimalDigits = 2;
                //_logger.Information("Create Sale oreder Billing Address");
                // Billing Address
                GPAddresses addresses = new GPAddresses(_addressService, _taxService, _greatPlains, _logger);
                oSalesOrder.BillToAddressKey = addresses.GetMatchingAddressKeyForOrder(gpCustomer.Customer, _nopOrder.BillingAddress, "BILL TO", _nopOrder);
                // Shipping Address - Updated to select the correct address from the list - ksf 7/22/2011
                oSalesOrder.ShipToAddressKey = addresses.GetMatchingAddressKeyForOrder(gpCustomer.Customer, _nopOrder.ShippingAddress, "SHIP TO", _nopOrder);

                //oSalesOrder.RequestedShipDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 2 , 0, 0, 0, 0);
                //oSalesOrder.ActualShipDate  = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day +2, 0, 0, 0, 0);
                //oSalesOrder.FulfillDate= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 2, 0, 0, 0, 0);

                // Sales Tax Schedule
                TorchDesignZip2TaxPlugin zip2TaxProvider = (TorchDesignZip2TaxPlugin)_taxService.LoadActiveTaxProvider();
                oSalesOrder.TaxScheduleKey = new TaxScheduleKey();
                CalculateTaxRequest oTaxRequest = new CalculateTaxRequest();
                oTaxRequest.Address = _nopOrder.ShippingAddress;
                string strTaxScheduleId = zip2TaxProvider.GetCounty(oTaxRequest.Address.ZipPostalCode.ToString().Trim());
                if (strTaxScheduleId == null)
                {
                    strTaxScheduleId = _torchDesignsDynamicsGPSettings.TaxScheduleId; // TODO: Add to Configuration
                }
                else if (strTaxScheduleId == "")
                {
                    strTaxScheduleId = _torchDesignsDynamicsGPSettings.TaxScheduleId; // TODO: Add to Configuration
                }
                else
                {
                    oSalesOrder.TaxScheduleKey.Id = strTaxScheduleId;
                }

                Nop.Core.Domain.Customers.Customer cust = _nopOrder.Customer;

                // Payment
                oSalesOrder.Payments = new SalesPayment[1];
                oSalesOrder.Payments[0] = new SalesPayment();
                oSalesOrder.Payments[0].Type = SalesPaymentType.PaymentCardDeposit;  // TODO Configurable
                var authCode = _nopOrder.AuthorizationTransactionCode;
                oSalesOrder.Payments[0].AuthorizationCode = !string.IsNullOrEmpty(authCode) ? authCode.ToUpper() : string.Empty;
                oSalesOrder.Payments[0].CurrencyKey = new CurrencyKey();
                oSalesOrder.Payments[0].CurrencyKey.ISOCode = _torchDesignsDynamicsGPSettings.CurrencyKeyISOCode;  // TODO Configurable
                oSalesOrder.Payments[0].Date = DateTime.Now.Date;
                oSalesOrder.Payments[0].PaymentCardNumber = _nopOrder.CardNumber;
                oSalesOrder.Payments[0].PaymentCardTypeKey = new PaymentCardTypeKey();
                oSalesOrder.Payments[0].PaymentCardTypeKey.Id = _torchDesignsDynamicsGPSettings.PaymentCardTypeKeyId;  // TODO Configurable
                oSalesOrder.Payments[0].PaymentAmount = new MoneyAmount();
                oSalesOrder.Payments[0].PaymentAmount.Currency = "USD";// TODO Configurable
                oSalesOrder.Payments[0].PaymentAmount.DecimalDigits = 2;
                oSalesOrder.Payments[0].PaymentAmount.Value = Decimal.Round(_nopOrder.OrderTotal, 2);

                // Map Shipping Methods
                oSalesOrder.ShippingMethodKey = GPShippingMethodKey.GetGpShippingMethodKey(_nopOrder);

                // Products Ordered
                oSalesOrder.Lines = GPProductList.GetProductList(_nopOrder, oSalesOrder);

                // Order Totals
                oSalesOrder.TotalAmount = new MoneyAmount();
                oSalesOrder.TotalAmount.Currency = "USD";  // TODO Configurable;
                oSalesOrder.TotalAmount.DecimalDigits = 2;
                oSalesOrder.TotalAmount.Value = Decimal.Round(_nopOrder.OrderTotal, 2);

                // Order Notes
                foreach (OrderNote oNote in _nopOrder.OrderNotes)
                {
                    if (oNote.Note.Substring(0, 6) == "GPNOTE")
                    {
                        oSalesOrder.Note = oNote.Note.Substring(7);
                    }
                }

                // Order Discount
                if (_nopOrder.OrderSubTotalDiscountExclTax != decimal.Zero)
                {
                    oSalesOrder.TradeDiscount = new MoneyPercentChoice();
                    oSalesOrder.TradeDiscount.Item = new MoneyAmount();
                    ((MoneyAmount)oSalesOrder.TradeDiscount.Item).Currency = "USD";  // TODO Configurable;;
                    ((MoneyAmount)oSalesOrder.TradeDiscount.Item).DecimalDigits = 2;
                    ((MoneyAmount)oSalesOrder.TradeDiscount.Item).Value = Decimal.Round(_nopOrder.OrderSubTotalDiscountExclTax, 2);
                }
                else if (_nopOrder.OrderDiscount != decimal.Zero)
                {
                    oSalesOrder.TradeDiscount = new MoneyPercentChoice();
                    oSalesOrder.TradeDiscount.Item = new MoneyAmount();
                    ((MoneyAmount)oSalesOrder.TradeDiscount.Item).Currency = "USD";  // TODO Configurable;;
                    ((MoneyAmount)oSalesOrder.TradeDiscount.Item).DecimalDigits = 2;
                    ((MoneyAmount)oSalesOrder.TradeDiscount.Item).Value = Decimal.Round(_nopOrder.OrderDiscount, 2);
                }

                // Create the Sales Order
                Policy oPolicy = _greatPlains.GetService().GetPolicyByOperation("CreateSalesOrder", _greatPlains.GpContext);
                _greatPlains.GetService().CreateSalesOrder(oSalesOrder, _greatPlains.GpContext, oPolicy);

                // Get the SalesOrderKey
                oKey = findSalesOrderByPo();

            }
            catch (Exception ex)
            {
                oKey = null;
                var logs = _greatPlains.GetService().GetLoggedExceptionDataList(DateTime.Now.AddHours(-1), DateTime.Now, _greatPlains.GpContext);
                if (logs.Length > 0)
                {
                    foreach (var log in logs)
                    {
                        _logger.Error("CreateSalesOrder Return : " + log.Message);
                    }
                }
                _logger.Error("CreateSalesOrder Return  : ", ex);
            }

            // Return Great Plains SalesOrderKey
            return oKey;
        }

        public static void ObjectToXml(object obj, string path_to_xml)
        {
            //serialize and persist it to it's file 
            try
            {
                XmlSerializer ser = new XmlSerializer(obj.GetType());
                FileStream fs = File.Open(
                        path_to_xml,
                        FileMode.OpenOrCreate,
                        FileAccess.Write,
                        FileShare.ReadWrite);
                ser.Serialize(fs, obj);
            }
            catch (Exception ex)
            {
                throw new Exception(
                        "Could Not Serialize object to " + path_to_xml,
                        ex);
            }
        }


        /// <summary>
        /// Find an Order Master Number using the Cross Reference Table
        /// </summary>
        /// <returns>Great Plains Master Number</returns>
        private SalesDocumentKey findSalesOrderByCrossReference(Order oCustomerOrder)
        {
            // Init
            SalesDocumentKey oKey = null;

            IList<GenericAttribute> attributes = _genericAttributeService.GetAttributesForEntity(_nopOrder.Id, "Order");
            foreach (GenericAttribute attribute in attributes)
            {
                if (attribute.Key == "gp_salesorder")
                {
                    oKey = new SalesDocumentKey();
                    oKey.Id = attribute.Value.Trim();
                    oCustomerOrder.OrderStatus = OrderStatus.Transmitted;
                    _orderservice.UpdateOrder(oCustomerOrder);
                }
            }

            return oKey;
        }

        /// <summary>
        /// Attempts to find the Great Plains Master Number using the Customer's last name and Nop Commerce Order ID (PO Number)
        /// </summary>
        /// <returns>Great Plains Master Number</returns>
        private SalesDocumentKey findSalesOrderByPo()
        {
            // Init
            SalesOrderSummary[] gpSalesOrderSummary = null;
            SalesDocumentKey oKey = null;

            try
            {
                // Get the Dynamics Order ID
                SalesOrderCriteria salesOrderCriteria = new SalesOrderCriteria();

                LikeRestrictionOfString nameCriteria = new LikeRestrictionOfString();
                LikeRestrictionOfString poCriteria = new LikeRestrictionOfString();
                string Lastname = _nopOrder.BillingAddress.LastName.Trim();
                // TODO: Get Last Name from Customer Object (anon)
                nameCriteria.Like = "%" + Lastname.Trim() + "%";
                poCriteria.Like = _torchDesignsDynamicsGPSettings.PONumberPrefix + _nopOrder.Id.ToString().Trim();

                salesOrderCriteria.CustomerPONumber = poCriteria;
                salesOrderCriteria.CustomerName = nameCriteria;
                gpSalesOrderSummary = _greatPlains.GetService().GetSalesOrderList(salesOrderCriteria, _greatPlains.GpContext);

            }
            catch (Exception ex)
            {
                gpSalesOrderSummary = null;
                _logger.InsertLog(Core.Domain.Logging.LogLevel.Error, ex.Message, ex.InnerException.ToString(), null);
            }

            if (gpSalesOrderSummary != null && gpSalesOrderSummary.Count() > 0)
            {
                oKey = gpSalesOrderSummary[0].Key;
            }
            else
            {
                // Get the Dynamics Order ID
                SalesOrderCriteria salesOrderCriteria = new SalesOrderCriteria();

                LikeRestrictionOfString poCriteria = new LikeRestrictionOfString();

                poCriteria.Like = _torchDesignsDynamicsGPSettings.PONumberPrefix + _nopOrder.Id.ToString().Trim(); // TODO Configurable

                salesOrderCriteria.CustomerPONumber = poCriteria;
                gpSalesOrderSummary = _greatPlains.GetService().GetSalesOrderList(salesOrderCriteria, _greatPlains.GpContext);

                if (gpSalesOrderSummary != null && gpSalesOrderSummary.Count() > 0)
                {
                    oKey = gpSalesOrderSummary[0].Key;
                }
            }

            return oKey;
        }

        /// <summary>
        /// Finds, Creates and Populates the Sales Order Object based on the NopCommerce Order.
        /// </summary>
        private SalesDocumentKey findOrCreateSalesOrder(bool bCreateSalesOrder, Order oCustomerOrder)
        {
            SalesDocumentKey oKey = null;
            bool bUpToDate = false;

            // Find the order using the cross-reference table
            oKey = findSalesOrderByCrossReference(oCustomerOrder);

            // Find the order using the last name and PO
            if (oKey == null)
            {
                oKey = findSalesOrderByPo();
            }
            else
            {
                bUpToDate = true;
            }

            // Create the SalesOrder
            if (oKey == null && bCreateSalesOrder == true)
            {
                oKey = createSalesOrder(); // Activated Sales Order Creation - ksf 7/3/2011

                // If the sales order was created, update the status
                if (oKey != null)
                {
                    _nopOrder.OrderStatus = OrderStatus.Transmitted;
                    _orderservice.UpdateOrder(_nopOrder);
                }
            }

            // Update Cross Reference
            if (oKey != null & bUpToDate == false)
            {
                updateOrderReference(oKey);
            }

            return oKey;
        }

        /// <summary>
        /// Get the Sales Order based on the key for this object
        /// </summary>
        /// <returns>Great Plains Sales Order</returns>
        public SalesOrder GetSalesOrder()
        {
            SalesOrder oSalesOrder = null;

            if (_oKey != null)
            {
                oSalesOrder = GetSalesOrderByKey(_oKey);
            }

            return oSalesOrder;

        }

        /// <summary>
        /// Get the sales order based upon the generic attribute stored on the NopCommerce Order
        /// </summary>
        /// <param name="nopOrder">NopCommerce Order</param>
        /// <returns>Great Plains Sales Order</returns>
        public SalesOrder GetSalesOrder(Order nopOrder)
        {
            IList<GenericAttribute> attributes = _genericAttributeService.GetAttributesForEntity(nopOrder.Id, "Order");
            foreach (GenericAttribute attribute in attributes)
            {
                if (attribute.Key == "gp_salesorder")
                {
                    _oKey = new SalesDocumentKey();
                    _oKey.Id = attribute.Value.Trim();
                    break;
                }
            }

            return GetSalesOrder(); ;
        }

        /// <summary>
        /// Get a Sales Order from Great Plains based on the specfied key
        /// </summary>
        /// <param name="Key">Great Plains Sales Document Key</param>
        /// <returns>Great Plains Sales Order</returns>
        private SalesOrder GetSalesOrderByKey(SalesDocumentKey Key)
        {
            SalesOrder gpSalesOrder = null;

            try
            {
                if (Key != null)
                {
                    gpSalesOrder = _greatPlains.GetService().GetSalesOrderByKey(Key, _greatPlains.GpContext);
                }
            }
            catch (Exception ex)
            {
                gpSalesOrder = null;
                _logger.InsertLog(Core.Domain.Logging.LogLevel.Error, ex.Message, ex.InnerException.ToString(), null);
            }

            return gpSalesOrder;
        }

        /// <summary>
        /// Updates the local cross reference for future use
        /// </summary>
        /// <param name="iMasterNumber">Great Plains Master Number</param>
        private void updateOrderReference(SalesDocumentKey oKey)
        {
            _genericAttributeService.SaveAttribute<string>(_nopOrder, "gp_salesorder", oKey.Id.Trim(), 0);
        }
    }
}