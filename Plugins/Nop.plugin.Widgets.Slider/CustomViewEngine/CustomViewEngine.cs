using Nop.Web.Framework.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.Slider.CustomViewEngines
{
    public class CustomViewEngine : ThemeableRazorViewEngine
    {
        public CustomViewEngine()
        {
            ViewLocationFormats = new[]
                                      {
                                            //Plugin
                                            "~/Plugins/Widgets.Event/Themes/{2}/Views/{1}/{0}.cshtml",

                                            "~/Plugins/Widgets.Event/Views/{1}/{0}.cshtml"
                                          
                                      };


            PartialViewLocationFormats = new[]
                                             {
                                               //Plugin
                                               "~/Plugins/Widgets.Event/Themes/{2}/Views/{1}/{0}.cshtml",

                                            "~/Plugins/Widgets.Event/Views/{1}/{0}.cshtml"
                                             };
        }
    }
}
