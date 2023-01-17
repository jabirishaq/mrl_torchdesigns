using System.Data.Entity.ModelConfiguration;
using Nop.Plugin.TorchDesign.Zip2Tax.Domain;
 
namespace Nop.Plugin.TorchDesign.Zip2Tax.Data
{
    public partial class Zip2TaxRecordMap : EntityTypeConfiguration<Td_Zip2Tax>
    {
        public Zip2TaxRecordMap()
        {
            this.ToTable("Td_Zip2Tax");
            this.HasKey(x => x.Id);
 
 
 
        }
    }
}
