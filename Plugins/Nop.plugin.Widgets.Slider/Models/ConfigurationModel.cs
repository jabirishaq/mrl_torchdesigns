using System;
using Nop.Web.Framework;
using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Localization;
using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Nop.Plugin.Widgets.Slider.Models
{
    public partial class ConfigurationModel : BaseNopEntityModel, ILocalizedModel<SliderLangLocalizedModel>
    {
        public ConfigurationModel()
        {
            Locales = new List<SliderLangLocalizedModel>();
            AvailableDisplayOption = new List<SelectListItem>();
            AvailableSliderEffect = new List<SelectListItem>();
            //this.Errors = new List<string>();
        }
        [NopResourceDisplayName("Plugins.Widgets.Slider.Title")]
        [StringLength(40, ErrorMessage = "Maximum 40 characters allowed")]
        [AllowHtml]
        public string Title { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.Slider.ShortDescription")]
        [StringLength(200, ErrorMessage = "Maximum 200 characters allowed")]
        [AllowHtml]
        public string ShortDescription { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.Slider.Link")]
        [AllowHtml]
        public string link { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.Slider.Displayorder")]
        [AllowHtml]
        public int DisplayOrder { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.Slider.Displayoption")]
        [AllowHtml]
        public int DisplayOptionid { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.Slider.transitionspeed")]
        [AllowHtml]
        public int SlidePushtime { get; set; }

        //-----------------For Global Trasition Spped
        //[NopResourceDisplayName("Plugins.Widgets.Slider.transitionspeed")]
        //[AllowHtml]
        //public int transitionSpeed { get; set; }
        //public bool transitionSpeed_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.Slider.Effect")]
        [AllowHtml]
        public int Slidereffect { get; set; }
        //public bool Slidereffect_OverrideForStore { get; set; }

        public IEnumerable<SelectListItem> AvailableDisplayOption { get; set; }
        public IEnumerable<SelectListItem> AvailableSliderEffect { get; set; }      
        //public IList<SelectListItem> AvailableDisplayOption { get; set; }
       

        [NopResourceDisplayName("Plugins.Widgets.Slider.PictureId")]
        [UIHint("Picture")]
        public int PictureId { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.Slider.PictureId")]
        [AllowHtml]
        public string PictureUrl { get; set; }

        [AllowHtml]
        [NopResourceDisplayName("Plugins.Widgets.Slider.Published")]
        public bool Published { get; set; }
       
        public IList<SliderLangLocalizedModel> Locales { get; set; }

        //public IList<string> Errors { get; set; }
        
        //System.Collections.Generic.IList<SliderLocalizedModel> ILocalizedModel<SliderLocalizedModel>.Locales
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }
        //    set
        //    {
        //        throw new NotImplementedException();
        //    }
        //}
    }

    public partial class SliderLangLocalizedModel : ILocalizedModelLocal
    {
        public int LanguageId { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.Slider.Title")]
        [AllowHtml]
        public string Title { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.Slider.ShortDescription")]
        [AllowHtml]
        public string ShortDescription { get; set; }
      


    }
}

