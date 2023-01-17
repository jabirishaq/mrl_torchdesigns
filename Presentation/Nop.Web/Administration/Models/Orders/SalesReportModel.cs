using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Models.Orders
{
    public partial class SalesReportModel : BaseNopModel
    {
        public SalesReportModel()
        {
            AvailableOrderStatuses = new List<SelectListItem>();
            AvailablePaymentStatuses = new List<SelectListItem>();
          
        }

        [NopResourceDisplayName("Admin.SalesReport.SalesReport.StartDate")]
        [UIHint("DateNullable")]
        public DateTime? StartDate { get; set; }

        [NopResourceDisplayName("Admin.SalesReport.SalesReport.EndDate")]
        [UIHint("DateNullable")]
        public DateTime? EndDate { get; set; }
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int? TotalCount { get; set; }


        [NopResourceDisplayName("Admin.SalesReport.SalesReport.OrderStatus")]
        public int OrderStatusId { get; set; }
        [NopResourceDisplayName("Admin.SalesReport.SalesReport.PaymentStatus")]
        public int PaymentStatusId { get; set; }

        public IList<SelectListItem> AvailableOrderStatuses { get; set; }
        public IList<SelectListItem> AvailablePaymentStatuses { get; set; }

        
    }
}