using Nop.Core.Plugins;
using Nop.Plugin.Widgets.Slider.Data;
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
namespace Nop.Plugin.Widgets.Slider
{
    public class SliderPlugin : BasePlugin, IAdminMenuPlugin,IWidgetPlugin 
    {
        private readonly SliderObjectContext _objectContext;
        private readonly ISettingService _settingService;

        public SliderPlugin(SliderObjectContext objectContext, ISettingService settingService)
        {
            this._objectContext = objectContext;
            this._settingService = settingService;
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
                Title = "Slider",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "area", null } },
            };

            var menuFirstItem = new SiteMapNode()
            {
                Title = "Configure",
                ControllerName = "Admin/Widget/",
                ActionName = "ConfigureWidget",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "systemName", "Widgets.Slider" }, { "area", null } },
            };
            menuItem.ChildNodes.Add(menuFirstItem);
            return menuItem;
        }


        #endregion

        #region Methods

        public IList<string> GetWidgetZones()
        {
            return new List<string>() { "home_page_top" };
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
            controllerName = "WidgetsSlider";
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
            controllerName = "WidgetsSlider";
            routeValues = new RouteValueDictionary() { { "Namespaces", "Nop.Plugin.Widgets.Slider.Controllers" }, { "area", null } };
        }
        #endregion

        public override void Install()
        {

            //settings
            var settings = new SliderPluginSettings()
            {
                //Slidereffect = 1,
                Transitionspeed = 500,
               
            };
            _settingService.SaveSetting(settings);

            _objectContext.Install();
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Slider.transitionspeed", "Transition Speed");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Slider.Effect", "Slider Effect");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Slider.AddSlider", "Add Slider");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Slider.PictureId", "Picture");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Slider.Title", "Title");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Slider.ShortDescription", "Short Description");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Slider.Published", "Published");
            this.AddOrUpdatePluginLocaleResource("Plugin.Widget.Slider.Sliders", "Sliders");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Slider.Name", "Name");
            this.AddOrUpdatePluginLocaleResource("Plugins.widgets.slider.Displayoption", "Display Option");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Slider.Link", "Link");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Slider.Displayorder", "Display Order");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Slider.resolution", "Image size should be 782 x 219 ");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Slider.Transitionspeedinmili", "Milliseconds");
            this.AddOrUpdatePluginLocaleResource("plugins.widgets.event.displayorder", "Display Order");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Slider.PictureId.Hint","Upload picture for slider");
            this.AddOrUpdatePluginLocaleResource("plugins.widgets.slider.displayoption.hint", "Select display option");
            this.AddOrUpdatePluginLocaleResource("plugins.widgets.slider.title.hint", "Enter text to display on slide");
            this.AddOrUpdatePluginLocaleResource("plugins.widgets.slider.shortdescription.hint", "Enter short descreption for slide");
            this.AddOrUpdatePluginLocaleResource("plugins.widgets.slider.link.hint", "Enter url to link slider with specific page");
            this.AddOrUpdatePluginLocaleResource("plugins.widgets.slider.displayorder.hint", "Enter display order of slide");
            this.AddOrUpdatePluginLocaleResource("plugins.widgets.slider.transitionspeed.hint", "Enter transition speed in millisecond");
            this.AddOrUpdatePluginLocaleResource("plugins.widgets.slider.effect.hint", "Select slide effect");
            this.AddOrUpdatePluginLocaleResource("plugins.widgets.slider.published.hint", "Check to publish this slide (visible in store). Uncheck to unpublish (slide not available in store).");
            this.AddOrUpdatePluginLocaleResource("plugins.widgets.event.displayorder.hint", "Set display order");
            base.Install();
        }

        public override void Uninstall()
        {
            _settingService.DeleteSetting<SliderPluginSettings>();
            _objectContext.Uninstall();
            this.DeletePluginLocaleResource("Plugins.Widgets.Slider.transitionspeed");
            this.DeletePluginLocaleResource("Plugins.Widgets.Slider.Effect");
            this.DeletePluginLocaleResource("Plugins.Widgets.Slider.AddSlider");
            this.DeletePluginLocaleResource("Plugins.Widgets.Slider.PictureId");
            this.DeletePluginLocaleResource("Plugins.Widgets.Slider.Title");
            this.DeletePluginLocaleResource("Plugins.Widgets.Slider.ShortDescription");
            this.DeletePluginLocaleResource("Plugins.Widgets.Slider.Published");
            this.DeletePluginLocaleResource("Plugin.Widget.Slider.Sliders");
            this.DeletePluginLocaleResource("Plugins.Widgets.Slider.Name");
            this.DeletePluginLocaleResource("Plugins.widgets.slider.Displayoption");
            this.DeletePluginLocaleResource("Plugins.Widgets.Slider.Link");
            this.DeletePluginLocaleResource("Plugins.Widgets.Slider.Displayorder");
            this.DeletePluginLocaleResource("Plugins.Widgets.Slider.resolution");
            this.DeletePluginLocaleResource("Plugins.Widgets.Slider.Transitionspeedinmili");
            this.DeletePluginLocaleResource("plugins.widgets.event.displayorder");
            this.DeletePluginLocaleResource("Plugins.Widgets.Slider.PictureId.Hint");
            this.DeletePluginLocaleResource("plugins.widgets.slider.displayoption.hint");
            this.DeletePluginLocaleResource("plugins.widgets.slider.title.hint");
            this.DeletePluginLocaleResource("plugins.widgets.slider.shortdescription.hint");
            this.DeletePluginLocaleResource("plugins.widgets.slider.link.hint");
            this.DeletePluginLocaleResource("plugins.widgets.slider.displayorder.hint");
            this.DeletePluginLocaleResource("plugins.widgets.slider.transitionspeed.hint");
            this.DeletePluginLocaleResource("plugins.widgets.slider.effect.hint");
            this.DeletePluginLocaleResource("plugins.widgets.slider.published.hint");
            this.DeletePluginLocaleResource("plugins.widgets.event.displayorder.hint");
            base.Uninstall();
        
         }

    

    }
}
