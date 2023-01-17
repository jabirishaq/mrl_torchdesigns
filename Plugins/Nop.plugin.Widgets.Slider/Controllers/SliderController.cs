using Nop.Core.Domain.Common;
using Nop.Core.Domain.Messages;
using Nop.Plugin.Widgets.Slider.Domain;
using Nop.Plugin.Widgets.Slider.Models;
using Nop.Plugin.Widgets.Slider.Services;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Web.Framework.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.IO;
using System.Net.Mail;
using System.Net;
using Nop.Core;
namespace Nop.Plugin.Widgets.Slider.Controllers
{
    public class SliderController : BasePluginController
    {
        #region Fields

        private readonly ISliderService _sliderService;
        private readonly IPictureService _pictureService;
        private readonly IEmailAccountService _emailAccountService;
        private readonly ILocalizationService _localizationService;
        private readonly EmailAccountSettings _emailAccountSettings;
        private readonly CommonSettings _commonSettings;
        private readonly IQueuedEmailService _queuedEmailService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IWorkContext _workContext;


        #endregion       

        #region Constructors

        public SliderController(ISliderService sliderService,
           IPictureService pictureService,
           IEmailAccountService emailAccountService,
           ILocalizationService localizationService,
           EmailAccountSettings emailAccountSettings,
           CommonSettings commonSettings,
           IQueuedEmailService queuedEmailService,
           ICustomerActivityService customerActivityService,
            IWorkContext workContext)
        {
            this._sliderService = sliderService;
            this._pictureService = pictureService;
            this._emailAccountService = emailAccountService;
            this._localizationService = localizationService;
            this._emailAccountSettings = emailAccountSettings;
            this._commonSettings = commonSettings;
            this._queuedEmailService = queuedEmailService;
            this._customerActivityService = customerActivityService;
            this._workContext = workContext;
        }
        
        #endregion

        #region Utilities

        [NonAction]
        //protected void PrepareSliderItemModel(SliderDetailModel model, SliderRecord sliderDetail, bool prepareComments)
        //{
        //    if (sliderDetail == null)
        //        throw new ArgumentNullException("newsItem");

        //    if (model == null)
        //        throw new ArgumentNullException("model");

        //    model.Id = sliderDetail.Id;
        //    model.Title = sliderDetail.Title;
        //    model.PictureUrl = _pictureService.GetPictureUrl(sliderDetail.PictureId);
           
        //    if (sliderDetail.StartDateUtc.HasValue)
        //    {
        //        if (sliderDetail.StartDateUtc.HasValue && sliderDetail.EndDateUtc.HasValue)
        //        {
        //            if (sliderDetail.StartDateUtc.Value.Year != sliderDetail.EndDateUtc.Value.Year)
        //            {
        //                model.StartEndDate = string.Format("{0} - {1}", sliderDetail.StartDateUtc.Value.ToString("MMMM dd, yyyy"), sliderDetail.EndDateUtc.Value.ToString("MMMM dd, yyyy"));
        //            }
        //            else
        //            {
        //                model.StartEndDate = string.Format("{0} - {1}", sliderDetail.StartDateUtc.Value.ToString("MMMM dd"), sliderDetail.EndDateUtc.Value.ToString("MMMM dd, yyyy"));
        //            }
        //        }
        //        else
        //        {
        //            model.StartEndDate = sliderDetail.StartDateUtc.Value.ToString("MMMM dd, yyyy");
        //        }
        //    }
        //    //model.MetaTitle = newsItem.MetaTitle;
        //    //model.MetaDescription = newsItem.MetaDescription;
        //    //model.MetaKeywords = newsItem.MetaKeywords;
        //    //model.SeName = newsItem.GetSeName(newsItem.LanguageId, ensureTwoPublishedLanguages: false);
        //    //model.Title = newsItem.Title;
        //    //model.Short = newsItem.Short;
        //    //model.Full = newsItem.Full;
        //    //model.AllowComments = newsItem.AllowComments;
        //    //model.CreatedOn = _dateTimeHelper.ConvertToUserTime(newsItem.CreatedOnUtc, DateTimeKind.Utc);
        //    //model.NumberOfComments = newsItem.CommentCount;
        //    //model.AddNewComment.DisplayCaptcha = _captchaSettings.Enabled && _captchaSettings.ShowOnNewsCommentPage;
        //    //if (prepareComments)
        //    //{
        //    //    var newsComments = eventDetail.NewsComments.OrderBy(pr => pr.CreatedOnUtc);
        //    //    foreach (var nc in newsComments)
        //    //    {
        //    //        var commentModel = new NewsCommentModel()
        //    //        {
        //    //            Id = nc.Id,
        //    //            CustomerId = nc.CustomerId,
        //    //            CustomerName = nc.Customer.FormatUserName(),
        //    //            CommentTitle = nc.CommentTitle,
        //    //            CommentText = nc.CommentText,
        //    //            CreatedOn = _dateTimeHelper.ConvertToUserTime(nc.CreatedOnUtc, DateTimeKind.Utc),
        //    //            AllowViewingProfiles = _customerSettings.AllowViewingProfiles && nc.Customer != null && !nc.Customer.IsGuest(),
        //    //        };
        //    //        if (_customerSettings.AllowCustomersToUploadAvatars)
        //    //        {
        //    //            commentModel.CustomerAvatarUrl = _pictureService.GetPictureUrl(
        //    //                nc.Customer.GetAttribute<int>(SystemCustomerAttributeNames.AvatarPictureId),
        //    //                _mediaSettings.AvatarPictureSize,
        //    //                _customerSettings.DefaultAvatarEnabled,
        //    //                defaultPictureType: PictureType.Avatar);
        //    //        }
        //    //        model.Comments.Add(commentModel);
        //    //    }
        //    //}
        //}

        //public string MakeEvent(SliderDetailModel.AppointmentFormModel model , string body)
        //{
        //    var customer = _workContext.CurrentCustomer;
        //    string filePath = string.Empty;
        //    string subject = string.Format("{0}. Book An Appointment", model.Name);
        //    string path = HttpContext.Server.MapPath(@"\Content\iCal\");
        //    FileInfo file = new FileInfo(path);
        //    if (!file.Directory.Exists)
        //        file.Directory.Create();

        //    filePath = path + model.Name.Replace(" ",String.Empty) + "_"+ customer.Id + ".ics";

        //    if (System.IO.File.Exists(filePath))
        //    {
        //        System.IO.File.Delete(filePath);
        //    }
        //    FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate);
        //    StreamWriter writer = new StreamWriter(fs);
        //    //var writer = new StreamWriter(filePath);
        //    writer.WriteLine("BEGIN:VCALENDAR");
        //    writer.WriteLine("VERSION:2.0");
        //    writer.WriteLine("PRODID:-//hacksw/handcal//NONSGML v1.0//EN");
        //    writer.WriteLine("BEGIN:VEVENT");
        //    //string startDay = "VALUE=DATE:" + GetFormatedDate(model.Date);
        //    //string endDay = "VALUE=DATE:" + GetFormatedDate(model.Date);
        //    writer.WriteLine("DTSTART:" + model.Date.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"));
        //    writer.WriteLine("DTEND:" + model.Date.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"));
        //    writer.WriteLine("SUMMARY:" + subject);
        //    writer.WriteLine("DESCRIPTION;ENCODING=QUOTED-PRINTABLE:" + body);
        //    //writer.WriteLine("LOCATION:" + location);
        //    writer.WriteLine("END:VEVENT");
        //    writer.WriteLine("END:VCALENDAR");            
        //    writer.Flush();
        //    writer.Close();
        //    fs.Close();
        //    return filePath;
        //}
        //
        #endregion

        #region Method

        //public ActionResult Detail(int sliderid)
        //{
        //    var sliderDetail = _sliderService.GetSliderById(sliderid);
        //    var model = new SliderDetailModel()
        //    {
        //     Title = sliderDetail.GetLocalized(x=>x.Title),
        //     PictureUrl = _pictureService.GetPictureUrl(sliderDetail.PictureId)

        //    };

        //    return View(model);
        //}

        
        #endregion
        
    }
}
