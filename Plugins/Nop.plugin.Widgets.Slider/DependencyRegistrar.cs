using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Plugin.Widgets.Slider.Data;
using Nop.Plugin.Widgets.Slider.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Mvc;
using Nop.Plugin.Widgets.Slider.Domain;
using Nop.Data;
using Nop.Core.Data;

namespace Nop.Plugin.Widgets.Slider
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<SliderService>().As<ISliderService>().InstancePerHttpRequest();

            //data context
            this.RegisterPluginDataContext<SliderObjectContext>(builder, "nop_object_context_Event");

            //override required repository with our custom context
            builder.RegisterType<EfRepository<SliderRecord>>()
                .As<IRepository<SliderRecord>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_Event"))
                .InstancePerHttpRequest();

        }
        public int Order
        {
            get { return 1; }
        }
    }
}
