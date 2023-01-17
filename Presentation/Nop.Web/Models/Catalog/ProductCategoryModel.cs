using System.Collections.Generic;
using Nop.Web.Framework.Mvc;
using Nop.Web.Models.Media;

namespace Nop.Web.Models.Catalog
{
    public partial class ProductCategoryModel : BaseNopEntityModel
    {
        public ProductCategoryModel()
        {
            ProductCategories = new List<productClickCategoryModel>();
        }

        public IList<productClickCategoryModel> ProductCategories { get; set; }

        public partial class productClickCategoryModel : BaseNopEntityModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string CategoryImageurl { get; set; }
            //public string MetaKeywords { get; set; }
            //public string MetaDescription { get; set; }
            //public string MetaTitle { get; set; }
            public string SeName { get; set; }
        }
       
    }
}