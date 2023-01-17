using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Localization;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Widgets.TorchDesign_Support.Models
{
	 //[Validator(typeof(TopicValidator))]
    public partial class PublicSupportTopicModel : BaseNopEntityModel
    {
        public PublicSupportTopicModel()
        {
			  AvailableSupportCategories = new List<SupportCategoryModel>();
			  AvailableCategories = new List<SelectListItem>();
              AddSupportTopicStepModel = new List<SupportInternalTopicStepModel>();
        }

		  [Required]
        [NopResourceDisplayName("Admin.Support.SupportTopics.Fields.Title")]
        [AllowHtml]
        public string Title { get; set; }


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
          public IList<SupportInternalTopicStepModel> AddSupportTopicStepModel { get; set; }
       
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
    public class SupportInternalTopicStepModel : BaseNopModel
    {
        public int Id { get; set; }

        public int SupportStepNo { get; set; }

        [NopResourceDisplayName("Admin.Support.SupportTopicStep.Fields.Name")]
        [AllowHtml]
        public string Title { get; set; }

        [NopResourceDisplayName("Admin.Support.SupportTopicStep.Fields.Description")]
        [AllowHtml]
        public string Description { get; set; }

       
        [NopResourceDisplayName("Admin.Support.SupportTopicStep.Fields.Picture")]
        public int PictureId { get; set; }

        [NopResourceDisplayName("Admin.Support.SupportTopicStep.Fields.Picture")]
        public string PictureThumbnailUrl { get; set; }



        [NopResourceDisplayName("Admin.Support.SupportTopicStep.Fields.Displayorder")]
        public int DisplayOrder { get; set; }

    }

}