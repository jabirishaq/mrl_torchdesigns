using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Plugin.Widgets.TorchDesign_StoreLocator.Data;
using Nop.Plugin.Widgets.TorchDesign_StoreLocator.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Mvc;
using Nop.Plugin.Widgets.TorchDesign_StoreLocator.Domain;
using Nop.Data;
using Nop.Core.Data;

namespace Nop.Plugin.Widgets.TorchDesign_StoreLocator
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<StoreLocatorService>().As<IStoreLocatorService>().InstancePerHttpRequest();

            //data context
            this.RegisterPluginDataContext<StoreLocatorObjctContext>(builder, "nop_object_context_Torch_StoreLocator");

            //override required repository with our custom context
            builder.RegisterType<EfRepository<StoreLocator>>()
                .As<IRepository<StoreLocator>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_Torch_StoreLocator"))
                .InstancePerHttpRequest();

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
