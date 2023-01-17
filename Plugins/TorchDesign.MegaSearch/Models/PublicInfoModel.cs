using System;
using Nop.Web.Framework;
using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Mvc;
using System.Collections.Generic;
using Nop.Web.Framework.Localization;
using System.Web.Mvc;
using Nop.Plugin.TorchDesign.MegaSearch.Models;



namespace Nop.Plugin.TorchDesign.MegaSearch.Models
{
    public partial class PublicInfoModel : BaseNopModel
    {
        public PublicInfoModel()
        {
            this.Products = new List<ProductOverviewModel>();
            this.Category = new List<CategoryModel>();
            // AvailableDisplayOption = new List<SelectListItem>();
            this.Manufacturer = new List<ManufacturerModel>();
            this.Blog = new List<BlogPostModel>();
            this.News = new List<NewsItemModel>();
            AvailableSupportCategories = new List<SearchSupportCategoryList>();
            AvailableSupportTopics = new List<SearchSupportTopicList>();
            AvailableSupportVideo = new List<SearchSupportVideo>();
            AvailableSupportDownload = new List<SearchSupportDownload>();

            //      PagingFilteringContext = new CatalogPagingFilteringModel();
        }
        public IList<SearchSupportCategoryList> AvailableSupportCategories { get; set; }
        public IList<SearchSupportTopicList> AvailableSupportTopics { get; set; }
        public IList<SearchSupportVideo> AvailableSupportVideo { get; set; }
        public IList<SearchSupportDownload> AvailableSupportDownload { get; set; }
        public bool NoResults { get; set; }

        [NopResourceDisplayName("Plugins.TorchDesign.MegaSearch.SearchKeyword")]
        public string KeywordText { get; set; }
        public bool IsResultFound { get; set; }
        public bool IsTextEmpty { get; set; }

        public IList<ProductOverviewModel> Products { get; set; }
        public IList<CategoryModel> Category { get; set; }
        public IList<ManufacturerModel> Manufacturer { get; set; }
        public IList<BlogPostModel> Blog { get; set; }
        public IList<NewsItemModel> News { get; set; }
        // public string Viewmode { get; set; }
        //  public CatalogPagingFilteringModel PagingFilteringContext { get; set; }
        // public IList<SelectListItem> AvailableDisplayOption { get; set; }
        //public string DisplayoptionId { get; set; }
        //#region Nested classes

        //public class CategoryModel : BaseNopEntityModel
        //{
        //    public string Breadcrumb { get; set; }
        //}

        //#endregion


    }
    public class SearchSupportCategoryList : BaseNopModel
    {
        public string SeName { get; set; }
        public int SupportCategoryId { get; set; }
        public string SupportCategoryName { get; set; }
        public int SupportCategoryPictureId { get; set; }
        public string SupportCategoryPictureUrl { get; set; }
    }
    public class SearchSupportTopicList : BaseNopModel
    {
        public string SeoName { get; set; }
        public int SupportTopicId { get; set; }
        public string SupportTopicTitle { get; set; }

    }
    public class SearchSupportVideo : BaseNopModel
    {
        public int SupportVideoId { get; set; }
        public string Title { get; set; }

        public int PictureId { get; set; }

        public string VimeoInformation { get; set; }
        public string PictureUrl { get; set; }

    }
    public class SearchSupportDownload : BaseNopModel
    {

        public int Id { get; set; }

        public string Title { get; set; }

        public int DownloadId { get; set; }


    }


}

