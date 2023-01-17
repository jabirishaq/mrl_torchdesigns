using System.Web.Mvc;
using System.Web.Routing;
using Nop.Web.Framework.Mvc.Routes;

namespace Nop.Plugin.Shipping.ByWeight
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {

           // routes.MapRoute("TorchDesignCustconfigure",
           //     "Plugins/TorchDesignCustomerOrigin/Configure",
           //     new { controller = "TorchDesignCustomerOrigin", action = "Configure" },
           //     new[] { "Nop.Plugin.TorchDesign.CustomerOrigin.Controllers" }
           //);


            //routes.MapRoute("TorchDesignCustomerOriginconfigure",
            //"MarkAsPrimarySelected/{optionName}",
            // new { controller = "TorchDesignCustomerOrigin", action = "MarkAsPrimarySelected" },
            //   new { imageid = @"\d+" },
            // new[] { "Nop.Plugin.TorchDesign.CustomerOrigin.Controllers" });
            routes.MapRoute("MiscCheckoutPaymentInfo",
                          "checkout/paymentinfo",
                          new { controller = "Checkout", action = "PaymentInfo" },
                          new[] { "Nop.Web.Controllers" });


            routes.MapRoute("TorchDesignCustomerOriginChart",
             "Plugins/TorchDesignCustomerOrigin/Chart",
             new { controller = "TorchDesignCustomerOrigin", action = "Chart" },
             new[] { "Nop.Plugin.TorchDesign.CustomerOrigin.Controllers" }
        );

            //routes.MapRoute("Plugin.TorchDesign.CustomerOrigin.EditPopup",
            //     "Plugins/TorchDesignCustomerOrigin/EditPopup",
            //     new { controller = "TorchDesignCustomerOrigin", action = "EditPopup" },
            //     new[] { "Nop.Plugin.TorchDesign.CustomerOrigin.Controllers" }
            //);

            routes.MapRoute("MiscCheckoutConfirm",
                            "checkout/confirm",
                            new { controller = "MiscCheckout", action = "Confirm" },
                            new[] { "Nop.Plugin.TorchDesign.CustomerOrigin.Controllers" });
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
