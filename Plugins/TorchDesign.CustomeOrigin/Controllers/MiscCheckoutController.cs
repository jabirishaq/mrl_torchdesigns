using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Nop.Core;
using Nop.Plugin.TorchDesign.CustomerOrigin.Services;
using Nop.Core.Domain.Directory;
using Nop.Plugin.TorchDesign.CustomerOrigin.Domain;
using Nop.Plugin.TorchDesign.CustomerOrigin.Models;
using Nop.Services.Configuration;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Kendoui;
using Nop.Web.Framework.Mvc;
using Nop.Plugin.TorchDesign.CustomerOrigin;
using Nop.Services.Stores;
using System.Xml;
using System.IO;
using System.Net;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using Nop.Services.Helpers;
using Nop.Services.Orders;
using Nop.Services.Tax;
using Nop.Services.Catalog;
using Nop.Services.Customers;
using Nop.Services.Common;
using Nop.Services.Shipping;
using Nop.Services.Payments;
using Nop.Core.Plugins;
using Nop.Services.Logging;
using System.Web;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Payments;
using Nop.Core.Domain.Shipping;
using Nop.Core.Domain.Common;

namespace Nop.Plugin.TorchDesign.CustomerOrigin.Controllers
{

    public partial class MiscCheckoutController : BasePluginController
    {
       
		#region Fields

        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IStoreMappingService _storeMappingService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly ILocalizationService _localizationService;
        private readonly ITaxService _taxService;
        private readonly ICurrencyService _currencyService;
        private readonly IPriceFormatter _priceFormatter;
        private readonly IOrderProcessingService _orderProcessingService;
        private readonly ICustomerService _customerService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ICountryService _countryService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly IShippingService _shippingService;
        private readonly IPaymentService _paymentService;
        private readonly IPluginFinder _pluginFinder;
        private readonly IOrderTotalCalculationService _orderTotalCalculationService;
        private readonly ILogger _logger;
        private readonly IOrderService _orderService;
        private readonly IWebHelper _webHelper;
        private readonly HttpContextBase _httpContext;
        private readonly ICustomerOriginService _customerorigin;

        private readonly OrderSettings _orderSettings;
        private readonly RewardPointsSettings _rewardPointsSettings;
        private readonly PaymentSettings _paymentSettings;
        private readonly ShippingSettings _shippingSettings;
        private readonly AddressSettings _addressSettings;

        #endregion

		#region Constructors

        public MiscCheckoutController(IWorkContext workContext,ICustomerOriginService customerorigin,
            IStoreContext storeContext,
            IStoreMappingService storeMappingService,
            IShoppingCartService shoppingCartService,  
            ILocalizationService localizationService, 
            ITaxService taxService, 
            ICurrencyService currencyService, 
            IPriceFormatter priceFormatter, 
            IOrderProcessingService orderProcessingService,
            ICustomerService customerService, 
            IGenericAttributeService genericAttributeService,
            ICountryService countryService,
            IStateProvinceService stateProvinceService,
            IShippingService shippingService, 
            IPaymentService paymentService,
            IPluginFinder pluginFinder,
            IOrderTotalCalculationService orderTotalCalculationService,
            ILogger logger,
            IOrderService orderService,
            IWebHelper webHelper,
            HttpContextBase httpContext,
            OrderSettings orderSettings, 
            RewardPointsSettings rewardPointsSettings,
            PaymentSettings paymentSettings,
            ShippingSettings shippingSettings,
            AddressSettings addressSettings)
        {
            this._workContext = workContext;
            this._storeContext = storeContext;
            this._storeMappingService = storeMappingService;
            this._shoppingCartService = shoppingCartService;
            this._localizationService = localizationService;
            this._taxService = taxService;
            this._currencyService = currencyService;
            this._priceFormatter = priceFormatter;
            this._orderProcessingService = orderProcessingService;
            this._customerService = customerService;
            this._genericAttributeService = genericAttributeService;
            this._countryService = countryService;
            this._stateProvinceService = stateProvinceService;
            this._shippingService = shippingService;
            this._paymentService = paymentService;
            this._pluginFinder = pluginFinder;
            this._orderTotalCalculationService = orderTotalCalculationService;
            this._logger = logger;
            this._orderService = orderService;
            this._webHelper = webHelper;
            this._httpContext = httpContext;
            this._customerorigin = customerorigin;
            this._orderSettings = orderSettings;
            this._rewardPointsSettings = rewardPointsSettings;
            this._paymentSettings = paymentSettings;
            this._shippingSettings = shippingSettings;
            this._addressSettings = addressSettings;
        }

        #endregion



        #region Utilities
        #region Checkout Confirm
        public ActionResult Confirm()
        { 
            int customerid=_workContext.CurrentCustomer.Id;
           var CustomerExist=_customerorigin.CustomerExistOrNotInOriginTable(customerid);

           if (CustomerExist != null)
            {
                CustomerExist.GivenOn = System.DateTime.UtcNow;
               _customerorigin.InsertCustomerOriginAnswer(CustomerExist);

                //validation
                var cart = _workContext.CurrentCustomer.ShoppingCartItems
                    .Where(sci => sci.ShoppingCartType == ShoppingCartType.ShoppingCart)
                    .LimitPerStore(_storeContext.CurrentStore.Id)
                    .ToList();
                if (cart.Count == 0)
                    return RedirectToRoute("ShoppingCart");

                if (_orderSettings.OnePageCheckoutEnabled)
                    return RedirectToRoute("CheckoutOnePage");

                if ((_workContext.CurrentCustomer.IsGuest() && !_orderSettings.AnonymousCheckoutAllowed))
                    return new HttpUnauthorizedResult();


                //model
                var model = PrepareConfirmOrderModel(cart);
                try
                {
                    var processPaymentRequest = _httpContext.Session["OrderPaymentInfo"] as ProcessPaymentRequest;


                    if (processPaymentRequest == null)
                    {
                        //Check whether payment workflow is required
                        if (IsPaymentWorkflowRequired(cart))
                            return RedirectToRoute("CheckoutPaymentInfo");
                        else
                            processPaymentRequest = new ProcessPaymentRequest();
                    }

                    //prevent 2 orders being placed within an X seconds time frame
                    if (!IsMinimumOrderPlacementIntervalValid(_workContext.CurrentCustomer))
                        throw new Exception(_localizationService.GetResource("Checkout.MinOrderPlacementInterval"));

                    //place order
                    processPaymentRequest.StoreId = _storeContext.CurrentStore.Id;
                    processPaymentRequest.CustomerId = _workContext.CurrentCustomer.Id;
                    processPaymentRequest.PaymentMethodSystemName = _workContext.CurrentCustomer.GetAttribute<string>(
                        SystemCustomerAttributeNames.SelectedPaymentMethod,
                        _genericAttributeService, _storeContext.CurrentStore.Id);
                    var placeOrderResult = _orderProcessingService.PlaceOrder(processPaymentRequest);
                    if (placeOrderResult.Success)
                    {
                        _httpContext.Session["OrderPaymentInfo"] = null;
                        var postProcessPaymentRequest = new PostProcessPaymentRequest()
                        {
                            Order = placeOrderResult.PlacedOrder
                        };
                        _paymentService.PostProcessPayment(postProcessPaymentRequest);

                        if (_webHelper.IsRequestBeingRedirected || _webHelper.IsPostBeingDone)
                        {
                            //redirection or POST has been done in PostProcessPayment
                            return Content("Redirected");
                        }
                        else
                        {
                            return RedirectToRoute("CheckoutCompleted", new { orderId = placeOrderResult.PlacedOrder.Id });
                        }
                    }
                    else
                    {
                        var MethodName = _paymentService.LoadPaymentMethodBySystemName(processPaymentRequest.PaymentMethodSystemName).PluginDescriptor.FriendlyName;
                        if (MethodName.ToLower() == "credit card" || MethodName.ToLower() == "payflow pro credit card")
                        {
                            IList<string> err = new List<string>();
                            foreach (var error in placeOrderResult.Errors)
                            {
                                err.Add(error);
                            }
                            this.Session["PaymentError"] = err;
                            return RedirectToRoute("MiscCheckoutPaymentInfo");
                        }
                        else
                        {
                            foreach (var error in placeOrderResult.Errors)
                            {
                                model.Warnings.Add(error);
                            }
                        }
                    }
                }
                catch (Exception exc)
                {
                    _logger.Warning(exc.Message, exc);
                    model.Warnings.Add(exc.Message);
                }

                //If we got this far, something failed, redisplay form
                return View(model);

            }
            else
            {

                //validation
                var cart = _workContext.CurrentCustomer.ShoppingCartItems
                    .Where(sci => sci.ShoppingCartType == ShoppingCartType.ShoppingCart)
                    .LimitPerStore(_storeContext.CurrentStore.Id)
                    .ToList();
                if (cart.Count == 0)
                    return RedirectToRoute("ShoppingCart");

                if (_orderSettings.OnePageCheckoutEnabled)
                    return RedirectToRoute("CheckoutOnePage");

                if ((_workContext.CurrentCustomer.IsGuest() && !_orderSettings.AnonymousCheckoutAllowed))
                    return new HttpUnauthorizedResult();

                //model
                var model = PrepareConfirmOrderModel(cart);
                return View("~/Plugins/TorchDesign.CustomerOrigin/Views/TorchDesignCustomerOrigin/Confirm.cshtml", model);
            }
          
        }
        [HttpPost, ActionName("Confirm")]
        [FormValueRequired("nextstep")]
        public ActionResult ConfirmOrder(CheckoutConfirmModel md)
        {
            if (md.Selecteditem != 0)
            {
                Td_CustomerOriginAnswer answermodel = new Td_CustomerOriginAnswer();
                answermodel.CustomerId = _workContext.CurrentCustomer.Id;
               
                TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                answermodel.GivenOn= TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, easternZone);
                 answermodel.Optionid = md.Selecteditem;
                _customerorigin.InsertCustomerOriginAnswer(answermodel);
                //  answermodel.Optionid=md.Configrationmodel.
                //validation
                var cart = _workContext.CurrentCustomer.ShoppingCartItems
                    .Where(sci => sci.ShoppingCartType == ShoppingCartType.ShoppingCart)
                    .LimitPerStore(_storeContext.CurrentStore.Id)
                    .ToList();
                if (cart.Count == 0)
                    return RedirectToRoute("ShoppingCart");

                if (_orderSettings.OnePageCheckoutEnabled)
                    return RedirectToRoute("CheckoutOnePage");

                if ((_workContext.CurrentCustomer.IsGuest() && !_orderSettings.AnonymousCheckoutAllowed))
                    return new HttpUnauthorizedResult();


                //model
                var model = PrepareConfirmOrderModel(cart);
                try
                {
                    var processPaymentRequest = _httpContext.Session["OrderPaymentInfo"] as ProcessPaymentRequest;
                    if (processPaymentRequest == null)
                    {
                        //Check whether payment workflow is required
                        if (IsPaymentWorkflowRequired(cart))
                            return RedirectToRoute("CheckoutPaymentInfo");
                        else
                            processPaymentRequest = new ProcessPaymentRequest();
                    }

                    //prevent 2 orders being placed within an X seconds time frame
                    if (!IsMinimumOrderPlacementIntervalValid(_workContext.CurrentCustomer))
                        throw new Exception(_localizationService.GetResource("Checkout.MinOrderPlacementInterval"));

                    //place order
                    processPaymentRequest.StoreId = _storeContext.CurrentStore.Id;
                    processPaymentRequest.CustomerId = _workContext.CurrentCustomer.Id;
                    processPaymentRequest.PaymentMethodSystemName = _workContext.CurrentCustomer.GetAttribute<string>(
                        SystemCustomerAttributeNames.SelectedPaymentMethod,
                        _genericAttributeService, _storeContext.CurrentStore.Id);
                    var placeOrderResult = _orderProcessingService.PlaceOrder(processPaymentRequest);
                    if (placeOrderResult.Success)
                    {
                        _httpContext.Session["OrderPaymentInfo"] = null;
                        var postProcessPaymentRequest = new PostProcessPaymentRequest()
                        {
                            Order = placeOrderResult.PlacedOrder
                        };
                        _paymentService.PostProcessPayment(postProcessPaymentRequest);

                        if (_webHelper.IsRequestBeingRedirected || _webHelper.IsPostBeingDone)
                        {
                            //redirection or POST has been done in PostProcessPayment
                            return Content("Redirected");
                        }
                        else
                        {
                            return RedirectToRoute("CheckoutCompleted", new { orderId = placeOrderResult.PlacedOrder.Id });
                        }
                    }
                    else
                    {
                        var MethodName = _paymentService.LoadPaymentMethodBySystemName(processPaymentRequest.PaymentMethodSystemName).PluginDescriptor.FriendlyName;
                        if (MethodName.ToLower() == "credit card" || MethodName.ToLower() == "payflow pro credit card")
                        {
                            IList<string> err = new List<string>();
                            foreach (var error in placeOrderResult.Errors)
                            {
                                err.Add(error);
                            }
                            this.Session["PaymentError"] = err;
                            return RedirectToRoute("CheckoutPaymentInfo");
                        }
                        else
                        {
                            foreach (var error in placeOrderResult.Errors)
                            {
                                model.Warnings.Add(error);
                            }
                        }
                    }
                }
                catch (Exception exc)
                {
                    _logger.Warning(exc.Message, exc);
                    model.Warnings.Add(exc.Message);
                }

                //If we got this far, something failed, redisplay form
                return View("~/Plugins/TorchDesign.CustomerOrigin/Views/TorchDesignCustomerOrigin/Confirm.cshtml",model);
            }
            else 
            {
                return RedirectToAction("Confirm");
            }
        }

        [HttpPost, ActionName("Confirm")]
        [FormValueRequired("nothanksbtn")]
        public ActionResult ConfirmOrderWithNoThanks(CheckoutConfirmModel md)
        {
           
                Td_CustomerOriginAnswer answermodel = new Td_CustomerOriginAnswer();
                answermodel.CustomerId = _workContext.CurrentCustomer.Id;
                answermodel.GivenOn = System.DateTime.UtcNow;
               int defaultoption= _customerorigin.GetAllCustomerOrigin().Where(x => x.DefaultSelected == true).SingleOrDefault().Id;
               answermodel.Optionid = defaultoption > 0 ? defaultoption : md.Selecteditem;
                _customerorigin.InsertCustomerOriginAnswer(answermodel);
                //  answermodel.Optionid=md.Configrationmodel.
                //validation
                var cart = _workContext.CurrentCustomer.ShoppingCartItems
                    .Where(sci => sci.ShoppingCartType == ShoppingCartType.ShoppingCart)
                    .LimitPerStore(_storeContext.CurrentStore.Id)
                    .ToList();
                if (cart.Count == 0)
                    return RedirectToRoute("ShoppingCart");

                if (_orderSettings.OnePageCheckoutEnabled)
                    return RedirectToRoute("CheckoutOnePage");

                if ((_workContext.CurrentCustomer.IsGuest() && !_orderSettings.AnonymousCheckoutAllowed))
                    return new HttpUnauthorizedResult();


                //model
                var model = PrepareConfirmOrderModel(cart);
                try
                {
                    var processPaymentRequest = _httpContext.Session["OrderPaymentInfo"] as ProcessPaymentRequest;
                    if (processPaymentRequest == null)
                    {
                        //Check whether payment workflow is required
                        if (IsPaymentWorkflowRequired(cart))
                            return RedirectToRoute("CheckoutPaymentInfo");
                        else
                            processPaymentRequest = new ProcessPaymentRequest();
                    }

                    //prevent 2 orders being placed within an X seconds time frame
                    if (!IsMinimumOrderPlacementIntervalValid(_workContext.CurrentCustomer))
                        throw new Exception(_localizationService.GetResource("Checkout.MinOrderPlacementInterval"));

                    //place order
                    processPaymentRequest.StoreId = _storeContext.CurrentStore.Id;
                    processPaymentRequest.CustomerId = _workContext.CurrentCustomer.Id;
                    processPaymentRequest.PaymentMethodSystemName = _workContext.CurrentCustomer.GetAttribute<string>(
                        SystemCustomerAttributeNames.SelectedPaymentMethod,
                        _genericAttributeService, _storeContext.CurrentStore.Id);
                    var placeOrderResult = _orderProcessingService.PlaceOrder(processPaymentRequest);
                    if (placeOrderResult.Success)
                    {
                        _httpContext.Session["OrderPaymentInfo"] = null;
                        var postProcessPaymentRequest = new PostProcessPaymentRequest()
                        {
                            Order = placeOrderResult.PlacedOrder
                        };
                        _paymentService.PostProcessPayment(postProcessPaymentRequest);

                        if (_webHelper.IsRequestBeingRedirected || _webHelper.IsPostBeingDone)
                        {
                            //redirection or POST has been done in PostProcessPayment
                            return Content("Redirected");
                        }
                        else
                        {
                            return RedirectToRoute("CheckoutCompleted", new { orderId = placeOrderResult.PlacedOrder.Id });
                        }
                    }
                    else
                    {
                        var MethodName = _paymentService.LoadPaymentMethodBySystemName(processPaymentRequest.PaymentMethodSystemName).PluginDescriptor.FriendlyName;
                        if (MethodName.ToLower() == "credit card" || MethodName.ToLower() == "payflow pro credit card")
                        {
                            IList<string> err = new List<string>();
                            foreach (var error in placeOrderResult.Errors)
                            {
                                err.Add(error);
                            }
                            this.Session["PaymentError"] = err;
                            return RedirectToRoute("CheckoutPaymentInfo");
                        }
                        else
                        {
                            foreach (var error in placeOrderResult.Errors)
                            {
                                model.Warnings.Add(error);
                            }
                        }
                    }
                }
                catch (Exception exc)
                {
                    _logger.Warning(exc.Message, exc);
                    model.Warnings.Add(exc.Message);
                }

                //If we got this far, something failed, redisplay form
                return View("~/Plugins/TorchDesign.CustomerOrigin/Views/TorchDesignCustomerOrigin/Confirm.cshtml", model);
           
        }
        [NonAction]
        protected virtual CheckoutConfirmModel PrepareConfirmOrderModel(IList<ShoppingCartItem> cart)
        {
            var optionlist = _customerorigin.GetAllCustomerOrigin().Where(x => x.Publish == true).ToList();
            var model = new CheckoutConfirmModel();
            //terms of service
            model.TermsOfServiceOnOrderConfirmPage = _orderSettings.TermsOfServiceOnOrderConfirmPage;
            //min order amount validation
            bool minOrderTotalAmountOk = _orderProcessingService.ValidateMinOrderTotalAmount(cart);
            if (!minOrderTotalAmountOk)
            {
                decimal minOrderTotalAmount = _currencyService.ConvertFromPrimaryStoreCurrency(_orderSettings.MinOrderTotalAmount, _workContext.WorkingCurrency);
                model.MinOrderTotalWarning = string.Format(_localizationService.GetResource("Checkout.MinOrderTotalAmount"), _priceFormatter.FormatPrice(minOrderTotalAmount, true, false));
            }
            model.DefaultSelectedOptionId = 0;
            foreach (var item in optionlist)
            {
                var m = new ConfigurationModel();
                m.Id = item.Id;
                m.OptionName = item.OptionName;
                m.DefaultSelected = item.DefaultSelected;
                model.Configrationmodel.Add(m);
                if (item.DefaultSelected)
                {
                    model.DefaultSelectedOptionId = item.Id;
                }

            }
           // model.defaultoption = false;
            //var point = new List<string>();
            //var availableoption = _customerorigin.GetAllCustomerOrigin().Where(x => x.Publish = true);
            //foreach (var item in availableoption)
            //{
            //    var custanswer = _customerorigin.GetCustomerOriginAnswerByOptionid(item.Id);
            //    point.Add(string.Format("{{y:{0},legendText:\"{1}\",indexLabel:\"{2}\"}}", custanswer.Count, item.OptionName + "(" + custanswer.Count + ")", item.OptionName + "(" + custanswer.Count + ")").Replace(" ", ""));
            //}
            //var PointJson = "[" + string.Join(",", point.ToArray()) + "]";
            //model.Charatdata = PointJson;
            //model.IsResultfound = true;
         
            return model;
        }


        [NonAction]
        protected virtual bool IsPaymentWorkflowRequired(IList<ShoppingCartItem> cart, bool ignoreRewardPoints = false)
        {
            bool result = true;

            //check whether order total equals zero
            decimal? shoppingCartTotalBase = _orderTotalCalculationService.GetShoppingCartTotal(cart, ignoreRewardPoints);
            if (shoppingCartTotalBase.HasValue && shoppingCartTotalBase.Value == decimal.Zero)
                result = false;
            return result;
        }



        protected virtual bool IsMinimumOrderPlacementIntervalValid(Customer customer)
        {
            //prevent 2 orders being placed within an X seconds time frame
            if (_orderSettings.MinimumOrderPlacementInterval == 0)
                return true;

            var lastOrder = _orderService.SearchOrders(storeId: _storeContext.CurrentStore.Id,
                customerId: _workContext.CurrentCustomer.Id, pageSize: 1)
                .FirstOrDefault();
            if (lastOrder == null)
                return true;

            var interval = DateTime.UtcNow - lastOrder.CreatedOnUtc;
            return interval.TotalSeconds > _orderSettings.MinimumOrderPlacementInterval;
        }
        #endregion


        [ChildActionOnly]
        public ActionResult CheckoutProgress(CheckoutProgressStep step)
        {
            var model = new CheckoutProgressModel() { CheckoutProgressStep = step };
           
            return PartialView("~/Plugins/TorchDesign.CustomerOrigin/Views/TorchDesignCustomerOrigin/CheckoutProgress.cshtml", model);
        }

        #endregion

    }
}
