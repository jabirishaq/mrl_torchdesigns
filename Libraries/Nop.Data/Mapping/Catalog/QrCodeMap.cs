using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Catalog;

namespace Nop.Data.Mapping.Catalog
{
    public partial class QrCodeMap : EntityTypeConfiguration<QrCode>
    {
        public QrCodeMap()
        {
            this.ToTable("Td_QrCode");
            this.HasKey(pp => pp.Id);
            
        }
    }
}