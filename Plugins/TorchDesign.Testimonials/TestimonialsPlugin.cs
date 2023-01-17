using Nop.Core.Plugins;
using Nop.Plugin.TorchDesign.Testimonials.Data;
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
using Nop.Plugin.TorchDesign.Testimonials;
using Nop.Core.Domain.Customers;


namespace Nop.Plugin.TorchDesign.Testimonials
{
    public class TestimonialsPlugin : BasePlugin, IAdminMenuPlugin, IMiscPlugin
    {
        private readonly TestimonialsObjctContext _objectContext;
        private readonly ISettingService _settingService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
    

        public TestimonialsPlugin(TestimonialsObjctContext objectContext,
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
                Title = "Testimonials",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "area", null } },
            };


            var menuFirstItem = new SiteMapNode()
            {
                Title = "Approval Queue",
                ControllerName = "Admin/Plugin/",
                ActionName = "ConfigureMiscPlugin",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "systemName", "Widgets.Testimonials" }, { "area", null } },
            };
            menuItem.ChildNodes.Add(menuFirstItem);
            return menuItem;
        }


        #endregion

        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "Testimonials";
            routeValues = new RouteValueDictionary() { { "Namespaces", "Nop.Plugin.TorchDesign.Testimonials.Controllers" }, { "area", null } };
        }

        public IList<string> GetWidgetZones()
        {
            return new List<string>() { "Testimonials" };
            //return new List<string>() { "content_before" };

        }

        #region Methods

        public override void Install()
        {
          
           
            _objectContext.Install();

            this.AddOrUpdatePluginLocaleResource("plugins.torchdesign.testimonial.customername", "Name:");
            this.AddOrUpdatePluginLocaleResource("plugins.torchdesign.testimonial.name", "Name");
            this.AddOrUpdatePluginLocaleResource("plugins.torchdesign.testimonial.email", "E-Mail Address");
            this.AddOrUpdatePluginLocaleResource("plugins.torchdesign.testimonial.email_address", "E-Mail Address:");

            this.AddOrUpdatePluginLocaleResource("plugins.torchdesign.testimonial.location", "Location:");
            this.AddOrUpdatePluginLocaleResource("plugins.torchdesign.testimonial.createon", "Date");
            this.AddOrUpdatePluginLocaleResource("plugins.torchdesign.testimonial.message", "Message");
            this.AddOrUpdatePluginLocaleResource("plugins.torchdesign.testimonial.from_message", "Message:");

            this.AddOrUpdatePluginLocaleResource("plugins.torchdesign.testimonial.isactive", "IS Approve");
            this.AddOrUpdatePluginLocaleResource("plugins.torchdesign.testimonial.isrejected", "Is Rejected");
            this.AddOrUpdatePluginLocaleResource("plugins.torchdesign.testimonial.rejected", "Rejected");
            this.AddOrUpdatePluginLocaleResource("plugins.torchdesign.testimonial.approve", "Approve");
            this.AddOrUpdatePluginLocaleResource("plugins.torchdesign.testimonial.approved", "Approved");
            this.AddOrUpdatePluginLocaleResource("plugins.torchdesign.testimonial.reject", "Reject");

            this.AddOrUpdatePluginLocaleResource("plugins.torchdesign.testimonial.back", "<- Back");
            this.AddOrUpdatePluginLocaleResource("plugins.torchdesign.testimonial.Action", "Action");
            this.AddOrUpdatePluginLocaleResource("Plugin.Fields.Message.Required", "Message is required");
            this.AddOrUpdatePluginLocaleResource("Plugin.Fields.Photos", "Photos:");;
            this.AddOrUpdatePluginLocaleResource("Common.FileUploader.UploadPhotos", "Upload Photos");
            base.Install();

        }

        public override void Uninstall()
        {
            _objectContext.Uninstall();

            this.DeletePluginLocaleResource("plugins.torchdesign.testimonial.customername");
            this.DeletePluginLocaleResource("plugins.torchdesign.testimonial.email");
            this.DeletePluginLocaleResource("plugins.torchdesign.testimonial.location");
            this.DeletePluginLocaleResource("plugins.torchdesign.testimonial.createon");
            this.DeletePluginLocaleResource("plugins.torchdesign.testimonial.message");
            this.DeletePluginLocaleResource("plugins.torchdesign.testimonial.isactive");
            this.DeletePluginLocaleResource("plugins.torchdesign.testimonial.isrejected");
            this.DeletePluginLocaleResource("plugins.torchdesign.testimonial.reject");
            this.DeletePluginLocaleResource("plugins.torchdesign.testimonial.approve");
            this.DeletePluginLocaleResource("plugins.torchdesign.testimonial.back");
            this.DeletePluginLocaleResource("plugins.torchdesign.testimonial.Action");
            this.DeletePluginLocaleResource("Plugin.Fields.Message.Required");
            this.DeletePluginLocaleResource("Plugin.Fields.Photos");
            this.DeletePluginLocaleResource("Common.FileUploader.UploadPhotos");
            this.DeletePluginLocaleResource("plugins.torchdesign.testimonial.reject");
            base.Uninstall();
        }

        #endregion


    }
}
