using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Nop.Core;
using Nop.Plugin.TorchDesign.CustomerOrigin.Services;
using Nop.Core.Domain.Directory;
using Nop.Plugin.TorchDesign.CustomerOrigin.Domain;
using Nop.Plugin.TorchDesign.CustomerOrigin.Models;
using Nop.Services.Configuration;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Kendoui;
using Nop.Web.Framework.Mvc;
using Nop.Plugin.TorchDesign.CustomerOrigin;
using Nop.Services.Stores;
using System.Xml;
using System.IO;
using System.Net;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using Nop.Services.Helpers;

namespace Nop.Plugin.TorchDesign.CustomerOrigin.Controllers
{

    public partial class TorchDesignCustomerOriginController : BasePluginController
    {
        #region Fields
        private readonly IWorkContext _workContext;
        private readonly ICustomerOriginService _customerorigin;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly ISettingService _settingService;
        private readonly IStoreService _storeService;
        private readonly IStoreContext _storeContext;
        private readonly TorchDesignCustomerOriginSettings _originsetting;
        private readonly IDateTimeHelper _dateTimeHelper;

        #endregion

        public TorchDesignCustomerOriginController(ICustomerOriginService customerorigin, ILanguageService languageService,
            ILocalizationService localizationService,
            ILocalizedEntityService localizedEntityService,
             IWorkContext workContext,
             ISettingService settingService, IStoreService storeService, IStoreContext storeContext, TorchDesignCustomerOriginSettings originsetting, IDateTimeHelper dateTimeHelper
            )
        {

            this._languageService = languageService;
            this._customerorigin = customerorigin;
            this._localizationService = localizationService;
            this._localizedEntityService = localizedEntityService;
            this._workContext = workContext;
            this._settingService = settingService;
            this._storeService = storeService;
            this._storeContext = storeContext;
            this._originsetting = originsetting;
            this._dateTimeHelper = dateTimeHelper;

        }

        #region Utilities
        #region Configure

       public ActionResult Configure()
        {ConfigurationModel model=new ConfigurationModel();
        return View("~/Plugins/TorchDesign.CustomerOrigin/Views/TorchDesignCustomerOrigin/Configure.cshtml", model);
       }
        [HttpPost]
         public ActionResult Configure(ConfigurationModel model)
        {
            return View("~/Plugins/TorchDesign.CustomerOrigin/Views/TorchDesignCustomerOrigin/Configure.cshtml", model);
        }

        public ActionResult Chart()
        {
            // ChartModel = new ChartModel();
            var point = new List<string>();
            var availableoption = _customerorigin.GetAllCustomerOrigin().Where(x => x.Publish = true);
            foreach (var item in availableoption)
            {
                var custanswer = _customerorigin.GetCustomerOriginAnswerByOptionid(item.Id);
                point.Add(string.Format("{{y:{0},legendText:\"{1}\",indexLabel:\"{2}\"}}", custanswer.Count, item.OptionName + "(" + custanswer.Count + ")", item.OptionName + "(" + custanswer.Count + ")").Replace(" ",""));
            }
            var PointJson = "[" + string.Join(",", point.ToArray()) + "]";
            ChartModel model = new ChartModel();
            model.Charatdata = PointJson;
            model.IsResultfound = true;
              return View("~/Plugins/TorchDesign.CustomerOrigin/Views/TorchDesignCustomerOrigin/Chart.cshtml",model);
           
        }
        [HttpPost]
        public ActionResult Chart(ChartModel model)
        {
            DateTime? startDateValue = (model.StartDate == null) ? null
                                       : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.StartDate.Value, _dateTimeHelper.CurrentTimeZone);

            DateTime? endDateValue = (model.EndDate == null) ? null
                            : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.EndDate.Value, _dateTimeHelper.CurrentTimeZone).AddDays(1);


            var point = new List<string>();
            var availableoption = _customerorigin.GetAllCustomerOrigin().Where(x => x.Publish = true);
            model.IsResultfound = false;
            foreach (var item in availableoption)
            {
                var Reviewlist = _customerorigin.Findbydate(createdFromUtc: startDateValue, createdToUtc: endDateValue,id: item.Id);
                point.Add(string.Format("{{y:{0},legendText:\"{1}\",indexLabel:\"{2}\"}}", Reviewlist.Count, item.OptionName, item.OptionName));
                
                if (Reviewlist.Count > 0)
                    model.IsResultfound = true;
            }

            var PointJson = "[" + string.Join(",", point.ToArray()) + "]";
            model.Charatdata = PointJson;
            return View("~/Plugins/TorchDesign.CustomerOrigin/Views/TorchDesignCustomerOrigin/Chart.cshtml", model);
        }

        [HttpPost]
        public ActionResult CustomerOriginList(DataSourceRequest command, int optionId)
        {
           
            var CustOrigin = _customerorigin.GetAllCustomerOrigin();
            var CustOriginModel = CustOrigin
                .Select(x =>
                {
                    return new ConfigurationModel()
                    {
                        Id = x.Id,
                        OptionName = x.OptionName,
                        DefaultSelected = x.DefaultSelected,
                        Publish = x.Publish
                    };
                })
                .ToList();

            var gridModel = new DataSourceResult
            {
                Data = CustOriginModel,
                Total = CustOriginModel.Count
            };

            return Json(gridModel);
        }

        //public ActionResult MarkAsPrimarySelected(string optionName)
        //{
        //    if (String.IsNullOrEmpty(optionName))
        //    {
        //        return RedirectToAction("Configure");
        //    }

        //    var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
        //    var CustomSetting = _settingService.LoadSetting<TorchDesignCustomerOriginSettings>(storeScope);
         
        //   if (CustomSetting.ActiveOptionName == optionName)
        //    {
        //        return RedirectToAction("Configure");
        //    }
        //    else 
        //    {
        //        CustomSetting.ActiveOptionName = optionName;
        //        _settingService.SaveSetting(CustomSetting);
        //        _settingService.ClearCache();
        //        return RedirectToAction("Configure");
        //    }
            
        //}

        [HttpPost]
        public ActionResult CustomerOriginInsert(ConfigurationModel model)
        {
              
                var CustOrigin = new Td_CustomerOrigin()
                {
                    OptionName=model.OptionName,
                    Publish=model.Publish,
                    DefaultSelected=model.DefaultSelected
                };
              
                _customerorigin.InsertCustomerOrigin(CustOrigin);
         
            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult CustomerOriginUpdate(ConfigurationModel model)
        {
            if(model.DefaultSelected == true)
            { 
            var listofrecord = _customerorigin.GetAllCustomerOrigin().Where(x=>x.DefaultSelected == true);
            foreach (var item in listofrecord)
            {
                item.DefaultSelected = false;
                _customerorigin.UpdateCustomerOrigin(item);
            }

            }
            var Customorg = _customerorigin.GetCustomerOriginById(model.Id);
          
            Customorg.Publish = model.Publish;
            Customorg.DefaultSelected = model.DefaultSelected;
            Customorg.OptionName = model.OptionName;
            
            _customerorigin.UpdateCustomerOrigin(Customorg);
            return new NullJsonResult();
        }

        //[HttpPost]
        //public ActionResult CustomerOriginDelete(int id)
        //{
        //    //if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
        //    //    return AccessDeniedView();

        //    //var productCategory = _categoryService.GetProductCategoryById(id);
        //    //if (productCategory == null)
        //    //    throw new ArgumentException("No product category mapping found with the specified id");

        //    //var optionId = productCategory.optionId;

        //    ////a vendor should have access only to his products
        //    //if (_workContext.CurrentVendor != null)
        //    //{
        //    //    var product = _productService.GetProductById(optionId);
        //    //    if (product != null && product.VendorId != _workContext.CurrentVendor.Id)
        //    //    {
        //    //        return Content("This is not your product");
        //    //    }
        //    //}

        //    //_categoryService.DeleteProductCategory(productCategory);

        //    return new NullJsonResult();
        //}

            
        #endregion

        

        #endregion

    }
}
