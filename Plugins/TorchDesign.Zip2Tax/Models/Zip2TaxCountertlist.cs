using System;
using Nop.Web.Framework;
using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Localization;
using System.Web.Mvc;
using System.Collections.Generic;

namespace Nop.Plugin.TorchDesign.Zip2Tax.Models
{
    public partial class Zip2TaxCountertlist : BaseNopEntityModel
    {
        [NopResourceDisplayName("Plugins.TorchDesign.Zip2Tax.Zipcode")]
        public string Zipcode { get; set; }

        [NopResourceDisplayName("Plugins.TorchDesign.Zip2Tax.TaxRate")]
        public string TaxRate { get; set; }

        [NopResourceDisplayName("Plugins.TorchDesign.Zip2Tax.CallDate")]
        public DateTime CallDate { get; set; }

        public string CallDatestring { get; set; }

    }
}

