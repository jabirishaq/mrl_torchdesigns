using Nop.Core;
using System;
using Nop.Core.Domain.Localization;
 
namespace Nop.Plugin.TorchDesign.Zip2Tax.Domain
{
    /// <summary>
    /// Represents a FAQ Categories
    /// </summary>
    public partial class Td_Zip2Tax : BaseEntity, ILocalizedEntity
    {
 
        /// <summary>
        /// Gets or sets the TaxRate
        /// </summary>
        public int Taxcategoryid { get; set; }
        public string Zipcode { get; set; }
        public string TaxRate { get; set; }
        public string County { get; set; }
        public DateTime CreateOn { get; set; }
        public DateTime ModifyOn { get; set; }
     
 
    }
}
