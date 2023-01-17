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
    public partial class SupportTopic : BaseEntity, ISlugSupported
    {
		private ICollection<SupportTopicProductCategory> _supportTopicProductCategories;
		private ICollection<SupportTopicSupportCategory> _supportTopicSupportCategory;
        //private ICollection<SupportTopicStepMapping> _supportTopicStepMapping;

        /// <summary>
        /// Gets or sets the title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the  description
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Gets or sets the template id
        /// </summary>
        public int TemplateId { get; set; }

		  /// <summary>
		  /// Gets or sets the collection of ProductCategory
		  /// </summary>
		  public virtual ICollection<SupportTopicProductCategory> SupportTopicProductCategories
		  {
			  get { return _supportTopicProductCategories ?? (_supportTopicProductCategories = new List<SupportTopicProductCategory>()); }
			  protected set { _supportTopicProductCategories = value; }
		  }

		  /// <summary>
		  /// Gets or sets the collection of SupportCategory
		  /// </summary>
		  public virtual ICollection<SupportTopicSupportCategory> SupportTopicSupportCategory
		  {
			  get { return _supportTopicSupportCategory ?? (_supportTopicSupportCategory = new List<SupportTopicSupportCategory>()); }
			  protected set { _supportTopicSupportCategory = value; }
		  }

          ///// <summary>
          ///// Gets or sets the collection of SupportCategory
          ///// </summary>
          //public virtual ICollection<SupportTopicStepMapping> SupportTopicStepMapping
          //{
          //    get { return _supportTopicStepMapping ?? (_supportTopicStepMapping = new List<SupportTopicStepMapping>()); }
          //    protected set { _supportTopicStepMapping = value; }
          //}

    }
}
