using System.Data.Entity.ModelConfiguration;
using Nop.Plugin.TorchDesign.FAQ.Domain;

namespace Nop.Plugin.TorchDesign.FAQ.Data
{
    public partial class FAQ_CategoryRecordMap : EntityTypeConfiguration<FAQ_Category>
    {
        public FAQ_CategoryRecordMap()
        {
            this.ToTable("Td_FAQ_Category");
            this.HasKey(x => x.Id);



        }
    }
}