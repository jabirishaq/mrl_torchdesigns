
using Nop.Core.Configuration;

namespace Nop.Plugin.TorchDesign.MegaSearch
{
    public class TorchDesignMegaSearchSettings : ISettings
    {

        public bool ProductSearch { get; set; }

        public bool ByProductDescription { get; set; }

        public bool ByProductSku { get; set; }

        public bool ByProductPartno { get; set; }

        public bool ByProductTag { get; set; }

        public bool ByVideoTag { get; set; }

        public bool CategorySearch { get; set; }

        public bool ByCategoryDescription { get; set; }

        public bool ManufacturerSearch { get; set; }

        public bool ByManufacturerDescription { get; set; }

        public bool BlogPostSearch { get; set; }

        public bool ByBlogPostDescription { get; set; }

        public bool NewsSearch { get; set; }

        public bool ByNewsDescription { get; set; }


    }
}