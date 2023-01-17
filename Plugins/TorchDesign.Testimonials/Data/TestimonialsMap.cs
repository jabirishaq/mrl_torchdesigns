using System.Data.Entity.ModelConfiguration;
using Nop.Plugin.TorchDesign.Testimonials.Domain;

namespace Nop.Plugin.TorchDesign.Testimonials.Data
{
    public partial class TestimonialsMap : EntityTypeConfiguration<Td_Testimonials>
    {
        public TestimonialsMap()
        {
            this.ToTable("Td_Testimonials");
            this.HasKey(x => x.Id);
          
          
        }
    }
}