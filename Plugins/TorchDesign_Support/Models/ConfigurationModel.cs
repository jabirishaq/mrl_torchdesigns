using System;
using Nop.Web.Framework;
using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Localization;
using System.Web.Mvc;
using System.Collections.Generic;

namespace Nop.Plugin.Widgets.TorchDesign_Support.Models
{
	public partial class ConfigurationModel : BaseNopEntityModel
	{
        public int ActiveStoreScopeConfiguration { get; set; }
        [NopResourceDisplayName("Admin.Configuration.Settings.Catalog.SupportEnabled")]
        public bool SupportEnabled { get; set; }
        public bool SupportEnabled_OverrideForStore { get; set; }
		public ConfigurationModel()
		{



		}
	}

}

