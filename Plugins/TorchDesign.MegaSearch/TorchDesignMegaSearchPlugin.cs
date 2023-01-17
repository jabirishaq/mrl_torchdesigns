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
using Nop.Plugin.TorchDesign.MegaSearch;
using Nop.Core.Domain.Customers;
using Nop.Plugin.TorchDesign.MegaSearch.Data;
using Nop.Core.Caching;



namespace Nop.Plugin.TorchDesign.MegaSearc
{
    public class StoreLocatorPlugin : BasePlugin, IAdminMenuPlugin, IWidgetPlugin
    {
        private readonly MegaSearchObjctContext _objectContext;
        private readonly ISettingService _settingService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly TorchDesignMegaSearchSettings _megasearchsetting;
        private readonly IStoreContext _storeContext;
        private readonly ICacheManager _cacheManager;


        public StoreLocatorPlugin(MegaSearchObjctContext objectContext,
             ISettingService settingService, IStoreService storeService, IWorkContext workContext, TorchDesignMegaSearchSettings megasearchsetting, IStoreContext storeContext, ICacheManager cacheManager)
        {

            this._objectContext = objectContext;
            this._settingService = settingService;
            this._storeService = storeService;
            this._workContext = workContext;
            this._megasearchsetting = megasearchsetting;
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
                Title = "MegaSearch",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "area", null } },
            };

            var menuFirstItem = new SiteMapNode()
            {
                Title = "Configure",
                ControllerName = "Admin/Widget/",
                ActionName = "ConfigureWidget",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "systemName", "TorchDesignMegaSearch" }, { "area", null } },
            };
            menuItem.ChildNodes.Add(menuFirstItem);
            return menuItem;
        }


        #endregion

        #region Methods

        public IList<string> GetWidgetZones()
        {
            return new List<string>() { "header" };
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
            actionName = "SearchPublicInfo";
            controllerName = "TorchDesignMegaSearch";
            routeValues = new RouteValueDictionary()
            {
                {"Namespaces", "Nop.Plugin.Widgets.Slider.Controllers"},
                {"area", null},
                {"widgetZone", widgetZone}
            };
        }

        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "TorchDesignMegaSearch";
            routeValues = new RouteValueDictionary() { { "Namespaces", "Nop.Plugin.TorchDesign.MegaSearch.Controllers" }, { "area", null } };
        }
        #endregion

        public override void Install()
        {
            var settings = new TorchDesignMegaSearchSettings()
            {
                ProductSearch = true,
                ByProductDescription = true,
                ByProductSku = true,
                ByProductTag = true,
                ByProductPartno=true,
                ByVideoTag=true,
                CategorySearch = false,
                ByCategoryDescription = false, 
                ManufacturerSearch = false,
                ByManufacturerDescription = false,
                BlogPostSearch = true,
                ByBlogPostDescription = true,
                NewsSearch=true,
                ByNewsDescription=true

            };
            _settingService.SaveSetting(settings);
            _objectContext.Install();
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.MegaSearch.SearchFromProduct", "Search From Product");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.MegaSearch.SearchProductbydescription", "Search Product By Description");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.MegaSearch.SearchProductbysku", "Search Product By Sku");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.MegaSearch.SearchProductbytag", "Search Product By Tag");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.MegaSearch.SearchProductVideobytag", "Search Product By Video Tag,Video Title And Description ");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.MegaSearch.SearchFromCategory", "Search From Category");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.MegaSearch.SearchCategorybydescription", "Search Category By Description");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.MegaSearch.SearchFromManufacturer", "Search From Manufacturer");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.MegaSearch.SearchManufacturerbydescription", "Search Manufacturer By Description");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.MegaSearch.SearchFromBlogPostSearch", "Search From Blog");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.MegaSearch.SearchBlogPostSearchbydescription", "Search Blog By Description");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.MegaSearch.SearchFromNewsSearch", "Search From NewsSearch");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.MegaSearch.SearchNewsSearchbydescription", "Search News By Description");
            this.AddOrUpdatePluginLocaleResource("Searchbox.placeholder.text", "Search Whole Site");
            this.AddOrUpdatePluginLocaleResource("megasearch.news.Producttext", "Search Keyword Related Product List");
            this.AddOrUpdatePluginLocaleResource("megasearch.news.Categorytext", "Search Keyword Related Category List");
            this.AddOrUpdatePluginLocaleResource("megasearch.news.Menufacturertext", "Search Keyword Related Manufacturer List");
            this.AddOrUpdatePluginLocaleResource("megasearch.news.Blogtext", "Search Keyword Related Blog List");
            this.AddOrUpdatePluginLocaleResource("megasearch.news.headertext", "Search Keyword Related News List");
            this.AddOrUpdatePluginLocaleResource("megasearch.resultnotfoud.text", "Sorry, we did not find any results that matched your search query.");
            this.AddOrUpdatePluginLocaleResource("Product.list", "Product");
            this.AddOrUpdatePluginLocaleResource("other.list", "Other");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.MegaSearch.SearchProductbyproductnumber", "Search Product By Partno");
            this.AddOrUpdatePluginLocaleResource("Plugins.TorchDesign.MegaSearch.SelectOptionFromWhichYouWantToSearch", "Select Option From Which You Want To Search");
            base.Install();
        }

        public override void Uninstall()
        {
            //_objectContext.Uninstall();
            this.DeletePluginLocaleResource("Plugins.TorchDesign.MegaSearch.SearchFromProduct");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.MegaSearch.SearchProductbydescription");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.MegaSearch.SearchProductbysku");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.MegaSearch.SearchProductbytag");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.MegaSearch.SearchProductVideobytag");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.MegaSearch.SearchFromCategory");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.MegaSearch.SearchCategorybydescription");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.MegaSearch.SearchFromManufacturer");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.MegaSearch.SearchManufacturerbydescription");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.MegaSearch.SearchFromBlogPostSearch");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.MegaSearch.SearchBlogPostSearchbydescription");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.MegaSearch.SearchFromNewsSearch");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.MegaSearch.SearchNewsSearchbydescription");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.MegaSearch.SelectOptionFromWhichYouWantToSearch");
            this.DeletePluginLocaleResource("Searchbox.placeholder.text");
            this.DeletePluginLocaleResource("megasearch.news.Producttext");
            this.DeletePluginLocaleResource("megasearch.news.Menufacturertext");
            this.DeletePluginLocaleResource("megasearch.news.Categorytext");
            this.DeletePluginLocaleResource("megasearch.news.Blogtext");
            this.DeletePluginLocaleResource("megasearch.news.headertext");
            this.DeletePluginLocaleResource("megasearch.resultnotfoud.text");
            this.DeletePluginLocaleResource("Product.list");
            this.DeletePluginLocaleResource("other.list");
            this.DeletePluginLocaleResource("Plugins.TorchDesign.MegaSearch.SearchProductbyproductnumber");

            base.Uninstall();
        }




        //public void GetDisplayWidgetRoute(string widgetZone, out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
