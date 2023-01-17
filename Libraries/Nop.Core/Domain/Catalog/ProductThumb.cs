using System;

namespace Nop.Core.Domain.Catalog
{
    /// <summary>
    /// Represents a back in stock subscription
    /// </summary>
    public partial class ProductThumb : BaseEntity
    {
       
        public int PictureId { get; set; }
        public int ThumbId { get; set; }
        public int ProductId { get; set; }

      
    }

}
