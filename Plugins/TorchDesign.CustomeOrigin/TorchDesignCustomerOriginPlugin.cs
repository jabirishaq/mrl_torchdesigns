using Nop.Core.Plugins;
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
using Nop.Plugin.TorchDesign.CustomerOrigin;
using Nop.Core.Domain.Customers;
using Nop.Core.Plugins;
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
using Nop.Plugin.TorchDesign.CustomerOrigin.Data;
using Nop.Core.Caching;

namespace Nop.Plugin.TorchDesign.CustomerOrigin
{
    public class TorchDesignCustomerOriginPlugin : BasePlugin, IAdminMenuPlugin, IWidgetPlugin
    {
        private readonly CustomerOriginObjctContext _objectContext;
        private readonly ISettingService _settingService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly TorchDesignCustomerOriginSettings _CustomerOrigin;
        private readonly IStoreContext _storeContext;
        private readonly ICacheManager _cacheManager;


        public TorchDesignCustomerOriginPlugin(CustomerOriginObjctContext objectContext,
             ISettingService settingService, IStoreService storeService, IWorkContext workContext, TorchDesignCustomerOriginSettings CustomerOrigin, IStoreContext storeContext, ICacheManager cacheManager)
        {

            this._objectContext = objectContext;
            this._settingService = settingService;
            this._storeService = storeService;
            this._workContext = workContext;
            this._CustomerOrigin = CustomerOrigin;
             this._storeContext = storeContext;
             this._cacheManager = cacheManager;

        }




        #region IAdminMenuPlugin Members

        public bool Authenticate()
        {
            return true;
        }

        public SiteMapNode BuildMenuItem()
        {
            var menuItem = new SiteMapNode()
            {
                Title = "Custome Origin",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "area", null } },
            };


            var menuFirstItem = new SiteMapNode()
            {
                Title = "Configure",
                ControllerName = "Admin/Widget/",
                ActionName = "ConfigureWidget",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "systemName", "TorchDesignCustomerOrigin" }, { "area", null } },
            };
            menuItem.ChildNodes.Add(menuFirstItem);
            return menuItem;
        }

        #endregion

        #region Methods


        public IList<string> GetWidgetZones()
        {
            return new List<string>() { "checkout_page_top" };
            //return new List<string>() { "content_before" };

        }

        /// <summary>
        /// Gets a route for displaying widget
        /// </summary>
        /// <param name="widgetZone">Widget zone where it's displayed</param>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public void GetDisplayWidgetRoute(string widgetZone, out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "PublicInfo";
            controllerName = "TorchDesignCustomerOrigin";
            routeValues = new RouteValueDictionary()
            {
                {"Namespaces", "Nop.Plugin.TorchDesign.CustomerOrigin.Controllers"},
                {"area", null},
                {"widgetZone", widgetZone}
            };
        }

        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "TorchDesignCustomerOrigin";
            routeValues = new RouteValueDictionary() { { "Namespaces", "Nop.Plugin.TorchDesign.CustomerOrigin.Controllers" }, { "area", null } };
        }

        #endregion

        public override void Install()
        {
            var settings = new TorchDesignCustomerOriginSettings()
            {
                
            };
            _settingService.SaveSetting(settings);
            _objectContext.Install();
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.Customerorigin.isdefaultselected", "Is Defaulr Selected");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.Customerorigin.isactive", "Is Active");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.Customerorigin.Option", "Option");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.Customerorigin.addoption", "Add New Option");
            this.AddOrUpdatePluginLocaleResource("Plugin.custom.origin.feedback.question", "How did you learn about Mister Landscaper?");
            this.AddOrUpdatePluginLocaleResource("plugin.custome.origin.feedbacktitle", "Customer Review Chart");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.Custome.FindReview", "Generate Chart Between Two Date");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.Custome.error", "Sorry!No Result Between This Dates");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.Customer.Title", "Review Chart");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.Customerorigin.Backlinktitle", "Go back To Configure Page");
            this.AddOrUpdatePluginLocaleResource("Show.Review", "Show Customer Feedback");
            base.Install();
        }

        public override void Uninstall()
        {
            //_objectContext.Uninstall();
            this.DeletePluginLocaleResource("Plugins.TorchDesign.Customerorigin.isdefaultselected");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.Customerorigin.isactive");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.Customerorigin.Option");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.Customerorigin.addoption");
            this.DeletePluginLocaleResource("Plugin.custom.origin.feedback.question");
            this.DeletePluginLocaleResource("plugin.custome.origin.feedbacktitle");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.Custome.FindReview");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.Custome.error");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.Customer.Title");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.Customerorigin.Backlinktitle");
            this.DeletePluginLocaleResource("Show.Review");
            base.Uninstall();
        }




        //public void GetDisplayWidgetRoute(string widgetZone, out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        //{
        //    throw new NotImplementedException();
        //}
    }
}

