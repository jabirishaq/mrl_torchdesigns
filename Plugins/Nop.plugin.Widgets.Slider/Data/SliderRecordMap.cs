using Nop.Plugin.Widgets.Slider.Data;
using Nop.Plugin.Widgets.Slider.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.Slider.Data
{
    public class SliderRecordMap : EntityTypeConfiguration<SliderRecord>
    {
        public SliderRecordMap()
        {
            this.ToTable("Slider");
            this.HasKey(x => x.Id);
        }
    }
}
