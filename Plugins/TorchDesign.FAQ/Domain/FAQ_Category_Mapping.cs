using Nop.Core;
using Nop.Core.Domain.Localization;

namespace Nop.Plugin.TorchDesign.FAQ.Domain
{
    /// <summary>
    /// Represents a FAQ record
    /// </summary>
    public partial class FAQ_Category_Mapping : BaseEntity, ILocalizedEntity
    {
      
        /// <summary>
        /// Gets or sets the Question
        /// </summary>
        public int FaqId { get; set; }

        /// <summary>
        /// Gets or sets the Category Id
        /// </summary>
        public int CategoryId { get; set; }

        /// Gets or sets the DisplayOrder
        /// /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets the category
        /// </summary>
        public virtual FAQ_Category Category { get; set; }

        /// <summary>
        /// Gets the product
        /// </summary>
        public virtual FrequentlyAskedQ Faq { get; set; }
    }
}