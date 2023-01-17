using System;
using System.Linq;
using System.Web.Mvc;
using Nop.Admin.Models.Catalog;
using Nop.Core.Domain.Catalog;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Security;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Kendoui;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Controllers
{
    public partial class SpecificationGroupController : BaseAdminController
    {
        #region Fields

        private readonly ISpecificationAttributeGroupService _specificationAttributeService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly ILocalizationService _localizationService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IPermissionService _permissionService;
        private readonly IProductService _productService;

        #endregion Fields

        #region Constructors

        public SpecificationGroupController(ISpecificationAttributeGroupService specificationAttributeService,
            ILanguageService languageService, 
            ILocalizedEntityService localizedEntityService,
            ILocalizationService localizationService, 
            ICustomerActivityService customerActivityService,
            IPermissionService permissionService, IProductService productService)
        {
            this._specificationAttributeService = specificationAttributeService;
            this._languageService = languageService;
            this._localizedEntityService = localizedEntityService;
            this._localizationService = localizationService;
            this._customerActivityService = customerActivityService;
            this._permissionService = permissionService;
            this._productService = productService;
        }

        #endregion
        
        
        
        #region Specification attributes

        //list
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageAttributes))
                return AccessDeniedView();
            SpecificationGroupModel model = new SpecificationGroupModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult List(DataSourceRequest command, int optionId)
        {

            var Grouplist = _specificationAttributeService.GetAllSpecificationGroup();
            var Specificationgroup = Grouplist
                .Select(x =>
                {
                    return new SpecificationAttributeGroupList()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        DisplayOrder = x.DisplayOrder,
                    };
                })
                .ToList();

            var gridModel = new DataSourceResult
            {
                Data = Specificationgroup,
                Total = Specificationgroup.Count
            };

            return Json(gridModel);
          
        }


        [HttpPost]
        public ActionResult GroupInsert(SpecificationGroupModel model)
        {

            var group = new SpecificationAttributeGroup()
            {
                Name  = model.Name,
                DisplayOrder = model.DisplayOrder,
            };

            _specificationAttributeService.InsertSpecificationGroup(group);

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult GroupUpdate(SpecificationGroupModel model)
        {
           
            var Group = _specificationAttributeService.GetSpecificationGroupById(model.Id);

            Group.Name = model.Name;
            Group.DisplayOrder = model.DisplayOrder;
            _specificationAttributeService.UpdateSpecificationGroup(Group);
            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult GroupDelete(SpecificationGroupModel model)
        {
            var Group = _specificationAttributeService.GetSpecificationGroupById(model.Id);
            if (Group == null)
                throw new ArgumentException("No Group category mapping found with the specified id");

            _specificationAttributeService.DeleteSpecificationGroup(Group);

            return new NullJsonResult();
        }

        #endregion

        
    }
}
