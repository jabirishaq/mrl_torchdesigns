using System.Data.Entity.ModelConfiguration;
using Nop.Plugin.Widgets.TorchDesign_Support.Domain;

namespace Nop.Plugin.Widgets.TorchDesign_Support.Data
{
	public partial class SupportCategoryMap : EntityTypeConfiguration<SupportCategory>
    {
		public SupportCategoryMap()
        {
			  this.ToTable("TD_SupportTopicSupportCategory");
			  this.HasKey(on => on.Id);
			  this.Property(on => on.Title).IsRequired().HasMaxLength(400);
			  this.Property(on => on.Description).HasMaxLength(4000);
			  this.Property(on => on.ShowInFooter).IsRequired();
        }
    }
}