using Nop.Core;
using Nop.Core.Domain.Catalog;
namespace Nop.Plugin.Widgets.TorchDesign_Support.Domain
{
    /// <summary>
    /// Represents a related product
    /// </summary>
	public partial class SupportDownloadRelatedProduct : BaseEntity
    {										  
        /// <summary>
         /// Gets or sets the support  Download identifier
        /// </summary>
		 public int SupportDownloadId { get; set; }

        /// <summary>
        /// Gets or sets the product identifier
        /// </summary>
		 public  int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }

		  /// <summary>
		  /// Gets the product
		  /// </summary>
		  //public virtual Product Product { get; set; }

		  /// <summary>
		  /// Gets the support download
		  /// </summary>
		  public virtual SupportDownload SupportDownload { get; set; }
    }

}
