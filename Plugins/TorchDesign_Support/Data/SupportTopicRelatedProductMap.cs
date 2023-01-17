using System.Data.Entity.ModelConfiguration;
using Nop.Plugin.Widgets.TorchDesign_Support.Domain;

namespace Nop.Plugin.Widgets.TorchDesign_Support.Data
{
	public partial class SupportTopicRelatedProductMap : EntityTypeConfiguration<SupportTopicRelatedProduct>
    {
        public SupportTopicRelatedProductMap()
        {
			  this.ToTable("TD_SupportTopicRelatedProduct");
            this.HasKey(c => c.Id);
        }
    }
}