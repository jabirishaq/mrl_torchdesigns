using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.Catalog;

namespace Nop.Plugin.Widgets.TorchDesign_Support.Domain
{
	public partial class SupportVideoProductCategory : BaseEntity
	{
		/// <summary>
		/// Gets or sets Video ID
		/// </summary>
		public int SupportVideoId { get; set; }

		/// <summary>
		/// Gets or sets the category identifier
		/// </summary>
		public int CategoryId { get; set; }

		/// <summary>
		/// Gets the Support Video
		/// </summary>
		public virtual SupportVideo SupportVideo { get; set; }

	}
}
