using System;
using Nop.Web.Framework;
using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Localization;
using System.Web.Mvc;
using System.Collections.Generic;

namespace Nop.Plugin.TorchDesign.MegaSearch.Models
{
    public partial class ConfigurationModel : BaseNopEntityModel
    {
        //public ConfigurationModel()
        //{
        //    Locales = new List<FAQLangLocalizedModel>();
        //    //AvailableOptions = new List<SelectListItem>();
        //    AvailableZones = new List<SelectListItem>();
        //    AvailableCategories = new List<SelectListItem>();

        //}
             
        
        [NopResourceDisplayName("Plugins.TorchDesign.MegaSearch.SearchFromProduct")]
        [AllowHtml]
        public bool ProductSearch { get; set; }
        public bool ProductSearch_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.TorchDesign.MegaSearch.SearchProductbydescription")]
        [AllowHtml]
        public bool ByProductDescription { get; set; }
        public bool ByProductDescription_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.TorchDesign.MegaSearch.SearchProductbysku")]
        [AllowHtml]
        public bool ByProductSku { get; set; }
        public bool ByProductSku_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.TorchDesign.MegaSearch.SearchProductbyproductnumber")]
        [AllowHtml]
        public bool ByProductPartNo { get; set; }
        public bool ByProductPartNo_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.TorchDesign.MegaSearch.SearchProductbytag")]
        [AllowHtml]
        public bool ByProductTag { get; set; }
        public bool ByProductTag_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.TorchDesign.MegaSearch.SearchProductVideobytag")]
        [AllowHtml]
        public bool ByVideoTag { get; set; }
        public bool ByVideoTag_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.TorchDesign.MegaSearch.SearchFromCategory")]
        [AllowHtml]
        public bool CategorySearch { get; set; }
        public bool CategorySearch_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.TorchDesign.MegaSearch.SearchCategorybydescription")]
        [AllowHtml]
        public bool ByCategoryDescription { get; set; }
        public bool ByCategoryDescription_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.TorchDesign.MegaSearch.SearchFromManufacturer")]
        [AllowHtml]
        public bool ManufacturerSearch { get; set; }
        public bool ManufacturerSearch_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.TorchDesign.MegaSearch.SearchManufacturerbydescription")]
        [AllowHtml]
        public bool ByManufacturerDescription { get; set; }
        public bool ByManufacturerDescription_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.TorchDesign.MegaSearch.SearchFromBlogPostSearch")]
        [AllowHtml]
        public bool BlogPostSearch { get; set; }
        public bool BlogPostSearch_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.TorchDesign.MegaSearch.SearchBlogPostSearchbydescription")]
        [AllowHtml]
        public bool ByBlogPostDescription { get; set; }
        public bool ByBlogPostDescription_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.TorchDesign.MegaSearch.SearchFromNewsSearch")]
        [AllowHtml]
        public bool NewsSearch { get; set; }
        public bool NewsSearch_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.TorchDesign.MegaSearch.SearchNewsSearchbydescription")]
        [AllowHtml]
        public bool ByNewsDescription { get; set; }
        public bool ByNewsDescription_OverrideForStore { get; set; }


        //public IList<SelectListItem> AvailableZones { get; set; }
        //[NopResourceDisplayName("Plugins.Widgets.FAQ.DisplayOn")]
        //[AllowHtml]
        //public string DisplayOn { get; set; }
        //public bool DisplayOn_OverrideForStore { get; set; }



        //public IList<FAQLangLocalizedModel> Locales { get; set; }

    }

    
}

