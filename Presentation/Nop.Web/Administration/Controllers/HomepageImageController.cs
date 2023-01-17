using System;
using System.Net;
using System.ServiceModel.Syndication;
using System.Web.Mvc;
using System.Xml;
using Nop.Admin.Infrastructure.Cache;
using Nop.Admin.Models.Home;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Common;
using Nop.Services.Configuration;
using Nop.Services.Stores;
using Nop.Services.Media;
using Nop.Admin.Models.HomepageImage;


namespace Nop.Admin.Controllers
{
    public class HomepageImageController : BaseAdminController
    {
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IStoreService _storeService;
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        private readonly ICacheManager _cacheManager;

        public HomepageImageController (IWorkContext workContext,
            IStoreContext storeContext,
            IStoreService storeService, 
            IPictureService pictureService,
            ISettingService settingService,
            ICacheManager cacheManager)
        {
            this._workContext = workContext;
            this._storeContext = storeContext;
            this._storeService = storeService;
            this._pictureService = pictureService;
            this._settingService = settingService;
            this._cacheManager = cacheManager;
        }

        //protected string GetPictureUrl(int pictureId)
        //{
        //    string cacheKey = string.Format(ModelCacheEventConsumer.PICTURE_URL_MODEL_KEY, pictureId);
        //    return _cacheManager.Get(cacheKey, () =>
        //    {
        //        var url = _pictureService.GetPictureUrl(pictureId, showDefaultPicture: false);
        //        //little hack here. nulls aren't cacheable so set it to ""
        //        if (url == null)
        //            url = "";

        //        return url;
        //    });
        //}

       
      
        public ActionResult Configure()
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var nivoSliderSettings = _settingService.LoadSetting<HomepageImageSettings>(storeScope);
            var model = new ConfigurationModel();
            model.Picture1Id = nivoSliderSettings.Picture1Id;
            //model.Text1 = nivoSliderSettings.Text1;
            model.Link1 = nivoSliderSettings.Link1;
            model.Picture2Id = nivoSliderSettings.Picture2Id;
            //model.Text2 = nivoSliderSettings.Text2;
            model.Link2 = nivoSliderSettings.Link2;
            model.ActiveStoreScopeConfiguration = storeScope;
            if (storeScope > 0)
            {
                model.Picture1Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture1Id, storeScope);
                //model.Text1_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text1, storeScope);
                model.Link1_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link1, storeScope);
                model.Picture2Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture2Id, storeScope);
                //model.Text2_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text2, storeScope);
                model.Link2_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link2, storeScope);
           }

            return View("HomepageTopic",model);
        }

        [HttpPost]
        public ActionResult Configure(ConfigurationModel model)
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var nivoSliderSettings = _settingService.LoadSetting<HomepageImageSettings>(storeScope);
            nivoSliderSettings.Picture1Id = model.Picture1Id;
            //nivoSliderSettings.Text1 = model.Text1;
            nivoSliderSettings.Link1 = model.Link1;
            nivoSliderSettings.Picture2Id = model.Picture2Id;
            //nivoSliderSettings.Text2 = model.Text2;
            nivoSliderSettings.Link2 = model.Link2;
           

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            if (model.Picture1Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Picture1Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Picture1Id, storeScope);
            
            //if (model.Text1_OverrideForStore || storeScope == 0)
            //    _settingService.SaveSetting(nivoSliderSettings, x => x.Text1, storeScope, false);
            //else if (storeScope > 0)
            //    _settingService.DeleteSetting(nivoSliderSettings, x => x.Text1, storeScope);
            
            if (model.Link1_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Link1, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Link1, storeScope);
            
            if (model.Picture2Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Picture2Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Picture2Id, storeScope);
            
            //if (model.Text2_OverrideForStore || storeScope == 0)
            //    _settingService.SaveSetting(nivoSliderSettings, x => x.Text2, storeScope, false);
            //else if (storeScope > 0)
            //    _settingService.DeleteSetting(nivoSliderSettings, x => x.Text2, storeScope);
            
            if (model.Link2_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Link2, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Link2, storeScope);
            
           
            
            //now clear settings cache
            _settingService.ClearCache();
            
            return Configure();
        }

        //[ChildActionOnly]
        //public ActionResult PublicInfo(string widgetZone, object additionalData = null)
        //{
        //    var nivoSliderSettings = _settingService.LoadSetting<HomepageImageSettings>(_storeContext.CurrentStore.Id);

        //    var model = new PublicInfoModel();
        //    model.Picture1Url = GetPictureUrl(nivoSliderSettings.Picture1Id);
        //    model.Text1 = nivoSliderSettings.Text1;
        //    model.Link1 = nivoSliderSettings.Link1;

        //    model.Picture2Url = GetPictureUrl(nivoSliderSettings.Picture2Id);
        //    model.Text2 = nivoSliderSettings.Text2;
        //    model.Link2 = nivoSliderSettings.Link2;

        //    if (string.IsNullOrEmpty(model.Picture1Url) && string.IsNullOrEmpty(model.Picture2Url) &&
        //   string.IsNullOrEmpty(model.Picture3Url) && string.IsNullOrEmpty(model.Picture4Url) &&
        //   string.IsNullOrEmpty(model.Picture5Url))
        //        //no pictures uploaded
        //        return Content("");


        //    return View("~/Plugins/Widgets.NivoSlider/Views/WidgetsNivoSlider/PublicInfo.cshtml", model);
        //}
    }
}