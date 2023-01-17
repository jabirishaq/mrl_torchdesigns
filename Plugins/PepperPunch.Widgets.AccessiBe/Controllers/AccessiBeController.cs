using System.Web.Mvc;
using Nop.Core;
using PepperPunch.Widgets.AccessiBe.Models;
using Nop.Services.Configuration;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Core.Domain.Cms;

namespace PepperPunch.Widgets.AccessiBe.Controllers
{
    public class AccessiBeController : BasePluginController
    {
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;
        private readonly IStoreService _storeService;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;

        public AccessiBeController(ILocalizationService localizationService,
            IPermissionService permissionService,
            ISettingService settingService,
            IStoreContext storeContext,
            IStoreService storeService,
            IWebHelper webHelper,
            IWorkContext workContext)
        {
            this._localizationService = localizationService;
            this._permissionService = permissionService;
            this._settingService = settingService;
            this._storeContext = storeContext;
            this._storeService = storeService;
            this._webHelper = webHelper;
            this._workContext = workContext;
        }

        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure()
        {
            var storeId = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var settings = _settingService.LoadSetting<AccessiBeSettings>(storeId);
            var widgetSettings = _settingService.LoadSetting<WidgetSettings>(storeId);

            var model = new ConfigurationModel
            {
                Script = settings.Script,
                Enabled = widgetSettings.ActiveWidgetSystemNames.Contains("PepperPunch.Widgets.AccessiBe"),
                ActiveStoreScopeConfiguration = storeId
            };

            if (storeId > 0)
            {
                model.Script_OverrideForStore = _settingService.SettingExists(settings, setting => setting.Script, storeId);
                model.Enabled_OverrideForStore = _settingService.SettingExists(widgetSettings, setting => setting.ActiveWidgetSystemNames, storeId);
            }

            //prepare store URL
            model.Url = storeId > 0
                ? (_storeService.GetStoreById(storeId))?.Url
                : _webHelper.GetStoreLocation();

            return View("~/Plugins/PepperPunch.Widgets.AccessiBe/Views/Configure.cshtml", model);
            
        }

        [HttpPost]
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure(ConfigurationModel model)
        {

            if (!ModelState.IsValid)
                return Configure();

            var storeId = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var settings = _settingService.LoadSetting<AccessiBeSettings>(storeId);
            var widgetSettings = _settingService.LoadSetting<WidgetSettings>(storeId);

            settings.Script = model.Script;

            if (model.Script_OverrideForStore || storeId == 0)
                _settingService.SaveSetting(settings, x => x.Script, storeId, false);
            else if (storeId > 0)
                _settingService.DeleteSetting(settings, x => x.Script, storeId);

            if (model.Enabled && !widgetSettings.ActiveWidgetSystemNames.Contains("PepperPunch.Widgets.AccessiBe"))
                widgetSettings.ActiveWidgetSystemNames.Add("PepperPunch.Widgets.AccessiBe");
            if (!model.Enabled && widgetSettings.ActiveWidgetSystemNames.Contains("PepperPunch.Widgets.AccessiBe"))
                widgetSettings.ActiveWidgetSystemNames.Remove("PepperPunch.Widgets.AccessiBe");

            if (model.Enabled_OverrideForStore || storeId == 0)
                _settingService.SaveSetting(widgetSettings, setting => setting.ActiveWidgetSystemNames, storeId, false);
            else if (storeId > 0)
                _settingService.DeleteSetting(widgetSettings, setting => setting.ActiveWidgetSystemNames, storeId);


            _settingService.ClearCache();

            this.SuccessNotification( _localizationService.GetResource("Admin.Plugins.Saved"));

            return Configure();
           
        }

        [ChildActionOnly]
        public ActionResult PublicInfo(string widgetZone, object additionalData = null)
        {
            var storeId = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var settings = _settingService.LoadSetting<AccessiBeSettings>(storeId);
            var script = "";
            script = widgetZone != settings.WidgetZone
                ? string.Empty
                : settings.Script;

            return Content(script);
            
        }

    }
}