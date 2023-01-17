using Nop.Core.Plugins;
using Nop.Plugin.TorchDesign.FAQ.Data;
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
using Nop.Plugin.TorchDesign.FAQ;
using Nop.Core.Domain.Customers;


namespace Nop.Plugin.TorchDesign.FAQ
{
    public class FAQPlugin : BasePlugin, IAdminMenuPlugin, IMiscPlugin
    {
        private readonly FrequentlyAskedQObjectContext _objectContext;
        private readonly ISettingService _settingService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
    

        public FAQPlugin(FrequentlyAskedQObjectContext objectContext,
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
                Title = "FAQ",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "area", null } },
            };


            var menuFirstItem = new SiteMapNode()
            {
                Title = "Configure",
                ControllerName = "Admin/Plugin/",
                ActionName = "ConfigureMiscPlugin",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "systemName", "Widgets.FAQ" }, { "area", null } },
            };
            menuItem.ChildNodes.Add(menuFirstItem);
            return menuItem;
        }


        #endregion

        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "FrequentlyAskedQ";
            routeValues = new RouteValueDictionary() { { "Namespaces", "Nop.Plugin.TorchDesign.FAQ.Controllers" }, { "area", null } };
        }

        public IList<string> GetWidgetZones()
        {
            return new List<string>() { "Faq" };
            //return new List<string>() { "content_before" };

        }

        #region Methods

        public override void Install()
        {
            _objectContext.Install();

            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.FAQ.Title", "Title");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.FAQ.Category", "Category");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.FAQ.Question", "Question");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.FAQ.Description", "Description");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.FAQ.DisplayOption", "Display Option");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.FAQ.DisplayOn", "Display On");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.FAQ.AddFAQ", "Add FAQ");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.FAQ.AddCategory", "Add Category");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.FAQ.DisplayAsWidget", "Display As Widget");
            this.AddOrUpdatePluginLocaleResource("plugins.widgets.faq.question.hint", "Enter faq question");
            this.AddOrUpdatePluginLocaleResource("plugins.torchdesign.customerorigin.isactive.hint", "Check to set it as active");
            this.AddOrUpdatePluginLocaleResource("plugins.widgets.event.displayorder", "Display order");
            this.AddOrUpdatePluginLocaleResource("plugins.widgets.faq.description.hint", "Enter faq description");
            base.Install();
        }

        public override void Uninstall()
        {
            _objectContext.Uninstall();

            this.DeletePluginLocaleResource("Plugins.Widgets.FAQ.Title");
            this.DeletePluginLocaleResource("Plugins.Widgets.FAQ.Category");
            this.DeletePluginLocaleResource("Plugins.Widgets.FAQ.Question");
            this.DeletePluginLocaleResource("Plugins.Widgets.FAQ.Description");
            this.DeletePluginLocaleResource("Plugins.Widgets.FAQ.DisplayOption");
            this.DeletePluginLocaleResource("Plugins.Widgets.FAQ.DisplayOn");
            this.DeletePluginLocaleResource("Plugins.Widgets.FAQ.AddFAQ");
            this.DeletePluginLocaleResource("Plugins.Widgets.FAQ.AddCategory");
            this.DeletePluginLocaleResource("Plugins.Widgets.FAQ.DisplayAsWidget");
            this.DeletePluginLocaleResource("plugins.widgets.faq.question.hint");
            this.DeletePluginLocaleResource("plugins.torchdesign.customerorigin.isactive.hint");
            this.DeletePluginLocaleResource("plugins.widgets.event.displayorder");
            this.DeletePluginLocaleResource("plugins.widgets.faq.description.hint");
            base.Uninstall();
        }

        #endregion


    }
}
