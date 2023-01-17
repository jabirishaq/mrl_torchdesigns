using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Plugin.Widgets.TorchDesign_Support.Domain;

namespace Nop.Plugin.Widgets.TorchDesign_Support.Data
{
	public partial class SupportDownloadRelatedProductMap : EntityTypeConfiguration<SupportDownloadRelatedProduct>
	{
		public SupportDownloadRelatedProductMap()
		{
			this.ToTable("TD_SupportDownloadRelatedProduct");
			this.HasKey(svp => svp.Id);

			this.HasRequired(sdrp => sdrp.SupportDownload)
					  .WithMany(p => p.SupportDownloadRelatedProducts)
					  .HasForeignKey(sdrp => sdrp.SupportDownloadId);

			//this.HasRequired(sdpc => sdpc.Product)
			//	  .WithMany(p => p.SupportDownloadRelatedProducts)
			//	  .HasForeignKey(sdpc => sdpc.ProductId);

		}
	}
}
