using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Seo;
using Nop.Core.Domain.Stores;

namespace Nop.Plugin.Widgets.TorchDesign_Support.Domain
{
    /// <summary>
	/// Created By Nick
	/// Represents a SupportTopic
    /// </summary>
	public partial class SupportDownload : BaseEntity
    {
		private ICollection<SupportDownloadProductCategory> _supportDownloadProductCategories;
		private ICollection<SupportDownloadRelatedProduct> _supportDownloadRelatedProducts;
	
        /// <summary>
        /// Gets or sets the title
        /// </summary>
        public string Title { get; set; }

		  /// <summary>
		  /// Gets or sets the download identifier
		  /// </summary>
		  public int DownloadId { get; set; }

          public string Description { get; set; }

          public int DisplayOrder { get; set; }
		  /// <summary>
		  /// Gets or sets the collection of ProductCategory
		  /// </summary>
		  public virtual ICollection<SupportDownloadProductCategory> SupportDownloadProductCategories
		  {
			  get { return _supportDownloadProductCategories ?? (_supportDownloadProductCategories = new List<SupportDownloadProductCategory>()); }
			  protected set { _supportDownloadProductCategories = value; }
		  }

		  public virtual ICollection<SupportDownloadRelatedProduct> SupportDownloadRelatedProducts
		  {
			  get { return _supportDownloadRelatedProducts ?? (_supportDownloadRelatedProducts = new List<SupportDownloadRelatedProduct>()); }
			  protected set { _supportDownloadRelatedProducts = value; }
		  }

		 

    }
}
