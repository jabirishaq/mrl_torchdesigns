using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Plugin.TorchDesign.CustomerOrigin.Data;
using Nop.Plugin.TorchDesign.CustomerOrigin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Mvc;
using Nop.Plugin.TorchDesign.CustomerOrigin.Domain;
using Nop.Data;
using Nop.Core.Data;

namespace Nop.Plugin.TorchDesign.CustomerOrigin
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<CustomerOriginService>().As<ICustomerOriginService>().InstancePerLifetimeScope();

            //data context
            this.RegisterPluginDataContext<CustomerOriginObjctContext>(builder, "nop_object_context_CustomerOrigin");

            //override required repository with our custom context
            builder.RegisterType<EfRepository<Td_CustomerOriginAnswer>>()
                .As<IRepository<Td_CustomerOriginAnswer>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_CustomerOrigin"))
                .InstancePerLifetimeScope();

            builder.RegisterType<EfRepository<Td_CustomerOrigin>>()
               .As<IRepository<Td_CustomerOrigin>>()
               .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_CustomerOrigin"))
               .InstancePerLifetimeScope();

        }
        public int Order
        {
            get { return 1; }
        }
    }
}
