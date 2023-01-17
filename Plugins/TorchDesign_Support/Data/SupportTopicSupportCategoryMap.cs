using System.Data.Entity.ModelConfiguration;
using Nop.Plugin.Widgets.TorchDesign_Support.Domain;

namespace Nop.Plugin.Widgets.TorchDesign_Support.Data
{
	public partial class SupportTopicSupportCategoryMap : EntityTypeConfiguration<SupportTopicSupportCategory>
    {
		public SupportTopicSupportCategoryMap()
        {
			  this.ToTable("TD_SupportTopicSupportCategoryMapping");
			  this.HasKey(pm => pm.Id);
			
			  this.HasRequired(pm => pm.SupportTopic)
					  .WithMany(p => p.SupportTopicSupportCategory)
					  .HasForeignKey(pm => pm.SupportTopicId);

			  this.HasRequired(pm => pm.SupportCategory)
				.WithMany(p => p.SupportTopicSupportCategory)
				.HasForeignKey(pm => pm.SupportCategoryId);

			  //this.Property(on => on.TopicId).IsRequired();
			  //this.Property(on => on.SpCatId).IsRequired();

			  //this.HasRequired(pm => pm.SupportTopic)
			  //	 .WithMany()
			  //	 .HasForeignKey(pm => pm.SupportTopicId);


			  //this.HasRequired(pm => pm.SupportCategory)
			  //  .WithMany()
			  //  .HasForeignKey(pm => pm.SupportCategoryId);


           
        }
		
    }
}