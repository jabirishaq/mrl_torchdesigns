﻿
using Nop.Core.Configuration;

namespace Nop.Plugin.TorchDesign.Testimonials
{
    public class Testimonialsettings : ISettings
    {
        public string Title { get; set; }
     
        //public int DisplayOption { get; set; }

        public string DisplayOn { get; set; }

        public bool DisplayAsWidget { get; set; }

    
    }
}