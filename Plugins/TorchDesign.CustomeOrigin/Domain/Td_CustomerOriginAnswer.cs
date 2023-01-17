using Nop.Core;
using Nop.Core.Domain.Localization;
using System;

namespace Nop.Plugin.TorchDesign.CustomerOrigin.Domain
{
    /// <summary>
    /// Represents a FAQ record
    /// </summary>
    public partial class Td_CustomerOriginAnswer : BaseEntity, ILocalizedEntity
    {
        public int Optionid { get; set; }
        public int CustomerId { get; set; }
        public DateTime GivenOn { get; set; }
       
    }
}