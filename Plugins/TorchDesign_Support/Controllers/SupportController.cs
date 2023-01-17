using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Catalog;
using Nop.Services.Stores;
using Nop.Services.Vendors;
using Nop.Services.Security;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Kendoui;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework;
using Nop.Plugin.Widgets.TorchDesign_Support.Models;

using Nop.Plugin.Widgets.TorchDesign_Support.Domain;
using Nop.Plugin.Widgets.TorchDesign_Support.Services;
using Nop.Core.Data;
using System.Data;
using Nop.Plugin.Widgets.TorchDesign_Support.Data;
using System.Text.RegularExpressions;
using Nop.Plugin.Widgets.TorchDesign_Support;
using Nop.Services.Configuration;
using Nop.Services.Seo;
using Nop.Core.Domain;

namespace Nop.Plugin.Widgets.TorchDesign_Support.Controllers
{
    public class SupportController : BasePluginController
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly ISupportService _supportService;
        private readonly IPictureService _pictureService;
        private readonly ICategoryService _categoryService;
        private readonly IPermissionService _permissionService;
        private readonly IWorkContext _workContext;
        private readonly IManufacturerService _manufacturerService;
        private readonly ISettingService _settingService;
        private readonly IStoreService _storeService;
        private readonly IVendorService _vendorService;
        private readonly IProductService _productService;
        private readonly IProductServiceCustome _productservicecustome;
        private readonly IDataProvider _dataProvider;
        private readonly SupportObjectContext _supportcontext;
        private readonly IDownloadService _downloadService;
        private readonly IUrlRecordService _seoservice;

        #endregion

        #region Constructors

        public SupportController(ILocalizationService LocalizationService,
                                        ICustomerActivityService CustomerActivityService,
                                        ISupportService SupportService,
            IPictureService PictureService,
            ICategoryService CategoryService,
            IPermissionService PermissionService,
            IWorkContext WorkContext,
            IManufacturerService ManufacturerService,
            ISettingService settingService,
            IStoreService StoreService,
            IVendorService VendorService,
            IProductService ProductService, IProductServiceCustome productservicecustome, IDataProvider dataProvider, SupportObjectContext supportcontext, IDownloadService downloadService, IUrlRecordService seoservice)
        {
            this._localizationService = LocalizationService;
            this._customerActivityService = CustomerActivityService;
            this._supportService = SupportService;
            this._pictureService = PictureService;
            this._categoryService = CategoryService;
            this._permissionService = PermissionService;
            this._workContext = WorkContext;
            this._manufacturerService = ManufacturerService;
            this._settingService = settingService;
            this._storeService = StoreService;
            this._vendorService = VendorService;
            this._productService = ProductService;
            this._productservicecustome = productservicecustome;
            this._dataProvider = dataProvider;
            this._supportcontext = supportcontext;
            this._downloadService = downloadService;
            this._seoservice = seoservice;
        }

        #endregion

        #region Support Category

        public ActionResult SupportCategoryList()
        {

            //return View("SupportCategoryList");
            return View("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/SupportCategoryList.cshtml");
        }

        [HttpPost]
        public ActionResult SupportCategoryList(DataSourceRequest command)
        {

            var supportcategories = _supportService.GetAllSupportCategory();
            var gridModel = new DataSourceResult
            {
                Data = supportcategories.Select(x =>
                {
                    var supportcategoryModel = new SupportCategoryModel();
                    supportcategoryModel.Id = x.Id;
                    supportcategoryModel.Title = x.Title;
                    supportcategoryModel.ShowInFooter = x.ShowInFooter;
                    supportcategoryModel.PictureId = x.PictureId;
                    supportcategoryModel.Description = x.Description;
                    return supportcategoryModel;
                }),
                Total = supportcategories.Count()
            };
            return Json(gridModel);
        }

        #endregion

        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure()
        {
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var Supportsetting = _settingService.LoadSetting<Nop.Plugin.Widgets.TorchDesign_Support.TorchDesignSupportSettings>(storeScope);
            var model = new ConfigurationModel();
            model.SupportEnabled = Supportsetting.SupportEnabled;
            if (storeScope > 0)
            {
                model.SupportEnabled_OverrideForStore = _settingService.SettingExists(Supportsetting, x => x.SupportEnabled, storeScope);

            }
            return View("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/Configure.cshtml", model);
        }

        [HttpPost, ActionName("Configure")]
        [FormValueRequired("Savesettingbtn")]
        public ActionResult Savesetting(ConfigurationModel model)
        {
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var Supportsetting = _settingService.LoadSetting<Nop.Plugin.Widgets.TorchDesign_Support.TorchDesignSupportSettings>(storeScope);
            Supportsetting.SupportEnabled = model.SupportEnabled;


            if (model.SupportEnabled_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(Supportsetting, x => x.SupportEnabled, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(Supportsetting, x => x.SupportEnabled, storeScope);


            _settingService.ClearCache();

            return View("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/Configure.cshtml", model);
        }

        #region Create / Edit / Delete	Support Category

        public ActionResult CreateSupportCategory()
        {
            var model = new SupportCategoryModel();
            model.IsActive = true;
            var categories = _categoryService.GetAllCategories(showHidden: true);
            foreach (var c in categories)
            {
                model.AvailableCategories.Add(new SelectListItem()
                {
                    Text = c.GetFormattedBreadCrumb(categories),
                    Value = c.Id.ToString()
                });
            }
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageSupport))
            //	return AccessDeniedView();
            return View("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/CreateSupportCategory.cshtml", model);
            //return View(model);


        }


        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult CreateSupportCategory(SupportCategoryModel model, bool continueEditing)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageSupport))
            //	return AccessDeniedView();
            //if (model.SelectedProductCategoryIds == null)
            //{
            //    ModelState.AddModelError("", "Please select at least one product category");
            //}
            if (ModelState.IsValid)
            {

                SupportCategory supportcategory = new SupportCategory();
                supportcategory.Title = model.Title;
                supportcategory.ShowInFooter = model.ShowInFooter;
                supportcategory.PictureId = model.PictureId;
                supportcategory.Description = model.Description;
                supportcategory.IsActive = true;
                supportcategory.ShowInFooter = false;


                var allProductCategories = _categoryService.GetAllCategories();
                var newTopicProductCategory = new List<SelectListItem>();
                foreach (var productCategory in allProductCategories)
                    if (model.SelectedProductCategoryIds != null && model.SelectedProductCategoryIds.Contains(productCategory.Id))
                    {
                        newTopicProductCategory.Add(new SelectListItem()
                        {
                            Text = productCategory.GetFormattedBreadCrumb(allProductCategories),
                            Value = productCategory.Id.ToString()
                        });
                    }
                foreach (var topicProductcategory in newTopicProductCategory)
                {
                    SupportCategoryProductCategory supportCategoryProductCategory = new SupportCategoryProductCategory();
                    supportCategoryProductCategory.CategoryId = int.Parse(topicProductcategory.Value);
                    supportcategory.SupportCategoryProductCategories.Add(supportCategoryProductCategory);
                }
                _supportService.InsertSupportCategory(supportcategory);

                //search engine name
                model.Title = supportcategory.ValidateSeName(model.Title, supportcategory.Title, true);
                _seoservice.SaveSlug(supportcategory, model.Title, 0);

                var productcategories = _categoryService.GetAllCategories();

                var categories = _categoryService.GetAllCategories(showHidden: true);
                foreach (var c in categories)
                {
                    model.AvailableCategories.Add(new SelectListItem()
                    {
                        Text = c.GetFormattedBreadCrumb(categories),
                        Value = c.Id.ToString()
                    });
                }
                SuccessNotification(_localizationService.GetResource("Admin.Support.SupportCategories.Added"));
                return continueEditing ? RedirectToAction("EditSupportCategory", new { id = supportcategory.Id }) : RedirectToAction("SupportCategoryList");
            }

            var categorieslist = _categoryService.GetAllCategories(showHidden: true);
            foreach (var c in categorieslist)
            {
                model.AvailableCategories.Add(new SelectListItem()
                {
                    Text = c.GetFormattedBreadCrumb(categorieslist),
                    Value = c.Id.ToString()
                });
            }
            return View("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/CreateSupportCategory.cshtml", model);

            //return View(model);
        }

        public ActionResult EditSupportCategory(int id)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageSupport))
            //	return AccessDeniedView();

            var supportcategory = _supportService.GetSupportCategoryById(id);
            if (supportcategory == null)
                //No support category found with the specified id
                return RedirectToAction("SupportCategoryList");

            var model = new SupportCategoryModel();
            model.Id = supportcategory.Id;
            model.Title = supportcategory.Title;
            model.Description = supportcategory.Description;
            model.PictureId = supportcategory.PictureId;
            model.IsActive = supportcategory.IsActive;
            model.PictureUrl = _pictureService.GetPictureUrl(supportcategory.PictureId);
            model.ShowInFooter = supportcategory.ShowInFooter;

            var categories = _categoryService.GetAllCategories(showHidden: true);
            foreach (var c in categories)
            {
                model.AvailableCategories.Add(new SelectListItem()
                {
                    Text = c.GetFormattedBreadCrumb(categories),
                    Value = c.Id.ToString()
                });
            }

            model.SelectedProductCategoryIds = supportcategory.SupportCategoryProductCategories.Select(x => x.CategoryId).ToArray();
            // return View(model);
            return View("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/EditSupportCategory.cshtml", model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult EditSupportCategory(SupportCategoryModel model, bool continueEditing)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageSupport))
            //	return AccessDeniedView();

            var supportcategory = _supportService.GetSupportCategoryById(model.Id);
            if (supportcategory == null)
                //No category found with the specified id
                return RedirectToAction("SupportCategoryList");

            var allProductCategories = _categoryService.GetAllCategories();
            var newTopicProductCategory = new List<SelectListItem>();
            foreach (var productCategory in allProductCategories)
                if (model.SelectedProductCategoryIds != null && model.SelectedProductCategoryIds.Contains(productCategory.Id))
                {
                    newTopicProductCategory.Add(new SelectListItem()
                    {
                        Text = productCategory.GetFormattedBreadCrumb(allProductCategories),
                        Value = productCategory.Id.ToString()
                    });
                }
            //if (model.SelectedProductCategoryIds == null)
            //{
            //    ModelState.AddModelError("", "Please select at least one product category");
            //}

            if (ModelState.IsValid)
            {
                int prevPictureId = supportcategory.PictureId;
                //delete an old picture (if deleted or updated)
                if (prevPictureId > 0 && model.PictureId != prevPictureId)
                {
                    var prevPicture = _pictureService.GetPictureById(prevPictureId);
                    if (prevPicture != null)
                        _pictureService.DeletePicture(prevPicture);
                }


                foreach (var productCategory in allProductCategories)
                {


                    if (model.SelectedProductCategoryIds != null &&
                          model.SelectedProductCategoryIds.Contains(productCategory.Id))
                    {


                        if (supportcategory.SupportCategoryProductCategories.Count(cr => cr.CategoryId == productCategory.Id) == 0)
                        {

                            SupportCategoryProductCategory supportCategoryProductCategory = new SupportCategoryProductCategory();
                            supportCategoryProductCategory.CategoryId = productCategory.Id;
                            supportcategory.SupportCategoryProductCategories.Add(supportCategoryProductCategory);
                        }
                    }
                    else
                    {

                        if (supportcategory.SupportCategoryProductCategories.Count(cr => cr.CategoryId == productCategory.Id) > 0)
                        {
                            SupportCategoryProductCategory supportCategoryProductCategory = new SupportCategoryProductCategory(); ;
                            supportCategoryProductCategory = supportcategory.SupportCategoryProductCategories.Where(x => x.CategoryId == productCategory.Id).FirstOrDefault();
                            _supportService.DeleteSupportCategoryProductCategory(supportCategoryProductCategory);


                        }
                    }

                }

                supportcategory.PictureId = model.PictureId;
                supportcategory.ShowInFooter = model.ShowInFooter;
                supportcategory.Title = model.Title;
                //supportcategory.IsActive = model.IsActive;
                supportcategory.Description = model.Description;
                _supportService.UpdateSupportCategory(supportcategory);

                //search engine name
                model.Title = supportcategory.ValidateSeName(model.Title, supportcategory.Title, true);
                _seoservice.SaveSlug(supportcategory, model.Title, 0);

                //activity log
                _customerActivityService.InsertActivity("EditSupportCategory", _localizationService.GetResource("ActivityLog.EditSupportCategory"), supportcategory.Title);
                SuccessNotification(_localizationService.GetResource("Admin.Support.SupportCategories.Updated"));
                if (continueEditing)
                {
                    //selected tab
                    SaveSelectedTabIndex();

                    return RedirectToAction("EditSupportCategory", supportcategory.Id);
                }
                return RedirectToAction("SupportCategoryList");
            }


            var categories = _categoryService.GetAllCategories(showHidden: true);
            foreach (var c in categories)
            {
                model.AvailableCategories.Add(new SelectListItem()
                {
                    Text = c.GetFormattedBreadCrumb(categories),
                    Value = c.Id.ToString()
                });
            }



            // return View(model);
            return View("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/EditSupportCategory.cshtml", model);
        }

        public ActionResult SupportCategoryDelete(int id)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageSupport))
            //	return AccessDeniedView();

            var supportcategory = _supportService.GetSupportCategoryById(id);
            if (supportcategory == null)
            {
                return Json(new { Result = false }, JsonRequestBehavior.AllowGet);
            }
            else
            {

                var supportTopicsupportCategory = _supportService.GetAllSupportTopicSupportCategoryBySupportCategoryId(id);

                foreach (var item in supportTopicsupportCategory)
                {
                    _supportService.DeleteSupportTopicSupportCategory(item);

                    //var suppoTopicStep = _supportService.GetAllSupportTopicStepBySupportTopicId(topicitem.Id);
                    //if (suppoTopicStep != null)
                    //{
                    //    foreach (var item in suppoTopicStep)
                    //    {
                    //        _supportService.DeleteSupportTopicStep(item);
                    //    }

                    //}
                }

                //activity log
                _supportService.DeleteSupportCategory(supportcategory);
                _customerActivityService.InsertActivity("DeleteSupportCategory", _localizationService.GetResource("ActivityLog.DeleteSupportTopic"), supportcategory.Title);

                SuccessNotification(_localizationService.GetResource("Admin.Support.SupportCategory.Deleted"));
                return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
            }//return Json(new { Result = true, url = Session["current"].ToString() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Support Topic

        public ActionResult SupportTopicList()
        {
            // return View("SupportTopicList");
            return View("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/SupportTopicList.cshtml");
        }

        [HttpPost]
        public ActionResult SupportTopicList(DataSourceRequest command)
        {


            var supportTopics = _supportService.GetAllSupportTopics();
            var gridModel = new DataSourceResult
            {
                Data = supportTopics.Select(x =>
                {
                      //var supportcategoryModel = x.ToModel();
                      var supportTopicModel = new SupportTopicModel();
                    supportTopicModel.Id = x.Id;
                    supportTopicModel.Title = x.Title;
                      // supportTopicModel.Description = x.Description;
                      return supportTopicModel;
                }),
                Total = supportTopics.Count()
            };
            return Json(gridModel);
        }

        #endregion Support Topic

        #region SupportTopic Step

        public ActionResult AddStep(int pictureId, string title, string description, int topicid, int StepNo)
        {

            if (pictureId == 0)
                throw new ArgumentException();

            if (title == null)
                throw new ArgumentException();



            var supporttopic = _supportService.GetSupportTopicById(topicid);
            if (supporttopic == null)
                throw new ArgumentException("No supporttopic found with the specified id");

            var allDisplayOrders = _supportService.GetAllSupportTopicStep();
            int newdisplayorder = 0;

            if (allDisplayOrders.Count > 0)
            {
                newdisplayorder = allDisplayOrders.Select(x => x.DisplayOrder).Max();
            }

            SupportTopicStep supporttopicstep = new SupportTopicStep();
            supporttopicstep.PictureId = pictureId;
            supporttopicstep.SupportStepNo = StepNo;
            supporttopicstep.DisplayOrder = newdisplayorder + 1;
            supporttopicstep.Title = title;
            supporttopicstep.SupportTopicId = topicid;
            supporttopicstep.Description = description;

            _supportService.InsertSupportTopicStep(supporttopicstep);

            return Json(new { Result = true }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult SupportTopicStepUpdate(int id)
        {
            var supportstep = _supportService.GetSupportTopicStepById(id);
            var model = new SupportTopicStepModel();
            model.PictureId = supportstep.PictureId;
            model.DisplayOrder = supportstep.DisplayOrder;
            model.Title = supportstep.Title;
            model.Description = supportstep.Description;

            return View("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/SupportTopicStepUpdate.cshtml", model);

        }


        [HttpPost]
        public ActionResult SupportTopicStepUpdate(string btnId, string formId, SupportTopicStepModel model)
        {

            var supportstep = _supportService.GetSupportTopicStepById(model.Id);

            supportstep.PictureId = model.PictureId;
            supportstep.Title = model.Title;
            supportstep.Description = model.Description;

            if (supportstep == null)
                throw new ArgumentException("No Step found with the specified id");

            ViewBag.RefreshPage = true;
            ViewBag.btnId = btnId;
            ViewBag.formId = formId;
            _supportService.UpdateSupportTopicStep(supportstep);
            return View("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/SupportTopicStepUpdate.cshtml", model);

        }


        public ActionResult SupportTopicStepDelete(int id)
        {
            var suportstep = _supportService.GetSupportTopicStepById(id);
            int oldsupportnumber = suportstep.SupportStepNo;
            var upersuportstep = _supportService.GetAllSupportTopicStepBySupportTopicId(suportstep.SupportTopicId).Where(x => x.DisplayOrder > suportstep.DisplayOrder).OrderBy(x => x.DisplayOrder);

            if (suportstep == null)
                return Json(new { Result = false }, JsonRequestBehavior.AllowGet);

            //if (upersuportstep.Count() == 0)
            //    return Json(new { Result = false }, JsonRequestBehavior.AllowGet);


            if (upersuportstep.Count() == 0)
            {
                var availablestep = _supportService.GetAllSupportTopicStepBySupportTopicId(suportstep.SupportTopicId);

                if (availablestep.Count >= 2)
                {
                    _supportService.DeleteSupportTopicStep(suportstep);
                }
                else
                {
                    return Json(new { Result = false }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                foreach (var item in upersuportstep)
                {

                    var lowerstep = _supportService.GetSupportTopicStepById(item.Id);
                    lowerstep.SupportStepNo = lowerstep.SupportStepNo - 1;
                    _supportService.UpdateSupportTopicStep(lowerstep);
                }

                _supportService.DeleteSupportTopicStep(suportstep);
            }

            return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Moveup(int id, int topicid)
        {
            if (id > 0)
            {
                var supportstep = _supportService.GetSupportTopicStepById(id);
                int olddisplayorder = supportstep.DisplayOrder;
                int oldsupportnumber = supportstep.SupportStepNo;
                var upersuportstep = _supportService.GetAllSupportTopicStepBySupportTopicId(topicid).Where(x => x.DisplayOrder < supportstep.DisplayOrder).OrderBy(x => x.DisplayOrder).LastOrDefault();
                if (upersuportstep == null)
                    return Json(new { Result = false }, JsonRequestBehavior.AllowGet);



                supportstep.DisplayOrder = upersuportstep.DisplayOrder;
                supportstep.SupportStepNo = upersuportstep.SupportStepNo;
                _supportService.UpdateSupportTopicStep(supportstep);

                upersuportstep.DisplayOrder = olddisplayorder;
                upersuportstep.SupportStepNo = oldsupportnumber;
                _supportService.UpdateSupportTopicStep(upersuportstep);


                return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {

                return Json(new { Result = false }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Movedown(int id, int topicid)
        {
            if (id > 0)
            {
                var supportstep = _supportService.GetSupportTopicStepById(id);
                int olddisplayorder = supportstep.DisplayOrder;
                int oldsupportnumber = supportstep.SupportStepNo;
                var upersuportstep = _supportService.GetAllSupportTopicStepBySupportTopicId(topicid).Where(x => x.DisplayOrder > supportstep.DisplayOrder).OrderBy(x => x.DisplayOrder).FirstOrDefault();
                if (upersuportstep == null)
                    return Json(new { Result = false }, JsonRequestBehavior.AllowGet);



                supportstep.DisplayOrder = upersuportstep.DisplayOrder;
                supportstep.SupportStepNo = upersuportstep.SupportStepNo;
                _supportService.UpdateSupportTopicStep(supportstep);

                upersuportstep.DisplayOrder = olddisplayorder;
                upersuportstep.SupportStepNo = oldsupportnumber;
                _supportService.UpdateSupportTopicStep(upersuportstep);


                return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {

                return Json(new { Result = false }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Create / Edit / Delete	Support Topic

        public ActionResult CreateSupportTopic(string redirect = null)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageSupport))
            //	return AccessDeniedView();


            Session["urlredirect"] = Request.UrlReferrer;

            var model = new SupportTopicModel();
            model.AddSupportTopicStepModel.SupportStepNo = 1;
            model.redirect = redirect;
            model.AddSupportTopicStepModel.Title = "Step 1";
            var supportcategories = _supportService.GetAllSupportCategory();
            model.AvailableSupportCategories = supportcategories.Select(x =>
                {
                        //var supportcategoryModel = x.ToModel();
                        var supportcategoryModel = new SupportCategoryModel();
                    supportcategoryModel.Id = x.Id;
                    supportcategoryModel.Title = x.Title;
                    supportcategoryModel.ShowInFooter = x.ShowInFooter;
                    supportcategoryModel.PictureId = x.PictureId;
                    supportcategoryModel.Description = x.Description;
                    return supportcategoryModel;
                }).ToList();
            var productcategories = _categoryService.GetAllCategories();

            var categories = _categoryService.GetAllCategories(showHidden: true);
            foreach (var c in categories)
            {
                model.AvailableCategories.Add(new SelectListItem()
                {
                    Text = c.GetFormattedBreadCrumb(categories),
                    Value = c.Id.ToString()
                });
            }

            SupportTopicStepModel step = new SupportTopicStepModel()
            {
                SupportStepNo = 1,
                Title = "Step 1",


            };
            model.CompareCount = 0;
            model.AvailableTopicStep.Add(step);

            //return View(model);
            return View("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/CreateSupportTopic.cshtml", model);
        }
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult CreateSupportTopic(SupportTopicModel model, bool continueEditing)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageSupport))
            //	return AccessDeniedView();
            if (model.AvailableTopicStep.Count() < 2)
            {

                //validate support categories
                var allSupportCategories = _supportService.GetAllSupportCategory();
                var newTopicSupportCategory = new List<SupportCategory>();
                foreach (var supportCategory in allSupportCategories)
                    if (model.SelectedSupportCategoryIds != null && model.SelectedSupportCategoryIds.Contains(supportCategory.Id))
                        newTopicSupportCategory.Add(supportCategory);

                var allProductCategories = _categoryService.GetAllCategories();
                var newTopicProductCategory = new List<SelectListItem>();
                foreach (var productCategory in allProductCategories)
                    if (model.SelectedProductCategoryIds != null && model.SelectedProductCategoryIds.Contains(productCategory.Id))
                    {
                        newTopicProductCategory.Add(new SelectListItem()
                        {
                            Text = productCategory.GetFormattedBreadCrumb(allProductCategories),
                            Value = productCategory.Id.ToString()
                        });
                    }



                if (model.SelectedSupportCategoryIds == null)
                {
                    ModelState.AddModelError("", "Please select at least one support category");
                }
                if (!String.IsNullOrEmpty(model.Description))
                {
                    string acceptable = "b|i";
                    string html = model.Description;
                    string stringPattern = @"</?(?(?=" + acceptable + @")notag|[a-zA-Z0-9]+)(?:\s[a-zA-Z0-9\-]+=?(?:(["",']?).*?\1?)?)*\s*/?>";
                    string filterhtml = Regex.Replace(html, stringPattern, "");

                    if (html.Length != filterhtml.Length)
                    {
                        ModelState.AddModelError("", "Html Tags Are Not Allow in Description");
                    }
                }
                foreach (var item in model.AvailableTopicStep)
                {
                    if (String.IsNullOrEmpty(item.Title))
                    {
                        ModelState.AddModelError("", "Step Title field is required");
                    }
                    if (item.PictureId == 0)
                    {
                        ModelState.AddModelError("", "Step Picture field is required");
                    }
                }


                if (ModelState.IsValid)
                {
                    // var supportcategory = model.ToEntity();
                    SupportTopic supportTopic = new SupportTopic();
                    supportTopic.Title = model.Title;
                    supportTopic.Description = model.Description;
                    foreach (var topicsupportcategory in newTopicSupportCategory)
                    {
                        SupportTopicSupportCategory supportTopicSupportCategory = new SupportTopicSupportCategory();
                        supportTopicSupportCategory.SupportCategoryId = topicsupportcategory.Id;
                        supportTopic.SupportTopicSupportCategory.Add(supportTopicSupportCategory);
                    }

                    foreach (var topicProductcategory in newTopicProductCategory)
                    {
                        SupportTopicProductCategory supportTopicProductCategory = new SupportTopicProductCategory();
                        supportTopicProductCategory.CategoryId = int.Parse(topicProductcategory.Value);
                        supportTopic.SupportTopicProductCategories.Add(supportTopicProductCategory);
                    }



                    //SupportTopicStep supportTopicStep = new SupportTopicStep();
                    //supportTopicStep.Title = model.AddSupportTopicStepModel.Title;
                    //supportTopicStep.Description = model.AddSupportTopicStepModel.Description;
                    //supportTopicStep.PictureId = model.AddSupportTopicStepModel.PictureId;
                    //SupportTopicStepMapping supportTopicStepMapping = new SupportTopicStepMapping();
                    //supportTopicStepMapping.TemplateId = model.AddSupportTopicStepModel.TemplateId;
                    //supportTopicStepMapping.SupportTopicStep = supportTopicStep;

                    //supportTopic.SupportTopicStepMapping.Add(supportTopicStepMapping);
                    supportTopic.TemplateId = model.TemplateId;
                    _supportService.InsertSupportTopic(supportTopic);
                    //search engine name
                    model.Title = supportTopic.ValidateSeName(model.Title, supportTopic.Title, true);
                    _seoservice.SaveSlug(supportTopic, model.Title, 0);


                    foreach (var item in model.AvailableTopicStep)
                    {
                        var allDisplayOrders = _supportService.GetAllSupportTopicStep();
                        int newdisplayorder = 0;

                        if (allDisplayOrders.Count > 0)
                        {
                            newdisplayorder = allDisplayOrders.Select(x => x.DisplayOrder).Max() + 1;
                        }
                        else
                        {
                            newdisplayorder++;
                        }

                        SupportTopicStep supporttopicstep = new SupportTopicStep();
                        supporttopicstep.PictureId = item.PictureId;
                        supporttopicstep.SupportStepNo = item.SupportStepNo;
                        supporttopicstep.DisplayOrder = newdisplayorder;
                        supporttopicstep.Title = item.Title;
                        supporttopicstep.SupportTopicId = supportTopic.Id;
                        supporttopicstep.Description = item.Description;

                        _supportService.InsertSupportTopicStep(supporttopicstep);
                        item.Id = supporttopicstep.Id;
                    }


                    //activity log
                    //_customerActivityService.InsertActivity("AddNewCategory", _localizationService.GetResource("ActivityLog.AddNewCategory"), category.Name);

                    SuccessNotification(_localizationService.GetResource("Admin.Support.SupportTopic.Added"));
                    if (continueEditing)
                    {
                        //Session["savecontinue"] = "true";
                        return RedirectToAction("EditSupportTopic", new { id = supportTopic.Id, redirect = model.redirect });
                    }
                    if (!String.IsNullOrEmpty(model.redirect))
                    {
                        //var newurl = "~/" + (model.redirect.Contains("-") ? model.redirect.Replace("-", "/") : model.redirect);
                        var newurl = Session["urlredirect"];

                        return Redirect(Url.Content(newurl.ToString()));
                    }
                    //if (!String.IsNullOrEmpty(model.redirect))
                    //{
                    //    var newurl = "~/" + model.redirect;
                    //    return Redirect(Url.Content(newurl));
                    //}

                    return RedirectToAction("SupportTopicList");
                }

                var supportcategories = _supportService.GetAllSupportCategory();
                model.AvailableSupportCategories = supportcategories.Select(x =>
                {
                      //var supportcategoryModel = x.ToModel();
                      var supportcategoryModel = new SupportCategoryModel();
                    supportcategoryModel.Id = x.Id;
                    supportcategoryModel.Title = x.Title;
                    supportcategoryModel.ShowInFooter = x.ShowInFooter;
                    supportcategoryModel.PictureId = x.PictureId;
                    supportcategoryModel.Description = x.Description;
                    return supportcategoryModel;
                }).ToList();
                var productcategories = _categoryService.GetAllCategories();

                var categories = _categoryService.GetAllCategories(showHidden: true);
                foreach (var c in categories)
                {
                    model.AvailableCategories.Add(new SelectListItem()
                    {
                        Text = c.GetFormattedBreadCrumb(categories),
                        Value = c.Id.ToString()
                    });
                }

                //return View(model);
                return View("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/CreateSupportTopic.cshtml", model);
            }
            else
            {
                int td_topicid = _supportService.GetSupportTopicStepById(model.AvailableTopicStep.FirstOrDefault().Id).SupportTopicId;
                model.Id = td_topicid;
                var supportTopic = _supportService.GetSupportTopicById(model.Id);
                if (supportTopic == null)
                    //No category found with the specified id
                    return RedirectToAction("SupportTopicList");

                var allSupportCategories = _supportService.GetAllSupportCategory();
                var newTopicSupportCategory = new List<SupportCategory>();
                foreach (var supportCategory in allSupportCategories)
                    if (model.SelectedSupportCategoryIds != null && model.SelectedSupportCategoryIds.Contains(supportCategory.Id))
                        newTopicSupportCategory.Add(supportCategory);

                var allProductCategories = _categoryService.GetAllCategories();
                var newTopicProductCategory = new List<SelectListItem>();
                foreach (var productCategory in allProductCategories)
                    if (model.SelectedProductCategoryIds != null && model.SelectedProductCategoryIds.Contains(productCategory.Id))
                    {
                        newTopicProductCategory.Add(new SelectListItem()
                        {
                            Text = productCategory.GetFormattedBreadCrumb(allProductCategories),
                            Value = productCategory.Id.ToString()
                        });
                    }

                if (model.SelectedSupportCategoryIds == null)
                {
                    ModelState.AddModelError("", "Please select at least one support category");

                }
                if (!String.IsNullOrEmpty(model.Description))
                {
                    string acceptable = "b|i";

                    string html = model.Description;
                    string stringPattern = @"</?(?(?=" + acceptable + @")notag|[a-zA-Z0-9]+)(?:\s[a-zA-Z0-9\-]+=?(?:(["",']?).*?\1?)?)*\s*/?>";
                    string filterhtml = Regex.Replace(html, stringPattern, "");

                    if (html.Length != filterhtml.Length)
                    {
                        ModelState.AddModelError("", "Html Tags Are Not Allow in Description");
                    }
                }
                foreach (var item in model.AvailableTopicStep)
                {
                    if (String.IsNullOrEmpty(item.Title))
                    {
                        ModelState.AddModelError("", "Step Title field is required");
                    }
                    if (item.PictureId == 0)
                    {
                        ModelState.AddModelError("", "Step Picture field is required");
                    }
                }

                // var supportcategory = model.ToEntity();
                supportTopic.Title = model.Title;
                supportTopic.Description = model.Description;
                supportTopic.TemplateId = model.TemplateId;

                //foreach (var topicsupportcategory in newTopicSupportCategory)
                //{
                //	SupportTopicSupportCategory supportTopicSupportCategory = new SupportTopicSupportCategory();
                //	supportTopicSupportCategory.SupportCategoryId = topicsupportcategory.Id;
                //	supportTopic.SupportTopicSupportCategory.Add(supportTopicSupportCategory);
                //}

                //foreach (var topicProductcategory in newTopicProductCategory)
                //{
                //	SupportTopicProductCategory supportTopicProductCategory = new SupportTopicProductCategory();
                //	supportTopicProductCategory.CategoryId = int.Parse(topicProductcategory.Value);
                //	supportTopic.SupportTopicProductCategories.Add(supportTopicProductCategory);
                //}


                if (ModelState.IsValid)
                {
                    foreach (var productCategory in allProductCategories)
                    {


                        if (model.SelectedProductCategoryIds != null &&
                              model.SelectedProductCategoryIds.Contains(productCategory.Id))
                        {


                            if (supportTopic.SupportTopicProductCategories.Count(cr => cr.CategoryId == productCategory.Id) == 0)
                            {

                                SupportTopicProductCategory supportTopicProductCategory = new SupportTopicProductCategory();
                                supportTopicProductCategory.CategoryId = productCategory.Id;
                                supportTopic.SupportTopicProductCategories.Add(supportTopicProductCategory);
                            }
                        }
                        else
                        {

                            if (supportTopic.SupportTopicProductCategories.Count(cr => cr.CategoryId == productCategory.Id) > 0)
                            {
                                SupportTopicProductCategory supportTopicProductCategory = new SupportTopicProductCategory(); ;
                                supportTopicProductCategory = supportTopic.SupportTopicProductCategories.Where(x => x.CategoryId == productCategory.Id).FirstOrDefault();
                                _supportService.DeleteSupportTopicProductCategory(supportTopicProductCategory);


                            }
                        }

                    }


                    foreach (var supportCategory in allSupportCategories)
                    {


                        if (model.SelectedSupportCategoryIds != null &&
                              model.SelectedSupportCategoryIds.Contains(supportCategory.Id))
                        {


                            if (supportTopic.SupportTopicSupportCategory.Count(cr => cr.SupportCategoryId == supportCategory.Id) == 0)
                            {

                                SupportTopicSupportCategory supportTopicSupportCategory = new SupportTopicSupportCategory();
                                supportTopicSupportCategory.SupportCategoryId = supportCategory.Id;
                                supportTopic.SupportTopicSupportCategory.Add(supportTopicSupportCategory);
                            }
                        }
                        else
                        {

                            if (supportTopic.SupportTopicSupportCategory.Count(cr => cr.SupportCategoryId == supportCategory.Id) > 0)
                            {
                                SupportTopicSupportCategory supportTopicSupportCategory = new SupportTopicSupportCategory(); ;
                                supportTopicSupportCategory = supportTopic.SupportTopicSupportCategory.Where(x => x.SupportCategoryId == supportCategory.Id).FirstOrDefault();
                                _supportService.DeleteSupportTopicSupportCategory(supportTopicSupportCategory);


                            }
                        }

                    }

                    foreach (var item in model.AvailableTopicStep)
                    {

                        var step = _supportService.GetSupportTopicStepById(item.Id);
                        if (step == null)
                        {
                            var avilablestep = _supportService.GetAllSupportTopicStepBySupportTopicId(model.Id);

                            var allDisplayOrders = _supportService.GetAllSupportTopicStep();
                            int newdisplayorder = 0;

                            if (allDisplayOrders.Count > 0)
                            {
                                newdisplayorder = allDisplayOrders.Select(x => x.DisplayOrder).Max();
                            }

                            SupportTopicStep supporttopicstep = new SupportTopicStep();
                            supporttopicstep.PictureId = item.PictureId;
                            supporttopicstep.SupportStepNo = avilablestep.Count() + 1;
                            supporttopicstep.DisplayOrder = newdisplayorder + 1;
                            supporttopicstep.Title = item.Title;
                            supporttopicstep.SupportTopicId = model.Id;
                            supporttopicstep.Description = item.Description;

                            _supportService.InsertSupportTopicStep(supporttopicstep);
                            item.Id = supporttopicstep.Id;
                        }
                        else
                        {
                            step.PictureId = item.PictureId;
                            step.Title = item.Title;
                            step.Description = item.Description;

                            _supportService.UpdateSupportTopicStep(step);
                        }

                    }

                    _supportService.UpdateSupportTopic(supportTopic);
                    //search engine name
                    model.Title = supportTopic.ValidateSeName(model.Title, supportTopic.Title, true);
                    _seoservice.SaveSlug(supportTopic, model.Title, 0);

                    //activity log
                    _customerActivityService.InsertActivity("EditSupportTopic", _localizationService.GetResource("ActivityLog.EditSupportTopic"), supportTopic.Title);

                    SuccessNotification(_localizationService.GetResource("Admin.Support.SupportTopic.Updated"));
                    if (continueEditing)
                    {
                        //selected tab
                        //Session["savecontinue"] = "true";
                        SaveSelectedTabIndex();
                        return RedirectToAction("EditSupportTopic", new { id = supportTopic.Id, redirect = model.redirect });
                        //return RedirectToAction("EditSupportTopic", (supportTopic.Id ,model.redirect));
                    }
                    if (model.redirect == "edit")
                    {
                        var newurl = Session["editurlredirect"];
                        return Redirect(Url.Content(newurl.ToString()));
                    }
                    if (!String.IsNullOrEmpty(model.redirect))
                    {
                        var newurl = Session["urlredirect"];

                        return Redirect(Url.Content(newurl.ToString()));
                    }
                    //if (model.redirect > 0)
                    //{
                    //    //return RedirectToAction("SupportTopicPublic", supportTopic.Id);
                    //    return RedirectToRoute("Public.SupportTopicSingle", new { suportTopicId = supportTopic.Id });
                    //}

                    return RedirectToAction("SupportTopicList");
                }
                var supportcategories = _supportService.GetAllSupportCategory();
                model.AvailableSupportCategories = supportcategories.Select(x =>
                {
                      //var supportcategoryModel = x.ToModel();
                      var supportcategoryModel = new SupportCategoryModel();
                    supportcategoryModel.Id = x.Id;
                    supportcategoryModel.Title = x.Title;
                    supportcategoryModel.ShowInFooter = x.ShowInFooter;
                    supportcategoryModel.PictureId = x.PictureId;
                    supportcategoryModel.Description = x.Description;
                    return supportcategoryModel;
                }).ToList();
                var productcategories = _categoryService.GetAllCategories();
                var categories = _categoryService.GetAllCategories(showHidden: true);
                foreach (var c in categories)
                {
                    model.AvailableCategories.Add(new SelectListItem()
                    {
                        Text = c.GetFormattedBreadCrumb(categories),
                        Value = c.Id.ToString()
                    });
                }




                //return View(model);
                return View("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/EditSupportTopic.cshtml", model);
            }

        }

        public ActionResult EditSupportTopic(int id, string redirect = null)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageSupport))
            //	return AccessDeniedView();
            //if (Session["savecontinue"] == null) { 
            //Session["current"] = Request.UrlReferrer.OriginalString;
            //}

            if (Session["editurlredirect"] == null && redirect == "edit")
            {
                Session["editurlredirect"] = Request.UrlReferrer;
            }
            var sportsteps = _supportService.GetAllSupportTopicStepBySupportTopicId(id);
            var supportTopic = _supportService.GetSupportTopicById(id);
            if (supportTopic == null)
                //No support category found with the specified id
                return RedirectToAction("SupportTopicList");

            var model = new SupportTopicModel();
            model.Id = supportTopic.Id;
            model.Title = supportTopic.Title;
            model.redirect = redirect;
            model.Description = supportTopic.Description;
            model.TemplateId = supportTopic.TemplateId;
            if (sportsteps.Count() > 0)
            {
                model.AddSupportTopicStepModel.Title = "Step " + (sportsteps.Count() + 1);
                model.AddSupportTopicStepModel.SupportStepNo = sportsteps.Count() + 1;
            }

            var supportcategories = _supportService.GetAllSupportCategory();
            model.AvailableSupportCategories = supportcategories.Select(x =>
            {
                  //var supportcategoryModel = x.ToModel();
                  var supportcategoryModel = new SupportCategoryModel();
                supportcategoryModel.Id = x.Id;
                supportcategoryModel.Title = x.Title;
                supportcategoryModel.ShowInFooter = x.ShowInFooter;
                supportcategoryModel.PictureId = x.PictureId;
                supportcategoryModel.Description = x.Description;
                return supportcategoryModel;
            }).ToList();

            model.SelectedSupportCategoryIds = supportTopic.SupportTopicSupportCategory.Select(x => x.SupportCategoryId).ToArray();
            var categories = _categoryService.GetAllCategories(showHidden: true);
            foreach (var c in categories)
            {
                model.AvailableCategories.Add(new SelectListItem()
                {
                    Text = c.GetFormattedBreadCrumb(categories),
                    Value = c.Id.ToString()
                });
            }

            model.SelectedProductCategoryIds = supportTopic.SupportTopicProductCategories.Select(x => x.CategoryId).ToArray();


            var supporttopicStep = _supportService.GetAllSupportTopicStepBySupportTopicId(id);
            if (supporttopicStep.Count() > 0)
            {
                model.CompareCount = supporttopicStep.Count();
            }
            else
            {
                model.CompareCount = 0;
            }

            foreach (var item in supporttopicStep)
            {
                var supportTopicStep = new SupportTopicStepModel();
                supportTopicStep.Id = item.Id;

                supportTopicStep.SupportStepNo = item.SupportStepNo;
                supportTopicStep.Title = item.Title;
                supportTopicStep.Description = item.Description;
                supportTopicStep.PictureId = item.PictureId;
                supportTopicStep.PictureThumbnailUrl = _pictureService.GetPictureUrl(item.PictureId, 198);

                model.AvailableTopicStep.Add(supportTopicStep);
            }


            // return View(model);
            return View("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/EditSupportTopic.cshtml", model);
        }
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult EditSupportTopic(SupportTopicModel model, bool continueEditing)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageSupport))
            //  return AccessDeniedView();


            var supportTopic = _supportService.GetSupportTopicById(model.Id);
            if (supportTopic == null)
                //No category found with the specified id
                return RedirectToAction("SupportTopicList");

            var allSupportCategories = _supportService.GetAllSupportCategory();
            var newTopicSupportCategory = new List<SupportCategory>();
            foreach (var supportCategory in allSupportCategories)
                if (model.SelectedSupportCategoryIds != null && model.SelectedSupportCategoryIds.Contains(supportCategory.Id))
                    newTopicSupportCategory.Add(supportCategory);

            var allProductCategories = _categoryService.GetAllCategories();
            var newTopicProductCategory = new List<SelectListItem>();
            foreach (var productCategory in allProductCategories)
                if (model.SelectedProductCategoryIds != null && model.SelectedProductCategoryIds.Contains(productCategory.Id))
                {
                    newTopicProductCategory.Add(new SelectListItem()
                    {
                        Text = productCategory.GetFormattedBreadCrumb(allProductCategories),
                        Value = productCategory.Id.ToString()
                    });
                }

            if (model.SelectedSupportCategoryIds == null)
            {
                ModelState.AddModelError("", "Please select at least one support category");

            }
            if (!String.IsNullOrEmpty(model.Description))
            {
                string acceptable = "b|i";

                string html = model.Description;
                string stringPattern = @"</?(?(?=" + acceptable + @")notag|[a-zA-Z0-9]+)(?:\s[a-zA-Z0-9\-]+=?(?:(["",']?).*?\1?)?)*\s*/?>";
                string filterhtml = Regex.Replace(html, stringPattern, "");

                if (html.Length != filterhtml.Length)
                {
                    ModelState.AddModelError("", "Html Tags Are Not Allow in Description");
                }
            }
            foreach (var item in model.AvailableTopicStep)
            {
                if (String.IsNullOrEmpty(item.Title))
                {
                    ModelState.AddModelError("", "Step Title field is required");
                }
                if (item.PictureId == 0)
                {
                    ModelState.AddModelError("", "Step Picture field is required");
                }
            }

            // var supportcategory = model.ToEntity();
            supportTopic.Title = model.Title;
            supportTopic.Description = model.Description;
            supportTopic.TemplateId = model.TemplateId;

            //foreach (var topicsupportcategory in newTopicSupportCategory)
            //{
            //	SupportTopicSupportCategory supportTopicSupportCategory = new SupportTopicSupportCategory();
            //	supportTopicSupportCategory.SupportCategoryId = topicsupportcategory.Id;
            //	supportTopic.SupportTopicSupportCategory.Add(supportTopicSupportCategory);
            //}

            //foreach (var topicProductcategory in newTopicProductCategory)
            //{
            //	SupportTopicProductCategory supportTopicProductCategory = new SupportTopicProductCategory();
            //	supportTopicProductCategory.CategoryId = int.Parse(topicProductcategory.Value);
            //	supportTopic.SupportTopicProductCategories.Add(supportTopicProductCategory);
            //}


            if (ModelState.IsValid)
            {
                foreach (var productCategory in allProductCategories)
                {


                    if (model.SelectedProductCategoryIds != null &&
                          model.SelectedProductCategoryIds.Contains(productCategory.Id))
                    {


                        if (supportTopic.SupportTopicProductCategories.Count(cr => cr.CategoryId == productCategory.Id) == 0)
                        {

                            SupportTopicProductCategory supportTopicProductCategory = new SupportTopicProductCategory();
                            supportTopicProductCategory.CategoryId = productCategory.Id;
                            supportTopic.SupportTopicProductCategories.Add(supportTopicProductCategory);
                        }
                    }
                    else
                    {

                        if (supportTopic.SupportTopicProductCategories.Count(cr => cr.CategoryId == productCategory.Id) > 0)
                        {
                            SupportTopicProductCategory supportTopicProductCategory = new SupportTopicProductCategory(); ;
                            supportTopicProductCategory = supportTopic.SupportTopicProductCategories.Where(x => x.CategoryId == productCategory.Id).FirstOrDefault();
                            _supportService.DeleteSupportTopicProductCategory(supportTopicProductCategory);


                        }
                    }

                }


                foreach (var supportCategory in allSupportCategories)
                {


                    if (model.SelectedSupportCategoryIds != null &&
                          model.SelectedSupportCategoryIds.Contains(supportCategory.Id))
                    {


                        if (supportTopic.SupportTopicSupportCategory.Count(cr => cr.SupportCategoryId == supportCategory.Id) == 0)
                        {

                            SupportTopicSupportCategory supportTopicSupportCategory = new SupportTopicSupportCategory();
                            supportTopicSupportCategory.SupportCategoryId = supportCategory.Id;
                            supportTopic.SupportTopicSupportCategory.Add(supportTopicSupportCategory);
                        }
                    }
                    else
                    {

                        if (supportTopic.SupportTopicSupportCategory.Count(cr => cr.SupportCategoryId == supportCategory.Id) > 0)
                        {
                            SupportTopicSupportCategory supportTopicSupportCategory = new SupportTopicSupportCategory(); ;
                            supportTopicSupportCategory = supportTopic.SupportTopicSupportCategory.Where(x => x.SupportCategoryId == supportCategory.Id).FirstOrDefault();
                            _supportService.DeleteSupportTopicSupportCategory(supportTopicSupportCategory);


                        }
                    }

                }

                foreach (var item in model.AvailableTopicStep)
                {

                    var step = _supportService.GetSupportTopicStepById(item.Id);
                    if (step == null)
                    {
                        var avilablestep = _supportService.GetAllSupportTopicStepBySupportTopicId(model.Id);

                        var allDisplayOrders = _supportService.GetAllSupportTopicStep();
                        int newdisplayorder = 0;

                        if (allDisplayOrders.Count > 0)
                        {
                            newdisplayorder = allDisplayOrders.Select(x => x.DisplayOrder).Max();
                        }

                        SupportTopicStep supporttopicstep = new SupportTopicStep();
                        supporttopicstep.PictureId = item.PictureId;
                        supporttopicstep.SupportStepNo = avilablestep.Count() + 1;
                        supporttopicstep.DisplayOrder = newdisplayorder + 1;
                        supporttopicstep.Title = item.Title;
                        supporttopicstep.SupportTopicId = model.Id;
                        supporttopicstep.Description = item.Description;

                        _supportService.InsertSupportTopicStep(supporttopicstep);
                        item.Id = supporttopicstep.Id;

                    }
                    else
                    {
                        step.PictureId = item.PictureId;
                        step.Title = item.Title;
                        step.Description = item.Description;

                        _supportService.UpdateSupportTopicStep(step);
                    }

                }

                _supportService.UpdateSupportTopic(supportTopic);
                //search engine name
                model.Title = supportTopic.ValidateSeName(model.Title, supportTopic.Title, true);
                _seoservice.SaveSlug(supportTopic, model.Title, 0);

                //activity log
                _customerActivityService.InsertActivity("EditSupportTopic", _localizationService.GetResource("ActivityLog.EditSupportTopic"), supportTopic.Title);

                SuccessNotification(_localizationService.GetResource("Admin.Support.SupportTopic.Updated"));
                if (continueEditing)
                {
                    //selected tab
                    //Session["savecontinue"] = "true";
                    SaveSelectedTabIndex();
                    return RedirectToAction("EditSupportTopic", new { id = supportTopic.Id, redirect = model.redirect });
                    //return RedirectToAction("EditSupportTopic", (supportTopic.Id ,model.redirect));
                }
                if (model.redirect == "edit")
                {
                    var newurl = Session["editurlredirect"];
                    return Redirect(Url.Content(newurl.ToString()));
                }
                if (!String.IsNullOrEmpty(model.redirect))
                {
                    var newurl = Session["urlredirect"];

                    return Redirect(Url.Content(newurl.ToString()));
                }
                //if (model.redirect > 0)
                //{
                //    //return RedirectToAction("SupportTopicPublic", supportTopic.Id);
                //    return RedirectToRoute("Public.SupportTopicSingle", new { suportTopicId = supportTopic.Id });
                //}

                return RedirectToAction("SupportTopicList");
            }
            var supportcategories = _supportService.GetAllSupportCategory();
            model.AvailableSupportCategories = supportcategories.Select(x =>
            {
                  //var supportcategoryModel = x.ToModel();
                  var supportcategoryModel = new SupportCategoryModel();
                supportcategoryModel.Id = x.Id;
                supportcategoryModel.Title = x.Title;
                supportcategoryModel.ShowInFooter = x.ShowInFooter;
                supportcategoryModel.PictureId = x.PictureId;
                supportcategoryModel.Description = x.Description;
                return supportcategoryModel;
            }).ToList();
            var productcategories = _categoryService.GetAllCategories();
            var categories = _categoryService.GetAllCategories(showHidden: true);
            foreach (var c in categories)
            {
                model.AvailableCategories.Add(new SelectListItem()
                {
                    Text = c.GetFormattedBreadCrumb(categories),
                    Value = c.Id.ToString()
                });
            }




            //return View(model);
            return View("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/EditSupportTopic.cshtml", model);
        }

        [HttpGet]
        public ActionResult EditSupportTopicAjax(int id)
        {
            SupportTopicModel model = new SupportTopicModel();

            var supporttopicStep = _supportService.GetAllSupportTopicStepBySupportTopicId(id);

            foreach (var item in supporttopicStep)
            {
                var supportTopicStep = new SupportTopicStepModel();
                supportTopicStep.Id = item.Id;
                supportTopicStep.SupportStepNo = item.SupportStepNo;
                //supportTopicStep.Title = item.Title;
                //supportTopicStep.Description = item.Description;
                //supportTopicStep.PictureId = item.PictureId;
                //supportTopicStep.PictureThumbnailUrl = _pictureService.GetPictureUrl(item.PictureId, 198);

                model.AvailableTopicStep.Add(supportTopicStep);
            }

            return PartialView("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/SupportTopicAjax.cshtml", model);
        }

        [HttpPost]
        public ActionResult EditSupportTopicAjax(SupportTopicModel model)
        {

            var supportTopic = _supportService.GetSupportTopicById(model.Id);
            if (supportTopic == null)
                //No category found with the specified id
                return RedirectToAction("SupportTopicList");

            var allSupportCategories = _supportService.GetAllSupportCategory();
            var newTopicSupportCategory = new List<SupportCategory>();
            foreach (var supportCategory in allSupportCategories)
                if (model.SelectedSupportCategoryIds != null && model.SelectedSupportCategoryIds.Contains(supportCategory.Id))
                    newTopicSupportCategory.Add(supportCategory);

            var allProductCategories = _categoryService.GetAllCategories();
            var newTopicProductCategory = new List<SelectListItem>();
            foreach (var productCategory in allProductCategories)
                if (model.SelectedProductCategoryIds != null && model.SelectedProductCategoryIds.Contains(productCategory.Id))
                {
                    newTopicProductCategory.Add(new SelectListItem()
                    {
                        Text = productCategory.GetFormattedBreadCrumb(allProductCategories),
                        Value = productCategory.Id.ToString()
                    });
                }

            string error = "";

            if (model.SelectedSupportCategoryIds == null)
            {
                error = error + "Please select at least one support category.";
                ModelState.AddModelError("", "Please select at least one support category");

            }
            if (!String.IsNullOrEmpty(model.Description))
            {
                string acceptable = "b|i";

                string html = model.Description;
                string stringPattern = @"</?(?(?=" + acceptable + @")notag|[a-zA-Z0-9]+)(?:\s[a-zA-Z0-9\-]+=?(?:(["",']?).*?\1?)?)*\s*/?>";
                string filterhtml = Regex.Replace(html, stringPattern, "");

                if (html.Length != filterhtml.Length)
                {
                    error = error + "    Html Tags Are Not Allow in Description.";

                    ModelState.AddModelError("", "Html Tags Are Not Allow in Description");
                }
            }
            foreach (var item in model.AvailableTopicStep)
            {
                if (String.IsNullOrEmpty(item.Title))
                {
                    error = error + "   Step Title field is required.";
                    ModelState.AddModelError("", "Step Title field is required");
                }
                if (item.PictureId == 0)
                {
                    error = error + "   Step Picture field is required.";
                    ModelState.AddModelError("", "Step Picture field is required");
                }
            }

            // var supportcategory = model.ToEntity();
            supportTopic.Title = model.Title;
            supportTopic.Description = model.Description;
            supportTopic.TemplateId = model.TemplateId;

            //foreach (var topicsupportcategory in newTopicSupportCategory)
            //{
            //	SupportTopicSupportCategory supportTopicSupportCategory = new SupportTopicSupportCategory();
            //	supportTopicSupportCategory.SupportCategoryId = topicsupportcategory.Id;
            //	supportTopic.SupportTopicSupportCategory.Add(supportTopicSupportCategory);
            //}

            //foreach (var topicProductcategory in newTopicProductCategory)
            //{
            //	SupportTopicProductCategory supportTopicProductCategory = new SupportTopicProductCategory();
            //	supportTopicProductCategory.CategoryId = int.Parse(topicProductcategory.Value);
            //	supportTopic.SupportTopicProductCategories.Add(supportTopicProductCategory);
            //}


            if (ModelState.IsValid)
            {
                foreach (var productCategory in allProductCategories)
                {


                    if (model.SelectedProductCategoryIds != null &&
                          model.SelectedProductCategoryIds.Contains(productCategory.Id))
                    {


                        if (supportTopic.SupportTopicProductCategories.Count(cr => cr.CategoryId == productCategory.Id) == 0)
                        {

                            SupportTopicProductCategory supportTopicProductCategory = new SupportTopicProductCategory();
                            supportTopicProductCategory.CategoryId = productCategory.Id;
                            supportTopic.SupportTopicProductCategories.Add(supportTopicProductCategory);
                        }
                    }
                    else
                    {

                        if (supportTopic.SupportTopicProductCategories.Count(cr => cr.CategoryId == productCategory.Id) > 0)
                        {
                            SupportTopicProductCategory supportTopicProductCategory = new SupportTopicProductCategory(); ;
                            supportTopicProductCategory = supportTopic.SupportTopicProductCategories.Where(x => x.CategoryId == productCategory.Id).FirstOrDefault();
                            _supportService.DeleteSupportTopicProductCategory(supportTopicProductCategory);


                        }
                    }

                }


                foreach (var supportCategory in allSupportCategories)
                {


                    if (model.SelectedSupportCategoryIds != null &&
                          model.SelectedSupportCategoryIds.Contains(supportCategory.Id))
                    {


                        if (supportTopic.SupportTopicSupportCategory.Count(cr => cr.SupportCategoryId == supportCategory.Id) == 0)
                        {

                            SupportTopicSupportCategory supportTopicSupportCategory = new SupportTopicSupportCategory();
                            supportTopicSupportCategory.SupportCategoryId = supportCategory.Id;
                            supportTopic.SupportTopicSupportCategory.Add(supportTopicSupportCategory);
                        }
                    }
                    else
                    {

                        if (supportTopic.SupportTopicSupportCategory.Count(cr => cr.SupportCategoryId == supportCategory.Id) > 0)
                        {
                            SupportTopicSupportCategory supportTopicSupportCategory = new SupportTopicSupportCategory(); ;
                            supportTopicSupportCategory = supportTopic.SupportTopicSupportCategory.Where(x => x.SupportCategoryId == supportCategory.Id).FirstOrDefault();
                            _supportService.DeleteSupportTopicSupportCategory(supportTopicSupportCategory);


                        }
                    }

                }

                foreach (var item in model.AvailableTopicStep)
                {

                    var step = _supportService.GetSupportTopicStepById(item.Id);
                    if (step == null)
                    {
                        var avilablestep = _supportService.GetAllSupportTopicStepBySupportTopicId(model.Id);

                        var allDisplayOrders = _supportService.GetAllSupportTopicStep();
                        int newdisplayorder = 0;

                        if (allDisplayOrders.Count > 0)
                        {
                            newdisplayorder = allDisplayOrders.Select(x => x.DisplayOrder).Max();
                        }

                        SupportTopicStep supporttopicstep = new SupportTopicStep();
                        supporttopicstep.PictureId = item.PictureId;
                        supporttopicstep.SupportStepNo = avilablestep.Count() + 1;
                        supporttopicstep.DisplayOrder = newdisplayorder + 1;
                        supporttopicstep.Title = item.Title;
                        supporttopicstep.SupportTopicId = model.Id;
                        supporttopicstep.Description = item.Description;

                        _supportService.InsertSupportTopicStep(supporttopicstep);
                        item.Id = supporttopicstep.Id;
                    }
                    else
                    {
                        step.PictureId = item.PictureId;
                        step.Title = item.Title;
                        step.Description = item.Description;

                        _supportService.UpdateSupportTopicStep(step);
                    }

                }

                _supportService.UpdateSupportTopic(supportTopic);
                //search engine name
                model.Title = supportTopic.ValidateSeName(model.Title, supportTopic.Title, true);
                _seoservice.SaveSlug(supportTopic, model.Title, 0);


                model.CompareCount = _supportService.GetAllSupportTopicStepBySupportTopicId(model.Id).Count();


                //for (int i = 0; i < model.AvailableTopicStep.Count(); i++)
                //{
                //    if (model.AvailableTopicStep[i].Title == null && model.AvailableTopicStep[i].PictureId == 0)
                //    {
                //        model.AvailableTopicStep.RemoveAt(i);

                //    }
                //}

                int a = model.AvailableTopicStep.Count();

                SupportTopicStepModel newstep = new SupportTopicStepModel()
                {
                    SupportStepNo = (a + 1),
                    Title = "Step " + (a + 1),


                };
                model.AvailableTopicStep.Add(newstep);
                //activity log
                _customerActivityService.InsertActivity("EditSupportTopic", _localizationService.GetResource("ActivityLog.EditSupportTopic"), supportTopic.Title);
                ViewBag.Error = error;
                SuccessNotification(_localizationService.GetResource("Admin.Support.SupportTopic.Updated"));
                return PartialView("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/SupportTopicAjax.cshtml", model);

            }
            //var supportcategories = _supportService.GetAllSupportCategory();
            //model.AvailableSupportCategories = supportcategories.Select(x =>
            //{
            //    //var supportcategoryModel = x.ToModel();
            //    var supportcategoryModel = new SupportCategoryModel();
            //    supportcategoryModel.Id = x.Id;
            //    supportcategoryModel.Title = x.Title;
            //    supportcategoryModel.ShowInFooter = x.ShowInFooter;
            //    supportcategoryModel.PictureId = x.PictureId;
            //    supportcategoryModel.Description = x.Description;
            //    return supportcategoryModel;
            //}).ToList();
            //var productcategories = _categoryService.GetAllCategories();
            //var categories = _categoryService.GetAllCategories(showHidden: true);
            //foreach (var c in categories)
            //{
            //    model.AvailableCategories.Add(new SelectListItem()
            //    {
            //        Text = c.GetFormattedBreadCrumb(categories),
            //        Value = c.Id.ToString()
            //    });
            //}
            ViewBag.Error = error;
            return PartialView("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/SupportTopicAjax.cshtml", model);
            //return Json(new { Result = false, ErrorMsg = error }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult RemovenullrecordAjax(SupportTopicModel model)
        {
            ModelState.Clear();
            model.CompareCount = _supportService.GetAllSupportTopicStepBySupportTopicId(model.Id).Count();
            int newstepno = 1;
            for (int i = 0; i < model.AvailableTopicStep.Count(); i++)
            {
                if (model.AvailableTopicStep[i].Title == null && model.AvailableTopicStep[i].PictureId == 0)
                {
                    model.AvailableTopicStep.RemoveAt(i);
                }

            }
            foreach (var item in model.AvailableTopicStep)
            {
                item.SupportStepNo = newstepno;
                newstepno++;
            }

            return PartialView("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/SupportTopicAjax.cshtml", model);
        }

        public ActionResult AddSupportTopicAjax(SupportTopicModel model)
        {
            model.CompareCount = 0;
            if (model.AvailableTopicStep.Count() < 2)
            {

                var allSupportCategories = _supportService.GetAllSupportCategory();
                var newTopicSupportCategory = new List<SupportCategory>();
                foreach (var supportCategory in allSupportCategories)
                    if (model.SelectedSupportCategoryIds != null && model.SelectedSupportCategoryIds.Contains(supportCategory.Id))
                        newTopicSupportCategory.Add(supportCategory);

                var allProductCategories = _categoryService.GetAllCategories();
                var newTopicProductCategory = new List<SelectListItem>();
                foreach (var productCategory in allProductCategories)
                    if (model.SelectedProductCategoryIds != null && model.SelectedProductCategoryIds.Contains(productCategory.Id))
                    {
                        newTopicProductCategory.Add(new SelectListItem()
                        {
                            Text = productCategory.GetFormattedBreadCrumb(allProductCategories),
                            Value = productCategory.Id.ToString()
                        });
                    }



                string error = "";

                if (model.SelectedSupportCategoryIds == null)
                {
                    error = error + "Please select at least one support category.";
                    ModelState.AddModelError("", "Please select at least one support category");

                }
                if (!String.IsNullOrEmpty(model.Description))
                {
                    string acceptable = "b|i";

                    string html = model.Description;
                    string stringPattern = @"</?(?(?=" + acceptable + @")notag|[a-zA-Z0-9]+)(?:\s[a-zA-Z0-9\-]+=?(?:(["",']?).*?\1?)?)*\s*/?>";
                    string filterhtml = Regex.Replace(html, stringPattern, "");

                    if (html.Length != filterhtml.Length)
                    {
                        error = error + "    Html Tags Are Not Allow in Description.";

                        ModelState.AddModelError("", "Html Tags Are Not Allow in Description");
                    }
                }
                foreach (var item in model.AvailableTopicStep)
                {
                    if (String.IsNullOrEmpty(item.Title))
                    {
                        error = error + "   Step Title field is required.";
                        ModelState.AddModelError("", "Step Title field is required");
                    }
                    if (item.PictureId == 0)
                    {
                        error = error + "   Step Picture field is required.";
                        ModelState.AddModelError("", "Step Picture field is required");
                    }
                }


                if (ModelState.IsValid)
                {
                    // var supportcategory = model.ToEntity();
                    SupportTopic supportTopic = new SupportTopic();
                    supportTopic.Title = model.Title;
                    supportTopic.Description = model.Description;
                    foreach (var topicsupportcategory in newTopicSupportCategory)
                    {
                        SupportTopicSupportCategory supportTopicSupportCategory = new SupportTopicSupportCategory();
                        supportTopicSupportCategory.SupportCategoryId = topicsupportcategory.Id;
                        supportTopic.SupportTopicSupportCategory.Add(supportTopicSupportCategory);
                    }

                    foreach (var topicProductcategory in newTopicProductCategory)
                    {
                        SupportTopicProductCategory supportTopicProductCategory = new SupportTopicProductCategory();
                        supportTopicProductCategory.CategoryId = int.Parse(topicProductcategory.Value);
                        supportTopic.SupportTopicProductCategories.Add(supportTopicProductCategory);
                    }



                    //SupportTopicStep supportTopicStep = new SupportTopicStep();
                    //supportTopicStep.Title = model.AddSupportTopicStepModel.Title;
                    //supportTopicStep.Description = model.AddSupportTopicStepModel.Description;
                    //supportTopicStep.PictureId = model.AddSupportTopicStepModel.PictureId;
                    //SupportTopicStepMapping supportTopicStepMapping = new SupportTopicStepMapping();
                    //supportTopicStepMapping.TemplateId = model.AddSupportTopicStepModel.TemplateId;
                    //supportTopicStepMapping.SupportTopicStep = supportTopicStep;

                    //supportTopic.SupportTopicStepMapping.Add(supportTopicStepMapping);
                    supportTopic.TemplateId = model.TemplateId;
                    _supportService.InsertSupportTopic(supportTopic);

                    //search engine name
                    model.Title = supportTopic.ValidateSeName(model.Title, supportTopic.Title, true);
                    _seoservice.SaveSlug(supportTopic, model.Title, 0);


                    int i = 0;
                    foreach (var item in model.AvailableTopicStep)
                    {
                        var allDisplayOrders = _supportService.GetAllSupportTopicStep();
                        int newdisplayorder = 0;

                        if (allDisplayOrders.Count > 0)
                        {
                            newdisplayorder = allDisplayOrders.Select(x => x.DisplayOrder).Max() + 1;
                        }
                        else
                        {
                            newdisplayorder++;
                        }

                        SupportTopicStep supporttopicstep = new SupportTopicStep();
                        supporttopicstep.PictureId = item.PictureId;
                        supporttopicstep.SupportStepNo = item.SupportStepNo;
                        supporttopicstep.DisplayOrder = newdisplayorder;
                        supporttopicstep.Title = item.Title;
                        supporttopicstep.SupportTopicId = supportTopic.Id;
                        supporttopicstep.Description = item.Description;

                        _supportService.InsertSupportTopicStep(supporttopicstep);
                        model.AvailableTopicStep[i].Id = supporttopicstep.Id;
                        i++;
                    }


                    //activity log
                    //_customerActivityService.InsertActivity("AddNewCategory", _localizationService.GetResource("ActivityLog.AddNewCategory"), category.Name);

                    SuccessNotification(_localizationService.GetResource("Admin.Support.SupportTopic.Added"));
                    //var supportcategories = _supportService.GetAllSupportCategory();
                    //model.AvailableSupportCategories = supportcategories.Select(x =>
                    //{
                    //    //var supportcategoryModel = x.ToModel();
                    //    var supportcategoryModel = new SupportCategoryModel();
                    //    supportcategoryModel.Id = x.Id;
                    //    supportcategoryModel.Title = x.Title;
                    //    supportcategoryModel.ShowInFooter = x.ShowInFooter;
                    //    supportcategoryModel.PictureId = x.PictureId;
                    //    supportcategoryModel.Description = x.Description;
                    //    return supportcategoryModel;
                    //}).ToList();
                    //var productcategories = _categoryService.GetAllCategories();

                    //var categories = _categoryService.GetAllCategories(showHidden: true);
                    //foreach (var c in categories)
                    //{
                    //    model.AvailableCategories.Add(new SelectListItem()
                    //    {
                    //        Text = c.GetFormattedBreadCrumb(categories),
                    //        Value = c.Id.ToString()
                    //    });
                    //}

                    int a = model.AvailableTopicStep.Count();

                    SupportTopicStepModel step = new SupportTopicStepModel()
                    {
                        SupportStepNo = (a + 1),
                        Title = "Step " + (a + 1),

                    };
                    model.AvailableTopicStep.Add(step);

                    return PartialView("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/SupportTopicAjax.cshtml", model);

                }

                ViewBag.Error = error;
                return PartialView("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/SupportTopicAjax.cshtml", model);

            }
            else
            {
                int td_topicid = _supportService.GetSupportTopicStepById(model.AvailableTopicStep.FirstOrDefault().Id).SupportTopicId;
                model.Id = td_topicid;
                var supportTopic = _supportService.GetSupportTopicById(model.Id);
                if (supportTopic == null)
                    //No category found with the specified id
                    return RedirectToAction("SupportTopicList");

                var allSupportCategories = _supportService.GetAllSupportCategory();
                var newTopicSupportCategory = new List<SupportCategory>();
                foreach (var supportCategory in allSupportCategories)
                    if (model.SelectedSupportCategoryIds != null && model.SelectedSupportCategoryIds.Contains(supportCategory.Id))
                        newTopicSupportCategory.Add(supportCategory);

                var allProductCategories = _categoryService.GetAllCategories();
                var newTopicProductCategory = new List<SelectListItem>();
                foreach (var productCategory in allProductCategories)
                    if (model.SelectedProductCategoryIds != null && model.SelectedProductCategoryIds.Contains(productCategory.Id))
                    {
                        newTopicProductCategory.Add(new SelectListItem()
                        {
                            Text = productCategory.GetFormattedBreadCrumb(allProductCategories),
                            Value = productCategory.Id.ToString()
                        });
                    }

                string error = "";

                if (model.SelectedSupportCategoryIds == null)
                {
                    error = error + "Please select at least one support category.";
                    ModelState.AddModelError("", "Please select at least one support category");

                }
                if (!String.IsNullOrEmpty(model.Description))
                {
                    string acceptable = "b|i";

                    string html = model.Description;
                    string stringPattern = @"</?(?(?=" + acceptable + @")notag|[a-zA-Z0-9]+)(?:\s[a-zA-Z0-9\-]+=?(?:(["",']?).*?\1?)?)*\s*/?>";
                    string filterhtml = Regex.Replace(html, stringPattern, "");

                    if (html.Length != filterhtml.Length)
                    {
                        error = error + "    Html Tags Are Not Allow in Description.";

                        ModelState.AddModelError("", "Html Tags Are Not Allow in Description");
                    }
                }
                foreach (var item in model.AvailableTopicStep)
                {
                    if (String.IsNullOrEmpty(item.Title))
                    {
                        error = error + "   Step Title field is required.";
                        ModelState.AddModelError("", "Step Title field is required");
                    }
                    if (item.PictureId == 0)
                    {
                        error = error + "   Step Picture field is required.";
                        ModelState.AddModelError("", "Step Picture field is required");
                    }
                }

                // var supportcategory = model.ToEntity();
                supportTopic.Title = model.Title;
                supportTopic.Description = model.Description;
                supportTopic.TemplateId = model.TemplateId;

                //foreach (var topicsupportcategory in newTopicSupportCategory)
                //{
                //	SupportTopicSupportCategory supportTopicSupportCategory = new SupportTopicSupportCategory();
                //	supportTopicSupportCategory.SupportCategoryId = topicsupportcategory.Id;
                //	supportTopic.SupportTopicSupportCategory.Add(supportTopicSupportCategory);
                //}

                //foreach (var topicProductcategory in newTopicProductCategory)
                //{
                //	SupportTopicProductCategory supportTopicProductCategory = new SupportTopicProductCategory();
                //	supportTopicProductCategory.CategoryId = int.Parse(topicProductcategory.Value);
                //	supportTopic.SupportTopicProductCategories.Add(supportTopicProductCategory);
                //}


                if (ModelState.IsValid)
                {
                    foreach (var productCategory in allProductCategories)
                    {


                        if (model.SelectedProductCategoryIds != null &&
                              model.SelectedProductCategoryIds.Contains(productCategory.Id))
                        {


                            if (supportTopic.SupportTopicProductCategories.Count(cr => cr.CategoryId == productCategory.Id) == 0)
                            {

                                SupportTopicProductCategory supportTopicProductCategory = new SupportTopicProductCategory();
                                supportTopicProductCategory.CategoryId = productCategory.Id;
                                supportTopic.SupportTopicProductCategories.Add(supportTopicProductCategory);
                            }
                        }
                        else
                        {

                            if (supportTopic.SupportTopicProductCategories.Count(cr => cr.CategoryId == productCategory.Id) > 0)
                            {
                                SupportTopicProductCategory supportTopicProductCategory = new SupportTopicProductCategory(); ;
                                supportTopicProductCategory = supportTopic.SupportTopicProductCategories.Where(x => x.CategoryId == productCategory.Id).FirstOrDefault();
                                _supportService.DeleteSupportTopicProductCategory(supportTopicProductCategory);


                            }
                        }

                    }


                    foreach (var supportCategory in allSupportCategories)
                    {


                        if (model.SelectedSupportCategoryIds != null &&
                              model.SelectedSupportCategoryIds.Contains(supportCategory.Id))
                        {


                            if (supportTopic.SupportTopicSupportCategory.Count(cr => cr.SupportCategoryId == supportCategory.Id) == 0)
                            {

                                SupportTopicSupportCategory supportTopicSupportCategory = new SupportTopicSupportCategory();
                                supportTopicSupportCategory.SupportCategoryId = supportCategory.Id;
                                supportTopic.SupportTopicSupportCategory.Add(supportTopicSupportCategory);
                            }
                        }
                        else
                        {

                            if (supportTopic.SupportTopicSupportCategory.Count(cr => cr.SupportCategoryId == supportCategory.Id) > 0)
                            {
                                SupportTopicSupportCategory supportTopicSupportCategory = new SupportTopicSupportCategory(); ;
                                supportTopicSupportCategory = supportTopic.SupportTopicSupportCategory.Where(x => x.SupportCategoryId == supportCategory.Id).FirstOrDefault();
                                _supportService.DeleteSupportTopicSupportCategory(supportTopicSupportCategory);


                            }
                        }

                    }

                    foreach (var item in model.AvailableTopicStep)
                    {

                        var step = _supportService.GetSupportTopicStepById(item.Id);
                        if (step == null)
                        {
                            var avilablestep = _supportService.GetAllSupportTopicStepBySupportTopicId(model.Id);

                            var allDisplayOrders = _supportService.GetAllSupportTopicStep();
                            int newdisplayorder = 0;

                            if (allDisplayOrders.Count > 0)
                            {
                                newdisplayorder = allDisplayOrders.Select(x => x.DisplayOrder).Max();
                            }

                            SupportTopicStep supporttopicstep = new SupportTopicStep();
                            supporttopicstep.PictureId = item.PictureId;
                            supporttopicstep.SupportStepNo = avilablestep.Count() + 1;
                            supporttopicstep.DisplayOrder = newdisplayorder + 1;
                            supporttopicstep.Title = item.Title;
                            supporttopicstep.SupportTopicId = model.Id;
                            supporttopicstep.Description = item.Description;

                            _supportService.InsertSupportTopicStep(supporttopicstep);

                            item.Id = supporttopicstep.Id;


                        }
                        else
                        {
                            step.PictureId = item.PictureId;
                            step.Title = item.Title;
                            step.Description = item.Description;

                            _supportService.UpdateSupportTopicStep(step);
                        }

                    }

                    _supportService.UpdateSupportTopic(supportTopic);
                    //search engine name
                    model.Title = supportTopic.ValidateSeName(model.Title, supportTopic.Title, true);
                    _seoservice.SaveSlug(supportTopic, model.Title, 0);


                    //model.CompareCount = _supportService.GetAllSupportTopicStepBySupportTopicId(model.Id).Count();


                    //for (int i = 0; i < model.AvailableTopicStep.Count(); i++)
                    //{
                    //    if (model.AvailableTopicStep[i].Title == null && model.AvailableTopicStep[i].PictureId == 0)
                    //    {
                    //        model.AvailableTopicStep.RemoveAt(i);

                    //    }
                    //}

                    int a = model.AvailableTopicStep.Count();

                    SupportTopicStepModel newstep = new SupportTopicStepModel()
                    {
                        SupportStepNo = (a + 1),
                        Title = "Step " + (a + 1),


                    };
                    model.AvailableTopicStep.Add(newstep);
                    //activity log
                    _customerActivityService.InsertActivity("EditSupportTopic", _localizationService.GetResource("ActivityLog.EditSupportTopic"), supportTopic.Title);
                    ViewBag.Error = error;
                    SuccessNotification(_localizationService.GetResource("Admin.Support.SupportTopic.Updated"));
                    return PartialView("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/SupportTopicAjax.cshtml", model);

                }

                ViewBag.Error = error;
                return PartialView("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/SupportTopicAjax.cshtml", model);

            }


        }

        public ActionResult SupportTopicDelete(int id)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageSupport))
            //	return AccessDeniedView();

            var supportTopic = _supportService.GetSupportTopicById(id);
            if (supportTopic == null)
                return Json(new { Result = false }, JsonRequestBehavior.AllowGet);



            _supportService.DeleteSupportTopic(supportTopic);

            var suppoTopicStep = _supportService.GetAllSupportTopicStepBySupportTopicId(id);
            if (suppoTopicStep != null)
            {
                foreach (var item in suppoTopicStep)
                {
                    _supportService.DeleteSupportTopicStep(item);
                }

            }
            //activity log
            _customerActivityService.InsertActivity("DeleteSupportTopic", _localizationService.GetResource("ActivityLog.DeleteSupportTopic"), supportTopic.Title);

            SuccessNotification(_localizationService.GetResource("Admin.Support.SupportTopic.Deleted"));
            return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
            //return Json(new { Result = true, url = Session["current"].ToString() }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult SupportTopicStepList(DataSourceRequest command, int supportTopicId)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageSupport))
            //	return AccessDeniedView();

            var supportTopicstep = _supportService.GetAllSupportTopicStepBySupportTopicId(supportTopicId);



            var data = supportTopicstep
                  .Select(x =>
                  {
                      var defaultProductPicture = _pictureService.GetPictureById(x.PictureId);
                      var supporttopicstepModel = new SupportTopicStepModel()
                      {

                          Id = x.Id,
                          SupportStepNo = x.SupportStepNo,
                          Title = x.Title,
                          Description = x.Description,
                          PictureId = x.PictureId,
                          PictureThumbnailUrl = _pictureService.GetPictureUrl(defaultProductPicture, 75, true),

                      };
                      return supporttopicstepModel;
                  })
                  .ToList();

            var gridModel = new DataSourceResult
            {
                Data = data,
                Total = supportTopicstep.Count
            };

            return Json(gridModel);
        }

        [HttpPost]
        public ActionResult RelatedProductList(DataSourceRequest command, int supportTopicId)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageSupport))
            //	return AccessDeniedView();

            //a vendor should have access only to his products
            var relatedProducts = _supportService.GetRelatedProductsBySupportTopicId(supportTopicId);
            var relatedProductsModel = relatedProducts
                  .Select(x =>
                  {
                      return new SupportTopicModel.SupportTopicRelatedProductModel()
                      {
                          Id = x.Id,
                            //ProductId1 = x.ProductId1,
                            ProductId = x.ProductId,
                          ProductName = ((!String.IsNullOrEmpty(_productService.GetProductById(x.ProductId).ManufacturerPartNumber)) ? (_productService.GetProductById(x.ProductId).ManufacturerPartNumber + " :   ") : "") + _productService.GetProductById(x.ProductId).Name,
                          DisplayOrder = x.DisplayOrder
                      };
                  })
                  .ToList();

            var gridModel = new DataSourceResult()
            {
                Data = relatedProductsModel,
                Total = relatedProductsModel.Count
            };

            return Json(gridModel);
        }
        [HttpPost]
        public ActionResult RelatedProductDelete(int id)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageSupport))
            //	return AccessDeniedView();

            var relatedProduct = _supportService.GetSupportTopicRelatedProductById(id);
            if (relatedProduct == null)
                throw new ArgumentException("No related product found with the specified id");

            _supportService.DeleteSupportTopicRelatedProduct(relatedProduct);
            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult RelatedProductDeleteSelected(ICollection<int> selectedIds)
        {
            var relatedProducts = new List<SupportTopicRelatedProduct>();
            if (selectedIds != null)
            {
                relatedProducts.AddRange(_supportService.GetSupportTopicRelatedProductsByIds(selectedIds.ToArray()));

                for (int i = 0; i < relatedProducts.Count; i++)
                {
                    var relatedProduct = relatedProducts[i];


                    _supportService.DeleteSupportTopicRelatedProduct(relatedProduct);
                }
            }

            return Json(new { Result = true });
        }

        public ActionResult RelatedProductAddPopup(int supportTopicId)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageSupport))
            //	return AccessDeniedView();

            var model = new SupportTopicModel.AddRelatedProductModel();
            //a vendor should have access only to his products
            model.IsLoggedInAsVendor = _workContext.CurrentVendor != null;

            //categories
            model.AvailableCategories.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            var categories = _categoryService.GetAllCategories(showHidden: true);
            foreach (var c in categories)
                model.AvailableCategories.Add(new SelectListItem() { Text = c.GetFormattedBreadCrumb(categories), Value = c.Id.ToString() });

            //manufacturers
            model.AvailableManufacturers.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            foreach (var m in _manufacturerService.GetAllManufacturers(showHidden: true))
                model.AvailableManufacturers.Add(new SelectListItem() { Text = m.Name, Value = m.Id.ToString() });

            //stores
            model.AvailableStores.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            foreach (var s in _storeService.GetAllStores())
                model.AvailableStores.Add(new SelectListItem() { Text = s.Name, Value = s.Id.ToString() });

            //vendors
            model.AvailableVendors.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            foreach (var v in _vendorService.GetAllVendors(0, int.MaxValue, true))
                model.AvailableVendors.Add(new SelectListItem() { Text = v.Name, Value = v.Id.ToString() });

            //product types
            model.AvailableProductTypes = ProductType.SimpleProduct.ToSelectList(false).ToList();
            model.AvailableProductTypes.Insert(0, new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });

            //return View(model);
            return View("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/RelatedProductAddPopup.cshtml", model);
        }

        [HttpPost]
        public ActionResult SupportTopicRelatedProductAddPopupList(DataSourceRequest command, SupportTopicModel.AddRelatedProductModel model)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
            //	return AccessDeniedView();

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null)
            {
                model.SearchVendorId = _workContext.CurrentVendor.Id;
            }

            //var products = _productService.SearchProducts(
            //   categoryIds: new List<int>() { model.SearchCategoryId },
            //   manufacturerId: model.SearchManufacturerId,
            //   storeId: model.SearchStoreId,
            //   vendorId: model.SearchVendorId,
            //   productType: model.SearchProductTypeId > 0 ? (ProductType?)model.SearchProductTypeId : null,
            //   keywords: model.SearchProductName,
            //   pageIndex: command.Page - 1,
            //   pageSize: command.PageSize,
            //   showHidden: true
            //   );
            var products = _productservicecustome.SearchProductsByLinq(
            categoryIds: new List<int>() { model.SearchCategoryId },
            manufacturerId: model.SearchManufacturerId,
            partno: model.Partno,
            storeId: model.SearchStoreId,
            vendorId: model.SearchVendorId,
            productType: model.SearchProductTypeId > 0 ? (ProductType?)model.SearchProductTypeId : null,
            keywords: model.SearchProductName,
            pageIndex: command.Page - 1,
            pageSize: command.PageSize,
            showHidden: true
            );

            var gridModel = new DataSourceResult();
            gridModel.Data = products.Select(x => x.ToModel());
            gridModel.Total = products.TotalCount;

            return Json(gridModel);
        }

        [HttpPost]
        [FormValueRequired("save")]
        public ActionResult RelatedProductAddPopup(string btnId, string formId, SupportTopicModel.AddRelatedProductModel model)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageSupport))
            //	return AccessDeniedView();

            if (model.SelectedProductIds != null)
            {
                foreach (int id in model.SelectedProductIds)
                {
                    var product = _productService.GetProductById(id);
                    if (product != null)
                    {
                        //a vendor should have access only to his products
                        if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
                            continue;

                        var existingRelatedProducts = _supportService.GetRelatedProductsBySupportTopicId(model.SupportTopicId);
                        if (existingRelatedProducts.FindRelatedProduct(model.SupportTopicId, id) == null)
                        {
                            _supportService.InsertSupportTopicRelatedProduct(
                                  new SupportTopicRelatedProduct()
                                  {
                                      SupportTopicId = model.SupportTopicId,
                                      ProductId = id,
                                      DisplayOrder = 1
                                  });
                        }
                    }
                }
            }

            //a vendor should have access only to his products
            model.IsLoggedInAsVendor = _workContext.CurrentVendor != null;
            ViewBag.RefreshPage = true;
            ViewBag.btnId = btnId;
            ViewBag.formId = formId;
            return View("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/RelatedProductAddPopup.cshtml", model);
        }

        #endregion


        #region Support Download

        public ActionResult SupportDownloadList()
        {
            // return View("SupportDownloadList");
            return View("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/SupportDownloadList.cshtml");
        }

        [HttpPost]
        public ActionResult SupportDownloadList(DataSourceRequest command)
        {

            var supportDownloads = _supportService.GetAllSupportDownloads();
            var gridModel = new DataSourceResult
            {
                Data = supportDownloads.Select(x =>
                {
                    var supportDownloadModel = new SupportDownloadModel();
                    supportDownloadModel.Id = x.Id;
                    supportDownloadModel.Title = x.Title;
                    supportDownloadModel.Description = x.Description;
                    return supportDownloadModel;
                }),
                Total = supportDownloads.Count()
            };
            return Json(gridModel);
        }

        #endregion Support Topic

        #region Create / Edit / Delete	Support Downlaod

        public ActionResult CreateSupportDownload(string redirect = null)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageSupport))
            //	return AccessDeniedView();

            Session["urlredirect"] = Request.UrlReferrer;

            var model = new SupportDownloadModel();
            model.redirect = redirect;
            var categories = _categoryService.GetAllCategories(showHidden: true);
            foreach (var c in categories)
            {
                model.AvailableCategories.Add(new SelectListItem()
                {
                    Text = c.GetFormattedBreadCrumb(categories),
                    Value = c.Id.ToString()
                });
            }

            // return View(model);
            return View("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/CreateSupportDownload.cshtml", model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult CreateSupportDownload(SupportDownloadModel model, bool continueEditing)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageSupport))
            //	return AccessDeniedView();

            //validate product categories

            var allProductCategories = _categoryService.GetAllCategories();
            var newDownloadProductCategory = new List<SelectListItem>();
            foreach (var productCategory in allProductCategories)
                if (model.SelectedProductCategoryIds != null && model.SelectedProductCategoryIds.Contains(productCategory.Id))
                {
                    newDownloadProductCategory.Add(new SelectListItem()
                    {
                        Text = productCategory.GetFormattedBreadCrumb(allProductCategories),
                        Value = productCategory.Id.ToString()
                    });
                }

            if (model.DownloadId == 0)
            {
                ModelState.AddModelError("", "Download field is required");
            }
            if (!String.IsNullOrEmpty(model.Description))
            {
                string acceptable = "b|i";
                string html = model.Description;
                string stringPattern = @"</?(?(?=" + acceptable + @")notag|[a-zA-Z0-9]+)(?:\s[a-zA-Z0-9\-]+=?(?:(["",']?).*?\1?)?)*\s*/?>";
                string filterhtml = Regex.Replace(html, stringPattern, "");

                if (html.Length != filterhtml.Length)
                {
                    ModelState.AddModelError("", "Html Tags Are Not Allow in Description");
                }
            }
            if (ModelState.IsValid)
            {

                SupportDownload supportDownload = new SupportDownload();
                supportDownload.Title = model.Title;
                supportDownload.DownloadId = model.DownloadId;
                supportDownload.Description = model.Description;

                foreach (var downloadProductcategory in newDownloadProductCategory)
                {
                    SupportDownloadProductCategory supportDownloadProductCategory = new SupportDownloadProductCategory();
                    supportDownloadProductCategory.CategoryId = int.Parse(downloadProductcategory.Value);
                    supportDownload.SupportDownloadProductCategories.Add(supportDownloadProductCategory);
                }

                _supportService.InsertSupportDownload(supportDownload);
                //activity log
                //_customerActivityService.InsertActivity("AddNewCategory", _localizationService.GetResource("ActivityLog.AddNewCategory"), category.Name);

                SuccessNotification(_localizationService.GetResource("Admin.Support.SupportDownload.Added"));

                if (continueEditing)
                {
                    //Session["savecontinue"] = "true";
                    return RedirectToAction("EditSupportDownload", new { id = supportDownload.Id, redirect = model.redirect });
                }
                if (!String.IsNullOrEmpty(model.redirect))
                {
                    var newurl = Session["urlredirect"];

                    return Redirect(Url.Content(newurl.ToString()));
                }

                return RedirectToAction("SupportDownloadList");
                //return continueEditing ? RedirectToAction("EditSupportDownload", new { id = supportDownload.Id }) : RedirectToAction("SupportDownloadList");
            }



            var categories = _categoryService.GetAllCategories(showHidden: true);
            foreach (var c in categories)
            {
                model.AvailableCategories.Add(new SelectListItem()
                {
                    Text = c.GetFormattedBreadCrumb(categories),
                    Value = c.Id.ToString()
                });
            }

            // return View(model);
            return View("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/CreateSupportDownload.cshtml", model);
        }

        public ActionResult EditSupportDownload(int id, string redirect = null)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageSupport))
            //	return AccessDeniedView();
            //if (Session["savecontinue"] == null)
            //{
            //    Session["current"] = Request.UrlReferrer.OriginalString;
            //}
            var supportDownload = _supportService.GetSupportDownloadById(id);
            if (supportDownload == null)
                //No support category found with the specified id
                return RedirectToAction("SupportDownloadList");

            var model = new SupportDownloadModel();
            model.redirect = redirect;
            model.Id = supportDownload.Id;
            model.Title = supportDownload.Title;
            model.DownloadId = supportDownload.DownloadId;
            model.Description = supportDownload.Description;
            var categories = _categoryService.GetAllCategories(showHidden: true);
            foreach (var c in categories)
            {
                model.AvailableCategories.Add(new SelectListItem()
                {
                    Text = c.GetFormattedBreadCrumb(categories),
                    Value = c.Id.ToString()
                });
            }

            model.SelectedProductCategoryIds = supportDownload.SupportDownloadProductCategories.Select(x => x.CategoryId).ToArray();
            // return View(model);
            return View("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/EditSupportDownload.cshtml", model);
        }
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult EditSupportDownload(SupportDownloadModel model, bool continueEditing)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageSupport))
            //	return AccessDeniedView();

            var supportDownload = _supportService.GetSupportDownloadById(model.Id);
            if (supportDownload == null)
                //No category found with the specified id
                return RedirectToAction("SupportDownloadList");

            var allProductCategories = _categoryService.GetAllCategories();
            var newDownloadProductCategory = new List<SelectListItem>();
            foreach (var productCategory in allProductCategories)
                if (model.SelectedProductCategoryIds != null && model.SelectedProductCategoryIds.Contains(productCategory.Id))
                {
                    newDownloadProductCategory.Add(new SelectListItem()
                    {
                        Text = productCategory.GetFormattedBreadCrumb(allProductCategories),
                        Value = productCategory.Id.ToString()
                    });
                }

            supportDownload.Title = model.Title;
            supportDownload.DownloadId = model.DownloadId;
            supportDownload.Description = model.Description;
            if (!String.IsNullOrEmpty(model.Description))
            {
                string acceptable = "b|i";
                string html = model.Description;
                string stringPattern = @"</?(?(?=" + acceptable + @")notag|[a-zA-Z0-9]+)(?:\s[a-zA-Z0-9\-]+=?(?:(["",']?).*?\1?)?)*\s*/?>";
                string filterhtml = Regex.Replace(html, stringPattern, "");

                if (html.Length != filterhtml.Length)
                {
                    ModelState.AddModelError("", "Html Tags Are Not Allow in Description");
                }
            }
            if (model.DownloadId == 0)
            {
                ModelState.AddModelError("", "Download field is required");
            }

            if (ModelState.IsValid)
            {
                //customer roles
                foreach (var productCategory in allProductCategories)
                {


                    if (model.SelectedProductCategoryIds != null &&
                          model.SelectedProductCategoryIds.Contains(productCategory.Id))
                    {
                        //new role

                        if (supportDownload.SupportDownloadProductCategories.Count(cr => cr.CategoryId == productCategory.Id) == 0)
                        {
                            // supportDownload.SupportDownloadProductCategories.Add(customerRole);
                            SupportDownloadProductCategory supportDownloadProductCategory = new SupportDownloadProductCategory();
                            supportDownloadProductCategory.CategoryId = productCategory.Id;
                            supportDownload.SupportDownloadProductCategories.Add(supportDownloadProductCategory);
                        }
                    }
                    else
                    {

                        if (supportDownload.SupportDownloadProductCategories.Count(cr => cr.CategoryId == productCategory.Id) > 0)
                        {
                            SupportDownloadProductCategory SupportDownloadProductCategory = new SupportDownloadProductCategory();
                            SupportDownloadProductCategory = supportDownload.SupportDownloadProductCategories.Where(x => x.CategoryId == productCategory.Id).FirstOrDefault();
                            _supportService.DeleteSupportDownloadProductCategory(SupportDownloadProductCategory);

                        }
                    }

                }
                _supportService.UpdateSupportDownload(supportDownload);

                //activity log
                _customerActivityService.InsertActivity("EditSupportDownload", _localizationService.GetResource("ActivityLog.EditSupportDownload"), supportDownload.Title);


                if (continueEditing)
                {
                    //Session["savecontinue"] = "true";
                    return RedirectToAction("EditSupportDownload", new { id = supportDownload.Id, redirect = model.redirect });
                }
                if (!String.IsNullOrEmpty(model.redirect))
                {
                    var newurl = Session["urlredirect"];

                    return Redirect(Url.Content(newurl.ToString()));
                }

                return RedirectToAction("SupportDownloadList");
            }

            var categories = _categoryService.GetAllCategories(showHidden: true);
            foreach (var c in categories)
            {
                model.AvailableCategories.Add(new SelectListItem()
                {
                    Text = c.GetFormattedBreadCrumb(categories),
                    Value = c.Id.ToString()
                });
            }

            // return View(model);
            return View("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/EditSupportDownload.cshtml", model);
        }

        public ActionResult DeleteSupportDownload(int id)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageSupport))
            //	return AccessDeniedView();

            var supportDownload = _supportService.GetSupportDownloadById(id);
            if (supportDownload == null)
                return Json(new { Result = false }, JsonRequestBehavior.AllowGet);



            _supportService.DeleteSupportDownload(supportDownload);

            _customerActivityService.InsertActivity("DeleteSupportTopic", _localizationService.GetResource("ActivityLog.DeleteSupportTopic"), supportDownload.Title);

            SuccessNotification(_localizationService.GetResource("Admin.Support.SupportTopic.Deleted"));
            return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
        }




        //[HttpPost]
        //public ActionResult DeleteTopic(int id)
        //{
        //	if (!_permissionService.Authorize(StandardPermissionProvider.ManageSupport))
        //		return AccessDeniedView();

        //	var supportTopic = _supportService.GetSupportTopicById(id);
        //	if (supportTopic == null)
        //		//No category found with the specified id
        //		return RedirectToAction("SupportTopicList");

        //	_supportService.DeleteSupportTopic(supportTopic);
        //	//activity log
        //	_customerActivityService.InsertActivity("DeleteSupportTopic", _localizationService.GetResource("ActivityLog.DeleteSupportTopic"), supportTopic.Title);

        //	SuccessNotification(_localizationService.GetResource("Admin.Support.SupportTopic.Deleted"));
        //	return RedirectToAction("SupportTopicList");
        //}



        [HttpPost]
        public ActionResult SupportDownloadRelatedProductList(DataSourceRequest command, int supportDownloadId)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageSupport))
            //	return AccessDeniedView();

            //a vendor should have access only to his products
            var relatedProducts = _supportService.GetRelatedProductsBySupportDownloadId(supportDownloadId);
            var relatedProductsModel = relatedProducts
                  .Select(x =>
                  {
                      return new SupportDownloadModel.SupportDownloadRelatedProductModel()
                      {
                          Id = x.Id,
                            //ProductId1 = x.ProductId1,
                            ProductId = x.ProductId,
                          ProductName = ((!String.IsNullOrEmpty(_productService.GetProductById(x.ProductId).ManufacturerPartNumber)) ? (_productService.GetProductById(x.ProductId).ManufacturerPartNumber + " :   ") : "") + _productService.GetProductById(x.ProductId).Name,
                          DisplayOrder = x.DisplayOrder
                      };
                  })
                  .ToList();

            var gridModel = new DataSourceResult()
            {
                Data = relatedProductsModel,
                Total = relatedProductsModel.Count
            };

            return Json(gridModel);
        }

        [HttpPost]
        public ActionResult SupportDownloadRelatedProductDelete(int id)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageSupport))
            //	return AccessDeniedView();

            var relatedProduct = _supportService.GetSupportDownloadRelatedProductById(id);
            if (relatedProduct == null)
                throw new ArgumentException("No related product found with the specified id");

            _supportService.DeleteSupportDownloadRelatedProduct(relatedProduct);
            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult SupportDownloadRelatedProductDeleteSelected(ICollection<int> selectedIds)
        {
            var relatedProducts = new List<SupportDownloadRelatedProduct>();
            if (selectedIds != null)
            {
                relatedProducts.AddRange(_supportService.GetSupportDownloadRelatedProductsByIds(selectedIds.ToArray()));

                for (int i = 0; i < relatedProducts.Count; i++)
                {
                    var relatedProduct = relatedProducts[i];

                    _supportService.DeleteSupportDownloadRelatedProduct(relatedProduct);
                }
            }

            return Json(new { Result = true });
        }

        public ActionResult SupportDownloadRelatedProductAddPopup(int supportDownloadId)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageSupport))
            //	return AccessDeniedView();

            var model = new SupportDownloadModel.AddSupportDownloadRelatedProductModel();
            //a vendor should have access only to his products
            model.IsLoggedInAsVendor = _workContext.CurrentVendor != null;

            //categories
            model.AvailableCategories.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            var categories = _categoryService.GetAllCategories(showHidden: true);
            foreach (var c in categories)
                model.AvailableCategories.Add(new SelectListItem() { Text = c.GetFormattedBreadCrumb(categories), Value = c.Id.ToString() });

            //manufacturers
            model.AvailableManufacturers.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            foreach (var m in _manufacturerService.GetAllManufacturers(showHidden: true))
                model.AvailableManufacturers.Add(new SelectListItem() { Text = m.Name, Value = m.Id.ToString() });

            //stores
            model.AvailableStores.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            foreach (var s in _storeService.GetAllStores())
                model.AvailableStores.Add(new SelectListItem() { Text = s.Name, Value = s.Id.ToString() });

            //vendors
            model.AvailableVendors.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            foreach (var v in _vendorService.GetAllVendors(0, int.MaxValue, true))
                model.AvailableVendors.Add(new SelectListItem() { Text = v.Name, Value = v.Id.ToString() });

            //product types
            model.AvailableProductTypes = ProductType.SimpleProduct.ToSelectList(false).ToList();
            model.AvailableProductTypes.Insert(0, new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });

            model.SupportDownloadId = supportDownloadId;
            //return View(model);
            return View("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/SupportDownloadRelatedProductAddPopup.cshtml", model);
        }

        [HttpPost]
        public ActionResult SupportDownloadRelatedProductAddPopupList(DataSourceRequest command, SupportDownloadModel.AddSupportDownloadRelatedProductModel model)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
            //	return AccessDeniedView();

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null)
            {
                model.SearchVendorId = _workContext.CurrentVendor.Id;
            }

            //var products = _productService.SearchProducts(
            //   categoryIds: new List<int>() { model.SearchCategoryId },
            //   manufacturerId: model.SearchManufacturerId,
            //   storeId: model.SearchStoreId,
            //   vendorId: model.SearchVendorId,
            //   productType: model.SearchProductTypeId > 0 ? (ProductType?)model.SearchProductTypeId : null,
            //   keywords: model.SearchProductName,
            //   pageIndex: command.Page - 1,
            //   pageSize: command.PageSize,
            //   showHidden: true
            //   );

            var products = _productservicecustome.SearchProductsByLinq(
         categoryIds: new List<int>() { model.SearchCategoryId },
         manufacturerId: model.SearchManufacturerId,
         partno: model.Partno,
         storeId: model.SearchStoreId,
         vendorId: model.SearchVendorId,
         productType: model.SearchProductTypeId > 0 ? (ProductType?)model.SearchProductTypeId : null,
         keywords: model.SearchProductName,
         pageIndex: command.Page - 1,
         pageSize: command.PageSize,
         showHidden: true
         );
            var gridModel = new DataSourceResult();
            gridModel.Data = products.Select(x => x.ToModel());
            gridModel.Total = products.TotalCount;

            return Json(gridModel);
        }

        [HttpPost]
        [FormValueRequired("save")]
        public ActionResult SupportDownloadRelatedProductAddPopup(string btnId, string formId, SupportDownloadModel.AddSupportDownloadRelatedProductModel model)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageSupport))
            //	return AccessDeniedView();

            if (model.SelectedProductIds != null)
            {
                foreach (int id in model.SelectedProductIds)
                {
                    var product = _productService.GetProductById(id);
                    if (product != null)
                    {
                        //a vendor should have access only to his products
                        if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
                            continue;

                        var existingRelatedProducts = _supportService.GetRelatedProductsBySupportDownloadId(model.SupportDownloadId);
                        if (existingRelatedProducts.FindRelatedProduct(model.SupportDownloadId, id) == null)
                        {
                            _supportService.InsertSupportDownloadRelatedProduct(
                                 new SupportDownloadRelatedProduct()
                                 {
                                     SupportDownloadId = model.SupportDownloadId,
                                     ProductId = id,
                                     DisplayOrder = 1
                                 });
                        }
                    }
                }
            }

            //a vendor should have access only to his products
            model.IsLoggedInAsVendor = _workContext.CurrentVendor != null;
            ViewBag.RefreshPage = true;
            ViewBag.btnId = btnId;
            ViewBag.formId = formId;
            //	return View(model);
            return View("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/SupportDownloadRelatedProductAddPopup.cshtml", model);
        }

        /// <summary>
        /// Save selected TAB index
        /// </summary>
        /// <param name="index">Idnex to save; null to automatically detect it</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        protected void SaveSelectedTabIndex(int? index = null, bool persistForTheNextRequest = true)
        {
            //keep this method synchronized with
            //"GetSelectedTabIndex" method of \Nop.Web.Framework\ViewEngines\Razor\WebViewPage.cs
            if (!index.HasValue)
            {
                int tmp = 0;
                if (int.TryParse(this.Request.Form["selected-tab-index"], out tmp))
                {
                    index = tmp;
                }
            }
            if (index.HasValue)
            {
                string dataKey = "nop.selected-tab-index";
                if (persistForTheNextRequest)
                {
                    TempData[dataKey] = index;
                }
                else
                {
                    ViewData[dataKey] = index;
                }
            }
        }

        #endregion

        #region Create / Edit / Delete	Support Video

        public ActionResult SupportVideoList()
        {
            return View("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/SupportVideoList.cshtml");
        }

        [HttpPost]
        public ActionResult SupportVideoList(DataSourceRequest command)
        {


            var supportVideos = _supportService.GetAllSupportVideos();
            var gridModel = new DataSourceResult
            {
                Data = supportVideos.Select(x =>
                {
                    var defaultProductPicture = _pictureService.GetPictureById(x.PictureId);
                    var supportVideoModel = new SupportVideoModel();
                    supportVideoModel.Id = x.Id;
                    supportVideoModel.Title = x.Title;
                    supportVideoModel.PictureId = x.PictureId;
                    supportVideoModel.PictureThumbnailUrl = _pictureService.GetPictureUrl(defaultProductPicture, 75, true);
                    supportVideoModel.VimeoInformation = x.VimeoInformation;
                    return supportVideoModel;
                }),
                Total = supportVideos.Count()
            };
            return Json(gridModel);
        }


        public ActionResult CreateSupportVideo(string redirect = null)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageSupport))
            //	return AccessDeniedView();
            Session["urlredirect"] = Request.UrlReferrer;
            var model = new SupportVideoModel();
            model.redirect = redirect;
            var categories = _categoryService.GetAllCategories(showHidden: true);
            foreach (var c in categories)
            {
                model.AvailableCategories.Add(new SelectListItem()
                {
                    Text = c.GetFormattedBreadCrumb(categories),
                    Value = c.Id.ToString()
                });
            }

            return View("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/CreateSupportVideo.cshtml", model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult CreateSupportVideo(SupportVideoModel model, bool continueEditing)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageSupport))
            //	return AccessDeniedView();

            //Validate Product Categories
            var allProductCategories = _categoryService.GetAllCategories();
            var newVideoProductCategory = new List<SelectListItem>();
            foreach (var productCategory in allProductCategories)
                if (model.SelectedProductCategoryIds != null && model.SelectedProductCategoryIds.Contains(productCategory.Id))
                {
                    newVideoProductCategory.Add(new SelectListItem()
                    {
                        Text = productCategory.GetFormattedBreadCrumb(allProductCategories),
                        Value = productCategory.Id.ToString()
                    });
                }

            if (!String.IsNullOrEmpty(model.Description))
            {
                string acceptable = "b|i";
                string html = model.Description;
                string stringPattern = @"</?(?(?=" + acceptable + @")notag|[a-zA-Z0-9]+)(?:\s[a-zA-Z0-9\-]+=?(?:(["",']?).*?\1?)?)*\s*/?>";
                string filterhtml = Regex.Replace(html, stringPattern, "");

                if (html.Length != filterhtml.Length)
                {
                    ModelState.AddModelError("", "Html Tags Are Not Allow in Description");
                }
            }
            if (model.AvailableCategories == null)
            {
                ModelState.AddModelError("", "Please Select At Least One Product Category");

            }
            if (model.PictureId == 0)
            {
                ModelState.AddModelError("", "Please Upload Picture");
            }

            if (ModelState.IsValid)
            {
                SupportVideo supportVideo = new SupportVideo();
                supportVideo.Title = model.Title;

                foreach (var videoProductcategory in newVideoProductCategory)
                {

                    SupportVideoProductCategory supportVideoProductCategory = new SupportVideoProductCategory();
                    supportVideoProductCategory.CategoryId = int.Parse(videoProductcategory.Value);
                    supportVideo.SupportVideoProductCategories.Add(supportVideoProductCategory);
                }

                supportVideo.PictureId = model.PictureId;
                supportVideo.Title = model.Title;
                supportVideo.VimeoInformation = model.VimeoInformation;
                supportVideo.Tag = model.Tag;
                supportVideo.DisplayOrder = model.DisplayOrder;
                supportVideo.Description = model.Description;

                _supportService.InsertSupportVideo(supportVideo);

                SuccessNotification(_localizationService.GetResource("Admin.Support.SupportVideo.Added"));

                if (continueEditing)
                {
                    //Session["savecontinue"] = "true";
                    return RedirectToAction("EditSupportVideo", new { id = supportVideo.Id, redirect = model.redirect });
                }
                if (!String.IsNullOrEmpty(model.redirect))
                {
                    var newurl = Session["urlredirect"];

                    return Redirect(Url.Content(newurl.ToString()));
                }
                return RedirectToAction("SupportVideoList");
                // return continueEditing ? RedirectToAction("EditSupportVideo", new { id = supportVideo.Id }) : RedirectToAction("SupportVideoList");
            }

            var categories = _categoryService.GetAllCategories(showHidden: true);
            foreach (var c in categories)
            {
                model.AvailableCategories.Add(new SelectListItem()
                {
                    Text = c.GetFormattedBreadCrumb(categories),
                    Value = c.Id.ToString()
                });
            }

            return View("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/CreateSupportVideo.cshtml", model);
        }

        public ActionResult EditSupportVideo(int id, string redirect = null)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageSupport))
            //	return AccessDeniedView();
            //if (Session["savecontinue"] == null)
            //{
            //    Session["current"] = Request.UrlReferrer.OriginalString;
            //}
            var supportVideo = _supportService.GetSupportVideoById(id);
            if (supportVideo == null)
                //No support category found with the specified id
                return RedirectToAction("SupportVideoList");

            var model = new SupportVideoModel();
            model.redirect = redirect;
            model.Id = supportVideo.Id;
            model.Title = supportVideo.Title;
            model.VimeoInformation = supportVideo.VimeoInformation;
            model.PictureId = supportVideo.PictureId;
            model.Description = supportVideo.Description;
            model.DisplayOrder = supportVideo.DisplayOrder;
            model.Tag = supportVideo.Tag;

            var categories = _categoryService.GetAllCategories(showHidden: true);
            foreach (var c in categories)
            {
                model.AvailableCategories.Add(new SelectListItem()
                {
                    Text = c.GetFormattedBreadCrumb(categories),
                    Value = c.Id.ToString()
                });
            }

            model.SelectedProductCategoryIds = supportVideo.SupportVideoProductCategories.Select(x => x.CategoryId).ToArray();
            return View("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/EditSupportVideo.cshtml", model);
        }
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult EditSupportVideo(SupportVideoModel model, bool continueEditing)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageSupport))
            //	return AccessDeniedView();

            var supportVideo = _supportService.GetSupportVideoById(model.Id);
            if (supportVideo == null)
                //No category found with the specified id
                return RedirectToAction("SupportVideoList");



            var allProductCategories = _categoryService.GetAllCategories();
            var newVideoProductCategory = new List<SelectListItem>();
            foreach (var productCategory in allProductCategories)
                if (model.SelectedProductCategoryIds != null && model.SelectedProductCategoryIds.Contains(productCategory.Id))
                {
                    newVideoProductCategory.Add(new SelectListItem()
                    {
                        Text = productCategory.GetFormattedBreadCrumb(allProductCategories),
                        Value = productCategory.Id.ToString()
                    });
                }

            supportVideo.Title = model.Title;
            supportVideo.PictureId = model.PictureId;
            supportVideo.VimeoInformation = model.VimeoInformation;
            supportVideo.Description = model.Description;
            supportVideo.DisplayOrder = model.DisplayOrder;
            supportVideo.Tag = model.Tag;

            //foreach (var videoProductcategory in newVideoProductCategory)
            //{

            //	SupportVideoProductCategory supportVideoProductCategory = new SupportVideoProductCategory();
            //	supportVideoProductCategory.CategoryId = int.Parse(videoProductcategory.Value);
            //	supportVideo.SupportVideoProductCategories.Add(supportVideoProductCategory);
            //}
            if (!String.IsNullOrEmpty(model.Description))
            {
                string acceptable = "b|i";
                string html = model.Description;
                string stringPattern = @"</?(?(?=" + acceptable + @")notag|[a-zA-Z0-9]+)(?:\s[a-zA-Z0-9\-]+=?(?:(["",']?).*?\1?)?)*\s*/?>";
                string filterhtml = Regex.Replace(html, stringPattern, "");

                if (html.Length != filterhtml.Length)
                {
                    ModelState.AddModelError("", "Html Tags Are Not Allow in Description");
                }
            }
            if (model.PictureId == 0)
            {
                ModelState.AddModelError("", "Please Upload Picture");
            }
            if (ModelState.IsValid)
            {

                foreach (var pr in allProductCategories)
                {
                    if (model.SelectedProductCategoryIds != null &&
                                       model.SelectedProductCategoryIds.Contains(pr.Id))
                    {
                        //new role
                        if (supportVideo.SupportVideoProductCategories.Count(cr => cr.CategoryId == pr.Id) == 0)
                        {
                            SupportVideoProductCategory supportVideoProductCategory = new SupportVideoProductCategory();
                            supportVideoProductCategory.CategoryId = pr.Id;
                            supportVideo.SupportVideoProductCategories.Add(supportVideoProductCategory);
                        }
                        //customer.CustomerRoles.Add(customerRole);
                    }
                    else
                    {
                        //remove role
                        if (supportVideo.SupportVideoProductCategories.Count(cr => cr.CategoryId == pr.Id) > 0)
                        {
                            SupportVideoProductCategory supportVideoProductCategory = new SupportVideoProductCategory();
                            supportVideoProductCategory = supportVideo.SupportVideoProductCategories.Where(x => x.CategoryId == pr.Id).FirstOrDefault();
                            _supportService.DeleteSupportVideoProductCategory(supportVideoProductCategory);
                            //supportVideo.SupportVideoProductCategories.Remove(supportVideoProductCategory);
                        }

                    }
                }

                _supportService.UpdateSupportVideo(supportVideo);
                //activity log
                _customerActivityService.InsertActivity("EditSupportVideo", _localizationService.GetResource("ActivityLog.EditSupportVideo"), supportVideo.Title);

                SuccessNotification(_localizationService.GetResource("Admin.Support.SupportVideo.Updated"));

                if (continueEditing)
                {
                    //Session["savecontinue"] = "true";
                    return RedirectToAction("EditSupportVideo", new { id = supportVideo.Id, redirect = model.redirect });
                }
                if (!String.IsNullOrEmpty(model.redirect))
                {
                    var newurl = Session["urlredirect"];

                    return Redirect(Url.Content(newurl.ToString()));
                }

                return RedirectToAction("SupportVideoList");
            }

            var categories = _categoryService.GetAllCategories(showHidden: true);
            foreach (var c in categories)
            {
                model.AvailableCategories.Add(new SelectListItem()
                {
                    Text = c.GetFormattedBreadCrumb(categories),
                    Value = c.Id.ToString()
                });
            }

            return View("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/EditSupportVideo.cshtml", model);
        }

        [HttpPost]
        public ActionResult SupportVideoRelatedProductList(DataSourceRequest command, int supportVideoId)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageSupport))
            //	return AccessDeniedView();

            //a vendor should have access only to his products
            var relatedProducts = _supportService.GetRelatedProductsBySupportVideoId(supportVideoId);
            var relatedProductsModel = relatedProducts
            .Select(x =>
            {
                return new SupportVideoModel.SupportVideoRelatedProductModel()
                {
                    Id = x.Id,
                      //ProductId1 = x.ProductId1,
                      ProductId = x.ProductId,
                    ProductName = ((!String.IsNullOrEmpty(_productService.GetProductById(x.ProductId).ManufacturerPartNumber)) ? (_productService.GetProductById(x.ProductId).ManufacturerPartNumber + " :   ") : "") + _productService.GetProductById(x.ProductId).Name,
                    DisplayOrder = x.DisplayOrder
                };
            })
            .ToList();

            var gridModel = new DataSourceResult()
            {
                Data = relatedProductsModel,
                Total = relatedProductsModel.Count
            };

            return Json(gridModel);
        }

        [HttpPost]
        public ActionResult SupportVideoRelatedProductDelete(int id)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageSupport))
            //	return AccessDeniedView();

            var relatedProduct = _supportService.GetSupportVideoRelatedProductById(id);
            if (relatedProduct == null)
                throw new ArgumentException("No related product found with the specified id");

            _supportService.DeleteSupportVideoRelatedProduct(relatedProduct);
            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult SupportVideoRelatedProductDeleteSelected(ICollection<int> selectedIds)
        {
            var relatedProducts = new List<SupportVideoRelatedProduct>();
            if (selectedIds != null)
            {
                relatedProducts.AddRange(_supportService.GetSupportVideoRelatedProductsByIds(selectedIds.ToArray()));

                for (int i = 0; i < relatedProducts.Count; i++)
                {
                    var relatedProduct = relatedProducts[i];

                    _supportService.DeleteSupportVideoRelatedProduct(relatedProduct);
                }
            }

            return Json(new { Result = true });
        }

        public ActionResult SupportVideoRelatedProductAddPopup(int supportVideoId)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageSupport))
            //	return AccessDeniedView();

            var model = new SupportVideoModel.AddSupportVideoRelatedProductModel();
            //a vendor should have access only to his products
            model.IsLoggedInAsVendor = _workContext.CurrentVendor != null;

            //categories
            model.AvailableCategories.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            var categories = _categoryService.GetAllCategories(showHidden: true);
            foreach (var c in categories)
                model.AvailableCategories.Add(new SelectListItem() { Text = c.GetFormattedBreadCrumb(categories), Value = c.Id.ToString() });

            //manufacturers
            model.AvailableManufacturers.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            foreach (var m in _manufacturerService.GetAllManufacturers(showHidden: true))
                model.AvailableManufacturers.Add(new SelectListItem() { Text = m.Name, Value = m.Id.ToString() });

            //stores
            model.AvailableStores.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            foreach (var s in _storeService.GetAllStores())
                model.AvailableStores.Add(new SelectListItem() { Text = s.Name, Value = s.Id.ToString() });

            //vendors
            model.AvailableVendors.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            foreach (var v in _vendorService.GetAllVendors(0, int.MaxValue, true))
                model.AvailableVendors.Add(new SelectListItem() { Text = v.Name, Value = v.Id.ToString() });

            //product types
            model.AvailableProductTypes = ProductType.SimpleProduct.ToSelectList(false).ToList();
            model.AvailableProductTypes.Insert(0, new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });

            model.SupportVideoId = supportVideoId;
            return View("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/SupportVideoRelatedProductAddPopup.cshtml", model);
        }

        [HttpPost]
        public ActionResult SupportVideoRelatedProductAddPopupList(DataSourceRequest command, SupportVideoModel.AddSupportVideoRelatedProductModel model)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
            //	return AccessDeniedView();

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null)
            {
                model.SearchVendorId = _workContext.CurrentVendor.Id;
            }

            //var products = _productService.SearchProducts(
            //categoryIds: new List<int>() { model.SearchCategoryId },
            //manufacturerId: model.SearchManufacturerId,
            //storeId: model.SearchStoreId,
            //vendorId: model.SearchVendorId,
            //productType: model.SearchProductTypeId > 0 ? (ProductType?)model.SearchProductTypeId : null,
            //keywords: model.SearchProductName,
            //pageIndex: command.Page - 1,
            //pageSize: command.PageSize,
            //showHidden: true
            //);
            var products = _productservicecustome.SearchProductsByLinq(
     categoryIds: new List<int>() { model.SearchCategoryId },
     manufacturerId: model.SearchManufacturerId,
     partno: model.Partno,
     storeId: model.SearchStoreId,
     vendorId: model.SearchVendorId,
     productType: model.SearchProductTypeId > 0 ? (ProductType?)model.SearchProductTypeId : null,
     keywords: model.SearchProductName,
     pageIndex: command.Page - 1,
     pageSize: command.PageSize,
     showHidden: true
     );

            var gridModel = new DataSourceResult();
            gridModel.Data = products.Select(x => x.ToModel());
            gridModel.Total = products.TotalCount;

            return Json(gridModel);
        }

        [HttpPost]
        [FormValueRequired("save")]
        public ActionResult SupportVideoRelatedProductAddPopup(string btnId, string formId, SupportVideoModel.AddSupportVideoRelatedProductModel model)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageSupport))
            //	return AccessDeniedView();

            if (model.SelectedProductIds != null)
            {
                foreach (int id in model.SelectedProductIds)
                {
                    var product = _productService.GetProductById(id);
                    if (product != null)
                    {
                        //a vendor should have access only to his products
                        if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
                            continue;

                        var existingRelatedProducts = _supportService.GetRelatedProductsBySupportVideoId(model.SupportVideoId);
                        if (existingRelatedProducts.FindRelatedProduct(model.SupportVideoId, id) == null)
                        {
                            _supportService.InsertSupportVideoRelatedProduct(
                               new SupportVideoRelatedProduct()
                               {
                                   SupportVideoId = model.SupportVideoId,
                                   ProductId = id,
                                   DisplayOrder = 1
                               });
                        }
                    }
                }
            }

            //a vendor should have access only to his products
            model.IsLoggedInAsVendor = _workContext.CurrentVendor != null;
            ViewBag.RefreshPage = true;
            ViewBag.btnId = btnId;
            ViewBag.formId = formId;
            return View("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/SupportVideoRelatedProductAddPopup.cshtml", model);
        }

        public ActionResult DeleteSupportVideo(int Id)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageSupport))
            //	return AccessDeniedView();

            var supportVideo = _supportService.GetSupportVideoById(Id);
            if (supportVideo == null)
                throw new ArgumentException("No Support Video found with the specified id");

            _supportService.DeleteSupportVideo(supportVideo);
            return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region PublicSide

        public ActionResult PublicInfo()
        {
            var model = new PublicInfoModel();
            var categoryList = _categoryService.GetAllCategories();

            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var storeInformationSettings = _settingService.LoadSetting<StoreInformationSettings>(storeScope);

            model.SystemDesignEnabled = storeInformationSettings.ShowSystemDesign;

            foreach (var item in categoryList)
            {
                CategoryList categories = new CategoryList();
                categories.Id = item.Id;
                categories.Category = item.Name;
                categories.SeoName = item.GetSeName();
                categories.PictureId = item.PictureId;
                categories.PictureUrl = _pictureService.GetPictureUrl(categories.PictureId, 125);
                model.AvailableCategories.Add(categories);
            }
            return View("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/PublicInfo.cshtml", model);
        }

        [HttpPost]
        public ActionResult PublicInfo(PublicInfoModel model)
        {
            if (!String.IsNullOrEmpty(model.KeywordText))
            {
                model.IsTextEmpty = false;

                var Keyword = _dataProvider.GetParameter();
                Keyword.ParameterName = "Keywords";
                Keyword.Value = model.KeywordText;
                Keyword.DbType = DbType.String;

                var useFullTSearchs = _dataProvider.GetParameter();
                useFullTSearchs.ParameterName = "UseFullTextSearch";
                useFullTSearchs.Value = 0;
                useFullTSearchs.DbType = DbType.Int32;

                var LangIds = _dataProvider.GetParameter();
                LangIds.ParameterName = "LanguageId";
                LangIds.Value = 1;
                LangIds.DbType = DbType.Int32;

                var FullModes = _dataProvider.GetParameter();
                FullModes.ParameterName = "FullTextMode";
                FullModes.Value = 1;
                FullModes.DbType = DbType.Int32;

                var SearchResult = _supportcontext.SqlQuery<SearchResult>("EXEC SupportSearch @Keywords,@UseFullTextSearch,@LanguageId,@FullTextMode", Keyword, useFullTSearchs, LangIds, FullModes);

                if (SearchResult.Count() > 0)
                {
                    model.IsResultFound = true;
                    int[] SupportCategoryids = new int[SearchResult.Count()];
                    int[] SupportTopicids = new int[SearchResult.Count()];
                    int[] SupportVideoids = new int[SearchResult.Count()];
                    int[] SupportDownloadids = new int[SearchResult.Count()];

                    int i = 0, j = 0, k = 0, l = 0;

                    foreach (var item in SearchResult)
                    {
                        if (item.Entitytype == 1)
                        {

                            SupportCategoryids[i] = item.Entityid;
                            i++;
                        }
                        if (item.Entitytype == 2)
                        {
                            SupportTopicids[j] = item.Entityid;
                            j++;
                        }
                        if (item.Entitytype == 3)
                        {
                            SupportVideoids[k] = item.Entityid;
                            k++;
                        }
                        if (item.Entitytype == 4)
                        {
                            SupportDownloadids[l] = item.Entityid;
                            l++;
                        }


                    }

                    SupportCategoryids.Distinct();
                    SupportTopicids.Distinct();
                    SupportVideoids.Distinct();
                    SupportDownloadids.Distinct();

                    var categorylist = _supportService.GetAllSupportCategory().Where(x => SupportCategoryids.Contains(x.Id)).ToList();

                    var topiclist = _supportService.GetAllSupportTopics().Where(x => SupportTopicids.Contains(x.Id)).ToList();

                    var videolist = _supportService.GetAllSupportVideos().Where(x => SupportVideoids.Contains(x.Id)).ToList();

                    var downloadlist = _supportService.GetAllSupportDownloads().Where(x => SupportDownloadids.Contains(x.Id)).ToList();

                    foreach (var Catitem in categorylist)
                    {
                        var supportcat = new SearchSupportCategoryList();
                        supportcat.SeName = Catitem.GetSeName();
                        supportcat.SupportCategoryId = Catitem.Id;
                        supportcat.SupportCategoryName = Catitem.Title;
                        supportcat.SupportCategoryPictureId = Catitem.PictureId;
                        supportcat.SupportCategoryPictureUrl = _pictureService.GetPictureUrl(Catitem.PictureId, 75);

                        model.AvailableSupportCategories.Add(supportcat);
                    }
                    foreach (var topic in topiclist)
                    {
                        var suporttopic = new SearchSupportTopicList();
                        suporttopic.SeoName = topic.GetSeName();
                        suporttopic.SupportTopicId = topic.Id;
                        suporttopic.SupportTopicTitle = topic.Title;
                        model.AvailableSupportTopics.Add(suporttopic);
                    }
                    foreach (var video in videolist)
                    {
                        var suportvideo = new SearchSupportVideo();
                        suportvideo.SupportVideoId = video.Id;
                        suportvideo.Title = video.Title;
                        suportvideo.VimeoInformation = video.VimeoInformation;
                        suportvideo.PictureId = video.PictureId;
                        suportvideo.PictureUrl = _pictureService.GetPictureUrl(video.PictureId, 200);
                        model.AvailableSupportVideo.Add(suportvideo);
                    }
                    foreach (var download in downloadlist)
                    {
                        var suportdownload = new SearchSupportDownload();
                        suportdownload.DownloadId = download.DownloadId;
                        suportdownload.Id = download.Id;
                        suportdownload.Title = download.Title;
                        model.AvailableSupportDownload.Add(suportdownload);
                    }

                }
                else
                {
                    model.IsResultFound = false;
                    var categoryList = _categoryService.GetAllCategories();

                    foreach (var item in categoryList)
                    {
                        CategoryList categories = new CategoryList();
                        categories.Id = item.Id;
                        categories.Category = item.Name;
                        categories.PictureId = item.PictureId;
                        categories.PictureUrl = _pictureService.GetPictureUrl(categories.PictureId, 125);
                        model.AvailableCategories.Add(categories);
                    }
                }
            }
            else
            {
                var categoryList = _categoryService.GetAllCategories();

                foreach (var item in categoryList)
                {
                    CategoryList categories = new CategoryList();
                    categories.Id = item.Id;
                    categories.Category = item.Name;
                    categories.PictureId = item.PictureId;
                    categories.PictureUrl = _pictureService.GetPictureUrl(categories.PictureId, 125);
                    model.AvailableCategories.Add(categories);
                }
                model.IsTextEmpty = true;
            }
            return View("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/PublicInfo.cshtml", model);
        }

        public ActionResult SupportCategory(string productCategorsename)
        {
            var model = new ProductCategorySupportCategory();
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var storeInformationSettings = _settingService.LoadSetting<StoreInformationSettings>(storeScope);

            model.SystemDesignEnabled = storeInformationSettings.ShowSystemDesign;

            if (String.IsNullOrEmpty(productCategorsename))
                return RedirectToAction("PublicInfo");

            int productCategoryId = 0;
            var result = _seoservice.GetBySlug(productCategorsename);
            if (result == null)
            {
                return RedirectToAction("PublicInfo");
            }
            else
            {
                productCategoryId = result.EntityId;
            }
            //var supportCategories = _supportService.GetAllSupportCategoryByProductCategoryId(productCategoryId).Where(x=>x.IsActive);
            var supportCategories = _supportService.GetAllSupportCategoryBySupporttopicFilter(0, productCategoryId);

            foreach (var sCategory in supportCategories)
            {
                var supportcategorylist = new SupportCategoryList();
                supportcategorylist.SupportCategoryId = sCategory.Id;
                supportcategorylist.SeName = sCategory.GetSeName();
                supportcategorylist.SupportCategoryName = sCategory.Title;
                supportcategorylist.SupportCategoryPictureId = sCategory.PictureId;
                supportcategorylist.SupportCategoryPictureUrl = _pictureService.GetPictureUrl(sCategory.PictureId, 72);
                //int IsAvailabletopic = _supportService.GetAllSupportTopicByProductCategoryFilter(sCategory.Id, productCategoryId).Count;
                // supportcategorylist.IsTopicAvailable =( IsAvailabletopic > 0) ? true : false;
                supportcategorylist.IsTopicAvailable = true;
                model.AvailableSupportCategories.Add(supportcategorylist);
            }
            var category = _categoryService.GetCategoryById(productCategoryId);
            model.ProductCategoryId = productCategoryId;
            model.ProductCategoryName = category.Name;
            model.SeoName = category.GetSeName();
            model.ProductCategoryPictureId = category.PictureId;
            model.ProductCategoryPictureUrl = _pictureService.GetPictureUrl(category.PictureId, 140);

            Session["ProductCategoryId"] = category.Id;
            Session["ProductCategoryName"] = category.Name;

            var supportVideo = _supportService.GetAllSupportVideoByProductCategoryId(productCategoryId);
            if (supportVideo.Count > 0)
            {
                model.AvailbleVideo = true;
            }
            else
            {
                model.AvailbleVideo = false;
            }

            var supportDownload = _supportService.GetAllSupportDownloadByProductCategoryId(productCategoryId);
            if (supportDownload.Count > 0)
            {
                model.AvailbleDownload = true;
            }
            else
            {
                model.AvailbleDownload = false;
            }

            return View("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/SupportCategory.cshtml", model);
        }

        public ActionResult SupportTopicListPublic(string suportCategorysename)
        {
            var model = new SupportTopicListModel();

            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var storeInformationSettings = _settingService.LoadSetting<StoreInformationSettings>(storeScope);

            model.SystemDesignEnabled = storeInformationSettings.ShowSystemDesign;

            if (String.IsNullOrEmpty(suportCategorysename))
                return RedirectToAction("PublicInfo");


            int suportCategoryId = 0;
            var result = _seoservice.GetBySlug(suportCategorysename);
            if (result == null)
            {
                return RedirectToAction("PublicInfo");
            }
            else
            {
                suportCategoryId = result.EntityId;
            }
            int productcat = 0;
            if (Session["ProductCategoryId"] != null)
            {
                productcat = Convert.ToInt32(Session["ProductCategoryId"].ToString());
            }

            var supportTopic = _supportService.GetAllSupportTopicByProductCategoryFilter(suportCategoryId, productcat);

            foreach (var sTopic in supportTopic)
            {
                var supportTopiclist = new SupportTopicContainer();
                supportTopiclist.SupportTopicId = sTopic.Id;
                supportTopiclist.SupportTopicName = sTopic.Title;
                supportTopiclist.Sename = sTopic.GetSeName();
                model.AvailableSupportTopic.Add(supportTopiclist);
            }
            var supportcategory = _supportService.GetSupportCategoryById(suportCategoryId);

            Session["SupportCategoryId"] = supportcategory.Id;
            Session["SupportCategoryName"] = supportcategory.Title;
            model.SupportCategoryId = suportCategoryId;
            model.SupportCategoryName = supportcategory.Title;
            model.SupportCategoryPictureId = supportcategory.PictureId;
            model.SupportCategoryPictureUrl = _pictureService.GetPictureUrl(supportcategory.PictureId, 140);

            return View("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/SupportTopicListPublic.cshtml", model);
        }

        public ActionResult SupportVideoListPublic(string productCategorsename)
        {
            var model = new SupportTopicListModel();
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var storeInformationSettings = _settingService.LoadSetting<StoreInformationSettings>(storeScope);

            model.SystemDesignEnabled = storeInformationSettings.ShowSystemDesign;

            if (String.IsNullOrEmpty(productCategorsename))
                return RedirectToAction("PublicInfo");


            int productCategoryId = 0;
            var result = _seoservice.GetBySlug(productCategorsename);
            if (result == null)
            {
                return RedirectToAction("PublicInfo");
            }
            else
            {
                productCategoryId = result.EntityId;
            }

            var supportvideo = _supportService.GetAllSupportVideoByProductCategoryId(productCategoryId);

            foreach (var sVideo in supportvideo)
            {
                var supportVideoList = new SupportVideoContainer();
                supportVideoList.SupportVideoId = sVideo.Id;
                supportVideoList.Title = sVideo.Title;
                supportVideoList.Description = sVideo.Description;
                supportVideoList.PictureId = sVideo.PictureId;
                supportVideoList.DisplayOrder = sVideo.DisplayOrder;
                supportVideoList.VimeoInformation = sVideo.VimeoInformation;

                supportVideoList.PictureUrl = _pictureService.GetPictureUrl(sVideo.PictureId, 200);

                model.AvailableSupportVideo.Add(supportVideoList);
            }


            return View("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/SupportVideoListPublic.cshtml", model);
        }
        public ActionResult SupportDownloadListPublic(string productCategorsename)
        {
            var model = new SupportTopicListModel();

            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var storeInformationSettings = _settingService.LoadSetting<StoreInformationSettings>(storeScope);

            model.SystemDesignEnabled = storeInformationSettings.ShowSystemDesign;

            if (String.IsNullOrEmpty(productCategorsename))
                return RedirectToAction("PublicInfo");

            int productCategoryId = 0;
            var result = _seoservice.GetBySlug(productCategorsename);
            if (result == null)
            {
                return RedirectToAction("PublicInfo");
            }
            else
            {
                productCategoryId = result.EntityId;
            }

            var supportdownload = _supportService.GetAllSupportDownloadByProductCategoryId(productCategoryId);

            foreach (var sDownload in supportdownload)
            {
                var supportDownList = new SupportDownloadContainer();
                supportDownList.Id = sDownload.Id;
                supportDownList.Title = sDownload.Title;
                supportDownList.DownloadId = sDownload.DownloadId;

                model.AvailableSupportDownaload.Add(supportDownList);

            }


            return View("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/SupportDownloadListPublic.cshtml", model);
        }

        public ActionResult SupportTopicPublic(string suportTopicsename)
        {
            var supporttopicmodel = new PublicSupportTopicModel();

            if (String.IsNullOrEmpty(suportTopicsename))
                return RedirectToAction("PublicInfo");

            int suportTopicId = 0;
            var result = _seoservice.GetBySlug(suportTopicsename);
            if (result == null)
            {
                return RedirectToAction("PublicInfo");
            }
            else
            {
                suportTopicId = result.EntityId;
            }

            var supportTopic = _supportService.GetSupportTopicById(suportTopicId);
            if (supportTopic == null)
            {
                var url = "~/supporttopiclist/" + ((Session["SupportCategoryId"] != null) ? Convert.ToInt32(Session["SupportCategoryId"].ToString()) : 0);
                return Redirect(Url.Content(url));
            }

            var supportTopicStepslist = _supportService.GetAllSupportTopicStepBySupportTopicId(suportTopicId);

            foreach (var step in supportTopicStepslist)
            {
                var supportTopicstep = new SupportInternalTopicStepModel();
                supportTopicstep.Id = step.Id;
                supportTopicstep.Title = step.Title;
                supportTopicstep.Description = step.Description;
                supportTopicstep.PictureId = step.PictureId;
                supportTopicstep.PictureThumbnailUrl = _pictureService.GetPictureUrl(step.PictureId, (supportTopic.TemplateId == 10) ? 390 : 500);
                supportTopicstep.DisplayOrder = step.DisplayOrder;
                supporttopicmodel.AddSupportTopicStepModel.Add(supportTopicstep);
            }
            supporttopicmodel.Id = supportTopic.Id;
            supporttopicmodel.TemplateId = supportTopic.TemplateId;
            supporttopicmodel.Title = supportTopic.Title;
            supporttopicmodel.Description = supportTopic.Description;



            return View("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/SupportTopicSinglePublic.cshtml", supporttopicmodel);
        }


        public ActionResult SupportDownloadContent(int downloadid)
        {


            var download = _downloadService.GetDownloadById(downloadid);
            if (download == null)
                return Content("Sample download is not available any more.");

            if (download.UseDownloadUrl)
            {
                return new RedirectResult(download.DownloadUrl);
            }
            else
            {
                if (download.DownloadBinary == null)
                    return Content("Download data is not available any more.");

                string fileName = !String.IsNullOrWhiteSpace(download.Filename) ? download.Filename : "SupportDowbload_MisterLandScaper";
                string contentType = !String.IsNullOrWhiteSpace(download.ContentType) ? download.ContentType : "application/octet-stream";
                return new FileContentResult(download.DownloadBinary, contentType) { FileDownloadName = fileName + download.Extension };
            }
        }

        public ActionResult HowToOnProductDetails(int productId = 0)
        {
            var model = new ProductDetailSupportCategory();

            if (productId == 0)
                return RedirectToAction("PublicInfo");

            var categoryIds = _categoryService.GetProductCategoriesByProductId(productId).Select(x => x.CategoryId);
            if ( categoryIds != null && categoryIds.Count() > 0)
            {
                var videobyproductId = _supportService.GetAllSupportVideoByProductCategoryIds(productId, categoryIds.ToList());
                foreach (var item in videobyproductId)
                {
                    var suppvideo = new ProductDetailSupportVideo();
                    suppvideo.SupportVideoId = item.Id;
                    suppvideo.Title = item.Title;
                    suppvideo.PictureId = item.PictureId;
                    suppvideo.VimeoInformation = item.VimeoInformation;
                    suppvideo.PictureUrl = _pictureService.GetPictureUrl(_pictureService.GetPictureById(item.PictureId), 201);

                    model.AvailableSupportVideo.Add(suppvideo);
                }
            }
            else
            {
                var videobyproductId = _supportService.GetAllSupportVideoByProductCategoryIds(productId);
                foreach (var item in videobyproductId)
                {
                    var suppvideo = new ProductDetailSupportVideo();
                    suppvideo.SupportVideoId = item.Id;
                    suppvideo.Title = item.Title;
                    suppvideo.PictureId = item.PictureId;
                    suppvideo.VimeoInformation = item.VimeoInformation;
                    suppvideo.PictureUrl = _pictureService.GetPictureUrl(_pictureService.GetPictureById(item.PictureId), 201);

                    model.AvailableSupportVideo.Add(suppvideo);
                }
            }

            if (categoryIds != null && categoryIds.Count() > 0)
            {
                var downloadbyproductcatId = _supportService.GetAllSupportDownloadByProductCategoryIds(productId, categoryIds.ToList());
                foreach (var item in downloadbyproductcatId)
                {
                    var suppdown = new ProductDetailSupportDownload();
                    suppdown.Id = item.Id;
                    suppdown.Title = item.Title;
                    suppdown.DownloadId = item.DownloadId;

                    model.AvailableSupportDownload.Add(suppdown);
                }
            }
            else
            {
                var downbyproductId = _supportService.GetAllSupportDownloadByProductCategoryIds(productId);
                foreach (var item in downbyproductId)
                {
                    var suppdown = new ProductDetailSupportDownload();
                    suppdown.Id = item.Id;
                    suppdown.Title = item.Title;
                    suppdown.DownloadId = item.DownloadId;

                    model.AvailableSupportDownload.Add(suppdown);
                }

            }




            var catId = _categoryService.GetProductCategoriesByProductId(productId);

            foreach (var sCategory in catId)
            {

                // var supportCategories = _supportService.GetAllSupportCategoryByProductCategoryId(sCategory.CategoryId);
                var supportCategories = _supportService.GetAllSupportCategoryBySupporttopicFilter(0, sCategory.CategoryId);
                foreach (var item in supportCategories)
                {
                    if (model.AvailableSupportCategories.Where(x => x.SupportCategoryId == item.Id).FirstOrDefault() == null)
                    {
                        var supportCat = new ProductDetailSupportCategoryList();
                        supportCat.SupportCategoryId = item.Id;
                        supportCat.SupportCategoryName = item.Title;
                        supportCat.SupportCategoryPictureId = item.PictureId;
                        supportCat.SupportCategoryPictureUrl = _pictureService.GetPictureUrl(_pictureService.GetPictureById(item.PictureId), 75);
                        model.AvailableSupportCategories.Add(supportCat);

                    }

                }

                var supportTopics = _supportService.GetAllSupportTopicsByProductCategoryFilter(sCategory.CategoryId);
                foreach (var item in supportTopics)
                {
                    if (model.AvailableSupportTopics.Where(x => x.SupportTopicId == item.Id).FirstOrDefault() == null)
                    {
                        var topiclist = new ProductDetailSupportTopicListModel();
                        topiclist.SupportTopicId = item.Id;
                        topiclist.SupportTopicName = item.Title;
                        topiclist.Sename = item.GetSeName();
                        topiclist.SupportCategoryId = item.SupportTopicSupportCategory.Select(x => x.SupportCategoryId).ToArray();
                        foreach (var topiccategory in item.SupportTopicSupportCategory)
                        {
                            if (model.AvailableSupportCategories.Where(x => x.SupportCategoryId == topiccategory.SupportCategoryId).FirstOrDefault() == null)
                            {
                                var supportCat = new ProductDetailSupportCategoryList();
                                supportCat.SupportCategoryId = topiccategory.SupportCategoryId;
                                supportCat.SupportCategoryName = topiccategory.SupportCategory.Title;
                                supportCat.SupportCategoryPictureId = topiccategory.SupportCategory.PictureId;
                                supportCat.SupportCategoryPictureUrl = _pictureService.GetPictureUrl(_pictureService.GetPictureById(topiccategory.SupportCategory.PictureId), 75);
                                model.AvailableSupportCategories.Add(supportCat);

                            }

                        }
                        model.AvailableSupportTopics.Add(topiclist);
                    }

                }
            }



            var supportTopicsbyproduct = _supportService.GetAllSupportTopicsByProductIdFilter(productId);
            foreach (var item in supportTopicsbyproduct)
            {
                if (model.AvailableSupportTopics.Where(x => x.SupportTopicId == item.Id).FirstOrDefault() == null)
                {
                    var topiclist = new ProductDetailSupportTopicListModel();
                    topiclist.SupportTopicId = item.Id;
                    topiclist.SupportTopicName = item.Title;
                    topiclist.Sename = item.GetSeName();
                    topiclist.SupportCategoryId = item.SupportTopicSupportCategory.Select(x => x.SupportCategoryId).ToArray();
                    foreach (var topiccategory in item.SupportTopicSupportCategory)
                    {
                        if (model.AvailableSupportCategories.Where(x => x.SupportCategoryId == topiccategory.SupportCategoryId).FirstOrDefault() == null)
                        {
                            var supportCat = new ProductDetailSupportCategoryList();
                            supportCat.SupportCategoryId = topiccategory.SupportCategoryId;
                            supportCat.SupportCategoryName = topiccategory.SupportCategory.Title;
                            supportCat.SupportCategoryPictureId = topiccategory.SupportCategory.PictureId;
                            supportCat.SupportCategoryPictureUrl = _pictureService.GetPictureUrl(_pictureService.GetPictureById(topiccategory.SupportCategory.PictureId), 75);
                            model.AvailableSupportCategories.Add(supportCat);

                        }

                    }
                    model.AvailableSupportTopics.Add(topiclist);
                }

            }


            //foreach (var item in supporttopicsbyproductid)
            //{
            //    if (model.AvailableSupportCategories.Where(x => x.SupportCategoryId == item.).FirstOrDefault() == null)
            //    {
            //        var supportCat = new ProductDetailSupportCategoryList();
            //        supportCat.SupportCategoryId = topiccategory.SupportCategoryId;
            //        supportCat.SupportCategoryName = topiccategory.SupportCategory.Title;
            //        supportCat.SupportCategoryPictureId = topiccategory.SupportCategory.PictureId;
            //        supportCat.SupportCategoryPictureUrl = _pictureService.GetPictureUrl(_pictureService.GetPictureById(topiccategory.SupportCategory.PictureId), 75);
            //        model.AvailableSupportCategories.Add(supportCat);

            //    }


            //    if (model.AvailableSupportCategories.Where(x => x.SupportCategoryId == item.Id).FirstOrDefault() == null)
            //    {
            //        var supportCat = new ProductDetailSupportCategoryList();
            //        supportCat.SupportCategoryId = item.Id;
            //        supportCat.SupportCategoryName = item.Title;
            //        supportCat.SupportCategoryPictureId = item.PictureId;
            //        supportCat.SupportCategoryPictureUrl = _pictureService.GetPictureUrl(_pictureService.GetPictureById(item.PictureId), 75);
            //        model.AvailableSupportCategories.Add(supportCat);

            //    }
            //}
            //foreach (var item in supportCategoriesbyproductid)
            //{
            //    if (model.AvailableSupportCategories.Where(x => x.SupportCategoryId == item.Id).FirstOrDefault() == null)
            //    {

            //    }
            //    else 
            //    {
            //        var cat = model.AvailableSupportCategories.Where(x => x.SupportCategoryId == item.Id).FirstOrDefault();
            //        var topic = _supportService.GetAllSupportTopicByProductid(item.Id);
            //        supportCat.IsSupportTopicAvalable = (topic.Count > 0) ? true : false;
            //        foreach (var topicitem in topic)
            //        {
            //            var topiclist = new ProductDetailSupportTopicListModel();
            //            topiclist.SupportTopicId = topicitem.Id;
            //            topiclist.SupportTopicName = topicitem.Title;
            //            topiclist.Sename = topicitem.GetSeName();
            //            supportCat.AvailableSupportTopics.Add(topiclist);
            //        }
            //        cat.AvailableSupportTopics.Add();
            //    }
            //    var supportCat = new ProductDetailSupportCategoryList();
            //    supportCat.SupportCategoryId = item.Id;
            //    supportCat.SupportCategoryName = item.Title;
            //    supportCat.SupportCategoryPictureId = item.PictureId;
            //    supportCat.SupportCategoryPictureUrl = _pictureService.GetPictureUrl(_pictureService.GetPictureById(item.PictureId), 75);

            //    //var topic = _supportService.GetAllSupportTopicBySupportCategoryId(item.Id);
            //    var topic = _supportService.GetAllSupportTopicBySupportCategoryId(item.Id);
            //    supportCat.IsSupportTopicAvalable = (topic.Count > 0) ? true : false;
            //    foreach (var topicitem in topic)
            //    {
            //        var topiclist = new ProductDetailSupportTopicListModel();
            //        topiclist.SupportTopicId = topicitem.Id;
            //        topiclist.SupportTopicName = topicitem.Title;
            //        topiclist.Sename = topicitem.GetSeName();
            //        supportCat.AvailableSupportTopics.Add(topiclist);
            //    }


            //    model.AvailableSupportCategories.Add(supportCat);
            //}

            return View("~/Plugins/Widgets.TorchDesign_Support/Views/TorchDesign_Support/HowToOnProductDetails.cshtml", model);
        }
        #endregion
    }
}