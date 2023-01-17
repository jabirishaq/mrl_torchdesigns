
using Nop.Core.Domain.Media;

namespace Nop.Core.Domain.Catalog
{
    /// <summary>
    /// Represents a product picture mapping
    /// </summary>
    public partial class ProductInBox : BaseEntity
    {
        /// <summary>
        /// Gets or sets the ParentProductId identifier
        /// </summary>
        public int ParentProductId { get; set; }

        /// <summary>
        /// Gets or sets the InboxProductId identifier
        /// </summary>
        public int InboxProductId { get; set; }


        /// <summary>
        /// Gets or sets the Title identifier
        /// </summary>
        public string Title { get; set; }

        ///// <summary>
        ///// Gets or sets the display order
        ///// </summary>
        ///// 
        public int Displayorder { get; set; }
        public int Quantity { get; set; }
        
    }

}
