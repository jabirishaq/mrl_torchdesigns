using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.Collections.Generic;

namespace Nop.Admin.Models.Catalog
{
    public partial class EditProductSpecificationAttributeModel : BaseNopEntityModel
    {
        public EditProductSpecificationAttributeModel()
        {
            
            AvailableSpecificationGroup = new List<SelectListItem>();
            AvailableAttribute = new List<SelectListItem>();
            AvailableAttributeOption = new List<SelectListItem>();
          
           
        }

       
        
       

        [NopResourceDisplayName("Admin.Catalog.Products.SpecificationAttributes.Fields.SpecificationAttribute")]
        [AllowHtml]
        public int SpecificationAttributeId { get; set; }
        public IList<SelectListItem> AvailableAttribute { get; set; }
        

        [NopResourceDisplayName("Admin.Catalog.Products.SpecificationAttributes.Fields.SpecificationAttributeOption")]
        [AllowHtml]
        public int SpecificationAttributeOptionId { get; set; }
        public IList<SelectListItem> AvailableAttributeOption { get; set; }
              
        [NopResourceDisplayName("Admin.Catalog.Products.SpecificationAttributes.Fields.SpecificationGroup")]
        [AllowHtml]
        public int SpecificationAttributeGroupId { get; set; }
        public IList<SelectListItem> AvailableSpecificationGroup { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Products.SpecificationAttributes.Fields.CustomValue")]
        [AllowHtml]
        public string CustomValue { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Products.SpecificationAttributes.Fields.AllowFiltering")]
        public bool AllowFiltering { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Products.SpecificationAttributes.Fields.ShowOnProductPage")]
        public bool ShowOnProductPage { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Products.SpecificationAttributes.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public int SpecificationAttributeGroupName { get; set; }
    }
}