using System;
using Nop.Web.Framework;
using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Localization;
using System.Web.Mvc;
using System.Collections.Generic;

namespace Nop.Plugin.TorchDesign.FAQ.Models
{
    public partial class ConfigurationModel : BaseNopEntityModel, ILocalizedModel<FAQLangLocalizedModel>
    {
        public ConfigurationModel()
        {
            Locales = new List<FAQLangLocalizedModel>();
            //AvailableOptions = new List<SelectListItem>();
            AvailableZones = new List<SelectListItem>();
            //AvailableCategories = new List<SelectListItem>();
           
        }

      
        //[NopResourceDisplayName("Plugins.Widgets.FAQ.Category")]
        //public int CategoryId { get; set; }
        //public IList<SelectListItem> AvailableCategories { get; set; }

        public bool IsAddMode { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.FAQ.Category")]
        [AllowHtml]
        public string CategoryName { get; set; }
     
        [NopResourceDisplayName("Plugins.Widgets.FAQ.Title")]
        [AllowHtml]
      
        public string Title { get; set; }
        public bool Title_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.FAQ.Question")]
        [AllowHtml]
        [Required]
        public string question { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.FAQ.Description")]
        [AllowHtml]
        public string description { get; set; }

        //public IList<SelectListItem> AvailableOptions { get; set; }
        [NopResourceDisplayName("Plugins.TorchDesign.Customerorigin.isactive")]
        [AllowHtml]
        public bool IsActive { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.Event.DisplayOrder")]
        [AllowHtml]
        public int DisplayOrder { get; set; }

        //public IList<SelectListItem> AvailableOptions { get; set; }
        //[NopResourceDisplayName("Plugins.Widgets.FAQ.DisplayOption")]
        //[AllowHtml]
        //public int DisplayOption { get; set; }
        //public bool DisplayOption_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.FAQ.DisplayAsWidget")]
        [AllowHtml]
        public bool DisplayAsWidget { get; set; }
        public bool DisplayAsWidget_OverrideForStore { get; set; }

        public IList<SelectListItem> AvailableZones { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.FAQ.DisplayOn")]
        [AllowHtml]
        public string DisplayOn { get; set; }
        public bool DisplayOn_OverrideForStore { get; set; }

       

        public IList<FAQLangLocalizedModel> Locales { get; set; }

        public partial class FaqCategoryMappingModel : BaseNopEntityModel
        {
            [NopResourceDisplayName("Admin.Catalog.Products.Categories.Fields.Category")]
            public string Category { get; set; }

            public int FaqId { get; set; }

            public int CategoryId { get; set; }

            [NopResourceDisplayName("Admin.Catalog.Products.Categories.Fields.DisplayOrder")]
            public int DisplayOrder { get; set; }
        }
    }

    public partial class FAQLangLocalizedModel : ILocalizedModelLocal
    {

        public int LanguageId { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.FAQ.Title")]
        [AllowHtml]
        public string Title { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.FAQ.Category")]
        [AllowHtml]
        public string category { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.FAQ.Question")]
        [AllowHtml]
        public string question { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.FAQ.Description")]
        [AllowHtml]
        public string description { get; set; }

        //[NopResourceDisplayName("Plugins.Widgets.FAQ.DisplayOption")]
        //[AllowHtml]
        //public int DisplayOption { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.FAQ.DisplayOn")]
        [AllowHtml]
        public string DisplayOn { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.FAQ.DisplayAsWidget")]
        [AllowHtml]
        public bool DisplayAsWidget { get; set; }



      
    }
}

