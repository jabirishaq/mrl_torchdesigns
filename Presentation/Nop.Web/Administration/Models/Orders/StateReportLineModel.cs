using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Models.Orders
{
    public partial class StateReportLineModel : BaseNopModel
    {
        [NopResourceDisplayName("Admin.SalesReport.State.Fields.StateName")]
        public string StateName { get; set; }

        [NopResourceDisplayName("Admin.SalesReport.State.Fields.TotalOrders")]
        public int TotalOrders { get; set; }

        [NopResourceDisplayName("Admin.SalesReport.State.Fields.SumOrders")]
        public string SumOrders { get; set; }
    }
}