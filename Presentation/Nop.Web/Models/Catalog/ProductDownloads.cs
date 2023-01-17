using System.Collections.Generic;
using Nop.Web.Framework.Mvc;
using Nop.Web.Models.Media;

namespace Nop.Web.Models.Catalog
{
    public partial class ProductDownloads : BaseNopEntityModel
    {
        public ProductDownloads()
        {
            Downloadlist = new List<Downloadlist>();
        }
        public IList<Downloadlist> Downloadlist { get; set; }
        public int ProductId { get; set; }

    }

    public class Downloadlist : BaseNopModel
    {
        public int Id { get; set; }
        public int DownloadId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

    }
}