using System.Web.Mvc;
using System.Web.Routing;
using Nop.Web.Framework.Mvc.Routes;

namespace Nop.Plugin.Shipping.ByWeight
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {

            routes.MapRoute("TorchDesignZip2TaxCounter",
                "Plugins/TorchDesignZip2Tax/Counter",
                new { controller = "TorchDesignZip2Tax", action = "Counter" },
                new[] { "Nop.Plugin.TorchDesign.Zip2Tax.Controllers" }
           );


            //routes.MapRoute("Plugin.TorchDesign.Zip2Tax.AddPopup",
            //     "Plugins/TorchDesignZip2Tax/AddPopup",
            //     new { controller = "TorchDesignZip2Tax", action = "AddPopup" },
            //     new[] { "Nop.Plugin.TorchDesign.Zip2Tax.Controllers" }
            //);

            //routes.MapRoute("Plugin.TorchDesign.Zip2Tax.EditPopup",
            //     "Plugins/TorchDesignZip2Tax/EditPopup",
            //     new { controller = "TorchDesignZip2Tax", action = "EditPopup" },
            //     new[] { "Nop.Plugin.TorchDesign.Zip2Tax.Controllers" }
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
