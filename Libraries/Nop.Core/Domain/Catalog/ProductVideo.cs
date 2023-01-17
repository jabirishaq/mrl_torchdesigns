
using Nop.Core.Domain.Media;

namespace Nop.Core.Domain.Catalog
{
    /// <summary>
    /// Represents a product picture mapping
    /// </summary>
    public partial class ProductVideo : BaseEntity
    {
        /// <summary>
        /// Gets or sets the product identifier
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the picture identifier
        /// </summary>
        public int PictureId { get; set; }

        public string VideoTitle { get; set; }
        public string VideoDescription { get; set; }

        /// <summary>
        /// Gets or sets the picture identifier
        /// </summary>
        public string VideoId { get; set; }

        public string TagName { get; set; }

        public string TabName { get; set; }
        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        /// 

        public int DisplayOrder { get; set; }
        
        /// <summary>
        /// Gets the picture
        /// </summary>
        public virtual Picture Picture { get; set; }

        /// <summary>
        /// Gets the product
        /// </summary>
        public virtual Product Product { get; set; }
    }

}
