using System;
using Nop.Web.Framework;
using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Widgets.Slider.Models
{
    public class SliderListModel : BaseNopEntityModel
    {
        [NopResourceDisplayName("Plugins.Widgets.Event.Title")]
        public string Title { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.Event.ShortDescription")]
        public string ShortDescription { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.Event.Link")]
        public string link { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.Event.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.Event.DisplayOption")]
        public string DisplayOption { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.Slider.transitionspeed")]
        public int SlidePushtime { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.Slider.Effect")]
        public string Slidingeffect { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.Event.PictureId")]
        [UIHint("Picture")]
        public int PictureId { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.Event.PictureId")]
        public string PictureUrl { get; set; }
                
        [NopResourceDisplayName("Plugins.Widgets.Event.Published")]
        public bool Published { get; set; }


    }
}
