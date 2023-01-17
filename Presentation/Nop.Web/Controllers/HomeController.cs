using System.Web.Mvc;
using Nop.Web.Framework.Security;
using Nop.Core;
using Nop.Services.Media;
using Nop.Services.Configuration;
using Nop.Web;
using System.Collections.Generic;
using Nop.Web.Models.HomepageImage;
using Nop.Services.Stores;
using Nop.Core.Caching;
using Nop.Web.Infrastructure.Cache;
using Nop.Services.Catalog;
using Nop.Core.Domain.Catalog;
using System;
using System.Linq;

namespace Nop.Web.Controllers
{
    public partial class HomeController : BasePublicController
    {
        private readonly IWorkContext _workContext;
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;
        private readonly IStoreService _storeService;
        private readonly ICacheManager _cacheManager;
        private readonly IQrCodeService _qrcodeService;

        public HomeController(IWorkContext workContext, IPictureService pictureService, ISettingService settingService, IStoreContext storeContext, IStoreService storeService, ICacheManager cacheManager, IQrCodeService qrcodeService)
        {
            this._workContext = workContext;
           this._pictureService = pictureService;
            this._settingService = settingService;
            this._storeContext = storeContext;
            this._storeService = storeService;
            this._cacheManager = cacheManager;
            this._qrcodeService = qrcodeService;
        }
        [NopHttpsRequirement(SslRequirement.No)]
        public ActionResult Index(string code =null)
        {
          
            var qrcode = Request.QueryString["qr"];
            if(String.IsNullOrEmpty(qrcode))
            {
            var newqrformatecode = Request.RawUrl;
            if (!String.IsNullOrEmpty(newqrformatecode))
            {
                if (newqrformatecode.Contains("?qr-"))
                {
                int len = newqrformatecode.Length-1;
                string qrc = null;
                qrc = newqrformatecode.Substring(5);

                QrCode qmodel = new QrCode();
                qmodel.QrCodeName = qrc;
                qmodel.Date = System.DateTime.UtcNow;
               
                var recod = _qrcodeService.GetAllQrCodeWithotCount().Where(x => x.QrCodeName == qrc).ToList();
                string url;
                if (recod.Count >0 )
                {
                    url = recod.FirstOrDefault().QrCodeUrl;
                    if(!String.IsNullOrEmpty(url))
                    {
                     qmodel.QrCodeUrl = url;
                    _qrcodeService.InsertQrCode(qmodel);
                    return Redirect(url);
                    }
                    else { _qrcodeService.InsertQrCode(qmodel); }
                }
                else 
                {
                  
                    _qrcodeService.InsertQrCode(qmodel);
                }
              
                 }
            }
        }
           else if (!String.IsNullOrEmpty(code))
            {
                QrCode qmodel = new QrCode();
                qmodel.QrCodeName = code;
                qmodel.Date = System.DateTime.UtcNow;
                //_qrcodeService.InsertQrCode(qmodel);
                var recod = _qrcodeService.GetAllQrCodeWithotCount().Where(x => x.QrCodeName == code);
                string url;
                if (recod.Count() > 0)
                {
                    url = recod.FirstOrDefault().QrCodeUrl;
                    if (!String.IsNullOrEmpty(url))
                    {
                        qmodel.QrCodeUrl = url;
                        _qrcodeService.InsertQrCode(qmodel);
                        return Redirect(url);
                    }
                    else {
                        _qrcodeService.InsertQrCode(qmodel);
                    }
                   
                }
                else
                {
                    _qrcodeService.InsertQrCode(qmodel);
                }
            }
           else if (!String.IsNullOrEmpty(qrcode))
            {
                QrCode qmodel = new QrCode();
                qmodel.QrCodeName = qrcode;
                qmodel.Date = System.DateTime.UtcNow;
                //_qrcodeService.InsertQrCode(qmodel);
                var recod = _qrcodeService.GetAllQrCodeWithotCount().Where(x => x.QrCodeName == qrcode);
                string url;
                if (recod.Count() > 0)
                {
                    url = recod.FirstOrDefault().QrCodeUrl;
                    if (!String.IsNullOrEmpty(url))
                    {
                        qmodel.QrCodeUrl = url;
                        _qrcodeService.InsertQrCode(qmodel);
                        return Redirect(url);
                    }
                    else { _qrcodeService.InsertQrCode(qmodel); }
                }
                else {
                    _qrcodeService.InsertQrCode(qmodel);
                }
            }
            var nivoSliderSettings = _settingService.LoadSetting<HomepageImageSettings>(_storeContext.CurrentStore.Id);

            var model = new PublicInfoModel();

            model.Picture1Url = GetPictureUrl(nivoSliderSettings.Picture1Id);
            //model.Text1 = nivoSliderSettings.Text1;
            model.Link1 = nivoSliderSettings.Link1;

            model.Picture2Url = GetPictureUrl(nivoSliderSettings.Picture2Id);
            //model.Text2 = nivoSliderSettings.Text2;
            model.Link2 = nivoSliderSettings.Link2;

          

            return View(model);
          
        }

        protected string GetPictureUrl(int pictureId)
        {
            string cacheKey = string.Format("GEt_Started_Image{0}", pictureId);
            return _cacheManager.Get(cacheKey, () =>
            {
                var url = _pictureService.GetPictureUrl(pictureId, showDefaultPicture: false);
                //little hack here. nulls aren't cacheable so set it to ""
                if (url == null)
                    url = "";

                return url;
            });
        }

    }
}
