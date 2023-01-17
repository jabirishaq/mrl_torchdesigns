using System.Web.Mvc;
using System.Web.Routing;
using Nop.Web.Framework.Mvc.Routes;

namespace Nop.Plugin.TorchDesign.PayflowPro
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            ////IPN
            //routes.MapRoute("Plugin.Payments.PayPalDirect.IPNHandler",
            //     "Plugins/PaymentPayPalDirect/IPNHandler",
            //     new { controller = "PaymentPayPalDirect", action = "IPNHandler" },
            //     new[] { "Nop.Plugin.TorchDesign.PayflowPro.Controllers" }
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
