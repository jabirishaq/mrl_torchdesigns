using System.Web.Mvc;
using System.Web.Routing;
using Nop.Web.Framework.Mvc.Routes;

namespace Nop.Plugin.Shipping.ByWeight
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {

            routes.MapRoute("Plugin.Widgets.Testimonials.EditPopup",
                 "Plugins/Testimonial/EditPopup",
                 new { controller = "Testimonials", action = "EditPopup" },
                 new[] { "Nop.Plugin.TorchDesign.Testimonials.Controllers" }
            );
            routes.MapRoute("Plugin.ActiveTestimonial",
              "Plugins/Testimonial/Activate",
              new { controller = "Testimonials", action = "Activate" },
              new[] { "Nop.Plugin.TorchDesign.Testimonials.Controllers" }
         );
            routes.MapRoute("Plugin.RejectTestimonial",
              "Plugins/Testimonial/Reject",
              new { controller = "Testimonials", action = "Reject" },
              new[] { "Nop.Plugin.TorchDesign.Testimonials.Controllers" }
         );

            routes.MapRoute("Plugin.Testimonial",
                        "plugintestomonial/{code}",
                        new { controller = "Testimonials", action = "Configure", code = UrlParameter.Optional },
                        new[] { "Nop.Plugin.TorchDesign.Testimonials.Controllers" }
                        );

            routes.MapRoute("Plugin.Testimonial.Import",
                     "plugintestomonialimport/import",
                     new { controller = "Testimonials", action = "Import" },
                     new[] { "Nop.Plugin.TorchDesign.Testimonials.Controllers" }
                     );

            routes.MapRoute("Plugin.SingleTestimonial",
              "testimonial/{testimonialid}",
              new { controller = "Testimonials", action = "SingleTestimonial" },
               new { testimonialid = @"\d+" },
              new[] { "Nop.Plugin.TorchDesign.Testimonials.Controllers" });
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
