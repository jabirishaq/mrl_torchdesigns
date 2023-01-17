
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nop.Plugin.TorchDesign.CustomerOrigin;
using Nop.Web.Framework;
using System.ComponentModel.DataAnnotations;

namespace Nop.Plugin.TorchDesign.CustomerOrigin.Models
{
    public partial class ChartModel 
    {

        [NopResourceDisplayName("Admin.Orders.List.StartDate")]
        [UIHint("DateNullable")]
        public DateTime? StartDate { get; set; }

        [NopResourceDisplayName("Admin.Orders.List.EndDate")]
        [UIHint("DateNullable")]
        public DateTime? EndDate { get; set; }

        public string Charatdata { get; set; }

        public bool IsResultfound { get; set; }
      
    }
}