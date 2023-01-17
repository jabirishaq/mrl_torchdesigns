using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;

namespace Nop.Plugin.Widgets.TorchDesign_Support.Domain
{
	/// <summary>
	/// Created By PPIS
	/// Represents Videos
	/// </summary>
	public partial class SupportVideo : BaseEntity
	{
		private ICollection<SupportVideoProductCategory> _supportVideoProductCategories;
		private ICollection<SupportVideoRelatedProduct> _supportVideosRelatedProducts;

		/// <summary>
		/// Gets or sets the title
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// Gets or sets the PictureId
		/// </summary>
		public int PictureId { get; set; }

		/// <summary>
		/// Gets or sets the Vimeo
		/// </summary>
		public string VimeoInformation { get; set; }

        /// <summary>
        /// Gets or sets the Descriptio
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the tag
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Gets or sets the Descriptio
        /// </summary>
        public int DisplayOrder { get; set; }
		public virtual ICollection<SupportVideoProductCategory> SupportVideoProductCategories
		{
			get { return _supportVideoProductCategories ?? (_supportVideoProductCategories = new List<SupportVideoProductCategory>()); }
			protected set { _supportVideoProductCategories = value; }
		}

		public virtual ICollection<SupportVideoRelatedProduct> SupportVideosRelatedProducts
		{
			get { return _supportVideosRelatedProducts ?? (_supportVideosRelatedProducts = new List<SupportVideoRelatedProduct>()); }
			protected set { _supportVideosRelatedProducts = value; }
		}

		//public virtual ICollection<SupportVideoRelatedProduct> SupportVideoSupportCategory
		//{
		//	get { return _supportVideoRelatedProduct ?? (_supportVideoRelatedProduct = new List<SupportVideoRelatedProduct>()); }
		//	protected set { _supportVideoRelatedProduct = value; }
		//}



	}
}
