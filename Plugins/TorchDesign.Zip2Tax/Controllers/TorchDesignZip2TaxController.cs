using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Nop.Core;
using Nop.Plugin.TorchDesign.Zip2Tax.Services;
using Nop.Core.Domain.Directory;
using Nop.Plugin.TorchDesign.Zip2Tax.Domain;
using Nop.Plugin.TorchDesign.Zip2Tax.Models;
using Nop.Services.Configuration;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Kendoui;
using Nop.Web.Framework.Mvc;
using Nop.Plugin.TorchDesign.Zip2Tax;
using Nop.Services.Stores;
using System.Xml;
using System.IO;
using System.Net;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using Nop.Services.Helpers;

namespace Nop.Plugin.TorchDesign.Zip2Tax.Controllers
{

    public partial class TorchDesignZip2TaxController : BasePluginController
    {
        #region Fields
        private readonly IWorkContext _workContext;
        private readonly IZip2TaxService _zip2textService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly ISettingService _settingService;
        private readonly IStoreService _storeService;
        private readonly IStoreContext _storeContext;
        private readonly TorchDesignZip2TaxSettings _zipsetting;
        private readonly IDateTimeHelper _dateTimeHelper;

        #endregion

        public TorchDesignZip2TaxController(IZip2TaxService zip2textService, ILanguageService languageService,
            ILocalizationService localizationService,
            ILocalizedEntityService localizedEntityService,
             IWorkContext workContext,
             ISettingService settingService, IStoreService storeService, IStoreContext storeContext, TorchDesignZip2TaxSettings zipsetting, IDateTimeHelper dateTimeHelper
            )
        {

            this._languageService = languageService;
            this._zip2textService = zip2textService;
            this._localizationService = localizationService;
            this._localizedEntityService = localizedEntityService;
            this._workContext = workContext;
            this._settingService = settingService;
            this._storeService = storeService;
            this._storeContext = storeContext;
            this._zipsetting = zipsetting;
            this._dateTimeHelper = dateTimeHelper;

        }

        #region Methods
        #region Configure

        [AdminAuthorize]
        public ActionResult Configure()
        {
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var Zipsetting = _settingService.LoadSetting<TorchDesignZip2TaxSettings>(storeScope);
            var model = new ConfigurationModel();
            model.DatabaseName = Zipsetting.DatabaseName;
            model.DatabasePassword = Zipsetting.DatabasePassword;
            model.UserName = Zipsetting.UserName;
            model.Password = Zipsetting.Password;
            model.DatabaseServer = Zipsetting.DatabaseServer;
            model.DatabaseUserName = Zipsetting.DatabaseUserName;
            model.Zip2TaxApiUrl = Zipsetting.Zip2TaxApiUrl;
            model.UpdateTimeStemp = Zipsetting.UpdateTimeStemp;
            model.DefaultTaxRate = Zipsetting.DefaultTaxRate;
            model.DefaultZipcode = Zipsetting.DefaultZipcode;
            model.AllowedStateIds = Zipsetting.AllowedStateIds;

            if (storeScope > 0)
            {
                model.DatabaseName_OverrideForStore = _settingService.SettingExists(Zipsetting, x => x.DatabaseName, storeScope);
                model.DatabasePassword_OverrideForStore = _settingService.SettingExists(Zipsetting, x => x.DatabasePassword, storeScope);
                model.UserName_OverrideForStore = _settingService.SettingExists(Zipsetting, x => x.UserName, storeScope);
                model.Password_OverrideForStore = _settingService.SettingExists(Zipsetting, x => x.Password, storeScope);
                model.DatabaseServer_OverrideForStore = _settingService.SettingExists(Zipsetting, x => x.DatabaseServer, storeScope);
                model.DatabaseUserName_OverrideForStore = _settingService.SettingExists(Zipsetting, x => x.DatabaseUserName, storeScope);
                model.Zip2TaxApiUrl_OverrideForStore = _settingService.SettingExists(Zipsetting, x => x.Zip2TaxApiUrl, storeScope);
                model.UpdateTimeStemp_OverrideForStore = _settingService.SettingExists(Zipsetting, x => x.UpdateTimeStemp, storeScope);
                model.DefaultZipcode_OverrideForStore = _settingService.SettingExists(Zipsetting, x => x.DefaultZipcode, storeScope);
                model.DefaultTaxRate_OverrideForStore = _settingService.SettingExists(Zipsetting, x => x.DefaultTaxRate, storeScope);
                model.AllowedStateIds_OverrideForStore = _settingService.SettingExists(Zipsetting, x => x.AllowedStateIds, storeScope);
            }
            model.IsTaxFound = false;

            return View("~/Plugins/TorchDesign.Zip2Tax/Views/TorchDesignZip2Tax/Configure.cshtml", model);
        }

        [HttpPost, ActionName("Configure")]
        [FormValueRequired("Getlivetax")]
        public ActionResult Searchtax(ConfigurationModel model)
        {
            //bool isZipvalid = IsUsZipCode(model.ZipcodeText);
            if (!String.IsNullOrEmpty(model.ZipcodeText))
            {
                model.Msglable = (_zip2textService.GetAvailableTaxDetailByZipcode(Zipcode: model.ZipcodeText,isFromConfigure:true)).ToString();
                model.IsTaxFound = true;
            }
            else
            {
                ModelState.AddModelError("", "Allow only Us Zipcode Enter Valid Data ");
                model.IsTaxFound = false;
            }

            return View("~/Plugins/TorchDesign.Zip2Tax/Views/TorchDesignZip2Tax/Configure.cshtml", model);
        }

        [HttpPost, ActionName("Configure")]
        [FormValueRequired("Savesettingbtn")]
        public ActionResult Savesetting(ConfigurationModel model)
        {
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var Zipsetting = _settingService.LoadSetting<TorchDesignZip2TaxSettings>(storeScope);
            Zipsetting.DatabaseName = model.DatabaseName;
            Zipsetting.DatabasePassword = model.DatabasePassword;
            Zipsetting.DatabaseServer = model.DatabaseServer;
            Zipsetting.UserName = model.UserName;
            Zipsetting.Password = model.Password;
            Zipsetting.DatabaseUserName = model.DatabaseUserName;
            Zipsetting.Zip2TaxApiUrl = model.Zip2TaxApiUrl;
            Zipsetting.UpdateTimeStemp = model.UpdateTimeStemp;
            Zipsetting.DefaultTaxRate = model.DefaultTaxRate;
            Zipsetting.DefaultZipcode = model.DefaultZipcode;
            Zipsetting.AllowedStateIds = model.AllowedStateIds;

            if (model.DatabaseName_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(Zipsetting, x => x.DatabaseName, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(Zipsetting, x => x.DatabaseName, storeScope);

            if (model.DatabasePassword_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(Zipsetting, x => x.DatabasePassword, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(Zipsetting, x => x.DatabasePassword, storeScope);

            if (model.UserName_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(Zipsetting, x => x.UserName, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(Zipsetting, x => x.UserName, storeScope);

            if (model.Password_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(Zipsetting, x => x.Password, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(Zipsetting, x => x.Password, storeScope);

            if (model.DatabaseServer_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(Zipsetting, x => x.DatabaseServer, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(Zipsetting, x => x.DatabaseServer, storeScope);

            if (model.DatabaseUserName_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(Zipsetting, x => x.DatabaseUserName, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(Zipsetting, x => x.DatabaseUserName, storeScope);

            if (model.Zip2TaxApiUrl_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(Zipsetting, x => x.Zip2TaxApiUrl, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(Zipsetting, x => x.Zip2TaxApiUrl, storeScope);

            if (model.UpdateTimeStemp_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(Zipsetting, x => x.UpdateTimeStemp, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(Zipsetting, x => x.UpdateTimeStemp, storeScope);

            if (model.DefaultTaxRate_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(Zipsetting, x => x.DefaultTaxRate, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(Zipsetting, x => x.DefaultTaxRate, storeScope);

            if (model.DefaultZipcode_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(Zipsetting, x => x.DefaultZipcode, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(Zipsetting, x => x.DefaultZipcode, storeScope);

            if (model.AllowedStateIds_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(Zipsetting, x => x.AllowedStateIds, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(Zipsetting, x => x.AllowedStateIds, storeScope);

            _settingService.ClearCache();

            return View("~/Plugins/TorchDesign.Zip2Tax/Views/TorchDesignZip2Tax/Configure.cshtml", model);
        }


        [HttpPost]
        public ActionResult TaxList(DataSourceRequest command, ConfigurationModel model)
        {
            var zip2texs = _zip2textService.GetAllZip2Tax(command.Page - 1, command.PageSize);
            var zip2texModel = zip2texs.Select(x =>
            {

                var m = new Zip2Taxtlist()
                {
                    Id = x.Id,
                    Taxcategoryid = x.Taxcategoryid,
                    Zipcode = x.Zipcode,
                    TaxRate = x.TaxRate,
                    County = x.County,
                    CreateOn = _dateTimeHelper.ConvertToUserTime(x.CreateOn, DateTimeKind.Utc),
                    ModifyOn = _dateTimeHelper.ConvertToUserTime(x.ModifyOn, DateTimeKind.Utc)

                };
                return m;
            })
              .ToList();
            var gridModel = new DataSourceResult
            {
                Data = zip2texModel,
                Total = zip2texs.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        public ActionResult TaxUpdate(ConfigurationModel model)
        {

            //var updateSlider = _zip2taxService.GetSliderById(model.Id);
            //if (updateSlider == null)
            //    throw new ArgumentException("No Slider found with the specified id");


            ////if (string.IsNullOrEmpty(model.Title) || string.IsNullOrEmpty(model.Description) || model.StartDate.ToString() == "" || model.EndDate.ToString() == "")
            ////{
            ////    ErrorNotification("Please fill all the field.");
            ////    return new NullJsonResult();
            ////}

            //var sliderRecord = _zip2taxService.GetSliderById(model.Id);
            //sliderRecord.Title = model.Title;
            //sliderRecord.ShortDescription = model.ShortDescription;

            //_zip2taxService.UpdateSlider(sliderRecord);
            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult TaxDelete(int id)
        {
            var deleteZip = _zip2textService.GetZip2TaxById(id);
            if (deleteZip == null)
                throw new ArgumentException("No Zipcode Recode found with the specified id");
            _zip2textService.DeleteZip2Tax(deleteZip);
            return new NullJsonResult();
        }

        //public bool IsUsZipCode(string zipCode)
        //{
        //    string _usZipRegEx = @"^\d{5}(?:[-\s]\d{4})?$";
        //    bool validZipCode = true;
        //    if ((!Regex.Match(zipCode, _usZipRegEx).Success))
        //    {
        //        validZipCode = false;
        //    }
        //    return validZipCode;
        //}



        #endregion

        #region Counter

        [AdminAuthorize]
        //   [ChildActionOnly]
        public ActionResult Counter()
        {
            CounterModel model = new CounterModel();
            return View("~/Plugins/TorchDesign.Zip2Tax/Views/TorchDesignZip2Tax/Counter.cshtml", model);
        }
        [HttpPost]
        public ActionResult Counterlist(DataSourceRequest command, CounterModel model)
        {
            DateTime? startDateValue = (model.StartDate == null) ? null
                                      : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.StartDate.Value, _dateTimeHelper.CurrentTimeZone);

            DateTime? endDateValue = (model.EndDate == null) ? null
                            : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.EndDate.Value, _dateTimeHelper.CurrentTimeZone).AddDays(1);

            var calllist = _zip2textService.SearchCall(createdFromUtc: startDateValue, createdToUtc: endDateValue, pageIndex: command.Page - 1, pageSize: command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = calllist.Select(x =>
                {
                    return new Zip2TaxCountertlist()
                    {
                        Id = x.Id,
                        Zipcode = x.Zipcode,
                        TaxRate = x.TaxRate,
                        CallDate = _dateTimeHelper.ConvertToUserTime(x.CallDate, DateTimeKind.Utc)
                    };
                }),
                Total = calllist.TotalCount
            };
            //summary report
            var TotalCount = calllist.TotalCount;
            gridModel.ExtraData = new CalltotalModel()
            {

                Totalcall = TotalCount.ToString(),

            };



            return new JsonResult
            {
                Data = gridModel
            };
        }
        #endregion

        #endregion

    }
}
