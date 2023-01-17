using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Catalog;

namespace Nop.Data.Mapping.Catalog
{
    public partial class ProductInBoxMap : EntityTypeConfiguration<ProductInBox>
    {
        public ProductInBoxMap()
        {
            this.ToTable("Td_Product_In_Box");
            this.HasKey(pp => pp.Id);
            
        }
    }
}