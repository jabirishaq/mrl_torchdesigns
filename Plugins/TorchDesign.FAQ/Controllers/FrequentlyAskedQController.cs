using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Nop.Core;
using Nop.Plugin.TorchDesign.FAQ.Services;
using Nop.Core.Domain.Directory;
using Nop.Plugin.TorchDesign.FAQ.Domain;
using Nop.Plugin.TorchDesign.FAQ.Models;
using Nop.Services.Configuration;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Kendoui;
using Nop.Web.Framework.Mvc;
using Nop.Plugin.TorchDesign.FAQ;
using Nop.Services.Stores;
using System.Xml;
using System.IO;

namespace Nop.Plugin.TorchDesign.FAQ.Controllers
{
    
    public class FrequentlyAskedQController : BasePluginController
    {
        private readonly IFrequentlyAskedQService _FAQService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly ISettingService _settingService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;


        public FrequentlyAskedQController(IFrequentlyAskedQService FAQService,ILanguageService languageService,
            ILocalizationService localizationService,ILocalizedEntityService localizedEntityService,
            ISettingService settingService, IStoreService storeService, IWorkContext workContext)
        {
            this._FAQService = FAQService;
            this._languageService = languageService;
            this._localizationService = localizationService;
            this._localizedEntityService = localizedEntityService;
            this._settingService = settingService;
            this._storeService = storeService;
            this._workContext = workContext;
            
        }
        
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            //little hack here
            //always set culture to 'en-US' (Telerik has a bug related to editing decimal values in other cultures). Like currently it's done for admin area in Global.asax.cs
            CommonHelper.SetTelerikCulture();

            base.Initialize(requestContext);
        }


        [HttpPost]
        public ActionResult FaqCategoryList(DataSourceRequest command, int faqId)
        {
           
                var faq = _FAQService.GetFAQById(faqId);


                var Faqcatmapping = _FAQService.GetAllFAQCategoryMappingByFaqId(faqId);
                var faqCategoriesModel = Faqcatmapping
                .Select(x =>
                {
                    return new ConfigurationModel.FaqCategoryMappingModel()
                    {
                        Id = x.Id,
                        Category = _FAQService.GetFAQCategoryById(x.CategoryId).CategoryName,
                        FaqId = x.FaqId,
                        CategoryId = x.CategoryId,
                        DisplayOrder = x.DisplayOrder
                    };
                })
                .ToList();

            var gridModel = new DataSourceResult
            {
                Data = faqCategoriesModel,
                Total = faqCategoriesModel.Count
            };

            return Json(gridModel);
        }

        [HttpPost]
        public ActionResult FaqCategoryInsert(ConfigurationModel.FaqCategoryMappingModel model, int faqId)
        {
           
            var categoryId = model.CategoryId;

            if (_FAQService.CategoryExistOrNot(model.CategoryId, faqId))
            {
                return new NullJsonResult();
            }
            else 
            {
                //var existingfaqCategories = _FAQService.GetFAQCategoryMappingById(categoryId);
                //if (existingfaqCategories.FindProductCategory(faqId, categoryId) == null)
                //{
                var faqCategory = new FAQ_Category_Mapping()
                {
                    FaqId = faqId,
                    CategoryId = categoryId,
                    DisplayOrder = model.DisplayOrder
                };

                _FAQService.InsertFAQCategoryMapping(faqCategory);
                ////}
            }

          

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult FaqCategoryUpdate(ConfigurationModel.FaqCategoryMappingModel model)
        {
         
            var faqCategorymap = _FAQService.GetFAQCategoryMappingById(model.Id);
            if (faqCategorymap == null)
                throw new ArgumentException("No product category mapping found with the specified id");

          
                //var product = _FAQService.GetFAQById(productCategory.ProductId);
                //if (product != null && product.VendorId != _workContext.CurrentVendor.Id)
                //{
                //    return Content("This is not your product");
                //}


            faqCategorymap.CategoryId = model.CategoryId;
            faqCategorymap.DisplayOrder = model.DisplayOrder;

            _FAQService.UpdateFAQCategoryMapping(faqCategorymap);

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult FaqCategoryDelete(int id)
        {

            var faqCategorymap = _FAQService.GetFAQCategoryMappingById(id);
            if (faqCategorymap == null)
                throw new ArgumentException("No product category mapping found with the specified id");

            var productId = faqCategorymap.FaqId;

            _FAQService.DeleteFAQCategoryMapping(faqCategorymap);

            return new NullJsonResult();
        }


       
        public ActionResult AddPopup()
        {
            var model = new ConfigurationModel();
            //model.AvailableCategories.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
            //foreach (var a in _FAQService.GetAllFAQCategory().Where(x=>x.Active==true))
            //{
            //    model.AvailableCategories.Add(new SelectListItem() { Text = a.CategoryName, Value = a.Id.ToString() });
            //}
            //AddLocales(_languageService, model.Locales);
            model.IsAddMode = false;

            return View("~/Plugins/TorchDesign.FAQs/Views/FrequentlyAskedQ/AddPopup.cshtml",model);
        }


        //[HttpPost, ActionName("AddPopup")]
        //[FormValueRequired("save")]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult AddPopup(string btnId, string formId, ConfigurationModel model, bool continueEditing)
        {

           
                var obj = new FrequentlyAskedQ
                {
                    Id = model.Id,
                    question = model.question,
                    description = model.description,
                    IsActive = model.IsActive,
                    DisplayOrder = model.DisplayOrder,


                };
                _FAQService.InsertFAQ(obj);
                //UpdateLocales(obj, model);
                var record = _FAQService.GetAllFAQs().Where(x => x.question == model.question).OrderBy(x=>x.Id).LastOrDefault();
                int newid = 0;
                if (record != null)
                {
                    newid = record.Id;
                }
            
           
            if (continueEditing)
            {
                return RedirectToAction("EditPopup", new { id = newid });
            }
            ViewBag.RefreshPage = true;
            ViewBag.btnId = "btnRefresh";
            ViewBag.formId = "Widgets-event-form";
                return View("~/Plugins/TorchDesign.FAQs/Views/FrequentlyAskedQ/AddPopup.cshtml", model);
         
            //return continueEditing ? RedirectToAction("EditPopup", new { id = newid }) : RedirectToAction("AddPopup");
            //return View("~/Plugins/TorchDesign.FAQs/Views/FrequentlyAskedQ/AddPopup.cshtml",model);
        }
        //[HttpPost, ActionName("AddPopup")]
        //[FormValueRequired("saveandcountinue")]
        //public ActionResult AddPopupwithcountinue(string btnId, string formId, ConfigurationModel model)
        //{


        //    var obj = new FrequentlyAskedQ
        //    {
               
        //        question = model.question,
        //        description = model.description,
        //        IsActive = model.IsActive,
        //        DisplayOrder = model.DisplayOrder,


        //    };
        //    _FAQService.InsertFAQ(obj);
        //   var record= _FAQService.GetAllFAQs().Where(x => x.question == model.question).LastOrDefault();
        //   int id = 0;
        //   if (record != null)
        //   {
        //        id = record.Id;
        //   }
           
        //    //UpdateLocales(obj, model);

        //    model.IsAddMode = true;
        //    model.Id=id;



        //    ViewBag.RefreshPage = false;
        //    ViewBag.btnId = btnId;
        //    ViewBag.formId = formId;
        //    return View("~/Plugins/TorchDesign.FAQs/Views/FrequentlyAskedQ/AddPopup.cshtml", model);
        //}

       
        public ActionResult EditPopup(int id)
        {



            var obj = _FAQService.GetFAQById(id);
            if (obj == null)
                //No record found with the specified id
                return RedirectToAction("Configure");
           
            var model = new ConfigurationModel
            {

                Id = obj.Id,
                question = obj.question,
                description = obj.description,
                DisplayOrder=obj.DisplayOrder,
                IsActive=obj.IsActive,
               

            };
            model.IsAddMode = true;



            //model.AvailableCategories.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
            //foreach (var a in _FAQService.GetAllFAQCategory().Where(x=>x.Active == true))
            //{
            //    model.AvailableCategories.Add(new SelectListItem() { Text = a.CategoryName, Value = a.Id.ToString() });
            //}
            //AddLocales(_languageService, model.Locales, (locale, languageId) =>
            //{
            //    locale.FullDescription = eventobj.GetLocalized(x => x.FullDescription, languageId, false, false);
            //    locale.ShortDescription = eventobj.GetLocalized(x => x.ShortDescription, languageId, false, false);
            //    locale.Title = eventobj.GetLocalized(x => x.Title, languageId, false, false);
            //});

            return View("~/Plugins/TorchDesign.FAQs/Views/FrequentlyAskedQ/EditPopup.cshtml", model);
        }


        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult EditPopup(string btnId, string formId, ConfigurationModel model, bool continueEditing)
        {


            var obj = _FAQService.GetFAQById(model.Id);
            if (obj == null)
                //No record found with the specified id
                return RedirectToAction("Configure");

            obj.Id = model.Id;
           
            obj.question = model.question;
            obj.description = model.description;
            obj.DisplayOrder = model.DisplayOrder;
            obj.IsActive = model.IsActive;
            
            _FAQService.UpdateFAQ(obj);

         
            if (continueEditing)
            {
                //selected tab


                return RedirectToAction("EditPopup", new { id = model.Id });
            }
            else
            {
                ViewBag.RefreshPage = true;
                ViewBag.btnId = "btnRefresh";
                ViewBag.formId = "Widgets-event-form";
                return View("~/Plugins/TorchDesign.FAQs/Views/FrequentlyAskedQ/EditPopup.cshtml", model);
            }
            //UpdateLocales(obj, model);

          
           
        }

       

        public ActionResult CategoryList()
        {

            CategoriesModel model = new CategoriesModel();
           
            return View("~/Plugins/TorchDesign.FAQs/Views/FrequentlyAskedQ/CategoryList.cshtml", model);
        }


        [HttpPost]
        public ActionResult UpdateFAQ(ConfigurationModel model)
        {
            var updateFAQ = _FAQService.GetFAQById(model.Id);
            if (updateFAQ == null)
                throw new ArgumentException("No FAQ found with the specified Id");


            //if (string.IsNullOrEmpty(model.Title) || string.IsNullOrEmpty(model.Description) || model.StartDate.ToString() == "" || model.EndDate.ToString() == "")
            //{
            //    ErrorNotification("Please fill all the field.");
            //    return new NullJsonResult();
            //}



            var FAQRecord = _FAQService.GetFAQById(model.Id);
            //FAQRecord.CategoryId = model.CategoryId;
            FAQRecord.question = model.question;
            FAQRecord.description = model.description;

            _FAQService.UpdateFAQ(FAQRecord);
            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult DeleteFAQ(int id)
        {
            var deleteFAQ = _FAQService.GetFAQById(id);
            if (deleteFAQ == null)
                throw new ArgumentException("No FAQ found with the specified Id");
            _FAQService.DeleteFAQ(deleteFAQ);
            return new NullJsonResult();
        }


        //[NonAction]
        //public void UpdateLocales(FrequentlyAskedQSetting FAQrecord, ConfigurationModel model)
        //{
        //    foreach (var localized in model.Locales)
        //    {
               
        //        _localizedEntityService.SaveLocalizedValue(FAQrecord,
        //                                                       x => x.title,
        //                                                       localized.title,
        //                                                       localized.LanguageId);
        //        _localizedEntityService.SaveLocalizedValue(FAQrecord,
        //                                                       x => x.displayOption,
        //                                                       localized.displayOption,
        //                                                       localized.LanguageId);
              

        //        search engine name
        //        var seName = product.ValidateSeName(localized.SeName, localized.Name, false);
        //        _urlRecordService.SaveSlug(eventrecord, localized.LanguageId);
        //    }
        //}
        //[NonAction]
        //public void UpdateLocales(FrequentlyAskedQ FAQrecord, ConfigurationModel model)
        //{
        //    foreach (var localized in model.Locales)
        //    {

        //        _localizedEntityService.SaveLocalizedValue(FAQrecord,
        //                                                       x => x.question,
        //                                                       localized.question,
        //                                                       localized.LanguageId);
        //        _localizedEntityService.SaveLocalizedValue(FAQrecord,
        //                                                       x => x.description,
        //                                                       localized.description,
        //                                                       localized.LanguageId);


                //search engine name
                //var seName = product.ValidateSeName(localized.SeName, localized.Name, false);
                //_urlRecordService.SaveSlug(eventrecord, localized.LanguageId);
        //    }
        //}

        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure()
        {
            //var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            //var FAQSettings = _settingService.LoadSetting<FAQSettings>(storeScope);
            //var model = new ConfigurationModel();

            //model.Title = FAQSettings.Title;
            ////model.DisplayOption = FAQSettings.DisplayOption;
            //model.DisplayAsWidget = FAQSettings.DisplayAsWidget;
            //if (storeScope > 0)
            //{
            //    model.Title_OverrideForStore = _settingService.SettingExists(FAQSettings, x => x.Title, storeScope);
            //    //model.DisplayOption_OverrideForStore = _settingService.SettingExists(FAQSettings, x => x.DisplayOption, storeScope);
            //    model.DisplayAsWidget_OverrideForStore = _settingService.SettingExists(FAQSettings, x => x.DisplayAsWidget, storeScope);
            //}


            //// Add widget zones from xml file
            //model.AvailableZones.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });

            //var ServerPathMap = Server.MapPath("~/Plugins/TorchDesign.FAQs/SupportedWidgetZones.xml");
            //XmlDataDocument xml = new XmlDataDocument();

            //xml.Load(ServerPathMap); //load xml 

            //XmlNodeList nodeList = xml.SelectNodes("/SupportedWidgetZones/WidgetZone");

            //foreach (XmlNode node in nodeList)
            //{
            //    var text = node["text"].InnerText;
            //    var value = node["value"].InnerText;
            //    model.AvailableZones.Add(new SelectListItem() { Text = text , Value = value });
                
            //}

            
            //model.AvailableOptions.Add(new SelectListItem() { Text = "-- Select Display Type --", Value = "0" });
            //model.AvailableOptions.Add(new SelectListItem() { Text = "Simple", Value = "1" });
            //model.AvailableOptions.Add(new SelectListItem() { Text = "Accordion", Value = "2" });
            //model.AvailableOptions.Add(new SelectListItem() { Text = "Expanded", Value = "3" });
            //model.AvailableOptions.Add(new SelectListItem() { Text = "Accordion Hover", Value = "4" });
            var model = new ConfigurationModel();

            return View("~/Plugins/TorchDesign.FAQs/Views/FrequentlyAskedQ/Configure.cshtml", model);

        }

        [HttpPost]
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure(ConfigurationModel model)
        {
            //var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            //var FAQSettings = _settingService.LoadSetting<FAQSettings>(storeScope);
            //FAQSettings.Title = model.Title;
            ////FAQSettings.DisplayOption = model.DisplayOption;
            //FAQSettings.DisplayOn = model.DisplayOn;
            //FAQSettings.DisplayAsWidget = model.DisplayAsWidget;

            ///* We do not clear cache after each setting update.
            //* This behavior can increase performance because cached settings will not be cleared 
            //* and loaded from database after each update */
            //if (model.Title_OverrideForStore || storeScope == 0)
            //    _settingService.SaveSetting(FAQSettings, x => x.Title, storeScope, false);
            //else if (storeScope > 0)
            //    _settingService.DeleteSetting(FAQSettings, x => x.Title, storeScope);

            ////if (model.DisplayOption_OverrideForStore || storeScope == 0)
            ////    _settingService.SaveSetting(FAQSettings, x => x.DisplayOption, storeScope, false);
            ////else if (storeScope > 0)
            ////    _settingService.DeleteSetting(FAQSettings, x => x.DisplayOption, storeScope);

            //if (model.DisplayOn_OverrideForStore || storeScope == 0)
            //    _settingService.SaveSetting(FAQSettings, x => x.DisplayOn, storeScope, false);
            //else if (storeScope > 0)
            //    _settingService.DeleteSetting(FAQSettings, x => x.DisplayOn, storeScope);


            //if (model.DisplayAsWidget_OverrideForStore || storeScope == 0)
            //    _settingService.SaveSetting(FAQSettings, x => x.DisplayAsWidget, storeScope, false);
            //else if (storeScope > 0)
            //    _settingService.DeleteSetting(FAQSettings, x => x.DisplayAsWidget, storeScope);

            ////now clear settings cache
            //_settingService.ClearCache();



            return Configure();
        }

        [NonAction]
        public string GetCategoryName(int CategoryId)
        {
            var category = _FAQService.GetFAQCategoryById(CategoryId);
            if (category != null)
            {
                return category.CategoryName;
            }
            return string.Empty;

        }

         
        [HttpPost]
        public ActionResult FAQList(DataSourceRequest command, ConfigurationModel model)
        {

           
            var FAQs = _FAQService.GetAllFAQs(command.Page - 1, command.PageSize);
            var FAQModel = FAQs.Select(x =>
            {
                var m = new ConfigurationModel()
                {
                    Id = x.Id,
                    //CategoryName = GetCategoryName(x.CategoryId),
                    question = x.question,
                    description = x.description,
                    DisplayOrder=x.DisplayOrder,
                    IsActive=x.IsActive,
                   
                };
                return m;
            })
              .ToList();
            var gridModel = new DataSourceResult
            {
                Data = FAQModel,
                Total = FAQs.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        public ActionResult CatagoryInsert(CategoriesModel model)
        {

            var Newcat = new FAQ_Category()
            {
                CategoryName = model.CategoryName,
                Active = model.Active,
               DisplayOrder=model.DisplayOrder,
            };

            _FAQService.InsertFAQCategory(Newcat);

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult CategoryUpdate(CategoriesModel model)
        {
          
            var Catagory= _FAQService.GetFAQCategoryById(model.Id);

            Catagory.Active = model.Active;
            Catagory.CategoryName = model.CategoryName;
            Catagory.DisplayOrder = model.DisplayOrder;
            
            _FAQService.UpdateFAQCategory(Catagory);
            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult CategoryList(DataSourceRequest command, int optionId)
        {

            var catagorylist = _FAQService.GetAllFAQCategory();
            var catagorylistModel = catagorylist
                .Select(x =>
                {
                    return new CategoriesModel()
                    {
                        Id = x.Id,
                        CategoryName = x.CategoryName,
                        Active = x.Active,
                        DisplayOrder=x.DisplayOrder,
                    };
                })
                .ToList();

            var gridModel = new DataSourceResult
            {
                Data = catagorylistModel,
                Total = catagorylistModel.Count
            };

            return Json(gridModel);
        }

        #region Public Info

      
        public ActionResult PublicInfo()
        {

            var model = new FAQ_Model();

            var fqCategories = _FAQService.GetAllFAQCategory().Where(x=>x.Active == true);
                
                foreach (var fqCategory in fqCategories)
                {
                    var qandaofCategory = _FAQService.GetFAQByCategoryId(fqCategory.Id).Where(x => x.IsActive == true);
                    if (qandaofCategory.Count() > 0)
                    { 
                    var fqCategoryModel = new CategoriesModel();
                    fqCategoryModel.CategoryName = fqCategory.CategoryName;
                    fqCategoryModel.Id = fqCategory.Id;

                    //var qandaofCategory = _FAQService.GetFAQByCategoryId(fqCategory.Id).Where(x => x.IsActive == true);
                    foreach (var qa in qandaofCategory)
                    {
                        var qandAModel = new QandAModel();

                        qandAModel.description = qa.description;
                        qandAModel.Id = qa.Id;
                        qandAModel.question = qa.question;

                        fqCategoryModel.AvailableQandAByCategory.Add(qandAModel);

                    }

                    model.AllCategories.Add(fqCategoryModel);
                    }

                }
            

            //var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            //var FAQSettings = _settingService.LoadSetting<FAQSettings>(storeScope);

           // model.Title = FAQSettings.Title;
            //model.DisplayOption = FAQSettings.DisplayOption;

                var qAndAnswerswithoutCategories = _FAQService.GetAllFAQWithoutCategotyMapping().Where(x => x.IsActive == true); ;

            foreach (var qa in qAndAnswerswithoutCategories)
            {
                var qandAModel = new QandAModel();

                qandAModel.description = qa.description;
                qandAModel.Id = qa.Id;
                qandAModel.question = qa.question;

                model.RemainedQandA.Add(qandAModel);
            }

          
           
            return View("~/Plugins/TorchDesign.FAQs/Views/FrequentlyAskedQ/PublicInfo.cshtml",model);
        }


        public ActionResult FAQPage()
        {

            var model = new FAQ_Model();

            var fqCategories = _FAQService.GetAllFAQCategory();

            foreach (var fqCategory in fqCategories)
            {
                var fqCategoryModel = new CategoriesModel();
                fqCategoryModel.CategoryName = fqCategory.CategoryName;
                fqCategoryModel.Id = fqCategory.Id;

                var qandaofCategory = _FAQService.GetFAQByCategoryId(fqCategory.Id);
                foreach (var qa in qandaofCategory)
                {
                    var qandAModel = new QandAModel();

                    qandAModel.description = qa.description;
                    qandAModel.Id = qa.Id;
                    qandAModel.question = qa.question;

                    fqCategoryModel.AvailableQandAByCategory.Add(qandAModel);

                }

                model.AllCategories.Add(fqCategoryModel);


            }

            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var FAQSettings = _settingService.LoadSetting<FAQSettings>(storeScope);

            model.Title = FAQSettings.Title;
            //model.DisplayOption = FAQSettings.DisplayOption;

           


            return View("~/Plugins/TorchDesign.FAQs/Views/FrequentlyAskedQ/FAQPage.cshtml", model);
        }

        #endregion
    }
}
