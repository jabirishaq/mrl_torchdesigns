using System;
using Nop.Web.Framework;
using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Localization;
using System.Web.Mvc;
using System.Collections.Generic;

namespace Nop.Plugin.TorchDesign.Zip2Tax.Models
{
    public partial class CounterModel : BaseNopEntityModel
    {

        [NopResourceDisplayName("Admin.Orders.List.StartDate")]
        [UIHint("DateNullable")]
        public DateTime? StartDate { get; set; }

        [NopResourceDisplayName("Admin.Orders.List.EndDate")]
        [UIHint("DateNullable")]
        public DateTime? EndDate { get; set; }

        [NopResourceDisplayName("Plugins.TorchDesign.Zip2Tax.Zipcode")]
        public string Zipcode { get; set; }

        [NopResourceDisplayName("Plugins.TorchDesign.Zip2Tax.TaxRate")]
        public string TaxRate { get; set; }

        [NopResourceDisplayName("Plugins.TorchDesign.Zip2Tax.CallDate")]
        public DateTime CallDate { get; set; }

        public string CallDatestring { get; set; }


    }
    public partial class CalltotalModel : BaseNopEntityModel
    {
        //aggergator properties
        public string Totalcall { get; set; }

    }
}

