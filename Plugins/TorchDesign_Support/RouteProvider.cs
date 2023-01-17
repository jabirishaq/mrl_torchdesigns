using System.Web.Mvc;
using System.Web.Routing;
using Nop.Web.Framework.Mvc.Routes;


namespace Nop.Plugin.Widgets.TorchDesign_StoreLocator
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {



            routes.MapRoute("Plugin.Widgets.Support.EditPopup",
              "Plugins/Support/EditPopup",
              new { controller = "Support", action = "SupportTopicStepUpdate" },
              new[] { "Nop.Plugin.Widgets.TorchDesign_Support.Controllers" }
         );
            routes.MapRoute("Plugin.SupportMoveup",
              "Plugins/Support/Moveup",
              new { controller = "Support", action = "Moveup" },
              new[] { "Nop.Plugin.Widgets.TorchDesign_Support.Controllers" }
         );
            routes.MapRoute("Plugin.SupportMoveDown",
              "Plugins/Support/Movedown",
              new { controller = "Support", action = "Movedown" },
              new[] { "Nop.Plugin.Widgets.TorchDesign_Support.Controllers" }
         );

            routes.MapRoute("Plugin.DeleteStep",
              "Plugins/Support/SupportTopicStepDelete",
              new { controller = "Support", action = "SupportTopicStepDelete" },
              new[] { "Nop.Plugin.Widgets.TorchDesign_Support.Controllers" }
         );

            routes.MapRoute("Plugin.addnewstepajax",
            "Plugins/Support/addstepbyajax",
            new { controller = "Support", action = "EditSupportTopicAjax" },
            new[] { "Nop.Plugin.Widgets.TorchDesign_Support.Controllers" }
       );
            routes.MapRoute("Plugin.addnewstepajaxfornewtopic",
      "Plugins/Support/addstepbyajaxnewtopic",
      new { controller = "Support", action = "AddSupportTopicAjax" },
      new[] { "Nop.Plugin.Widgets.TorchDesign_Support.Controllers" }
 );

            routes.MapRoute("Plugin.removenullstep",
    "Plugins/Support/removestep",
    new { controller = "Support", action = "RemovenullrecordAjax" },
    new[] { "Nop.Plugin.Widgets.TorchDesign_Support.Controllers" }
);
            //    routes.MapRoute("Plugin.UpdateStep",
         //     "Plugins/Support/SupportTopicStepUpdate",
         //     new { controller = "Support", action = "SupportTopicStepUpdate" },
         //     new[] { "Nop.Plugin.Widgets.TorchDesign_Support.Controllers" }
         //);

            
        //    routes.MapRoute("Plugin.BackSupportTopicList",
        //     "Plugins/Support/backSupportTopicList",
        //     new { controller = "Support", action = "SupportTopicList" },
        //     new[] { "Nop.Plugin.Widgets.TorchDesign_Support.Controllers" }
        //);
            
              routes.MapRoute("Plugin.DeleteSupportTopic",
              "Plugins/Support/SupportopicDelete",
              new { controller = "Support", action = "SupportTopicDelete" },
              new[] { "Nop.Plugin.Widgets.TorchDesign_Support.Controllers" }
         );

              routes.MapRoute("Plugin.DeleteSupportDownload",
              "Plugins/Support/DeleteSupportDownload",
              new { controller = "Support", action = "DeleteSupportDownload" },
              new[] { "Nop.Plugin.Widgets.TorchDesign_Support.Controllers" }
         );
              routes.MapRoute("Plugin.DeleteSupportVideo",
             "Plugins/Support/DeleteSupportVideo",
             new { controller = "Support", action = "DeleteSupportVideo" },
             new[] { "Nop.Plugin.Widgets.TorchDesign_Support.Controllers" }
        );
              routes.MapRoute("Plugin.DeleteSupportCategory",
             "Plugins/Support/DeleteSupportCategory",
             new { controller = "Support", action = "SupportCategoryDelete" },
             new[] { "Nop.Plugin.Widgets.TorchDesign_Support.Controllers" }
        );
            routes.MapRoute("Public.SupportCategory",
                     "supportcatgory/{productCategorsename}",
                     new { controller = "Support", action = "SupportCategory" },
                     new[] { "Nop.Plugin.Widgets.TorchDesign_Support.Controllers" });
            routes.MapRoute("Public.SupportTopiclist",
                    "supporttopiclist/{suportCategorysename}",
                    new { controller = "Support", action = "SupportTopicListPublic" },
                    new[] { "Nop.Plugin.Widgets.TorchDesign_Support.Controllers" });

            routes.MapRoute("Public.SupportVideolist",
                   "supportvideolist/{productCategorsename}",
                   new { controller = "Support", action = "SupportVideoListPublic" },
                   new[] { "Nop.Plugin.Widgets.TorchDesign_Support.Controllers" });

            routes.MapRoute("Public.SupportDownloadlist",
                 "supportdownloadlist/{productCategorsename}",
                 new { controller = "Support", action = "SupportDownloadListPublic" },
                 new[] { "Nop.Plugin.Widgets.TorchDesign_Support.Controllers" });
 
            routes.MapRoute("Public.SupportTopicSingle",
                  "supporttopic/{suportTopicsename}",
                  new { controller = "Support", action = "SupportTopicPublic" },
                  new[] { "Nop.Plugin.Widgets.TorchDesign_Support.Controllers" });
            routes.MapRoute("EditSupportTopic",
                "editsupporttopic/{id}/{redirect}",
                new { controller = "Support", action = "EditSupportTopic" },
                 new { id = @"\d+"},
                new[] { "Nop.Plugin.Widgets.TorchDesign_Support.Controllers" });


            routes.MapRoute("CreateSupportTopic",
                "CreateSupportTopic/{redirect}",
                new { controller = "Support", action = "CreateSupportTopic" },
                new[] { "Nop.Plugin.Widgets.TorchDesign_Support.Controllers" });

            routes.MapRoute("CreateSupportVideo",
                "CreateSupportVideo/{redirect}",
                new { controller = "Support", action = "CreateSupportVideo" },
                new[] { "Nop.Plugin.Widgets.TorchDesign_Support.Controllers" });

            routes.MapRoute("CreateSupportDownload",
                "CreateSupportDownload/{redirect}",
                new { controller = "Support", action = "CreateSupportDownload" },
                new[] { "Nop.Plugin.Widgets.TorchDesign_Support.Controllers" });

            routes.MapRoute("GetDownloadContent",
                          "support/downloadcontent/{downloadid}",
                          new { controller = "Support", action = "SupportDownloadContent" },
                          new { downloadid = @"\d+" },
                          new[] { "Nop.Plugin.Widgets.TorchDesign_Support.Controllers" });
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
