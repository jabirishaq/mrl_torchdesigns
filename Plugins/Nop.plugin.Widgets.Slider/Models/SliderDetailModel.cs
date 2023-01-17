using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Web.Framework.Localization;
using System.Collections.Generic;

namespace Nop.Plugin.Widgets.Slider.Models
{
    public class SliderDetailModel : BaseNopEntityModel 
    {
        public SliderDetailModel()
        {
            Slider = new List<Slider>();
        }
      //  public int TransitionSpeed { get; set; } For Global Trasition Spped

        public IList<Slider> Slider { get; set; }
      
   }
    public class Slider : BaseNopEntityModel
    {
        public string Title { get; set; }

        public string ShortDescription { get; set; }

        public string PictureUrl { get; set; }

        public string link { get; set; }

        public string SliderEffect { get; set; }

        public string SlidePushtime { get; set; }
    }
}
