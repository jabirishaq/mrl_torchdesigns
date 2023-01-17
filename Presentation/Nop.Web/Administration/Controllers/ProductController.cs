using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Nop.Admin.Models.Catalog;
using Nop.Admin.Models.Orders;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Discounts;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Orders;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Discounts;
using Nop.Services.ExportImport;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Orders;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Shipping;
using Nop.Services.Stores;
using Nop.Services.Tax;
using Nop.Services.Vendors;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Kendoui;
using Nop.Web.Framework.Mvc;
using Nop.Core.Infrastructure;
using Nop.Plugin.Widgets.TorchDesign_Support;
using Nop.Plugin.Widgets.TorchDesign_Support.Services;
using Nop.Core.Plugins;
using Nop.Plugin.Widgets.TorchDesign_Support.Domain;


namespace Nop.Admin.Controllers
{
    public partial class ProductController : BaseAdminController
    {
        #region Fields
        private readonly IProductThumbService _productthumbService;
        private readonly IProductService _productService;
        private readonly IProductTemplateService _productTemplateService;
        private readonly ICategoryService _categoryService;
        private readonly IManufacturerService _manufacturerService;
        private readonly ICustomerService _customerService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IWorkContext _workContext;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly ISpecificationAttributeService _specificationAttributeService;
        private readonly IPictureService _pictureService;
        private readonly ITaxCategoryService _taxCategoryService;
        private readonly IProductTagService _productTagService;
        private readonly IProductServiceCustome _productservicecustome;
        private readonly ICopyProductService _copyProductService;
        private readonly IPdfService _pdfService;
        private readonly IExportManager _exportManager;
        private readonly IImportManager _importManager;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IPermissionService _permissionService;
        private readonly IAclService _aclService;
        private readonly IStoreService _storeService;
        private readonly IOrderService _orderService;
        private readonly IStoreMappingService _storeMappingService;
        private readonly IVendorService _vendorService;
        private readonly IShippingService _shippingService;
        private readonly ICurrencyService _currencyService;
        private readonly CurrencySettings _currencySettings;
        private readonly IMeasureService _measureService;
        private readonly MeasureSettings _measureSettings;
        private readonly AdminAreaSettings _adminAreaSettings;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IDiscountService _discountService;
        private readonly IProductAttributeService _productAttributeService;
        private readonly IBackInStockSubscriptionService _backInStockSubscriptionService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IProductAttributeFormatter _productAttributeFormatter;
        private readonly IProductAttributeParser _productAttributeParser;
        private readonly IDownloadService _downloadService;
        private readonly ISpecificationAttributeGroupService _specificationgroupservice;
        private readonly ISupportService _supportservice;

        #endregion

        #region Constructors

        public ProductController(IProductThumbService productthumbService, IProductService productService, IProductServiceCustome productservicecustome,
            IProductTemplateService productTemplateService,
            ICategoryService categoryService,
            IManufacturerService manufacturerService,
            ICustomerService customerService,
            IUrlRecordService urlRecordService,
            IWorkContext workContext,
            ILanguageService languageService,
            ILocalizationService localizationService,
            ILocalizedEntityService localizedEntityService,
            ISpecificationAttributeService specificationAttributeService,
            IPictureService pictureService,
            ITaxCategoryService taxCategoryService,
            IProductTagService productTagService,
            ICopyProductService copyProductService,
            IPdfService pdfService,
            IExportManager exportManager,
            IImportManager importManager,
            ICustomerActivityService customerActivityService,
            IPermissionService permissionService,
            IAclService aclService,
            IStoreService storeService,
            IOrderService orderService,
            IStoreMappingService storeMappingService,
             IVendorService vendorService,
            IShippingService shippingService,
            ICurrencyService currencyService,
            CurrencySettings currencySettings,
            IMeasureService measureService,
            MeasureSettings measureSettings,
            AdminAreaSettings adminAreaSettings,
            IDateTimeHelper dateTimeHelper,
            IDiscountService discountService,
            IProductAttributeService productAttributeService,
            IBackInStockSubscriptionService backInStockSubscriptionService,
            IShoppingCartService shoppingCartService,
            IProductAttributeFormatter productAttributeFormatter,
            IProductAttributeParser productAttributeParser,
            IDownloadService downloadService, ISpecificationAttributeGroupService specificationgroupservice, ISupportService supportservice)
        {
            this._productthumbService = productthumbService;
            this._productService = productService;
            this._productservicecustome = productservicecustome;
            this._productTemplateService = productTemplateService;
            this._categoryService = categoryService;
            this._manufacturerService = manufacturerService;
            this._customerService = customerService;
            this._urlRecordService = urlRecordService;
            this._workContext = workContext;
            this._languageService = languageService;
            this._localizationService = localizationService;
            this._localizedEntityService = localizedEntityService;
            this._specificationAttributeService = specificationAttributeService;
            this._pictureService = pictureService;
            this._taxCategoryService = taxCategoryService;
            this._productTagService = productTagService;
            this._copyProductService = copyProductService;
            this._pdfService = pdfService;
            this._exportManager = exportManager;
            this._importManager = importManager;
            this._customerActivityService = customerActivityService;
            this._permissionService = permissionService;
            this._aclService = aclService;
            this._storeService = storeService;
            this._orderService = orderService;
            this._storeMappingService = storeMappingService;
            this._vendorService = vendorService;
            this._shippingService = shippingService;
            this._currencyService = currencyService;
            this._currencySettings = currencySettings;
            this._measureService = measureService;
            this._measureSettings = measureSettings;
            this._adminAreaSettings = adminAreaSettings;
            this._dateTimeHelper = dateTimeHelper;
            this._discountService = discountService;
            this._productAttributeService = productAttributeService;
            this._backInStockSubscriptionService = backInStockSubscriptionService;
            this._shoppingCartService = shoppingCartService;
            this._productAttributeFormatter = productAttributeFormatter;
            this._productAttributeParser = productAttributeParser;
            this._downloadService = downloadService;
            this._specificationgroupservice = specificationgroupservice;
            this._supportservice = supportservice;
        }

        #endregion

        #region Utilities

        [NonAction]
        protected virtual void UpdateLocales(Product product, ProductModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(product,
                                                               x => x.Name,
                                                               localized.Name,
                                                               localized.LanguageId);
                _localizedEntityService.SaveLocalizedValue(product,
                                                               x => x.ShortDescription,
                                                               localized.ShortDescription,
                                                               localized.LanguageId);
                _localizedEntityService.SaveLocalizedValue(product,
                                                               x => x.FullDescription,
                                                               localized.FullDescription,
                                                               localized.LanguageId);
                _localizedEntityService.SaveLocalizedValue(product,
                                                               x => x.MetaKeywords,
                                                               localized.MetaKeywords,
                                                               localized.LanguageId);
                _localizedEntityService.SaveLocalizedValue(product,
                                                               x => x.MetaDescription,
                                                               localized.MetaDescription,
                                                               localized.LanguageId);
                _localizedEntityService.SaveLocalizedValue(product,
                                                               x => x.MetaTitle,
                                                               localized.MetaTitle,
                                                               localized.LanguageId);

                //search engine name
                var seName = product.ValidateSeName(localized.SeName, localized.Name, false);
                _urlRecordService.SaveSlug(product, seName, localized.LanguageId);
            }
        }

        [NonAction]
        protected virtual void UpdateLocales(ProductTag productTag, ProductTagModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(productTag,
                                                               x => x.Name,
                                                               localized.Name,
                                                               localized.LanguageId);
            }
        }

        [NonAction]
        protected virtual void UpdateLocales(ProductVariantAttributeValue pvav, ProductModel.ProductVariantAttributeValueModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(pvav,
                                                               x => x.Name,
                                                               localized.Name,
                                                               localized.LanguageId);
            }
        }

        [NonAction]
        protected virtual void UpdatePictureSeoNames(Product product)
        {
            foreach (var pp in product.ProductPictures)
                _pictureService.SetSeoFilename(pp.PictureId, _pictureService.GetPictureSeName(product.Name));
        }

        [NonAction]
        protected virtual void PrepareAclModel(ProductModel model, Product product, bool excludeProperties)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            model.AvailableCustomerRoles = _customerService
                .GetAllCustomerRoles(true)
                .Select(cr => cr.ToModel())
                .ToList();
            if (!excludeProperties)
            {
                if (product != null)
                {
                    model.SelectedCustomerRoleIds = _aclService.GetCustomerRoleIdsWithAccess(product);
                }
                else
                {
                    model.SelectedCustomerRoleIds = new int[0];
                }
            }
        }

        [NonAction]
        protected virtual void SaveProductAcl(Product product, ProductModel model)
        {
            var existingAclRecords = _aclService.GetAclRecords(product);
            var allCustomerRoles = _customerService.GetAllCustomerRoles(true);
            foreach (var customerRole in allCustomerRoles)
            {
                if (model.SelectedCustomerRoleIds != null && model.SelectedCustomerRoleIds.Contains(customerRole.Id))
                {
                    //new role
                    if (existingAclRecords.Count(acl => acl.CustomerRoleId == customerRole.Id) == 0)
                        _aclService.InsertAclRecord(product, customerRole.Id);
                }
                else
                {
                    //remove role
                    var aclRecordToDelete = existingAclRecords.FirstOrDefault(acl => acl.CustomerRoleId == customerRole.Id);
                    if (aclRecordToDelete != null)
                        _aclService.DeleteAclRecord(aclRecordToDelete);
                }
            }
        }

        [NonAction]
        protected virtual void PrepareStoresMappingModel(ProductModel model, Product product, bool excludeProperties)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            model.AvailableStores = _storeService
                .GetAllStores()
                .Select(s => s.ToModel())
                .ToList();
            if (!excludeProperties)
            {
                if (product != null)
                {
                    model.SelectedStoreIds = _storeMappingService.GetStoresIdsWithAccess(product);
                }
                else
                {
                    model.SelectedStoreIds = new int[0];
                }
            }
        }

        [NonAction]
        protected virtual void SaveStoreMappings(Product product, ProductModel model)
        {
            var existingStoreMappings = _storeMappingService.GetStoreMappings(product);
            var allStores = _storeService.GetAllStores();
            foreach (var store in allStores)
            {
                if (model.SelectedStoreIds != null && model.SelectedStoreIds.Contains(store.Id))
                {
                    //new store
                    if (existingStoreMappings.Count(sm => sm.StoreId == store.Id) == 0)
                        _storeMappingService.InsertStoreMapping(product, store.Id);
                }
                else
                {
                    //remove store
                    var storeMappingToDelete = existingStoreMappings.FirstOrDefault(sm => sm.StoreId == store.Id);
                    if (storeMappingToDelete != null)
                        _storeMappingService.DeleteStoreMapping(storeMappingToDelete);
                }
            }
        }

        [NonAction]
        protected virtual void PrepareAddProductAttributeCombinationModel(AddProductVariantAttributeCombinationModel model, Product product)
        {
            if (model == null)
                throw new ArgumentNullException("model");
            if (product == null)
                throw new ArgumentNullException("product");

            model.ProductId = product.Id;
            model.StockQuantity = 10000;

            var productVariantAttributes = _productAttributeService.GetProductVariantAttributesByProductId(product.Id);
            foreach (var attribute in productVariantAttributes)
            {
                var pvaModel = new AddProductVariantAttributeCombinationModel.ProductVariantAttributeModel()
                {
                    Id = attribute.Id,
                    ProductAttributeId = attribute.ProductAttributeId,
                    Name = attribute.ProductAttribute.Name,
                    TextPrompt = attribute.TextPrompt,
                    IsRequired = attribute.IsRequired,
                    AttributeControlType = attribute.AttributeControlType
                };

                if (attribute.ShouldHaveValues())
                {
                    //values
                    var pvaValues = _productAttributeService.GetProductVariantAttributeValues(attribute.Id);
                    foreach (var pvaValue in pvaValues)
                    {
                        var pvaValueModel = new AddProductVariantAttributeCombinationModel.ProductVariantAttributeValueModel()
                        {
                            Id = pvaValue.Id,
                            Name = pvaValue.Name,
                            IsPreSelected = pvaValue.IsPreSelected
                        };
                        pvaModel.Values.Add(pvaValueModel);
                    }
                }

                model.ProductVariantAttributes.Add(pvaModel);
            }
        }

        [NonAction]
        protected virtual string[] ParseProductTags(string productTags)
        {
            var result = new List<string>();
            if (!String.IsNullOrWhiteSpace(productTags))
            {
                string[] values = productTags.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string val1 in values)
                    if (!String.IsNullOrEmpty(val1.Trim()))
                        result.Add(val1.Trim());
            }
            return result.ToArray();
        }

        [NonAction]
        protected virtual void SaveProductTags(Product product, string[] productTags)
        {
            if (product == null)
                throw new ArgumentNullException("product");

            //product tags
            var existingProductTags = product.ProductTags.ToList();
            var productTagsToRemove = new List<ProductTag>();
            foreach (var existingProductTag in existingProductTags)
            {
                bool found = false;
                foreach (string newProductTag in productTags)
                {
                    if (existingProductTag.Name.Equals(newProductTag, StringComparison.InvariantCultureIgnoreCase))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    productTagsToRemove.Add(existingProductTag);
                }
            }
            foreach (var productTag in productTagsToRemove)
            {
                product.ProductTags.Remove(productTag);
                _productService.UpdateProduct(product);
            }
            foreach (string productTagName in productTags)
            {
                ProductTag productTag = null;
                var productTag2 = _productTagService.GetProductTagByName(productTagName);
                if (productTag2 == null)
                {
                    //add new product tag
                    productTag = new ProductTag()
                    {
                        Name = productTagName
                    };
                    _productTagService.InsertProductTag(productTag);
                }
                else
                {
                    productTag = productTag2;
                }
                if (!product.ProductTagExists(productTag.Id))
                {
                    product.ProductTags.Add(productTag);
                    _productService.UpdateProduct(product);
                }
            }
        }

        [NonAction]
        protected virtual void PrepareProductModel(ProductModel model, Product product,
            bool setPredefinedValues, bool excludeProperties)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            if (product != null)
            {
                var parentGroupedProduct = _productService.GetProductById(product.ParentGroupedProductId);
                if (parentGroupedProduct != null)
                {
                    model.AssociatedToProductId = product.ParentGroupedProductId;
                    model.AssociatedToProductName = parentGroupedProduct.Name;
                }
            }

            model.PrimaryStoreCurrencyCode = _currencyService.GetCurrencyById(_currencySettings.PrimaryStoreCurrencyId).CurrencyCode;
            model.BaseWeightIn = _measureService.GetMeasureWeightById(_measureSettings.BaseWeightId).Name;
            model.BaseDimensionIn = _measureService.GetMeasureDimensionById(_measureSettings.BaseDimensionId).Name;
            if (product != null)
            {
                model.CreatedOn = _dateTimeHelper.ConvertToUserTime(product.CreatedOnUtc, DateTimeKind.Utc);
                model.UpdatedOn = _dateTimeHelper.ConvertToUserTime(product.UpdatedOnUtc, DateTimeKind.Utc);
            }

            model.NumberOfAvailableProductAttributes = _productAttributeService.GetAllProductAttributes(0, 1).TotalCount;
            model.NumberOfAvailableManufacturers = _manufacturerService.GetAllManufacturers("",
                pageIndex: 0,
                pageSize: 1,
                showHidden: true).TotalCount;
            model.NumberOfAvailableCategories = _categoryService.GetAllCategories(
                pageIndex: 0,
                pageSize: 1,
                showHidden: true).TotalCount;

            //copy product
            if (product != null)
            {
                model.CopyProductModel.Id = product.Id;
                model.CopyProductModel.Name = "Copy of " + product.Name;
                model.CopyProductModel.Published = true;
                model.CopyProductModel.CopyImages = true;
            }

            //templates
            var templates = _productTemplateService.GetAllProductTemplates();
            foreach (var template in templates)
            {
                model.AvailableProductTemplates.Add(new SelectListItem()
                {
                    Text = template.Name,
                    Value = template.Id.ToString()
                });
            }

            //vendors
            model.IsLoggedInAsVendor = _workContext.CurrentVendor != null;
            model.AvailableVendors.Add(new SelectListItem()
            {
                Text = _localizationService.GetResource("Admin.Catalog.Products.Fields.Vendor.None"),
                Value = "0"
            });
            var vendors = _vendorService.GetAllVendors(0, int.MaxValue, true);
            foreach (var vendor in vendors)
            {
                model.AvailableVendors.Add(new SelectListItem()
                {
                    Text = vendor.Name,
                    Value = vendor.Id.ToString()
                });
            }

            //delivery dates
            model.AvailableDeliveryDates.Add(new SelectListItem()
            {
                Text = _localizationService.GetResource("Admin.Catalog.Products.Fields.DeliveryDate.None"),
                Value = "0"
            });
            var deliveryDates = _shippingService.GetAllDeliveryDates();
            foreach (var deliveryDate in deliveryDates)
            {
                model.AvailableDeliveryDates.Add(new SelectListItem()
                {
                    Text = deliveryDate.Name,
                    Value = deliveryDate.Id.ToString()
                });
            }

            //warehouses
            model.AvailableWarehouses.Add(new SelectListItem()
            {
                Text = _localizationService.GetResource("Admin.Catalog.Products.Fields.Warehouse.None"),
                Value = "0"
            });
            var warehouses = _shippingService.GetAllWarehouses();
            foreach (var warehouse in warehouses)
            {
                model.AvailableWarehouses.Add(new SelectListItem()
                {
                    Text = warehouse.Name,
                    Value = warehouse.Id.ToString()
                });
            }

            //product tags
            if (product != null)
            {
                var result = new StringBuilder();
                for (int i = 0; i < product.ProductTags.Count; i++)
                {
                    var pt = product.ProductTags.ToList()[i];
                    result.Append(pt.Name);
                    if (i != product.ProductTags.Count - 1)
                        result.Append(", ");
                }
                model.ProductTags = result.ToString();
            }

            //tax categories
            var taxCategories = _taxCategoryService.GetAllTaxCategories();
            model.AvailableTaxCategories.Add(new SelectListItem() { Text = "---", Value = "0" });
            foreach (var tc in taxCategories)
                model.AvailableTaxCategories.Add(new SelectListItem() { Text = tc.Name, Value = tc.Id.ToString(), Selected = product != null && !setPredefinedValues && tc.Id == product.TaxCategoryId });


            //specification attributes Group
            var specificationAttributeGroup = _specificationgroupservice.GetAllSpecificationGroup();
            for (int i = 0; i < specificationAttributeGroup.Count; i++)
            {
                var sa = specificationAttributeGroup[i];
                model.AddSpecificationAttributeModel.AvailableGroupOptions.Add(new SelectListItem() { Text = sa.Name, Value = sa.Id.ToString() });

            }

            //specification attributes
            var specificationAttributes = _specificationAttributeService.GetSpecificationAttributes();
            for (int i = 0; i < specificationAttributes.Count; i++)
            {
                var sa = specificationAttributes[i];
                model.AddSpecificationAttributeModel.AvailableAttributes.Add(new SelectListItem() { Text = sa.Name, Value = sa.Id.ToString() });
                if (i == 0)
                {
                    //attribute options
                    foreach (var sao in _specificationAttributeService.GetSpecificationAttributeOptionsBySpecificationAttribute(sa.Id))
                        model.AddSpecificationAttributeModel.AvailableOptions.Add(new SelectListItem() { Text = sao.Name, Value = sao.Id.ToString() });
                }
            }
            //default specs values
            model.AddSpecificationAttributeModel.ShowOnProductPage = true;

            //discounts
            model.AvailableDiscounts = _discountService
                .GetAllDiscounts(DiscountType.AssignedToSkus, null, true)
                .Select(d => d.ToModel())
                .ToList();
            if (!excludeProperties && product != null)
            {
                model.SelectedDiscountIds = product.AppliedDiscounts.Select(d => d.Id).ToArray();
            }


            //Maximum Run
            model.AvailableRuns = RunType.PolyDetail.ToSelectList(false).ToList();
            model.AvailableRuns.Insert(0, new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.None"), Value = "0" });

            //default values
            if (setPredefinedValues)
            {
                model.MaximumCustomerEnteredPrice = 1000;
                model.MaxNumberOfDownloads = 10;
                model.RecurringCycleLength = 100;
                model.RecurringTotalCycles = 10;
                model.StockQuantity = 10000;
                model.NotifyAdminForQuantityBelow = 1;
                model.OrderMinimumQuantity = 1;
                model.OrderMaximumQuantity = 10000;
                //model.Quantity = "0";
                //model.ShowMaximumRuns = false;
                model.UnlimitedDownloads = true;
                model.IsShipEnabled = true;
                //model.IsEligibleforFree = true;
                model.AllowCustomerReviews = true;
                model.Published = true;
                model.VisibleIndividually = true;
            }
        }

        [NonAction]
        protected virtual List<int> GetChildCategoryIds(int parentCategoryId)
        {
            var categoriesIds = new List<int>();
            var categories = _categoryService.GetAllCategoriesByParentCategoryId(parentCategoryId, true);
            foreach (var category in categories)
            {
                categoriesIds.Add(category.Id);
                categoriesIds.AddRange(GetChildCategoryIds(category.Id));
            }
            return categoriesIds;
        }

        #endregion

        #region Methods

        #region Product list / create / edit / delete

        //list products
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var model = new ProductListModel();
            model.DisplayProductPictures = _adminAreaSettings.DisplayProductPictures;
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

            //stores
            model.AvailableWarehouses.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            foreach (var wh in _shippingService.GetAllWarehouses())
                model.AvailableWarehouses.Add(new SelectListItem() { Text = wh.Name, Value = wh.Id.ToString() });

            //vendors
            model.AvailableVendors.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            foreach (var v in _vendorService.GetAllVendors(0, int.MaxValue, true))
                model.AvailableVendors.Add(new SelectListItem() { Text = v.Name, Value = v.Id.ToString() });

            //product types
            model.AvailableProductTypes = ProductType.SimpleProduct.ToSelectList(false).ToList();
            model.AvailableProductTypes.Insert(0, new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });

            return View(model);
        }

        [HttpPost]
        public ActionResult ProductList(DataSourceRequest command, ProductListModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null)
            {
                model.SearchVendorId = _workContext.CurrentVendor.Id;
            }

            var categoryIds = new List<int>() { model.SearchCategoryId };
            //include subcategories
            if (model.SearchIncludeSubCategories && model.SearchCategoryId > 0)
                categoryIds.AddRange(GetChildCategoryIds(model.SearchCategoryId));

            var products = _productService.SearchProducts(
                categoryIds: categoryIds,
                manufacturerId: model.SearchManufacturerId,
                storeId: model.SearchStoreId,
                vendorId: model.SearchVendorId,
                warehouseId: model.SearchWarehouseId,
                productType: model.SearchProductTypeId > 0 ? (ProductType?)model.SearchProductTypeId : null,
                keywords: model.SearchProductName,
                pageIndex: command.Page - 1,
                pageSize: command.PageSize,
                showHidden: true
            );
            var gridModel = new DataSourceResult();
            gridModel.Data = products.Select(x =>
            {
                var productModel = x.ToModel();
                //little hack here:
                //ensure that product full descriptions are not returned
                //otherwise, we can get the following error if products have too long descriptions:
                //"Error during serialization or deserialization using the JSON JavaScriptSerializer. The length of the string exceeds the value set on the maxJsonLength property. "
                //also it improves performance
                productModel.FullDescription = "";

                if (_adminAreaSettings.DisplayProductPictures)
                {
                    var defaultProductPicture = _pictureService.GetPicturesByProductId(x.Id, 1).FirstOrDefault();
                    productModel.PictureThumbnailUrl = _pictureService.GetPictureUrl(defaultProductPicture, 75, true);
                }
                productModel.ProductTypeName = x.ProductType.GetLocalizedEnum(_localizationService, _workContext);
                return productModel;
            });
            gridModel.Total = products.TotalCount;

            return Json(gridModel);
        }

        [HttpPost, ActionName("List")]
        [FormValueRequired("go-to-product-by-sku")]
        public ActionResult GoToSku(ProductListModel model)
        {
            string sku = model.GoDirectlyToSku;
            var product = _productService.GetProductBySku(sku);
            if (product != null)
                return RedirectToAction("Edit", "Product", new { id = product.Id });

            //not found
            return List();
        }
        [HttpPost, ActionName("List")]
        [FormValueRequired("go-to-partnumber")]
        public ActionResult GoToPartnumber(ProductListModel model)
        {
            string partnumber = model.GoDirectlyToProductpartnumber;
            var product = _productservicecustome.GetProductByPartnumber(partnumber);
            if (product != null)
                return RedirectToAction("Edit", "Product", new { id = product.Id });

            //not found
            return List();
        }
        //create product
        public ActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var model = new ProductModel();
            PrepareProductModel(model, null, true, true);

            //defalut value for produc custome field
            //model.ShowMaximumRuns = false;
            model.IsEligibleforFree = true;
            model.MadeInUsa = false;
            model.Quantity = "0";


            AddLocales(_languageService, model.Locales);
            PrepareAclModel(model, null, false);
            PrepareStoresMappingModel(model, null, false);
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Create(ProductModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                //a vendor should have access only to his products
                if (_workContext.CurrentVendor != null)
                {
                    model.VendorId = _workContext.CurrentVendor.Id;
                }
                //vendors cannot edit "Show on home page" property
                if (_workContext.CurrentVendor != null && model.ShowOnHomePage)
                {
                    model.ShowOnHomePage = false;
                }
                if (_workContext.CurrentVendor != null && model.MadeInUsa)
                {
                    model.MadeInUsa = false;
                }

                //product

                var product = model.ToEntity();
                product.CreatedOnUtc = DateTime.UtcNow;
                product.UpdatedOnUtc = DateTime.UtcNow;
                _productService.InsertProduct(product);

                //Insert produc custome field
                ProductCustomeField customfield = new ProductCustomeField();
                customfield.ProductId = product.Id;
                //customfield.ShowMaximumRuns = model.ShowMaximumRuns;
                customfield.RunId = model.RunId;
                customfield.MadeInUsa = model.MadeInUsa;
                customfield.IsEligibleforFree = model.IsEligibleforFree;
                customfield.Quantity = model.Quantity;
                _productservicecustome.InsertProductCustomeField(customfield);


                //search engine name
                model.SeName = product.ValidateSeName(model.SeName, product.Name, true);
                _urlRecordService.SaveSlug(product, model.SeName, 0);
                //locales
                UpdateLocales(product, model);
                //ACL (customer roles)
                SaveProductAcl(product, model);
                //Stores
                SaveStoreMappings(product, model);
                //tags
                SaveProductTags(product, ParseProductTags(model.ProductTags));
                //discounts
                var allDiscounts = _discountService.GetAllDiscounts(DiscountType.AssignedToSkus, null, true);
                foreach (var discount in allDiscounts)
                {
                    if (model.SelectedDiscountIds != null && model.SelectedDiscountIds.Contains(discount.Id))
                        product.AppliedDiscounts.Add(discount);
                }
                _productService.UpdateProduct(product);
                _productService.UpdateHasDiscountsApplied(product);

                //activity log
                _customerActivityService.InsertActivity("AddNewProduct", _localizationService.GetResource("ActivityLog.AddNewProduct"), product.Name);

                SuccessNotification(_localizationService.GetResource("Admin.Catalog.Products.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = product.Id }) : RedirectToAction("List");
            }

            //If we got this far, something failed, redisplay form
            PrepareProductModel(model, null, false, true);
            PrepareAclModel(model, null, true);
            PrepareStoresMappingModel(model, null, true);
            return View(model);
        }

        //edit product
        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var product = _productService.GetProductById(id);
            if (product == null || product.Deleted)
                //No product found with the specified id
                return RedirectToAction("List");

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
                return RedirectToAction("List");

            var model = product.ToModel();
            PrepareProductModel(model, product, false, false);
            var customefield = _productservicecustome.GetCustomeFieldByProductId(model.Id);
            if (customefield != null)
            {
                model.IsEligibleforFree = customefield.IsEligibleforFree;
                model.Quantity = customefield.Quantity;
                model.MadeInUsa = customefield.MadeInUsa;
                //model.ShowMaximumRuns = customefield.ShowMaximumRuns;
                model.RunId = customefield.RunId;
            }

            AddLocales(_languageService, model.Locales, (locale, languageId) =>
                {
                    locale.Name = product.GetLocalized(x => x.Name, languageId, false, false);
                    locale.ShortDescription = product.GetLocalized(x => x.ShortDescription, languageId, false, false);
                    locale.FullDescription = product.GetLocalized(x => x.FullDescription, languageId, false, false);
                    locale.MetaKeywords = product.GetLocalized(x => x.MetaKeywords, languageId, false, false);
                    locale.MetaDescription = product.GetLocalized(x => x.MetaDescription, languageId, false, false);
                    locale.MetaTitle = product.GetLocalized(x => x.MetaTitle, languageId, false, false);
                    locale.SeName = product.GetSeName(languageId, false, false);
                });

            PrepareAclModel(model, product, false);
            PrepareStoresMappingModel(model, product, false);
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Edit(ProductModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();


            var product = _productService.GetProductById(model.Id);
            if (product == null || product.Deleted)
                //No product found with the specified id
                return RedirectToAction("List");

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                //a vendor should have access only to his products
                if (_workContext.CurrentVendor != null)
                {
                    model.VendorId = _workContext.CurrentVendor.Id;
                }
                //vendors cannot edit "Show on home page" property
                if (_workContext.CurrentVendor != null && model.ShowOnHomePage != product.ShowOnHomePage)
                {
                    model.ShowOnHomePage = product.ShowOnHomePage;
                }
                //if (_workContext.CurrentVendor != null && model.MadeInUsa != product.MadeInUsa)
                //{
                //    model.MadeInUsa = product.MadeInUsa;
                //}
                var prevStockQuantity = product.StockQuantity;

                //product

                product = model.ToEntity(product);
                product.UpdatedOnUtc = DateTime.UtcNow;
                _productService.UpdateProduct(product);



                //Update produc custome field
                var customefield = _productservicecustome.GetCustomeFieldByProductId(model.Id);

                //customefield.ProductId = model.Id;
                customefield.RunId = model.RunId;

                //Maximum run
                model.AvailableRuns = RunType.PolyDetail.ToSelectList(false).ToList();
                model.AvailableRuns.Insert(0, new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.None"), Value = "0" });


                customefield.MadeInUsa = model.MadeInUsa;
                customefield.IsEligibleforFree = model.IsEligibleforFree;
                customefield.Quantity = model.Quantity;
                _productservicecustome.UpdateProductCustomeField(customefield);




                //search engine name
                model.SeName = product.ValidateSeName(model.SeName, product.Name, true);
                _urlRecordService.SaveSlug(product, model.SeName, 0);
                //locales
                UpdateLocales(product, model);
                //tags
                SaveProductTags(product, ParseProductTags(model.ProductTags));
                //ACL (customer roles)
                SaveProductAcl(product, model);
                //Stores
                SaveStoreMappings(product, model);
                //picture seo names
                UpdatePictureSeoNames(product);
                //discounts
                var allDiscounts = _discountService.GetAllDiscounts(DiscountType.AssignedToSkus, null, true);
                foreach (var discount in allDiscounts)
                {
                    if (model.SelectedDiscountIds != null && model.SelectedDiscountIds.Contains(discount.Id))
                    {
                        //new discount
                        if (product.AppliedDiscounts.Count(d => d.Id == discount.Id) == 0)
                            product.AppliedDiscounts.Add(discount);
                    }
                    else
                    {
                        //remove discount
                        if (product.AppliedDiscounts.Count(d => d.Id == discount.Id) > 0)
                            product.AppliedDiscounts.Remove(discount);
                    }
                }
                _productService.UpdateProduct(product);
                _productService.UpdateHasDiscountsApplied(product);
                //back in stock notifications
                if (product.ManageInventoryMethod == ManageInventoryMethod.ManageStock &&
                    product.BackorderMode == BackorderMode.NoBackorders &&
                    product.AllowBackInStockSubscriptions &&
                    product.StockQuantity > 0 &&
                    prevStockQuantity <= 0 &&
                    product.Published &&
                    !product.Deleted)
                {
                    _backInStockSubscriptionService.SendNotificationsToSubscribers(product);
                }

                //activity log
                _customerActivityService.InsertActivity("EditProduct", _localizationService.GetResource("ActivityLog.EditProduct"), product.Name);

                SuccessNotification(_localizationService.GetResource("Admin.Catalog.Products.Updated"));

                if (continueEditing)
                {
                    //selected tab
                    SaveSelectedTabIndex();

                    return RedirectToAction("Edit", new { id = product.Id });
                }
                else
                {
                    return RedirectToAction("List");
                }
            }

            //If we got this far, something failed, redisplay form
            PrepareProductModel(model, product, false, true);
            PrepareAclModel(model, product, true);
            PrepareStoresMappingModel(model, product, true);
            return View(model);
        }

        //delete product
        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var product = _productService.GetProductById(id);
            if (product == null)
                //No product found with the specified id
                return RedirectToAction("List");

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
                return RedirectToAction("List");

            _productService.DeleteProduct(product);

            //activity log
            _customerActivityService.InsertActivity("DeleteProduct", _localizationService.GetResource("ActivityLog.DeleteProduct"), product.Name);

            SuccessNotification(_localizationService.GetResource("Admin.Catalog.Products.Deleted"));
            return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult DeleteSelected(ICollection<int> selectedIds)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var products = new List<Product>();
            if (selectedIds != null)
            {
                products.AddRange(_productService.GetProductsByIds(selectedIds.ToArray()));

                for (int i = 0; i < products.Count; i++)
                {
                    var product = products[i];

                    //a vendor should have access only to his products
                    if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
                        continue;

                    _productService.DeleteProduct(product);
                }
            }

            return Json(new { Result = true });
        }

        [HttpPost]
        public ActionResult CopyProduct(ProductModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var copyModel = model.CopyProductModel;
            try
            {
                var originalProduct = _productService.GetProductById(copyModel.Id);

                //a vendor should have access only to his products
                if (_workContext.CurrentVendor != null && originalProduct.VendorId != _workContext.CurrentVendor.Id)
                    return RedirectToAction("List");

                var newProduct = _copyProductService.CopyProduct(originalProduct,
                    copyModel.Name, copyModel.Published, copyModel.CopyImages);

                var customFields = _productservicecustome.GetCustomeFieldByProductId(originalProduct.Id);
                if (customFields != null)
                {
                    var newCustomField = new ProductCustomeField(){
                        IsEligibleforFree = customFields.IsEligibleforFree,
                        MadeInUsa = customFields.MadeInUsa,
                        ProductId = newProduct.Id,
                        Quantity = customFields.Quantity,
                        RunId = customFields.RunId
                    };
                    _productservicecustome.InsertProductCustomeField(newCustomField);
                }

                SuccessNotification("The product has been copied successfully");
                return RedirectToAction("Edit", new { id = newProduct.Id });
            }
            catch (Exception exc)
            {
                ErrorNotification(exc.Message);
                return RedirectToAction("Edit", new { id = copyModel.Id });
            }
        }

        #endregion

        #region Product categories

        [HttpPost]
        public ActionResult ProductCategoryList(DataSourceRequest command, int productId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null)
            {
                var product = _productService.GetProductById(productId);
                if (product != null && product.VendorId != _workContext.CurrentVendor.Id)
                {
                    return Content("This is not your product");
                }
            }

            var productCategories = _categoryService.GetProductCategoriesByProductId(productId, true);
            var productCategoriesModel = productCategories
                .Select(x =>
                {
                    return new ProductModel.ProductCategoryModel()
                    {
                        Id = x.Id,
                        Category = _categoryService.GetCategoryById(x.CategoryId).GetFormattedBreadCrumb(_categoryService),
                        ProductId = x.ProductId,
                        CategoryId = x.CategoryId,
                        IsFeaturedProduct = x.IsFeaturedProduct,
                        DisplayOrder = x.DisplayOrder
                    };
                })
                .ToList();

            var gridModel = new DataSourceResult
            {
                Data = productCategoriesModel,
                Total = productCategoriesModel.Count
            };

            return Json(gridModel);
        }

        [HttpPost]
        public ActionResult ProductCategoryInsert(ProductModel.ProductCategoryModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var productId = model.ProductId;
            var categoryId = model.CategoryId;

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null)
            {
                var product = _productService.GetProductById(productId);
                if (product != null && product.VendorId != _workContext.CurrentVendor.Id)
                {
                    return Content("This is not your product");
                }
            }

            var existingProductCategories = _categoryService.GetProductCategoriesByCategoryId(categoryId, 0, int.MaxValue, true);
            if (existingProductCategories.FindProductCategory(productId, categoryId) == null)
            {
                var productCategory = new ProductCategory()
                {
                    ProductId = productId,
                    CategoryId = categoryId,
                    DisplayOrder = model.DisplayOrder
                };
                //a vendor cannot edit "IsFeaturedProduct" property
                if (_workContext.CurrentVendor == null)
                {
                    productCategory.IsFeaturedProduct = model.IsFeaturedProduct;
                }
                _categoryService.InsertProductCategory(productCategory);
            }

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult ProductCategoryUpdate(ProductModel.ProductCategoryModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var productCategory = _categoryService.GetProductCategoryById(model.Id);
            if (productCategory == null)
                throw new ArgumentException("No product category mapping found with the specified id");

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null)
            {
                var product = _productService.GetProductById(productCategory.ProductId);
                if (product != null && product.VendorId != _workContext.CurrentVendor.Id)
                {
                    return Content("This is not your product");
                }
            }

            productCategory.CategoryId = model.CategoryId;
            productCategory.DisplayOrder = model.DisplayOrder;
            //a vendor cannot edit "IsFeaturedProduct" property
            if (_workContext.CurrentVendor == null)
            {
                productCategory.IsFeaturedProduct = model.IsFeaturedProduct;
            }
            _categoryService.UpdateProductCategory(productCategory);

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult ProductCategoryDelete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var productCategory = _categoryService.GetProductCategoryById(id);
            if (productCategory == null)
                throw new ArgumentException("No product category mapping found with the specified id");

            var productId = productCategory.ProductId;

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null)
            {
                var product = _productService.GetProductById(productId);
                if (product != null && product.VendorId != _workContext.CurrentVendor.Id)
                {
                    return Content("This is not your product");
                }
            }

            _categoryService.DeleteProductCategory(productCategory);

            return new NullJsonResult();
        }

        #endregion

        #region Product manufacturers

        [HttpPost]
        public ActionResult ProductManufacturerList(DataSourceRequest command, int productId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null)
            {
                var product = _productService.GetProductById(productId);
                if (product != null && product.VendorId != _workContext.CurrentVendor.Id)
                {
                    return Content("This is not your product");
                }
            }

            var productManufacturers = _manufacturerService.GetProductManufacturersByProductId(productId, true);
            var productManufacturersModel = productManufacturers
                .Select(x =>
                {
                    return new ProductModel.ProductManufacturerModel()
                    {
                        Id = x.Id,
                        Manufacturer = _manufacturerService.GetManufacturerById(x.ManufacturerId).Name,
                        ProductId = x.ProductId,
                        ManufacturerId = x.ManufacturerId,
                        IsFeaturedProduct = x.IsFeaturedProduct,
                        DisplayOrder = x.DisplayOrder
                    };
                })
                .ToList();

            var gridModel = new DataSourceResult
            {
                Data = productManufacturersModel,
                Total = productManufacturersModel.Count
            };

            return Json(gridModel);
        }

        [HttpPost]
        public ActionResult ProductManufacturerInsert(ProductModel.ProductManufacturerModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var productId = model.ProductId;
            var manufacturerId = model.ManufacturerId;

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null)
            {
                var product = _productService.GetProductById(productId);
                if (product != null && product.VendorId != _workContext.CurrentVendor.Id)
                {
                    return Content("This is not your product");
                }
            }

            var existingProductmanufacturers = _manufacturerService.GetProductManufacturersByManufacturerId(manufacturerId, 0, int.MaxValue, true);
            if (existingProductmanufacturers.FindProductManufacturer(productId, manufacturerId) == null)
            {
                var productManufacturer = new ProductManufacturer()
                {
                    ProductId = productId,
                    ManufacturerId = manufacturerId,
                    DisplayOrder = model.DisplayOrder
                };
                //a vendor cannot edit "IsFeaturedProduct" property
                if (_workContext.CurrentVendor == null)
                {
                    productManufacturer.IsFeaturedProduct = model.IsFeaturedProduct;
                }
                _manufacturerService.InsertProductManufacturer(productManufacturer);
            }

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult ProductManufacturerUpdate(ProductModel.ProductManufacturerModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var productManufacturer = _manufacturerService.GetProductManufacturerById(model.Id);
            if (productManufacturer == null)
                throw new ArgumentException("No product manufacturer mapping found with the specified id");

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null)
            {
                var product = _productService.GetProductById(productManufacturer.ProductId);
                if (product != null && product.VendorId != _workContext.CurrentVendor.Id)
                {
                    return Content("This is not your product");
                }
            }

            productManufacturer.ManufacturerId = model.ManufacturerId;
            productManufacturer.DisplayOrder = model.DisplayOrder;
            //a vendor cannot edit "IsFeaturedProduct" property
            if (_workContext.CurrentVendor == null)
            {
                productManufacturer.IsFeaturedProduct = model.IsFeaturedProduct;
            }
            _manufacturerService.UpdateProductManufacturer(productManufacturer);

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult ProductManufacturerDelete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var productManufacturer = _manufacturerService.GetProductManufacturerById(id);
            if (productManufacturer == null)
                throw new ArgumentException("No product manufacturer mapping found with the specified id");

            var productId = productManufacturer.ProductId;

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null)
            {
                var product = _productService.GetProductById(productId);
                if (product != null && product.VendorId != _workContext.CurrentVendor.Id)
                {
                    return Content("This is not your product");
                }
            }

            _manufacturerService.DeleteProductManufacturer(productManufacturer);

            return new NullJsonResult();
        }

        #endregion

        #region Related products

        [HttpPost]
        public ActionResult RelatedProductList(DataSourceRequest command, int productId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null)
            {
                var product = _productService.GetProductById(productId);
                if (product != null && product.VendorId != _workContext.CurrentVendor.Id)
                {
                    return Content("This is not your product");
                }
            }

            var relatedProducts = _productService.GetRelatedProductsByProductId1(productId, true);
            var relatedProductsModel = relatedProducts
                .Select(x =>
                {
                    return new ProductModel.RelatedProductModel()
                    {
                        Id = x.Id,
                        //ProductId1 = x.ProductId1,
                        ProductId2 = x.ProductId2,
                        Product2Name = _productService.GetProductById(x.ProductId2).Name,
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
        public ActionResult RelatedProductUpdate(ProductModel.RelatedProductModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var relatedProduct = _productService.GetRelatedProductById(model.Id);
            if (relatedProduct == null)
                throw new ArgumentException("No related product found with the specified id");

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null)
            {
                var product = _productService.GetProductById(relatedProduct.ProductId1);
                if (product != null && product.VendorId != _workContext.CurrentVendor.Id)
                {
                    return Content("This is not your product");
                }
            }

            relatedProduct.DisplayOrder = model.DisplayOrder;
            _productService.UpdateRelatedProduct(relatedProduct);

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult RelatedProductDelete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var relatedProduct = _productService.GetRelatedProductById(id);
            if (relatedProduct == null)
                throw new ArgumentException("No related product found with the specified id");

            var productId = relatedProduct.ProductId1;

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null)
            {
                var product = _productService.GetProductById(productId);
                if (product != null && product.VendorId != _workContext.CurrentVendor.Id)
                {
                    return Content("This is not your product");
                }
            }

            _productService.DeleteRelatedProduct(relatedProduct);

            return new NullJsonResult();
        }

        public ActionResult RelatedProductAddPopup(int productId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var model = new ProductModel.AddRelatedProductModel();
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

            return View(model);
        }

        [HttpPost]
        public ActionResult RelatedProductAddPopupList(DataSourceRequest command, ProductModel.AddRelatedProductModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null)
            {
                model.SearchVendorId = _workContext.CurrentVendor.Id;
            }

            var products = _productService.SearchProducts(
                categoryIds: new List<int>() { model.SearchCategoryId },
                manufacturerId: model.SearchManufacturerId,
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
        public ActionResult RelatedProductAddPopup(string btnId, string formId, ProductModel.AddRelatedProductModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

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

                        var existingRelatedProducts = _productService.GetRelatedProductsByProductId1(model.ProductId);
                        if (existingRelatedProducts.FindRelatedProduct(model.ProductId, id) == null)
                        {
                            _productService.InsertRelatedProduct(
                                new RelatedProduct()
                                {
                                    ProductId1 = model.ProductId,
                                    ProductId2 = id,
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
            return View(model);
        }

        #endregion

        #region Cross-sell products

        [HttpPost]
        public ActionResult CrossSellProductList(DataSourceRequest command, int productId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null)
            {
                var product = _productService.GetProductById(productId);
                if (product != null && product.VendorId != _workContext.CurrentVendor.Id)
                {
                    return Content("This is not your product");
                }
            }

            var crossSellProducts = _productService.GetCrossSellProductsByProductId1(productId, true);
            var crossSellProductsModel = crossSellProducts
                .Select(x =>
                {
                    return new ProductModel.CrossSellProductModel()
                    {
                        Id = x.Id,
                        //ProductId1 = x.ProductId1,
                        ProductId2 = x.ProductId2,
                        Product2Name = _productService.GetProductById(x.ProductId2).Name,
                    };
                })
                .ToList();

            var gridModel = new DataSourceResult()
            {
                Data = crossSellProductsModel,
                Total = crossSellProductsModel.Count
            };

            return Json(gridModel);
        }

        [HttpPost]
        public ActionResult CrossSellProductDelete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var crossSellProduct = _productService.GetCrossSellProductById(id);
            if (crossSellProduct == null)
                throw new ArgumentException("No cross-sell product found with the specified id");

            var productId = crossSellProduct.ProductId1;

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null)
            {
                var product = _productService.GetProductById(productId);
                if (product != null && product.VendorId != _workContext.CurrentVendor.Id)
                {
                    return Content("This is not your product");
                }
            }

            _productService.DeleteCrossSellProduct(crossSellProduct);

            return new NullJsonResult();
        }

        public ActionResult CrossSellProductAddPopup(int productId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var model = new ProductModel.AddCrossSellProductModel();
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

            return View(model);
        }

        [HttpPost]
        public ActionResult CrossSellProductAddPopupList(DataSourceRequest command, ProductModel.AddCrossSellProductModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null)
            {
                model.SearchVendorId = _workContext.CurrentVendor.Id;
            }

            var products = _productService.SearchProducts(
                categoryIds: new List<int>() { model.SearchCategoryId },
                manufacturerId: model.SearchManufacturerId,
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
        public ActionResult CrossSellProductAddPopup(string btnId, string formId, ProductModel.AddCrossSellProductModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

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

                        var existingCrossSellProducts = _productService.GetCrossSellProductsByProductId1(model.ProductId);
                        if (existingCrossSellProducts.FindCrossSellProduct(model.ProductId, id) == null)
                        {
                            _productService.InsertCrossSellProduct(
                                new CrossSellProduct()
                                {
                                    ProductId1 = model.ProductId,
                                    ProductId2 = id,
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
            return View(model);
        }

        #endregion

        #region Associated products

        [HttpPost]
        public ActionResult AssociatedProductList(DataSourceRequest command, int productId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null)
            {
                var product = _productService.GetProductById(productId);
                if (product != null && product.VendorId != _workContext.CurrentVendor.Id)
                {
                    return Content("This is not your product");
                }
            }

            //a vendor should have access only to his products
            var vendorId = 0;
            if (_workContext.CurrentVendor != null)
            {
                vendorId = _workContext.CurrentVendor.Id;
            }

            var associatedProducts = _productService.SearchProducts(parentGroupedProductId: productId,
                vendorId: vendorId,
                showHidden: true);
            var associatedProductsModel = associatedProducts
                .Select(x =>
                {
                    return new ProductModel.AssociatedProductModel()
                    {
                        Id = x.Id,
                        ProductName = x.Name,
                        DisplayOrder = x.DisplayOrder
                    };
                })
                .ToList();

            var gridModel = new DataSourceResult
            {
                Data = associatedProductsModel,
                Total = associatedProductsModel.Count
            };

            return Json(gridModel);
        }

        [HttpPost]
        public ActionResult AssociatedProductUpdate(ProductModel.AssociatedProductModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var associatedProduct = _productService.GetProductById(model.Id);
            if (associatedProduct == null)
                throw new ArgumentException("No associated product found with the specified id");

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null && associatedProduct.VendorId != _workContext.CurrentVendor.Id)
            {
                return Content("This is not your product");
            }

            associatedProduct.DisplayOrder = model.DisplayOrder;
            _productService.UpdateProduct(associatedProduct);

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult AssociatedProductDelete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var product = _productService.GetProductById(id);
            if (product == null)
                throw new ArgumentException("No associated product found with the specified id");

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
                return Content("This is not your product");

            var originalParentGroupedProductId = product.ParentGroupedProductId;

            product.ParentGroupedProductId = 0;
            _productService.UpdateProduct(product);

            return new NullJsonResult();
        }

        public ActionResult AssociatedProductAddPopup(int productId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var model = new ProductModel.AddAssociatedProductModel();
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

            return View(model);
        }

        [HttpPost]
        public ActionResult AssociatedProductAddPopupList(DataSourceRequest command, ProductModel.AddAssociatedProductModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null)
            {
                model.SearchVendorId = _workContext.CurrentVendor.Id;
            }

            var products = _productService.SearchProducts(
                categoryIds: new List<int>() { model.SearchCategoryId },
                manufacturerId: model.SearchManufacturerId,
                storeId: model.SearchStoreId,
                vendorId: model.SearchVendorId,
                productType: model.SearchProductTypeId > 0 ? (ProductType?)model.SearchProductTypeId : null,
                keywords: model.SearchProductName,
                pageIndex: command.Page - 1,
                pageSize: command.PageSize,
                showHidden: true
                );
            var gridModel = new DataSourceResult();
            gridModel.Data = products.Select(x =>
            {
                var productModel = x.ToModel();
                //display already associated products
                var parentGroupedProduct = _productService.GetProductById(x.ParentGroupedProductId);
                if (parentGroupedProduct != null)
                {
                    productModel.AssociatedToProductId = x.ParentGroupedProductId;
                    productModel.AssociatedToProductName = parentGroupedProduct.Name;
                }
                return productModel;
            });
            gridModel.Total = products.TotalCount;

            return Json(gridModel);
        }

        [HttpPost]
        [FormValueRequired("save")]
        public ActionResult AssociatedProductAddPopup(string btnId, string formId, ProductModel.AddAssociatedProductModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

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

                        product.ParentGroupedProductId = model.ProductId;
                        _productService.UpdateProduct(product);
                    }
                }
            }

            //a vendor should have access only to his products
            model.IsLoggedInAsVendor = _workContext.CurrentVendor != null;
            ViewBag.RefreshPage = true;
            ViewBag.btnId = btnId;
            ViewBag.formId = formId;
            return View(model);
        }

        #endregion

        #region Product In Box

        //public ActionResult OtherProductInBoxAdd(int parentid, string title, int quantity)
        //{
        //    if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
        //        return AccessDeniedView();

        //    if (title == null)
        //        throw new ArgumentException();

        //    if (parentid == 0)
        //        throw new ArgumentException();

        //    var product = _productService.GetProductById(parentid);
        //    if (product == null)
        //        throw new ArgumentException("No product found with the specified id");

        //    //a vendor should have access only to his products
        //    if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
        //        return RedirectToAction("List");
        //    ProductInBox model = new ProductInBox();
        //    model.Title = title;
        //    model.ParentProductId = parentid;
        //    model.InboxProductId = 0;
        //    model.Quantity = quantity;

        //    _productService.InsertProductInBox(model);

        //    return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
        //}


        [HttpPost]
        public ActionResult ProsuctInBoxList(DataSourceRequest command, int productId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null)
            {
                var product = _productService.GetProductById(productId);
                if (product != null && product.VendorId != _workContext.CurrentVendor.Id)
                {
                    return Content("This is not your product");
                }
            }

            //a vendor should have access only to his products
            var vendorId = 0;
            if (_workContext.CurrentVendor != null)
            {
                vendorId = _workContext.CurrentVendor.Id;
            }

            var inboxproductlist = _productservicecustome.GetProductFromBoxByPerantProductId(productId);
            var inboxproductlistModel = inboxproductlist
                .Select(x =>
                {
                    return new ProductModel.ProductinboxModel()
                    {
                        Id = x.Id,
                        ProductName = x.Title,
                        Quantity = x.Quantity,
                        DisplayOrder = x.Displayorder,
                        InBoxProductId = x.InboxProductId
                    };
                })
                .ToList();

            var gridModel = new DataSourceResult
            {
                Data = inboxproductlistModel,
                Total = inboxproductlistModel.Count
            };

            return Json(gridModel);
        }

        [HttpPost]
        public ActionResult ProsuctInBoxUpdate(ProductModel.ProductinboxModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var productbox = _productservicecustome.GetProductFromBoxById(model.Id);
            if (productbox == null)
                throw new ArgumentException("No associated product found with the specified id");

            productbox.Quantity = model.Quantity;
            productbox.Displayorder = model.DisplayOrder;
            _productservicecustome.UpdateProductInBox(productbox);

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult ProsuctInBoxDelete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var product = _productservicecustome.GetProductFromBoxById(id);
            if (product == null)
                throw new ArgumentException("No associated product found with the specified id");



            _productservicecustome.DeleteProductFromBox(product);

            return new NullJsonResult();
        }

        public ActionResult ProductInBoxSimpleAddPopup(int productId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var model = new AddProductInBoxSimpleModel();
            model.Parentid = productId;

            return View(model);
        }

        [HttpPost]
        [FormValueRequired("save")]
        public ActionResult ProductInBoxSimpleAddPopup(string btnId, string formId, AddProductInBoxSimpleModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            if (model.Title == null)
            {
                model.titleerro = true;
            }
            else
            {
                model.titleerro = false;
            }
            if (model.Quantity == 0)
            {
                model.quantityerro = true;
            }
            else
            {
                model.quantityerro = false;
            }

            if (!model.quantityerro && !model.titleerro)
            {
                var product = _productService.GetProductById(model.Parentid);
                if (product == null)
                    throw new ArgumentException("No product found with the specified id");

                //a vendor should have access only to his products
                if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
                    return RedirectToAction("List");

                ProductInBox boxmodel = new ProductInBox();
                boxmodel.Title = model.Title;
                boxmodel.ParentProductId = model.Parentid;
                boxmodel.InboxProductId = 0;
                boxmodel.Quantity = model.Quantity;
                boxmodel.Displayorder = model.DisplayOrder;

                _productservicecustome.InsertProductInBox(boxmodel);
                ViewBag.RefreshPage = true;
                ViewBag.btnId = btnId;
                ViewBag.formId = formId;

            }

            return View(model);
        }

        public ActionResult ProductInBoxAddPopup(int productId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var model = new ProductModel.AddProductInBoxModel();
            model.ProductId = productId;
            //a vendor should have access only to his products
            model.IsLoggedInAsVendor = _workContext.CurrentVendor != null;
            model.ProductId = productId;
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

            return View(model);
        }



        [HttpPost]
        public ActionResult ProsuctInBoxAddPopupList(DataSourceRequest command, ProductModel.AddProductInBoxModel model, int pproduct)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null)
            {
                model.SearchVendorId = _workContext.CurrentVendor.Id;
            }
            var availableid = _productservicecustome.GetProductFromBoxByPerantProductId(pproduct);

            int[] idlist = new int[availableid.Count];
            int k = 0;
            foreach (var item in availableid)
            {
                if (item.InboxProductId != 0)
                {
                    idlist[k] = item.InboxProductId;
                    k++;
                }
            }
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
            gridModel.Data = products.Select(x =>
            {
                var productModel = x.ToModel();
                //display already associated products
                var parentGroupedProduct = _productService.GetProductById(x.ParentGroupedProductId);
                if (parentGroupedProduct != null)
                {
                    productModel.AssociatedToProductId = x.ParentGroupedProductId;
                    productModel.AssociatedToProductName = parentGroupedProduct.Name;
                }
                return productModel;
            }).Where(x => !idlist.Contains(x.Id));
            gridModel.Total = products.TotalCount;

            return Json(gridModel);
        }

        [HttpPost]
        [FormValueRequired("save")]
        public ActionResult ProductInBoxAddPopup(string btnId, string formId, ProductModel.AddProductInBoxModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

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
                        ProductInBox boxmodel = new ProductInBox();
                        boxmodel.Title = product.Name;
                        boxmodel.ParentProductId = model.ProductId;
                        boxmodel.InboxProductId = product.Id;
                        boxmodel.Quantity = 1;
                        boxmodel.Displayorder = 0;
                        _productservicecustome.InsertProductInBox(boxmodel);
                    }
                }
            }

            //a vendor should have access only to his products
            model.IsLoggedInAsVendor = _workContext.CurrentVendor != null;
            ViewBag.RefreshPage = true;
            ViewBag.btnId = btnId;
            ViewBag.formId = formId;
            return View(model);
        }



        public ActionResult ProductInBoxAjaxButton(int[] SelectedProductIds, int productid)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            if (SelectedProductIds.Count() > 0)
            {
                foreach (int id in SelectedProductIds)
                {
                    var product = _productService.GetProductById(id);
                    if (product != null)
                    {
                        //a vendor should have access only to his products
                        if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
                            continue;
                        ProductInBox boxmodel = new ProductInBox();
                        boxmodel.Title = product.Name;
                        boxmodel.ParentProductId = productid;
                        boxmodel.InboxProductId = product.Id;
                        boxmodel.Quantity = 1;
                        boxmodel.Displayorder = 0;
                        _productservicecustome.InsertProductInBox(boxmodel);
                    }
                }
            }

            //a vendor should have access only to his products
            // model.IsLoggedInAsVendor = _workContext.CurrentVendor != null;
            // ViewBag.RefreshPage = true;
            //ViewBag.btnId = btnId;
            //ViewBag.formId = formId;
            return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Product pictures

        public ActionResult ProductPictureAdd(int pictureId, int thumbId, int displayOrder, int productId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            if (pictureId == 0)
                throw new ArgumentException();

            if (thumbId == 0)
                throw new ArgumentException();

            var product = _productService.GetProductById(productId);
            if (product == null)
                throw new ArgumentException("No product found with the specified id");

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
                return RedirectToAction("List");

            _productService.InsertProductPicture(new ProductPicture()
            {
                PictureId = pictureId,
                ProductId = productId,
                DisplayOrder = displayOrder,
            });
            _productthumbService.InsertProductThumbPicture(new ProductThumb()
            {
                PictureId = pictureId,
                ThumbId = thumbId,
                ProductId = productId

            });

            _pictureService.SetSeoFilename(pictureId, _pictureService.GetPictureSeName(product.Name));

            return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ProductPictureList(DataSourceRequest command, int productId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null)
            {
                var product = _productService.GetProductById(productId);
                if (product != null && product.VendorId != _workContext.CurrentVendor.Id)
                {
                    return Content("This is not your product");
                }
            }

            var productPictures = _productService.GetProductPicturesByProductId(productId);
            var productPicturesModel = productPictures
                .Select(x =>
                {
                    return new ProductModel.ProductPictureModel()
                    {
                        Id = x.Id,
                        ProductId = x.ProductId,
                        PictureId = x.PictureId,
                        PictureUrl = _pictureService.GetPictureUrl(x.PictureId),
                        DisplayOrder = x.DisplayOrder,
                        ThumbPictureUrl = (_productthumbService.Getthumbbypictureid(x.PictureId) != null) ? (_productthumbService.Getthumbbypictureid(x.PictureId).ThumbId > 0 ? _pictureService.GetPictureUrl(_productthumbService.Getthumbbypictureid(x.PictureId).ThumbId) : "") : "",
                    };
                })
                .ToList();

            var gridModel = new DataSourceResult
            {
                Data = productPicturesModel,
                Total = productPicturesModel.Count
            };

            return Json(gridModel);
        }

        public ActionResult ProductPictureUpdate(int id)
        {
            var pthumb = _productthumbService.Getthumbbypictureid(id);
            var picturedata = _productservicecustome.GetProductPictureByPictureId(id);
            var picture = _pictureService.GetPictureUrl(id);
            var picturethumburl = _pictureService.GetPictureUrl(pthumb.ThumbId);
            var model = new ProductPictureCombination();
            model.Pictureurl = picture;
            model.ThumbUrl = picturethumburl;
            model.DisplayOrder = picturedata.DisplayOrder;
            model.PictureId = id;
            model.ThumbId = pthumb.ThumbId;
            model.ClonePictureid = id;
            model.CloneThumbid = pthumb.ThumbId;
            return View(model);
        }

        [HttpPost]
        public ActionResult ProductPictureUpdate(string btnId, string formId, ProductPictureCombination model)
        {

            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var productPicture = _productservicecustome.GetProductPictureByPictureId(model.ClonePictureid);
            var pthumb = _productthumbService.Getthumbbypictureid(model.CloneThumbid);
            var picture = _pictureService.GetPictureById(model.PictureId);
            var thumbPicture = _pictureService.GetPictureById(model.ThumbId);
            var thumbDeleted = false;


            if (productPicture != null && picture != null)
            {
                productPicture.DisplayOrder = model.DisplayOrder;
                productPicture.PictureId = model.PictureId;
            }
            else if (productPicture != null && model.PictureId == 0)
            {
                _productService.DeleteProductPicture(productPicture);
                if (pthumb != null)
                {
                    _productthumbService.DeleteProductThumbPicture(pthumb);
                    thumbDeleted = true;
                }
            }


            if (pthumb != null && thumbPicture != null)
            {
                pthumb.PictureId = model.PictureId;
                pthumb.ThumbId = model.ThumbId;
            }
            else if (!thumbDeleted && pthumb != null && model.ThumbId == 0)
            {
                _productthumbService.DeleteProductThumbPicture(pthumb);
            }


            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null)
            {
                var product = _productService.GetProductById(productPicture.ProductId);
                if (product != null && product.VendorId != _workContext.CurrentVendor.Id)
                {
                    return Content("This is not your product");
                }
            }
            ViewBag.RefreshPage = true;
            ViewBag.btnId = btnId;
            ViewBag.formId = formId;
            if (productPicture != null && picture != null)
                _productService.UpdateProductPicture(productPicture);


            if (pthumb != null && thumbPicture != null)
                _productthumbService.UpdateProductThumbPicture(pthumb);
            else if (pthumb == null && thumbPicture != null && picture != null && productPicture != null)
            { 
                pthumb = new ProductThumb()
                {
                    PictureId = picture.Id,
                    ProductId = productPicture.ProductId,
                    ThumbId = thumbPicture.Id
                };
                _productthumbService.InsertProductThumbPicture(pthumb);
            }

            return View(model);
        }


        [HttpPost]
        public ActionResult ProductPictureDelete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();


            var productPicture = _productService.GetProductPictureById(id);
            var thumbdetail = _productthumbService.Getthumbbypictureid(productPicture.PictureId);
            //var productThumb = _productthumbService.GetProductThumbPictureById(thumbdetail.Id);
            if (productPicture == null)
                throw new ArgumentException("No product picture found with the specified id");

            if (thumbdetail == null)
                throw new ArgumentException("No product picture found with the specified id");

            var productId = productPicture.ProductId;

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null)
            {
                var product = _productService.GetProductById(productId);
                if (product != null && product.VendorId != _workContext.CurrentVendor.Id)
                {
                    return Content("This is not your product");
                }
            }
            var pictureId = productPicture.PictureId;
            _productService.DeleteProductPicture(productPicture);
            var picture = _pictureService.GetPictureById(pictureId);
            _pictureService.DeletePicture(picture);
            var thumb = _pictureService.GetPictureById(thumbdetail.ThumbId);
            _pictureService.DeletePicture(thumb);
            _productthumbService.DeleteProductThumbPicture(thumbdetail);




            return new NullJsonResult();
        }

        #endregion

        #region Product Video

        public ActionResult ProductVideoAdd(int pictureId, string videotitle, string videodescription, string videourl, int displayOrder, int productId, string tags)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            if (videourl == null)
                throw new ArgumentException();

            if (pictureId == 0)
                throw new ArgumentException();
            var pluginFinder = Nop.Core.Infrastructure.EngineContext.Current.Resolve<IPluginFinder>();

            // check plugin is installed
            var pluginDescriptor = pluginFinder.GetPluginDescriptorBySystemName("TorchDesign_Support");


            if (pluginDescriptor != null)
            {
                var product = _productService.GetProductById(productId);
                if (product == null)
                    throw new ArgumentException("No product found with the specified id");

                //a vendor should have access only to his products
                if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
                    return RedirectToAction("List");

                SupportVideo supportVideo = new SupportVideo();
                SupportVideoRelatedProduct supportVideorelatedProduct = new SupportVideoRelatedProduct();
                supportVideorelatedProduct.ProductId = productId;
                supportVideo.SupportVideosRelatedProducts.Add(supportVideorelatedProduct);

                supportVideo.PictureId = pictureId;
                supportVideo.Title = videotitle;
                supportVideo.VimeoInformation = videourl;
                supportVideo.Tag = tags;
                supportVideo.Description = videodescription;
                supportVideo.DisplayOrder = displayOrder;

                _supportservice.InsertSupportVideo(supportVideo);

                return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var product = _productService.GetProductById(productId);
                if (product == null)
                    throw new ArgumentException("No product found with the specified id");

                //a vendor should have access only to his products
                if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
                    return RedirectToAction("List");

                _productservicecustome.InsertProductVideo(new ProductVideo()
                {
                    ProductId = productId,
                    PictureId = pictureId,
                    VideoId = videourl,
                    DisplayOrder = displayOrder,
                    VideoDescription = videodescription,
                    VideoTitle = videotitle,
                    TagName = tags
                });



                return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult ProductVideoList(DataSourceRequest command, int productId)
        {


            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null)
            {
                var product = _productService.GetProductById(productId);
                if (product != null && product.VendorId != _workContext.CurrentVendor.Id)
                {
                    return Content("This is not your product");
                }
            }
            var pluginFinder = Nop.Core.Infrastructure.EngineContext.Current.Resolve<IPluginFinder>();

            // check plugin is installed
            var pluginDescriptor = pluginFinder.GetPluginDescriptorBySystemName("TorchDesign_Support");

            //if (pluginDescriptor != null)
            //{
            //    //Your code
            //}
            //var _pluginFinder = EngineContext.Current.Resolve<Nop.Core.Plugins.IPluginFinder>();
            //var pluginDescriptor = _pluginFinder.GetPluginDescriptors(false).FirstOrDefault(x => x.SystemName.Equals("TorchDesign_Support", StringComparison.InvariantCultureIgnoreCase));

            if (pluginDescriptor != null)
            {
                var productVideo = _supportservice.GetSupportVideoByProductId(productId);
                var productVideoModel = productVideo
                    .Select(x =>
                    {
                        return new ProductModel.ProductVidModel()
                        {
                            Id = x.Id,

                            PictureId = x.PictureId,
                            PictureUrl = _pictureService.GetPictureUrl(x.PictureId),
                            VideoDescription = x.Description,
                            VideoTitle = x.Title,
                            VideoId = x.VimeoInformation,
                            Tagname = x.Tag
                        };
                    })
                    .ToList();

                var gridModel = new DataSourceResult
                {
                    Data = productVideoModel,
                    Total = productVideoModel.Count
                };

                return Json(gridModel);
            }
            else
            {
                var productVideo = _productservicecustome.GetProductVideoByProductId(productId);
                var productVideoModel = productVideo
                    .Select(x =>
                    {
                        return new ProductModel.ProductVidModel()
                        {
                            Id = x.Id,
                            ProductId = x.ProductId,
                            PictureId = x.PictureId,
                            PictureUrl = _pictureService.GetPictureUrl(x.PictureId),
                            DisplayOrder = x.DisplayOrder,
                            VideoDescription = x.VideoDescription,
                            VideoTitle = x.VideoTitle,
                            VideoId = x.VideoId,
                            Tagname = x.TagName
                        };
                    })
                    .ToList();

                var gridModel = new DataSourceResult
                {
                    Data = productVideoModel,
                    Total = productVideoModel.Count
                };

                return Json(gridModel);
            }

        }

        public ActionResult ProductVideoUpdate(int id)
        {
            var pluginFinder = Nop.Core.Infrastructure.EngineContext.Current.Resolve<IPluginFinder>();

            // check plugin is installed
            var pluginDescriptor = pluginFinder.GetPluginDescriptorBySystemName("TorchDesign_Support");

            if (pluginDescriptor != null)
            {
                var videodata = _supportservice.GetSupportVideoById(id);

                var picture = _pictureService.GetPictureUrl(id);

                var model = new ProductsVideos();
                model.Id = videodata.Id;
                model.Pictureurl = picture;
                model.DisplayOrder = videodata.DisplayOrder;
                model.PictureId = videodata.PictureId;
                model.VideoUrl = videodata.VimeoInformation;
                model.ClonePictureid = videodata.PictureId;
                model.Tags = videodata.Tag;
                model.VideoTitle = videodata.Title;
                model.VideoDescription = videodata.Description;

                return View(model);
            }
            else
            {
                var videodata = _productservicecustome.GetProductVideoById(id);
                var picture = _pictureService.GetPictureUrl(id);

                var model = new ProductsVideos();
                model.Id = videodata.Id;
                model.Pictureurl = picture;
                model.DisplayOrder = videodata.DisplayOrder;
                model.PictureId = videodata.PictureId;
                model.VideoUrl = videodata.VideoId;
                model.ClonePictureid = videodata.PictureId;
                model.Tags = videodata.TagName;
                model.VideoTitle = videodata.VideoTitle;
                model.VideoDescription = videodata.VideoDescription;

                return View(model);

            }

        }

        [HttpPost]
        public ActionResult ProductVideoUpdate(string btnId, string formId, ProductsVideos model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();


            var pluginFinder = Nop.Core.Infrastructure.EngineContext.Current.Resolve<IPluginFinder>();

            // check plugin is installed
            var pluginDescriptor = pluginFinder.GetPluginDescriptorBySystemName("TorchDesign_Support");

            if (pluginDescriptor != null)
            {
                var productVideo = _supportservice.GetSupportVideoById(model.Id);


                productVideo.DisplayOrder = model.DisplayOrder;
                productVideo.PictureId = model.PictureId;
                productVideo.VimeoInformation = model.VideoUrl;
                productVideo.Tag = model.Tags;
                productVideo.Title = model.VideoTitle;
                productVideo.Description = model.VideoDescription;



                if (productVideo == null)
                    throw new ArgumentException("No product Video thumbnail found with the specified id");


                ViewBag.RefreshPage = true;
                ViewBag.btnId = btnId;
                ViewBag.formId = formId;
                _supportservice.UpdateSupportVideo(productVideo);

                return View(model);

            }
            else
            {
                var productVideo = _productservicecustome.GetProductVideoById(model.Id);


                productVideo.DisplayOrder = model.DisplayOrder;
                productVideo.PictureId = model.PictureId;
                productVideo.VideoId = model.VideoUrl;
                productVideo.TagName = model.Tags;
                productVideo.VideoTitle = model.VideoTitle;
                productVideo.VideoDescription = model.VideoDescription;



                if (productVideo == null)
                    throw new ArgumentException("No product Video thumbnail found with the specified id");

                //a vendor should have access only to his products
                if (_workContext.CurrentVendor != null)
                {
                    var product = _productService.GetProductById(productVideo.ProductId);
                    if (product != null && product.VendorId != _workContext.CurrentVendor.Id)
                    {
                        return Content("This is not your product");
                    }
                }
                ViewBag.RefreshPage = true;
                ViewBag.btnId = btnId;
                ViewBag.formId = formId;
                _productservicecustome.UpdateProductVideo(productVideo);

                return View(model);
            }

        }

        [HttpPost]
        public ActionResult ProductVedioDelete(int id, int productId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var pluginFinder = Nop.Core.Infrastructure.EngineContext.Current.Resolve<IPluginFinder>();

            // check plugin is installed
            var pluginDescriptor = pluginFinder.GetPluginDescriptorBySystemName("TorchDesign_Support");

            if (pluginDescriptor != null)
            {
                //    var productVideo = _supportservice.GetSupportVideoById(id);


                //    if (productVideo == null)
                //        throw new ArgumentException("No product Vedio found with the specified id");

                //    var pictureId = productVideo.PictureId;
                //    _supportservice.DeleteSupportVideo(productVideo);
                //    var picture = _pictureService.GetPictureById(pictureId);
                //    _pictureService.DeletePicture(picture);

                //    return new NullJsonResult();
                var relatedProductID = _supportservice.GetRelatedProductsBySupportVideoId(id).Where(x => x.ProductId == productId).FirstOrDefault();
                if (relatedProductID != null)
                {
                    var relatedProduct = _supportservice.GetSupportVideoRelatedProductById(relatedProductID.Id);
                    if (relatedProduct == null)
                        throw new ArgumentException("No related product found with the specified id");
                    _supportservice.DeleteSupportVideoRelatedProduct(relatedProduct);
                }

                return new NullJsonResult();
            }
            else
            {
                var productVideo = _productservicecustome.GetProductVideoById(id);


                if (productVideo == null)
                    throw new ArgumentException("No product Vedio found with the specified id");



                var NEWproductId = productVideo.ProductId;

                //a vendor should have access only to his products
                if (_workContext.CurrentVendor != null)
                {
                    var product = _productService.GetProductById(NEWproductId);
                    if (product != null && product.VendorId != _workContext.CurrentVendor.Id)
                    {
                        return Content("This is not your product");
                    }
                }
                var pictureId = productVideo.PictureId;
                _productservicecustome.DeleteProductVideo(productVideo);
                var picture = _pictureService.GetPictureById(pictureId);
                _pictureService.DeletePicture(picture);

                return new NullJsonResult();
            }

        }

        #endregion

        #region Product Download

        public ActionResult ProductDownloadAdd(int downloadid, string description, int displayOrder, int productId, string title)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            if (downloadid == 0)
                throw new ArgumentException();

            var pluginFinder = Nop.Core.Infrastructure.EngineContext.Current.Resolve<IPluginFinder>();
            var pluginDescriptor = pluginFinder.GetPluginDescriptorBySystemName("TorchDesign_Support");


            if (pluginDescriptor != null)
            {
                var product = _productService.GetProductById(productId);
                if (product == null)
                    throw new ArgumentException("No product found with the specified id");

                //a vendor should have access only to his products
                if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
                    return RedirectToAction("List");


                SupportDownload supportDownload = new SupportDownload();
                SupportDownloadRelatedProduct supportDownloadrelatedProduct = new SupportDownloadRelatedProduct();
                supportDownloadrelatedProduct.ProductId = productId;
                supportDownload.SupportDownloadRelatedProducts.Add(supportDownloadrelatedProduct);

                supportDownload.Title = title;
                supportDownload.DownloadId = downloadid;
                supportDownload.Description = description;
                supportDownload.DisplayOrder = displayOrder;

                _supportservice.InsertSupportDownload(supportDownload);

                return Json(new { Result = true }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var product = _productService.GetProductById(productId);
                if (product == null)
                    throw new ArgumentException("No product found with the specified id");

                //a vendor should have access only to his products
                if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
                    return RedirectToAction("List");

                _productservicecustome.InsertProductDownload(new ProductDownload()
                {
                    ProductId = productId,
                    DownloadTitle = title,
                    DownloadId = downloadid,
                    DisplayOrder = displayOrder,
                    DownloadDescription = description
                });
                return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult ProductDownloadList(DataSourceRequest command, int productId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null)
            {
                var product = _productService.GetProductById(productId);
                if (product != null && product.VendorId != _workContext.CurrentVendor.Id)
                {
                    return Content("This is not your product");
                }
            }

            var pluginFinder = Nop.Core.Infrastructure.EngineContext.Current.Resolve<IPluginFinder>();

            // check plugin is installed
            var pluginDescriptor = pluginFinder.GetPluginDescriptorBySystemName("TorchDesign_Support");

            if (pluginDescriptor != null)
            {
                var productDownload = _supportservice.GetSupportDownloadByProductId(productId);
                var productDownloadModel = productDownload
                    .Select(x =>
                    {
                        return new ProductModel.ProductDownlModel()
                        {
                            Id = x.Id,
                            DownloadsId = x.DownloadId,
                            FileName = _downloadService.GetDownloadById(x.DownloadId).Filename,
                            DownloadTitle = x.Title,
                            DownloadDescription = x.Description,

                        };
                    })
                    .ToList();

                var gridModel = new DataSourceResult
                {
                    Data = productDownloadModel,
                    Total = productDownloadModel.Count
                };

                return Json(gridModel);
            }
            else
            {
                var productDownload = _productservicecustome.GetProductDownloadByProductId(productId);
                var productDownloadModel = productDownload
                    .Select(x =>
                    {
                        return new ProductModel.ProductDownlModel()
                        {
                            Id = x.Id,
                            //ProductId = x.ProductId,
                            DownloadsId = x.DownloadId,
                            FileName = _downloadService.GetDownloadById(x.DownloadId).Filename,
                            DownloadTitle = x.DownloadTitle,
                            DownloadDescription = x.DownloadDescription,

                        };
                    })
                    .ToList();

                var gridModel = new DataSourceResult
                {
                    Data = productDownloadModel,
                    Total = productDownloadModel.Count
                };

                return Json(gridModel);
            }
        }

        public ActionResult ProductDownloadUpdate(int id)
        {
            var pluginFinder = Nop.Core.Infrastructure.EngineContext.Current.Resolve<IPluginFinder>();

            // check plugin is installed
            var pluginDescriptor = pluginFinder.GetPluginDescriptorBySystemName("TorchDesign_Support");

            if (pluginDescriptor != null)
            {

                var downloaddata = _supportservice.GetSupportDownloadById(id);
                var download = _downloadService.GetDownloadById(id);

                var model = new ProductsDownloadsModel();
                model.Id = downloaddata.Id;
                model.DisplayOrder = downloaddata.DisplayOrder;
                model.DownloadsId = downloaddata.DownloadId;
                model.DownloadTitle = downloaddata.Title;
                model.DownloadDescription = downloaddata.Description;

                return View(model);
            }
            else
            {
                var downloaddata = _productservicecustome.GetProductDownloadById(id);
                var download = _downloadService.GetDownloadById(id);

                var model = new ProductsDownloadsModel();
                model.Id = downloaddata.Id;
                model.DisplayOrder = downloaddata.DisplayOrder;
                model.DownloadsId = downloaddata.DownloadId;
                model.DownloadTitle = downloaddata.DownloadTitle;
                model.DownloadDescription = downloaddata.DownloadDescription;
                //  model.Tags = downloaddata.fi;

                return View(model);
            }
        }

        [HttpPost]
        public ActionResult ProductDownloadUpdate(string btnId, string formId, ProductsDownloadsModel model)
        {

            var pluginFinder = Nop.Core.Infrastructure.EngineContext.Current.Resolve<IPluginFinder>();

            // check plugin is installed
            var pluginDescriptor = pluginFinder.GetPluginDescriptorBySystemName("TorchDesign_Support");

            if (pluginDescriptor != null)
            {
                var productDownload = _supportservice.GetSupportDownloadById(model.Id);

                if (productDownload == null)
                    throw new ArgumentException("No product Download thumbnail found with the specified id");

                productDownload.DisplayOrder = model.DisplayOrder;
                productDownload.DownloadId = model.DownloadsId;
                productDownload.Title = model.DownloadTitle;
                productDownload.Description = model.DownloadDescription;

                ViewBag.RefreshPage = true;
                ViewBag.btnId = btnId;
                ViewBag.formId = formId;
                _supportservice.UpdateSupportDownload(productDownload);

                return View(model);

            }
            else
            {
                var productDownload = _productservicecustome.GetProductDownloadById(model.Id);

                if (productDownload == null)
                    throw new ArgumentException("No product Download thumbnail found with the specified id");

                //a vendor should have access only to his products
                if (_workContext.CurrentVendor != null)
                {
                    var product = _productService.GetProductById(productDownload.ProductId);
                    if (product != null && product.VendorId != _workContext.CurrentVendor.Id)
                    {
                        return Content("This is not your product");
                    }
                }

                productDownload.DisplayOrder = model.DisplayOrder;
                productDownload.DownloadId = model.DownloadsId;
                productDownload.DownloadTitle = model.DownloadTitle;
                productDownload.DownloadDescription = model.DownloadDescription;

                ViewBag.RefreshPage = true;
                ViewBag.btnId = btnId;
                ViewBag.formId = formId;
                _productservicecustome.UpdateProductDownload(productDownload);

                return View(model);
            }
        }

        [HttpPost]
        public ActionResult ProductDownloadDelete(int id, int productId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();
            var pluginFinder = Nop.Core.Infrastructure.EngineContext.Current.Resolve<IPluginFinder>();

            // check plugin is installed
            var pluginDescriptor = pluginFinder.GetPluginDescriptorBySystemName("TorchDesign_Support");

            if (pluginDescriptor != null)
            {
                var relatedProductID = _supportservice.GetRelatedProductsBySupportDownloadId(id).Where(x => x.ProductId == productId).FirstOrDefault();
                if (relatedProductID != null)
                {
                    var relatedProduct = _supportservice.GetSupportDownloadRelatedProductById(relatedProductID.Id);
                    if (relatedProduct == null)
                        throw new ArgumentException("No related product found with the specified id");
                    _supportservice.DeleteSupportDownloadRelatedProduct(relatedProduct);
                }

                return new NullJsonResult();
            }
            else
            {

                var productDownload = _productservicecustome.GetProductDownloadById(id);


                if (productDownload == null)
                    throw new ArgumentException("No product Vedio found with the specified id");

                var newproductId = productDownload.ProductId;

                //a vendor should have access only to his products
                if (_workContext.CurrentVendor != null)
                {
                    var product = _productService.GetProductById(newproductId);
                    if (product != null && product.VendorId != _workContext.CurrentVendor.Id)
                    {
                        return Content("This is not your product");
                    }
                }
                var Downloadid = productDownload.DownloadId;
                _productservicecustome.DeleteProductDownload(productDownload);
                var download = _downloadService.GetDownloadById(Downloadid);
                _downloadService.DeleteDownload(download);

                return new NullJsonResult();
            }
        }

        #endregion

        #region Product specification attributes

        [ValidateInput(false)]
        public ActionResult ProductSpecificationAttributeAdd(int specificationAttributeOptionId, int specificationAttributeGroupId,
            string customValue, bool allowFiltering, bool showOnProductPage,
            int displayOrder, int productId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null)
            {
                var product = _productService.GetProductById(productId);
                if (product != null && product.VendorId != _workContext.CurrentVendor.Id)
                {
                    return RedirectToAction("List");
                }
            }

            var psa = new ProductSpecificationAttribute()
            {
                SpecificationAttributeOptionId = specificationAttributeOptionId,
                ProductId = productId,
                CustomValue = customValue,
                AllowFiltering = allowFiltering,
                ShowOnProductPage = showOnProductPage,
                DisplayOrder = displayOrder,
                SpecificationAttributeGroupId = specificationAttributeGroupId
            };
            _specificationAttributeService.InsertProductSpecificationAttribute(psa);

            return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ProductSpecAttrList(DataSourceRequest command, int productId, int SpecificationAttributeGroupId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null)
            {
                var product = _productService.GetProductById(productId);
                if (product != null && product.VendorId != _workContext.CurrentVendor.Id)
                {
                    return Content("This is not your product");
                }
            }

            var productrSpecs = _specificationAttributeService.GetProductSpecificationAttributesByProductId(productId).Where(x => SpecificationAttributeGroupId == x.SpecificationAttributeGroupId);

            var productrSpecsModel = productrSpecs
                .Select(x =>
                {
                    var psaModel = new ProductSpecificationAttributeModel()
                    {
                        Id = x.Id,
                        SpecificationAttributeName = x.SpecificationAttributeOption.SpecificationAttribute.Name,
                        SpecificationAttributeOptionName = x.SpecificationAttributeOption.Name,
                        CustomValue = x.CustomValue,
                        AllowFiltering = x.AllowFiltering,
                        ShowOnProductPage = x.ShowOnProductPage,
                        DisplayOrder = x.DisplayOrder,
                        SpecificationAttributeGroupId = x.SpecificationAttributeGroupId,
                        SpecificationAttributeGroupName = _specificationgroupservice.GetSpecificationGroupById(x.SpecificationAttributeGroupId).Name
                    };
                    return psaModel;
                })
                .ToList();

            var gridModel = new DataSourceResult
            {
                Data = productrSpecsModel,
                Total = productrSpecsModel.Count
            };

            return Json(gridModel);
        }


        public ActionResult ProductSpecAttrUpdate(int id)
        {
            //ProductModel model=new ProductModel();
            var obj = _specificationAttributeService.GetProductSpecificationAttributeById(id);

            var model = new EditProductSpecificationAttributeModel
            {

                Id = obj.Id,
                //SpecificationAttributeName = obj.SpecificationAttributeOption.SpecificationAttribute.Name,
                //SpecificationAttributeOptionName = obj.SpecificationAttributeOption.Name,
                CustomValue = obj.CustomValue,
                AllowFiltering = obj.AllowFiltering,
                ShowOnProductPage = obj.ShowOnProductPage,
                DisplayOrder = obj.DisplayOrder,
                SpecificationAttributeGroupId = obj.SpecificationAttributeGroupId,
                //SpecificationAttributeGroupName = _specificationgroupservice.GetSpecificationGroupById(obj.SpecificationAttributeGroupId).Name


            };
            //specification attributes Group
            var specificationAttributeGroup = _specificationgroupservice.GetAllSpecificationGroup();
            for (int i = 0; i < specificationAttributeGroup.Count; i++)
            {
                var sa = specificationAttributeGroup[i];
                model.AvailableSpecificationGroup.Add(new SelectListItem() { Text = sa.Name, Value = sa.Id.ToString(), Selected = (sa.Id == _specificationgroupservice.GetSpecificationGroupById(obj.SpecificationAttributeGroupId).Id) ? true : false });

            }

            //specification attributes
            var specificationAttributes = _specificationAttributeService.GetSpecificationAttributes();
            for (int i = 0; i < specificationAttributes.Count; i++)
            {
                var sa = specificationAttributes[i];
                model.AvailableAttribute.Add(new SelectListItem() { Text = sa.Name, Value = sa.Id.ToString(), Selected = (sa.Id == obj.SpecificationAttributeOption.SpecificationAttribute.Id) ? true : false });
                if (i == 0)
                {
                    //attribute options
                    foreach (var sao in _specificationAttributeService.GetSpecificationAttributeOptionsBySpecificationAttribute(sa.Id))
                        model.AvailableAttributeOption.Add(new SelectListItem() { Text = sao.Name, Value = sao.Id.ToString(), Selected = (sao.Id == obj.SpecificationAttributeOption.Id) ? true : false });
                }
            }

            return View(model);
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
            //    return AccessDeniedView();

            //var psa = _specificationAttributeService.GetProductSpecificationAttributeById(model.Id);
            //if (psa == null)
            //    return Content("No product specification attribute found with the specified id");

            //var productId = psa.Product.Id;

            ////a vendor should have access only to his products
            //if (_workContext.CurrentVendor != null)
            //{
            //    var product = _productService.GetProductById(productId);
            //    if (product != null && product.VendorId != _workContext.CurrentVendor.Id)
            //    {
            //        return Content("This is not your product");
            //    }
            //}

            //psa.CustomValue = model.CustomValue;
            //psa.AllowFiltering = model.AllowFiltering;
            //psa.ShowOnProductPage = model.ShowOnProductPage;
            //psa.DisplayOrder = model.DisplayOrder;
            //_specificationAttributeService.UpdateProductSpecificationAttribute(psa);

            //return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult ProductSpecAttrUpdate(string btnId, string formId, EditProductSpecificationAttributeModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var psa = _specificationAttributeService.GetProductSpecificationAttributeById(model.Id);
            if (psa == null)
                return Content("No product specification attribute found with the specified id");

            var productId = psa.Product.Id;

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null)
            {
                var product = _productService.GetProductById(productId);
                if (product != null && product.VendorId != _workContext.CurrentVendor.Id)
                {
                    return Content("This is not your product");
                }
            }

            psa.CustomValue = model.CustomValue;
            psa.AllowFiltering = model.AllowFiltering;
            psa.ShowOnProductPage = model.ShowOnProductPage;
            psa.DisplayOrder = model.DisplayOrder;
            psa.SpecificationAttributeGroupId = model.SpecificationAttributeGroupId;
            psa.SpecificationAttributeOptionId = model.SpecificationAttributeOptionId;
            //psa.SpecificationAttributeOption.SpecificationAttributeId = model.SpecificationAttributeId;

            _specificationAttributeService.UpdateProductSpecificationAttribute(psa);


            ViewBag.RefreshPage = true;
            ViewBag.btnId = btnId;
            ViewBag.formId = formId;

            return View(model);
        }

        [HttpPost]
        public ActionResult ProductSpecAttrDelete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var psa = _specificationAttributeService.GetProductSpecificationAttributeById(id);
            if (psa == null)
                throw new ArgumentException("No specification attribute found with the specified id");

            var productId = psa.ProductId;

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null)
            {
                var product = _productService.GetProductById(productId);
                if (product != null && product.VendorId != _workContext.CurrentVendor.Id)
                {
                    return Content("This is not your product");
                }
            }

            _specificationAttributeService.DeleteProductSpecificationAttribute(psa);

            return new NullJsonResult();
        }

        #endregion

        #region Product tags

        public ActionResult ProductTags()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProductTags))
                return AccessDeniedView();

            return View();
        }

        [HttpPost]
        public ActionResult ProductTags(DataSourceRequest command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProductTags))
                return AccessDeniedView();

            var tags = _productTagService.GetAllProductTags()
                //order by product count
                .OrderByDescending(x => _productTagService.GetProductCount(x.Id, 0))
                .Select(x =>
                {
                    return new ProductTagModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ProductCount = _productTagService.GetProductCount(x.Id, 0)
                    };
                })
                .ToList();

            var gridModel = new DataSourceResult
            {
                Data = tags.PagedForCommand(command),
                Total = tags.Count
            };

            return Json(gridModel);
        }

        [HttpPost]
        public ActionResult ProductTagDelete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProductTags))
                return AccessDeniedView();

            var tag = _productTagService.GetProductTagById(id);
            if (tag == null)
                throw new ArgumentException("No product tag found with the specified id");
            _productTagService.DeleteProductTag(tag);

            return new NullJsonResult();
        }

        //edit
        public ActionResult EditProductTag(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProductTags))
                return AccessDeniedView();

            var productTag = _productTagService.GetProductTagById(id);
            if (productTag == null)
                //No product tag found with the specified id
                return RedirectToAction("List");

            var model = new ProductTagModel()
            {
                Id = productTag.Id,
                Name = productTag.Name,
                ProductCount = _productTagService.GetProductCount(productTag.Id, 0)
            };
            //locales
            AddLocales(_languageService, model.Locales, (locale, languageId) =>
            {
                locale.Name = productTag.GetLocalized(x => x.Name, languageId, false, false);
            });

            return View(model);
        }

        [HttpPost]
        public ActionResult EditProductTag(string btnId, string formId, ProductTagModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProductTags))
                return AccessDeniedView();

            var productTag = _productTagService.GetProductTagById(model.Id);
            if (productTag == null)
                //No product tag found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                productTag.Name = model.Name;
                _productTagService.UpdateProductTag(productTag);
                //locales
                UpdateLocales(productTag, model);

                ViewBag.RefreshPage = true;
                ViewBag.btnId = btnId;
                ViewBag.formId = formId;
                return View(model);
            }

            //If we got this far, something failed, redisplay form
            return View(model);
        }

        #endregion

        #region Purchased with order

        [HttpPost]
        public ActionResult PurchasedWithOrders(DataSourceRequest command, int productId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var product = _productService.GetProductById(productId);
            if (product == null)
                throw new ArgumentException("No product found with the specified id");

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
                return Content("This is not your product");

            var orders = _orderService.SearchOrders(
                productId: productId,
                pageIndex: command.Page - 1,
                pageSize: command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = orders.Select(x =>
                {
                    var store = _storeService.GetStoreById(x.StoreId);
                    return new OrderModel()
                    {
                        Id = x.Id,
                        StoreName = store != null ? store.Name : "Unknown",
                        OrderStatus = x.OrderStatus.GetLocalizedEnum(_localizationService, _workContext),
                        PaymentStatus = x.PaymentStatus.GetLocalizedEnum(_localizationService, _workContext),
                        ShippingStatus = x.ShippingStatus.GetLocalizedEnum(_localizationService, _workContext),
                        CustomerEmail = x.BillingAddress.Email,
                        CreatedOn = _dateTimeHelper.ConvertToUserTime(x.CreatedOnUtc, DateTimeKind.Utc)
                    };
                }),
                Total = orders.TotalCount
            };

            return Json(gridModel);
        }

        #endregion

        #region Export / Import

        public ActionResult DownloadCatalogAsPdf()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            try
            {
                //a vendor should have access only to his products
                int vendorId = _workContext.CurrentVendor != null ? _workContext.CurrentVendor.Id : 0;

                var products = _productService.SearchProducts(vendorId: vendorId, showHidden: true);

                byte[] bytes = null;
                using (var stream = new MemoryStream())
                {
                    _pdfService.PrintProductsToPdf(stream, products);
                    bytes = stream.ToArray();
                }
                return File(bytes, "application/pdf", "pdfcatalog.pdf");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("List");
            }
        }

        public ActionResult ExportXmlAll()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            try
            {
                //a vendor should have access only to his products
                int vendorId = _workContext.CurrentVendor != null ? _workContext.CurrentVendor.Id : 0;

                var products = _productService.SearchProducts(vendorId: vendorId, showHidden: true);
                var xml = _exportManager.ExportProductsToXml(products);
                return new XmlDownloadResult(xml, "products.xml");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("List");
            }
        }

        public ActionResult ExportXmlSelected(string selectedIds)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var products = new List<Product>();
            if (selectedIds != null)
            {
                var ids = selectedIds
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Convert.ToInt32(x))
                    .ToArray();
                products.AddRange(_productService.GetProductsByIds(ids));
            }
            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null)
            {
                products = products.Where(p => p.VendorId == _workContext.CurrentVendor.Id).ToList();
            }

            var xml = _exportManager.ExportProductsToXml(products);
            return new XmlDownloadResult(xml, "products.xml");
        }

        public ActionResult ExportExcelAll()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            try
            {
                //a vendor should have access only to his products
                int vendorId = _workContext.CurrentVendor != null ? _workContext.CurrentVendor.Id : 0;

                var products = _productService.SearchProducts(vendorId: vendorId, showHidden: true);

                byte[] bytes = null;
                using (var stream = new MemoryStream())
                {
                    _exportManager.ExportProductsToXlsx(stream, products);
                    bytes = stream.ToArray();
                }
                return File(bytes, "text/xls", "products.xlsx");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("List");
            }
        }

        public ActionResult ExportExcelSelected(string selectedIds)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var products = new List<Product>();
            if (selectedIds != null)
            {
                var ids = selectedIds
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Convert.ToInt32(x))
                    .ToArray();
                products.AddRange(_productService.GetProductsByIds(ids));
            }
            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null)
            {
                products = products.Where(p => p.VendorId == _workContext.CurrentVendor.Id).ToList();
            }

            byte[] bytes = null;
            using (var stream = new MemoryStream())
            {
                _exportManager.ExportProductsToXlsx(stream, products);
                bytes = stream.ToArray();
            }
            return File(bytes, "text/xls", "products.xlsx");
        }

        [HttpPost]
        public ActionResult ImportExcel()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            //a vendor cannot import products
            if (_workContext.CurrentVendor != null)
                return AccessDeniedView();

            try
            {
                var file = Request.Files["importexcelfile"];
                if (file != null && file.ContentLength > 0)
                {
                    _importManager.ImportProductsFromXlsx(file.InputStream);
                }
                else
                {
                    ErrorNotification(_localizationService.GetResource("Admin.Common.UploadFile"));
                    return RedirectToAction("List");
                }
                SuccessNotification(_localizationService.GetResource("Admin.Catalog.Products.Imported"));
                return RedirectToAction("List");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("List");
            }

        }

        #endregion

        #region Low stock reports

        public ActionResult LowStockReport()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            return View();
        }
        [HttpPost]
        public ActionResult LowStockReportList(DataSourceRequest command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            int vendorId = 0;
            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null)
                vendorId = _workContext.CurrentVendor.Id;

            IList<Product> products;
            IList<ProductVariantAttributeCombination> combinations;
            _productService.GetLowStockProducts(vendorId, out products, out combinations);

            var models = new List<LowStockProductModel>();
            //products
            foreach (var product in products)
            {
                var lowStockModel = new LowStockProductModel()
                {
                    Id = product.Id,
                    Name = product.Name,
                    ManageInventoryMethod = product.ManageInventoryMethod.GetLocalizedEnum(_localizationService, _workContext.WorkingLanguage.Id),
                    StockQuantity = product.StockQuantity,
                    Published = product.Published
                };
                models.Add(lowStockModel);
            }
            //combinations
            foreach (var combination in combinations)
            {
                var product = combination.Product;
                var lowStockModel = new LowStockProductModel()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Attributes = _productAttributeFormatter.FormatAttributes(product, combination.AttributesXml, _workContext.CurrentCustomer, "<br />", true, true, true, false),
                    ManageInventoryMethod = product.ManageInventoryMethod.GetLocalizedEnum(_localizationService, _workContext.WorkingLanguage.Id),
                    StockQuantity = combination.StockQuantity,
                    Published = product.Published
                };
                models.Add(lowStockModel);
            }
            var gridModel = new DataSourceResult
            {
                Data = models.PagedForCommand(command),
                Total = models.Count
            };

            return Json(gridModel);
        }

        #endregion

        #region Bulk editing

        public ActionResult BulkEdit()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var model = new BulkEditListModel();
            //categories
            model.AvailableCategories.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            var categories = _categoryService.GetAllCategories(showHidden: true);
            foreach (var c in categories)
                model.AvailableCategories.Add(new SelectListItem() { Text = c.GetFormattedBreadCrumb(categories), Value = c.Id.ToString() });

            //manufacturers
            model.AvailableManufacturers.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            foreach (var m in _manufacturerService.GetAllManufacturers(showHidden: true))
                model.AvailableManufacturers.Add(new SelectListItem() { Text = m.Name, Value = m.Id.ToString() });

            //product types
            model.AvailableProductTypes = ProductType.SimpleProduct.ToSelectList(false).ToList();
            model.AvailableProductTypes.Insert(0, new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });

            return View(model);
        }

        [HttpPost]
        public ActionResult BulkEditSelect(DataSourceRequest command, BulkEditListModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            int vendorId = 0;
            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null)
                vendorId = _workContext.CurrentVendor.Id;

            var products = _productService.SearchProducts(categoryIds: new List<int>() { model.SearchCategoryId },
                manufacturerId: model.SearchManufacturerId,
                vendorId: vendorId,
                productType: model.SearchProductTypeId > 0 ? (ProductType?)model.SearchProductTypeId : null,
                keywords: model.SearchProductName,
                pageIndex: command.Page - 1,
                pageSize: command.PageSize,
                showHidden: true);
            var gridModel = new DataSourceResult();
            gridModel.Data = products.Select(x =>
            {
                var productModel = new BulkEditProductModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Sku = x.Sku,
                    OldPrice = x.OldPrice,
                    Price = x.Price,
                    ManageInventoryMethod = x.ManageInventoryMethod.GetLocalizedEnum(_localizationService, _workContext.WorkingLanguage.Id),
                    StockQuantity = x.StockQuantity,
                    Published = x.Published
                };

                return productModel;
            });
            gridModel.Total = products.TotalCount;

            return Json(gridModel);
        }

        [HttpPost]
        public ActionResult BulkEditUpdate(IEnumerable<BulkEditProductModel> products)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            if (products != null)
            {
                foreach (var pModel in products)
                {
                    //update
                    var product = _productService.GetProductById(pModel.Id);
                    if (product != null)
                    {
                        //a vendor should have access only to his products
                        if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
                            continue;

                        product.Sku = pModel.Sku;
                        product.Price = pModel.Price;
                        product.OldPrice = pModel.OldPrice;
                        product.StockQuantity = pModel.StockQuantity;
                        product.Published = pModel.Published;
                        _productService.UpdateProduct(product);
                    }
                }
            }

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult BulkEditDelete(IEnumerable<BulkEditProductModel> products)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            if (products != null)
            {
                foreach (var pModel in products)
                {
                    //delete
                    var product = _productService.GetProductById(pModel.Id);
                    if (product != null)
                    {
                        //a vendor should have access only to his products
                        if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
                            continue;

                        _productService.DeleteProduct(product);
                    }
                }
            }
            return new NullJsonResult();
        }

        #endregion

        #region Tier prices

        [HttpPost]
        public ActionResult TierPriceList(DataSourceRequest command, int productId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var product = _productService.GetProductById(productId);
            if (product == null)
                throw new ArgumentException("No product found with the specified id");

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
                return Content("This is not your product");

            var tierPricesModel = product.TierPrices
                .OrderBy(x => x.StoreId)
                .ThenBy(x => x.Quantity)
                .ThenBy(x => x.CustomerRoleId)
                .Select(x =>
                {
                    var storeName = "";
                    if (x.StoreId > 0)
                    {
                        var store = _storeService.GetStoreById(x.StoreId);
                        storeName = store != null ? store.Name : "Deleted";
                    }
                    else
                    {
                        storeName = _localizationService.GetResource("Admin.Catalog.Products.TierPrices.Fields.Store.All");
                    }
                    return new ProductModel.TierPriceModel()
                    {
                        Id = x.Id,
                        StoreId = x.StoreId,
                        Store = storeName,
                        CustomerRole = x.CustomerRoleId.HasValue ? _customerService.GetCustomerRoleById(x.CustomerRoleId.Value).Name : _localizationService.GetResource("Admin.Catalog.Products.TierPrices.Fields.CustomerRole.All"),
                        ProductId = x.ProductId,
                        CustomerRoleId = x.CustomerRoleId.HasValue ? x.CustomerRoleId.Value : 0,
                        Quantity = x.Quantity,
                        Price = x.Price
                    };
                })
                .ToList();

            var gridModel = new DataSourceResult
            {
                Data = tierPricesModel,
                Total = tierPricesModel.Count
            };

            return Json(gridModel);
        }

        [HttpPost]
        public ActionResult TierPriceInsert(ProductModel.TierPriceModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var product = _productService.GetProductById(model.ProductId);
            if (product == null)
                throw new ArgumentException("No product found with the specified id");

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
                return Content("This is not your product");

            var tierPrice = new TierPrice()
            {
                ProductId = model.ProductId,
                StoreId = model.StoreId,
                CustomerRoleId = model.CustomerRoleId > 0 ? model.CustomerRoleId : (int?)null,
                Quantity = model.Quantity,
                Price = model.Price
            };
            _productService.InsertTierPrice(tierPrice);

            //update "HasTierPrices" property
            _productService.UpdateHasTierPricesProperty(product);

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult TierPriceUpdate(ProductModel.TierPriceModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var tierPrice = _productService.GetTierPriceById(model.Id);
            if (tierPrice == null)
                throw new ArgumentException("No tier price found with the specified id");

            var product = _productService.GetProductById(tierPrice.ProductId);
            if (product == null)
                throw new ArgumentException("No product found with the specified id");

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
                return Content("This is not your product");

            tierPrice.StoreId = model.StoreId;
            tierPrice.CustomerRoleId = model.CustomerRoleId > 0 ? model.CustomerRoleId : (int?)null;
            tierPrice.Quantity = model.Quantity;
            tierPrice.Price = model.Price;
            _productService.UpdateTierPrice(tierPrice);

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult TierPriceDelete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var tierPrice = _productService.GetTierPriceById(id);
            if (tierPrice == null)
                throw new ArgumentException("No tier price found with the specified id");

            var productId = tierPrice.ProductId;
            var product = _productService.GetProductById(productId);
            if (product == null)
                throw new ArgumentException("No product found with the specified id");

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
                return Content("This is not your product");

            _productService.DeleteTierPrice(tierPrice);

            //update "HasTierPrices" property
            _productService.UpdateHasTierPricesProperty(product);

            return new NullJsonResult();
        }

        #endregion

        #region Product variant attributes

        [HttpPost]
        public ActionResult ProductVariantAttributeList(DataSourceRequest command, int productId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var product = _productService.GetProductById(productId);
            if (product == null)
                throw new ArgumentException("No product found with the specified id");

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
                return Content("This is not your product");

            var productVariantAttributes = _productAttributeService.GetProductVariantAttributesByProductId(productId);
            var productVariantAttributesModel = productVariantAttributes
                .Select(x =>
                {
                    var pvaModel = new ProductModel.ProductVariantAttributeModel()
                    {
                        Id = x.Id,
                        ProductId = x.ProductId,
                        ProductAttribute = _productAttributeService.GetProductAttributeById(x.ProductAttributeId).Name,
                        ProductAttributeId = x.ProductAttributeId,
                        TextPrompt = x.TextPrompt,
                        IsRequired = x.IsRequired,
                        AttributeControlType = x.AttributeControlType.GetLocalizedEnum(_localizationService, _workContext),
                        AttributeControlTypeId = x.AttributeControlTypeId,
                        DisplayOrder = x.DisplayOrder
                    };

                    if (x.ShouldHaveValues())
                    {
                        pvaModel.ViewEditValuesUrl = Url.Action("EditAttributeValues", "Product", new { productVariantAttributeId = x.Id });
                        pvaModel.ViewEditValuesText = string.Format(_localizationService.GetResource("Admin.Catalog.Products.ProductVariantAttributes.Attributes.Values.ViewLink"), x.ProductVariantAttributeValues != null ? x.ProductVariantAttributeValues.Count : 0);
                    }

                    pvaModel.ValidationRulesAllowed = x.ValidationRulesAllowed();
                    return pvaModel;
                })
                .ToList();

            var gridModel = new DataSourceResult
            {
                Data = productVariantAttributesModel,
                Total = productVariantAttributesModel.Count
            };

            return Json(gridModel);
        }

        [HttpPost]
        public ActionResult ProductVariantAttributeInsert(ProductModel.ProductVariantAttributeModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var product = _productService.GetProductById(model.ProductId);
            if (product == null)
                throw new ArgumentException("No product found with the specified id");

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
            {
                return Content("This is not your product");
            }

            var pva = new ProductVariantAttribute()
            {
                ProductId = model.ProductId,
                ProductAttributeId = model.ProductAttributeId,
                TextPrompt = model.TextPrompt,
                IsRequired = model.IsRequired,
                AttributeControlTypeId = model.AttributeControlTypeId,
                DisplayOrder = model.DisplayOrder
            };
            _productAttributeService.InsertProductVariantAttribute(pva);

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult ProductVariantAttributeUpdate(ProductModel.ProductVariantAttributeModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var pva = _productAttributeService.GetProductVariantAttributeById(model.Id);
            if (pva == null)
                throw new ArgumentException("No product variant attribute found with the specified id");

            var product = _productService.GetProductById(pva.ProductId);
            if (product == null)
                throw new ArgumentException("No product found with the specified id");

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
                return Content("This is not your product");

            pva.ProductAttributeId = model.ProductAttributeId;
            pva.TextPrompt = model.TextPrompt;
            pva.IsRequired = model.IsRequired;
            pva.AttributeControlTypeId = model.AttributeControlTypeId;
            pva.DisplayOrder = model.DisplayOrder;
            _productAttributeService.UpdateProductVariantAttribute(pva);

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult ProductVariantAttributeDelete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var pva = _productAttributeService.GetProductVariantAttributeById(id);
            if (pva == null)
                throw new ArgumentException("No product variant attribute found with the specified id");

            var productId = pva.ProductId;
            var product = _productService.GetProductById(productId);
            if (product == null)
                throw new ArgumentException("No product found with the specified id");


            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
                return Content("This is not your product");

            _productAttributeService.DeleteProductVariantAttribute(pva);

            return new NullJsonResult();
        }


        //edit
        public ActionResult ProductAttributeValidationRulesPopup(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var pva = _productAttributeService.GetProductVariantAttributeById(id);
            if (pva == null)
                //No attribute value found with the specified id
                return RedirectToAction("List", "Product");

            var product = _productService.GetProductById(pva.ProductId);
            if (product == null)
                throw new ArgumentException("No product found with the specified id");

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
                return RedirectToAction("List", "Product");

            var model = new ProductModel.ProductVariantAttributeModel()
            {
                //prepare only used properties
                Id = pva.Id,
                ValidationRulesAllowed = pva.ValidationRulesAllowed(),
                AttributeControlTypeId = pva.AttributeControlTypeId,
                ValidationMinLength = pva.ValidationMinLength,
                ValidationMaxLength = pva.ValidationMaxLength,
                ValidationFileAllowedExtensions = pva.ValidationFileAllowedExtensions,
                ValidationFileMaximumSize = pva.ValidationFileMaximumSize,
                DefaultValue = pva.DefaultValue,
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult ProductAttributeValidationRulesPopup(string btnId, string formId, ProductModel.ProductVariantAttributeModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var pva = _productAttributeService.GetProductVariantAttributeById(model.Id);
            if (pva == null)
                //No attribute value found with the specified id
                return RedirectToAction("List", "Product");

            var product = _productService.GetProductById(pva.ProductId);
            if (product == null)
                throw new ArgumentException("No product found with the specified id");

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
                return RedirectToAction("List", "Product");

            if (ModelState.IsValid)
            {
                pva.ValidationMinLength = model.ValidationMinLength;
                pva.ValidationMaxLength = model.ValidationMaxLength;
                pva.ValidationFileAllowedExtensions = model.ValidationFileAllowedExtensions;
                pva.ValidationFileMaximumSize = model.ValidationFileMaximumSize;
                pva.DefaultValue = model.DefaultValue;
                _productAttributeService.UpdateProductVariantAttribute(pva);

                ViewBag.RefreshPage = true;
                ViewBag.btnId = btnId;
                ViewBag.formId = formId;
                return View(model);
            }

            //If we got this far, something failed, redisplay form
            model.ValidationRulesAllowed = pva.ValidationRulesAllowed();
            model.AttributeControlTypeId = pva.AttributeControlTypeId;
            return View(model);
        }

        #endregion

        #region Product variant attribute values

        //list
        public ActionResult EditAttributeValues(int productVariantAttributeId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var pva = _productAttributeService.GetProductVariantAttributeById(productVariantAttributeId);
            if (pva == null)
                throw new ArgumentException("No product variant attribute found with the specified id");

            var product = _productService.GetProductById(pva.ProductId);
            if (product == null)
                throw new ArgumentException("No product found with the specified id");

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
                return RedirectToAction("List", "Product");

            var model = new ProductModel.ProductVariantAttributeValueListModel()
            {
                ProductName = product.Name,
                ProductId = pva.ProductId,
                ProductVariantAttributeName = pva.ProductAttribute.Name,
                ProductVariantAttributeId = pva.Id,
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult ProductAttributeValueList(int productVariantAttributeId, DataSourceRequest command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var pva = _productAttributeService.GetProductVariantAttributeById(productVariantAttributeId);
            if (pva == null)
                throw new ArgumentException("No product variant attribute found with the specified id");

            var product = _productService.GetProductById(pva.ProductId);
            if (product == null)
                throw new ArgumentException("No product found with the specified id");

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
                return Content("This is not your product");

            var values = _productAttributeService.GetProductVariantAttributeValues(productVariantAttributeId);
            var gridModel = new DataSourceResult
            {
                Data = values.Select(x =>
                {
                    Product associatedProduct = null;
                    if (x.AttributeValueType == AttributeValueType.AssociatedToProduct)
                    {
                        associatedProduct = _productService.GetProductById(x.AssociatedProductId);
                    }
                    var pictureThumbnailUrl = _pictureService.GetPictureUrl(x.PictureId, 75, false);
                    //little hack here. Grid is rendered wrong way with <inmg> without "src" attribute
                    if (String.IsNullOrEmpty(pictureThumbnailUrl))
                        pictureThumbnailUrl = _pictureService.GetPictureUrl(null, 1, true);
                    return new ProductModel.ProductVariantAttributeValueModel()
                    {
                        Id = x.Id,
                        ProductVariantAttributeId = x.ProductVariantAttributeId,
                        AttributeValueTypeId = x.AttributeValueTypeId,
                        AttributeValueTypeName = x.AttributeValueType.GetLocalizedEnum(_localizationService, _workContext),
                        AssociatedProductId = x.AssociatedProductId,
                        AssociatedProductName = associatedProduct != null ? associatedProduct.Name : "",
                        Name = x.ProductVariantAttribute.AttributeControlType != AttributeControlType.ColorSquares ? x.Name : string.Format("{0} - {1}", x.Name, x.ColorSquaresRgb),
                        ColorSquaresRgb = x.ColorSquaresRgb,
                        PriceAdjustment = x.PriceAdjustment,
                        PriceAdjustmentStr = x.AttributeValueType == AttributeValueType.Simple ? x.PriceAdjustment.ToString("G29") : "",
                        WeightAdjustment = x.WeightAdjustment,
                        WeightAdjustmentStr = x.AttributeValueType == AttributeValueType.Simple ? x.WeightAdjustment.ToString("G29") : "",
                        Cost = x.Cost,
                        Quantity = x.Quantity,
                        IsPreSelected = x.IsPreSelected,
                        DisplayOrder = x.DisplayOrder,
                        PictureId = x.PictureId,
                        PictureThumbnailUrl = pictureThumbnailUrl
                    };
                }),
                Total = values.Count()
            };

            return Json(gridModel);
        }

        //create
        public ActionResult ProductAttributeValueCreatePopup(int productAttributeAttributeId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var pva = _productAttributeService.GetProductVariantAttributeById(productAttributeAttributeId);
            if (pva == null)
                throw new ArgumentException("No product variant attribute found with the specified id");

            var product = _productService.GetProductById(pva.ProductId);
            if (product == null)
                throw new ArgumentException("No product found with the specified id");

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
                return RedirectToAction("List", "Product");

            var model = new ProductModel.ProductVariantAttributeValueModel();
            model.ProductVariantAttributeId = productAttributeAttributeId;

            //color squares
            model.DisplayColorSquaresRgb = pva.AttributeControlType == AttributeControlType.ColorSquares;
            model.ColorSquaresRgb = "#000000";

            //default qantity for associated product
            model.Quantity = 1;

            //locales
            AddLocales(_languageService, model.Locales);

            //pictures
            model.ProductPictureModels = _productService.GetProductPicturesByProductId(product.Id)
                .Select(x =>
                {
                    return new ProductModel.ProductPictureModel()
                    {
                        Id = x.Id,
                        ProductId = x.ProductId,
                        PictureId = x.PictureId,
                        PictureUrl = _pictureService.GetPictureUrl(x.PictureId),
                        DisplayOrder = x.DisplayOrder
                    };
                })
                .ToList();

            return View(model);
        }

        [HttpPost]
        public ActionResult ProductAttributeValueCreatePopup(string btnId, string formId, ProductModel.ProductVariantAttributeValueModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var pva = _productAttributeService.GetProductVariantAttributeById(model.ProductVariantAttributeId);
            if (pva == null)
                //No product variant attribute found with the specified id
                return RedirectToAction("List", "Product");

            var product = _productService.GetProductById(pva.ProductId);
            if (product == null)
                throw new ArgumentException("No product found with the specified id");

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
                return RedirectToAction("List", "Product");

            if (pva.AttributeControlType == AttributeControlType.ColorSquares)
            {
                //ensure valid color is chosen/entered
                if (String.IsNullOrEmpty(model.ColorSquaresRgb))
                    ModelState.AddModelError("", "Color is required");
                try
                {
                    var color = System.Drawing.ColorTranslator.FromHtml(model.ColorSquaresRgb);
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError("", exc.Message);
                }
            }

            if (ModelState.IsValid)
            {
                var pvav = new ProductVariantAttributeValue()
                {
                    ProductVariantAttributeId = model.ProductVariantAttributeId,
                    AttributeValueTypeId = model.AttributeValueTypeId,
                    AssociatedProductId = model.AssociatedProductId,
                    Name = model.Name,
                    ColorSquaresRgb = model.ColorSquaresRgb,
                    PriceAdjustment = model.PriceAdjustment,
                    WeightAdjustment = model.WeightAdjustment,
                    Cost = model.Cost,
                    Quantity = model.Quantity,
                    IsPreSelected = model.IsPreSelected,
                    DisplayOrder = model.DisplayOrder,
                    PictureId = model.PictureId,
                };

                _productAttributeService.InsertProductVariantAttributeValue(pvav);
                UpdateLocales(pvav, model);

                ViewBag.RefreshPage = true;
                ViewBag.btnId = btnId;
                ViewBag.formId = formId;
                return View(model);
            }

            //If we got this far, something failed, redisplay form


            //pictures
            model.ProductPictureModels = _productService.GetProductPicturesByProductId(product.Id)
                .Select(x =>
                {
                    return new ProductModel.ProductPictureModel()
                    {
                        Id = x.Id,
                        ProductId = x.ProductId,
                        PictureId = x.PictureId,
                        PictureUrl = _pictureService.GetPictureUrl(x.PictureId),
                        DisplayOrder = x.DisplayOrder
                    };
                })
                .ToList();

            var associatedProduct = _productService.GetProductById(model.AssociatedProductId);
            model.AssociatedProductName = associatedProduct != null ? associatedProduct.Name : "";

            return View(model);
        }

        //edit
        public ActionResult ProductAttributeValueEditPopup(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var pvav = _productAttributeService.GetProductVariantAttributeValueById(id);
            if (pvav == null)
                //No attribute value found with the specified id
                return RedirectToAction("List", "Product");

            var product = _productService.GetProductById(pvav.ProductVariantAttribute.ProductId);
            if (product == null)
                throw new ArgumentException("No product found with the specified id");

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
                return RedirectToAction("List", "Product");

            var associatedProduct = _productService.GetProductById(pvav.AssociatedProductId);

            var model = new ProductModel.ProductVariantAttributeValueModel()
            {
                ProductVariantAttributeId = pvav.ProductVariantAttributeId,
                AttributeValueTypeId = pvav.AttributeValueTypeId,
                AttributeValueTypeName = pvav.AttributeValueType.GetLocalizedEnum(_localizationService, _workContext),
                AssociatedProductId = pvav.AssociatedProductId,
                AssociatedProductName = associatedProduct != null ? associatedProduct.Name : "",
                Name = pvav.Name,
                ColorSquaresRgb = pvav.ColorSquaresRgb,
                DisplayColorSquaresRgb = pvav.ProductVariantAttribute.AttributeControlType == AttributeControlType.ColorSquares,
                PriceAdjustment = pvav.PriceAdjustment,
                WeightAdjustment = pvav.WeightAdjustment,
                Cost = pvav.Cost,
                Quantity = pvav.Quantity,
                IsPreSelected = pvav.IsPreSelected,
                DisplayOrder = pvav.DisplayOrder,
                PictureId = pvav.PictureId
            };
            if (model.DisplayColorSquaresRgb && String.IsNullOrEmpty(model.ColorSquaresRgb))
            {
                model.ColorSquaresRgb = "#000000";
            }
            //locales
            AddLocales(_languageService, model.Locales, (locale, languageId) =>
            {
                locale.Name = pvav.GetLocalized(x => x.Name, languageId, false, false);
            });
            //pictures
            model.ProductPictureModels = _productService.GetProductPicturesByProductId(product.Id)
                .Select(x =>
                {
                    return new ProductModel.ProductPictureModel()
                    {
                        Id = x.Id,
                        ProductId = x.ProductId,
                        PictureId = x.PictureId,
                        PictureUrl = _pictureService.GetPictureUrl(x.PictureId),
                        DisplayOrder = x.DisplayOrder
                    };
                })
                .ToList();

            return View(model);
        }

        [HttpPost]
        public ActionResult ProductAttributeValueEditPopup(string btnId, string formId, ProductModel.ProductVariantAttributeValueModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var pvav = _productAttributeService.GetProductVariantAttributeValueById(model.Id);
            if (pvav == null)
                //No attribute value found with the specified id
                return RedirectToAction("List", "Product");

            var product = _productService.GetProductById(pvav.ProductVariantAttribute.ProductId);
            if (product == null)
                throw new ArgumentException("No product found with the specified id");

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
                return RedirectToAction("List", "Product");

            if (pvav.ProductVariantAttribute.AttributeControlType == AttributeControlType.ColorSquares)
            {
                //ensure valid color is chosen/entered
                if (String.IsNullOrEmpty(model.ColorSquaresRgb))
                    ModelState.AddModelError("", "Color is required");
                try
                {
                    var color = System.Drawing.ColorTranslator.FromHtml(model.ColorSquaresRgb);
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError("", exc.Message);
                }
            }

            if (ModelState.IsValid)
            {
                pvav.AttributeValueTypeId = model.AttributeValueTypeId;
                pvav.AssociatedProductId = model.AssociatedProductId;
                pvav.Name = model.Name;
                pvav.ColorSquaresRgb = model.ColorSquaresRgb;
                pvav.PriceAdjustment = model.PriceAdjustment;
                pvav.WeightAdjustment = model.WeightAdjustment;
                pvav.Cost = model.Cost;
                pvav.Quantity = model.Quantity;
                pvav.IsPreSelected = model.IsPreSelected;
                pvav.DisplayOrder = model.DisplayOrder;
                pvav.PictureId = model.PictureId;
                _productAttributeService.UpdateProductVariantAttributeValue(pvav);

                UpdateLocales(pvav, model);

                ViewBag.RefreshPage = true;
                ViewBag.btnId = btnId;
                ViewBag.formId = formId;
                return View(model);
            }

            //If we got this far, something failed, redisplay form

            //pictures
            model.ProductPictureModels = _productService.GetProductPicturesByProductId(product.Id)
                .Select(x =>
                {
                    return new ProductModel.ProductPictureModel()
                    {
                        Id = x.Id,
                        ProductId = x.ProductId,
                        PictureId = x.PictureId,
                        PictureUrl = _pictureService.GetPictureUrl(x.PictureId),
                        DisplayOrder = x.DisplayOrder
                    };
                })
                .ToList();

            var associatedProduct = _productService.GetProductById(model.AssociatedProductId);
            model.AssociatedProductName = associatedProduct != null ? associatedProduct.Name : "";

            return View(model);
        }

        //delete
        [HttpPost]
        public ActionResult ProductAttributeValueDelete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var pvav = _productAttributeService.GetProductVariantAttributeValueById(id);
            if (pvav == null)
                throw new ArgumentException("No product variant attribute value found with the specified id");

            var product = _productService.GetProductById(pvav.ProductVariantAttribute.ProductId);
            if (product == null)
                throw new ArgumentException("No product found with the specified id");

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
                return Content("This is not your product");

            _productAttributeService.DeleteProductVariantAttributeValue(pvav);

            return new NullJsonResult();
        }





        public ActionResult AssociateProductToAttributeValuePopup()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var model = new ProductModel.ProductVariantAttributeValueModel.AssociateProductToAttributeValueModel();
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

            return View(model);
        }

        [HttpPost]
        public ActionResult AssociateProductToAttributeValuePopupList(DataSourceRequest command,
            ProductModel.ProductVariantAttributeValueModel.AssociateProductToAttributeValueModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null)
            {
                model.SearchVendorId = _workContext.CurrentVendor.Id;
            }

            var products = _productService.SearchProducts(
                categoryIds: new List<int>() { model.SearchCategoryId },
                manufacturerId: model.SearchManufacturerId,
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
        public ActionResult AssociateProductToAttributeValuePopup(string productIdInput,
            string productNameInput, ProductModel.ProductVariantAttributeValueModel.AssociateProductToAttributeValueModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var associatedProduct = _productService.GetProductById(model.AssociatedToProductId);
            if (associatedProduct == null)
                return Content("Cannot load a product");

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null && associatedProduct.VendorId != _workContext.CurrentVendor.Id)
                return Content("This is not your product");

            //a vendor should have access only to his products
            model.IsLoggedInAsVendor = _workContext.CurrentVendor != null;
            ViewBag.RefreshPage = true;
            ViewBag.productIdInput = productIdInput;
            ViewBag.productNameInput = productNameInput;
            ViewBag.productId = associatedProduct.Id;
            ViewBag.productName = associatedProduct.Name;
            return View(model);
        }


        #endregion

        #region Product variant attribute combinations

        [HttpPost]
        public ActionResult ProductVariantAttributeCombinationList(DataSourceRequest command, int productId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var product = _productService.GetProductById(productId);
            if (product == null)
                throw new ArgumentException("No product found with the specified id");

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
                return Content("This is not your product");

            var productVariantAttributeCombinations = _productAttributeService.GetAllProductVariantAttributeCombinations(productId);
            var productVariantAttributesModel = productVariantAttributeCombinations
                .Select(x =>
                {
                    var pvacModel = new ProductModel.ProductVariantAttributeCombinationModel()
                    {
                        Id = x.Id,
                        ProductId = x.ProductId,
                        AttributesXml = _productAttributeFormatter.FormatAttributes(x.Product, x.AttributesXml, _workContext.CurrentCustomer, "<br />", true, true, true, false),
                        StockQuantity = x.StockQuantity,
                        AllowOutOfStockOrders = x.AllowOutOfStockOrders,
                        Sku = x.Sku,
                        ManufacturerPartNumber = x.ManufacturerPartNumber,
                        Gtin = x.Gtin,
                        OverriddenPrice = x.OverriddenPrice
                    };
                    //warnings
                    var warnings = _shoppingCartService.GetShoppingCartItemAttributeWarnings(_workContext.CurrentCustomer,
                        ShoppingCartType.ShoppingCart, x.Product, 1, x.AttributesXml);
                    for (int i = 0; i < warnings.Count; i++)
                    {
                        pvacModel.Warnings += warnings[i];
                        if (i != warnings.Count - 1)
                            pvacModel.Warnings += "<br />";
                    }

                    return pvacModel;
                })
                .ToList();

            var gridModel = new DataSourceResult
            {
                Data = productVariantAttributesModel,
                Total = productVariantAttributesModel.Count
            };

            return Json(gridModel);
        }

        [HttpPost]
        public ActionResult ProductVariantAttributeCombinationUpdate(ProductModel.ProductVariantAttributeCombinationModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var pvac = _productAttributeService.GetProductVariantAttributeCombinationById(model.Id);
            if (pvac == null)
                throw new ArgumentException("No product variant attribute combination found with the specified id");

            var product = _productService.GetProductById(pvac.ProductId);
            if (product == null)
                throw new ArgumentException("No product found with the specified id");

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
                return Content("This is not your product");

            pvac.StockQuantity = model.StockQuantity;
            pvac.AllowOutOfStockOrders = model.AllowOutOfStockOrders;
            pvac.Sku = model.Sku;
            pvac.ManufacturerPartNumber = model.ManufacturerPartNumber;
            pvac.Gtin = model.Gtin;
            pvac.OverriddenPrice = model.OverriddenPrice;
            _productAttributeService.UpdateProductVariantAttributeCombination(pvac);

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult ProductVariantAttributeCombinationDelete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var pvac = _productAttributeService.GetProductVariantAttributeCombinationById(id);
            if (pvac == null)
                throw new ArgumentException("No product variant attribute combination found with the specified id");

            var product = _productService.GetProductById(pvac.ProductId);
            if (product == null)
                throw new ArgumentException("No product found with the specified id");

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
                return Content("This is not your product");

            var productId = pvac.ProductId;
            _productAttributeService.DeleteProductVariantAttributeCombination(pvac);

            return new NullJsonResult();
        }

        //edit
        public ActionResult AddAttributeCombinationPopup(string btnId, string formId, int productId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var product = _productService.GetProductById(productId);
            if (product == null)
                //No product found with the specified id
                return RedirectToAction("List", "Product");

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
                return RedirectToAction("List", "Product");

            ViewBag.btnId = btnId;
            ViewBag.formId = formId;
            var model = new AddProductVariantAttributeCombinationModel();
            PrepareAddProductAttributeCombinationModel(model, product);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddAttributeCombinationPopup(string btnId, string formId, int productId,
            AddProductVariantAttributeCombinationModel model, FormCollection form)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var product = _productService.GetProductById(productId);
            if (product == null)
                //No product found with the specified id
                return RedirectToAction("List", "Product");

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
                return RedirectToAction("List", "Product");

            ViewBag.btnId = btnId;
            ViewBag.formId = formId;

            //attributes
            string attributes = "";
            var warnings = new List<string>();

            #region Product attributes
            string selectedAttributes = string.Empty;
            var productVariantAttributes = _productAttributeService.GetProductVariantAttributesByProductId(product.Id);
            foreach (var attribute in productVariantAttributes)
            {
                string controlId = string.Format("product_attribute_{0}_{1}", attribute.ProductAttributeId, attribute.Id);
                switch (attribute.AttributeControlType)
                {
                    case AttributeControlType.DropdownList:
                    case AttributeControlType.RadioList:
                    case AttributeControlType.ColorSquares:
                        {
                            var ctrlAttributes = form[controlId];
                            if (!String.IsNullOrEmpty(ctrlAttributes))
                            {
                                int selectedAttributeId = int.Parse(ctrlAttributes);
                                if (selectedAttributeId > 0)
                                    selectedAttributes = _productAttributeParser.AddProductAttribute(selectedAttributes,
                                        attribute, selectedAttributeId.ToString());
                            }
                        }
                        break;
                    case AttributeControlType.Checkboxes:
                        {
                            var cblAttributes = form[controlId];
                            if (!String.IsNullOrEmpty(cblAttributes))
                            {
                                foreach (var item in cblAttributes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                                {
                                    int selectedAttributeId = int.Parse(item);
                                    if (selectedAttributeId > 0)
                                        selectedAttributes = _productAttributeParser.AddProductAttribute(selectedAttributes,
                                            attribute, selectedAttributeId.ToString());
                                }
                            }
                        }
                        break;
                    case AttributeControlType.ReadonlyCheckboxes:
                        {
                            //load read-only (already server-side selected) values
                            var pvaValues = _productAttributeService.GetProductVariantAttributeValues(attribute.Id);
                            foreach (var selectedAttributeId in pvaValues
                                .Where(pvav => pvav.IsPreSelected)
                                .Select(pvav => pvav.Id)
                                .ToList())
                            {
                                selectedAttributes = _productAttributeParser.AddProductAttribute(selectedAttributes,
                                    attribute, selectedAttributeId.ToString());
                            }
                        }
                        break;
                    case AttributeControlType.TextBox:
                    case AttributeControlType.MultilineTextbox:
                        {
                            var ctrlAttributes = form[controlId];
                            if (!String.IsNullOrEmpty(ctrlAttributes))
                            {
                                string enteredText = ctrlAttributes.Trim();
                                selectedAttributes = _productAttributeParser.AddProductAttribute(selectedAttributes,
                                    attribute, enteredText);
                            }
                        }
                        break;
                    case AttributeControlType.Datepicker:
                        {
                            var date = form[controlId + "_day"];
                            var month = form[controlId + "_month"];
                            var year = form[controlId + "_year"];
                            DateTime? selectedDate = null;
                            try
                            {
                                selectedDate = new DateTime(Int32.Parse(year), Int32.Parse(month), Int32.Parse(date));
                            }
                            catch { }
                            if (selectedDate.HasValue)
                            {
                                selectedAttributes = _productAttributeParser.AddProductAttribute(selectedAttributes,
                                    attribute, selectedDate.Value.ToString("D"));
                            }
                        }
                        break;
                    case AttributeControlType.FileUpload:
                        {
                            var httpPostedFile = this.Request.Files[controlId];
                            if ((httpPostedFile != null) && (!String.IsNullOrEmpty(httpPostedFile.FileName)))
                            {
                                var fileSizeOk = true;
                                if (attribute.ValidationFileMaximumSize.HasValue)
                                {
                                    //compare in bytes
                                    var maxFileSizeBytes = attribute.ValidationFileMaximumSize.Value * 1024;
                                    if (httpPostedFile.ContentLength > maxFileSizeBytes)
                                    {
                                        warnings.Add(string.Format(_localizationService.GetResource("ShoppingCart.MaximumUploadedFileSize"), attribute.ValidationFileMaximumSize.Value));
                                        fileSizeOk = false;
                                    }
                                }
                                if (fileSizeOk)
                                {
                                    //save an uploaded file
                                    var download = new Download()
                                    {
                                        DownloadGuid = Guid.NewGuid(),
                                        UseDownloadUrl = false,
                                        DownloadUrl = "",
                                        DownloadBinary = httpPostedFile.GetDownloadBits(),
                                        ContentType = httpPostedFile.ContentType,
                                        Filename = System.IO.Path.GetFileNameWithoutExtension(httpPostedFile.FileName),
                                        Extension = System.IO.Path.GetExtension(httpPostedFile.FileName),
                                        IsNew = true
                                    };
                                    _downloadService.InsertDownload(download);
                                    //save attribute
                                    selectedAttributes = _productAttributeParser.AddProductAttribute(selectedAttributes,
                                        attribute, download.DownloadGuid.ToString());
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            attributes = selectedAttributes;

            #endregion

            warnings.AddRange(_shoppingCartService.GetShoppingCartItemAttributeWarnings(_workContext.CurrentCustomer,
                ShoppingCartType.ShoppingCart, product, 1, attributes));
            if (warnings.Count == 0)
            {
                //save combination
                var combination = new ProductVariantAttributeCombination()
                {
                    ProductId = product.Id,
                    AttributesXml = attributes,
                    StockQuantity = model.StockQuantity,
                    AllowOutOfStockOrders = model.AllowOutOfStockOrders,
                    Sku = model.Sku,
                    ManufacturerPartNumber = model.ManufacturerPartNumber,
                    Gtin = model.Gtin,
                    OverriddenPrice = model.OverriddenPrice
                };
                _productAttributeService.InsertProductVariantAttributeCombination(combination);

                ViewBag.RefreshPage = true;
                return View(model);
            }
            else
            {
                //If we got this far, something failed, redisplay form
                PrepareAddProductAttributeCombinationModel(model, product);
                model.Warnings = warnings;
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult GenerateAllAttributeCombinations(int productId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var product = _productService.GetProductById(productId);
            if (product == null)
                throw new ArgumentException("No product found with the specified id");

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
                return Content("This is not your product");

            var allAttributesXml = _productAttributeParser.GenerateAllCombinations(product);
            foreach (var attributesXml in allAttributesXml)
            {
                var existingCombination = _productAttributeParser.FindProductVariantAttributeCombination(product, attributesXml);

                //already exists?
                if (existingCombination != null)
                    continue;

                //new one
                var warnings = new List<string>();
                warnings.AddRange(_shoppingCartService.GetShoppingCartItemAttributeWarnings(_workContext.CurrentCustomer,
                    ShoppingCartType.ShoppingCart, product, 1, attributesXml));
                if (warnings.Count != 0)
                    continue;

                //save combination
                var combination = new ProductVariantAttributeCombination()
                {
                    ProductId = product.Id,
                    AttributesXml = attributesXml,
                    StockQuantity = 10000,
                    AllowOutOfStockOrders = false,
                    Sku = null,
                    ManufacturerPartNumber = null,
                    Gtin = null,
                    OverriddenPrice = null
                };
                _productAttributeService.InsertProductVariantAttributeCombination(combination);
            }
            return Json(new { Success = true });
        }

        #endregion

        #endregion
    }
}
