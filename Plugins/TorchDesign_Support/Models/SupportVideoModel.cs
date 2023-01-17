using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Localization;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Widgets.TorchDesign_Support.Models
{
	 //[Validator(typeof(TopicValidator))]
    public partial class SupportVideoModel : BaseNopEntityModel
    {
		 public SupportVideoModel()
        {
			  AvailableCategories = new List<SelectListItem>();
        }

		 [Required]
        [NopResourceDisplayName("Admin.Support.SupportVideos.Fields.Title")]
        [StringLength(50, ErrorMessage = "Maximum 50 characters allowed")]
        public string Title { get; set; }

         public string redirect { get; set; }

		 [Required]
		  [NopResourceDisplayName("Admin.Support.SupportVideos.Fields.Vemio.Information")]
          [StringLength(50, ErrorMessage = "Maximum 50 characters allowed")]
        public string VimeoInformation { get; set; }

       [NopResourceDisplayName("Admin.Support.SupportTopics.Fields.Description")]
        [AllowHtml]
       //[RegularExpression("^[^<>,<|>]+$", ErrorMessage = "Html tags are not allowed.")]
        public string Description { get; set; }

         [NopResourceDisplayName("Admin.Support.SupportTopics.Fields.Tag")]
         public string Tag { get; set; }


        [NopResourceDisplayName("Admin.Support.SupportTopics.Fields.DisplayOrder")]
         public int DisplayOrder {get; set; }

		  [UIHint("SupportPicture")]
		  [NopResourceDisplayName("Admin.Support.SupportVideos.Fields.Picture")]
		  public int PictureId { get; set; }

		  [NopResourceDisplayName("Admin.Support.SupportVideos.Fields.Picture")]
		  public string PictureThumbnailUrl { get; set; }

		  //ProductCategories
		  [NopResourceDisplayName("Admin.Support.SupportVideos.Fields.ProductCategories")]
		  public string ProductCategoriesName { get; set; }
		  public IList<SelectListItem> AvailableCategories { get; set; }
		  public int[] SelectedProductCategoryIds { get; set; }

		  public partial class SupportVideoRelatedProductModel : BaseNopEntityModel
		  {
			  public int ProductId { get; set; }

			  [NopResourceDisplayName("Admin.Support.SupportVideo.RelatedProducts.Fields.Product")]
			  public string ProductName { get; set; }

			  [NopResourceDisplayName("Admin.Support.SupportVideo.RelatedProducts.Fields.DisplayOrder")]
			  public int DisplayOrder { get; set; }
		  }

		  public partial class AddSupportVideoRelatedProductModel : BaseNopModel
		  {
			  public AddSupportVideoRelatedProductModel()
			  {
				  AvailableCategories = new List<SelectListItem>();
				  AvailableManufacturers = new List<SelectListItem>();
				  AvailableStores = new List<SelectListItem>();
				  AvailableVendors = new List<SelectListItem>();
				  AvailableProductTypes = new List<SelectListItem>();
			  }

			  [NopResourceDisplayName("Admin.Catalog.Products.List.SearchProductName")]
			  [AllowHtml]
			  public string SearchProductName { get; set; }
			  [NopResourceDisplayName("Admin.Catalog.Products.List.SearchCategory")]
			  public int SearchCategoryId { get; set; }
			  [NopResourceDisplayName("Admin.Catalog.Products.List.SearchManufacturer")]
			  public int SearchManufacturerId { get; set; }
			  [NopResourceDisplayName("Admin.Catalog.Products.List.SearchStore")]
			  public int SearchStoreId { get; set; }
			  [NopResourceDisplayName("Admin.Catalog.Products.List.SearchVendor")]
			  public int SearchVendorId { get; set; }
			  [NopResourceDisplayName("Admin.Catalog.Products.List.SearchProductType")]
			  public int SearchProductTypeId { get; set; }
              [NopResourceDisplayName("Admin.Catalog.Products.Fields.ManufacturerPartNumber")]
              public string Partno { get; set; }

			  public IList<SelectListItem> AvailableCategories { get; set; }
			  public IList<SelectListItem> AvailableManufacturers { get; set; }
			  public IList<SelectListItem> AvailableStores { get; set; }
			  public IList<SelectListItem> AvailableVendors { get; set; }
			  public IList<SelectListItem> AvailableProductTypes { get; set; }

			  public int SupportVideoId { get; set; }

			  public int[] SelectedProductIds { get; set; }

			  //vendor
			  public bool IsLoggedInAsVendor { get; set; }
		  }
     
    }

	 public partial class ProductCategoryInformationModel : BaseNopModel
	 {
		 public bool HasSupportVideos { get; set; }
		 public bool HasSupportDownloads { get; set; }
	 }


}