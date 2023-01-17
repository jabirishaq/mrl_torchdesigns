using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Catalog;

namespace Nop.Data.Mapping.Catalog
{
    public partial class ProductCustomeFieldMap : EntityTypeConfiguration<ProductCustomeField>
    {
        public ProductCustomeFieldMap()
        {
            this.ToTable("Td_ProductCustomeField");
            this.HasKey(p => p.Id);

            //this.HasMany(p => p.ProductsCustome)
            //   .WithMany(pt => pt.ProductCustomField)
            //.Map(m => m.ToTable("Product"));

            //this.HasMany(p => p.ProductTags)
            //  .WithMany(pt => pt.Products)
            //  .Map(m => m.ToTable("Product_ProductTag_Mapping"));
        }
    }
}