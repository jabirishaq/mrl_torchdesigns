using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Stores;
using Nop.Data;
using Nop.Services.Events;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Services.Stores;

namespace Nop.Services.Catalog
{
    /// <summary>
    /// Product service
    /// </summary>
    public partial class ProductServiceCustome : IProductServiceCustome
    {
        #region Constants
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : product ID
        /// </remarks>
        private const string PRODUCTS_BY_ID_KEY = "Nop.product.id-{0}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string PRODUCTS_PATTERN_KEY = "Nop.product.";
        #endregion

        #region Fields

        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProductCustomeField> _customeproductfieldsRepository;
        private readonly IRepository<RelatedProduct> _relatedProductRepository;
        private readonly IRepository<CrossSellProduct> _crossSellProductRepository;
        private readonly IRepository<TierPrice> _tierPriceRepository;
        private readonly IRepository<LocalizedProperty> _localizedPropertyRepository;
        private readonly IRepository<AclRecord> _aclRepository;
        private readonly IRepository<StoreMapping> _storeMappingRepository;
        private readonly IRepository<ProductPicture> _productPictureRepository;
        private readonly IRepository<ProductSpecificationAttribute> _productSpecificationAttributeRepository;
        private readonly IRepository<ProductReview> _productReviewRepository;
        private readonly IProductAttributeService _productAttributeService;
        private readonly IProductAttributeParser _productAttributeParser;
        private readonly ILanguageService _languageService;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly IDataProvider _dataProvider;
        private readonly IDbContext _dbContext;
        private readonly ICacheManager _cacheManager;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly LocalizationSettings _localizationSettings;
        private readonly CommonSettings _commonSettings;
        private readonly CatalogSettings _catalogSettings;
        private readonly IEventPublisher _eventPublisher;
        private readonly IAclService _aclService;
        private readonly IStoreMappingService _storeMappingService;
        private readonly IRepository<ProductVideo> _productVideoRepository;
        private readonly IRepository<ProductInBox> _productinboxRepository;
        private readonly IRepository<ProductDownload> _productDownloadRepository;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="productRepository">Product repository</param>
        /// <param name="relatedProductRepository">Related product repository</param>
        /// <param name="crossSellProductRepository">Cross-sell product repository</param>
        /// <param name="tierPriceRepository">Tier price repository</param>
        /// <param name="localizedPropertyRepository">Localized property repository</param>
        /// <param name="aclRepository">ACL record repository</param>
        /// <param name="storeMappingRepository">Store mapping repository</param>
        /// <param name="productPictureRepository">Product picture repository</param>
        /// <param name="productSpecificationAttributeRepository">Product specification attribute repository</param>
        /// <param name="productReviewRepository">Product review repository</param>
        /// <param name="productAttributeService">Product attribute service</param>
        /// <param name="productAttributeParser">Product attribute parser service</param>
        /// <param name="languageService">Language service</param>
        /// <param name="workflowMessageService">Workflow message service</param>
        /// <param name="dataProvider">Data provider</param>
        /// <param name="dbContext">Database Context</param>
        /// <param name="workContext">Work context</param>
        /// <param name="storeContext">Store context</param>
        /// <param name="localizationSettings">Localization settings</param>
        /// <param name="commonSettings">Common settings</param>
        /// <param name="catalogSettings">Catalog settings</param>
        /// <param name="eventPublisher">Event published</param>
        /// <param name="aclService">ACL service</param>
        /// <param name="storeMappingService">Store mapping service</param>
        public ProductServiceCustome(ICacheManager cacheManager,
            IRepository<Product> productRepository,
            IRepository<ProductCustomeField> maximumRunsRepository,
            IRepository<RelatedProduct> relatedProductRepository,
            IRepository<CrossSellProduct> crossSellProductRepository,
            IRepository<TierPrice> tierPriceRepository,
            IRepository<ProductPicture> productPictureRepository,
            IRepository<LocalizedProperty> localizedPropertyRepository,
            IRepository<AclRecord> aclRepository,
            IRepository<StoreMapping> storeMappingRepository,
            IRepository<ProductSpecificationAttribute> productSpecificationAttributeRepository,
            IRepository<ProductReview> productReviewRepository,
            IProductAttributeService productAttributeService,
            IProductAttributeParser productAttributeParser,
            ILanguageService languageService,
            IWorkflowMessageService workflowMessageService,
            IDataProvider dataProvider,
            IDbContext dbContext,
            IWorkContext workContext,
            IStoreContext storeContext,
            LocalizationSettings localizationSettings,
            CommonSettings commonSettings,
            CatalogSettings catalogSettings,
            IEventPublisher eventPublisher,
            IAclService aclService,
            IStoreMappingService storeMappingService,IRepository<ProductVideo> productVideoRepository,
            IRepository<ProductInBox> productinboxRepository,
            IRepository<ProductDownload> productDownloadRepository)
        {
            this._cacheManager = cacheManager;
            this._productRepository = productRepository;
            this._customeproductfieldsRepository = maximumRunsRepository;
            this._relatedProductRepository = relatedProductRepository;
            this._crossSellProductRepository = crossSellProductRepository;
            this._tierPriceRepository = tierPriceRepository;
            this._productPictureRepository = productPictureRepository;
            this._localizedPropertyRepository = localizedPropertyRepository;
            this._aclRepository = aclRepository;
            this._storeMappingRepository = storeMappingRepository;
            this._productSpecificationAttributeRepository = productSpecificationAttributeRepository;
            this._productReviewRepository = productReviewRepository;
            this._productAttributeService = productAttributeService;
            this._productAttributeParser = productAttributeParser;
            this._languageService = languageService;
            this._workflowMessageService = workflowMessageService;
            this._dataProvider = dataProvider;
            this._dbContext = dbContext;
            this._workContext = workContext;
            this._storeContext = storeContext;
            this._localizationSettings = localizationSettings;
            this._commonSettings = commonSettings;
            this._catalogSettings = catalogSettings;
            this._eventPublisher = eventPublisher;
            this._aclService = aclService;
            this._storeMappingService = storeMappingService;
            this._productVideoRepository = productVideoRepository;
            this._productinboxRepository = productinboxRepository;
            this._productDownloadRepository = productDownloadRepository;
        }

        #endregion

        #region Methods

        #region Products

      

        public virtual IPagedList<Product> SearchProductsByLinq(
          int pageIndex = 0,
          int pageSize = int.MaxValue,
          IList<int> categoryIds = null,
          int manufacturerId = 0,
          string partno = null,
          int storeId = 0,
          int vendorId = 0,
          int warehouseId = 0,
          int parentGroupedProductId = 0,
          ProductType? productType = null,
          bool visibleIndividuallyOnly = false,
          bool? featuredProducts = null,
          decimal? priceMin = null,
          decimal? priceMax = null,
          int productTagId = 0,
          string keywords = null,
          bool searchDescriptions = false,
          bool searchSku = true,
          bool searchProductTags = false,
          int languageId = 0,
          IList<int> filteredSpecs = null,
          ProductSortingEnum orderBy = ProductSortingEnum.Position,
          bool showHidden = false)
        {



            IList<int> filterableSpecificationAttributeOptionIds = null;
            return SearchProductsusinglinq(out filterableSpecificationAttributeOptionIds, false,
                pageIndex, pageSize, categoryIds, manufacturerId, partno,
                storeId, vendorId, warehouseId,
                parentGroupedProductId, productType, visibleIndividuallyOnly, featuredProducts,
                priceMin, priceMax, productTagId, keywords, searchDescriptions, searchSku,
                searchProductTags, languageId, filteredSpecs, orderBy, showHidden);

        }



        public virtual IPagedList<Product> SearchProductsusinglinq(
         out IList<int> filterableSpecificationAttributeOptionIds,
         bool loadFilterableSpecificationAttributeOptionIds = false,
         int pageIndex = 0,
         int pageSize = 2147483647,  //Int32.MaxValue
         IList<int> categoryIds = null,
         int manufacturerId = 0,
         string partno = null,
         int storeId = 0,
         int vendorId = 0,
         int warehouseId = 0,
         int parentGroupedProductId = 0,
         ProductType? productType = null,
         bool visibleIndividuallyOnly = false,
         bool? featuredProducts = null,
         decimal? priceMin = null,
         decimal? priceMax = null,
         int productTagId = 0,
         string keywords = null,
         bool searchDescriptions = false,
         bool searchSku = true,
         bool searchProductTags = false,
         int languageId = 0,
         IList<int> filteredSpecs = null,
         ProductSortingEnum orderBy = ProductSortingEnum.Position,
         bool showHidden = false)
        {
            filterableSpecificationAttributeOptionIds = new List<int>();

            //search by keyword
            bool searchLocalizedValue = false;
            if (languageId > 0)
            {
                if (showHidden)
                {
                    searchLocalizedValue = true;
                }
                else
                {
                    //ensure that we have at least two published languages
                    var totalPublishedLanguages = _languageService.GetAllLanguages(storeId: _storeContext.CurrentStore.Id).Count;
                    searchLocalizedValue = totalPublishedLanguages >= 2;
                }
            }

            //validate "categoryIds" parameter
            if (categoryIds != null && categoryIds.Contains(0))
                categoryIds.Remove(0);

            //Access control list. Allowed customer roles
            var allowedCustomerRolesIds = _workContext.CurrentCustomer.CustomerRoles
                .Where(cr => cr.Active).Select(cr => cr.Id).ToList();


            //stored procedures aren't supported. Use LINQ

            #region Search products

            //products
            var query = _productRepository.Table;
            query = query.Where(p => !p.Deleted);
            if (!showHidden)
            {
                query = query.Where(p => p.Published);
            }
            if (parentGroupedProductId > 0)
            {
                query = query.Where(p => p.ParentGroupedProductId == parentGroupedProductId);
            }
            if (visibleIndividuallyOnly)
            {
                query = query.Where(p => p.VisibleIndividually);
            }
            if (productType.HasValue)
            {
                int productTypeId = (int)productType.Value;
                query = query.Where(p => p.ProductTypeId == productTypeId);
            }

            //The function 'CurrentUtcDateTime' is not supported by SQL Server Compact. 
            //That's why we pass the date value
            var nowUtc = DateTime.UtcNow;
            if (priceMin.HasValue)
            {
                //min price
                query = query.Where(p =>
                    //special price (specified price and valid date range)
                                    ((p.SpecialPrice.HasValue &&
                                      ((!p.SpecialPriceStartDateTimeUtc.HasValue ||
                                        p.SpecialPriceStartDateTimeUtc.Value < nowUtc) &&
                                       (!p.SpecialPriceEndDateTimeUtc.HasValue ||
                                        p.SpecialPriceEndDateTimeUtc.Value > nowUtc))) &&
                                     (p.SpecialPrice >= priceMin.Value))
                                    ||
                                        //regular price (price isn't specified or date range isn't valid)
                                    ((!p.SpecialPrice.HasValue ||
                                      ((p.SpecialPriceStartDateTimeUtc.HasValue &&
                                        p.SpecialPriceStartDateTimeUtc.Value > nowUtc) ||
                                       (p.SpecialPriceEndDateTimeUtc.HasValue &&
                                        p.SpecialPriceEndDateTimeUtc.Value < nowUtc))) &&
                                     (p.Price >= priceMin.Value)));
            }
            if (priceMax.HasValue)
            {
                //max price
                query = query.Where(p =>
                    //special price (specified price and valid date range)
                                    ((p.SpecialPrice.HasValue &&
                                      ((!p.SpecialPriceStartDateTimeUtc.HasValue ||
                                        p.SpecialPriceStartDateTimeUtc.Value < nowUtc) &&
                                       (!p.SpecialPriceEndDateTimeUtc.HasValue ||
                                        p.SpecialPriceEndDateTimeUtc.Value > nowUtc))) &&
                                     (p.SpecialPrice <= priceMax.Value))
                                    ||
                                        //regular price (price isn't specified or date range isn't valid)
                                    ((!p.SpecialPrice.HasValue ||
                                      ((p.SpecialPriceStartDateTimeUtc.HasValue &&
                                        p.SpecialPriceStartDateTimeUtc.Value > nowUtc) ||
                                       (p.SpecialPriceEndDateTimeUtc.HasValue &&
                                        p.SpecialPriceEndDateTimeUtc.Value < nowUtc))) &&
                                     (p.Price <= priceMax.Value)));
            }
            if (!showHidden)
            {
                //available dates
                query = query.Where(p =>
                    (!p.AvailableStartDateTimeUtc.HasValue || p.AvailableStartDateTimeUtc.Value < nowUtc) &&
                    (!p.AvailableEndDateTimeUtc.HasValue || p.AvailableEndDateTimeUtc.Value > nowUtc));
            }

            //searching by keyword
            if (!String.IsNullOrWhiteSpace(keywords))
            {
                query = from p in query
                        join lp in _localizedPropertyRepository.Table on p.Id equals lp.EntityId into p_lp
                        from lp in p_lp.DefaultIfEmpty()
                        from pt in p.ProductTags.DefaultIfEmpty()
                        where (p.Name.Contains(keywords)) ||
                              (searchDescriptions && p.ShortDescription.Contains(keywords)) ||
                              (searchDescriptions && p.FullDescription.Contains(keywords)) ||
                              (searchProductTags && pt.Name.Contains(keywords)) ||
                            //localized values
                              (searchLocalizedValue && lp.LanguageId == languageId && lp.LocaleKeyGroup == "Product" && lp.LocaleKey == "Name" && lp.LocaleValue.Contains(keywords)) ||
                              (searchDescriptions && searchLocalizedValue && lp.LanguageId == languageId && lp.LocaleKeyGroup == "Product" && lp.LocaleKey == "ShortDescription" && lp.LocaleValue.Contains(keywords)) ||
                              (searchDescriptions && searchLocalizedValue && lp.LanguageId == languageId && lp.LocaleKeyGroup == "Product" && lp.LocaleKey == "FullDescription" && lp.LocaleValue.Contains(keywords))
                        //UNDONE search localized values in associated product tags
                        select p;
            }

            if (!showHidden && !_catalogSettings.IgnoreAcl)
            {
                //ACL (access control list)
                query = from p in query
                        join acl in _aclRepository.Table
                        on new { c1 = p.Id, c2 = "Product" } equals new { c1 = acl.EntityId, c2 = acl.EntityName } into p_acl
                        from acl in p_acl.DefaultIfEmpty()
                        where !p.SubjectToAcl || allowedCustomerRolesIds.Contains(acl.CustomerRoleId)
                        select p;
            }

            if (storeId > 0 && !_catalogSettings.IgnoreStoreLimitations)
            {
                //Store mapping
                query = from p in query
                        join sm in _storeMappingRepository.Table
                        on new { c1 = p.Id, c2 = "Product" } equals new { c1 = sm.EntityId, c2 = sm.EntityName } into p_sm
                        from sm in p_sm.DefaultIfEmpty()
                        where !p.LimitedToStores || storeId == sm.StoreId
                        select p;
            }

            //search by specs
            if (filteredSpecs != null && filteredSpecs.Count > 0)
            {
                query = from p in query
                        where !filteredSpecs
                                   .Except(
                                       p.ProductSpecificationAttributes.Where(psa => psa.AllowFiltering).Select(
                                           psa => psa.SpecificationAttributeOptionId))
                                   .Any()
                        select p;
            }

            //category filtering
            if (categoryIds != null && categoryIds.Count > 0)
            {
                query = from p in query
                        from pc in p.ProductCategories.Where(pc => categoryIds.Contains(pc.CategoryId))
                        where (!featuredProducts.HasValue || featuredProducts.Value == pc.IsFeaturedProduct)
                        select p;
            }

            //manufacturer filtering
            if (manufacturerId > 0)
            {
                query = from p in query
                        from pm in p.ProductManufacturers.Where(pm => pm.ManufacturerId == manufacturerId)
                        where (!featuredProducts.HasValue || featuredProducts.Value == pm.IsFeaturedProduct)
                        select p;
            }

            if (partno != null)
            {
                query = from p in query.Where(p => p.ManufacturerPartNumber == partno)
                        select p;
            }
            //vendor filtering
            if (vendorId > 0)
            {
                query = query.Where(p => p.VendorId == vendorId);
            }

            //warehouse filtering
            if (warehouseId > 0)
            {
                query = query.Where(p => p.WarehouseId == warehouseId);
            }

            //related products filtering
            //if (relatedToProductId > 0)
            //{
            //    query = from p in query
            //            join rp in _relatedProductRepository.Table on p.Id equals rp.ProductId2
            //            where (relatedToProductId == rp.ProductId1)
            //            select p;
            //}

            //tag filtering
            if (productTagId > 0)
            {
                query = from p in query
                        from pt in p.ProductTags.Where(pt => pt.Id == productTagId)
                        select p;
            }

            //only distinct products (group by ID)
            //if we use standard Distinct() method, then all fields will be compared (low performance)
            //it'll not work in SQL Server Compact when searching products by a keyword)
            query = from p in query
                    group p by p.Id
                        into pGroup
                        orderby pGroup.Key
                        select pGroup.FirstOrDefault();

            //sort products
            if (orderBy == ProductSortingEnum.Position && categoryIds != null && categoryIds.Count > 0)
            {
                //category position
                var firstCategoryId = categoryIds[0];
                query = query.OrderBy(p => p.ProductCategories.FirstOrDefault(pc => pc.CategoryId == firstCategoryId).DisplayOrder);
            }
            else if (orderBy == ProductSortingEnum.Position && manufacturerId > 0)
            {
                //manufacturer position
                query =
                    query.OrderBy(p => p.ProductManufacturers.FirstOrDefault(pm => pm.ManufacturerId == manufacturerId).DisplayOrder);
            }
            else if (orderBy == ProductSortingEnum.Position && parentGroupedProductId > 0)
            {
                //parent grouped product specified (sort associated products)
                query = query.OrderBy(p => p.DisplayOrder);
            }
            else if (orderBy == ProductSortingEnum.Position)
            {
                //otherwise sort by name
                query = query.OrderBy(p => p.Name);
            }
            else if (orderBy == ProductSortingEnum.NameAsc)
            {
                //Name: A to Z
                query = query.OrderBy(p => p.Name);
            }
            else if (orderBy == ProductSortingEnum.NameDesc)
            {
                //Name: Z to A
                query = query.OrderByDescending(p => p.Name);
            }
            else if (orderBy == ProductSortingEnum.PriceAsc)
            {
                //Price: Low to High
                query = query.OrderBy(p => p.Price);
            }
            else if (orderBy == ProductSortingEnum.PriceDesc)
            {
                //Price: High to Low
                query = query.OrderByDescending(p => p.Price);
            }
            else if (orderBy == ProductSortingEnum.CreatedOn)
            {
                //creation date
                query = query.OrderByDescending(p => p.CreatedOnUtc);
            }
            else
            {
                //actually this code is not reachable
                query = query.OrderBy(p => p.Name);
            }

            var products = new PagedList<Product>(query, pageIndex, pageSize);

            //get filterable specification attribute option identifier
            if (loadFilterableSpecificationAttributeOptionIds)
            {
                var querySpecs = from p in query
                                 join psa in _productSpecificationAttributeRepository.Table on p.Id equals psa.ProductId
                                 where psa.AllowFiltering
                                 select psa.SpecificationAttributeOptionId;
                //only distinct attributes
                filterableSpecificationAttributeOptionIds = querySpecs
                    .Distinct()
                    .ToList();
            }

            //return products
            return products;

            #endregion

        }

        /// <summary>
        /// Gets a product by Product Part Number
        /// </summary>
        /// <param name="partnumber">Part Number</param>
        /// <returns>Product</returns>
        public virtual Product GetProductByPartnumber(string partnumber)
        {
            if (String.IsNullOrEmpty(partnumber))
                return null;

            partnumber = partnumber.Trim();

            var query = from p in _productRepository.Table
                        orderby p.Id
                        where !p.Deleted &&
                        p.ManufacturerPartNumber == partnumber
                        select p;
            var product = query.FirstOrDefault();
            return product;
        }

        public virtual ProductPicture GetProductPictureByPictureId(int PictureId)
        {
            if (PictureId == 0)
                return null;
            var query = (from a in _productPictureRepository.Table
                         where a.PictureId == PictureId
                         select a);
            var productPictures = query.Single();
            return productPictures;

        }



        /// <summary>
        /// Inserts a product
        /// </summary>
        /// <param name="product">Product</param>
        public virtual void InsertProductCustomeField(ProductCustomeField product)
        {
            if (product == null)
                throw new ArgumentNullException("product field not inserted");

            //insert
            _customeproductfieldsRepository.Insert(product);

            //clear cache
            _cacheManager.RemoveByPattern(PRODUCTS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(product);
        }

        /// <summary>
        /// Updates the product
        /// </summary>
        /// <param name="product">Product</param>
        public virtual void UpdateProductCustomeField(ProductCustomeField product)
        {
            if (product == null)
                throw new ArgumentNullException("product field not updated");

            //update
            _customeproductfieldsRepository.Update(product);

            //cache
            _cacheManager.RemoveByPattern(PRODUCTS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(product);
        }


        public virtual ProductCustomeField GetCustomeFieldByProductId(int productId)
          {
            var query = from p in _customeproductfieldsRepository.Table
                        where p.ProductId == productId
                        select p;
            
            return query.FirstOrDefault();
        }

        #endregion


        #region Product Video

        /// <summary>
        /// Deletes a product video
        /// </summary>
        /// <param name="productvideo">Product video</param>
        public virtual void DeleteProductVideo(ProductVideo productvideo)
        {
            if (productvideo == null)
                throw new ArgumentNullException("productvideo");

            _productVideoRepository.Delete(productvideo);

            //event notification
            _eventPublisher.EntityDeleted(productvideo);
        }

        /// <summary>
        /// Gets a product videos by product identifier
        /// </summary>
        /// <param name="productId">The product identifier</param>
        /// <returns>Product videos</returns>
        public virtual IList<ProductVideo> GetProductVideoByProductId(int productId)
        {
            var query = from pp in _productVideoRepository.Table
                        where pp.ProductId == productId
                        orderby pp.DisplayOrder
                        select pp;
            var productvideos = query.ToList();
            return productvideos;
        }

        /// <summary>
        /// Gets a product video
        /// </summary>
        /// <param name="productvideoId">Product video identifier</param>
        /// <returns>Product video</returns>
        public virtual ProductVideo GetProductVideoById(int productvideoId)
        {
            if (productvideoId == 0)
                return null;

            return _productVideoRepository.GetById(productvideoId);
        }
        public virtual ProductVideo GetProductVideoByPictureId(int pictureid)
        {
            if (pictureid == 0)
                return null;
            var query = (from a in _productVideoRepository.Table
                         where a.PictureId == pictureid
                         select a);
            var productvideos = query.Single();
            return productvideos;

        }

        /// <summary>
        /// Inserts a product video
        /// </summary>
        /// <param name="productvideo">Product video</param>
        public virtual void InsertProductVideo(ProductVideo productvideo)
        {
            if (productvideo == null)
                throw new ArgumentNullException("productvideo");

            _productVideoRepository.Insert(productvideo);

            //event notification
            _eventPublisher.EntityInserted(productvideo);
        }

        /// <summary>
        /// Updates a product video
        /// </summary>
        /// <param name="productvideo">Product video</param>
        public virtual void UpdateProductVideo(ProductVideo productvideo)
        {
            if (productvideo == null)
                throw new ArgumentNullException("productvideo");

            _productVideoRepository.Update(productvideo);

            //event notification
            _eventPublisher.EntityUpdated(productvideo);
        }

        #endregion

        #region Product In Box

        /// <summary>
        /// Deletes a product  In Box
        /// </summary>
        /// <param name="product In Box">Product  In Box</param>
        public virtual void DeleteProductFromBox(ProductInBox product)
        {
            if (product == null)
                throw new ArgumentNullException("Product In Box");

            _productinboxRepository.Delete(product);

            //event notification
            _eventPublisher.EntityDeleted(product);
        }

        /// <summary>
        /// Gets a product  In Boxs by product identifier
        /// </summary>
        /// <param name="productId">The product identifier</param>
        /// <returns>Product  In Boxs</returns>
        public virtual IList<ProductInBox> GetProductFromBoxByPerantProductId(int parentproductid)
        {
            var query = from pp in _productinboxRepository.Table
                        where pp.ParentProductId == parentproductid
                        orderby pp.Displayorder
                        select pp;
            var productinbox = query.ToList();
            return productinbox;
        }

        /// <summary>
        /// Gets a product  In Box
        /// </summary>
        /// <param name="product In BoxId">Product  In Box identifier</param>
        /// <returns>Product  In Box</returns>
        public virtual ProductInBox GetProductFromBoxById(int id)
        {
            if (id == 0)
                return null;

            return _productinboxRepository.GetById(id);
        }
        public virtual ProductInBox GetProductFromBoxByInBoxProductId(int inboxproductid)
        {
            if (inboxproductid == 0)
                return null;
            var query = (from a in _productinboxRepository.Table
                         where a.InboxProductId == inboxproductid
                         orderby a.Displayorder
                         select a);
            var productInBoxs = query.Single();
            return productInBoxs;

        }

        /// <summary>
        /// Inserts a product  In Box
        /// </summary>
        /// <param name="product In Box">Product  In Box</param>
        public virtual void InsertProductInBox(ProductInBox product)
        {
            if (product == null)
                throw new ArgumentNullException("product in box");

            _productinboxRepository.Insert(product);

            //event notification
            _eventPublisher.EntityInserted(product);
        }

        /// <summary>
        /// Updates a product  In Box
        /// </summary>
        /// <param name="product In Box">Product  In Box</param>
        public virtual void UpdateProductInBox(ProductInBox product)
        {
            if (product == null)
                throw new ArgumentNullException("product in box");

            _productinboxRepository.Update(product);

            //event notification
            _eventPublisher.EntityUpdated(product);
        }

        #endregion

        #region Product Download

        /// <summary>
        /// Deletes a product download
        /// </summary>
        /// <param name="productdownload">Product download</param>
        public virtual void DeleteProductDownload(ProductDownload productdownload)
        {
            if (productdownload == null)
                throw new ArgumentNullException("productdownload");

            _productDownloadRepository.Delete(productdownload);

            //event notification
            _eventPublisher.EntityDeleted(productdownload);
        }

        /// <summary>
        /// Gets a product downloads by product identifier
        /// </summary>
        /// <param name="productId">The product identifier</param>
        /// <returns>Product downloads</returns>
        public virtual IList<ProductDownload> GetProductDownloadByProductId(int productId)
        {
            var query = from pp in _productDownloadRepository.Table
                        where pp.ProductId == productId
                        orderby pp.DisplayOrder
                        select pp;
            var productdownloads = query.ToList();
            return productdownloads;
        }

        /// <summary>
        /// Gets a product download
        /// </summary>
        /// <param name="productdownloadId">Product download identifier</param>
        /// <returns>Product download</returns>
        public virtual ProductDownload GetProductDownloadById(int productdownloadId)
        {
            if (productdownloadId == 0)
                return null;

            return _productDownloadRepository.GetById(productdownloadId);
        }
        //public virtual ProductDownload GetProductDownloadByPictureId(int pictureid)
        //{
        //    if (pictureid == 0)
        //        return null;
        //    var query = (from a in _productDownloadRepository.Table
        //                 where a.PictureId == pictureid
        //                 select a);
        //    var productdownloads = query.Single();
        //    return productdownloads;

        //}

        /// <summary>
        /// Inserts a product download
        /// </summary>
        /// <param name="productdownload">Product download</param>
        public virtual void InsertProductDownload(ProductDownload productdownload)
        {
            if (productdownload == null)
                throw new ArgumentNullException("productdownload");

            _productDownloadRepository.Insert(productdownload);

            //event notification
            _eventPublisher.EntityInserted(productdownload);
        }

        /// <summary>
        /// Updates a product download
        /// </summary>
        /// <param name="productdownload">Product download</param>
        public virtual void UpdateProductDownload(ProductDownload productdownload)
        {
            if (productdownload == null)
                throw new ArgumentNullException("productdownload");

            _productDownloadRepository.Update(productdownload);

            //event notification
            _eventPublisher.EntityUpdated(productdownload);
        }

        #endregion

        #endregion



    }
}
