using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Models.Catalog
{
    public partial class AddProductInBoxSimpleModel : BaseNopEntityModel
    {
        public int Parentid { get; set; }

        [NopResourceDisplayName("admin.orders.products.quantity")]
        public int Quantity { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Products.AssociatedProducts.Fields.Product")]
        public string Title { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Products.AssociatedProducts.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; } 

        public bool quantityerro { get; set; }

        public bool titleerro { get; set; }
    }
}