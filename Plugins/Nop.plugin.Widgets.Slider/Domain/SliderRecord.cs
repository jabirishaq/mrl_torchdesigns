using Nop.Core;
using Nop.Core.Domain.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.Slider.Domain
{
    public partial class SliderRecord : BaseEntity, ILocalizedEntity
    {
        /// <summary>
        /// GET Or SET the Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// GET Or SET the Short Description
        /// </summary>
        public string ShortDescription { get; set; }
        public string link { get; set; }
        public int DisplayOrder { get; set; }
        public int DisplayOption { get; set; }
        public int SlidePushtime { get; set; }
        public int SlidingEffect { get; set; }

        /// <summary>
        /// GET Or SET the Picture
        /// </summary>
        public int PictureId { get; set; }


        /// <summary>
        /// GET Or SET the Publish
        /// </summary>
        /// 
        public bool Published { get; set; }
    }
}
