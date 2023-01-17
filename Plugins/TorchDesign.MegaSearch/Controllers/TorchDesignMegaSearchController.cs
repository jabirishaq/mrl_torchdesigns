using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Nop.Core;
using Nop.Plugin.TorchDesign.MegaSearch.Services;
using Nop.Core.Domain.Directory;
using Nop.Plugin.TorchDesign.MegaSearch.Domain;
using Nop.Plugin.TorchDesign.MegaSearch.Models;
using Nop.Services.Configuration;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Kendoui;
using Nop.Web.Framework.Mvc;
using Nop.Plugin.TorchDesign.MegaSearch;
using Nop.Services.Stores;
using System.Xml;
using System.IO;
using System.Net;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using Nop.Services.Helpers;
using Nop.Core.Data;
using System.Data;
using Nop.Plugin.TorchDesign.MegaSearch.Data;
using Nop.Core.Caching;
using Nop.Core.Domain.Forums;
using Nop.Core.Domain.Blogs;
using Nop.Core.Domain.Vendors;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Media;
using Nop.Services.Common;
using Nop.Services.Events;
using Nop.Services.Logging;
using Nop.Services.Catalog;
using Nop.Services.Media;
using Nop.Services.Tax;
using Nop.Services.Vendors;
using Nop.Services.Orders;
using Nop.Services.Blogs;
using Nop.Services.News;
using Nop.Services.Seo;
using Nop.Core.Plugins;
using Nop.Plugin.Widgets.TorchDesign_Support.Services;
namespace Nop.Plugin.TorchDesign.MegaSearch.Controllers
{

    public partial class TorchDesignMegaSearchController : BasePluginController
    {
        #region Fields
        private readonly ILanguageService _languageService;
        private readonly ISettingService _settingService;
        private readonly IStoreService _storeService;
        private readonly TorchDesignMegaSearchSettings _megasearchsetting;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IDataProvider _dataProvider;
        private readonly MegaSearchObjctContext _megasearchcontext;
        private readonly ISupportService _supportService;
        private readonly IOrderReportService _orderReportService;
        private readonly ICategoryService _categoryService;
        private readonly IManufacturerService _manufacturerService;
        private readonly IBlogService _blogService;
        private readonly INewsService _newsService;

        private readonly IProductService _productService;
        private readonly IVendorService _vendorService;
        private readonly ICategoryTemplateService _categoryTemplateService;
        private readonly IManufacturerTemplateService _manufacturerTemplateService;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly ITaxService _taxService;
        private readonly ICurrencyService _currencyService;
        private readonly IPictureService _pictureService;
        private readonly ILocalizationService _localizationService;
        private readonly IPriceCalculationService _priceCalculationService;
        private readonly IPriceFormatter _priceFormatter;
        private readonly IWebHelper _webHelper;
        private readonly ISpecificationAttributeService _specificationAttributeService;
        private readonly IProductTagService _productTagService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IAclService _aclService;
        private readonly IStoreMappingService _storeMappingService;
        private readonly IPermissionService _permissionService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IEventPublisher _eventPublisher;
        private readonly ISearchTermService _searchTermService;
        private readonly MediaSettings _mediaSettings;
        private readonly CatalogSettings _catalogSettings;
        private readonly VendorSettings _vendorSettings;
        private readonly BlogSettings _blogSettings;
        private readonly ForumSettings _forumSettings;
        private readonly ICacheManager _cacheManager;
        
        #endregion

		#region Constructors

        public TorchDesignMegaSearchController(ILanguageService languageService, ISettingService settingService,IStoreService storeService,TorchDesignMegaSearchSettings megasearchsetting,IDateTimeHelper dateTimeHelper,IDataProvider dataProvider,MegaSearchObjctContext megasearchcontext, IOrderReportService orderReportService, ICategoryService categoryService, 
            IManufacturerService manufacturerService,
            IProductService productService, 
            IVendorService vendorService,
            ICategoryTemplateService categoryTemplateService,
            IManufacturerTemplateService manufacturerTemplateService,

            IBlogService blogService,
            INewsService newsService,

            IWorkContext workContext, 
            IStoreContext storeContext,
            ITaxService taxService, 
            ICurrencyService currencyService,
            IPictureService pictureService, 
            ILocalizationService localizationService,
            IPriceCalculationService priceCalculationService,
            IPriceFormatter priceFormatter,
            IWebHelper webHelper, 
            ISpecificationAttributeService specificationAttributeService,
            IProductTagService productTagService,
            IGenericAttributeService genericAttributeService,
            IAclService aclService,
            IStoreMappingService storeMappingService,
            IPermissionService permissionService, 
            ICustomerActivityService customerActivityService,
            IEventPublisher eventPublisher,
            ISearchTermService searchTermService,
            MediaSettings mediaSettings,
            CatalogSettings catalogSettings,
            VendorSettings vendorSettings,
            BlogSettings blogSettings,
            ForumSettings  forumSettings,
            ICacheManager cacheManager, ISupportService supportService)
        {
            this._languageService = languageService;
          
            this._localizationService = localizationService;
            this._workContext = workContext;
            this._settingService = settingService;
            this._storeService = storeService;
            this._dateTimeHelper = dateTimeHelper;
            this._dataProvider = dataProvider;
            this._megasearchcontext = megasearchcontext;
            this._orderReportService = orderReportService;
            this._categoryService = categoryService;
            this._manufacturerService = manufacturerService;
            this._megasearchsetting = megasearchsetting;
            
            this._blogService = blogService;
            this._newsService = newsService;

            this._productService = productService;
            this._vendorService = vendorService;
            this._categoryTemplateService = categoryTemplateService;
            this._manufacturerTemplateService = manufacturerTemplateService;
            this._workContext = workContext;
            this._storeContext = storeContext;
            this._taxService = taxService;
            this._currencyService = currencyService;
            this._pictureService = pictureService;
            this._localizationService = localizationService;
            this._priceCalculationService = priceCalculationService;
            this._priceFormatter = priceFormatter;
            this._webHelper = webHelper;
            this._specificationAttributeService = specificationAttributeService;
            this._productTagService = productTagService;
            this._genericAttributeService = genericAttributeService;
            this._aclService = aclService;
            this._storeMappingService = storeMappingService;
            this._permissionService = permissionService;
            this._customerActivityService = customerActivityService;
            this._eventPublisher = eventPublisher;
            this._searchTermService = searchTermService;
            this._mediaSettings = mediaSettings;
            this._catalogSettings = catalogSettings;
            this._vendorSettings = vendorSettings;
            this._blogSettings = blogSettings;
            this._forumSettings = forumSettings;
            this._cacheManager = cacheManager;
            this._supportService = supportService;
        }
        #endregion

        #region Utilities
        #region Configure

        [AdminAuthorize]
        public ActionResult Configure()
        {
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var Searchsetting = _settingService.LoadSetting<TorchDesignMegaSearchSettings>(storeScope);
            var model = new ConfigurationModel();
            model.ProductSearch = Searchsetting.ProductSearch;
            model.BlogPostSearch = Searchsetting.BlogPostSearch;
            model.ByBlogPostDescription = Searchsetting.ByBlogPostDescription;
            model.ManufacturerSearch = Searchsetting.ManufacturerSearch;
            model.NewsSearch = Searchsetting.NewsSearch;
            model.CategorySearch = Searchsetting.CategorySearch;
            model.ByManufacturerDescription = Searchsetting.ByManufacturerDescription;
            model.ByNewsDescription = Searchsetting.ByNewsDescription;
            model.ByCategoryDescription = Searchsetting.ByCategoryDescription;
            model.ByProductDescription = Searchsetting.ByProductDescription;
            model.ByProductSku = Searchsetting.ByProductSku;
            model.ByProductTag = Searchsetting.ByProductTag;
            model.ByVideoTag = Searchsetting.ByVideoTag;
            model.ByProductPartNo = Searchsetting.ByProductPartno;

            if (storeScope > 0)
            {
                model.ProductSearch_OverrideForStore = _settingService.SettingExists(Searchsetting, x => x.ProductSearch, storeScope);
                model.BlogPostSearch_OverrideForStore = _settingService.SettingExists(Searchsetting, x => x.BlogPostSearch, storeScope);
                model.ByBlogPostDescription_OverrideForStore = _settingService.SettingExists(Searchsetting, x => x.ByBlogPostDescription, storeScope);
                model.ManufacturerSearch_OverrideForStore = _settingService.SettingExists(Searchsetting, x => x.ManufacturerSearch, storeScope);
                model.NewsSearch_OverrideForStore = _settingService.SettingExists(Searchsetting, x => x.NewsSearch, storeScope);
                model.CategorySearch_OverrideForStore = _settingService.SettingExists(Searchsetting, x => x.CategorySearch, storeScope);
                model.ByManufacturerDescription_OverrideForStore = _settingService.SettingExists(Searchsetting, x => x.ByManufacturerDescription, storeScope);
                model.ByCategoryDescription_OverrideForStore = _settingService.SettingExists(Searchsetting, x => x.ByCategoryDescription, storeScope);
                model.ByNewsDescription_OverrideForStore = _settingService.SettingExists(Searchsetting, x => x.ByNewsDescription, storeScope);
                model.ByProductTag_OverrideForStore = _settingService.SettingExists(Searchsetting, x => x.ByProductTag, storeScope);
                model.ByVideoTag_OverrideForStore = _settingService.SettingExists(Searchsetting, x => x.ByVideoTag, storeScope);
                model.ByProductSku_OverrideForStore = _settingService.SettingExists(Searchsetting, x => x.ByProductSku, storeScope);
                model.ByProductDescription_OverrideForStore = _settingService.SettingExists(Searchsetting, x => x.ByProductDescription, storeScope);
                 model.ByProductPartNo_OverrideForStore = _settingService.SettingExists(Searchsetting, x => x.ByProductPartno, storeScope);

            }

            return View("~/Plugins/TorchDesign.MegaSearch/Views/TorchDesignMegaSearch/Configure.cshtml", model);
        }

        [HttpPost]
        public ActionResult Configure(ConfigurationModel model)
        {
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var MegaSearchsetting = _settingService.LoadSetting<TorchDesignMegaSearchSettings>(storeScope);
            MegaSearchsetting.ProductSearch = model.ProductSearch;
            MegaSearchsetting.BlogPostSearch = model.BlogPostSearch;
            MegaSearchsetting.NewsSearch = model.NewsSearch;
            MegaSearchsetting.CategorySearch = model.CategorySearch;
            MegaSearchsetting.ManufacturerSearch = model.ManufacturerSearch;
            MegaSearchsetting.ByCategoryDescription = model.ByCategoryDescription;
            MegaSearchsetting.ByManufacturerDescription = model.ByManufacturerDescription;
            MegaSearchsetting.ByNewsDescription = model.ByNewsDescription;
            MegaSearchsetting.ByBlogPostDescription = model.ByBlogPostDescription;
            MegaSearchsetting.ByProductTag = model.ByProductTag;
            MegaSearchsetting.ByProductSku = model.ByProductSku;
            MegaSearchsetting.ByProductDescription = model.ByProductDescription;
            MegaSearchsetting.ByVideoTag = model.ByVideoTag;
            MegaSearchsetting.ByProductPartno = model.ByProductPartNo;


            if (model.ProductSearch_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(MegaSearchsetting, x => x.ProductSearch, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(MegaSearchsetting, x => x.ProductSearch, storeScope);

            if (model.BlogPostSearch_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(MegaSearchsetting, x => x.BlogPostSearch, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(MegaSearchsetting, x => x.BlogPostSearch, storeScope);

            if (model.ByBlogPostDescription_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(MegaSearchsetting, x => x.ByBlogPostDescription, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(MegaSearchsetting, x => x.ByBlogPostDescription, storeScope);

            if (model.ManufacturerSearch_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(MegaSearchsetting, x => x.ManufacturerSearch, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(MegaSearchsetting, x => x.ManufacturerSearch, storeScope);

            if (model.NewsSearch_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(MegaSearchsetting, x => x.NewsSearch, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(MegaSearchsetting, x => x.NewsSearch, storeScope);

            if (model.CategorySearch_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(MegaSearchsetting, x => x.CategorySearch, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(MegaSearchsetting, x => x.CategorySearch, storeScope);
            if (model.ByCategoryDescription_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(MegaSearchsetting, x => x.ByCategoryDescription, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(MegaSearchsetting, x => x.ByCategoryDescription, storeScope);
            if (model.ByManufacturerDescription_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(MegaSearchsetting, x => x.ByManufacturerDescription, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(MegaSearchsetting, x => x.ByManufacturerDescription, storeScope);
            if (model.ByNewsDescription_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(MegaSearchsetting, x => x.ByNewsDescription, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(MegaSearchsetting, x => x.ByNewsDescription, storeScope);
            if (model.ByProductDescription_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(MegaSearchsetting, x => x.ByProductDescription, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(MegaSearchsetting, x => x.ByProductDescription, storeScope);
            if (model.ByProductSku_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(MegaSearchsetting, x => x.ByProductSku, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(MegaSearchsetting, x => x.ByProductSku, storeScope);
            if (model.ByProductTag_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(MegaSearchsetting, x => x.ByProductTag, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(MegaSearchsetting, x => x.ByProductTag, storeScope);
            if (model.ByVideoTag_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(MegaSearchsetting, x => x.ByVideoTag, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(MegaSearchsetting, x => x.ByVideoTag, storeScope);
            if (model.ByProductPartNo_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(MegaSearchsetting, x => x.ByProductPartno, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(MegaSearchsetting, x => x.ByProductPartno, storeScope);

            _settingService.ClearCache();

            return View("~/Plugins/TorchDesign.MegaSearch/Views/TorchDesignMegaSearch/Configure.cshtml", model);

        }



        #endregion

        #region SearchPublicInfo

        public ActionResult SearchPublicInfo()
        {
            PublicInfoModel model = new PublicInfoModel();
            model.IsResultFound = false;
            model.IsTextEmpty = true;
            //model.AvailableDisplayOption.Add(new SelectListItem() { Text = "List", Value = "List" });
            //model.AvailableDisplayOption.Add(new SelectListItem() { Text = "Grid", Value = "Grid",Selected=true });
            //model.Viewmode = "Grid";
            return View("~/Plugins/TorchDesign.MegaSearch/Views/TorchDesignMegaSearch/SearchPublicInfo.cshtml", model);
        }

        [HttpGet]
        //[ValidateAntiForgeryToken]
        public ActionResult SearchPublicInfo(DataSourceRequest command, PublicInfoModel model)
        {
            if (!String.IsNullOrEmpty(model.KeywordText))
            {
                model.IsTextEmpty = false;
                int productserch, bysku, bytag, byvideotag ,bypartno, bypdisc, manufacsearch, bymdisc, categorysearch, bycdesc, newssearch, byndesc, blogserch, bybsearch;
                int fulltext = 1, usefullmode = 0, langid = 1;
                if (_megasearchsetting.ProductSearch)
                    productserch = 1;
                else
                    productserch = 0;

                if (_megasearchsetting.ByProductSku)
                    bysku = 1;
                else
                    bysku = 0;

                if (_megasearchsetting.ByProductTag)
                    bytag = 1;
                else
                    bytag = 0;

                if (_megasearchsetting.ByProductPartno)
                    bypartno = 1;
                else
                    bypartno = 0;

                if (_megasearchsetting.ByVideoTag)
                    byvideotag = 1;
                else
                    byvideotag = 0;

                if (_megasearchsetting.ByProductDescription)
                    bypdisc = 1;
                else
                    bypdisc = 0;

                if (_megasearchsetting.ManufacturerSearch)
                    manufacsearch = 1;
                else
                    manufacsearch = 0;

                if (_megasearchsetting.ByManufacturerDescription)
                    bymdisc = 1;
                else
                    bymdisc = 0;

                if (_megasearchsetting.CategorySearch)
                    categorysearch = 1;
                else
                    categorysearch = 0;

                if (_megasearchsetting.ByCategoryDescription)
                    bycdesc = 1;
                else
                    bycdesc = 0;

                if (_megasearchsetting.BlogPostSearch)
                    blogserch = 1;
                else
                    blogserch = 0;

                if (_megasearchsetting.ByBlogPostDescription)
                    bybsearch = 1;
                else
                    bybsearch = 0;

                if (_megasearchsetting.NewsSearch)
                    newssearch = 1;
                else
                    newssearch = 0;

                if (_megasearchsetting.ByNewsDescription)
                    byndesc = 1;
                else
                    byndesc = 0;


                var Keyword = _dataProvider.GetParameter();
                Keyword.ParameterName = "Keywords";
                Keyword.Value = model.KeywordText;
                Keyword.DbType = DbType.String;

                var Productsearch = _dataProvider.GetParameter();
                Productsearch.ParameterName = "ProductSearch";
                Productsearch.Value = productserch;
                Productsearch.DbType = DbType.Int32;

                var Produtbysku = _dataProvider.GetParameter();
                Produtbysku.ParameterName = "SearchSku";
                Produtbysku.Value = bysku;
                Produtbysku.DbType = DbType.Int32;

                var Productbypartno = _dataProvider.GetParameter();
                Productbypartno.ParameterName = "SearchPartNo";
                Productbypartno.Value = bypartno;
                Productbypartno.DbType = DbType.Int32;

                var Productbytag = _dataProvider.GetParameter();
                Productbytag.ParameterName = "SearchProductTags";
                Productbytag.Value = bytag;
                Productbytag.DbType = DbType.Int32;

                var ProductbyVideotag = _dataProvider.GetParameter();
                ProductbyVideotag.ParameterName = "SearchVideoTags";
                ProductbyVideotag.Value = byvideotag;
                ProductbyVideotag.DbType = DbType.Int32;

                var ProductDescript = _dataProvider.GetParameter();
                ProductDescript.ParameterName = "SearchDescriptions";
                ProductDescript.Value = bypdisc;
                ProductDescript.DbType = DbType.Int32;



                var Category = _dataProvider.GetParameter();
                Category.ParameterName = "CategorySearch";
                Category.Value = categorysearch;
                Category.DbType = DbType.Int32;

                var catDisc = _dataProvider.GetParameter();
                catDisc.ParameterName = "CategorySearchDescriptions";
                catDisc.Value = bycdesc;
                catDisc.DbType = DbType.Int32;



                var Manufacturers = _dataProvider.GetParameter();
                Manufacturers.ParameterName = "ManufacturerSearch";
                Manufacturers.Value = manufacsearch;
                Manufacturers.DbType = DbType.Int32;

                var ManfactureDesc = _dataProvider.GetParameter();
                ManfactureDesc.ParameterName = "ManufacturerSearchDescriptions";
                ManfactureDesc.Value = bymdisc;
                ManfactureDesc.DbType = DbType.Int32;



                var Blogsearch = _dataProvider.GetParameter();
                Blogsearch.ParameterName = "BlogpostSearch";
                Blogsearch.Value = blogserch;
                Blogsearch.DbType = DbType.Int32;

                var Blogdesc = _dataProvider.GetParameter();
                Blogdesc.ParameterName = "BlogpostSearchDescription";
                Blogdesc.Value = bybsearch;
                Blogdesc.DbType = DbType.Int32;



                var Newssearch = _dataProvider.GetParameter();
                Newssearch.ParameterName = "NewsSearch";
                Newssearch.Value = newssearch;
                Newssearch.DbType = DbType.Int32;

                var Newssearchdesc = _dataProvider.GetParameter();
                Newssearchdesc.ParameterName = "NewsSearchDescriptions";
                Newssearchdesc.Value = byndesc;
                Newssearchdesc.DbType = DbType.Int32;

                var useFullTSearchs = _dataProvider.GetParameter();
                useFullTSearchs.ParameterName = "UseFullTextSearch";
                useFullTSearchs.Value = usefullmode;
                useFullTSearchs.DbType = DbType.Int32;

                var LangIds = _dataProvider.GetParameter();
                LangIds.ParameterName = "LanguageId";
                LangIds.Value = fulltext;
                LangIds.DbType = DbType.Int32;

                var FullModes = _dataProvider.GetParameter();
                FullModes.ParameterName = "FullTextMode";
                FullModes.Value = langid;
                FullModes.DbType = DbType.Int32;

                var SearchResult = _megasearchcontext.SqlQuery<Td_MegaSearch>("EXEC Megasearch @Keywords, @ProductSearch,@SearchDescriptions ,@SearchSku,@SearchPartNo,@SearchProductTags,@SearchVideoTags ,@CategorySearch ,@CategorySearchDescriptions ,@ManufacturerSearch ,@ManufacturerSearchDescriptions ,@BlogpostSearch ,@BlogpostSearchDescription ,@NewsSearch ,@NewsSearchDescriptions ,@UseFullTextSearch,@LanguageId,@FullTextMode", Keyword, Productsearch, ProductDescript, Produtbysku, Productbypartno, Productbytag, ProductbyVideotag, Category, catDisc, Manufacturers, ManfactureDesc, Blogsearch, Blogdesc, Newssearch, Newssearchdesc, useFullTSearchs, LangIds, FullModes);

                if (SearchResult.Count() > 0)
                {
                    
                    int[] Productids = new int[SearchResult.Count()];
                    int[] Categoryids = new int[SearchResult.Count()];
                    int[] Manufactureids = new int[SearchResult.Count()];
                    int[] Blogids = new int[SearchResult.Count()];
                    int[] Newsids = new int[SearchResult.Count()];
                    int[] SupportCategoryIds = new int[SearchResult.Count()];
                    int[] SupportTopicIds = new int[SearchResult.Count()];
                    int[] SupportVideoIds = new int[SearchResult.Count()];
                    int[] SupportDownloadIds = new int[SearchResult.Count()];
                    int i = 0, j = 0, k = 0, l = 0, m = 0,n=0, o = 0 , p = 0 , q = 0;

                    foreach (var item in SearchResult)
                    {
                        if (item.Entitytype == 1)
                        {

                            Productids[i] = item.Entityid;
                            i++;
                        }
                        if (item.Entitytype == 2)
                        {
                            Categoryids[j] = item.Entityid;
                            j++;
                        }
                        if (item.Entitytype == 3)
                        {
                            Manufactureids[k] = item.Entityid;
                            k++;
                        }
                        if (item.Entitytype == 4)
                        {
                            Blogids[l] = item.Entityid;
                            l++;
                        }
                        if (item.Entitytype == 5)
                        {
                            Newsids[m] = item.Entityid;
                            m++;
                        }

                        //if (item.Entitytype == 6)
                        //{
                        //    SupportCategoryIds[n] = item.Entityid;
                        //    n++;
                        //}
                        if (item.Entitytype == 7)
                        {
                            SupportTopicIds[o] = item.Entityid;
                            o++;
                        }
                        if (item.Entitytype == 8)
                        {
                            SupportVideoIds[p] = item.Entityid;
                            p++;
                        }
                        if (item.Entitytype == 9)
                        {
                            SupportDownloadIds[q] = item.Entityid;
                            q++;
                        }

                    }

                    int[] result=Productids.Distinct().ToArray();

                   //SupportCategoryIds.Distinct();
                   //SupportTopicIds.Distinct(); 
                   //SupportVideoIds.Distinct();
                   //SupportDownloadIds.Distinct();

                    var products = _productService.GetProductsByIds(result).ToList();

                    var categorylist = _categoryService.GetAllCategories().Where(x => Categoryids.Contains(x.Id)).ToList();

                    var manufacturerlist = _manufacturerService.GetAllManufacturers().Where(x => Manufactureids.Contains(x.Id)).ToList();

                    var bloglist = _blogService.GetAllBlogPosts(0, 0, null, null, 0, int.MaxValue).Where(x => Blogids.Contains(x.Id)).ToList();

                    var newslist = _newsService.GetAllNews(0, 0, 0, int.MaxValue).Where(x => Newsids.Contains(x.Id)).ToList();

                    var pluginFinder = Nop.Core.Infrastructure.EngineContext.Current.Resolve<IPluginFinder>();

                    var pluginDescriptor = pluginFinder.GetPluginDescriptorBySystemName("TorchDesign_Support");

                    if (pluginDescriptor != null)
                    {
                        //var supportcategorylist = _supportService.GetAllSupportCategory().Where(x => SupportCategoryIds.Contains(x.Id)).ToList();

                        var topiclist = _supportService.GetAllSupportTopics().Where(x =>(SupportTopicIds.Distinct()).Contains(x.Id)).ToList();

                        var videolist = _supportService.GetAllSupportVideos().Where(x => (SupportVideoIds.Distinct()).Contains(x.Id)).ToList();

                        var downloadlist = _supportService.GetAllSupportDownloads().Where(x => (SupportDownloadIds.Distinct()).Contains(x.Id)).ToList();

                        //foreach (var Catitem in supportcategorylist)
                        //{
                        //    var supportcat = new SearchSupportCategoryList();
                        //    supportcat.SeName = Catitem.GetSeName();
                        //    supportcat.SupportCategoryId = Catitem.Id;
                        //    supportcat.SupportCategoryName = Catitem.Title;
                        //    supportcat.SupportCategoryPictureId = Catitem.PictureId;
                        //    supportcat.SupportCategoryPictureUrl = _pictureService.GetPictureUrl(Catitem.PictureId, 75);

                        //    model.AvailableSupportCategories.Add(supportcat);
                        //}
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

                    model.Products = PrepareProductOverviewModels(products).ToList();


                    int pictureSize = _mediaSettings.CategoryThumbPictureSize;
                    foreach (var Catitem in categorylist)
                    {
                        var mc = new CategoryModel();
                        mc.Id = Catitem.Id;
                        mc.Name = Catitem.Name;
                        mc.Description = Catitem.Description;
                        mc.PictureModel.ImageUrl = _pictureService.GetPictureUrl(Catitem.PictureId, pictureSize);
                        mc.PictureModel.Title = Catitem.Name;
                        mc.SeName = Catitem.GetSeName();
                        model.Category.Add(mc);
                    }
                    foreach (var manitem in manufacturerlist)
                    {
                        var mm = new ManufacturerModel();
                        mm.Id = manitem.Id;
                        mm.Name = manitem.Name;
                        mm.Description = manitem.Description;
                        mm.SeName = manitem.GetSeName();
                        mm.PictureModel.Title = manitem.Name;
                        mm.PictureModel.ImageUrl = _pictureService.GetPictureUrl(manitem.PictureId, pictureSize);
                        model.Manufacturer.Add(mm);
                    }
                    foreach (var blogitem in bloglist)
                    {
                        
                        var blog = _blogService.GetBlogPostById(blogitem.Id);
                        var mb = new BlogPostModel();
                        mb.Id = blogitem.Id;
                        mb.Title = blogitem.Title;
                        string string1 = Regex.Replace(blog.Body, @"(<img\/?[^>]+>)", @"", RegexOptions.IgnoreCase);
                        string string2 = Regex.Replace(string1, "<(.|\n)*?>", "");
                        string blogtext = Regex.Replace(string2, "\r\n", "");
                        string discriptionofblog;
                        if (blogtext.Length > 196)
                        {
                             discriptionofblog = (blogtext.Substring(0, 196)) + ("...");
                        }
                        else
                        {
                            discriptionofblog = blogtext;
                        }
                        mb.Body = discriptionofblog;
                        mb.SeName = blog.GetSeName(blog.LanguageId, ensureTwoPublishedLanguages: false);
                        model.Blog.Add(mb);
                    }
                    foreach (var newsitem in newslist)
                    {
                        var mn = new NewsItemModel();
                        mn.Id = newsitem.Id;
                        mn.Title = newsitem.Title;
                        mn.Short = newsitem.Short;
                        mn.SeName = newsitem.GetSeName();
                        model.News.Add(mn);
                    }
                    model.IsResultFound = true;
                }
                else
                {
                    model.IsResultFound = false;
                }

            }
            else 
            {
                model.IsTextEmpty = true;
                model.IsResultFound = false;
            }

          //  if (model.Viewmode == null)
          //  {

          //      model.Viewmode = "Grid";
          //      model.AvailableDisplayOption.Add(new SelectListItem() { Text = "Grid", Value = "Grid", Selected = true });
          //      model.AvailableDisplayOption.Add(new SelectListItem() { Text = "List", Value = "List" });

          //  }
          //  else if (model.Viewmode == "Grid")
          //  {
               
          //      model.AvailableDisplayOption.Add(new SelectListItem() { Text = "Grid", Value = "Grid" ,Selected=true});
          //      model.AvailableDisplayOption.Add(new SelectListItem() { Text = "List", Value = "List" });

          //  }
          //else
          //  {
          //      model.Viewmode = "List";
          //      model.AvailableDisplayOption.Add(new SelectListItem() { Text = "List", Value = "List", Selected = true });
          //      model.AvailableDisplayOption.Add(new SelectListItem() { Text = "Grid", Value = "Grid" });

          //  }
          
           // CatalogPagingFilteringModel command=new CatalogPagingFilteringModel();

            return View("~/Plugins/TorchDesign.MegaSearch/Views/TorchDesignMegaSearch/SearchPublicInfo.cshtml", model);
        }

        [NonAction]
        protected virtual IEnumerable<ProductOverviewModel> PrepareProductOverviewModels(IEnumerable<Product> products,
          bool preparePriceModel = true, bool preparePictureModel = true,
          int? productThumbPictureSize = null, bool prepareSpecificationAttributes = false,
          bool forceRedirectionAfterAddingToCart = false)
        {
            return this.PrepareProductOverviewModels(_workContext,
                _storeContext, _categoryService, _productService, _specificationAttributeService,
                _priceCalculationService, _priceFormatter, _permissionService,
                _localizationService, _taxService, _currencyService,
                _pictureService, _webHelper, _cacheManager,
                _catalogSettings, _mediaSettings, _orderReportService, _aclService, _storeMappingService, products,
                preparePriceModel, preparePictureModel,
                productThumbPictureSize, prepareSpecificationAttributes,
                forceRedirectionAfterAddingToCart);
        }


      
        #endregion
        #endregion

    }
}