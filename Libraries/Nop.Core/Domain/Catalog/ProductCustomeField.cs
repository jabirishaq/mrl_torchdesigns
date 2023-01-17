using System;
using System.Collections.Generic;
using Nop.Core.Domain.Discounts;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Seo;
using Nop.Core.Domain.Stores;
using Nop.Core.Domain.Media;

namespace Nop.Core.Domain.Catalog
{
    /// <summary>
    /// Represents a product
    /// </summary>
    public partial class ProductCustomeField : BaseEntity
    {
        
        private ICollection<Product> _products;
        
        /// <summary>
        /// Gets or sets the product type identifier
        /// </summary>
        public int ProductId { get; set; }


        /// <summary>
        /// Gets or sets a value of MaximumRuns
        /// </summary>
        public int RunId { get; set; }


        /// <summary>
        /// Gets or sets a value indicating Product Is made in Usa Or Not
        /// </summary>
        public bool MadeInUsa { get; set; }

        /// <summary>
        /// Gets or sets the product Quantity
        /// </summary>
        public string Quantity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is enable for free for flate rate enabled
        /// </summary>
        public bool IsEligibleforFree { get; set; }



    }
}