using System.Web.Mvc;
using System.Web.Routing;
using Nop.Web.Framework.Mvc.Routes;


namespace Nop.Plugin.Widgets.TorchDesign_StoreLocator
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {


            //routes.MapRoute("Plugin.Shipping.ByWeight.SaveGeneralSettings",
            //     "Plugins/ShippingByWeight/SaveGeneralSettings",
            //     new { controller = "ShippingByWeight", action = "SaveGeneralSettings", },
            //     new[] { "Nop.Plugin.Shipping.ByWeight.Controllers" }
            //);
            routes.MapRoute("Plugin.Widgets.TorchDesign_StoreLocator.Configure",
            "Plugins/WidgetsSlider/Configure",
            new { controller = "TorchDesign_StoreLocator", action = "Configure" },
                    new[] { "Nop.Plugin.Widgets.TorchDesign_StoreLocatorr.Controllers" });

            routes.MapRoute("Plugin.Widgets.TorchDesign_StoreLocator.AddPopup",
                 "PluginsStoreLocator/AddPopup",
                 new { controller = "TorchDesign_StoreLocator", action = "AddPopup" },
                 new[] { "Nop.Plugin.Widgets.TorchDesign_StoreLocator.Controllers" }
            );

            routes.MapRoute("Plugin.Widgets.TorchDesign_StoreLocator.EditPopup",
                 "Plugins/StoreLocator/EditPopup",
                 new { controller = "TorchDesign_StoreLocator", action = "EditPopup" },
                 new[] { "Nop.Plugin.Widgets.TorchDesign_StoreLocator.Controllers" }
            );

            routes.MapRoute("Plugin.Widgets.TorchDesign_StoreLocator.publicinfo",
                 "Plugins/StoreLocator/publicinfo",
                new { controller = "TorchDesign_StoreLocator", action = "PublicInfo" },
                new[] { "Nop.Plugin.Widgets.TorchDesign_StoreLocator.Controllers" }
            );
         
            routes.MapRoute("Plugin.Widgets.TorchDesign_StoreLocator.GetModifiedImage",
             "GetModifiedImage/{imageid}",
             new { controller = "TorchDesign_StoreLocator", action = "GetModifiedImage" },
               new { imageid = @"\d+" },
             new[] { "Nop.Plugin.Widgets.TorchDesign_StoreLocator.Controllers" }
        );
            routes.MapRoute("Plugin.Widgets.TorchDesign_StoreLocator.GetModifiedImageRed",
            "GetModifiedImageRed/{imageid}",
            new { controller = "TorchDesign_StoreLocator", action = "GetModifiedImageRed" },
              new { imageid = @"\d+" },
            new[] { "Nop.Plugin.Widgets.TorchDesign_StoreLocator.Controllers" }
       );
            routes.MapRoute("Plugin.Widgets.TorchDesign_StoreLocator.Getlet",
         "Getlet/{id}/{address}",
         new { controller = "TorchDesign_StoreLocator", action = "Getlet" },
           new { imageid = @"\d+" },
         new[] { "Nop.Plugin.Widgets.TorchDesign_StoreLocator.Controllers" }
    );
            // routes.MapRoute("Plugin.Widgets.StoreLocator.AddCategoryPopup",
            //     "Plugins/StoreLocator/AddCategoryPopup",
            //     new { controller = "TorchDesign_StoreLocator", action = "AddCategoryPopup" },
            //     new[] { "Nop.Plugin.Widgets.FAQ.Controllers" }
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
