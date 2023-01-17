using System;
using Nop.Web.Framework;
using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Localization;
using System.Web.Mvc;
using System.Collections.Generic;

namespace Nop.Plugin.TorchDesign.CustomerOrigin.Models
{
    public partial class ConfigurationModel : BaseNopEntityModel
    {
        public string OptionName { get; set; }

        public bool DefaultSelected { get; set; }

        public bool Publish { get; set; }
   }

}

