using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Routing;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Core.Plugins;
using Nop.Plugin.TorchDesign.PayflowPro.Controllers;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Core.Domain.Customers;
using System.Threading;
using PayPal.Payments.DataObjects;
using PayPal.Payments.Transactions;
using PayPal.Payments.Common.Utility;
using PayPal.Payments.Communication;
using PayPal.Payments.DataObjects;
using Nop.Core.Infrastructure;

using Nop.Plugin.TorchDesign.PayflowPro.Models;
using System.Collections.Specialized;
using Nop.Services.Messages;
using Nop.Plugin.TorchDesign.PayflowPro.Data;
using Nop.Plugin.TorchDesign.PayflowPro.Domain;
using Nop.Plugin.TorchDesign.PayflowPro.Services;

namespace Nop.Plugin.TorchDesign.PayflowPro
{
    /// <summary>
    /// PayPalDirect payment processor
    /// </summary>
    public class TorchDesignPayflowProProcessor : BasePlugin, IPaymentMethod
    {
        #region Fields
        private bool useSandBox = true;
        private string user;
        private string vendor;
        private string partner;
        private string password;
        private readonly TorchDesignPayflowProSettings _paypalDirectPaymentSettings;
        private readonly ISettingService _settingService;
        private readonly ICurrencyService _currencyService;
        private readonly ICustomerService _customerService;
        private readonly CurrencySettings _currencySettings;
        private readonly IWebHelper _webHelper;
        private readonly IOrderTotalCalculationService _orderTotalCalculationService;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly IWorkContext _workContext;
        private readonly PayflowObjectContext _objectContext;
        private readonly ICreditCardDeclinedLogService _creditCardDeclinedLogService;
        private readonly IStoreContext _storeContext;
        #endregion

        #region Ctor

        public TorchDesignPayflowProProcessor(TorchDesignPayflowProSettings paypalDirectPaymentSettings,
            ISettingService settingService,
            ICurrencyService currencyService, ICustomerService customerService,
            CurrencySettings currencySettings, IWebHelper webHelper,
            IOrderTotalCalculationService orderTotalCalculationService,
            IWorkflowMessageService workflowMessageService,
            IWorkContext workContext,
            PayflowObjectContext objectContext,
            ICreditCardDeclinedLogService creditCardDeclinedLogService,
            IStoreContext storeContext)
        {
            this._paypalDirectPaymentSettings = paypalDirectPaymentSettings;
            this._settingService = settingService;
            this._currencyService = currencyService;
            this._customerService = customerService;
            this._currencySettings = currencySettings;
            this._webHelper = webHelper;
            this._orderTotalCalculationService = orderTotalCalculationService;
            this._workflowMessageService = workflowMessageService;
            this._workContext = workContext;
            this._objectContext = objectContext;
            this._creditCardDeclinedLogService = creditCardDeclinedLogService;
            this._storeContext = storeContext;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Gets Paypal URL
        /// </summary>
        /// <returns></returns>
        private string GetPaypalUrl()
        {
            return _paypalDirectPaymentSettings.UseSandbox ? "pilot-payflowpro.paypal.com" :
                "payflowpro.paypal.com";
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets transaction mode configured by store owner
        /// </summary>
        /// <returns></returns>
        private TransactMode GetCurrentTransactionMode()
        {
            // TransactMode transactionModeEnum = TransactMode.Authorize;
            //   string transactionMode = IoC.Resolve<ISettingManager>().GetSettingValue("PaymentMethod.PayFlowPro.TransactionMode");
            //if (!String.IsNullOrEmpty(transactionMode))
            //    transactionModeEnum = (TransactMode)Enum.Parse(typeof(TransactMode), transactionMode);
            return _paypalDirectPaymentSettings.TransactMode;
        }

        /// <summary>
        /// Initializes the PayPalDirectPaymentProcessor
        /// </summary>
        private void InitSettings()
        {
            useSandBox = _paypalDirectPaymentSettings.UseSandbox;
            user = _paypalDirectPaymentSettings.ApiAccountName;
            vendor = _paypalDirectPaymentSettings.Vendor;
            partner = _paypalDirectPaymentSettings.Partner;
            password = _paypalDirectPaymentSettings.ApiAccountPassword;
        }

        /// <summary>
        /// Post process payment (used by payment gateways that require redirecting to a third-party URL)
        /// </summary>
        /// <param name="postProcessPaymentRequest">Payment info required for an order processing</param>
        public void PostProcessPayment(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            //nothing
        }

        /// <summary>
        /// Gets additional handling fee
        /// </summary>
        /// <param name="cart">Shoping cart</param>
        /// <returns>Additional handling fee</returns>
        public decimal GetAdditionalHandlingFee(IList<ShoppingCartItem> cart)
        {
            var result = this.CalculateAdditionalFee(_orderTotalCalculationService, cart,
                _paypalDirectPaymentSettings.AdditionalFee, _paypalDirectPaymentSettings.AdditionalFeePercentage);
            return result;
        }

        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest processPaymentRequest)
        {
            var result = new ProcessPaymentResult();
            var customer = _customerService.GetCustomerById(processPaymentRequest.CustomerId);
            InitSettings();
            TransactMode transactionMode = GetCurrentTransactionMode();

            //little hack here

            var culture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;


            //public store


            try
            {
                Invoice invoice = new Invoice();

                BillTo to = new BillTo();
                to.FirstName = customer.BillingAddress.FirstName;
                to.LastName = customer.BillingAddress.LastName;
                to.Street = customer.BillingAddress.Address1;
                to.City = customer.BillingAddress.City;
                to.Zip = customer.BillingAddress.ZipPostalCode;
                if (customer.BillingAddress.StateProvince != null)
                    to.State = customer.BillingAddress.StateProvince.Abbreviation;
                invoice.BillTo = to;

                if (customer.ShippingAddress != null)
                {
                    ShipTo to2 = new ShipTo();
                    to2.ShipToFirstName = customer.ShippingAddress.FirstName;
                    to2.ShipToLastName = customer.ShippingAddress.LastName;
                    to2.ShipToStreet = customer.ShippingAddress.Address1;
                    to2.ShipToCity = customer.ShippingAddress.City;
                    to2.ShipToZip = customer.ShippingAddress.ZipPostalCode;
                    if (customer.ShippingAddress.StateProvince != null)
                        to2.ShipToState = customer.ShippingAddress.StateProvince.Abbreviation;
                    invoice.ShipTo = to2;
                }

                invoice.InvNum = processPaymentRequest.OrderGuid.ToString();
                decimal orderTotal = Math.Round(processPaymentRequest.OrderTotal, 2);
                invoice.Amt = new PayPal.Payments.DataObjects.Currency(orderTotal, _currencyService.GetCurrencyById(_currencySettings.PrimaryStoreCurrencyId).CurrencyCode);

                string creditCardExp = string.Empty;
                if (processPaymentRequest.CreditCardExpireMonth < 10)
                {
                    creditCardExp = "0" + processPaymentRequest.CreditCardExpireMonth.ToString();
                }
                else
                {
                    creditCardExp = processPaymentRequest.CreditCardExpireMonth.ToString();
                }
                creditCardExp = creditCardExp + processPaymentRequest.CreditCardExpireYear.ToString().Substring(2, 2);
                CreditCard credCard = new CreditCard(processPaymentRequest.CreditCardNumber, creditCardExp);
                credCard.Cvv2 = processPaymentRequest.CreditCardCvv2;
                CardTender tender = new CardTender(credCard);
                // <vendor> = your merchant (login id)  
                // <user> = <vendor> unless you created a separate <user> for Payflow Pro
                // partner = paypal
                UserInfo userInfo = new UserInfo(user, vendor, partner, password);
                string url = GetPaypalUrl();
                PayflowConnectionData payflowConnectionData = new PayflowConnectionData(url, 443, null, 0, null, null);

                Response response = null;
                if (transactionMode == TransactMode.Authorize)
                {
                    response = new AuthorizationTransaction(userInfo, payflowConnectionData, invoice, tender, PayflowUtility.RequestId).SubmitTransaction();

                }
                else
                {
                    response = new SaleTransaction(userInfo, payflowConnectionData, invoice, tender, PayflowUtility.RequestId).SubmitTransaction();
                }

                if (response.TransactionResponse != null)
                {
                    if (response.TransactionResponse.Result == 0)
                    {
                        //Set the Payment as pending upto we will do AVS and CVV check.
                        result.NewPaymentStatus = PaymentStatus.Pending;

                        //If we got the CVV Or AVS match Fails then Void the authorized transaction.
                      
                        //if (response.TransactionResponse.AVSAddr == "N" || response.TransactionResponse.AVSZip == "N" || response.TransactionResponse.CVV2Match == "N")
                        if (response.TransactionResponse.AVSZip == "N" || response.TransactionResponse.CVV2Match == "N")
                        {
                            if (transactionMode == TransactMode.Authorize)
                            {
                                //-----------Void Method Code------------//
                                var voidresult = new VoidPaymentResult();
                                VoidTransaction Trans = new VoidTransaction(response.TransactionResponse.Pnref, userInfo, payflowConnectionData, PayflowUtility.RequestId);
                                Response resp = Trans.SubmitTransaction();
                                //We try to Void transaction as the AVS or CVV is not matching..
                                if (resp.TransactionResponse.Result == 0)
                                {
                                    voidresult.NewPaymentStatus = PaymentStatus.Voided;
                                }

                                if (response.TransactionResponse.AVSAddr == "N" || response.TransactionResponse.AVSZip == "N")
                                {
                                    //result.AddError(int.MaxValue.ToString());
                                    result.AddError("ZIP code does not match with the information of cardholder's bank.");

                                    var getCreditCardDeclineLogs = _creditCardDeclinedLogService.SearchTd_CreditCardDeclinedLogs(_workContext.CurrentCustomer.Id, DateTime.UtcNow.Date);
                                    if (getCreditCardDeclineLogs.Count >= 4)
                                    {
                                        var currentIPaddress = _webHelper.GetCurrentIpAddress();
                                        _creditCardDeclinedLogService.AddIPToBannedList(currentIPaddress, _workContext.CurrentCustomer);
                                    }

                                    var creditCardDeclined = new Td_CreditCardDeclinedLog()
                                    {
                                        CreatedOn = DateTime.UtcNow.Date,
                                        CreditCardDeclinedDueTo = "ZIP code does not match with the information of cardholder's bank.",
                                        CustomerId = _workContext.CurrentCustomer.Id
                                    };
                                    _creditCardDeclinedLogService.InsertCreditCardDeclinedLog(creditCardDeclined);

                                    // email notification
                                    _workflowMessageService.SendCreditCardDeclineMessage(_workContext.CurrentCustomer, _workContext.WorkingLanguage.Id, "sales@misterlandscaper.com", "ZIP code does not match with the information of cardholder's bank.");
                                }
                                else if (response.TransactionResponse.CVV2Match == "N")
                                {
                                    result.AddError(string.Format("Your bank has indicated that the CVV code you entered is incorrect. Please check the CVV code on the back of your card and try again."));
                                    result.AddError("cvv50");

                                    var getCreditCardDeclineLogs = _creditCardDeclinedLogService.SearchTd_CreditCardDeclinedLogs(_workContext.CurrentCustomer.Id, DateTime.UtcNow.Date);
                                    if (getCreditCardDeclineLogs.Count >= 4)
                                    {
                                        var currentIPaddress = _webHelper.GetCurrentIpAddress();
                                        _creditCardDeclinedLogService.AddIPToBannedList(currentIPaddress, _workContext.CurrentCustomer);
                                    }

                                    var creditCardDeclined = new Td_CreditCardDeclinedLog()
                                    {
                                        CreatedOn = DateTime.UtcNow.Date,
                                        CreditCardDeclinedDueTo = "Your bank has indicated that the CVV code you entered is incorrect. Please check the CVV code on the back of your card and try again.",
                                        CustomerId = _workContext.CurrentCustomer.Id
                                    };
                                    _creditCardDeclinedLogService.InsertCreditCardDeclinedLog(creditCardDeclined);

                                    // email notification
                                    _workflowMessageService.SendCreditCardDeclineMessage(_workContext.CurrentCustomer, _workContext.WorkingLanguage.Id, "sales@misterlandscaper.com", "Your bank has indicated that the CVV code you entered is incorrect. Please check the CVV code on the back of your card and try again.");
                                }


                            }
                            else
                            {
                                //if the transaction mode is not authorize&Capture we need to give refund to customer as the address and CVV not matched.
                             
                            }
                            return result;
                        }
                        // If Authorized, mark payment as Authorized
                        if (transactionMode == TransactMode.Authorize)
                        {
                            result.NewPaymentStatus = PaymentStatus.Authorized;
                            result.AuthorizationTransactionId = response.TransactionResponse.Pnref;
                            result.AuthorizationTransactionResult = response.TransactionResponse.RespMsg;
                        }
                        else
                        {
                            result.NewPaymentStatus = PaymentStatus.Paid;
                            result.CaptureTransactionId = response.TransactionResponse.Pnref;
                            result.CaptureTransactionResult = response.TransactionResponse.RespMsg;
                        }
                    }
                    else
                    {
                        result.AddError(string.Format("{0} - {1}", response.TransactionResponse.Result, response.TransactionResponse.RespMsg));
                        result.AddError(string.Format("Response Code : {0}. Response Description : {1}", response.TransactionResponse.Result, response.TransactionResponse.RespMsg));

                        var getCreditCardDeclineLogs = _creditCardDeclinedLogService.SearchTd_CreditCardDeclinedLogs(_workContext.CurrentCustomer.Id, DateTime.UtcNow.Date);
                        if (getCreditCardDeclineLogs.Count >= 4)
                        {
                            var currentIPaddress = _webHelper.GetCurrentIpAddress();
                            _creditCardDeclinedLogService.AddIPToBannedList(currentIPaddress, _workContext.CurrentCustomer);
                        }

                        var creditCardDeclined = new Td_CreditCardDeclinedLog()
                        {
                            CreatedOn = DateTime.UtcNow.Date,
                            CreditCardDeclinedDueTo = string.Format("Response Code : {0}. Response Description : {1}", response.TransactionResponse.Result, response.TransactionResponse.RespMsg),
                            CustomerId = _workContext.CurrentCustomer.Id
                        };
                        _creditCardDeclinedLogService.InsertCreditCardDeclinedLog(creditCardDeclined);

                        // email notification
                        _workflowMessageService.SendCreditCardDeclineMessage(_workContext.CurrentCustomer, _workContext.WorkingLanguage.Id, "sales@misterlandscaper.com", string.Format("Response Code : {0}. Response Description : {1}", response.TransactionResponse.Result, response.TransactionResponse.RespMsg));
                    }

                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                var workContext = EngineContext.Current.Resolve<IWorkContext>();
                culture = new CultureInfo(workContext.WorkingLanguage.LanguageCulture);
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
            }
        }

        public CapturePaymentResult Capture(CapturePaymentRequest capturePaymentRequest)
        {
            var result = new CapturePaymentResult();
            InitSettings();

            CaptureTransaction CaptureTrans;
            string url = GetPaypalUrl();

            UserInfo userInfo = new UserInfo(user, vendor, partner, password);
            PayflowConnectionData payflowConnectionData = new PayflowConnectionData(url, 443, null, 0, null, null);

            CaptureTrans = new CaptureTransaction(capturePaymentRequest.Order.AuthorizationTransactionId, userInfo, payflowConnectionData, PayflowUtility.RequestId);
            Response response = CaptureTrans.SubmitTransaction();

            if (response.TransactionResponse.Result == 0)
            {
                result.NewPaymentStatus = PaymentStatus.Paid;
                result.CaptureTransactionId = response.TransactionResponse.Pnref;
                result.CaptureTransactionResult = response.TransactionResponse.RespMsg;
            }
            else
            {
                result.NewPaymentStatus = PaymentStatus.Pending;
                result.AddError("Capture Failed");
                result.AddError(response.TransactionResponse.RespMsg);
            }

            return result;
        }

        public RefundPaymentResult Refund(RefundPaymentRequest refundPaymentRequest)
        {
            var result = new RefundPaymentResult();

            string refundurl = "TRXTYPE=C&ORIGID=" + refundPaymentRequest.Order.CaptureTransactionId
                + "&AMT=" + refundPaymentRequest.AmountToRefund
                 + "&USER=" + _paypalDirectPaymentSettings.ApiAccountName
                 + "&VENDOR=" + _paypalDirectPaymentSettings.Vendor
                + "&PARTNER=" + _paypalDirectPaymentSettings.Partner
                 + "&PWD=" + _paypalDirectPaymentSettings.ApiAccountPassword;

            // Create an instance of PayflowNETAPI.
            PayflowNETAPI PayflowNETAPI = new PayflowNETAPI(GetPaypalUrl(), 443, null, 0, null, null);

            // RequestId is a unique string that is required for each & every transaction. 
            // The merchant can use her/his own algorithm to generate 
            // this unique request id or use the SDK provided API to generate this
            // as shown below (PayflowUtility.RequestId).
            string PayPalResponse = PayflowNETAPI.SubmitTransaction(refundurl, PayflowUtility.RequestId);

            //place data from PayPal into a namevaluecollection
            NameValueCollection RequestCollection = GetPayPalCollection(PayflowNETAPI.TransactionRequest);
            NameValueCollection ResponseCollection = GetPayPalCollection(PayPalResponse);

            var resultmsg = ResponseCollection["RESPMSG"];

            if (resultmsg == "Approved")
            {
                result.NewPaymentStatus = PaymentStatus.Refunded;
            }
            else
            {
                result.AddError(resultmsg);
            }

            string TransErrors = PayflowNETAPI.TransactionContext.ToString();

            if (TransErrors != null && TransErrors.Length > 0)
            {
                result.AddError(string.Format("Transaction Errors:- {1}", TransErrors));

            }

            return result;
        }

        public VoidPaymentResult Void(VoidPaymentRequest voidPaymentRequest)
        {
            var result = new VoidPaymentResult();
            // Init
            InitSettings();
            UserInfo userInfo = new UserInfo(user, vendor, partner, password);
            string url = GetPaypalUrl();
            PayflowConnectionData payflowConnectionData = new PayflowConnectionData(url, 443, null, 0, null, null);

            // Attempt to void transaction
            VoidTransaction Trans = new VoidTransaction(voidPaymentRequest.Order.AuthorizationTransactionId, userInfo, payflowConnectionData, PayflowUtility.RequestId);
            Response resp = Trans.SubmitTransaction();

            // Update transaction or alert user to failure
            if (resp.TransactionResponse.Result == 0)
            {
                result.NewPaymentStatus = PaymentStatus.Voided;
            }
            else
            {
                result.AddError(string.Format("Response Code : {0}. Response Description : {1}", resp.TransactionResponse.Result, resp.TransactionResponse.RespMsg));
            }
            return result;
        }

        public ProcessPaymentResult ProcessRecurringPayment(ProcessPaymentRequest processPaymentRequest)
        {
            //throw new NopException("Recurring payments not supported");
            var result = new ProcessPaymentResult();
            result.AddError("Recurring payment not supported");
            return result;
        }

        public CancelRecurringPaymentResult CancelRecurringPayment(CancelRecurringPaymentRequest cancelPaymentRequest)
        {
            //throw new NotImplementedException();
            var result = new CancelRecurringPaymentResult();
            result.AddError("Recurring payment not supported");
            return result;
        }

        public bool CanRePostProcessPayment(Order order)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            //it's not a redirection payment method. So we always return false
            return false;
        }


        /// <summary>
        /// Gets a route for provider configuration
        /// </summary>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "TorchDesignPayflowPro";
            routeValues = new RouteValueDictionary() { { "Namespaces", "Nop.Plugin.TorchDesign.PayflowPro.Controllers" }, { "area", null } };
        }

        /// <summary>
        /// Gets a route for payment info
        /// </summary>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public void GetPaymentInfoRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "PaymentInfo";
            controllerName = "TorchDesignPayflowPro";
            routeValues = new RouteValueDictionary() { { "Namespaces", "Nop.Plugin.TorchDesign.PayflowPro.Controllers" }, { "area", null } };
        }

        public Type GetControllerType()
        {
            return typeof(TorchDesignPayflowProController);
        }
        private NameValueCollection GetPayPalCollection(string payPalInfo)
        {
            //place the responses into collection
            NameValueCollection PayPalCollection =
            new System.Collections.Specialized.NameValueCollection();
            string[] ArrayReponses = payPalInfo.Split('&');

            for (int i = 0; i < ArrayReponses.Length; i++)
            {
                string[] Temp = ArrayReponses[i].Split('=');
                PayPalCollection.Add(Temp[0], Temp[1]);
            }
            return PayPalCollection;
        }


        public override void Install()
        {
            _objectContext.Install();

            //settings
            var settings = new TorchDesignPayflowProSettings()
            {
                TransactMode = TransactMode.Authorize,
                UseSandbox = true,
            };
            _settingService.SaveSetting(settings);

            //locales
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.PayPalDirect.Fields.Partner", "Partner");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.PayPalDirect.Fields.UseSandbox", "Use Sandbox");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.PayPalDirect.Fields.UseSandbox.Hint", "Check to enable Sandbox (testing environment).");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.PayPalDirect.Fields.TransactMode", "Transaction mode");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.PayPalDirect.Fields.TransactMode.Hint", "Specify transaction mode.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.PayPalDirect.Fields.ApiAccountName", "API Account Name");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.PayPalDirect.Fields.ApiAccountName.Hint", "Specify API account name.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.PayPalDirect.Fields.ApiAccountPassword", "API Account Password");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.PayPalDirect.Fields.ApiAccountPassword.Hint", "Specify API account password.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.PayPalDirect.Fields.Signature", "Signature");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.PayPalDirect.Fields.Signature.Hint", "Specify signature.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.PayPalDirect.Fields.AdditionalFee", "Additional fee");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.PayPalDirect.Fields.AdditionalFee.Hint", "Enter additional fee to charge your customers.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.PayPalDirect.Fields.AdditionalFeePercentage", "Additional fee. Use percentage");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.PayPalDirect.Fields.AdditionalFeePercentage.Hint", "Determines whether to apply a percentage additional fee to the order total. If not enabled, a fixed value is used.");

            base.Install();
        }

        public override void Uninstall()
        {
            _objectContext.Uninstall();

            //settings
            _settingService.DeleteSetting<TorchDesignPayflowProSettings>();

            //locales
            this.DeletePluginLocaleResource("Plugins.Payments.PayPalDirect.Fields.Partner");
            this.DeletePluginLocaleResource("Plugins.Payments.PayPalDirect.Fields.UseSandbox");
            this.DeletePluginLocaleResource("Plugins.Payments.PayPalDirect.Fields.UseSandbox.Hint");
            this.DeletePluginLocaleResource("Plugins.Payments.PayPalDirect.Fields.TransactMode");
            this.DeletePluginLocaleResource("Plugins.Payments.PayPalDirect.Fields.TransactMode.Hint");
            this.DeletePluginLocaleResource("Plugins.Payments.PayPalDirect.Fields.ApiAccountName");
            this.DeletePluginLocaleResource("Plugins.Payments.PayPalDirect.Fields.ApiAccountName.Hint");
            this.DeletePluginLocaleResource("Plugins.Payments.PayPalDirect.Fields.ApiAccountPassword");
            this.DeletePluginLocaleResource("Plugins.Payments.PayPalDirect.Fields.ApiAccountPassword.Hint");
            this.DeletePluginLocaleResource("Plugins.Payments.PayPalDirect.Fields.Signature");
            this.DeletePluginLocaleResource("Plugins.Payments.PayPalDirect.Fields.Signature.Hint");
            this.DeletePluginLocaleResource("Plugins.Payments.PayPalDirect.Fields.AdditionalFee");
            this.DeletePluginLocaleResource("Plugins.Payments.PayPalDirect.Fields.AdditionalFee.Hint");
            this.DeletePluginLocaleResource("Plugins.Payments.PayPalDirect.Fields.AdditionalFeePercentage");
            this.DeletePluginLocaleResource("Plugins.Payments.PayPalDirect.Fields.AdditionalFeePercentage.Hint");

            base.Uninstall();
        }
        #endregion

        #region Properies
        public bool SupportCapture
        {
            get
            {
                return true;
            }
        }

        public bool SupportPartiallyRefund
        {
            get
            {
                return true;
            }
        }

        public bool SupportRefund
        {
            get
            {
                return true;
            }
        }

        public bool SupportVoid
        {

            get
            {
                return true;
            }
        }

        public RecurringPaymentType RecurringPaymentType
        {

            get
            {
                return RecurringPaymentType.Automatic;
            }
        }
        /// <summary>
        /// Gets a payment method type
        /// </summary>
        public PaymentMethodType PaymentMethodType
        {
            get
            {
                return PaymentMethodType.Standard;
            }
        }


        public bool SkipPaymentInfo
        {
            get
            {
                return false;
            }
        }
        #endregion
    }
}