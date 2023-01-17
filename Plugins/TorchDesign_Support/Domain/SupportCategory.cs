using System;
using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Seo;

namespace Nop.Plugin.Widgets.TorchDesign_Support.Domain
{
    /// <summary>
	 /// Created By Nick
    /// Represents an Support category and  Topic 	  Mapping
    /// </summary>
    public partial class SupportCategory : BaseEntity, ISlugSupported
    {
        private ICollection<SupportCategoryProductCategory> _supportTopicProductCategories;
		private ICollection<SupportTopicSupportCategory> _supportTopicSupportCategory;

		 /// <summary>
		 /// Gets or sets the title
		 /// </summary>
		 public string Title { get; set; }

		 /// <summary>
		 /// Gets or sets the  description
		 /// </summary>
		 public string Description { get; set; }
		 
		 /// <summary>
		 /// Gets or sets the  description
		 /// </summary> 
		 public bool ShowInFooter { get; set; }


         public bool IsActive { get; set; }

		 /// <summary>
		 /// Gets or sets the picture identifier
		 /// </summary>
		 public int PictureId { get; set; }

		 /// <summary>
		 /// Gets the picture
		 /// </summary>
		 //public virtual Picture Picture { get; set; }

		 /// <summary>
		 /// Gets or sets the collection of ProductManufacturer
		 /// </summary>
		 public virtual ICollection<SupportTopicSupportCategory> SupportTopicSupportCategory
		 {
			 get { return _supportTopicSupportCategory ?? (_supportTopicSupportCategory = new List<SupportTopicSupportCategory>()); }
			 protected set { _supportTopicSupportCategory = value; }
		 }

         /// <summary>
         /// Gets or sets the collection of ProductCategory
         /// </summary>
         public virtual ICollection<SupportCategoryProductCategory> SupportCategoryProductCategories
         {
             get { return _supportTopicProductCategories ?? (_supportTopicProductCategories = new List<SupportCategoryProductCategory>()); }
             protected set { _supportTopicProductCategories = value; }
         }
        
    }
}
