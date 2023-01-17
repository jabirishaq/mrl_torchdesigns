using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Plugin.TorchDesign.MegaSearch.Data;
using Nop.Plugin.TorchDesign.MegaSearch.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Mvc;
using Nop.Plugin.TorchDesign.MegaSearch.Domain;
using Nop.Data;
using Nop.Core.Data;

namespace Nop.Plugin.TorchDesign.MegaSearch
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<MegaSearchService>().As<IMegaSearchService>().InstancePerLifetimeScope();

            //data context
            this.RegisterPluginDataContext<MegaSearchObjctContext>(builder, "nop_object_context_Megasearch");

            //override required repository with our custom context
            //builder.RegisterType<EfRepository<Td_Zip2TaxCounter>>()
            //    .As<IRepository<Td_Zip2TaxCounter>>()
            //    .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_Megasearch"))
            //    .InstancePerLifetimeScope();

         

        }
        public int Order
        {
            get { return 1; }
        }
    }
}
