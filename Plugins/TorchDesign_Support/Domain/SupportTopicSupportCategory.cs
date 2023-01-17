using System;
using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.Catalog;

namespace Nop.Plugin.Widgets.TorchDesign_Support.Domain
{
    /// <summary>
	 /// Created By Nick
    /// Represents an support category
    /// </summary>
	public partial class SupportTopicSupportCategory : BaseEntity
    {
    

        /// <summary>
		 /// Gets or sets the support category identifier
        /// </summary>
		  public int SupportCategoryId { get; set; }

        /// <summary>
		  /// Gets or sets the support topic identifier
        /// </summary>
		  public int SupportTopicId { get; set; }

		  /// <summary>
		  /// Gets the SupportCategory
		  /// </summary>
		  public virtual SupportCategory SupportCategory { get; set; }

		  /// <summary>
		  /// Gets the support topic
		  /// </summary>
		  public virtual SupportTopic SupportTopic { get; set; } 

    
    }
}
