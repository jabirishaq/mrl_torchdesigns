using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Nop.Web.Framework.Localization;
using System.Collections.Generic;

namespace Nop.Plugin.Widgets.TorchDesign_Support.Models
{
	public class PublicInfoModel : BaseNopEntityModel
	{
		public PublicInfoModel()
		{

            AvailableCategories = new List<CategoryList>();
            AvailableSupportCategories = new List<SearchSupportCategoryList>();
            AvailableSupportTopics = new List<SearchSupportTopicList>();
            AvailableSupportVideo = new List<SearchSupportVideo>();
            AvailableSupportDownload = new List<SearchSupportDownload>();
		}
        [NopResourceDisplayName("Plugins.TorchDesign.MegaSearch.SearchKeyword")]
        public string KeywordText { get; set; }
        public bool IsResultFound { get; set; }
        public bool IsTextEmpty { get; set; }
        public bool SystemDesignEnabled { get; set; }

        public IList<CategoryList> AvailableCategories { get; set; }
        public IList<SearchSupportCategoryList> AvailableSupportCategories { get; set; }
        public IList<SearchSupportTopicList> AvailableSupportTopics { get; set; }
        public IList<SearchSupportVideo> AvailableSupportVideo { get; set; }
        public IList<SearchSupportDownload> AvailableSupportDownload { get; set; }
	}
    public class CategoryList : BaseNopModel
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public int PictureId { get; set; }
        public string PictureUrl { get; set; }
        public string SeoName { get; set; }

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
