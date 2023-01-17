using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Messages;
using Nop.Core.Domain.Tax;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Directory;
using Nop.Services.Events;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Services.Stores;
using Nop.Services.Messages;
using Nop.Plugin.TorchDesign.Testimonials.Domain;

namespace Nop.Plugin.TorchDesign.Testimonials.Services
{
    public partial class TestimonialTokenProvider : ITestimonialTokenProvider
    {
        #region Fields

        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IPriceFormatter _priceFormatter;
        private readonly ICurrencyService _currencyService;
        private readonly IWorkContext _workContext;
        private readonly IDownloadService _downloadService;
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;
        private readonly IProductAttributeParser _productAttributeParser;
        private readonly IStoreService _storeService;
        private readonly IStoreContext _storeContext;

        private readonly MessageTemplatesSettings _templatesSettings;
        private readonly CatalogSettings _catalogSettings;
        private readonly TaxSettings _taxSettings;

        private readonly IEventPublisher _eventPublisher;
     
        private readonly IPictureService _pictureServices;

        private readonly IProductService _productServices;
        private readonly ISpecificationAttributeService _specificationAttributeService;

        #endregion

        #region Ctor

        public TestimonialTokenProvider(ILanguageService languageService,
            ILocalizationService localizationService, 
            IDateTimeHelper dateTimeHelper,
            IPriceFormatter priceFormatter, 
            ICurrencyService currencyService,
            IWorkContext workContext,
            IDownloadService downloadService,
            IOrderService orderService,
            IPaymentService paymentService,
            IStoreService storeService,
            IStoreContext storeContext,
            IProductAttributeParser productAttributeParser,
            MessageTemplatesSettings templatesSettings,
            CatalogSettings catalogSettings,
            TaxSettings taxSettings, 
            IEventPublisher eventPublisher,
          
            IPictureService pictureServices,
            IProductService productServices,
            ISpecificationAttributeService specificationAttributeService) 
        {
            this._languageService = languageService;
            this._localizationService = localizationService;
            this._dateTimeHelper = dateTimeHelper;
            this._priceFormatter = priceFormatter;
            this._currencyService = currencyService;
            this._workContext = workContext;
            this._downloadService = downloadService;
            this._orderService = orderService;
            this._paymentService = paymentService;
            this._productAttributeParser = productAttributeParser;
            this._storeService = storeService;
            this._storeContext = storeContext;

            this._templatesSettings = templatesSettings;
            this._catalogSettings = catalogSettings;
            this._taxSettings = taxSettings;
            this._eventPublisher = eventPublisher;

       
            this._pictureServices = pictureServices;
            this._productServices = productServices;
            this._specificationAttributeService = specificationAttributeService;

        }

        #endregion

        #region Utilities     

        /// <summary>
        /// Get store URL
        /// </summary>
        /// <param name="storeId">Store identifier; Pass 0 to load URL of the current store</param>
        /// <param name="useSsl">Use SSL</param>
        /// <returns></returns>
        protected virtual string GetStoreUrl(int storeId = 0, bool useSsl = false)
        {
            var store = _storeService.GetStoreById(storeId) ?? _storeContext.CurrentStore;

            if (store == null)
                throw new Exception("No store could be loaded");

            return useSsl ? store.SecureUrl : store.Url;
        }

        #endregion

        #region Methods
        //Custom Token
        public virtual void AddTestimonialTokens(IList<Token> tokens, Td_Testimonials testimonial, int[] pictureid, int languageId) 
        {
            tokens.Add(new Token("testimonial.location", testimonial.Location));
            tokens.Add(new Token("testimonial.CustomerEmail", testimonial.Email.ToString()));
            tokens.Add(new Token("testimonial.SubmittedOn", testimonial.CreateOn.ToString()));
            tokens.Add(new Token("testimonial.Message", testimonial.Message));
            tokens.Add(new Token("testimonial.CustomerName", testimonial.CustomerName.ToString()));
            tokens.Add(new Token("testimonial.Id", testimonial.Id.ToString()));
            tokens.Add(new Token("testimonial.ActiveId", "Active-"+ (testimonial.Id.ToString())));
            tokens.Add(new Token("testimonial.RejectId", "Reject-"+ (testimonial.Id.ToString())));

            const string urlFormat = "{0}Testimonials/ApproveTestimonial/{1}";

            var testimonialApproveLink = String.Format(urlFormat, GetStoreUrl(), testimonial.Id);
            tokens.Add(new Token("testimonial.ApproveUrl", testimonialApproveLink,true));

            
            //TODO add a method for getting URL (use routing because it handles all SEO friendly URLs)
            // int id = testimonial.Id;

            StringBuilder sb =new StringBuilder();
            sb.Append("<div>");
            if (pictureid != null)
            {
                if (pictureid.Count() > 0)
                {
                    for (int i = 0; i < pictureid.Count(); i++)
                    {
                        var pictureUrl = _pictureServices.GetPictureUrl(_pictureServices.GetPictureById(pictureid[i]),0);
                        // var tag = "<img width:'100px' height='100px' src='" + pictureUrl + "'/>";
                        var tag = string.Format("<img src=\"{0}\"/>", pictureUrl);
                        sb.AppendLine(tag);
                    }
                }
            }
            sb.Append("</div>");
            //testimonial Image
            //var picture = _pictureServices.GetPictureById(testimonial., 1).FirstOrDefault();
            //var pictureUrl = _pictureServices.GetPictureUrl(picture);
            tokens.Add(new Token("testimonial.ImageUrl", sb.ToString(),true));
            
            _eventPublisher.EntityTokensAdded(testimonial, tokens);
        }


        /// <summary>
        /// Gets list of allowed (supported) message tokens for campaigns
        /// </summary>
        /// <returns>List of allowed (supported) message tokens for campaigns</returns>
       
        public virtual string[] GetListOfAllowedTokens()
        {
            var allowedTokens = new List<string>()
            {
                "%testimonial.location%",
                "%testimonial.Message%",
                "%testimonial.CustomerName%",
                "%testimonial.Id%",
                "%testimonial.ImageUrl%",
                "%testimonial.ActiveId%",
                 "%testimonial.RejectId%",
               
            };
            return allowedTokens.ToArray();
        }
        
        #endregion
    }
}
