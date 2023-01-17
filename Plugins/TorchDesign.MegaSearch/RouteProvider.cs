using System.Web.Mvc;
using System.Web.Routing;
using Nop.Web.Framework.Mvc.Routes;

namespace Nop.Plugin.Shipping.ByWeight
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {

            routes.MapRoute("TorchDesignMegaSearch",
                "Plugins/TorchDesignMegaSearch/Configure",
                new { controller = "TorchDesignMegaSearchController", action = "Configure" },
                new[] { "Nop.Plugin.TorchDesign.MegaSearch.Controllers" }
           );

            routes.MapRoute("TorchDesignMegaSearch.SearchPublicInfo",
                "Plugins/TorchDesignMegaSearch/SearchPublicInfo",
                new { controller = "TorchDesignMegaSearchController", action = "SearchPublicInfo" },
                new[] { "Nop.Plugin.TorchDesign.MegaSearch.Controllers" }
           );



            //routes.MapRoute("Plugin.TorchDesign.MegaSearch.AddPopup",
            //     "Plugins/TorchDesignZip2Tax/AddPopup",
            //     new { controller = "TorchDesignZip2Tax", action = "AddPopup" },
            //     new[] { "Nop.Plugin.TorchDesign.MegaSearch.Controllers" }
            //);

            //routes.MapRoute("Plugin.TorchDesign.MegaSearch.EditPopup",
            //     "Plugins/TorchDesignZip2Tax/EditPopup",
            //     new { controller = "TorchDesignZip2Tax", action = "EditPopup" },
            //     new[] { "Nop.Plugin.TorchDesign.MegaSearch.Controllers" }
            //);


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
