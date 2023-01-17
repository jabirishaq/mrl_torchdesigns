using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Localization;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Widgets.TorchDesign_Support.Models
{
    
    public partial class SupportTopicStepModel : BaseNopEntityModel
    {
		 public SupportTopicStepModel()
        {

        }
         public int SupportStepNo { get; set; }
		

        [NopResourceDisplayName("Admin.Support.SupportTopicStep.Fields.Name")]
        [AllowHtml]
        public string Title { get; set; }

		  [NopResourceDisplayName("Admin.Support.SupportTopicStep.Fields.Description")]
          //[RegularExpression("^[^<>,<|>]+$", ErrorMessage = "Html tags are not allowed.")]
          [AllowHtml]
        public string Description { get; set; }

		  [UIHint("SupportPicture")]
		  [NopResourceDisplayName("Admin.Support.SupportTopicStep.Fields.Picture")]
		  public int PictureId { get; set; }

		  [NopResourceDisplayName("Admin.Support.SupportTopicStep.Fields.Picture")]
		  public string PictureThumbnailUrl { get; set; }



          [NopResourceDisplayName("Admin.Support.SupportTopicStep.Fields.Displayorder")]
          public int DisplayOrder { get; set; }

		

       
    }

   
}