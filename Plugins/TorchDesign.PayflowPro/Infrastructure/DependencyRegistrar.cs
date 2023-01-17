using Autofac;
using Autofac.Core;
using Nop.Core.Data;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Data;
using Nop.Plugin.TorchDesign.PayflowPro.Data;
using Nop.Plugin.TorchDesign.PayflowPro.Domain;
using Nop.Plugin.TorchDesign.PayflowPro.Services;
using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.TorchDesign.PayflowPro.Infrastructure
{
    public partial class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<CreditCardDeclinedLogService>().As<ICreditCardDeclinedLogService>().InstancePerLifetimeScope();

            //data context
            this.RegisterPluginDataContext<PayflowObjectContext>(builder, "nop_object_context_Payflow");

            //override required repository with our custom context
            builder.RegisterType<EfRepository<Td_CreditCardDeclinedLog>>()
                .As<IRepository<Td_CreditCardDeclinedLog>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_Payflow"))
                .InstancePerLifetimeScope();
        }
        public int Order
        {
            get { return 1; }
        }
    }
}
