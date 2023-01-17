using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Catalog;

namespace Nop.Data.Mapping.Catalog
{
    public partial class ProductThumbs : EntityTypeConfiguration<ProductThumb>
    {
        public ProductThumbs()
        {
            this.ToTable("TD_ProductThumb");
            this.HasKey(p => p.Id);
        }
    }
}