
using Nop.Core.Configuration;

namespace Nop.Plugin.TorchDesign.Zip2Tax
{
    public class TorchDesignZip2TaxSettings : ISettings
    {

        public string DatabaseServer { get; set; }

        public string DatabaseUserName { get; set; }

        public string DatabasePassword { get; set; }

        public string DatabaseName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Zip2TaxApiUrl { get; set; }

        public int UpdateTimeStemp { get; set; }

        public string DefaultZipcode { get; set; }

        public decimal DefaultTaxRate { get; set; }

        public string AllowedStateIds { get; set; }

    }
}