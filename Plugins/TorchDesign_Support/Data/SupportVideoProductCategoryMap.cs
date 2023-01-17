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
	/// Represents Support Video Category
	/// </summary>
	public partial class SupportVideoProductCategoryMap : EntityTypeConfiguration<SupportVideoProductCategory>
	{
		public SupportVideoProductCategoryMap()
		{
			this.ToTable("TD_SupportVideoProductCategory");
			this.HasKey(svc => svc.Id);

			this.HasRequired(svc => svc.SupportVideo)
					  .WithMany(p => p.SupportVideoProductCategories)
					  .HasForeignKey(svc => svc.SupportVideoId);

			//this.HasRequired(svc => svc.Category)
			//	  .WithMany(p => p.SupportVideoProductCategories)
			//	  .HasForeignKey(svc => svc.CategoryId);
		}

	}
}