using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Catalog;

namespace Nop.Data.Mapping.Catalog
{
    public partial class ProductVideoMap : EntityTypeConfiguration<ProductVideo>
    {
        public ProductVideoMap()
        {
            this.ToTable("Td_Product_Videos_Mapping");
            this.HasKey(pp => pp.Id);
            
            this.HasRequired(pp => pp.Picture)
                .WithMany(p => p.ProductVideoPictures)
                .HasForeignKey(pp => pp.PictureId);


            this.HasRequired(pp => pp.Product)
                .WithMany(p => p.ProductvideoPictures)
                .HasForeignKey(pp => pp.ProductId);
        }
    }
}