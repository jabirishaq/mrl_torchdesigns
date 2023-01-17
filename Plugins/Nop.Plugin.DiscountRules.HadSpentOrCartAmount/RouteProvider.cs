using System.Web.Mvc;
using System.Web.Routing;
using Nop.Web.Framework.Mvc.Routes;

namespace Nop.Plugin.DiscountRules.HadSpentOrCartAmount
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("Plugin.DiscountRules.HadSpentOrCartAmount.Configure",
                 "Plugins/DiscountRulesHadSpentOrCartAmount/Configure",
                 new { controller = "DiscountRulesHadSpentOrCartAmount", action = "Configure" },
                 new[] { "Nop.Plugin.DiscountRules.HadSpentOrCartAmount.Controllers" }
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
