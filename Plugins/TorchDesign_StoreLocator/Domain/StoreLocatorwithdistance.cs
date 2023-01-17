using Nop.Core;
using Nop.Core.Domain.Localization;

namespace Nop.Plugin.Widgets.TorchDesign_StoreLocator.Domain
{
    /// <summary>
    /// Represents a FAQ Categories
    /// </summary>
    public partial class StoreLocatorwithdistance : BaseEntity, ILocalizedEntity
    {
        /// <summary>
        /// Gets or sets the Store Location
        /// </summary>
        //public int StoreNumber { get; set; }
        public string StoreName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string CountryCode { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string StoreType { get; set; } 
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal ditance { get; set; }
      
    }
}