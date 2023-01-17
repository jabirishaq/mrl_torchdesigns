using Nop.Web.Framework.Mvc;
using System.Collections.Generic;


namespace Nop.Web.Models.Catalog
{
    public partial class ProductSpecificationModel : BaseNopModel
    {
        public ProductSpecificationModel()
        {
           Grouplist = new List<Grouplist>();
        }
        public IList<Grouplist> Grouplist { get; set; }
       
    }

    public class SpecificatioAttribute : BaseNopModel
    {
        public int SpecificationAttributeId { get; set; }

        public string SpecificationAttributeName { get; set; }

        public string SpecificationAttributeOption { get; set; }
    }
    public class  Grouplist : BaseNopModel
    {
        public Grouplist()
        {
            SpecificatioAttribute = new List<SpecificatioAttribute>();
          
        }
        public int Groupid { get; set; }

        public string GroupName { get; set; }
        public IList<SpecificatioAttribute> SpecificatioAttribute { get; set; }
     }
}