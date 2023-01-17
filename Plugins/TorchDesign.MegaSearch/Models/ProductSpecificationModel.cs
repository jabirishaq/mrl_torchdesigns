using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.TorchDesign.MegaSearch.Models
{
    public partial class ProductSpecificationModel : BaseNopModel
    {
        public int SpecificationAttributeId { get; set; }

        public string SpecificationAttributeName { get; set; }

        public string SpecificationAttributeOption { get; set; }
    }
}