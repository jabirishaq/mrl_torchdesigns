using System.Data.Entity.ModelConfiguration;
using Nop.Plugin.Widgets.TorchDesign_Support.Domain;

namespace Nop.Plugin.Widgets.TorchDesign_Support.Data
{
	public partial class SupportTopicProductCategoryMap : EntityTypeConfiguration<SupportTopicProductCategory>
    {
		public SupportTopicProductCategoryMap()
        {

			  this.ToTable("TD_SupportTopicProductCategoryMapping");
            this.HasKey(on => on.Id);
				
				//this.HasRequired(pm => pm.SupportTopic)
				// .WithMany(p => p.SupportTopicProductCategories)
				// .HasForeignKey(pm => pm.SupportTopicId);

				//this.HasRequired(pm => pm.Category)
				// .WithMany(p => p.SupportTopicProductCategories)
				// .HasForeignKey(pm => pm.CategoryId);

			


           
        }
    }
}