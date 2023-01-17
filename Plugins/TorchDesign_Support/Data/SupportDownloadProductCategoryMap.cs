using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Plugin.Widgets.TorchDesign_Support.Domain;

namespace Nop.Plugin.Widgets.TorchDesign_Support.Data
{
	/// <summary>
	/// Created By Nick
	/// Represents Support Download Product Category
	/// </summary>
	public partial class SupportDownloadProductCategoryMap : EntityTypeConfiguration<SupportDownloadProductCategory>
	{
		public SupportDownloadProductCategoryMap()
		{
			this.ToTable("TD_SupportDownloadProductCategory");
			this.HasKey(sdpc => sdpc.Id);

			this.HasRequired(sdpc => sdpc.SupportDownload)
					  .WithMany(p => p.SupportDownloadProductCategories)
					  .HasForeignKey(sdpc => sdpc.SupportDownloadId);

			//this.HasRequired(sdpc => sdpc.Category)
			//	  .WithMany(p => p.SupportDownloadProductCategories)
			//	  .HasForeignKey(sdpc => sdpc.CategoryId);
		}

	}
}