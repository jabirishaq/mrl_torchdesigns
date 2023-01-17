using System;

namespace Nop.Core.Domain.Catalog
{
    /// <summary>
    /// Represents a best sellers report line
    /// </summary>
    [Serializable]
    public partial class QrCodeDiaplay
    {
        /// <summary>
        /// Gets or sets the QrCode identifier
        /// </summary>
        public int id { get; set; }
        public string QrCodeName { get; set; }
        public string QrCodeUrl { get; set; }
        /// <summary>
        /// Gets or sets the Date
        /// </summary>
        public DateTime Date { get; set; }

        public int Count { get; set; }

    }
}
