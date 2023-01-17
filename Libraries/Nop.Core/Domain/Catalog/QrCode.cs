
using Nop.Core.Domain.Media;
using System;

namespace Nop.Core.Domain.Catalog
{
    /// <summary>
    /// Represents a product picture mapping
    /// </summary>
    public partial class QrCode : BaseEntity
    {
        /// <summary>
        /// Gets or sets the QrCode identifier
        /// </summary>
        public string QrCodeName { get; set; }

        public string QrCodeUrl { get; set; }

        /// <summary>
        /// Gets or sets the Date
        /// </summary>
        public DateTime Date { get; set; }

        
    }

}
