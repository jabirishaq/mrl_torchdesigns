using System.Data.Entity.ModelConfiguration;
using Nop.Plugin.TorchDesign.MegaSearch.Domain;
 
namespace Nop.Plugin.TorchDesign.MegaSearch.Data
{
    public partial class Td_MegaSearchRecordMap : EntityTypeConfiguration<Td_MegaSearch>
    {
        public Td_MegaSearchRecordMap()
        {
            this.ToTable("Td_MegaSearch");
            this.HasKey(x => x.Id);
 
 
 
        }
    }
}
