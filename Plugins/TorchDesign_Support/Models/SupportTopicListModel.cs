using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Widgets.TorchDesign_Support.Models
{
    public partial class SupportTopicListModel : BaseNopModel
    {
        public SupportTopicListModel()
        {
            AvailableSupportTopic = new List<SupportTopicContainer>();
            AvailableSupportVideo = new List<SupportVideoContainer>();
            AvailableSupportDownaload = new List<SupportDownloadContainer>();
        }



        public int SupportCategoryId { get; set; }
        public int SupportCategoryPictureId { get; set; }
        public string SupportCategoryPictureUrl { get; set; }
        public string SupportCategoryName { get; set; }
        public bool SystemDesignEnabled { get; set; }

        public IList<SupportTopicContainer> AvailableSupportTopic { get; set; }
        public IList<SupportVideoContainer> AvailableSupportVideo { get; set; }
        public IList<SupportDownloadContainer> AvailableSupportDownaload { get; set; }

    }
    public class SupportTopicContainer : BaseNopModel
    {
        public int SupportTopicId { get; set; }
        public string Sename { get; set; }
        public string SupportTopicName { get; set; }

    }

    public class SupportVideoContainer : BaseNopModel
    {
        public int SupportVideoId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int PictureId { get; set; }
        public string PictureUrl { get; set; }
        public int DisplayOrder { get; set; }
        public string VimeoInformation { get; set; }

    }
    public class SupportDownloadContainer : BaseNopModel
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public int DownloadId { get; set; }


    }



}