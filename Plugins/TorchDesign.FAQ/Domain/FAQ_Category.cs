using Nop.Core;
using Nop.Core.Domain.Localization;

namespace Nop.Plugin.TorchDesign.FAQ.Domain
{
    /// <summary>
    /// Represents a FAQ Categories
    /// </summary>
    public partial class FAQ_Category : BaseEntity, ILocalizedEntity
    {


      
        /// <summary>
        /// Gets or sets the Question
        /// </summary>
        public string CategoryName { get; set; }

         public bool Active { get; set; }

         public int DisplayOrder { get; set; }
      
    }
}