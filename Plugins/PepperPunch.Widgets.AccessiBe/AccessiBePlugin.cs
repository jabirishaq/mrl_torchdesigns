using System.Collections.Generic;
using System.Web.Routing;
using Nop.Core.Plugins;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;

namespace PepperPunch.Widgets.AccessiBe
{
    /// <summary>
    /// Live person provider
    /// </summary>
    public class AccessiBePlugin : BasePlugin, IWidgetPlugin
    {
        private readonly ISettingService _settingService;

        public AccessiBePlugin(ISettingService settingService)
        {
            this._settingService = settingService;
        }

        /// <summary>
        /// Gets widget zones where this widget should be rendered
        /// </summary>
        /// <returns>Widget zones</returns>
        public IList<string> GetWidgetZones()
        {
            return new List<string>()
            {
                "head_html_tag"
            };
        }

        /// <summary>
        /// Gets a route for provider configuration
        /// </summary>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "AccessiBe";
            routeValues = new RouteValueDictionary() { { "Namespaces", "PepperPunch.Widgets.AccessiBe.Controllers" }, { "area", null } };
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
            controllerName = "AccessiBe";
            routeValues = new RouteValueDictionary()
            {
                {"Namespaces", "PepperPunch.Widgets.AccessiBe.Controllers"},
                {"area", null},
                {"widgetZone", widgetZone}
            };
        }

        /// <summary>
        /// Install plugin
        /// </summary>
        public override void Install()
        {
            var settings = new AccessiBeSettings()
            {
                WidgetZone = "head_html_tag"
            };
            _settingService.SaveSetting(settings);

            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.AccessiBe.Fields.Enabled", "Enable");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.AccessiBe.Fields.Enabled.Hint", "Check to activate this widget.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.AccessiBe.Fields.Script", "Installation script");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.AccessiBe.Fields.Script.Hint", "Find your unique installation script on the Installation tab in your account and then copy it into this field.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.AccessiBe.Fields.Script.Required", "Installation script is required");
            base.Install();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<AccessiBeSettings>();

            //locales
            this.DeletePluginLocaleResource("Plugins.Widgets.AccessiBe.Fields.Enabled");
            this.DeletePluginLocaleResource("Plugins.Widgets.AccessiBe.Fields.Enabled.Hint");
            this.DeletePluginLocaleResource("Plugins.Widgets.AccessiBe.Fields.Script");
            this.DeletePluginLocaleResource("Plugins.Widgets.AccessiBe.Fields.Script.Hint");
            this.DeletePluginLocaleResource("Plugins.Widgets.AccessiBe.Fields.Script.Required");
            
            base.Uninstall();
        }
    }
}
