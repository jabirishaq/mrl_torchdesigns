using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Localization;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Widgets.TorchDesign_Support.Models
{
    //[Validator(typeof(TopicValidator))]
    public class ProductDetailSupportCategory : BaseNopEntityModel
    {
        public ProductDetailSupportCategory()
        {
            AvailableSupportCategories = new List<ProductDetailSupportCategoryList>();
            AvailableSupportVideo = new List<ProductDetailSupportVideo>();
            AvailableSupportDownload = new List<ProductDetailSupportDownload>();
            AvailableSupportTopics = new List<ProductDetailSupportTopicListModel>();
        }

        public int ProductCategoryId { get; set; }
        public int ProductCategoryPictureId { get; set; }
        public string ProductCategoryPictureUrl { get; set; }
        public string ProductCategoryName { get; set; }

        public IList<ProductDetailSupportCategoryList> AvailableSupportCategories { get; set; }
        public IList<ProductDetailSupportVideo> AvailableSupportVideo { get; set; }
        public IList<ProductDetailSupportDownload> AvailableSupportDownload { get; set; }
        public IList<ProductDetailSupportTopicListModel> AvailableSupportTopics { get; set; }
    }

    public class ProductDetailSupportCategoryList : BaseNopModel
    {

       
        public bool IsSupportTopicAvalable { get; set; }
        public int SupportCategoryId { get; set; }
        public string SupportCategoryName { get; set; }
        public int SupportCategoryPictureId { get; set; }
        public string SupportCategoryPictureUrl { get; set; }

      

    }
    public class ProductDetailSupportTopicListModel : BaseNopModel
    {
        public string Sename { get; set; }
        public int SupportTopicId { get; set; }
        public string SupportTopicName { get; set; }
        public int[] SupportCategoryId { get; set; }

    }
    public class ProductDetailSupportVideo : BaseNopModel
    {
        public int SupportVideoId { get; set; }
        public string Title { get; set; }

        public int PictureId { get; set; }

        public string VimeoInformation { get; set; }
        public string PictureUrl { get; set; }

    }
    public class ProductDetailSupportDownload : BaseNopModel
    {

        public int Id { get; set; }

        public string Title { get; set; }

        public int DownloadId { get; set; }


    }
}


