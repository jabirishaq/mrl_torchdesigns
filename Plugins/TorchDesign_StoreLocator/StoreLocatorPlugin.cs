using Nop.Core.Plugins;
using Nop.Plugin.Widgets.TorchDesign_StoreLocator.Data;
using Nop.Services.Common;
using Nop.Web.Framework.Menu;
using Nop.Web.Framework.Web;
using System.Web.Routing;
using Nop.Services.Localization;
using Nop.Services.Configuration;
using Nop.Services.Stores;
using Nop.Core;
using Nop.Core.Domain.Customers;


namespace Nop.Plugin.Widgets.TorchDesign_StoreLocator
{
    public class StoreLocatorPlugin : BasePlugin,IAdminMenuPlugin,IMiscPlugin
    {
        private readonly StoreLocatorObjctContext _objectContext;
        private readonly ISettingService _settingService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;


        public StoreLocatorPlugin(StoreLocatorObjctContext objectContext,
             ISettingService settingService, IStoreService storeService, IWorkContext workContext)
        {

            this._objectContext = objectContext;
            this._settingService = settingService;
            this._storeService = storeService;
            this._workContext = workContext;

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
                Title = "Store Locator",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "area", null } },
            };

            var menuFirstItem = new SiteMapNode()
            {
                Title = "Configure",
                ControllerName = "Admin/Plugin/",
                ActionName = "ConfigureMiscPlugin",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "systemName", "TorchDesign_StoreLocator" }, { "area", null } },
            };
            menuItem.ChildNodes.Add(menuFirstItem);
            return menuItem;
        }


        #endregion

        #region Methods
        public virtual int GetActiveStoreScopeConfiguration(IStoreService storeService, IWorkContext workContext)
        {
            //ensure that we have 2 (or more) stores
            if (storeService.GetAllStores().Count < 2)
                return 0;


            var storeId = workContext.CurrentCustomer.GetAttribute<int>(SystemCustomerAttributeNames.AdminAreaStoreScopeConfiguration);
            var store = storeService.GetStoreById(storeId);
            return store != null ? store.Id : 0;
        }



        //public IList<string> GetWidgetZones()
        //{

        //    var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
        //    var FAQSettings = _settingService.LoadSetting<StoreLocatorSettings>(storeScope);


        //    return !string.IsNullOrWhiteSpace(FAQSettings.DisplayOn)
        //           ? new List<string>() { FAQSettings.DisplayOn }
        //           : new List<string>() { "home_page_bottom" };



        //}

        //public IList<string> GetWidgetZones()
        //{
        //    return new List<string>() { "home_page_top" };
        //    //return new List<string>() { "content_before" };

        //}

        /// <summary>
        /// Gets a route for displaying widget
        /// </summary>
        /// <param name="widgetZone">Widget zone where it's displayed</param>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>

        //public void GetDisplayWidgetRoute(string widgetZone, out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        //{
        //    actionName = "PublicInfo";
        //    controllerName = "TorchDesign_StoreLocator";
        //    routeValues = new RouteValueDictionary()
        //    {
        //        {"Namespaces", "Nop.Plugin.Widgets.TorchDesign_StoreLocator.Controllers"},
        //        {"area", null},
        //        {"widgetZone", widgetZone}
        //    };
        //}

        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "TorchDesign_StoreLocator";
            routeValues = new RouteValueDictionary() { { "Namespaces", "Nop.Plugin.Widgets.TorchDesign_StoreLocator.Controllers" }, { "area", null } };
        }



        public override void Install()
        {
            var settings = new StoreLocatorSettings()
            {
                Description = "Since 1992, Mister Landscaper has partnered with Lowe's to bring you the highest quality low-volume irrigation products available. We are proud of our long history with Lowe's and look forward to the years ahead as we continue to produce and make available the finest micro spray and drip irrigation on the market. And while we are not in ALL Lowe’s, we can currently be found in the ‘Rough Plumbing’ aisle of more than 1000 stores. Use the search tool below to see if there is a location near you offering Mister Landscaper.",
             
            };
            _settingService.SaveSetting(settings);

            _objectContext.Install();
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Storelocator.addstore", "Add New Store Location");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Storelocator.StoreNumber", "Store Number");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Storelocator.StoreName", "Store Name");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Storelocator.Address", "Address");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Storelocator.City", "City");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Storelocator.Region", "Region");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Storelocator.CountryCode", "Country Code");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Storelocator.PostelCode", "Postel Code");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Storelocator.PhoneNumber", "Phone Number");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Storelocator.StoreType", "Store Type");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Storelocator.Latitude", "Latitude");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Storelocator.Longitude", "Longitude");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Storelocator.header", "Enter your address, city, or postal code to find stores near you!");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Storelocator.youraddress", "Location");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Storelocator.EnterRadius", "Enter Radius");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Storelocator.Miles", "miles");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Storelocator.Didyoumean", "Did you mean");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Storelocator.AvailableStoreslist", "Available Stores");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Storelocator.Distance", "Distance");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Storelocator.StoreId", "Store Id");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Storelocator.storenotfound", "Unfortunately, we were not able to find a store within 100 miles of your location.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Storelocator.Store", "Stores");
            this.AddOrUpdatePluginLocaleResource("placeholdertext", "Address, city, or Zip code");
            this.AddOrUpdatePluginLocaleResource("storelocator.title", "Lowe’s Store Locator");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Storelocator.Description", "Store Locatore Homepage Description");
            base.Install();
        }

        public override void Uninstall()
        {
            //_objectContext.Uninstall();
            this.DeletePluginLocaleResource("Plugins.Widgets.Storelocator.addstore");
            this.DeletePluginLocaleResource("Plugins.Widgets.Storelocator.StoreNumber");
            this.DeletePluginLocaleResource("Plugins.Widgets.Storelocator.StoreName");
            this.DeletePluginLocaleResource("Plugins.Widgets.Storelocator.Address");
            this.DeletePluginLocaleResource("Plugins.Widgets.Storelocator.City");
            this.DeletePluginLocaleResource("Plugins.Widgets.Storelocator.Region");
            this.DeletePluginLocaleResource("Plugins.Widgets.Storelocator.CountryCode");
            this.DeletePluginLocaleResource("Plugins.Widgets.Storelocator.PostelCode");
            this.DeletePluginLocaleResource("Plugins.Widgets.Storelocator.PhoneNumber");
            this.DeletePluginLocaleResource("Plugins.Widgets.Storelocator.StoreType");
            this.DeletePluginLocaleResource("Plugins.Widgets.Storelocator.Latitude");
            this.DeletePluginLocaleResource("Plugins.Widgets.Storelocator.Longitude");
            this.DeletePluginLocaleResource("Plugins.Widgets.Storelocator.header");
            this.DeletePluginLocaleResource("Plugins.Widgets.Storelocator.youraddress");
            this.DeletePluginLocaleResource("Plugins.Widgets.Storelocator.EnterRadius");
            this.DeletePluginLocaleResource("Plugins.Widgets.Storelocator.Miles");
            this.DeletePluginLocaleResource("Plugins.Widgets.Storelocator.Didyoumean");
            this.DeletePluginLocaleResource("Plugins.Widgets.Storelocator.AvailableStoreslist");
            this.DeletePluginLocaleResource("Plugins.Widgets.Storelocator.Distance");
            this.DeletePluginLocaleResource("Plugins.Widgets.Storelocator.StoreId");
            this.DeletePluginLocaleResource("Plugins.Widgets.Storelocator.storenotfound");
            this.DeletePluginLocaleResource("Plugins.Widgets.Storelocator.Store");
            this.DeletePluginLocaleResource("placeholdertext");
            this.DeletePluginLocaleResource("storelocator.title");
            this.DeletePluginLocaleResource("Plugins.Widgets.Storelocator.Description");
         
            base.Uninstall();
        }

        #endregion




        //public void GetDisplayWidgetRoute(string widgetZone, out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
