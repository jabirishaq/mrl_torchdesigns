using System.Data.Entity.ModelConfiguration;
using Nop.Plugin.TorchDesign.FAQ.Domain;

namespace Nop.Plugin.TorchDesign.FAQ.Data
{
    public partial class FrequentlyAskedQRecordMap : EntityTypeConfiguration<FrequentlyAskedQ>
    {
        public FrequentlyAskedQRecordMap()
        {
            this.ToTable("Td_FAQ");
            this.HasKey(x => x.Id);

          
        }
    }
}