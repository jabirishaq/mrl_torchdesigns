using Autofac;
using Autofac.Core;
using Nop.Core.Data;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Data;
using Nop.Plugin.Payments.AuthorizeNet.Data;
using Nop.Plugin.Payments.AuthorizeNet.Domain;
using Nop.Plugin.Payments.AuthorizeNet.Services;
using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Payments.AuthorizeNet.Infrastructure
{
    public partial class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<AuthorizeNetCardDeclinedLogService>().As<IAuthorizeNetCardDeclinedLogService>().InstancePerLifetimeScope();

            //data context
            this.RegisterPluginDataContext<AuthorizeNet_ObjectContext>(builder, "nop_object_context_authorizenet");

            //override required repository with our custom context
            builder.RegisterType<EfRepository<Td_AuthorizeNet_DeclinedCard_Log>>()
                .As<IRepository<Td_AuthorizeNet_DeclinedCard_Log>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_authorizenet"))
                .InstancePerLifetimeScope();
        }
        public int Order
        {
            get { return 1; }
        }
    }
}
