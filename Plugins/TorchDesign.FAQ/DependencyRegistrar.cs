using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Plugin.TorchDesign.FAQ.Data;
using Nop.Plugin.TorchDesign.FAQ.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Mvc;
using Nop.Plugin.TorchDesign.FAQ.Domain;
using Nop.Data;
using Nop.Core.Data;

namespace Nop.Plugin.TorchDesign.FAQ
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<FrequentlyAskedQService>().As<IFrequentlyAskedQService>().InstancePerLifetimeScope();

            //data context
            this.RegisterPluginDataContext<FrequentlyAskedQObjectContext>(builder, "faq_object");

            //override required repository with our custom context
            builder.RegisterType<EfRepository<FrequentlyAskedQ>>()
                .As<IRepository<FrequentlyAskedQ>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>("faq_object"))
                .InstancePerLifetimeScope();

            builder.RegisterType<EfRepository<FAQ_Category>>()
               .As<IRepository<FAQ_Category>>()
               .WithParameter(ResolvedParameter.ForNamed<IDbContext>("faq_object"))
               .InstancePerLifetimeScope();

            builder.RegisterType<EfRepository<FAQ_Category_Mapping>>()
             .As<IRepository<FAQ_Category_Mapping>>()
             .WithParameter(ResolvedParameter.ForNamed<IDbContext>("faq_object"))
             .InstancePerLifetimeScope();

        }
        public int Order
        {
            get { return 1; }
        }
    }
}
