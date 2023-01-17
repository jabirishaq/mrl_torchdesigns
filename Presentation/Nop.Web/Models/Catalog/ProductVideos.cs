using System.Collections.Generic;
using Nop.Web.Framework.Mvc;
using Nop.Web.Models.Media;

namespace Nop.Web.Models.Catalog
{
    public partial class ProductVideos : BaseNopEntityModel
    {
        public ProductVideos()
        {
            Videolist = new List<Videolist>();
        }
        public IList<Videolist> Videolist { get; set; }

    }

    public class Videolist : BaseNopModel
    {
        public int Id { get; set; }
        public int PictureId { get; set; }
        public string PictureUrl { get; set; }
        public string VideoUrl { get; set; }

    }
}