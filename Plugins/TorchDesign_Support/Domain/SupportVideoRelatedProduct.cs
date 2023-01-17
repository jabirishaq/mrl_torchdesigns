using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.Catalog;

namespace Nop.Plugin.Widgets.TorchDesign_Support.Domain
{
	public partial class SupportVideoRelatedProduct : BaseEntity
	{
		/// <summary>
		/// Gets or sets the support  Topic identifier
		/// </summary>
		public int SupportVideoId { get; set; }

		/// <summary>
		/// Gets or sets the product identifier
		/// </summary>
		public int ProductId { get; set; }

		/// <summary>
		/// Gets or sets the display order
		/// </summary>
		public int DisplayOrder { get; set; }


		/// <summary>
		/// Gets the category
		/// </summary>
		public virtual SupportVideo SupportedVideos { get; set; }


	}
}
