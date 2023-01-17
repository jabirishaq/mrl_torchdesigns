
using Nop.Core.Domain.Media;

namespace Nop.Core.Domain.Catalog
{
    /// <summary>
    /// Represents a product picture mapping
    /// </summary>
    public partial class ProductDownload : BaseEntity
    {
        /// <summary>
        /// Gets or sets the product identifier
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the picture identifier
        /// </summary>
        public int DownloadId { get; set; }

        /// <summary>
        /// Gets or sets the picture identifier
        /// </summary>
        public string DownloadTitle { get; set; }

        public string DownloadDescription { get; set; }

      
        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        /// 

        public int DisplayOrder { get; set; }
        
        public virtual Product Product { get; set; }

        public virtual Download Download { get; set; }
    }

}
