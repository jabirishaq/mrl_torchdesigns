using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Nop.Plugin.TorchDesigns.DynamicsGPWCF.Models
{
    public partial class ConfigurationModel : BaseNopEntityModel
    {
        public ConfigurationModel()
        {
            AvailableCompanies = new List<SelectListItem>();
            Errors = new List<string>();
        }

        public List<SelectListItem> AvailableCompanies { get; set; }
        public List<string> Errors { get; set; }
        [NopResourceDisplayName("Plugins.TorchDesign.DynamicsGP.SalespersonKeyId")]
        [AllowHtml]
        public string SalespersonKeyId { get; set; }
        public bool SalespersonKeyId_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.TorchDesign.DynamicsGP.SalesTerritoryKeyId")]
        [AllowHtml]
        public string SalesTerritoryKeyId { get; set; }
        public bool SalesTerritoryKeyId_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.TorchDesign.DynamicsGP.TaxScheduleId")]
        public string TaxScheduleId { get; set; }
        public bool TaxScheduleId_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.TorchDesign.DynamicsGP.PaymentCardTypeKeyId")]
        public string PaymentCardTypeKeyId { get; set; }
        public bool PaymentCardTypeKeyId_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.TorchDesign.DynamicsGP.CurrencyKeyISOCode")]
        public string CurrencyKeyISOCode { get; set; }
        public bool CurrencyKeyISOCode_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.TorchDesign.DynamicsGP.WebServiceAddress")]
        public string WebServiceAddress { get; set; }
        public bool WebServiceAddress_OverrideForStore { get; set; }


        [NopResourceDisplayName("Plugins.TorchDesign.DynamicsGP.UserName")]
        public string UserName { get; set; }
        public bool UserName_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.TorchDesign.DynamicsGP.Password")]
        public string Password { get; set; }
        public bool Password_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.TorchDesign.DynamicsGP.PONumberPrefix")]
        public string PONumberPrefix { get; set; }
        public bool PONumberPrefix_OverrideForStore { get; set; }


        [NopResourceDisplayName("Plugins.TorchDesign.DynamicsGP.Domain")]
        public string Domain { get; set; }
        public bool Domain_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.TorchDesign.DynamicsGP.CompanyKey")]
        public string CompanyKey { get; set; }
        public bool CompanyKey_OverrideForStore { get; set; }

        public bool Status { get; set; }

        //[NopResourceDisplayName("Plugins.TorchDesign.DynamicsGP.TaxScheduleIdForCustomersOutsideFlorida")]
        //public string TaxScheduleIdForCustomersOutsideFlorida { get; set; }
        //public bool TaxScheduleIdForCustomersOutsideFlorida_OverrideForStore { get; set; }

    }
}
