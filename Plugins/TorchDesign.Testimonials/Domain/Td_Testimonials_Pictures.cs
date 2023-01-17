using Nop.Core;
using Nop.Core.Domain.Localization;

namespace Nop.Plugin.TorchDesign.Testimonials.Domain
{
    /// <summary>
    /// Represents a Testimonials record
    /// </summary>
    public partial class Td_Testimonials_Pictures : BaseEntity, ILocalizedEntity
    {
      
        /// <summary>
        /// Gets or sets the TestimonialsId
        /// </summary>
        public int TestimonialsId { get; set; }

        /// <summary>
        /// Gets or sets the PictureId 
        /// </summary>
        public int PictureId { get; set; }

      
    }
}