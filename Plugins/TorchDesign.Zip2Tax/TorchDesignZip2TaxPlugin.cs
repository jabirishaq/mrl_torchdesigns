using Nop.Core.Plugins;
using Nop.Plugin.TorchDesign.Zip2Tax.Data;
using Nop.Services.Common;
using Nop.Web.Framework.Menu;
using Nop.Web.Framework.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;
using Nop.Services.Localization;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Stores;
using Nop.Core;
using Nop.Web.Framework.Controllers;
using Nop.Core.Domain.Customers;
using Nop.Core.Caching;
using Nop.Services.Tax;
using Nop.Plugin.TorchDesign.Zip2Tax.Services;
using System.Net;
using System.Xml.Linq;
using Nop.Plugin.TorchDesign.Zip2Tax.Models;
using Nop.Plugin.TorchDesign.Zip2Tax.Domain;
using System.Text.RegularExpressions;

namespace Nop.Plugin.TorchDesign.Zip2Tax
{
    public class TorchDesignZip2TaxPlugin : BasePlugin, ITaxProvider, IAdminMenuPlugin, IMiscPlugin
    {
        private readonly IStoreContext _storeContext;
        private readonly ICacheManager _cacheManager;
        private readonly Zip2TaxObjctContext _objectContext;
        private readonly ISettingService _settingService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly IZip2TaxService _zip2taxService;
        private readonly TorchDesignZip2TaxSettings _zipsetting;

        public TorchDesignZip2TaxPlugin(IStoreContext storeContext, ICacheManager cacheManager, Zip2TaxObjctContext objectContext,
             ISettingService settingService, IStoreService storeService, IWorkContext workContext, IZip2TaxService zip2taxService,
            TorchDesignZip2TaxSettings zipsetting)
        {
            this._cacheManager = cacheManager;
            this._storeContext = storeContext;
            this._objectContext = objectContext;
            this._settingService = settingService;
            this._storeService = storeService;
            this._workContext = workContext;
            this._zip2taxService = zip2taxService;
            this._zipsetting = zipsetting;
        }

        #region Methods

        public string GetCounty(string zipCode)
        {
            return _zip2taxService.GetCounty(zipCode);
        }

        public string GetCountyNameUsingZip2TaxIfNull(string zipCode)
        {
            return _zip2taxService.GetCountyNameUsingZip2Tax(zipCode);
        }

        public CalculateTaxResult GetTaxRate(CalculateTaxRequest calculateTaxRequest)
        {
            string CurrentZipcode;
            decimal CurrentTaxrate;
            var result = new CalculateTaxResult();
            if (calculateTaxRequest.Address != null)
            {
                if (calculateTaxRequest.Address.Country.ThreeLetterIsoCode != "USA")
                {
                    result.TaxRate = 0;
                    return result;
                }
            }
            if (calculateTaxRequest.Address == null)
            {
                CurrentZipcode = _zipsetting.DefaultZipcode;
            }
            else
            {
                CurrentZipcode = calculateTaxRequest.Address.ZipPostalCode;
            }
            var stateCode = calculateTaxRequest.Address.StateProvince != null ? calculateTaxRequest.Address.StateProvince.Abbreviation.ToLower() :null;
            CurrentTaxrate = _zip2taxService.GetAvailableTaxDetailByZipcode(CurrentZipcode, stateCode);
            result.TaxRate = CurrentTaxrate;

            return result;
        }



        //public bool IsUsaZipCode(string zipCode)
        //{
        //    string _usZipRegEx = @"^\d{5}(?:[-\s]\d{4})?$";
        //    bool validZipCode = true;
        //    if ((!Regex.Match(zipCode, _usZipRegEx).Success))
        //    {
        //        validZipCode = false;
        //    }
        //    return validZipCode;
        //}

        public bool Authenticate()
        {
            return true;
        }
        public SiteMapNode BuildMenuItem()
        {
            var menuItem = new SiteMapNode()
            {
                Title = "Get Tax Detail",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "area", null } },
            };


            var menuFirstItem = new SiteMapNode()
            {
                Title = "Configure",
                ControllerName = "Admin/Plugin/",
                ActionName = "ConfigureMiscPlugin",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "systemName", "TorchDesignZip2Tax" }, { "area", null } },
            };
            menuItem.ChildNodes.Add(menuFirstItem);
            return menuItem;
        }

        /// <summary>
        /// Gets a route for displaying widget
        /// </summary>
        /// <param name="widgetZone">Widget zone where it's displayed</param>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>

        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "TorchDesignZip2Tax";
            routeValues = new RouteValueDictionary() { { "Namespaces", "Nop.Plugin.TorchDesign.Zip2Tax.Controllers" }, { "area", null } };
        }

        public override void Install()
        {
            var settings = new TorchDesignZip2TaxSettings()
            {
                DatabaseServer = "",
                DatabaseName = "",
                DatabasePassword = "",
                Password = "",
                UserName = "",
                DatabaseUserName = "",
                UpdateTimeStemp = 30,
                Zip2TaxApiUrl = "https://api.zip2tax.com/TaxRate-USA.xml",
                DefaultTaxRate = 6,
                DefaultZipcode = "33838",
                AllowedStateIds = "FL"

            };
            _settingService.SaveSetting(settings);


            _objectContext.Install();


            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.Taxcategoryid", "Taxcategoryid");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.Zipcode", "Zipcode");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.ZipcodeText", "Enter Zipcode To Get Tax");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.TaxRate", "TaxRate");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.CreateOn", "CreateOn");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.ModifyOn", "ModifyOn");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.CallDate", "Call Date");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.DatabaseServer", "Zip2Tax Database Server");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.DatabaseUserName", "Database UserName");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.DatabasePassword", "Database Password");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.DatabaseName", "Database Name");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.UserName", "Zip2Tax UserName");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.Password", "Zip2Tax Password");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.UpdateTimeStamp", "Enter Day Where You Want to Update TaxRate");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.Zip2TaxApiUrl", "Enter Url Of Your TaxProvider Api");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.Taxprovidersetting", "Zip2Text Connection Setting");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.Searchtaxrate", "Find Tax Rate");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.ZipcodeText", "Enter Us Zipcode");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.DefaultZipcode", "Set Default Zipcode");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.DefaultTaxrate", "Set Default Tax Rate");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.Hour", "Hours");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.Counterheading", "Calculate Call");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.Counterbacklinl", "back to configration page");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.TotalCall", "Total Call");
            this.AddOrUpdatePluginLocaleResource("Show.callcount", "Show All Call History");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.Findcall", "Find Total Call Between Two Dates");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.Country", "Country");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.AllowedStateIds", "Allowed StateIds (Comma[,] seperated)");
            base.Install();
        }

        public override void Uninstall()
        {
            //_settingService.DeleteSetting<TorchDesignZip2TaxSettings>();
            _objectContext.Uninstall();

            this.DeletePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.Taxcategoryid");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.ZipcodeText");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.Zipcode");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.TaxRate");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.CreateOn");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.ModifyOn");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.CallDate");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.DatabaseServer");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.DatabaseUserName");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.DatabasePassword");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.DatabaseName");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.UserName");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.Password");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.UpdateTimeStamp");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.Zip2TaxApiUrl");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.Taxprovidersetting");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.Searchtaxrate");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.ZipcodeText");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.DefaultZipcode");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.DefaultTaxrate");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.Hour");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.Counterheading");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.Counterbacklinl");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.TotalCall");
            this.DeletePluginLocaleResource("Show.callcount");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.FindcallShow.callcount");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.Country");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.Zip2Tax.AllowedStateIds", "Allowed StateIds (Comma[,] seperated)");
            base.Uninstall();
        }

        #endregion

    }
}
