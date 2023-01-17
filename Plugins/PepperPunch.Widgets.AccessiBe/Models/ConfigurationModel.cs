using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace PepperPunch.Widgets.AccessiBe.Models
{
    public class ConfigurationModel : BaseNopModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.AccessiBe.Fields.Enabled")]
        public bool Enabled { get; set; }
        public bool Enabled_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.AccessiBe.Fields.Script")]
        [AllowHtml]
        public string Script { get; set; }
        public bool Script_OverrideForStore { get; set; }

        public string Url { get; set; }

    }
}