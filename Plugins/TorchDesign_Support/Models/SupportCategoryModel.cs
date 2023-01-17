using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Localization;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Widgets.TorchDesign_Support.Models
{
    
    public partial class SupportCategoryModel : BaseNopEntityModel
    {
		 public SupportCategoryModel()
        {

            AvailableCategories = new List<SelectListItem>();
			  AvailableSupportCategories = new List<SelectListItem>();
        }

		
		  [Required]
        [NopResourceDisplayName("Admin.Support.SupportCategories.Fields.Name")]
        [AllowHtml]
        public string Title { get; set; }

		  [NopResourceDisplayName("Admin.Support.SupportCategories.Fields.Description")]
        [AllowHtml]
        public string Description { get; set; }
        
		  [NopResourceDisplayName("Admin.Support.SupportCategories.Fields.ShowInFooter")]
        public bool ShowInFooter { get; set; }

		 
         
          [NopResourceDisplayName("Admin.Support.SupportCategories.Fields.Picture")]
          [UIHint("Picture")]
          public int PictureId { get; set; }

         [NopResourceDisplayName("Admin.Support.SupportCategories.Fields.IsActive")]
          public bool IsActive { get; set; }

          [NopResourceDisplayName("Plugins.Widgets.Slider.PictureId")]
          [AllowHtml]
          public string PictureUrl { get; set; }


		  public IList<SelectListItem> AvailableSupportCategories { get; set; }
          [NopResourceDisplayName("Admin.Support.SupportTopics.Fields.ProductCategories")]
          public string ProductCategoriesName { get; set; }
          public IList<SelectListItem> AvailableCategories { get; set; }
          public int[] SelectedProductCategoryIds { get; set; }
		 

        #region Nested classes

       

        #endregion
    }

   
}