using System;
using System.Linq;
//using System.Text;
using System.Web.Mvc;
using Nop.Core;
using Nop.Plugin.TorchDesign.Testimonials.Services;
using Nop.Plugin.TorchDesign.Testimonials.Domain;
using Nop.Plugin.TorchDesign.Testimonials.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Kendoui;
using Nop.Web.Framework.Mvc;
using Nop.Services.Stores;
using Nop.Services.Media;
using System.Web;
using Nop.Core.Domain.Localization;
using System.Data;
using System.Data.SqlClient;
using Nop.Core.Domain.Common;
using Nop.Core.Data;
using Nop.Services.Seo;
using Nop.Services.Common;
using Nop.Web.Framework.UI.Captcha;

namespace Nop.Plugin.TorchDesign.Testimonials.Controllers
{


    public class TestimonialsController : BasePluginController
    {
       
        private readonly ITestimonialservice _Testimonialservice;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly ISettingService _settingService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly IPictureService _pictureService;
        private readonly ITestimonialMessageService _messageservice;
        private readonly LocalizationSettings _localizationSettings;
        private readonly CommonSettings _commonsettings;
        private readonly IUrlRecordService _seoservice;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IStoreContext _storeContext;
        private readonly CaptchaSettings _captchaSettings;

        public TestimonialsController(ITestimonialservice Testimonialservice, ILanguageService languageService,
            ILocalizationService localizationService, ILocalizedEntityService localizedEntityService,
            ISettingService settingService, IStoreService storeService, IWorkContext workContext,
            IPictureService pictureService, ITestimonialMessageService messageservice,
            LocalizationSettings localizationSettings, CommonSettings commonSettings,
            IUrlRecordService seoservice,
            IGenericAttributeService genericAttributeService,
            IStoreContext storeContext,
            CaptchaSettings captchaSettings)
        {
            this._Testimonialservice = Testimonialservice;
            this._languageService = languageService;
            this._localizationService = localizationService;
            this._localizedEntityService = localizedEntityService;
            this._settingService = settingService;
            this._storeService = storeService;
            this._workContext = workContext;
            this._pictureService = pictureService;
            this._messageservice = messageservice;
            this._localizationSettings = localizationSettings;
            this._commonsettings = commonSettings;
            this._seoservice = seoservice;
            this._genericAttributeService = genericAttributeService;
            this._storeContext = storeContext;
            this._captchaSettings = captchaSettings;
            this._captchaSettings = captchaSettings;
        }



        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            //little hack here
            //always set culture to 'en-US' (Telerik has a bug related to editing decimal values in other cultures). Like currently it's done for admin area in Global.asax.cs
            CommonHelper.SetTelerikCulture();

            base.Initialize(requestContext);
        }

        #region Configure
        public ActionResult Configure()
        {
            TestimonialsModel model = new TestimonialsModel();
            model.Appprovebymail = false;
            model.ApproveValue = "";
            model.filterby = 1;
            var testomonialid = Request.QueryString["testomonialid"];
            if (!String.IsNullOrEmpty(testomonialid))
            {
                model.Appprovebymail = true;

                string[] words = testomonialid.Split('-');
                if (words[0] == "Active")
                {
                    var testomonial = _Testimonialservice.GetTestimonialsById(Convert.ToInt32(words[1]));
                    testomonial.IsPublished = true;
                    testomonial.IsRejected = false;
                    _Testimonialservice.UpdateTestimonials(testomonial);
                    model.ApproveValue = "Testimonial has been approved";
                }
                if (words[0] == "Reject")
                {
                    var testomonial = _Testimonialservice.GetTestimonialsById(Convert.ToInt32(words[1]));

                         testomonial.IsPublished = false;
                         testomonial.IsRejected = true;
                        _Testimonialservice.UpdateTestimonials(testomonial);
                        model.ApproveValue = "Testimonial has been rejected";

                    
                }

            }
            return View("~/Plugins/TorchDesign.Testimonials/Views/Testimonials/Configure.cshtml", model);
        }
        [HttpPost]
        public ActionResult Configure(TestimonialsModel model)
        {

            //var testimonial =( (model.filterby == 0) ? (_Testimonialservice.GetAllTestimonials()) :((model.filterby == 1) ? (_Testimonialservice.GetAllDeactiveTestimonials()) :( (model.filterby == 2) ? _Testimonialservice.GetAllActiveTestimonials() : (model.filterby == 3) ? _Testimonialservice.GetAllRejectedTestimonials() : null )));
            //var gridModel = new DataSourceResult
            //{
            //    Data = testimonial.Select(x =>
            //    {

            //        return new TestimonialsModel()
            //        {
            //            Id = x.Id,
            //            CustomerName = x.CustomerName,
            //            Email = x.Email,
            //            Location = x.Location,
            //            Message = x.Message,
            //            CreateOn = x.CreateOn,
            //            IsPublished = x.IsPublished
            //        };
            //    }),
            //    Total = testimonial.TotalCount
            //};
            //return new JsonResult
            //{
            //    Data = gridModel
            //};


            return View("~/Plugins/TorchDesign.Testimonials/Views/Testimonials/Configure.cshtml", model);
        }

        [HttpPost]
        public ActionResult TestomonialList(DataSourceRequest command, TestimonialsModel model)
        {

            var testimonial = ((model.filterby == 0) ? (_Testimonialservice.GetAllTestimonials(command.Page-1, command.PageSize)) : ((model.filterby == 1) ? (_Testimonialservice.GetAllDeactiveTestimonials(command.Page-1, command.PageSize)) : ((model.filterby == 2) ? _Testimonialservice.GetAllActiveTestimonials(command.Page-1, command.PageSize) : (model.filterby == 3) ? _Testimonialservice.GetAllRejectedTestimonials(command.Page-1, command.PageSize) : null)));
            var gridModel = new DataSourceResult
            {
                Data = testimonial.Select(x =>
                {

                    return new TestimonialsModel()
                    {
                        Id = x.Id,
                        CustomerName = x.CustomerName,
                        Email = x.Email,
                        //Location = x.Location,
                        //Message = x.Message,
                        CreateOn = x.CreateOn,
                        IsPublished = x.IsPublished,
                        IsRejected = x.IsRejected
                    };
                }),
                Total = testimonial.TotalCount
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }


        [HttpPost]
        public ActionResult TestomonialUpdate(TestimonialsModel model)
        {

            var testomonial = _Testimonialservice.GetTestimonialsById(model.Id);

            testomonial.IsPublished = model.IsPublished;
            //testomonial.DefaultSelected = model.DefaultSelected;
            //testomonial.OptionName = model.OptionName;

            _Testimonialservice.UpdateTestimonials(testomonial);
            return new NullJsonResult();
        }


        public ActionResult EditPopup(int id)
        {
            // var model = new TestimonialsModel();
            //ar testimonial = _Testimonialservice.GetTestimonialsById(id);
            var obj = _Testimonialservice.GetTestimonialsById(id);

            var model = new TestimonialsModel
            {

                Id = obj.Id,
                Message = obj.Message,
                Location = obj.Location,
                CreateOn = obj.CreateOn,
                Email = obj.Email,
                CustomerName = obj.CustomerName,
                IsPublished = obj.IsPublished,
                IsRejected = obj.IsRejected,

            };
            var testopicture = _Testimonialservice.GetAllTestimonialsPicturesByTestimonialId(id);

            foreach (var pictureitem in testopicture)
            {
                TestimonialsPicture testomonialpicture = new TestimonialsPicture();

                testomonialpicture.PictureId = pictureitem.PictureId;
                testomonialpicture.FullSizePicture = _pictureService.GetPictureUrl(pictureitem.PictureId, 0);
                testomonialpicture.PictureUrl = _pictureService.GetPictureUrl(pictureitem.PictureId, 150);
                model.Picturelist.Add(testomonialpicture);
            }


            return View("~/Plugins/TorchDesign.Testimonials/Views/Testimonials/EditPopup.cshtml", model);
        }

        [HttpPost, ActionName("EditPopup")]
        [FormValueRequired("save")]
        public ActionResult SaveMethod(TestimonialsModel model)
        {


            var obj = _Testimonialservice.GetTestimonialsById(model.Id);
            if (obj == null)
                //No record found with the specified id
                return RedirectToAction("Configure");

            obj.Location = model.Location;
            obj.Message = model.Message;
            obj.CustomerName = model.CustomerName;
            _Testimonialservice.UpdateTestimonials(obj);


            ViewBag.RefreshPage = true;
            ViewBag.btnId = "btnRefresh";
            ViewBag.formId = "Widgets-event-form";

            return View("~/Plugins/TorchDesign.Testimonials/Views/Testimonials/EditPopup.cshtml", model);

        }

        [HttpPost, ActionName("EditPopup")]
        [FormValueRequired("Approve")]
        public ActionResult ApproveMethod(TestimonialsModel model)
        {

            var obj = _Testimonialservice.GetTestimonialsById(model.Id);
            if (obj == null)
                //No record found with the specified id
                return RedirectToAction("Configure");

            obj.IsPublished = true;
            obj.IsRejected = false;
            _Testimonialservice.UpdateTestimonials(obj);


            ViewBag.RefreshPage = true;
            ViewBag.btnId = "btnRefresh";
            ViewBag.formId = "Widgets-event-form";

            return View("~/Plugins/TorchDesign.Testimonials/Views/Testimonials/EditPopup.cshtml", model);
        }

        [HttpPost, ActionName("EditPopup")]
        [FormValueRequired("Reject")]
        public ActionResult RejectMethod(TestimonialsModel model)
        {
            var obj = _Testimonialservice.GetTestimonialsById(model.Id);
            if (obj == null)
                //No record found with the specified id
                return RedirectToAction("Configure");

            obj.IsRejected = true;
            obj.IsPublished = false;
            _Testimonialservice.UpdateTestimonials(obj);

            ViewBag.RefreshPage = true;
            ViewBag.btnId = "btnRefresh";
            ViewBag.formId = "Widgets-event-form";

            return View("~/Plugins/TorchDesign.Testimonials/Views/Testimonials/EditPopup.cshtml", model);
        }

        [HttpPost, ActionName("EditPopup")]
        [FormValueRequired("back")]
        public ActionResult backmethod(TestimonialsModel model)
        {
            ViewBag.RefreshPage = true;
            ViewBag.btnId = "btnRefresh";
            ViewBag.formId = "Widgets-event-form";

            return View("~/Plugins/TorchDesign.Testimonials/Views/Testimonials/EditPopup.cshtml", model);

        }

        [AdminAuthorize]
        public ActionResult Activate(int id)
        {
            if (id > 0)
            {
                var testomonial = _Testimonialservice.GetTestimonialsById(id);
                testomonial.IsPublished = true;
                testomonial.IsRejected = false;
                _Testimonialservice.UpdateTestimonials(testomonial);


                return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {

                return Json(new { Result = false }, JsonRequestBehavior.AllowGet);
            }

        }

        [AdminAuthorize]
        public ActionResult Reject(int id)
        {
            if (id > 0)
            {
                var testomonial = _Testimonialservice.GetTestimonialsById(id);
               
                    testomonial.IsRejected = true;
                    testomonial.IsPublished = false;
                    _Testimonialservice.UpdateTestimonials(testomonial);
                     return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
                
            }
            else
            {
                return Json(new { Result = false}, JsonRequestBehavior.AllowGet);
            }
        }

        [AdminAuthorize]
        public ActionResult ApproveTestimonial(int id)
        {
            if (id > 0)
            {
                var testomonial = _Testimonialservice.GetTestimonialsById(id);
                if(testomonial != null)
                {
                    testomonial.IsPublished = true;
                    testomonial.IsRejected = false;
                    _Testimonialservice.UpdateTestimonials(testomonial);
                }
                SuccessNotification("Testimonial has been approved");
                return Redirect(_storeContext.CurrentStore.Url + "Admin/Plugin/ConfigureMiscPlugin?systemName=Widgets.Testimonials");
            }
            else
            {
                return Redirect(_storeContext.CurrentStore.Url + "Admin/Plugin/ConfigureMiscPlugin?systemName=Widgets.Testimonials");
            }

        }

        #endregion

        #region Public Info

        public ActionResult PublicInfo()
        {
            PublicInfoModel model = new PublicInfoModel();
                PreparePublicInfoModel(model);

            return View("~/Plugins/TorchDesign.Testimonials/Views/Testimonials/PublicInfo.cshtml", model);
        }

        private void PreparePublicInfoModel(PublicInfoModel model)
        {
            var addModel = new AddTestiMonialModel();
            var listModel = new TestimonialListModel();
            addModel.IsInserted = Convert.ToBoolean(Session["IsInserted"]);
            Session["IsInserted"] = false;
            Session["MultiImageUplode"] = null;
            listModel.pageindex = 0;
            var testomoniallist = _Testimonialservice.GetAllActiveTestimonials(listModel.pageindex, 10);

            foreach (var item in testomoniallist)
            {
                TestimonialsList testomonial = new TestimonialsList();
                var testopicture = _Testimonialservice.GetAllTestimonialsPicturesByTestimonialId(item.Id);
                if (testopicture.Count() > 0)
                {
                    testomonial.DifaultPicture = _pictureService.GetPictureUrl(testopicture.FirstOrDefault().PictureId, 0);
                    foreach (var pictureitem in testopicture)
                    {
                        PictureList testomonialpicture = new PictureList();
                        testomonialpicture.PictureId = pictureitem.PictureId;
                        testomonialpicture.FullSizeImageUrl = _pictureService.GetPictureUrl(pictureitem.PictureId, 0);
                        testomonialpicture.PictureUrl = _pictureService.GetPictureUrl(pictureitem.PictureId, 100);
                        testomonial.PictureList.Add(testomonialpicture);
                    }


                }
                testomonial.tId = item.Id;
                testomonial.CustomerName = item.CustomerName;
                testomonial.Location = item.Location;
                testomonial.Message = item.Message;
                listModel.AvailableTestomonials.Add(testomonial);
            }
            model.addTestiMonialModel = addModel;
            model.testiMonialListModel = listModel;
         
        }

        [HttpPost, ActionName("PublicInfo")]
        [FormValueRequired("pagingbutton")]
        public ActionResult Nextprev(PublicInfoModel model)
        {
            var testomoniallist = _Testimonialservice.GetAllActiveTestimonials(model.testiMonialListModel.pageindex, 10);
            var listModel = new TestimonialListModel();
            listModel.pageindex = model.testiMonialListModel.pageindex;
           
            foreach (var item in testomoniallist)
            {
                TestimonialsList testomonial = new TestimonialsList();
                var testopicture = _Testimonialservice.GetAllTestimonialsPicturesByTestimonialId(item.Id);
               
                if (testopicture.Count() > 0)
                {
                    testomonial.DifaultPicture = _pictureService.GetPictureUrl(testopicture.FirstOrDefault().PictureId, 0);
                    foreach (var pictureitem in testopicture)
                    {
                        PictureList testomonialpicture = new PictureList();
                        testomonialpicture.PictureId = pictureitem.PictureId;
                        testomonialpicture.FullSizeImageUrl = _pictureService.GetPictureUrl(pictureitem.PictureId, 0);
                        testomonialpicture.PictureUrl = _pictureService.GetPictureUrl(pictureitem.PictureId, 100);
                        testomonial.PictureList.Add(testomonialpicture);
                    }

                 
                }
                testomonial.tId = item.Id;
                testomonial.CustomerName = item.CustomerName;
                testomonial.Location = item.Location;
                testomonial.Message = item.Message;
                listModel.AvailableTestomonials.Add(testomonial);
            }

            var addModel = new AddTestiMonialModel();
            addModel.IsInserted = false;
            model.testiMonialListModel = listModel;
            model.addTestiMonialModel = addModel;
            return View("~/Plugins/TorchDesign.Testimonials/Views/Testimonials/PublicInfo.cshtml", model);
        }

        [HttpPost, ActionName("PublicInfo")]
        [FormValueRequired("Savesettingbtn")]
        [CaptchaValidator]
        public ActionResult SaveTestimonial(PublicInfoModel model, bool captchaValid)
        {
            //validate CAPTCHA
            if (_captchaSettings.Enabled && !captchaValid)
            {
                ModelState.AddModelError("", _captchaSettings.GetWrongCaptchaMessage(_localizationService));
            }
            if (ModelState.IsValid) {
                if (Session["MultiImageUplode"] != null)
                {
                    string picturidlist = Session["MultiImageUplode"].ToString();
                    string[] PIdArray = picturidlist.Split('+');

                    var testomonial = new Td_Testimonials
                    {

                        CustomerName = model.addTestiMonialModel.CustomerName,
                        Email = model.addTestiMonialModel.Email,
                        IsPublished = false,
                        CreateOn = System.DateTime.UtcNow,
                        CustomerId = _workContext.CurrentCustomer.Id,
                        Location = model.addTestiMonialModel.Location,
                        Message = model.addTestiMonialModel.Message,

                    };
                    _Testimonialservice.InsertTestimonials(testomonial);


                    //Insert Picture List Of Testomonial
                    for (int i = 0; i < PIdArray.Count(); i++)
                    {
                        Td_Testimonials_Pictures tpicture = new Td_Testimonials_Pictures();
                        tpicture.PictureId = Convert.ToInt32(PIdArray[i]);
                        tpicture.TestimonialsId = testomonial.Id;
                        _Testimonialservice.InsertTestimonialsPicture(tpicture);
                    }

                    int[] pictureids = Array.ConvertAll(PIdArray, int.Parse);
                    _messageservice.SendCustomerTestimonialMail(testomonial, pictureids, _localizationSettings.DefaultAdminLanguageId);
                    model.addTestiMonialModel.IsInserted = true;


                }
                else
                {
                    var testomonial = new Td_Testimonials
                    {

                        CustomerName = model.addTestiMonialModel != null ? model.addTestiMonialModel.CustomerName : string.Empty,
                        Email = model.addTestiMonialModel != null ? model.addTestiMonialModel.Email : string.Empty,
                        IsPublished = false,
                        CreateOn = System.DateTime.UtcNow,
                        CustomerId = _workContext.CurrentCustomer.Id,
                        Location = model.addTestiMonialModel != null ? model.addTestiMonialModel.Location : string.Empty,
                        Message = model.addTestiMonialModel != null ? model.addTestiMonialModel.Message : string.Empty,

                    };
                    _Testimonialservice.InsertTestimonials(testomonial);

                    _messageservice.SendCustomerTestimonialMail(testomonial, null, _localizationSettings.DefaultAdminLanguageId);
                    model.addTestiMonialModel.IsInserted = true;

                }
                Session["IsInserted"] = true;
                return RedirectToAction("PublicInfo");
            }
            PreparePublicInfoModel(model);

            return View("~/Plugins/TorchDesign.Testimonials/Views/Testimonials/PublicInfo.cshtml", model);
        }


        public ActionResult SingleTestimonial(int testimonialid)
        {
            if (testimonialid == 0)
            {
                return RedirectToAction("PublicInfo");
            }
            else 
            {
                var obj = _Testimonialservice.GetTestimonialsById(testimonialid);

                if (obj == null)
                {
                    return RedirectToAction("PublicInfo");
                }
                else 
                {
                    var model = new TestimonialsModel
                    {

                        Id = obj.Id,
                        Message = obj.Message,
                        Location = obj.Location,
                        CreateOn = obj.CreateOn,
                        Email = obj.Email,
                        CustomerName = obj.CustomerName,
                        IsPublished = obj.IsPublished,
                        IsRejected = obj.IsRejected,

                    };
                    var testopicture = _Testimonialservice.GetAllTestimonialsPicturesByTestimonialId(testimonialid);
                    if (testopicture.Count > 0 )
                    {
                        model.DifaultPicture = _pictureService.GetPictureUrl(testopicture.FirstOrDefault().PictureId, 0);

                        foreach (var pictureitem in testopicture)
                        {
                            TestimonialsPicture testomonialpicture = new TestimonialsPicture();

                            testomonialpicture.PictureId = pictureitem.PictureId;
                            testomonialpicture.FullSizePicture = _pictureService.GetPictureUrl(pictureitem.PictureId, 0);
                            testomonialpicture.PictureUrl = _pictureService.GetPictureUrl(pictureitem.PictureId, 150);
                            model.Picturelist.Add(testomonialpicture);
                        }
                    }
                   
                    return View("~/Plugins/TorchDesign.Testimonials/Views/Testimonials/SingleTestimonial.cshtml", model);

                }
            }

        }

        #endregion

        #region Import Data
        //[ChildActionOnly]
        public ActionResult Import()
        {
            /* For testimonials*/

            var model = new Td_Testimonials();
            //var serverName = _commonsettings.serverName;
            //var databaseName = _commonsettings.databaseName;
            //var usderId = _commonsettings.usderId;
            //var password = _commonsettings.password;
            //var strCon = "Data Source="+ serverName +";Initial Catalog="+ databaseName +";Integrated Security=False;Persist Security Info=False;User ID="+ usderId +";Password="+ password +"";
            

            var strCon = new DataSettingsManager().LoadSettings().DataConnectionString;
            SqlConnection con = new SqlConnection(strCon);
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from CustomerStoryQueue", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            cmd.ExecuteNonQuery();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {

                model.CustomerId = 0;
                model.CustomerName = ds.Tables[0].Rows[i]["customerName"].ToString();
                model.Email = null;
                model.Location = ds.Tables[0].Rows[i]["location"].ToString();
                model.Message = ds.Tables[0].Rows[i]["customerStory"].ToString();
                if (ds.Tables[0].Rows[i]["storyStatus"].ToString() == "Approved")
                {
                    model.IsPublished = true;
                }
                else if (ds.Tables[0].Rows[0]["storyStatus"].ToString() == "Pendding")
                {
                    model.IsPublished = false;
                }
                else if (ds.Tables[0].Rows[0]["storyStatus"].ToString() == "Denied")
                {
                    model.IsRejected = true;
                }
                else
                {
                    model.IsPublished = false;
                }
                model.CreateOn = Convert.ToDateTime(ds.Tables[0].Rows[0]["submitDate"]);

                _Testimonialservice.InsertTestimonials(model);
            }
            con.Close();




            /*For testimonial pictures*/

            var model1 = new Td_Testimonials_Pictures();
            //var strCon1 = "Data Source=" + serverName + ";Initial Catalog=" + databaseName + ";Integrated Security=False;Persist Security Info=False;User ID=" + usderId + ";Password=" + password + "";
            var strCon1 = new DataSettingsManager().LoadSettings().DataConnectionString;
            SqlConnection con1 = new SqlConnection(strCon1);
            con1.Open();
            SqlCommand cmd1 = new SqlCommand("select * from GardenTours", con1);
            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1);
            cmd1.ExecuteNonQuery();
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {

                var imageName = ds1.Tables[0].Rows[i]["fileName"].ToString();
                var fullPath = Server.MapPath("~/Content/Images/MrL/images/GardenTours/" + imageName);
                if (System.IO.File.Exists(fullPath))
               { 
                var mimeType = MimeMapping.GetMimeMapping(fullPath);

                if (mimeType == "application/octet-stream")
                    mimeType = "image/jpeg";
                var newPictureBinary = System.IO.File.ReadAllBytes(fullPath);
                var Picture = _pictureService.InsertPicture(newPictureBinary, mimeType, imageName, true, true);

                model1.PictureId = Picture.Id;
                model1.TestimonialsId = Convert.ToInt32(ds1.Tables[0].Rows[i]["storyId"]);
                _Testimonialservice.InsertTestimonialsPicture(model1);
               }
            }
            con1.Close();

            return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
