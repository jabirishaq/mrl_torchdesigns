using System.Web.Mvc;
using System.Web.Routing;
using Nop.Web.Framework.Mvc.Routes;

namespace Nop.Plugin.Shipping.ByWeight
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {

            
                routes.MapRoute("Plugin.Widgets.FAQ",
                 "faq",
                 new { controller = "FrequentlyAskedQ", action = "FAQPage" },
                 new[] { "Nop.Plugin.TorchDesign.FAQ.Controllers" }
            );


           

            routes.MapRoute("Plugin.Widgets.FAQ.AddPopup",
                 "Plugins/FrequentlyAskedQ/AddPopup",
                 new { controller = "FrequentlyAskedQ", action = "AddPopup" },
                 new[] { "Nop.Plugin.TorchDesign.FAQ.Controllers" }
            );

            routes.MapRoute("Plugin.Widgets.FAQ.EditPopup",
                 "Plugins/FrequentlyAskedQ/EditPopup",
                 new { controller = "FrequentlyAskedQ", action = "EditPopup" },
                 new[] { "Nop.Plugin.TorchDesign.FAQ.Controllers" }
            );

            routes.MapRoute("Plugin.Widgets.FAQ.CategoryList",
                "Plugins/FrequentlyAskedQ/AddCategoryPopup",
                new { controller = "FrequentlyAskedQ", action = "CategoryList" },
                new[] { "Nop.Plugin.TorchDesign.FAQ.Controllers" }
           );


        }

        
        public int Priority
        {
            get
            {
                return 0;
            }
        }
    }
}
