using System.Web.Mvc;
using System.Web.Routing;
using Nop.Web.Framework.Mvc.Routes;
using Nop.Plugin.Widgets.Slider.CustomViewEngines;

namespace Nop.Plugin.Widgets.Slider
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            ViewEngines.Engines.Add(new CustomViewEngine());
            routes.MapRoute("Slider",
                         "manageslider/",
                         new { controller = "WidgetsSlider", action = "ManageSlider" },
                         new[] { "Nop.Plugin.Widgets.Slider.Controllers" });

            routes.MapRoute("SliderDetail",
                         "slider/detail/{sliderid}",
                         new { controller = "Slider", action = "Detail" },
                         new { sliderid = @"\d+" },
                         new[] { "Nop.Plugin.Widgets.Slider.Controllers" });

            routes.MapRoute("Plugin.Widgets.Slider.Configure",
                "Plugins/WidgetsSlider/Configure",
                new { controller = "WidgetsSlider", action = "Configure"},
                        new[] { "Nop.Plugin.Widgets.Slider.Controllers" });

            //routes.MapRoute("Plugin.Widgets.Slider.MyConfigure",
            //     "Plugins/WidgetsSlider/MyConfigure",
            //     new { controller = "WidgetsSlider", action = "MyConfigure" },
            //             new[] { "Nop.Plugin.Widgets.Slider.Controllers" });


            routes.MapRoute("Plugin.Widgets.Slider.AddPopup",
                 "Plugins/WidgetsSlider/AddPopup",
                 new { controller = "WidgetsSlider", action = "AddPopup" },
                         new[] { "Nop.Plugin.Widgets.Slider.Controllers" });


            routes.MapRoute("Plugin.Widgets.Slider.EditPopup",
                 "Plugins/WidgetsSlider/EditPopup",
                 new { controller = "WidgetsSlider", action = "EditPopup" },
                 new[] { "Nop.Plugin.Widgets.Slider.Controllers" });

            routes.MapRoute("AllEventDetail",
                          "slider/allsliderdetail/",
                          new { controller = "WidgetsSlider", action = "AllSliderDetail" },
                         
                          new[] { "Nop.Plugin.Widgets.Slider.Controllers" });
           
        }
        public int Priority
        {
            get
            {
                return 100;
            }
        }
    }
}
