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
using Nop.Services.Catalog;
using Nop.Web.Framework.Kendoui;
using Nop.Core.Domain.Catalog;
using System.Linq;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Controllers
{
    public class QrCodeController : BaseAdminController
    {
         private readonly IQrCodeService _qrcodeservice;

         public QrCodeController(IQrCodeService qrcodeservice)
         {
             this._qrcodeservice = qrcodeservice;

         }

         public ActionResult CountQrCode()
        {
            QrCodeDiaplay model = new QrCodeDiaplay();
            return View(model);
        }

        [HttpPost]
         public ActionResult CountQrCode(QrCodeDiaplay model)
        {

            return CountQrCode();
        }

      [HttpPost]
        public ActionResult ListofQrCode(DataSourceRequest command, QrCodeDiaplay model)
        {

            var qrcode = _qrcodeservice.GetAllQrCode(command.Page - 1, command.PageSize);
            var qrcodeModel = qrcode.Select(x =>
            {
                var m = new QrCodeDiaplay()
                {
                    QrCodeName = x.QrCodeName,
                    Count = x.Count,
                    id=x.id,
                    QrCodeUrl=x.QrCodeUrl,
                };
                return m;
            })
              .ToList();
            var gridModel = new DataSourceResult
            {
                Data = qrcodeModel,
                Total = qrcode.TotalCount
            };

            return Json(gridModel);
        }

      [HttpPost]
      public ActionResult QrcodeInsert(QrCodeDiaplay model)
      {

          var CustOrigin = new QrCode()
          {
              QrCodeUrl = model.QrCodeUrl,
              QrCodeName = model.QrCodeName,
              Date = System.DateTime.Now
          };

          _qrcodeservice.InsertQrCode(CustOrigin);

          return new NullJsonResult();
      }

      [HttpPost]
      public ActionResult QrcodeUpdate(QrCodeDiaplay model)
      {
         
      string orignalqrname = _qrcodeservice.GetQrCodeById(model.id).QrCodeName;
      if (!String.IsNullOrEmpty(orignalqrname))
      {
          var qrlist = _qrcodeservice.GetAllQrCodeWithotCount().Where(x => x.QrCodeName == orignalqrname);

          foreach(var item in qrlist)
          {            
             item.QrCodeName = model.QrCodeName;
             item.QrCodeUrl = model.QrCodeUrl;
            _qrcodeservice.UpdateQrCode(item);
          }
 
      }
         
          return new NullJsonResult();
      }

    }
}