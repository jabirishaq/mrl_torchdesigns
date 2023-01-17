using System.Data.Entity.ModelConfiguration;
using Nop.Plugin.Widgets.TorchDesign_Support.Domain;

namespace Nop.Plugin.Widgets.TorchDesign_Support.Data
{
	public partial class SupportTopicStepMap : EntityTypeConfiguration<SupportTopicStep>
    {
		public SupportTopicStepMap()
        {
			  this.ToTable("TD_SupportTopicStep");
			  this.HasKey(on => on.Id);
           this.Property(on => on.Title).IsRequired().HasMaxLength(400);
			 
           
        }
    }
}