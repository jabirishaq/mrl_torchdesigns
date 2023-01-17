using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Localization;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Widgets.TorchDesign_Support.Models
{
	 //[Validator(typeof(TopicValidator))]
    public partial class SupportDownloadModel : BaseNopEntityModel
    {
		 public SupportDownloadModel()
        {
			  AvailableCategories = new List<SelectListItem>();
        }

		  [Required]
          [StringLength(250, ErrorMessage = "Maximum 250 characters allowed")]
		  [NopResourceDisplayName("Admin.Support.SupportDownloads.Fields.Title")]
          [AllowHtml]
          public string Title { get; set; }

          public string redirect { get; set; }
          [UIHint("SupportDownload")]
		  [NopResourceDisplayName("Admin.Support.SupportDownloads.Fields.Download")]
		  public int DownloadId { get; set; }

          [NopResourceDisplayName("Admin.Support.SupportDownloads.Fields.Description")]
          //[RegularExpression("^[^<>,<|>]+$", ErrorMessage = "Html tags are not allowed.")]
          public string Description { get; set; }

		  //ProductCategories
		  [NopResourceDisplayName("Admin.Support.SupportDownloads.Fields.ProductCategories")]
		  public string ProductCategoriesName { get; set; }
		  public IList<SelectListItem> AvailableCategories { get; set; }
		  public int[] SelectedProductCategoryIds { get; set; }

		  public partial class SupportDownloadRelatedProductModel : BaseNopEntityModel
		  {
			  public int ProductId { get; set; }

			  [NopResourceDisplayName("Admin.Support.SupportDownload.RelatedProducts.Fields.Product")]
			  public string ProductName { get; set; }

			  [NopResourceDisplayName("Admin.Support.SupportDownload.RelatedProducts.Fields.DisplayOrder")]
			  public int DisplayOrder { get; set; }
		  }
		  public partial class AddSupportDownloadRelatedProductModel : BaseNopModel
		  {
			  public AddSupportDownloadRelatedProductModel()
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

			  public int SupportDownloadId { get; set; }

			  public int[] SelectedProductIds { get; set; }

			  //vendor
			  public bool IsLoggedInAsVendor { get; set; }
		  }
     
    }


}