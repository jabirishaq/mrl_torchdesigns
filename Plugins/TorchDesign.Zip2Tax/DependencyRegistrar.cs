using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Plugin.TorchDesign.Zip2Tax.Data;
using Nop.Plugin.TorchDesign.Zip2Tax.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Mvc;
using Nop.Plugin.TorchDesign.Zip2Tax.Domain;
using Nop.Data;
using Nop.Core.Data;

namespace Nop.Plugin.TorchDesign.Zip2Tax
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<Zip2TaxService>().As<IZip2TaxService>().InstancePerLifetimeScope();

            //data context
            this.RegisterPluginDataContext<Zip2TaxObjctContext>(builder, "nop_object_context_Zip2Tax");

            //override required repository with our custom context
            builder.RegisterType<EfRepository<Td_Zip2TaxCounter>>()
                .As<IRepository<Td_Zip2TaxCounter>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_Zip2Tax"))
                .InstancePerLifetimeScope();

            builder.RegisterType<EfRepository<Td_Zip2Tax>>()
               .As<IRepository<Td_Zip2Tax>>()
               .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_Zip2Tax"))
               .InstancePerLifetimeScope();

        }
        public int Order
        {
            get { return 1; }
        }
    }
}
