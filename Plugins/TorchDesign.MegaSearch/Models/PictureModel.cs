using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.TorchDesign.MegaSearch.Models
{
    public partial class PictureModel : BaseNopModel
    {
        public string ImageUrl { get; set; }

        public string FullSizeImageUrl { get; set; }

        public string Title { get; set; }

        public string AlternateText { get; set; }
        public string DefaultPictureImageUrl { get; set; }
    }
}