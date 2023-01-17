using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Nop.Admin.Models.Blogs;
using Nop.Core.Domain.Blogs;
using Nop.Core.Domain.Customers;
using Nop.Services.Blogs;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Stores;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Kendoui;
using Nop.Web.Framework.Mvc;
using Nop.Services.Messages;
using Nop.Core;
using Nop.Core.Domain.Messages;

namespace Nop.Admin.Controllers
{
    public partial class BlogController : BaseAdminController
	{
		#region Fields

        private readonly IBlogService _blogService;
        private readonly ILanguageService _languageService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IStoreService _storeService;
        private readonly INewsLetterSubscriptionService _newsLetterSubscriptionService;
        private readonly IStoreMappingService _storeMappingService;
        private readonly INewsLetterSubscriptionService _newsletterservice;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;

        #endregion

        #region Constructors

        public BlogController(IBlogService blogService, ILanguageService languageService,
            IDateTimeHelper dateTimeHelper,
            INewsLetterSubscriptionService newsLetterSubscriptionService,
            ILocalizationService localizationService, IPermissionService permissionService,
            IUrlRecordService urlRecordService,
            IStoreContext storeContext,
            IStoreService storeService, IStoreMappingService storeMappingService, INewsLetterSubscriptionService newsletterservice, IWorkflowMessageService workflowMessageService, IWorkContext workContext)
        {
            this._blogService = blogService;
            this._languageService = languageService;
            this._dateTimeHelper = dateTimeHelper;
            this._localizationService = localizationService;
            this._permissionService = permissionService;
            this._newsLetterSubscriptionService = newsLetterSubscriptionService;
            this._urlRecordService = urlRecordService;
            this._storeContext = storeContext;
            this._storeService = storeService;
            this._storeMappingService = storeMappingService;
            this._newsletterservice = newsletterservice;
            this._workflowMessageService = workflowMessageService;
            this._workContext = workContext;
		}

		#endregion 
        
        #region Utilities

        [NonAction]
        protected virtual void PrepareStoresMappingModel(BlogPostModel model, BlogPost blogPost, bool excludeProperties)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            model.AvailableStores = _storeService
                .GetAllStores()
                .Select(s => s.ToModel())
                .ToList();
            if (!excludeProperties)
            {
                if (blogPost != null)
                {
                    model.SelectedStoreIds = _storeMappingService.GetStoresIdsWithAccess(blogPost);
                }
                else
                {
                    model.SelectedStoreIds = new int[0];
                }
            }
        }

        [NonAction]
        protected virtual void SaveStoreMappings(BlogPost blogPost, BlogPostModel model)
        {
            var existingStoreMappings = _storeMappingService.GetStoreMappings(blogPost);
            var allStores = _storeService.GetAllStores();
            foreach (var store in allStores)
            {
                if (model.SelectedStoreIds != null && model.SelectedStoreIds.Contains(store.Id))
                {
                    //new store
                    if (existingStoreMappings.Count(sm => sm.StoreId == store.Id) == 0)
                        _storeMappingService.InsertStoreMapping(blogPost, store.Id);
                }
                else
                {
                    //remove store
                    var storeMappingToDelete = existingStoreMappings.FirstOrDefault(sm => sm.StoreId == store.Id);
                    if (storeMappingToDelete != null)
                        _storeMappingService.DeleteStoreMapping(storeMappingToDelete);
                }
            }
        }

        #endregion
        
		#region Blog posts

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

		public ActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageBlog))
                return AccessDeniedView();

			return View();
		}

		[HttpPost]
        public ActionResult List(DataSourceRequest command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageBlog))
                return AccessDeniedView();

            var blogPosts = _blogService.GetAllBlogPosts(0, 0, null, null, command.Page - 1, command.PageSize, true);
            var gridModel = new DataSourceResult
            {
                Data = blogPosts.Select(x =>
                {
                    var m = x.ToModel();
                    if (x.StartDateUtc.HasValue)
                        m.StartDate = _dateTimeHelper.ConvertToUserTime(x.StartDateUtc.Value, DateTimeKind.Utc);
                    if (x.EndDateUtc.HasValue)
                        m.EndDate = _dateTimeHelper.ConvertToUserTime(x.EndDateUtc.Value, DateTimeKind.Utc);
                    m.CreatedOn = _dateTimeHelper.ConvertToUserTime(x.CreatedOnUtc, DateTimeKind.Utc);
                    m.LanguageName = x.Language.Name;
                    m.Comments = x.CommentCount;
                    return m;
                }),
                Total = blogPosts.TotalCount
            };
			return new JsonResult
			{
				Data = gridModel
			};
		}
        
        public ActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageBlog))
                return AccessDeniedView();

            ViewBag.AllLanguages = _languageService.GetAllLanguages(true);
            var model = new BlogPostModel();
            //Stores
            PrepareStoresMappingModel(model, null, false);
            //default values
            model.AllowComments = true;
            return View(model);
        }

        public ActionResult SendTestmail(int blogid, string testmail)
        {
            var blog = _blogService.GetBlogPostById(blogid);
            if (blog != null)
            {
                BlogPost blogpost = new BlogPost();
                blogpost.Body = blog.Body;
                blogpost.Title = blog.Title;
                string senames = blog.GetSeName(blog.LanguageId, ensureTwoPublishedLanguages: false);
                if (testmail != null)
                {
                    NewsLetterSubscription subscription = new NewsLetterSubscription();
                  
                    _workflowMessageService.SendNewBlogPostNotificationMessage(subscription,testmail, blogpost, _workContext.WorkingLanguage.Id,senames);
                }
            }
                return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Sendmailtolist(int blogid)
        {
            var blog = _blogService.GetBlogPostById(blogid);
            if (blog != null)
            {
                BlogPost blogpost = new BlogPost();
                blogpost.Body = blog.Body;
                blogpost.Title = blog.Title;
                string senames = blog.GetSeName(blog.LanguageId, ensureTwoPublishedLanguages: false);
                var activenewsletter = _newsletterservice.GetAllNewsLetterSubscriptions().Where(x => x.Active);
               
                if (activenewsletter != null)
                {
                    foreach (var item in activenewsletter)
                    {
                        var subscription = _newsLetterSubscriptionService.GetNewsLetterSubscriptionByEmailAndStoreId(item.Email, _storeContext.CurrentStore.Id);
                       _workflowMessageService.SendNewBlogPostNotificationMessage(subscription,item.Email, blogpost, _workContext.WorkingLanguage.Id, senames);
                    }
                }
            }
          
            return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Create(BlogPostModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageBlog))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                //BlogPost blogpost = new BlogPost();
                //blogpost.Body = model.Body;
                //blogpost.Title = model.Title;


                var blogPost = model.ToEntity();
                blogPost.StartDateUtc = model.StartDate;
                blogPost.EndDateUtc = model.EndDate;
                blogPost.CreatedOnUtc = DateTime.UtcNow;
                _blogService.InsertBlogPost(blogPost);
                
                //search engine name
                var seName = blogPost.ValidateSeName(model.SeName, model.Title, true);
                _urlRecordService.SaveSlug(blogPost, seName, blogPost.LanguageId);
               

                //if (model.TestEmail != null)
                //{
                //    _workflowMessageService.SendNewBlogPostNotificationMessage(model.TestEmail, blogpost, _workContext.WorkingLanguage.Id, seName);
                //}
                //else
                //{
                //    if (model.SendMailToList == true)
                //    {
                //        var activenewsletter = _newsletterservice.GetAllNewsLetterSubscriptions().Where(x => x.Active);
                //        if (activenewsletter != null)
                //        {
                //            foreach (var item in activenewsletter)
                //            {
                //                _workflowMessageService.SendNewBlogPostNotificationMessage(item.Email, blogpost, _workContext.WorkingLanguage.Id, seName);
                //            }
                //        }
                //    }

                //}

                //Stores
                SaveStoreMappings(blogPost, model);
                SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.Blog.BlogPosts.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = blogPost.Id }) : RedirectToAction("List");
            }

            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = _languageService.GetAllLanguages(true);
            //Stores
            PrepareStoresMappingModel(model, null, true);
            return View(model);
        }

		public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageBlog))
                return AccessDeniedView();

            var blogPost = _blogService.GetBlogPostById(id);
            if (blogPost == null)
                //No blog post found with the specified id
                return RedirectToAction("List");



            ViewBag.AllLanguages = _languageService.GetAllLanguages(true);
            var model = blogPost.ToModel();
            model.StartDate = blogPost.StartDateUtc;
            model.EndDate = blogPost.EndDateUtc;
            //Store
            PrepareStoresMappingModel(model, blogPost, false);
            return View(model);
		}

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
		public ActionResult Edit(BlogPostModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageBlog))
                return AccessDeniedView();

            var blogPost = _blogService.GetBlogPostById(model.Id);
            if (blogPost == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {

                //BlogPost blogpost = new BlogPost();
                //blogpost.Body = model.Body;
                //blogpost.Title = model.Title;

               
                
                blogPost = model.ToEntity(blogPost);
                blogPost.StartDateUtc = model.StartDate;
                blogPost.EndDateUtc = model.EndDate;
                _blogService.UpdateBlogPost(blogPost);

                //search engine name
                var seName = blogPost.ValidateSeName(model.SeName, model.Title, true);
                _urlRecordService.SaveSlug(blogPost, seName, blogPost.LanguageId);

                //if (model.TestEmail != null)
                //{
                //    _workflowMessageService.SendNewBlogPostNotificationMessage(model.TestEmail, blogpost, _workContext.WorkingLanguage.Id, seName);
                //}
                //else
                //{
                //    if (model.SendMailToList == true)
                //    {
                //        var activenewsletter = _newsletterservice.GetAllNewsLetterSubscriptions().Where(x => x.Active);
                //        if (activenewsletter != null)
                //        {
                //            foreach (var item in activenewsletter)
                //            {
                //                _workflowMessageService.SendNewBlogPostNotificationMessage(item.Email, blogpost, _workContext.WorkingLanguage.Id, seName);
                //            }
                //        }
                //    }

                //}



                //Stores
                SaveStoreMappings(blogPost, model);




                SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.Blog.BlogPosts.Updated"));
                if (continueEditing)
                {
                    //selected tab
                    SaveSelectedTabIndex();

                    return RedirectToAction("Edit", new {id = blogPost.Id});
                }
                else
                {
                    return RedirectToAction("List");
                }
            }

            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = _languageService.GetAllLanguages(true);
            //Store
            PrepareStoresMappingModel(model, blogPost, true);
            return View(model);
		}

		[HttpPost]
		public ActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageBlog))
                return AccessDeniedView();

            var blogPost = _blogService.GetBlogPostById(id);
            if (blogPost == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            _blogService.DeleteBlogPost(blogPost);

            SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.Blog.BlogPosts.Deleted"));
			return RedirectToAction("List");
		}

		#endregion

        #region Comments

        public ActionResult Comments(int? filterByBlogPostId ,int id=0)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageBlog))
                return AccessDeniedView();
            BlogCommentModel model = new BlogCommentModel();
            if (id != 0)
            {
                var blogcomment = _blogService.GetBlogCommentById(id);

                blogcomment.IsApproved = true;

                _blogService.ApprovedComment(blogcomment);
                model.IsApproved = true;
             
            }
            else
            {
                model.IsApproved = false;
            }

            ViewBag.FilterByBlogPostId = filterByBlogPostId;
            return View(model);
        }

        [HttpPost]
        public ActionResult Comments(int? filterByBlogPostId, DataSourceRequest command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageBlog))
                return AccessDeniedView();

            IList<BlogComment> comments;
            if (filterByBlogPostId.HasValue)
            {
                //filter comments by blog
                var blogPost = _blogService.GetBlogPostById(filterByBlogPostId.Value);
                comments = blogPost.BlogComments.OrderBy(bc => bc.CreatedOnUtc).ToList();
            }
            else
            {
                //load all blog comments
                comments = _blogService.GetAllComments(0);
            }

            var gridModel = new DataSourceResult
            {
                Data = comments.PagedForCommand(command).Select(blogComment =>
                {
                    var commentModel = new BlogCommentModel();
                    commentModel.Id = blogComment.Id;
                    commentModel.BlogPostId = blogComment.BlogPostId;
                    commentModel.BlogPostTitle = blogComment.BlogPost.Title;
                    commentModel.IsApproved = blogComment.IsApproved;
                    commentModel.CustomerId = blogComment.CustomerId;
                    var customer = blogComment.Customer;
                    commentModel.CustomerInfo = customer.IsRegistered() ? customer.Email : _localizationService.GetResource("Admin.Customers.Guest");
                    commentModel.CreatedOn = _dateTimeHelper.ConvertToUserTime(blogComment.CreatedOnUtc, DateTimeKind.Utc);
                    commentModel.Comment = Core.Html.HtmlHelper.FormatText(blogComment.CommentText, false, true, false, false, false, false);
                    return commentModel;
                }),
                Total = comments.Count,
            };
            return Json(gridModel);
        }

        public ActionResult CommentDelete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageBlog))
                return AccessDeniedView();

            var comment = _blogService.GetBlogCommentById(id);
            if (comment == null)
                throw new ArgumentException("No comment found with the specified id");

            var blogPost = comment.BlogPost;
            _blogService.DeleteBlogComment(comment);
            //update totals
            blogPost.CommentCount = blogPost.BlogComments.Count;
            _blogService.UpdateBlogPost(blogPost);

            return new NullJsonResult();
        }

        public ActionResult CommentUpdate(BlogCommentModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageBlog))
                return AccessDeniedView();
            if (model == null)
                throw new ArgumentException("No comment found with the specified id");

            var blogcomment = _blogService.GetBlogCommentById(model.Id);

            blogcomment.IsApproved = model.IsApproved;

            _blogService.ApprovedComment(blogcomment);
           
            return new NullJsonResult();
        }
        #endregion
    }
}
