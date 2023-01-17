using System.Collections.Generic;
using Nop.Web.Framework.Mvc;
using Nop.Web.Models.Media;

namespace Nop.Web.Models.Catalog
{
    public partial class ProductIntoBox : BaseNopEntityModel
    {
        public ProductIntoBox()
        {
            Productlistinbox = new List<Productlistinbox>();
        }
        public IList<Productlistinbox> Productlistinbox { get; set; }
      
    }

    public class Productlistinbox : BaseNopModel
    {
       public int Id { get; set; }
        public string Sename { get; set; }
        public string Title { get; set; }
        public int Type { get; set; }
        public string Partno { get; set; }
        public int Quantity { get; set; }

    }
}