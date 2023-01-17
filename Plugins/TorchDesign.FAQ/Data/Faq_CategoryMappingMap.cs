using System.Data.Entity.ModelConfiguration;
using Nop.Plugin.TorchDesign.FAQ.Domain;

namespace Nop.Plugin.TorchDesign.FAQ.Data
{
    public partial class Faq_CategoryMappingMap : EntityTypeConfiguration<FAQ_Category_Mapping>
    {
        public Faq_CategoryMappingMap()
        {
            this.ToTable("Td_Faq_Category_Mapping");
            this.HasKey(pc => pc.Id);
                      

            this.HasRequired(pc => pc.Category)
                .WithMany()
                .HasForeignKey(pc => pc.CategoryId);


            this.HasRequired(pc => pc.Faq)
                .WithMany(p => p.FAqMapping)
                .HasForeignKey(pc => pc.FaqId);

          
        }
    }
}