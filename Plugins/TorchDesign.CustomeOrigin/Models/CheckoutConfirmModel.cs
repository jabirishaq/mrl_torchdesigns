using System.Collections.Generic;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.TorchDesign.CustomerOrigin.Models
{
    public partial class CheckoutConfirmModel : BaseNopModel
    {
        public CheckoutConfirmModel()
        {
            Warnings = new List<string>();
            Configrationmodel = new List<ConfigurationModel>();
        }

        public bool TermsOfServiceOnOrderConfirmPage { get; set; }
        public string MinOrderTotalWarning { get; set; }
        public int Selecteditem { get; set; }
        public IList<ConfigurationModel> Configrationmodel { get; set; }
        public IList<string> Warnings { get; set; }
        public int DefaultSelectedOptionId { get; set; }
        //public string Charatdata { get; set; }
        //public bool defaultoption { get; set; }
       // public bool IsResultfound { get; set; }
    }
}