using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Models.HomepageImage
{
    public class ConfigurationModel : BaseNopModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }


        [NopResourceDisplayName("Admin.picture")]
        [UIHint("Picturewithoutremove")]
        public int Picture1Id { get; set; }
        public bool Picture1Id_OverrideForStore { get; set; }
        //[NopResourceDisplayName("Plugins.Widgets.NivoSlider.Text")]
        //[AllowHtml]
        //public string Text1 { get; set; }
        //public bool Text1_OverrideForStore { get; set; }
        [NopResourceDisplayName("Admin.picture.link")]
        [AllowHtml]
        public string Link1 { get; set; }
        public bool Link1_OverrideForStore { get; set; }

        [NopResourceDisplayName("Admin.picture")]
        [UIHint("Picturewithoutremove")]
        public int Picture2Id { get; set; }
        public bool Picture2Id_OverrideForStore { get; set; }
        //[NopResourceDisplayName("Plugins.Widgets.NivoSlider.Text")]
        //[AllowHtml]
        //public string Text2 { get; set; }
        //public bool Text2_OverrideForStore { get; set; }
        [NopResourceDisplayName("Admin.picture.link")]
        [AllowHtml]
        public string Link2 { get; set; }
        public bool Link2_OverrideForStore { get; set; }

     
    }
}