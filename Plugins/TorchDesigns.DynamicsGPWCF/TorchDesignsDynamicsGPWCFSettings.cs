
using Nop.Core.Configuration;

namespace Nop.Plugin.TorchDesigns.DynamicsGPWCF
{
    public class TorchDesignsDynamicsGPWCFSettings : ISettings
    {
        //public string Title { get; set; }
     
        //public int DisplayOption { get; set; }

        //public string DisplayOn { get; set; }

        public string SalespersonKeyId { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }

        public string PONumberPrefix { get; set; }

        public string Domain { get; set; }


        public string SalesTerritoryKeyId { get; set; }

        public string TaxScheduleId { get; set; }

        //public string CustomerPONumber { get; set; }

        public string CurrencyKeyISOCode { get; set; }

        public string PaymentCardTypeKeyId { get; set; }

        public string CompanyKey { get; set; }

        public string WebServiceAddress { get; set; }

        //public string TaxScheduleIdForCustomersOutsideFlorida { get; set; }
    }
}