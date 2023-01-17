using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using Nop.Plugin.Widgets.TorchDesign_Support.Domain;

namespace Nop.Plugin.Widgets.TorchDesign_Support.Data
{
	public partial class SupportDownloadMap : EntityTypeConfiguration<SupportDownload>
	{
		public SupportDownloadMap()
        {
			   this.ToTable("TD_SupportDownloadMapping");
				this.HasKey(sd => sd.Id);
				this.Property(sd => sd.Title).IsRequired().HasMaxLength(252);
                this.Property(sd => sd.Description);
        }
	}
}
