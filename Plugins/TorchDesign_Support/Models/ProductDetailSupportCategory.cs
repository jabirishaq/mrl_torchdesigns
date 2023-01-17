using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Localization;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Widgets.TorchDesign_Support.Models
{
	 //[Validator(typeof(TopicValidator))]
    public  class ProductCategorySupportCategory: BaseNopEntityModel
    {
        public ProductCategorySupportCategory()
        {
			  AvailableSupportCategories = new List<SupportCategoryList>();
              AvailableSupportVideo = new List<SupportVideoList>();
              AvailableSupportDownload = new List<SupportDownloadList>();
        }
       
        public int ProductCategoryId { get; set; }
        public int ProductCategoryPictureId { get; set; }
        public string ProductCategoryPictureUrl { get; set; }
        public string SeoName { get; set; }
        public string ProductCategoryName { get; set; }
        public bool AvailbleVideo { get; set; }
        public bool AvailbleDownload{ get; set; }
        public bool SystemDesignEnabled { get; set; }
        public IList<SupportCategoryList> AvailableSupportCategories { get; set; }
        public IList<SupportVideoList> AvailableSupportVideo { get; set; }
        public IList<SupportDownloadList> AvailableSupportDownload { get; set; }
    }

    public class SupportCategoryList : BaseNopModel
    {
        public bool IsTopicAvailable { get; set; }
        public string SeName { get; set; }
			  public int SupportCategoryId { get; set; }
			  public string SupportCategoryName { get; set; }
              public int SupportCategoryPictureId { get; set; }
              public string SupportCategoryPictureUrl { get; set; }
		  }
    public class SupportVideoList : BaseNopModel
    {
        public int SupportVideoId { get; set; }
        public string Title { get; set; }

        public int PictureId { get; set; }

        public string VimeoInformation { get; set; }
        public string PictureUrl { get; set; }

    }
    public class SupportDownloadList : BaseNopModel
    {

        public int Id { get; set; }

        public string Title { get; set; }

        public int DownloadId { get; set; }


    }
		 
    }


