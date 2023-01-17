using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Mvc;
using Nop.Data;
using Nop.Core.Data;
using Nop.Plugin.Widgets.TorchDesign_Support.Services;
using Nop.Plugin.Widgets.TorchDesign_Support.Data;
using Nop.Plugin.Widgets.TorchDesign_Support.Domain;

namespace Nop.Plugin.Widgets.TorchDesign_StoreLocator
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
			  builder.RegisterType<SupportService>().As<ISupportService>().InstancePerLifetimeScope();

            //data context
			  this.RegisterPluginDataContext<SupportObjectContext>(builder, "nop_object_context_Torch_Support");

            //override required repository with our custom context
			  builder.RegisterType<EfRepository<SupportCategory>>()
					 .As<IRepository<SupportCategory>>()
					 .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_Torch_Support"))
					 .InstancePerLifetimeScope();

			  builder.RegisterType<EfRepository<SupportTopic>>()
					 .As<IRepository<SupportTopic>>()
					 .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_Torch_Support"))
					 .InstancePerLifetimeScope();

			  builder.RegisterType<EfRepository<SupportTopicProductCategory>>()
					 .As<IRepository<SupportTopicProductCategory>>()
					 .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_Torch_Support"))
					 .InstancePerLifetimeScope();

			  builder.RegisterType<EfRepository<SupportTopicRelatedProduct>>()
				  .As<IRepository<SupportTopicRelatedProduct>>()
				  .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_Torch_Support"))
				  .InstancePerLifetimeScope();

			  builder.RegisterType<EfRepository<SupportTopicStep>>()
					 .As<IRepository<SupportTopicStep>>()
					 .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_Torch_Support"))
					 .InstancePerLifetimeScope();

              builder.RegisterType<EfRepository<SupportCategoryProductCategory>>()
                      .As<IRepository<SupportCategoryProductCategory>>()
                      .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_Torch_Support"))
                      .InstancePerLifetimeScope();

              //builder.RegisterType<EfRepository<SupportTopicStepMapping>>()
              //       .As<IRepository<SupportTopicStepMapping>>()
              //       .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_Torch_Support"))
              //       .InstancePerLifetimeScope();

			  

			  builder.RegisterType<EfRepository<SupportTopicSupportCategory>>()
					 .As<IRepository<SupportTopicSupportCategory>>()
					 .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_Torch_Support"))
					 .InstancePerLifetimeScope();

			  builder.RegisterType<EfRepository<SupportVideo>>()
						 .As<IRepository<SupportVideo>>()
						 .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_Torch_Support"))
						 .InstancePerLifetimeScope();

			  builder.RegisterType<EfRepository<SupportVideoProductCategory>>()
					.As<IRepository<SupportVideoProductCategory>>()
					.WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_Torch_Support"))
					.InstancePerLifetimeScope();

			  builder.RegisterType<EfRepository<SupportVideoRelatedProduct>>()
				 .As<IRepository<SupportVideoRelatedProduct>>()
				 .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_Torch_Support"))
				 .InstancePerLifetimeScope();

			  builder.RegisterType<EfRepository<SupportDownload>>()
					.As<IRepository<SupportDownload>>()
					.WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_Torch_Support"))
					.InstancePerLifetimeScope();

			  builder.RegisterType<EfRepository<SupportDownloadProductCategory>>()
					.As<IRepository<SupportDownloadProductCategory>>()
					.WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_Torch_Support"))
					.InstancePerLifetimeScope();

			  builder.RegisterType<EfRepository<SupportDownloadRelatedProduct>>()
				 .As<IRepository<SupportDownloadRelatedProduct>>()
				 .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_Torch_Support"))
				 .InstancePerLifetimeScope();

		

			 

        //    builder.RegisterType<EfRepository<FAQ_Category>>()
        //       .As<IRepository<FAQ_Category>>()
        //       .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_Event"))
        //       .InstancePerHttpRequest();

        }
        public int Order
        {
            get { return 1; }
        }
    }
}
