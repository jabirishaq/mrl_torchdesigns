using System;
using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.Catalog;

namespace Nop.Plugin.Widgets.TorchDesign_Support.Domain
{
    /// <summary>
	 /// Created By Nick
    /// Represents an Topic and Product Category mapping
    /// </summary>
    public partial class SupportTopicProductCategory : BaseEntity
    {
    

        /// <summary>
        /// Gets or sets the Support Topic identifier
        /// </summary>
        public int SupportTopicId { get; set; }

        /// <summary>
        /// Gets or sets the category identifier
        /// </summary>
        public int CategoryId { get; set; }


		  /// <summary>
		  /// Gets the category
		  /// </summary>
		  //public virtual Category Category { get; set; }

		  /// <summary>
		  /// Gets the support topic
		  /// </summary>
		  public virtual SupportTopic SupportTopic { get; set; }

    
    }
}
