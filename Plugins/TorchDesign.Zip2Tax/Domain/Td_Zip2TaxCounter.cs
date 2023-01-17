using Nop.Core;
using Nop.Core.Domain.Localization;
using System;

namespace Nop.Plugin.TorchDesign.Zip2Tax.Domain
{
    /// <summary>
    /// Represents a FAQ record
    /// </summary>
    public partial class Td_Zip2TaxCounter : BaseEntity, ILocalizedEntity
    {


        /// <summary>
        /// Gets or sets the TaxRate
        /// </summary>
        public string TaxRate { get; set; }

        /// <summary>
        /// Gets or sets the Zipcode
        /// </summary>
        public string Zipcode { get; set; }

        /// <summary>
        /// Gets or sets the CallDate
        /// </summary>
        public DateTime CallDate { get; set; }

        ///<summary>
        /// Gets or sets the Flag for wrong zipcode
        /// </summary>
        public bool IsValideZipcode { get; set; }

    }
}