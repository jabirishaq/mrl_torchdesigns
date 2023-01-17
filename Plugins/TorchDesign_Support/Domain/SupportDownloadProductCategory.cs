using System;
using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.Catalog;

namespace Nop.Plugin.Widgets.TorchDesign_Support.Domain
{
    /// <summary>
	 /// Created By Nick
    /// Represents Support Download and Product Category mapping
    /// </summary>
	public partial class SupportDownloadProductCategory : BaseEntity
    {
    

        /// <summary>
        /// Gets or sets the Support Download identifier
        /// </summary>
        public int SupportDownloadId { get; set; }

        /// <summary>
        /// Gets or sets the category identifier
        /// </summary>
        public int CategoryId { get; set; }


		  /// <summary>
		  /// Gets the category
		  /// </summary>
		//  public virtual Category Category { get; set; }

		  /// <summary>
		  /// Gets the support download
		  /// </summary>
		  public virtual SupportDownload SupportDownload { get; set; }

    
    }
}
