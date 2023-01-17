using System;
using Nop.Web.Framework;
using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Localization;
using System.Web.Mvc;
using System.Collections.Generic;

namespace Nop.Plugin.TorchDesign.Zip2Tax.Models
{
    public partial class ConfigurationModel : BaseNopEntityModel
    {
        //public ConfigurationModel()
        //{
        //    Locales = new List<FAQLangLocalizedModel>();
        //    //AvailableOptions = new List<SelectListItem>();
        //    AvailableZones = new List<SelectListItem>();
        //    AvailableCategories = new List<SelectListItem>();

        //}

        [NopResourceDisplayName("Plugins.TorchDesign.Zip2Tax.DatabaseServer")]
        [AllowHtml]
        public string DatabaseServer { get; set; }
        public bool DatabaseServer_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.TorchDesign.Zip2Tax.DatabaseUserName")]
        [AllowHtml]
        public string DatabaseUserName { get; set; }
        public bool DatabaseUserName_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.TorchDesign.Zip2Tax.DatabasePassword")]
        [AllowHtml]
        public string DatabasePassword { get; set; }
        public bool DatabasePassword_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.TorchDesign.Zip2Tax.DatabaseName")]
        [AllowHtml]
        public string DatabaseName { get; set; }
        public bool DatabaseName_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.TorchDesign.Zip2Tax.UserName")]
        [AllowHtml]
        public string UserName { get; set; }
        public bool UserName_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.TorchDesign.Zip2Tax.Password")]
        [AllowHtml]
        public string Password { get; set; }
        public bool Password_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.TorchDesign.Zip2Tax.Url")]
        [AllowHtml]
        public string Zip2TaxApiUrl { get; set; }
        public bool Zip2TaxApiUrl_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.TorchDesign.Zip2Tax.UpdateTimeStamp")]
        [AllowHtml]
        public int UpdateTimeStemp { get; set; }
        public bool UpdateTimeStemp_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.TorchDesign.Zip2Tax.DefaultZipcode")]
        [AllowHtml]
        public string DefaultZipcode { get; set; }
        public bool DefaultZipcode_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.TorchDesign.Zip2Tax.DefaultTaxRate")]
        [AllowHtml]
        public decimal DefaultTaxRate { get; set; }
        public bool DefaultTaxRate_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.TorchDesign.Zip2Tax.AllowedStateIds")]
        [AllowHtml]
        public string AllowedStateIds { get; set; }
        public bool AllowedStateIds_OverrideForStore { get; set; }

        public bool IsTaxFound { get; set; }

        [NopResourceDisplayName("Plugins.TorchDesign.Zip2Tax.Taxcategoryid")]
        public int Taxcategoryid { get; set; }

        [NopResourceDisplayName("Plugins.TorchDesign.Zip2Tax.ZipcodeText")]
        public string ZipcodeText { get; set; }

        [NopResourceDisplayName("Plugins.TorchDesign.Zip2Tax.TaxRate")]
        public string TaxRate { get; set; }

        [NopResourceDisplayName("Plugins.TorchDesign.Zip2Tax.CreateOn")]
        public DateTime CreateOn { get; set; }

        [NopResourceDisplayName("Plugins.TorchDesign.Zip2Tax.ModifyOn")]
        public DateTime ModifyOn { get; set; }

        public string CreateOnstring { get; set; }
        public string ModifyOnstring { get; set; }
        public string Msglable { get; set; }
      

        //public IList<SelectListItem> AvailableZones { get; set; }
        //[NopResourceDisplayName("Plugins.Widgets.FAQ.DisplayOn")]
        //[AllowHtml]
        //public string DisplayOn { get; set; }
        //public bool DisplayOn_OverrideForStore { get; set; }



        //public IList<FAQLangLocalizedModel> Locales { get; set; }

    }

    //public partial class Zip2TaxCounterModel : ILocalizedModelLocal
    //{



    //    [NopResourceDisplayName("Plugins.TorchDesign.Zip2Tax.Zipcode")]
    //    public string Zipcode { get; set; }

    //    [NopResourceDisplayName("Plugins.TorchDesign.Zip2Tax.TaxRate")]
    //    public string TaxRate { get; set; }

    //    [NopResourceDisplayName("Plugins.TorchDesign.Zip2Tax.CallDate")]
    //    public DateTime CallDate { get; set; }

    //    public string CallDatestring { get; set; }




    //}
}

