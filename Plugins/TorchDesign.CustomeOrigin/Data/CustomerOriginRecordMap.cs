using System.Data.Entity.ModelConfiguration;
using Nop.Plugin.TorchDesign.CustomerOrigin.Domain;
 
namespace Nop.Plugin.TorchDesign.CustomerOrigin.Data
{
    public partial class CustomerOriginRecordMap : EntityTypeConfiguration<Td_CustomerOrigin>
    {
        public CustomerOriginRecordMap()
        {
            this.ToTable("Td_CustomerOrigin");
            this.HasKey(x => x.Id);
 
 
 
        }
    }
}
