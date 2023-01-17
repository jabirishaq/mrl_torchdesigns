using System.Data.Entity.ModelConfiguration;
using Nop.Plugin.TorchDesign.Testimonials.Domain;

namespace Nop.Plugin.TorchDesign.Testimonials.Data
{
    public partial class Testimonials_PictureMap : EntityTypeConfiguration<Td_Testimonials_Pictures>
    {
        public Testimonials_PictureMap()
        {
            this.ToTable("Td_Testimonials_Pictures");
            this.HasKey(pc => pc.Id);
                      

            //this.HasRequired(pc => pc.Category)
            //    .WithMany()
            //    .HasForeignKey(pc => pc.CategoryId);


            //this.HasRequired(pc => pc.Testimonials)
            //    .WithMany(p => p.TestimonialsMapping)
            //    .HasForeignKey(pc => pc.TestimonialsId);

          
        }
    }
}