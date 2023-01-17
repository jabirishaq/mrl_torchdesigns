using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Plugin.Widgets.TorchDesign_Support.Domain;

namespace Nop.Plugin.Widgets.TorchDesign_Support.Data
{
	public partial class SupportVideoRelatedProductMap : EntityTypeConfiguration<SupportVideoRelatedProduct>
	{
		public SupportVideoRelatedProductMap()
		{
			this.ToTable("TD_SupportVideoRelatedProduct");
			this.HasKey(svp => svp.Id);

			this.HasRequired(svp => svp.SupportedVideos)
					  .WithMany(p => p.SupportVideosRelatedProducts)
					  .HasForeignKey(svc => svc.SupportVideoId);

			//this.HasRequired(svc => svc.RelatedProducts)
			//	  .WithMany(p => p.SupportVideoSupportCategory)
			//	  .HasForeignKey(svc => svc.ProductId);
		}
	}
}
