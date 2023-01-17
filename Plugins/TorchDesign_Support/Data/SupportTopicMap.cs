using System.Data.Entity.ModelConfiguration;
using Nop.Plugin.Widgets.TorchDesign_Support.Domain;

namespace Nop.Plugin.Widgets.TorchDesign_Support.Data
{
	public partial class SupportTopicMap : EntityTypeConfiguration<SupportTopic>
    {
		public SupportTopicMap()
        {
			  this.ToTable("TD_SupportTopic");
			  this.HasKey(on => on.Id);
			  this.Property(on => on.Title).IsRequired().HasMaxLength(400);
			  this.Property(on => on.Description);
        }
    }
}