using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Nop.Web.Framework.Localization;

namespace Nop.Plugin.TorchDesign.FAQ.Models
{
    public class QandAModel : BaseNopEntityModel 
    {

        public string question { get; set; }

        [AllowHtml]
        public string description { get; set; }
        

     
    }
}
