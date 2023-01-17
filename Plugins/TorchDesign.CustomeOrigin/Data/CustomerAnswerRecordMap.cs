using System.Data.Entity.ModelConfiguration;
using Nop.Plugin.TorchDesign.CustomerOrigin.Domain;

namespace Nop.Plugin.TorchDesign.CustomerOrigin.Data
{
    public partial class CustomerOriginAnswerRecordMap : EntityTypeConfiguration<Td_CustomerOriginAnswer>
    {
        public CustomerOriginAnswerRecordMap()
        {
            this.ToTable("Td_CustomerOriginAnswer");
            this.HasKey(x => x.Id);


        }
    }
}