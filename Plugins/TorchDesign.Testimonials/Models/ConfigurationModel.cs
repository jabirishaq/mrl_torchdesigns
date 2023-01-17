using System;
using Nop.Web.Framework;
using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Localization;
using System.Web.Mvc;
using System.Collections.Generic;

namespace Nop.Plugin.TorchDesign.Testimonials.Models
{
    public partial class ConfigurationModel : BaseNopEntityModel, ILocalizedModel<TestimonialsLangLocalizedModel>
    {
        public ConfigurationModel()
        {
            Locales = new List<TestimonialsLangLocalizedModel>();
            //AvailableOptions = new List<SelectListItem>();
            AvailableZones = new List<SelectListItem>();
            //AvailableCategories = new List<SelectListItem>();
           
        }

      
        //[NopResourceDisplayName("Plugins.Widgets.Testimonials.Category")]
        //public int CategoryId { get; set; }
        //public IList<SelectListItem> AvailableCategories { get; set; }

        public bool IsAddMode { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.Testimonials.Category")]
        [AllowHtml]
        public string CategoryName { get; set; }
     
        [NopResourceDisplayName("Plugins.Widgets.Testimonials.Title")]
        [AllowHtml]
      
        public string Title { get; set; }
        public bool Title_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.Testimonials.Question")]
        [AllowHtml]
        [Required]
        public string question { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.Testimonials.Description")]
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
        //[NopResourceDisplayName("Plugins.Widgets.Testimonials.DisplayOption")]
        //[AllowHtml]
        //public int DisplayOption { get; set; }
        //public bool DisplayOption_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.Testimonials.DisplayAsWidget")]
        [AllowHtml]
        public bool DisplayAsWidget { get; set; }
        public bool DisplayAsWidget_OverrideForStore { get; set; }

        public IList<SelectListItem> AvailableZones { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.Testimonials.DisplayOn")]
        [AllowHtml]
        public string DisplayOn { get; set; }
        public bool DisplayOn_OverrideForStore { get; set; }

       

        public IList<TestimonialsLangLocalizedModel> Locales { get; set; }

        public partial class TestimonialsCategoryMappingModel : BaseNopEntityModel
        {
            [NopResourceDisplayName("Admin.Catalog.Products.Categories.Fields.Category")]
            public string Category { get; set; }

            public int TestimonialsId { get; set; }

            public int CategoryId { get; set; }

            [NopResourceDisplayName("Admin.Catalog.Products.Categories.Fields.DisplayOrder")]
            public int DisplayOrder { get; set; }
        }
    }

    public partial class TestimonialsLangLocalizedModel : ILocalizedModelLocal
    {

        public int LanguageId { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.Testimonials.Title")]
        [AllowHtml]
        public string Title { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.Testimonials.Category")]
        [AllowHtml]
        public string category { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.Testimonials.Question")]
        [AllowHtml]
        public string question { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.Testimonials.Description")]
        [AllowHtml]
        public string description { get; set; }

        //[NopResourceDisplayName("Plugins.Widgets.Testimonials.DisplayOption")]
        //[AllowHtml]
        //public int DisplayOption { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.Testimonials.DisplayOn")]
        [AllowHtml]
        public string DisplayOn { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.Testimonials.DisplayAsWidget")]
        [AllowHtml]
        public bool DisplayAsWidget { get; set; }



      
    }
}

