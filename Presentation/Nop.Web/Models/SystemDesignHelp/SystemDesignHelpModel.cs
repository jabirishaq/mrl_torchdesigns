using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using Nop.Web.Validators.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Nop.Web.Models.SystemDesignHelp 
{
    [Validator(typeof(SystemDesignHelpValidator))]
    public partial class SystemDesignHelpModel : BaseNopModel
    {
        public SystemDesignHelpModel()
        {
            this.AvailableStates = new List<SelectListItem>();
            this.AvailableSoilType = new List<SelectListItem>();
            this.AvailableCityWell = new List<SelectListItem>();
            //this.AvailableWaterPressure = new List<SelectListItem>();
            this.AvailableHaveFaucets = new List<SelectListItem>();
            this.AvailableUseFaucets = new List<SelectListItem>();
            this.AvailableDrippers = new List<SelectListItem>();
        }

        //[NopResourceDisplayName("Plugins.Widgets.Event.PictureId")]
        //[UIHint("Download")]
        //public int PictureId { get; set; }
        //public HttpPostedFileBase PictureId { get; set; }

       

        [NopResourceDisplayName("account.fields.firstname")]
        [AllowHtml]
        public string FirstName { get; set; }

        [NopResourceDisplayName("account.fields.lastname")]
        [AllowHtml]
        public string LastName { get; set; }

        [NopResourceDisplayName("account.fields.email")]
        [AllowHtml]
        public string Email { get; set; }

        public string PSI { get; set; }
        public string Second { get; set; }

        public string StateId { get; set; }
        public IList<SelectListItem> AvailableStates { get; set; }

        public string SoilTypeId { get; set; }
        public IList<SelectListItem> AvailableSoilType { get; set; }

        public string CityWellId { get; set; }
        public IList<SelectListItem> AvailableCityWell { get; set; }

        //public string WaterPressureId { get; set; }
        //public IList<SelectListItem> AvailableWaterPressure { get; set; }

        public string HaveFaucetsId { get; set; } 
        public IList<SelectListItem> AvailableHaveFaucets { get; set; }

        public string UseFaucetsId { get; set; }
        public IList<SelectListItem> AvailableUseFaucets { get; set; }

        public string DrippersId { get; set; }
        public IList<SelectListItem> AvailableDrippers { get; set; } 
    }
}