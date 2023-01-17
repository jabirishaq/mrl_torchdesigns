using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Localization;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Widgets.TorchDesign_Support.Models
{
	 //[Validator(typeof(TopicValidator))]
    public partial class SupportTopicModel : BaseNopEntityModel
    {
		 public SupportTopicModel()
        {
			  AvailableSupportCategories = new List<SupportCategoryModel>();
			  AvailableCategories = new List<SelectListItem>();
              AvailableTopicStep = new List<SupportTopicStepModel>();
             
			  AddSupportTopicStepModel = new SupportTopicStepModel();
        }

		  [Required]
          [StringLength(250, ErrorMessage = "Maximum 250 characters allowed")]
        [NopResourceDisplayName("Admin.Support.SupportTopics.Fields.Title")]
        [AllowHtml]
         //public int AddCount { get; set; }

        public string Title { get; set; }

          public string redirect { get; set; }

		[NopResourceDisplayName("Admin.Support.SupportTopics.Fields.Description")]
        [AllowHtml]
        public string Description { get; set; }

       
		  //SupportCategories
		  [NopResourceDisplayName("Admin.Support.SupportTopics.Fields.SupportCategories")]
		  public string SupportCategoriesTitle { get; set; }
		  public List<SupportCategoryModel> AvailableSupportCategories { get; set; }
		  public int[] SelectedSupportCategoryIds { get; set; }


		  //ProductCategories
		  [NopResourceDisplayName("Admin.Support.SupportTopics.Fields.ProductCategories")]
		  public string ProductCategoriesName { get; set; }
		  public IList<SelectListItem> AvailableCategories { get; set; }
		  public int[] SelectedProductCategoryIds { get; set; }

		  //add specification attribute model
		  //public AddProductSpecificationAttributeModel AddSpecificationAttributeModel { get; set; }

		  //add support topic model
          public int CompareCount { get; set; }
          public IList<SupportTopicStepModel> AvailableTopicStep { get; set; }
		  public SupportTopicStepModel AddSupportTopicStepModel { get; set; }

          [NopResourceDisplayName("Admin.Support.SupportTopicStep.Fields.Template")]
          public string TemplateType { get; set; }

          [NopResourceDisplayName("Admin.Support.SupportTopicStep.Fields.Template")]
          public int TemplateId { get; set; }
		  public partial class SupportTopicRelatedProductModel : BaseNopEntityModel
		  {
			  public int ProductId { get; set; }

			  [NopResourceDisplayName("Admin.Support.SupportTopic.RelatedProducts.Fields.Product")]
			  public string ProductName { get; set; }

			  [NopResourceDisplayName("Admin.Support.SupportTopic.RelatedProducts.Fields.DisplayOrder")]
			  public int DisplayOrder { get; set; }
		  }
		  public partial class AddRelatedProductModel : BaseNopModel
		  {
			  public AddRelatedProductModel()
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

			  public int SupportTopicId { get; set; }

			  public int[] SelectedProductIds { get; set; }

			  //vendor
			  public bool IsLoggedInAsVendor { get; set; }
		  }
      
    }


}