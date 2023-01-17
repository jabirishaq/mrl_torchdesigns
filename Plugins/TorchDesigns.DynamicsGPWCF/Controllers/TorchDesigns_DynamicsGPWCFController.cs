using System.Web.Mvc;
using Nop.Core;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Web.Framework.Controllers;
using Nop.Services.Stores;
using Nop.Data;
using Nop.Core.Data;
using System.Net;
using Nop.Core.Infrastructure;
using Nop.Plugin.TorchDesigns.DynamicsGPWCF.Models;
using Nop.Plugin.TorchDesigns.DynamicsGPWCF.DynamicsGP;
using Nop.Services.Logging;
using Nop.Services.Orders;
using Nop.Services.Shipping;
using Nop.Services.Customers;
using Nop.Services.Common;
using Nop.Services.Tax;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Nop.Plugin.TorchDesigns.DynamicsGPWCF.Controllers
{

    public class TorchDesigns_DynamicsGPWCFController : BasePluginController
    {
        private readonly IDbContext _dbContext;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly ISettingService _settingService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly IDataProvider _dataProvider;
        private readonly TorchDesignsDynamicsGPWCFSettings _torchDesignsDynamicsGPSettings;
        private readonly ILogger _logger;
        private readonly IOrderService _orderService;
        private readonly IShippingService _shippingService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ICustomerService _customerService;
        private readonly ITaxService _taxService;
        private readonly IAddressService _addressService;
        //private readonly GreatPlains _greatPlains;


        public TorchDesigns_DynamicsGPWCFController(IDbContext dbContext, ILanguageService languageService,
            ILocalizationService localizationService, ILocalizedEntityService localizedEntityService,
            ISettingService settingService, IStoreService storeService, IWorkContext workContext,
            IDataProvider dataProvider, ILogger logger,
            IShippingService shippingService, IGenericAttributeService genericAttributeService,
             ICustomerService customerService, ITaxService taxService, IAddressService addressService, IOrderService orderService)
        {
            this._dbContext = dbContext;
            this._languageService = languageService;
            this._localizationService = localizationService;
            this._localizedEntityService = localizedEntityService;
            this._settingService = settingService;
            this._storeService = storeService;
            this._workContext = workContext;
            this._dataProvider = dataProvider;
            this._torchDesignsDynamicsGPSettings = EngineContext.Current.Resolve<TorchDesignsDynamicsGPWCFSettings>();
            this._orderService = orderService;
            this._logger = logger;
            this._shippingService = shippingService;
            this._genericAttributeService = genericAttributeService;
            this._customerService = customerService;
            this._taxService = taxService;
            this._addressService = addressService;
            //_greatPlains = new GreatPlains(_settingService, _logger, _orderService, _shippingService, _genericAttributeService, _customerService, _taxService, _addressService);
            // Get Customer List

        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            //little hack here
            //always set culture to 'en-US' (Telerik has a bug related to editing decimal values in other cultures). Like currently it's done for admin area in Global.asax.cs
            CommonHelper.SetTelerikCulture();

            base.Initialize(requestContext);
        }

        protected bool IsPasswordBase64Encoded(string str)
        {
            try
            {
                // If no exception is caught, then it is possibly a base64 encoded string
                byte[] data = Convert.FromBase64String(str);
                // The part that checks if the string was properly padded to the
                return (str.Replace(" ", "").Length % 4 == 0);
            }
            catch
            {
                // If exception is caught, then it is not a base64 encoded string
                return false;
            }
        }


        [AdminAuthorize]
        public ActionResult Configure()
        {
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var dynamicsGPSetting = _settingService.LoadSetting<TorchDesignsDynamicsGPWCFSettings>(storeScope);
            var model = new ConfigurationModel()
            {
                CurrencyKeyISOCode = dynamicsGPSetting.CurrencyKeyISOCode,
                PaymentCardTypeKeyId = dynamicsGPSetting.PaymentCardTypeKeyId,
                SalespersonKeyId = dynamicsGPSetting.SalespersonKeyId,
                SalesTerritoryKeyId = dynamicsGPSetting.SalesTerritoryKeyId,
                TaxScheduleId = dynamicsGPSetting.TaxScheduleId,
                //TaxScheduleIdForCustomersOutsideFlorida = dynamicsGPSetting.TaxScheduleIdForCustomersOutsideFlorida,
                UserName = dynamicsGPSetting.UserName,
                Password = dynamicsGPSetting.Password,
                PONumberPrefix = dynamicsGPSetting.PONumberPrefix,
                Domain = dynamicsGPSetting.Domain,
                WebServiceAddress = dynamicsGPSetting.WebServiceAddress,
                CompanyKey = dynamicsGPSetting.CompanyKey

            };
            IList<DynamicsGP.Company> companylist = new List<DynamicsGP.Company>();
            //companylist = _greatPlains.CompanyList;
            if (string.IsNullOrEmpty(_torchDesignsDynamicsGPSettings.WebServiceAddress) ||
                string.IsNullOrWhiteSpace(_torchDesignsDynamicsGPSettings.UserName) ||
                string.IsNullOrWhiteSpace(_torchDesignsDynamicsGPSettings.Password) ||
                string.IsNullOrWhiteSpace(_torchDesignsDynamicsGPSettings.Domain))
            {
                if (string.IsNullOrEmpty(_torchDesignsDynamicsGPSettings.WebServiceAddress))
                    model.Errors.Add("Plugins.TorchDesign.DynamicsGP.Error.WebserviceAddress");
                if (string.IsNullOrEmpty(_torchDesignsDynamicsGPSettings.UserName))
                    model.Errors.Add("Plugins.TorchDesign.DynamicsGP.Error.UserName");
                if (string.IsNullOrEmpty(_torchDesignsDynamicsGPSettings.Password))
                    model.Errors.Add("Plugins.TorchDesign.DynamicsGP.Error.Password");
                if (string.IsNullOrEmpty(_torchDesignsDynamicsGPSettings.Domain))
                    model.Errors.Add("Plugins.TorchDesign.DynamicsGP.Error.Domain");
            }
            // Create Service
           

            if (!string.IsNullOrEmpty(_torchDesignsDynamicsGPSettings.WebServiceAddress) && !string.IsNullOrWhiteSpace(_torchDesignsDynamicsGPSettings.UserName) && !string.IsNullOrWhiteSpace(_torchDesignsDynamicsGPSettings.Password) && !string.IsNullOrWhiteSpace(_torchDesignsDynamicsGPSettings.Domain))
            {
                try
                {
                    DynamicsGP.DynamicsGP gpService = new DynamicsGP.DynamicsGP();
                    gpService.Url = _torchDesignsDynamicsGPSettings.WebServiceAddress;
                    var URL = gpService.Url.ToString();
                    if (storeScope > 0)
                    {
                        model.CurrencyKeyISOCode_OverrideForStore = _settingService.SettingExists(dynamicsGPSetting, x => x.CurrencyKeyISOCode, storeScope);
                        model.PaymentCardTypeKeyId_OverrideForStore = _settingService.SettingExists(dynamicsGPSetting, x => x.PaymentCardTypeKeyId, storeScope);
                        model.SalespersonKeyId_OverrideForStore = _settingService.SettingExists(dynamicsGPSetting, x => x.SalespersonKeyId, storeScope);
                        model.SalesTerritoryKeyId_OverrideForStore = _settingService.SettingExists(dynamicsGPSetting, x => x.SalesTerritoryKeyId, storeScope);
                        model.TaxScheduleId_OverrideForStore = _settingService.SettingExists(dynamicsGPSetting, x => x.TaxScheduleId, storeScope);
                        //model.TaxScheduleIdForCustomersOutsideFlorida_OverrideForStore = _settingService.SettingExists(dynamicsGPSetting, x => x.TaxScheduleIdForCustomersOutsideFlorida, storeScope);
                        model.UserName_OverrideForStore = _settingService.SettingExists(dynamicsGPSetting, x => x.UserName, storeScope);
                        model.Password_OverrideForStore = _settingService.SettingExists(dynamicsGPSetting, x => x.Password, storeScope);
                        model.PONumberPrefix_OverrideForStore = _settingService.SettingExists(dynamicsGPSetting, x => x.PONumberPrefix, storeScope);
                        model.Domain_OverrideForStore = _settingService.SettingExists(dynamicsGPSetting, x => x.Domain, storeScope);
                        model.WebServiceAddress_OverrideForStore = _settingService.SettingExists(dynamicsGPSetting, x => x.WebServiceAddress, storeScope);
                        model.CompanyKey_OverrideForStore = _settingService.SettingExists(dynamicsGPSetting, x => x.CompanyKey, storeScope);

                    }
                    _logger.Information("GPServiceAddress = " + URL);
                    var password = string.Empty;
                    var base64EncodedBytes = System.Convert.FromBase64String(_torchDesignsDynamicsGPSettings.Password);
                    password = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
                    NetworkCredential credentials = new NetworkCredential(_torchDesignsDynamicsGPSettings.UserName, password, _torchDesignsDynamicsGPSettings.Domain);
                    gpService.Credentials = credentials;
                    // Get Company List
                    DynamicsGP.Company[] gpCompanies = gpService.GetCompanyList(new DynamicsGP.CompanyCriteria(), new DynamicsGP.Context());
                    companylist = gpCompanies.ToList();
                    if (companylist != null && companylist.Count > 0)
                    {
                        model.Status = true;
                        model.AvailableCompanies = companylist.Select(x => new SelectListItem()
                        {
                            Text = x.Name.ToString(),
                            Value = x.Key.Id.ToString()
                        }).ToList();

                    }
                    else
                    {
                        model.Status = false;
                        model.AvailableCompanies.Add(new SelectListItem
                        {
                            Text = "TEST MRL <TEST>",
                            Value = "4"
                        });
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error("GreatPlains.BindData:  Could not connect to Great Plains.", ex, null);
                    model.Status = false;
                    model.AvailableCompanies.Add(new SelectListItem
                    {
                        Text = "TEST MRL <TEST>",
                        Value = "4"
                    });
                    return View("~/Plugins/TorchDesigns.DynamicsGPWCF/Views/TorchDesigns_DynamicsGPWCF/Configure.cshtml", model);
                }

            }


            //model.WebServiceAddress = _settingService.GetSettingByKey<string>("gp_webserviceaddress");


            return View("~/Plugins/TorchDesigns.DynamicsGPWCF/Views/TorchDesigns_DynamicsGPWCF/Configure.cshtml", model);
        }

        [HttpPost, ActionName("Configure")]
        [FormValueRequired("SaveGPSettingBtn")]
        public ActionResult Savesetting(ConfigurationModel model)
        {
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var dynamicsGPSetting = _settingService.LoadSetting<TorchDesignsDynamicsGPWCFSettings>(storeScope);
            dynamicsGPSetting.CurrencyKeyISOCode = model.CurrencyKeyISOCode;
            dynamicsGPSetting.PaymentCardTypeKeyId = model.PaymentCardTypeKeyId;
            dynamicsGPSetting.SalespersonKeyId = model.SalespersonKeyId;
            dynamicsGPSetting.SalesTerritoryKeyId = model.SalesTerritoryKeyId;
            dynamicsGPSetting.TaxScheduleId = model.TaxScheduleId;
            //dynamicsGPSetting.TaxScheduleIdForCustomersOutsideFlorida = model.TaxScheduleIdForCustomersOutsideFlorida;
            dynamicsGPSetting.UserName = model.UserName;
            //dynamicsGPSetting.Password = model.Password;
            dynamicsGPSetting.PONumberPrefix = model.PONumberPrefix;
            dynamicsGPSetting.Domain = model.Domain;
            dynamicsGPSetting.WebServiceAddress = model.WebServiceAddress;
            dynamicsGPSetting.CompanyKey = model.CompanyKey;


            _settingService.SaveSetting(dynamicsGPSetting);
            DynamicsGP.DynamicsGP gpService = new DynamicsGP.DynamicsGP();
            gpService.Url = model.WebServiceAddress;
            _logger.Information("gpservice.url = " + gpService.Url.ToString());
            var isPasswordEncoded = IsPasswordBase64Encoded(model.Password);
            if (!isPasswordEncoded)
            {
                var passwordBytes = System.Text.Encoding.UTF8.GetBytes(model.Password);
                dynamicsGPSetting.Password = System.Convert.ToBase64String(passwordBytes);
            }
            else
                dynamicsGPSetting.Password = model.Password;
            //_settingService.SetSetting<string>("gp_webserviceaddress", model.WebServiceAddress);

            if (model.CurrencyKeyISOCode_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(dynamicsGPSetting, x => x.CurrencyKeyISOCode, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(dynamicsGPSetting, x => x.CurrencyKeyISOCode, storeScope);

            if (model.PaymentCardTypeKeyId_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(dynamicsGPSetting, x => x.PaymentCardTypeKeyId, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(dynamicsGPSetting, x => x.PaymentCardTypeKeyId, storeScope);

            if (model.SalespersonKeyId_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(dynamicsGPSetting, x => x.SalespersonKeyId, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(dynamicsGPSetting, x => x.SalespersonKeyId, storeScope);

            if (model.SalesTerritoryKeyId_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(dynamicsGPSetting, x => x.SalesTerritoryKeyId, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(dynamicsGPSetting, x => x.SalesTerritoryKeyId, storeScope);

            if (model.TaxScheduleId_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(dynamicsGPSetting, x => x.TaxScheduleId, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(dynamicsGPSetting, x => x.TaxScheduleId, storeScope);

            //if (model.TaxScheduleIdForCustomersOutsideFlorida_OverrideForStore || storeScope == 0)
            //    _settingService.SaveSetting(dynamicsGPSetting, x => x.TaxScheduleIdForCustomersOutsideFlorida, storeScope, false);
            //else if (storeScope > 0)
            //    _settingService.DeleteSetting(dynamicsGPSetting, x => x.TaxScheduleIdForCustomersOutsideFlorida, storeScope);

            if (model.UserName_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(dynamicsGPSetting, x => x.UserName, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(dynamicsGPSetting, x => x.UserName, storeScope);

            if (model.Password_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(dynamicsGPSetting, x => x.Password, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(dynamicsGPSetting, x => x.Password, storeScope);

            if (model.PONumberPrefix_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(dynamicsGPSetting, x => x.PONumberPrefix, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(dynamicsGPSetting, x => x.PONumberPrefix, storeScope);

            if (model.Domain_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(dynamicsGPSetting, x => x.Domain, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(dynamicsGPSetting, x => x.Domain, storeScope);

            //if (model.WebServiceAddress_OverrideForStore || storeScope == 0)
            //    _settingService.SaveSetting(dynamicsGPSetting, x => x.WebServiceAddress, storeScope, false);
            //else if (storeScope > 0)
            //    _settingService.DeleteSetting(dynamicsGPSetting, x => x.WebServiceAddress, storeScope);

            //if (model.CompanyKey_OverrideForStore || storeScope == 0)
            //    _settingService.SaveSetting(dynamicsGPSetting, x => x.CompanyKey, storeScope, false);
            //else if (storeScope > 0)
            //    _settingService.DeleteSetting(dynamicsGPSetting, x => x.CompanyKey, storeScope);
            _settingService.ClearCache();
            SuccessNotification(_localizationService.GetResource("Plugins.TorchDesign.DynamicsGp.SuccessSave"));
            return Configure();
            //return View("~/Plugins/TorchDesigns.DynamicsGPWCF/Views/TorchDesigns_DynamicsGPWCF/Configure.cshtml", model);
        }

        [HttpPost, ActionName("Configure")]
        [FormValueRequired("TestConnection")]
        public ActionResult TestConnection(ConfigurationModel model)
        {
            // Create Service
            DynamicsGP.DynamicsGP gpService = new DynamicsGP.DynamicsGP();
            gpService.Url = _torchDesignsDynamicsGPSettings.WebServiceAddress;
            var URL = gpService.Url.ToString();
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var dynamicsGPSetting = _settingService.LoadSetting<TorchDesignsDynamicsGPWCFSettings>(storeScope);
            dynamicsGPSetting.CurrencyKeyISOCode = model.CurrencyKeyISOCode;
            dynamicsGPSetting.PaymentCardTypeKeyId = model.PaymentCardTypeKeyId;
            dynamicsGPSetting.SalespersonKeyId = model.SalespersonKeyId;
            dynamicsGPSetting.SalesTerritoryKeyId = model.SalesTerritoryKeyId;
            dynamicsGPSetting.TaxScheduleId = model.TaxScheduleId;
            //dynamicsGPSetting.TaxScheduleIdForCustomersOutsideFlorida = model.TaxScheduleIdForCustomersOutsideFlorida;
            dynamicsGPSetting.UserName = model.UserName;
            //dynamicsGPSetting.Password = model.Password;
            dynamicsGPSetting.PONumberPrefix = model.PONumberPrefix;
            dynamicsGPSetting.Domain = model.Domain;
            dynamicsGPSetting.WebServiceAddress = model.WebServiceAddress;
            dynamicsGPSetting.CompanyKey = model.CompanyKey;

            var isPasswordEncoded = IsPasswordBase64Encoded(model.Password);
            if (!isPasswordEncoded)
            {
                var passwordBytes = System.Text.Encoding.UTF8.GetBytes(model.Password);
                dynamicsGPSetting.Password = System.Convert.ToBase64String(passwordBytes);
            }
            else
                dynamicsGPSetting.Password = model.Password;
            //_settingService.SetSetting<string>("gp_webserviceaddress", model.WebServiceAddress);

            if (model.CurrencyKeyISOCode_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(dynamicsGPSetting, x => x.CurrencyKeyISOCode, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(dynamicsGPSetting, x => x.CurrencyKeyISOCode, storeScope);

            if (model.PaymentCardTypeKeyId_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(dynamicsGPSetting, x => x.PaymentCardTypeKeyId, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(dynamicsGPSetting, x => x.PaymentCardTypeKeyId, storeScope);

            if (model.SalespersonKeyId_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(dynamicsGPSetting, x => x.SalespersonKeyId, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(dynamicsGPSetting, x => x.SalespersonKeyId, storeScope);

            if (model.SalesTerritoryKeyId_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(dynamicsGPSetting, x => x.SalesTerritoryKeyId, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(dynamicsGPSetting, x => x.SalesTerritoryKeyId, storeScope);

            if (model.TaxScheduleId_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(dynamicsGPSetting, x => x.TaxScheduleId, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(dynamicsGPSetting, x => x.TaxScheduleId, storeScope);

            //if (model.TaxScheduleIdForCustomersOutsideFlorida_OverrideForStore || storeScope == 0)
            //    _settingService.SaveSetting(dynamicsGPSetting, x => x.TaxScheduleIdForCustomersOutsideFlorida, storeScope, false);
            //else if (storeScope > 0)
            //    _settingService.DeleteSetting(dynamicsGPSetting, x => x.TaxScheduleIdForCustomersOutsideFlorida, storeScope);

            if (model.UserName_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(dynamicsGPSetting, x => x.UserName, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(dynamicsGPSetting, x => x.UserName, storeScope);

            if (model.Password_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(dynamicsGPSetting, x => x.Password, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(dynamicsGPSetting, x => x.Password, storeScope);

            if (model.PONumberPrefix_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(dynamicsGPSetting, x => x.PONumberPrefix, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(dynamicsGPSetting, x => x.PONumberPrefix, storeScope);

            if (model.Domain_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(dynamicsGPSetting, x => x.Domain, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(dynamicsGPSetting, x => x.Domain, storeScope);

            //if (model.WebServiceAddress_OverrideForStore || storeScope == 0)
            //    _settingService.SaveSetting(dynamicsGPSetting, x => x.WebServiceAddress, storeScope, false);
            //else if (storeScope > 0)
            //    _settingService.DeleteSetting(dynamicsGPSetting, x => x.WebServiceAddress, storeScope);

            //if (model.CompanyKey_OverrideForStore || storeScope == 0)
            //    _settingService.SaveSetting(dynamicsGPSetting, x => x.CompanyKey, storeScope, false);
            //else if (storeScope > 0)
            //    _settingService.DeleteSetting(dynamicsGPSetting, x => x.CompanyKey, storeScope);

           // _settingService.ClearCache();
            // Set Credentials
            var ConnectionModel = new TestConnectionModel();
            if (string.IsNullOrEmpty(_torchDesignsDynamicsGPSettings.WebServiceAddress) ||
               string.IsNullOrWhiteSpace(_torchDesignsDynamicsGPSettings.UserName) ||
               string.IsNullOrWhiteSpace(_torchDesignsDynamicsGPSettings.Password) ||
               string.IsNullOrWhiteSpace(_torchDesignsDynamicsGPSettings.Domain))
            {
                if (string.IsNullOrEmpty(_torchDesignsDynamicsGPSettings.WebServiceAddress))
                    ConnectionModel.Errors.Add("Plugins.TorchDesign.DynamicsGP.Error.WebserviceAddress");
                if (string.IsNullOrEmpty(_torchDesignsDynamicsGPSettings.UserName))
                    ConnectionModel.Errors.Add("Plugins.TorchDesign.DynamicsGP.Error.UserName");
                if (string.IsNullOrEmpty(_torchDesignsDynamicsGPSettings.Password))
                    ConnectionModel.Errors.Add("Plugins.TorchDesign.DynamicsGP.Error.Password");
                if (string.IsNullOrEmpty(_torchDesignsDynamicsGPSettings.Domain))
                    ConnectionModel.Errors.Add("Plugins.TorchDesign.DynamicsGP.Error.Domain");
            }
            // Create Service
            if (!string.IsNullOrWhiteSpace(_torchDesignsDynamicsGPSettings.WebServiceAddress) && !string.IsNullOrWhiteSpace(_torchDesignsDynamicsGPSettings.UserName) && !string.IsNullOrWhiteSpace(_torchDesignsDynamicsGPSettings.Password) && !string.IsNullOrWhiteSpace(_torchDesignsDynamicsGPSettings.Domain))
            {
                try
                {
                    _logger.Information("GPServiceAddress = " + URL);
                    var password = string.Empty;
                    var base64EncodedBytes = System.Convert.FromBase64String(_torchDesignsDynamicsGPSettings.Password);
                    password = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
                    NetworkCredential credentials = new NetworkCredential(_torchDesignsDynamicsGPSettings.UserName, password, _torchDesignsDynamicsGPSettings.Domain);
                    gpService.Credentials = credentials;
                    // Get Company List
                    DynamicsGP.Company[] gpCompanies = gpService.GetCompanyList(new DynamicsGP.CompanyCriteria(), new DynamicsGP.Context());
                    foreach (var companyName in gpCompanies)
                    {
                        ConnectionModel.CompanyName.Add(companyName.Name);
                        if (companyName.Key.Id > 0)
                        {
                            ConnectionModel.CompanyName.Add(companyName.Key.Id.ToString());
                        }
                    }
                    //var customerOFUser =  _greatPlains.GetService().GetCustomerByKey(new CustomerKey() { Id = "FARNSWORTH-K-37" }, _greatPlains.GpContext);
                    // var customers = gpService.GetCustomerList(new CustomerCriteria() { Scope = CustomerScope.ReturnAll}, _greatPlains.GpContext);
                    // foreach (var customer in customers)
                    // {
                    //     model.CustomerName.Add(customer.Name);

                    // }


                    return View("~/Plugins/TorchDesigns.DynamicsGPWCF/Views/TorchDesigns_DynamicsGPWCF/TestConnection.cshtml", ConnectionModel);
                }
                catch (Exception ex)
                {
                    var errorModel = new TestConnectionModel();
                    errorModel.Errors.Add(ex.Message);
                    return View("~/Plugins/TorchDesigns.DynamicsGPWCF/Views/TorchDesigns_DynamicsGPWCF/TestConnection.cshtml", errorModel);
                }
               
            }
            else
            {
                ErrorNotification(_localizationService.GetResource("Plugins.TorchDesign.DynamicsGp.ErrorNotify"));
                //return Redirect(Request.UrlReferrer.ToString());
                return View("~/Plugins/TorchDesigns.DynamicsGPWCF/Views/TorchDesigns_DynamicsGPWCF/TestConnection.cshtml", ConnectionModel);

            }
        }
    }
}
