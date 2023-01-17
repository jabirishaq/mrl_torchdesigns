using Nop.Core;
using System;
using Nop.Core.Domain.Localization;
 
namespace Nop.Plugin.TorchDesign.CustomerOrigin.Domain
{
   
    public partial class Td_CustomerOrigin : BaseEntity, ILocalizedEntity
    {
        public string OptionName { get; set; }
        public bool DefaultSelected { get; set; }
        public bool Publish { get; set; }
         
 
    }
}
