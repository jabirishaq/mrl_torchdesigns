using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Domain.Directory;
using Nop.Plugin.Shipping.ByWeight.Domain;
using Nop.Plugin.Shipping.ByWeight.Models;
using Nop.Plugin.Shipping.ByWeight.Services;
using Nop.Services.Configuration;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Services.Shipping;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Kendoui;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Shipping.ByWeight.Controllers
{
    [AdminAuthorize]
    public class ShippingByWeightController : BasePluginController
    {
        private readonly IShippingService _shippingService;
        private readonly IStoreService _storeService;
        private readonly ICountryService _countryService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly ShippingByWeightSettings _shippingByWeightSettings;
        private readonly IShippingByWeightService _shippingByWeightService;
        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;

        private readonly ICurrencyService _currencyService;
        private readonly CurrencySettings _currencySettings;
        private readonly IMeasureService _measureService;
        private readonly MeasureSettings _measureSettings;

        public ShippingByWeightController(IShippingService shippingService,
            IStoreService storeService, ICountryService countryService, IStateProvinceService stateProvinceService,
            ShippingByWeightSettings shippingByWeightSettings,
            IShippingByWeightService shippingByWeightService, ISettingService settingService,
            ILocalizationService localizationService, IPermissionService permissionService,
            ICurrencyService currencyService, CurrencySettings currencySettings,
            IMeasureService measureService, MeasureSettings measureSettings)
        {
            this._shippingService = shippingService;
            this._storeService = storeService;
            this._countryService = countryService;
            this._stateProvinceService = stateProvinceService;
            this._shippingByWeightSettings = shippingByWeightSettings;
            this._shippingByWeightService = shippingByWeightService;
            this._settingService = settingService;
            this._localizationService = localizationService;
            this._permissionService = permissionService;

            this._currencyService = currencyService;
            this._currencySettings = currencySettings;
            this._measureService = measureService;
            this._measureSettings = measureSettings;
        }
        
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            //little hack here
            //always set culture to 'en-US' (Telerik has a bug related to editing decimal values in other cultures). Like currently it's done for admin area in Global.asax.cs
            CommonHelper.SetTelerikCulture();

            base.Initialize(requestContext);
        }

        [ChildActionOnly]
        public ActionResult Configure()
        {
            var model = new ShippingByWeightListModel();
            //other settings
            model.LimitMethodsToCreated = _shippingByWeightSettings.LimitMethodsToCreated;

            return View("~/Plugins/Shipping.ByWeight/Views/ShippingByWeight/Configure.cshtml", model);
        }

        [HttpPost]
        public ActionResult SaveGeneralSettings(ShippingByWeightListModel model)
        {
            //save settings
            _shippingByWeightSettings.LimitMethodsToCreated = model.LimitMethodsToCreated;
            _settingService.SaveSetting(_shippingByWeightSettings);

            return Json(new { Result = true });
        }

        [HttpPost]
        public ActionResult RatesList(DataSourceRequest command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageShippingSettings))
                return Content("Access denied");

            var records = _shippingByWeightService.GetAll(command.Page - 1, command.PageSize);
            var sbwModel = records.Select(x =>
                {
                    var m = new ShippingByWeightModel()
                    {
                        Id = x.Id,
                        StoreId = x.StoreId,
                        ShippingMethodId = x.ShippingMethodId,
                        CountryId = x.CountryId,
                        From = x.From,
                        To = x.To,
                        AdditionalFixedCost = x.AdditionalFixedCost,
                        PercentageRateOfSubtotal = x.PercentageRateOfSubtotal,
                        RatePerWeightUnit = x.RatePerWeightUnit,
                        LowerWeightLimit = x.LowerWeightLimit,
                    };
                    //shipping method
                    var shippingMethod = _shippingService.GetShippingMethodById(x.ShippingMethodId);
                    m.ShippingMethodName = (shippingMethod != null) ? shippingMethod.Name : "Unavailable";
                    //store
                    var store = _storeService.GetStoreById(x.StoreId);
                    m.StoreName = (store != null) ? store.Name : "*";
                    //country
                    var c = _countryService.GetCountryById(x.CountryId);
                    m.CountryName = (c != null) ? c.Name : "*";
                    //state
                    var s = _stateProvinceService.GetStateProvinceById(x.StateProvinceId);
                    m.StateProvinceName = (s != null) ? s.Name : "*";
                    //zip
                    m.Zip = (!String.IsNullOrEmpty(x.Zip)) ? x.Zip : "*";


                    var htmlSb = new StringBuilder("<div>");
                    htmlSb.AppendFormat("{0}: {1}", _localizationService.GetResource("Plugins.Shipping.ByWeight.Fields.From"), m.From);
                    htmlSb.Append("<br />");
                    htmlSb.AppendFormat("{0}: {1}", _localizationService.GetResource("Plugins.Shipping.ByWeight.Fields.To"), m.To);
                    htmlSb.Append("<br />");
                    htmlSb.AppendFormat("{0}: {1}", _localizationService.GetResource("Plugins.Shipping.ByWeight.Fields.AdditionalFixedCost"), m.AdditionalFixedCost);
                    htmlSb.Append("<br />");
                    htmlSb.AppendFormat("{0}: {1}", _localizationService.GetResource("Plugins.Shipping.ByWeight.Fields.RatePerWeightUnit"), m.RatePerWeightUnit);
                    htmlSb.Append("<br />");
                    htmlSb.AppendFormat("{0}: {1}", _localizationService.GetResource("Plugins.Shipping.ByWeight.Fields.LowerWeightLimit"), m.LowerWeightLimit);
                    htmlSb.Append("<br />");
                    htmlSb.AppendFormat("{0}: {1}", _localizationService.GetResource("Plugins.Shipping.ByWeight.Fields.PercentageRateOfSubtotal"), m.PercentageRateOfSubtotal);
                    
                    htmlSb.Append("</div>");
                    m.DataHtml = htmlSb.ToString();

                    return m;
                })
                .ToList();
            var gridModel = new DataSourceResult
            {
                Data = sbwModel,
                Total = records.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        public ActionResult RateDelete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageShippingSettings))
                return Content("Access denied");

            var sbw = _shippingByWeightService.GetById(id);
            if (sbw != null)
                _shippingByWeightService.DeleteShippingByWeightRecord(sbw);

            return new NullJsonResult();
        }

        //add
        public ActionResult AddPopup()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageShippingSettings))
                return Content("Access denied");

            var model = new ShippingByWeightModel();
            model.PrimaryStoreCurrencyCode = _currencyService.GetCurrencyById(_currencySettings.PrimaryStoreCurrencyId).CurrencyCode;
            model.BaseWeightIn = _measureService.GetMeasureWeightById(_measureSettings.BaseWeightId).Name;
            model.To = 1000000;

            var shippingMethods = _shippingService.GetAllShippingMethods();
            if (shippingMethods.Count == 0)
                return Content("No shipping methods can be loaded");

            //stores
            model.AvailableStores.Add(new SelectListItem() { Text = "*", Value = "0" });
            foreach (var store in _storeService.GetAllStores())
                model.AvailableStores.Add(new SelectListItem() { Text = store.Name, Value = store.Id.ToString() });
            //shipping methods
            foreach (var sm in shippingMethods)
                model.AvailableShippingMethods.Add(new SelectListItem() { Text = sm.Name, Value = sm.Id.ToString() });
            //countries
            model.AvailableCountries.Add(new SelectListItem() { Text = "*", Value = "0" });
            var countries = _countryService.GetAllCountries(true);
            foreach (var c in countries)
                model.AvailableCountries.Add(new SelectListItem() { Text = c.Name, Value = c.Id.ToString() });
            //states
            model.AvailableStates.Add(new SelectListItem() { Text = "*", Value = "0" });

            return View("~/Plugins/Shipping.ByWeight/Views/ShippingByWeight/AddPopup.cshtml", model);
        }
        [HttpPost]
        public ActionResult AddPopup(string btnId, string formId, ShippingByWeightModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageShippingSettings))
                return Content("Access denied");

            var sbw = new ShippingByWeightRecord()
            {
                StoreId = model.StoreId,
                CountryId = model.CountryId,
                StateProvinceId = model.StateProvinceId,
                Zip = model.Zip == "*" ? null : model.Zip,
                ShippingMethodId = model.ShippingMethodId,
                From = model.From,
                To = model.To,
                AdditionalFixedCost = model.AdditionalFixedCost,
                RatePerWeightUnit = model.RatePerWeightUnit,
                PercentageRateOfSubtotal = model.PercentageRateOfSubtotal,
                LowerWeightLimit = model.LowerWeightLimit
            };
            _shippingByWeightService.InsertShippingByWeightRecord(sbw);

            ViewBag.RefreshPage = true;
            ViewBag.btnId = btnId;
            ViewBag.formId = formId;

            return View("~/Plugins/Shipping.ByWeight/Views/ShippingByWeight/AddPopup.cshtml", model);
        }

        //edit
        public ActionResult EditPopup(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageShippingSettings))
                return Content("Access denied");

            var sbw = _shippingByWeightService.GetById(id);
            if (sbw == null)
                //No record found with the specified id
                return RedirectToAction("Configure");

            var model = new ShippingByWeightModel()
            {
                Id = sbw.Id,
                StoreId = sbw.StoreId,
                CountryId = sbw.CountryId,
                StateProvinceId = sbw.StateProvinceId,
                Zip = sbw.Zip,
                ShippingMethodId = sbw.ShippingMethodId,
                From = sbw.From,
                To = sbw.To,
                AdditionalFixedCost = sbw.AdditionalFixedCost,
                PercentageRateOfSubtotal = sbw.PercentageRateOfSubtotal,
                RatePerWeightUnit = sbw.RatePerWeightUnit,
                LowerWeightLimit = sbw.LowerWeightLimit,
                PrimaryStoreCurrencyCode = _currencyService.GetCurrencyById(_currencySettings.PrimaryStoreCurrencyId).CurrencyCode,
                BaseWeightIn = _measureService.GetMeasureWeightById(_measureSettings.BaseWeightId).Name
            };

            var shippingMethods = _shippingService.GetAllShippingMethods();
            if (shippingMethods.Count == 0)
                return Content("No shipping methods can be loaded");

            var selectedStore = _storeService.GetStoreById(sbw.StoreId);
            var selectedShippingMethod = _shippingService.GetShippingMethodById(sbw.ShippingMethodId);
            var selectedCountry = _countryService.GetCountryById(sbw.CountryId);
            var selectedState = _stateProvinceService.GetStateProvinceById(sbw.StateProvinceId);
            //stores
            model.AvailableStores.Add(new SelectListItem() { Text = "*", Value = "0" });
            foreach (var store in _storeService.GetAllStores())
                model.AvailableStores.Add(new SelectListItem() { Text = store.Name, Value = store.Id.ToString(), Selected = (selectedStore != null && store.Id == selectedStore.Id) });
            //shipping methods
            foreach (var sm in shippingMethods)
                model.AvailableShippingMethods.Add(new SelectListItem() { Text = sm.Name, Value = sm.Id.ToString(), Selected = (selectedShippingMethod != null && sm.Id == selectedShippingMethod.Id) });
            //countries
            model.AvailableCountries.Add(new SelectListItem() { Text = "*", Value = "0" });
            var countries = _countryService.GetAllCountries(true);
            foreach (var c in countries)
                model.AvailableCountries.Add(new SelectListItem() { Text = c.Name, Value = c.Id.ToString(), Selected = (selectedCountry != null && c.Id == selectedCountry.Id) });
            //states
            var states = selectedCountry != null ? _stateProvinceService.GetStateProvincesByCountryId(selectedCountry.Id, true).ToList() : new List<StateProvince>();
            model.AvailableStates.Add(new SelectListItem() { Text = "*", Value = "0" });
            foreach (var s in states)
                model.AvailableStates.Add(new SelectListItem() { Text = s.Name, Value = s.Id.ToString(), Selected = (selectedState != null && s.Id == selectedState.Id) });

            return View("~/Plugins/Shipping.ByWeight/Views/ShippingByWeight/EditPopup.cshtml", model);
        }
        [HttpPost]
        public ActionResult EditPopup(string btnId, string formId, ShippingByWeightModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageShippingSettings))
                return Content("Access denied");

            var sbw = _shippingByWeightService.GetById(model.Id);
            if (sbw == null)
                //No record found with the specified id
                return RedirectToAction("Configure");

            sbw.StoreId = model.StoreId;
            sbw.CountryId = model.CountryId;
            sbw.StateProvinceId = model.StateProvinceId;
            sbw.Zip = model.Zip == "*" ? null : model.Zip;
            sbw.ShippingMethodId = model.ShippingMethodId;
            sbw.From = model.From;
            sbw.To = model.To;
            sbw.AdditionalFixedCost = model.AdditionalFixedCost;
            sbw.RatePerWeightUnit = model.RatePerWeightUnit;
            sbw.PercentageRateOfSubtotal = model.PercentageRateOfSubtotal;
            sbw.LowerWeightLimit = model.LowerWeightLimit;
            _shippingByWeightService.UpdateShippingByWeightRecord(sbw);

            ViewBag.RefreshPage = true;
            ViewBag.btnId = btnId;
            ViewBag.formId = formId;

            return View("~/Plugins/Shipping.ByWeight/Views/ShippingByWeight/EditPopup.cshtml", model);
        }
    }
}
