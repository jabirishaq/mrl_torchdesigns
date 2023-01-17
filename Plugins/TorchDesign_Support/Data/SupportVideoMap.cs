using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using Nop.Plugin.Widgets.TorchDesign_Support.Domain;

namespace Nop.Plugin.Widgets.TorchDesign_Support.Data
{
	public partial class SupportVideoMap : EntityTypeConfiguration<SupportVideo>
	{
		public SupportVideoMap()
        {
			  this.ToTable("TD_SupportVideo");
            this.HasKey(sv => sv.Id);
				this.Property(sv => sv.Title).IsRequired().HasMaxLength(400);
				this.Property(sv => sv.PictureId).IsRequired();
				this.Property(sv => sv.VimeoInformation).IsRequired();

        }
	}
}
