using System.Collections.Generic;
using Nop.Web.Framework.Mvc;

namespace Nop.Web.Models.Catalog
{
    public partial class ProductsByTagModel : BaseNopEntityModel
    {
        public ProductsByTagModel()
        {
            Products = new List<ProductOverviewModel>();
            PagingFilteringContext = new CatalogPagingFilteringModel();
        }

        public string TagName { get; set; }

        public CatalogPagingFilteringModel PagingFilteringContext { get; set; }

        public IList<ProductOverviewModel> Products { get; set; }
    }
}