using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Catalog;

namespace Nop.Data.Mapping.Catalog
{
    public partial class SpecificationAttributeGroupMap : EntityTypeConfiguration<SpecificationAttributeGroup>
    {
        public SpecificationAttributeGroupMap()
        {
            this.ToTable("SpecificationAttributeGroup");
            this.HasKey(c => c.Id);
        }
    }
}
