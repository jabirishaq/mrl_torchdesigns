using System.Data.Entity.ModelConfiguration;
using Nop.Plugin.TorchDesign.Zip2Tax.Domain;

namespace Nop.Plugin.TorchDesign.Zip2Tax.Data
{
    public partial class Zip2TaxCounterRecordMap : EntityTypeConfiguration<Td_Zip2TaxCounter>
    {
        public Zip2TaxCounterRecordMap()
        {
            this.ToTable("Td_Zip2TaxCounter");
            this.HasKey(x => x.Id);


        }
    }
}