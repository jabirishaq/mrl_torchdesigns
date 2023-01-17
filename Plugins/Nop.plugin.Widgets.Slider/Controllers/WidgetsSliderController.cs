using Nop.Core;
using Nop.Plugin.Widgets.Slider.Domain;
using Nop.Plugin.Widgets.Slider.Models;
using Nop.Plugin.Widgets.Slider.Services;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Seo;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Kendoui;
using Nop.Web.Framework.Localization;
using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Nop.Plugin.Widgets.Slider;
using Nop.Services.Blogs;
using Nop.Services.News;
using System.Text.RegularExpressions;
using Nop.Services.Stores;


namespace Nop.Plugin.Widgets.Slider.Controllers
{
    public partial class WidgetsSliderController : BasePluginController
    {
        #region Fields
        private readonly IWorkContext _workContext;
        private readonly ISliderService _sliderService;
        private readonly IPictureService _pictureService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly IUrlRecordService _urlRecordService;
        //private readonly SliderPluginSettings _sliderpluginsettings;
        private readonly ISettingService _settingService;
        private readonly IBlogService _blogservice;
        private readonly INewsService _news;

        private readonly IStoreService _storeService;
        private readonly IStoreContext _storeContext;

        #endregion

        public WidgetsSliderController(ISliderService eventService,
            IPictureService pictureService,
            ILanguageService languageService,
            ILocalizationService localizationService,
            ILocalizedEntityService localizedEntityService,
            IUrlRecordService urlRecordService,
            IWorkContext workContext,
            //SliderPluginSettings sliderpluginsettings,
            ISettingService settingService, IBlogService blogservice, INewsService news, IStoreService storeService, IStoreContext storeContext
            )
        {
            this._sliderService = eventService;
            this._pictureService = pictureService;
            this._languageService = languageService;
            this._localizationService = localizationService;
            this._localizedEntityService = localizedEntityService;
            this._urlRecordService = urlRecordService;
            this._workContext = workContext;
            //this._sliderpluginsettings = sliderpluginsettings;
            this._settingService = settingService;
            this._blogservice = blogservice;
            this._news = news;
            this._storeService = storeService;
            this._storeContext= storeContext;

        }

        #region Utilities

        protected string GetPictureUrl(int pictureId)
        {
            //string cacheKey = string.Format(ModelCacheEventConsumer.PICTURE_URL_MODEL_KEY, pictureId);
            //return _cacheManager.Get(cacheKey, () =>
            //{
            var url = _pictureService.GetPictureUrl(pictureId, showDefaultPicture: false);
            //little hack here. nulls aren't cacheable so set it to ""
            if (url == null)
                url = "";

            return url;
            //});
        }

        #endregion
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            //little hack here
            //always set culture to 'en-US' (Telerik has a bug related to editing decimal values in other cultures). Like currently it's done for admin area in Global.asax.cs
            Nop.Core.CommonHelper.SetTelerikCulture();

            base.Initialize(requestContext);
        }



        //public ActionResult MyConfigure()
        //{
        //    var model = new ConfigurationModel();
        //    //model.EventQuntityOnHomepage = _sliderpluginsettings.EventQuntityOnHomepage;
        //    //return View("~/Plugins/Widgets.Slider/Views/WidgetsSlider/MyConfigure.cshtml", model);
        //    //return Redirect("ConfigureWidget?systemName=Widgets.Slider");
        //    return Redirect("/Plugin/ConfigureMiscPlugin?systemName=Widgets.Slider");
        //}

        #region Configure

        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure()
        {
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var Slidersetting = _settingService.LoadSetting<SliderPluginSettings>(storeScope);
            var model = new ConfigurationModel();
            //IEnumerable<SlidingEffect> actionTypes = Enum.GetValues(typeof(SlidingEffect))
            //                                          .Cast<SlidingEffect>();
            //model.AvailableSliderEffect = from action in actionTypes
            //                               select new SelectListItem
            //                               {
            //                                   Text = action.ToString(),
            //                                   Value = ((int)action).ToString()
            //                               };

            //For Global Trasition Spped
            //model.transitionSpeed = Slidersetting.Transitionspeed;
            //model.Slidereffect = Slidersetting.Slidereffect;
            //if (storeScope > 0)
            //{
            //    model.transitionSpeed_OverrideForStore = _settingService.SettingExists(Slidersetting, x => x.Transitionspeed, storeScope);
            //    //model.Slidereffect_OverrideForStore = _settingService.SettingExists(Slidersetting, x => x.Slidereffect, storeScope);
            //}
            //model.EventQuntityOnHomepage = _sliderpluginsettings.EventQuntityOnHomepage;
            return View("~/Plugins/Widgets.Slider/Views/WidgetsSlider/Configure.cshtml", model);
        }

        [HttpPost]
        public ActionResult Configure(ConfigurationModel model)
        {
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var Slidersetting = _settingService.LoadSetting<SliderPluginSettings>(storeScope);
            //_sliderpluginsettings.EventQuntityOnHomepage = model.EventQuntityOnHomepage;
            //_settingService.SaveSetting(_sliderpluginsettings);
            //Slidersetting.Slidereffect = model.Slidereffect;

            //For Global Trasition Spped
           // Slidersetting.Transitionspeed = model.transitionSpeed;

            //if (model.Slidereffect_OverrideForStore || storeScope == 0)
            //    _settingService.SaveSetting(Slidersetting, x => x.Slidereffect, storeScope, false);
            //else if (storeScope > 0)
            //    _settingService.DeleteSetting(Slidersetting, x => x.Slidereffect, storeScope);

            //For Global Trasition Spped
            //if (model.transitionSpeed_OverrideForStore || storeScope == 0)
            //    _settingService.SaveSetting(Slidersetting, x => x.Transitionspeed, storeScope, false);
            //else if (storeScope > 0)
            //    _settingService.DeleteSetting(Slidersetting, x => x.Transitionspeed, storeScope);

            ////now clear settings cache
            //_settingService.ClearCache();

            //IEnumerable<SlidingEffect> actionTypes = Enum.GetValues(typeof(SlidingEffect))
            //                                        .Cast<SlidingEffect>();
            //model.AvailableSliderEffect = from action in actionTypes
            //                              select new SelectListItem
            //                              {
            //                                  Text = action.ToString(),
            //                                  Value = ((int)action).ToString()
            //                              };
            return View("~/Plugins/Widgets.Slider/Views/WidgetsSlider/Configure.cshtml", model);
        }


        [NonAction]
        public void UpdateLocales(SliderRecord sliderrecord, ConfigurationModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(sliderrecord,
                                                               x => x.Title,
                                                               localized.Title,
                                                               localized.LanguageId);
                _localizedEntityService.SaveLocalizedValue(sliderrecord,
                                                               x => x.ShortDescription,
                                                               localized.ShortDescription,
                                                               localized.LanguageId);



                //search engine name
                //var seName = product.ValidateSeName(localized.SeName, localized.Name, false);
                //_urlRecordService.SaveSlug(eventrecord, localized.LanguageId);
            }
        }

        public ActionResult AddPopup()
        {
            ConfigurationModel model = new ConfigurationModel();

            IEnumerable<Option> actionTypes = Enum.GetValues(typeof(Option))
                                                       .Cast<Option>();
            model.AvailableDisplayOption = from action in actionTypes
                                           select new SelectListItem
                                           {
                                               Text = action.ToString(),
                                               Value = ((int)action).ToString()
                                           };
            AddLocales(_languageService, model.Locales);

            IEnumerable<SlidingEffect> slidingeffect = Enum.GetValues(typeof(SlidingEffect))
                                                      .Cast<SlidingEffect>();
            model.AvailableSliderEffect = from action in slidingeffect
                                          select new SelectListItem
                                          {
                                              Text = action.ToString(),
                                              Value = ((int)action).ToString()
                                          };

            return View("~/Plugins/Widgets.Slider/Views/WidgetsSlider/AddPopup.cshtml", model);
        }

        [HttpPost]
        public ActionResult AddPopup(string btnId, string formId, ConfigurationModel model)
        {

            if (model.PictureId == 0)
            {
               
                ModelState.AddModelError("", "Please upload image.");
                //model.Errors.Add("Please upload image.");
            }
            if (model.DisplayOptionid == (int)Option.Custom)
            {
                if (string.IsNullOrEmpty(model.Title) || string.IsNullOrEmpty(model.ShortDescription) || string.IsNullOrEmpty(model.link))
            {
                ModelState.AddModelError("", "Please fill all the field.");
                //model.Errors.Add("Please fill all the field.");

            }
            }
            if (model.SlidePushtime <= 0) 
            {
                ModelState.AddModelError("", "Please Enter Transition Speed More Then 0.");
            }
            if (ModelState.IsValid)
            {
                var sliderobj = new SliderRecord
                {
                    Id = model.Id,
                    PictureId = model.PictureId,
                    ShortDescription = model.ShortDescription,
                    link = model.link,
                    DisplayOrder = model.DisplayOrder,
                    SlidePushtime=model.SlidePushtime,
                    DisplayOption = model.DisplayOptionid,
                    Title = model.Title,
                    SlidingEffect=model.Slidereffect,
                    Published = model.Published

                };
                _sliderService.InsertSlider(sliderobj);
                UpdateLocales(sliderobj, model);

                ViewBag.RefreshPage = true;
                ViewBag.btnId = btnId;
                ViewBag.formId = formId;
            }
            IEnumerable<Option> actionTypes = Enum.GetValues(typeof(Option))
                                                     .Cast<Option>();
            model.AvailableDisplayOption = from action in actionTypes
                                           select new SelectListItem
                                           {
                                               Text = action.ToString(),
                                               Value = ((int)action).ToString()
                                           };

            IEnumerable<SlidingEffect> slidingeffect = Enum.GetValues(typeof(SlidingEffect))
                                                     .Cast<SlidingEffect>();
            model.AvailableSliderEffect = from action in slidingeffect
                                          select new SelectListItem
                                          {
                                              Text = action.ToString(),
                                              Value = ((int)action).ToString()
                                          };
            return View("~/Plugins/Widgets.Slider/Views/WidgetsSlider/AddPopup.cshtml", model);
        }

        protected virtual void AddLocales<TLocalizedModelLocal>(ILanguageService languageService, IList<TLocalizedModelLocal> locales) where TLocalizedModelLocal : ILocalizedModelLocal
        {
            AddLocales(languageService, locales, null);
        }
        protected virtual void AddLocales<TLocalizedModelLocal>(ILanguageService languageService, IList<TLocalizedModelLocal> locales, Action<TLocalizedModelLocal, int> configure) where TLocalizedModelLocal : ILocalizedModelLocal
        {
            foreach (var language in languageService.GetAllLanguages(true))
            {
                var locale = Activator.CreateInstance<TLocalizedModelLocal>();
                locale.LanguageId = language.Id;
                if (configure != null)
                {
                    configure.Invoke(locale, locale.LanguageId);
                }
                locales.Add(locale);
            }
        }
        //edit
        public ActionResult EditPopup(int id)
        {


            var sliderobj = _sliderService.GetSliderById(id);
            if (sliderobj == null)
                //No record found with the specified id
                return RedirectToAction("~/Plugins/Widgets.Slider/Views/WidgetsSlider/Configure.cshtml");

            var model = new ConfigurationModel
            {

                Id = sliderobj.Id,
                PictureId = sliderobj.PictureId,
                PictureUrl = _pictureService.GetPictureUrl(sliderobj.PictureId),
                ShortDescription = sliderobj.ShortDescription,
                link = sliderobj.link,
                DisplayOrder = sliderobj.DisplayOrder,
                SlidePushtime=sliderobj.SlidePushtime,
                DisplayOptionid = sliderobj.DisplayOption,
                Slidereffect=sliderobj.SlidingEffect,
                Title = sliderobj.Title,
                Published = sliderobj.Published

            };

            IEnumerable<Option> actionTypes = Enum.GetValues(typeof(Option))
                                                      .Cast<Option>();
            model.AvailableDisplayOption = from action in actionTypes
                                           select new SelectListItem
                                           {
                                               Text = action.ToString(),
                                               Value = ((int)action).ToString()
                                           };

            IEnumerable<SlidingEffect> slidingeffect = Enum.GetValues(typeof(SlidingEffect))
                                                     .Cast<SlidingEffect>();
            model.AvailableSliderEffect = from action in slidingeffect
                                          select new SelectListItem
                                          {
                                              Text = action.ToString(),
                                              Value = ((int)action).ToString()
                                          };

            AddLocales(_languageService, model.Locales, (locale, languageId) =>
            {
                locale.ShortDescription = sliderobj.GetLocalized(x => x.ShortDescription, languageId, false, false);
                locale.Title = sliderobj.GetLocalized(x => x.Title, languageId, false, false);
            });





            return View("~/Plugins/Widgets.Slider/Views/WidgetsSlider/EditPopup.cshtml", model);
        }


        [HttpPost]
        public ActionResult EditPopup(string btnId, string formId, ConfigurationModel model)
        {

            if (model.PictureId == 0)
            {

                ModelState.AddModelError("", "Please upload image.");
                //model.Errors.Add("Please upload image.");
            }
            if (model.DisplayOptionid == (int)Option.Custom)
            {
                if (string.IsNullOrEmpty(model.Title) || string.IsNullOrEmpty(model.ShortDescription) || string.IsNullOrEmpty(model.link))
                {
                    ModelState.AddModelError("", "Please fill all the field.");
                    //model.Errors.Add("Please fill all the field.");

                }
            }
            if (model.SlidePushtime <= 0)
            {
                ModelState.AddModelError("", "Please Enter Transition Speed More Then 0.");
            }
            if (ModelState.IsValid)
            {
                var sliderobj = _sliderService.GetSliderById(model.Id);
                if (sliderobj == null)
                    //No record found with the specified id
                    return RedirectToAction("~/Plugins/Widgets.Slider/Views/WidgetsSlider/Configure.cshtml");

                sliderobj.Id = model.Id;
                sliderobj.PictureId = model.PictureId;
                sliderobj.ShortDescription = model.ShortDescription;
                sliderobj.link = model.link;
                sliderobj.DisplayOrder = model.DisplayOrder;
                sliderobj.DisplayOption = model.DisplayOptionid;
                sliderobj.SlidePushtime = model.SlidePushtime;
                sliderobj.Title = model.Title;
                sliderobj.SlidingEffect = model.Slidereffect;
                sliderobj.Published = model.Published;
                _sliderService.UpdateSlider(sliderobj);
                UpdateLocales(sliderobj, model);
                ViewBag.RefreshPage = true;
                ViewBag.btnId = btnId;
                ViewBag.formId = formId;
             }


            IEnumerable<Option> actionTypes = Enum.GetValues(typeof(Option))
                                                      .Cast<Option>();
            model.AvailableDisplayOption = from action in actionTypes
                                           select new SelectListItem
                                           {
                                               Text = action.ToString(),
                                               Value = ((int)action).ToString()
                                           };

            IEnumerable<SlidingEffect> slidingeffect = Enum.GetValues(typeof(SlidingEffect))
                                                     .Cast<SlidingEffect>();
            model.AvailableSliderEffect = from action in slidingeffect
                                          select new SelectListItem
                                          {
                                              Text = action.ToString(),
                                              Value = ((int)action).ToString()
                                          };
            return View("~/Plugins/Widgets.Slider/Views/WidgetsSlider/EditPopup.cshtml", model);
        }
    

        [HttpPost]
        public ActionResult SliderList(DataSourceRequest command, ConfigurationModel model)
        {
            var sliders = _sliderService.GetAllSliders(command.Page - 1, command.PageSize);
            var sliderModel = sliders.Select(x =>
            {
                var option = (Option)x.DisplayOption;
                var effect = (SlidingEffect)x.SlidingEffect;
                var m = new SliderListModel()
                {

                    Id = x.Id,
                    PictureId = x.PictureId,
                    PictureUrl = GetPictureUrl(x.PictureId),
                    ShortDescription = x.ShortDescription,
                    DisplayOption = option.ToString(),
                    link = x.link,
                    DisplayOrder = x.DisplayOrder,
                    SlidePushtime=x.SlidePushtime,
                    Title = x.Title,
                    Slidingeffect=effect.ToString(),
                    Published = x.Published
                };
                return m;
            })
              .ToList();
            var gridModel = new DataSourceResult
            {
                Data = sliderModel,
                Total = sliders.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        public ActionResult SliderUpdate(ConfigurationModel model)
        {
            var updateSlider = _sliderService.GetSliderById(model.Id);
            if (updateSlider == null)
                throw new ArgumentException("No Slider found with the specified id");


            //if (string.IsNullOrEmpty(model.Title) || string.IsNullOrEmpty(model.Description) || model.StartDate.ToString() == "" || model.EndDate.ToString() == "")
            //{
            //    ErrorNotification("Please fill all the field.");
            //    return new NullJsonResult();
            //}

            var sliderRecord = _sliderService.GetSliderById(model.Id);
            sliderRecord.Title = model.Title;
            sliderRecord.ShortDescription = model.ShortDescription;

            _sliderService.UpdateSlider(sliderRecord);
            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult SliderDelete(int id)
        {
            var deleteSlider = _sliderService.GetSliderById(id);
            if (deleteSlider == null)
                throw new ArgumentException("No event found with the specified id");
            _sliderService.DeleteSlider(deleteSlider);
            return new NullJsonResult();
        }


        #endregion

        #region Public Info

        [ChildActionOnly]
        public ActionResult PublicInfo()
        {
             var SliderSettings = _settingService.LoadSetting<SliderPluginSettings>(_storeContext.CurrentStore.Id);
            //int Quntityonhomepage = _sliderpluginsettings.EventQuntityOnHomepage;
            var activeSlider = _sliderService.GetAllActiveSliders();
            //var slideeffect = (SlidingEffect)SliderSettings.Slidereffect;

            //For Global Trasition Spped
           // var Transitiontime = SliderSettings.Transitionspeed;

            var model = new SliderDetailModel();
            var dis = string.Empty;
            var title = string.Empty;
            var redirectlink = string.Empty;


            foreach (var eve in activeSlider)
            {

                switch ((Option)eve.DisplayOption)
                {
                    case Option.NoDescription:
                        dis = string.Empty;
                        title = string.Empty;
                        redirectlink = eve.link;
                        break;
                    case Option.Blog:
                        var blog = _blogservice.Getlastblogbody();
                        if (blog != null)
                        {
                            title = blog.Title; 
                            redirectlink = blog.GetSeName(blog.LanguageId, ensureTwoPublishedLanguages: false);
                            string string1 = Regex.Replace(blog.Body, @"(<img\/?[^>]+>)", @"",RegexOptions.IgnoreCase);
                            string string2 = Regex.Replace(string1, "<(.|\n)*?>", "");
                            string blogtext = Regex.Replace(string2, "\r\n", "");
                            if (blogtext.Length > 196)
                            {
                                dis = (blogtext.Substring(0, 196)) + ("...");
                            }
                            else
                            {
                                dis = blogtext;
                            }
                        }
                        break;
                    case Option.News:
                        var newslist = _news.Getlastnews();

                        if (newslist != null)
                        {
                            title = newslist.Title;
                            redirectlink = newslist.GetSeName(newslist.LanguageId, ensureTwoPublishedLanguages: false);
                            var strignews1 = Regex.Replace(newslist.Short, "<(.|\n)*?>", "");
                            string newsdescription = Regex.Replace(strignews1, "\r\n", "");
                            if (newsdescription.Length > 196)
                            {
                                dis = (newsdescription.Substring(0, 196)) + ("...");
                            }
                            else
                            {
                                dis = newsdescription;
                            }

                        }
                        break;
                    case Option.Custom:
                        title = eve.Title;
                        dis = eve.ShortDescription;
                        redirectlink = eve.link;
                        break;
                    default:
                        break;
                }
                
              model.Slider.Add(new Models.Slider(){
                    Title = title,
                    PictureUrl = _pictureService.GetPictureUrl(eve.PictureId),
                    Id = eve.Id,
                    ShortDescription = dis,
                    link = redirectlink,
                    SlidePushtime=eve.SlidePushtime.ToString(),
                    SliderEffect = ((SlidingEffect)eve.SlidingEffect).ToString()
              });
            }
          
            //Bellow Code Use For Set Global Transition Speed 
          //  model.TransitionSpeed=Convert.ToInt32(Transitiontime);


            return View("~/Plugins/Widgets.Slider/Views/WidgetsSlider/PublicInfo.cshtml", model);



        }

        #endregion


        #region Public All Event Info


        public ActionResult AllSliderDetail()
        {
            var activeSliders = _sliderService.GetAllActiveSliders();


            var model = new SliderDetailModel();
            foreach (var eve in activeSliders)
            {
                model.Slider.Add(new Models.Slider()
                {
                    Title = eve.GetLocalized(x => x.Title),
                    ShortDescription = eve.GetLocalized(x => x.ShortDescription),
                    PictureUrl = _pictureService.GetPictureUrl(eve.PictureId),
                    Id = eve.Id,
                    link = eve.link
                });
            }
            return View("~/Plugins/Widgets.Slider/Views/WidgetsSlider/AllSliderDetail.cshtml", model);
        }

        #endregion
    }
}

