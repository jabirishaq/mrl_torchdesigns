using Nop.Core.Plugins;
using Nop.Services.Common;
using Nop.Web.Framework.Menu;
using Nop.Web.Framework.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;
using Nop.Services.Localization;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Stores;
using Nop.Core;
using Nop.Web.Framework.Controllers;
using Nop.Core.Domain.Customers;
using Nop.Plugin.Widgets.TorchDesign_Support.Data;
using Nop.Plugin.Widgets.TorchDesign_Support;


namespace Nop.Plugin.Widgets.TorchDesign_Support
{
	public class SupportPlugin : BasePlugin, IAdminMenuPlugin,IWidgetPlugin
	{
		private readonly SupportObjectContext _objectContext;
		private readonly ISettingService _settingService;
		private readonly IStoreService _storeService;
		private readonly IWorkContext _workContext;


		public SupportPlugin(SupportObjectContext objectContext,
			  ISettingService settingService, IStoreService storeService, IWorkContext workContext)
		{

			this._objectContext = objectContext;
			this._settingService = settingService;
			this._storeService = storeService;
			this._workContext = workContext;

		}

		#region IAdminMenuPlugin Members

		public bool Authenticate()
		{
			return true;
		}

		public SiteMapNode BuildMenuItem()
		{
			var menuItem = new SiteMapNode()
			{
				Title = "Support",

				Visible = true,
				RouteValues = new RouteValueDictionary() { { "area", null } },
			};


			
			var menuFirstItem = new SiteMapNode()
			{
				Title = "Manage Category List",
				ControllerName = "Support",
				ActionName = "SupportCategoryList",
				Visible = true,
				RouteValues = new RouteValueDictionary() { { "area", null } },
			};
			menuItem.ChildNodes.Add(menuFirstItem);

			var menuSecondItem = new SiteMapNode()
			{
				Title = "Manage Topic List",
				ControllerName = "Support",
				ActionName = "SupportTopicList",
				Visible = true,
				RouteValues = new RouteValueDictionary() { { "area", null } },
			};
			menuItem.ChildNodes.Add(menuSecondItem);


			var menuThirdItem = new SiteMapNode()
			{
				Title = "Manage Video List",
				ControllerName = "Support",
				ActionName = "SupportVideoList",
				Visible = true,
				RouteValues = new RouteValueDictionary() { { "area", null } },
			};
			menuItem.ChildNodes.Add(menuThirdItem);

			var menuForthItem = new SiteMapNode()
			{
				Title = "Manage Download List",
				ControllerName = "Support",
				ActionName = "SupportDownloadList",
				Visible = true,
				RouteValues = new RouteValueDictionary() { { "area", null } },
			};
			menuItem.ChildNodes.Add(menuForthItem);

			return menuItem;
		}


		#endregion

		#region Methods
		public virtual int GetActiveStoreScopeConfiguration(IStoreService storeService, IWorkContext workContext)
		{
			//ensure that we have 2 (or more) stores
			if (storeService.GetAllStores().Count < 2)
				return 0;


			var storeId = workContext.CurrentCustomer.GetAttribute<int>(SystemCustomerAttributeNames.AdminAreaStoreScopeConfiguration);
			var store = storeService.GetStoreById(storeId);
			return store != null ? store.Id : 0;
		}



		public IList<string> GetWidgetZones()
		{

			return new List<string>() { "header_menu_after" };
		}

	

		/// <summary>
		/// Gets a route for displaying widget
		/// </summary>
		/// <param name="widgetZone">Widget zone where it's displayed</param>
		/// <param name="actionName">Action name</param>
		/// <param name="controllerName">Controller name</param>
		/// <param name="routeValues">Route values</param>

		public void GetDisplayWidgetRoute(string widgetZone, out string actionName, out string controllerName, out RouteValueDictionary routeValues)
		{
			actionName = "PublicInfo";
			controllerName = "Support";
			routeValues = new RouteValueDictionary()
		    {
		        {"Namespaces", "Nop.Plugin.Widgets.TorchDesign_Support.Controllers"},
		        {"area", null}
		       
		    };
		}

		public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
		{
			actionName = "Configure";
			controllerName = "Support";
			routeValues = new RouteValueDictionary() { { "Namespaces", "Nop.Plugin.Widgets.TorchDesign_Support.Controllers" }, { "area", null } };
		}



		public override void Install()
		{
            var settings = new TorchDesignSupportSettings()
            {
                SupportEnabled = true

            };
            _settingService.SaveSetting(settings);


            

			_objectContext.Install();
			this.AddOrUpdatePluginLocaleResource("admin.support", "Support");
            this.AddOrUpdatePluginLocaleResource("support.supporttopics", "Support Topics");
            this.AddOrUpdatePluginLocaleResource("support.supportvideos", "Videos");
            this.AddOrUpdatePluginLocaleResource("support.supportdownloads", "Downloads");
            this.AddOrUpdatePluginLocaleResource("support.supportcategories", "Support Categories");
            this.AddOrUpdatePluginLocaleResource("support.Searchbox.placeholder.text", "Search Keyword");
            this.AddOrUpdatePluginLocaleResource("Admin.Support.SupportCategories.Fields.IsActive", "Is Active");
            this.AddOrUpdatePluginLocaleResource("admin.common.supportcategorieslist", "Support categories List");
			this.AddOrUpdatePluginLocaleResource("admin.common.supporttopicslist", "Support Topics List");
            this.AddOrUpdatePluginLocaleResource("admin.support.supportcategories.manage", "Manage Support Categories");
			this.AddOrUpdatePluginLocaleResource("admin.support.supportcategories.addnew", "Add New Support Category");
			this.AddOrUpdatePluginLocaleResource("admin.support.supportcategories.fields.name", "Title");
			this.AddOrUpdatePluginLocaleResource("admin.support.supportcategories.fields.description", "Description");
			this.AddOrUpdatePluginLocaleResource("Admin.Support.SupportCategories.Fields.Picture", "Picture");
			this.AddOrUpdatePluginLocaleResource("Admin.Support.SupportCategories.Fields.ShowInFooter", "Show In Footer");
			this.AddOrUpdatePluginLocaleResource("admin.support.supportcategories.added", "Support Category Added");
			this.AddOrUpdatePluginLocaleResource("admin.support.supportcategories.editsupportcategorydetails", "Edit Support Category Details");
			this.AddOrUpdatePluginLocaleResource("admin.support.suportcategories.backtolist", "Back To List");
			this.AddOrUpdatePluginLocaleResource("admin.support.supportcategories.updated", "Support Category Updated");
			this.AddOrUpdatePluginLocaleResource("Admin.Support.SupportTopics.Manage", "Manage Support Topics");
			this.AddOrUpdatePluginLocaleResource("admin.support.supporttopics.fields.title", "Title");
			this.AddOrUpdatePluginLocaleResource("admin.support.supporttopics.addnew", "Add New Support Topic");
            this.AddOrUpdatePluginLocaleResource("admin.support.supporttopics.addnewcustom", "Add Support Topic");

			this.AddOrUpdatePluginLocaleResource("admin.support.supporttopics.backtolist", "Back To List");
            this.AddOrUpdatePluginLocaleResource("Admin.Support.SupportDownload.BackToList", "Back To List");
            
			this.AddOrUpdatePluginLocaleResource("admin.suport.supporttopics.info", "Support Topic Info");
			this.AddOrUpdatePluginLocaleResource("Admin.Support.SupportTopics.Fields.Description", "Description");
			this.AddOrUpdatePluginLocaleResource("admin.suport.supporttopics.supportcategory", "Support Categories");
			this.AddOrUpdatePluginLocaleResource("admin.suport.supporttopics.productcategory", "Product Category");
			this.AddOrUpdatePluginLocaleResource("admin.suport.supporttopics.steps", "	Steps");
			this.AddOrUpdatePluginLocaleResource("Admin.Support.SupportTopicStep.Fields.Name", "Title");
			this.AddOrUpdatePluginLocaleResource("Admin.Support.SupportTopicStep.Fields.Description", "Description");
			this.AddOrUpdatePluginLocaleResource("Admin.Support.SupportTopicStep.Fields.Picture", "Picture");
			this.AddOrUpdatePluginLocaleResource("admin.support.supporttopic.added", "Support Topic Added");
			this.AddOrUpdatePluginLocaleResource("admin.support.supporttopics.editsupporttopicdetails", "Edit Support Topic Details");
            this.AddOrUpdatePluginLocaleResource("Admin.Support.SupportCategories.EditCategoryDetails", "Edit Support Category Details");
			this.AddOrUpdatePluginLocaleResource("admin.support.supporttopics.topics.backtolist", "Back To List");
			this.AddOrUpdatePluginLocaleResource("admin.support.supporttopics.supporttopicstep.fields.title", "Title");
			this.AddOrUpdatePluginLocaleResource("admin.support.supporttopics.supporttopicstep.fields.picturethumbnailurl", "Picture");
			this.AddOrUpdatePluginLocaleResource("admin.support.supporttopic.supporttopicstep.addbutton", "Add Step");
			this.AddOrUpdatePluginLocaleResource("admin.support.supporttopic.updated", "Support Topic Updated");
			this.AddOrUpdatePluginLocaleResource("admin.support.supportcategories.info", "Support Category Info");
			this.AddOrUpdatePluginLocaleResource("admin.support.supporttopics.supporttopicstep.fields.template", "Template");
			this.AddOrUpdatePluginLocaleResource("Admin.Support.SupportTopicStep.Fields.Template", "Template");
			this.AddOrUpdatePluginLocaleResource("admin.suport.supporttopics.relatedproducts", "Related Products");
			this.AddOrUpdatePluginLocaleResource("admin.support.supporttopic.relatedproducts.fields.product", "Product");
			this.AddOrUpdatePluginLocaleResource("admin.support.supporttopic.relatedproducts.fields.displayorder", "Display Order");
			this.AddOrUpdatePluginLocaleResource("Admin.Support.SupportTopic.RelatedProducts.SaveBeforeEdit", "You need to save the support topic before you can add related products for this support topic.");
			this.AddOrUpdatePluginLocaleResource("admin.suport.supportdownloads.info", "Download Info");
			this.AddOrUpdatePluginLocaleResource("Admin.Support.SupportDownloads.Fields.Title", "Title");
			this.AddOrUpdatePluginLocaleResource("Admin.Support.SupportDownloads.Fields.Download", "Download");
			this.AddOrUpdatePluginLocaleResource("admin.suport.supportdownloads.productcategory", "Product Category");
			this.AddOrUpdatePluginLocaleResource("admin.suport.supportdownloads.relatedproducts", "Related Products");
			this.AddOrUpdatePluginLocaleResource("Admin.Support.supportdownloads.RelatedProducts.AddNew", "Add New Related Products");
			this.AddOrUpdatePluginLocaleResource("admin.support.supportdownloads.addnew", "Add New Support Download");
            this.AddOrUpdatePluginLocaleResource("admin.support.supportdownloads.addnewcustom", "Add Download");

			this.AddOrUpdatePluginLocaleResource("admin.common.supportdownloadslist", "Support Download List");
			this.AddOrUpdatePluginLocaleResource("admin.support.supportdownloads.manage", "Mange Support Downloads");
			this.AddOrUpdatePluginLocaleResource("admin.support.supportdownload.added", "Support Download Added");
			this.AddOrUpdatePluginLocaleResource("admin.support.supportdownloads.editsupportdownloaddetails", "Edit Support Download Details");
			this.AddOrUpdatePluginLocaleResource("admin.support.supportdownloads.downloads.backtolist", "Back To List");
			this.AddOrUpdatePluginLocaleResource("admin.support.supportdownload.relatedproducts.fields.product", "Product");
			this.AddOrUpdatePluginLocaleResource("admin.support.supportdownload.relatedproducts.fields.displayorder", "Display Order");
			this.AddOrUpdatePluginLocaleResource("admin.support.supportdownload.updated", "Support Download Updated");
            this.AddOrUpdatePluginLocaleResource("Admin.Support.Supportdownload.Fields.Description", "Description");

			this.AddOrUpdatePluginLocaleResource("admin.common.support", "Support");
			this.AddOrUpdatePluginLocaleResource("admin.support.supportvideo.added", "Support Video Added");
            this.AddOrUpdatePluginLocaleResource("Admin.Support.Supportvideo.Fields.Description", "Description");

			this.AddOrUpdatePluginLocaleResource("Admin.Support.SupportVideos.Fields.Title", "Title");
			this.AddOrUpdatePluginLocaleResource("Admin.Support.SupportVideos.Fields.Picture", "Picture");
            this.AddOrUpdatePluginLocaleResource("Admin.Support.SupportVideos.Fields.Vemio.Information", "Vimeo Information");
			this.AddOrUpdatePluginLocaleResource("admin.support.supportvideo.updated", "Support Video Updated");
            this.AddOrUpdatePluginLocaleResource("Admin.Support.SupportTopics.Fields.Tag", "Search Tags");
            this.AddOrUpdatePluginLocaleResource("admin.common.support.supportvideo.list", "Support Video List");
			this.AddOrUpdatePluginLocaleResource("admin.support.supportvideos.manage", "Manage Support Videos");
			this.AddOrUpdatePluginLocaleResource("admin.support.supportvideo.fields.title", "Title");
			this.AddOrUpdatePluginLocaleResource("support.supportvideo.fields.picturethumbnailurl", "Picture");
			this.AddOrUpdatePluginLocaleResource("admin.support.supportvideo.fields.vimeoinformation", "Vimeo Information");
			this.AddOrUpdatePluginLocaleResource("admin.support.supportvideo.addnew", "Add New Support Video");
            this.AddOrUpdatePluginLocaleResource("admin.support.supportvideo.addnewcustom", "Add Video");
            this.AddOrUpdatePluginLocaleResource("support.resultnotfoud.text", "Sorry, we did not find any results that matched your search query.");
            this.AddOrUpdatePluginLocaleResource("support.resultnotfoud.text.video", "Sorry, we did not find any support video for this keyword");
            this.AddOrUpdatePluginLocaleResource("support.resultnotfoud.text.topic", "Sorry, we did not find any support topic for this keyword");
            this.AddOrUpdatePluginLocaleResource("support.resultnotfoud.text.category", "Sorry, we did not find any support category for this keyword");
            this.AddOrUpdatePluginLocaleResource("support.resultnotfoud.text.download", "Sorry, we did not find any support download for this keyword");
            this.AddOrUpdatePluginLocaleResource("admin.support.supportvideo.backtolist", "Back To List");
			this.AddOrUpdatePluginLocaleResource("admin.suport.supportvideo.info", "Support Video Info");
			this.AddOrUpdatePluginLocaleResource("admin.suport.supportvideo.productcategory", "Product Category");
			this.AddOrUpdatePluginLocaleResource("admin.suport.supportvideo.relatedproducts", "Related Product");
			this.AddOrUpdatePluginLocaleResource("Admin.Support.SupportVideos.RelatedProducts.AddNew", "Add New Related Products");
			this.AddOrUpdatePluginLocaleResource("admin.support.supportvideo.relatedproducts.savebeforeedit", "You need to save the support video before you can add products for this related product page.");
			this.AddOrUpdatePluginLocaleResource("Admin.support.supportvideo.Fields.Title", "Title");
			this.AddOrUpdatePluginLocaleResource("Admin.support.supportvideoFields.Picture", "Picture");
			this.AddOrUpdatePluginLocaleResource("Admin.support.supportvideoFields.Vemio.Information", "Vemio Information");
			this.AddOrUpdatePluginLocaleResource("admin.support.supportvideo.editsupportvideodetails", "Edit Support Video");
			this.AddOrUpdatePluginLocaleResource("admin.support.supportvideo.videos.backtolist", "Back To List");
			this.AddOrUpdatePluginLocaleResource("admin.support.supportvideo.relatedproducts.fields.product", "Product");
			this.AddOrUpdatePluginLocaleResource("admin.support.supportvideo.relatedproducts.fields.displayorder", "Display Order");
            this.AddOrUpdatePluginLocaleResource("support.description", "Search for help and info by product. Or access top support resources");
            this.AddOrUpdatePluginLocaleResource("admin.support.fields.supportcategories.nosupportcategories", "No support categories are configured");
            this.AddOrUpdatePluginLocaleResource("Admin.Support.SupportTopics.Fields.DisplayOrder", "Display Order");
            this.AddOrUpdatePluginLocaleResource("Admin.Configuration.Settings.Catalog.SupportEnabled", "Include In Top Menu");

            this.AddOrUpdatePluginLocaleResource("admin.support.supportvideos.fields.title.hint", "Enter video title");
            this.AddOrUpdatePluginLocaleResource("admin.support.supporttopics.fields.description.hint", "Enter video description");
            this.AddOrUpdatePluginLocaleResource("admin.support.supportvideos.fields.vemio.information.hint", "Enter vemio information");
            this.AddOrUpdatePluginLocaleResource("admin.support.supporttopics.fields.tag.hint", "Enter tag for video");
            this.AddOrUpdatePluginLocaleResource("admin.support.supportvideos.fields.picture.hint", "Upload picture file");
            this.AddOrUpdatePluginLocaleResource("admin.support.supporttopics.fields.displayorder.hint", "Enter display order of video");
            this.AddOrUpdatePluginLocaleResource("admin.support.supportcategories.fields.name.hint", "Enter category name");
            this.AddOrUpdatePluginLocaleResource("admin.support.supportcategories.fields.picture.hint", "Upload picture of cateogry");
            this.AddOrUpdatePluginLocaleResource("admin.support.supporttopics.fields.title.hint", "Enter topic name");
            this.AddOrUpdatePluginLocaleResource("admin.support.supporttopicstep.fields.template.hint", "Select layout template");
            this.AddOrUpdatePluginLocaleResource("admin.support.supporttopics.fields.description.hint", "Enter description for topic");
            this.AddOrUpdatePluginLocaleResource("admin.support.supportdownloads.fields.title.hint", "Enter download title");
            this.AddOrUpdatePluginLocaleResource("Admin.Support.SupportDownloads.Fields.Description", "Description");
            this.AddOrUpdatePluginLocaleResource("Admin.Support.SupportDownloads.Fields.Description.Hint", "Enter download description");
            this.AddOrUpdatePluginLocaleResource("admin.support.supportdownloads.fields.download.hint", "Upload file");
            this.AddOrUpdatePluginLocaleResource("Admin.Support.SupportVideos.Fields.ProductCategories", "Product categories");
            this.AddOrUpdatePluginLocaleResource("admin.support.supporttopics.fields.productcategories", "Product categories");
            this.AddOrUpdatePluginLocaleResource("Admin.Catalog.Products.Video.Fields.VideoId.Hint", "Upload video");
            this.AddOrUpdatePluginLocaleResource("admin.catalog.products.pictures.fields.picturethumb.hint", "Picture thumbnail");
            this.AddOrUpdatePluginLocaleResource("admin.catalog.products.fields.iseligibleforfree.hint", "Check if it is eligible for free");
            this.AddOrUpdatePluginLocaleResource("admin.catalog.products.tierprices.fields.quantity.hint", "Set quantity for tier price");
            this.AddOrUpdatePluginLocaleResource("admin.catalog.products.fields.madeinusa.hint", "Check to set made in usa");

            base.Install();
		}

		public override void Uninstall()
		{
            
			_objectContext.Uninstall();
			//this.DeletePluginLocaleResource("Plugins.Widgets.Storelocator.addstore");
            this.DeletePluginLocaleResource("admin.support");
            this.DeletePluginLocaleResource("support.supporttopics");
            this.DeletePluginLocaleResource("support.supportvideos");
            this.DeletePluginLocaleResource("support.supportdownloads");
            this.DeletePluginLocaleResource("support.supportcategories");
            this.DeletePluginLocaleResource("support.Searchbox.placeholder.text");
            this.DeletePluginLocaleResource("Admin.Support.SupportCategories.Fields.IsActive");
			this.DeletePluginLocaleResource("admin.common.supportcategorieslist");
			this.DeletePluginLocaleResource("admin.common.supporttopicslist");
			this.DeletePluginLocaleResource("admin.support.supportcategories.manage");
			this.DeletePluginLocaleResource("admin.support.supportcategories.addnew");
			this.DeletePluginLocaleResource("admin.support.supportcategories.fields.name");
			this.DeletePluginLocaleResource("admin.support.supportcategories.fields.description");
			this.DeletePluginLocaleResource("Admin.Support.SupportCategories.Fields.Picture");
			this.DeletePluginLocaleResource("Admin.Support.SupportCategories.Fields.ShowInFooter");
			this.DeletePluginLocaleResource("admin.support.supportcategories.added");
			this.DeletePluginLocaleResource("admin.support.supportcategories.editsupportcategorydetails");
			this.DeletePluginLocaleResource("admin.support.suportcategories.backtolist");
			this.DeletePluginLocaleResource("admin.support.supportcategories.updated");
			this.DeletePluginLocaleResource("Admin.Support.SupportTopics.Manage");
			this.DeletePluginLocaleResource("admin.support.supporttopics.fields.title");
			this.DeletePluginLocaleResource("admin.support.supporttopics.addnew");
			this.DeletePluginLocaleResource("admin.support.supporttopics.backtolist");
            this.DeletePluginLocaleResource("Admin.Support.SupportDownload.BackToList");
			this.DeletePluginLocaleResource("admin.suport.supporttopics.info");
			this.DeletePluginLocaleResource("Admin.Support.SupportTopics.Fields.Description");
			this.DeletePluginLocaleResource("admin.suport.supporttopics.supportcategory");
			this.DeletePluginLocaleResource("admin.suport.supporttopics.productcategory");
			this.DeletePluginLocaleResource("admin.suport.supporttopics.steps");
			this.DeletePluginLocaleResource("Admin.Support.SupportTopicStep.Fields.Name");
			this.DeletePluginLocaleResource("Admin.Support.SupportTopicStep.Fields.Description");
			this.DeletePluginLocaleResource("Admin.Support.SupportTopicStep.Fields.Picture");
			this.DeletePluginLocaleResource("admin.support.supporttopic.added");
			this.DeletePluginLocaleResource("admin.support.supporttopics.editsupporttopicdetails");
            this.DeletePluginLocaleResource("Admin.Support.SupportCategories.EditCategoryDetails");
			this.DeletePluginLocaleResource("admin.support.supporttopics.topics.backtolist");
			this.DeletePluginLocaleResource("admin.support.supporttopics.supporttopicstep.fields.title");
			this.DeletePluginLocaleResource("admin.support.supporttopics.supporttopicstep.fields.picturethumbnailurl");
			this.DeletePluginLocaleResource("admin.support.supporttopic.supporttopicstep.addbutton");
			this.DeletePluginLocaleResource("admin.support.supporttopic.updated");
			this.DeletePluginLocaleResource("admin.support.supportcategories.info");
			this.DeletePluginLocaleResource("admin.support.supporttopics.supporttopicstep.fields.template");
			this.DeletePluginLocaleResource("Admin.Support.SupportTopicStep.Fields.Template");
            this.DeletePluginLocaleResource("admin.suport.supporttopics.relatedproducts");
            this.DeletePluginLocaleResource("Admin.Support.SupportTopics.Fields.Tag");
			this.DeletePluginLocaleResource("admin.support.supporttopic.relatedproducts.fields.product");
			this.DeletePluginLocaleResource("admin.support.supporttopic.relatedproducts.fields.displayorder");
			this.DeletePluginLocaleResource("Admin.Support.SupportTopic.RelatedProducts.SaveBeforeEdit");
			this.DeletePluginLocaleResource("admin.suport.supportdownloads.info");
			this.DeletePluginLocaleResource("Admin.Support.SupportDownloads.Fields.Title");
			this.DeletePluginLocaleResource("Admin.Support.SupportDownloads.Fields.Download");
			this.DeletePluginLocaleResource("admin.suport.supportdownloads.productcategory");
			this.DeletePluginLocaleResource("admin.suport.supportdownloads.relatedproducts");
			this.DeletePluginLocaleResource("admin.support.supportdownloads.addnew");
			this.DeletePluginLocaleResource("admin.common.supportdownloadslist");
			this.DeletePluginLocaleResource("admin.support.supportdownloads.manage");
			this.DeletePluginLocaleResource("admin.support.supportdownload.added");
			this.DeletePluginLocaleResource("admin.support.supportdownloads.editsupportdownloaddetails");
			this.DeletePluginLocaleResource("admin.support.supportdownloads.downloads.backtolist");
			this.DeletePluginLocaleResource("Admin.Support.supportdownloads.RelatedProducts.AddNew");
			this.DeletePluginLocaleResource("admin.support.supportdownload.relatedproducts.fields.product");
			this.DeletePluginLocaleResource("admin.support.supportdownload.relatedproducts.fields.displayorder");
			this.DeletePluginLocaleResource("admin.support.supportdownload.updated");
			this.DeletePluginLocaleResource("admin.common.support");
			this.DeletePluginLocaleResource("admin.common.supportvideoslist");
			this.DeletePluginLocaleResource("admin.support.supportvideos.manage");
			this.DeletePluginLocaleResource("admin.support.supportvideo.fields.title");
			this.DeletePluginLocaleResource("support.supportvideo.fields.picturethumbnailurl");
			this.DeletePluginLocaleResource("admin.support.supportvideo.fields.vimeoinformation");
			this.DeletePluginLocaleResource("admin.support.supportvideos.addnew");
			this.DeletePluginLocaleResource("admin.support.supportvideos.backtolist");
			this.DeletePluginLocaleResource("admin.suport.supportvideos.info");
			this.DeletePluginLocaleResource("admin.suport.supportvideos.productcategory");
			this.DeletePluginLocaleResource("admin.suport.supportvideos.relatedproducts");
			this.DeletePluginLocaleResource("admin.support.supportvideo.relatedproducts.savebeforeedit");
			this.DeletePluginLocaleResource("Admin.Support.SupportVideos.Fields.Title");
			this.DeletePluginLocaleResource("Admin.Support.SupportVideos.Fields.Picture");
			this.DeletePluginLocaleResource("Admin.Support.SupportVideos.Fields.Vemio.Information");
			this.DeletePluginLocaleResource("admin.support.supportvideos.editsupportvideodetails");
			this.DeletePluginLocaleResource("admin.support.supportvideos.videos.backtolist");
			this.DeletePluginLocaleResource("Admin.Support.SupportVideos.RelatedProducts.AddNew");
			this.DeletePluginLocaleResource("admin.support.supportvideo.relatedproducts.fields.product");
			this.DeletePluginLocaleResource("admin.support.supportvideo.relatedproducts.fields.displayorder");
			this.DeletePluginLocaleResource("admin.support.supportvideo.added");
			this.DeletePluginLocaleResource("Admin.Support.SupportVideos.Fields.Title");
			this.DeletePluginLocaleResource("Admin.Support.SupportVideos.Fields.Picture");
			this.DeletePluginLocaleResource("Admin.Support.SupportVideos.Fields.Vemio.Information");
			this.DeletePluginLocaleResource("admin.support.supportvideo.updated");
            this.DeletePluginLocaleResource("support.resultnotfoud.text");
            this.DeletePluginLocaleResource("support.resultnotfoud.text.video");
            this.DeletePluginLocaleResource("support.resultnotfoud.text.topic");
            this.DeletePluginLocaleResource("support.resultnotfoud.text.category");
            this.DeletePluginLocaleResource("support.resultnotfoud.text.download");
            this.DeletePluginLocaleResource("admin.support.fields.supportcategories.nosupportcategories");
            this.DeletePluginLocaleResource("Admin.Support.SupportTopics.Fields.DisplayOrder");
            this.DeletePluginLocaleResource("Admin.Configuration.Settings.Catalog.SupportEnabled");

            this.DeletePluginLocaleResource("admin.support.supportvideos.fields.title.hint");
            this.DeletePluginLocaleResource("admin.support.supporttopics.fields.description.hint");
            this.DeletePluginLocaleResource("admin.support.supportvideos.fields.vemio.information.hint");
            this.DeletePluginLocaleResource("admin.support.supporttopics.fields.tag.hint");
            this.DeletePluginLocaleResource("admin.support.supportvideos.fields.picture.hint");
            this.DeletePluginLocaleResource("admin.support.supporttopics.fields.displayorder.hint");
            this.DeletePluginLocaleResource("admin.support.supportcategories.fields.name.hint");
            this.DeletePluginLocaleResource("admin.support.supportcategories.fields.picture.hint");
            this.DeletePluginLocaleResource("admin.support.supporttopics.fields.title.hint");
            this.DeletePluginLocaleResource("admin.support.supporttopicstep.fields.template.hint");
            this.DeletePluginLocaleResource("admin.support.supporttopics.fields.description.hint");
            this.DeletePluginLocaleResource("admin.support.supportdownloads.fields.title.hint");
            this.DeletePluginLocaleResource("Admin.Support.SupportDownloads.Fields.Description");
            this.DeletePluginLocaleResource("Admin.Support.SupportDownloads.Fields.Description.Hint");
            this.DeletePluginLocaleResource("admin.support.supportdownloads.fields.download.hint");
            this.DeletePluginLocaleResource("Admin.Support.SupportVideos.Fields.ProductCategories");
            this.DeletePluginLocaleResource("admin.support.supporttopics.fields.productcategories");
            this.DeletePluginLocaleResource("Admin.Catalog.Products.Video.Fields.VideoId.Hint");
            this.DeletePluginLocaleResource("admin.catalog.products.pictures.fields.picturethumb.hint");
            this.DeletePluginLocaleResource("admin.catalog.products.fields.iseligibleforfree.hint");
            this.DeletePluginLocaleResource("admin.catalog.products.tierprices.fields.quantity.hint");
            this.DeletePluginLocaleResource("admin.catalog.products.fields.madeinusa.hint");
            base.Uninstall();
		}

		#endregion




		//public void GetDisplayWidgetRoute(string widgetZone, out string actionName, out string controllerName, out RouteValueDictionary routeValues)
		//{
		//    throw new NotImplementedException();
		//}
	}
}
