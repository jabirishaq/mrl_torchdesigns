using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Models.Orders
{
    public partial class SalesReportLineModel : BaseNopModel
    {
        public int ProductId { get; set; }
       
        public string ProductName { get; set; }

        public int? TotalProduct { get; set; }
    }
}