using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Plugin.TorchDesign.Testimonials.Data;
using Nop.Plugin.TorchDesign.Testimonials.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Mvc;
using Nop.Plugin.TorchDesign.Testimonials.Domain;
using Nop.Data;
using Nop.Core.Data;


namespace Nop.Plugin.TorchDesign.Testimonials
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<Testimonialservice>().As<ITestimonialservice>().InstancePerLifetimeScope();
           // builder.RegisterType<SendTestimoialMailService>().As<ISendTestimoialMailService>().InstancePerHttpRequest();
            builder.RegisterType<TestimonialMessageService>().As<ITestimonialMessageService>().InstancePerHttpRequest();
            builder.RegisterType<TestimonialTokenProvider>().As<ITestimonialTokenProvider>().InstancePerHttpRequest();
            //data context
            this.RegisterPluginDataContext<TestimonialsObjctContext>(builder, "Testimonials_object");

            //override required repository with our custom context
            builder.RegisterType<EfRepository<Td_Testimonials>>()
                .As<IRepository<Td_Testimonials>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>("Testimonials_object"))
                .InstancePerLifetimeScope();

            builder.RegisterType<EfRepository<Td_Testimonials_Pictures>>()
               .As<IRepository<Td_Testimonials_Pictures>>()
               .WithParameter(ResolvedParameter.ForNamed<IDbContext>("Testimonials_object"))
               .InstancePerLifetimeScope();

            //builder.RegisterType<EfRepository<Testimonials_Category_Mapping>>()
            // .As<IRepository<Testimonials_Category_Mapping>>()
            // .WithParameter(ResolvedParameter.ForNamed<IDbContext>("Testimonials_object"))
            // .InstancePerLifetimeScope();

        }
        public int Order
        {
            get { return 1; }
        }
    }
}
