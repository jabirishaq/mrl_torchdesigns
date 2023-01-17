using System;
using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Media;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nop.Plugin.Widgets.TorchDesign_Support.Domain
{
    /// <summary>
	 /// Created By Nick
    /// Represents an Topic Steps
    /// </summary>
	public partial class SupportTopicStep : BaseEntity
    {

        //private ICollection<SupportTopicStepMapping> _supportTopicStepMapping;


        public int SupportTopicId { get; set; }

        public int SupportStepNo { get; set; }
        /// <summary>
        /// Gets or sets the Step title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description 
        /// </summary>
         public string Description { get; set; }

		  /// <summary>
		  /// Gets or sets the picture identifier
		  /// </summary>
		  public int PictureId { get; set; }

         
          public int DisplayOrder { get; set; }



        
		  /// <summary>
		  /// Gets the picture
		  /// </summary>
		 // public virtual Picture Picture { get; set; }

		  /// <summary>
		  /// Gets or sets the collection of SupportCategory
		  /// </summary>
          //public virtual ICollection<SupportTopicStepMapping> SupportTopicStepMapping
          //{
          //    get { return _supportTopicStepMapping ?? (_supportTopicStepMapping = new List<SupportTopicStepMapping>()); }
          //    protected set { _supportTopicStepMapping = value; }
          //}

    
    }
}
