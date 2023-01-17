using System.Web.Mvc;
using System.Web.Routing;
using Nop.Web.Framework.Mvc.Routes;

namespace Nop.Plugin.TorchDesigns.DynamicsGPWCF
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("TestConnection",
                 "Plugins/DynamicGpWCF/TestConnection",
                 new { controller = "TorchDesigns_DynamicsGPWCFController", action = "TestConnection", },
                 new[] { "Nop.Plugin.TorchDesigns.DynamicsGPWCF.Controllers" }
            );

           // routes.MapRoute("Configure",
           //     "Plugins/DynamicGp/Configure",
           //     new { controller = "TorchDesigns_DynamicsGPController", action = "Configure", },
           //     new[] { "Nop.Plugin.TorchDesigns.DynamicsGP.Controllers" }
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
