using Nop.Core;
using Nop.Core.Domain.Localization;
using System.Collections.Generic;

namespace Nop.Plugin.TorchDesign.FAQ.Domain
{
    /// <summary>
    /// Represents a FAQ record
    /// </summary>
    public partial class FrequentlyAskedQ : BaseEntity, ILocalizedEntity
    {

        private ICollection<FAQ_Category_Mapping> faqcategorysmap;
      
        /// <summary>
        /// Gets or sets the Question
        /// </summary>
        public string question { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// Gets or sets the Category Id
        /// </summary>
       /// public int CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the Active
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Gets or sets the DisplayOrder
        /// /// </summary>
        public int DisplayOrder { get; set; }

        public virtual ICollection<FAQ_Category_Mapping> FAqMapping
        {
            get { return faqcategorysmap ?? (faqcategorysmap = new List<FAQ_Category_Mapping>()); }
            protected set { faqcategorysmap = value; }
        }
    }
}