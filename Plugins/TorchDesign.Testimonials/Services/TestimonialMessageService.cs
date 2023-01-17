using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core;
using Nop.Core.Domain.Blogs;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Forums;
using Nop.Core.Domain.Messages;
using Nop.Core.Domain.News;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Shipping;
using Nop.Core.Domain.Stores;
using Nop.Core.Domain.Vendors;
using Nop.Services.Customers;
using Nop.Services.Events;
using Nop.Services.Localization;
using Nop.Services.Stores;
using Nop.Services.Messages;
using Nop.Core.Domain.Discounts;
using System.Web;
using Nop.Plugin.TorchDesign.Testimonials.Models;
using Nop.Plugin.TorchDesign.Testimonials.Domain;


namespace Nop.Plugin.TorchDesign.Testimonials.Services 
{
    public partial class TestimonialMessageService : Nop.Services.Messages.WorkflowMessageService, ITestimonialMessageService
    {
        #region Fields

        private readonly IMessageTemplateService _messageTemplateService;
        private readonly IQueuedEmailService _queuedEmailService;
        private readonly ILanguageService _languageService;
        private readonly ITokenizer _tokenizer;
        private readonly IEmailAccountService _emailAccountService;
        private readonly IMessageTokenProvider _messageTokenProvider;
        private readonly IStoreService _storeService;
        private readonly IStoreContext _storeContext;

        private readonly EmailAccountSettings _emailAccountSettings;
        private readonly IEventPublisher _eventPublisher;

        private readonly ITestimonialTokenProvider _TestimonialTokenProvider;
        private readonly NewsSettings _newsSettings;
        private readonly Nop.Services.Messages.IMessageTokenProvider _coreMessageTokenProvider;
        private HttpContextBase _context { get; set; }
        private readonly ICustomerService _customerService;

        #endregion

        #region Ctor

        public TestimonialMessageService(IMessageTemplateService messageTemplateService,
            HttpContextBase context,
            IQueuedEmailService queuedEmailService, 
            ILanguageService languageService,
            ITokenizer tokenizer,
            IEmailAccountService emailAccountService,
            IMessageTokenProvider messageTokenProvider,
            IStoreService storeService,
            IStoreContext storeContext,
            EmailAccountSettings emailAccountSettings,
            IEventPublisher eventPublisher,
            Nop.Services.Messages.IMessageTokenProvider coreMessageTokenProvider, NewsSettings newsSettings,
            ITestimonialTokenProvider TestimonialTokenProvider,
            ICustomerService customerService)
            : base(messageTemplateService, queuedEmailService, languageService, tokenizer, emailAccountService, coreMessageTokenProvider, storeService, storeContext, emailAccountSettings, eventPublisher, newsSettings)
        {
            this._messageTemplateService = messageTemplateService;
            this._queuedEmailService = queuedEmailService;
            this._languageService = languageService;
            this._tokenizer = tokenizer;
            this._emailAccountService = emailAccountService;
            this._messageTokenProvider = messageTokenProvider;
            this._storeService = storeService;
            this._storeContext = storeContext;
            this._context = context;
            this._emailAccountSettings = emailAccountSettings;
            this._eventPublisher = eventPublisher;
            this._coreMessageTokenProvider = coreMessageTokenProvider;
            this._TestimonialTokenProvider = TestimonialTokenProvider;
            this._customerService = customerService;
        }

        #endregion

        #region Utilities

        private int SendNotification(MessageTemplate messageTemplate,
            EmailAccount emailAccount, int languageId, IEnumerable<Token> tokens,
            string toEmailAddress, string toName)
        {
            //retrieve localized message template data
            var bcc = messageTemplate.GetLocalized((mt) => mt.BccEmailAddresses, languageId);
            var subject = messageTemplate.GetLocalized((mt) => mt.Subject, languageId);
            var body = messageTemplate.GetLocalized((mt) => mt.Body, languageId);

            //Replace subject and body tokens 
            var subjectReplaced = _tokenizer.Replace(subject, tokens, false);
            var bodyReplaced = _tokenizer.Replace(body, tokens, true);

            var email = new QueuedEmail()
            {
                Priority = 5,
                From = emailAccount.Email,
                FromName = emailAccount.DisplayName,
                To = toEmailAddress,
                ToName = toName,
                CC = string.Empty,
                Bcc = bcc,
                Subject = subjectReplaced,
                Body = bodyReplaced,
                CreatedOnUtc = DateTime.UtcNow,
                EmailAccountId = emailAccount.Id
            };

            _queuedEmailService.InsertQueuedEmail(email);
            return email.Id;
        }

        private MessageTemplate GetLocalizedActiveMessageTemplate(string messageTemplateName,
            int languageId, int storeId)
        {
            //TODO remove languageId parameter
            var messageTemplate = _messageTemplateService.GetMessageTemplateByName(messageTemplateName, storeId);

            //no template found
            if (messageTemplate == null)
                return null;

            //ensure it's active
            var isActive = messageTemplate.IsActive;
            if (!isActive)
                return null;

            return messageTemplate;
        }

        private EmailAccount GetEmailAccountOfMessageTemplate(MessageTemplate messageTemplate, int languageId)
        {
            var emailAccounId = messageTemplate.GetLocalized(mt => mt.EmailAccountId, languageId);
            var emailAccount = _emailAccountService.GetEmailAccountById(emailAccounId);
            if (emailAccount == null)
                emailAccount = _emailAccountService.GetEmailAccountById(_emailAccountSettings.DefaultEmailAccountId);
            if (emailAccount == null)
                emailAccount = _emailAccountService.GetAllEmailAccounts().FirstOrDefault();
            return emailAccount;

        }

        private int EnsureLanguageIsActive(int languageId, int storeId)
        {
            //load language by specified ID
            var language = _languageService.GetLanguageById(languageId);

            if (language == null || !language.Published)
            {
                //load any language from the specified store
                language = _languageService.GetAllLanguages(storeId: storeId).FirstOrDefault();
            }
            if (language == null || !language.Published)
            {
                //load any language
                language = _languageService.GetAllLanguages().FirstOrDefault();
            }

            if (language == null)
                throw new Exception("No active language could be loaded");
            return language.Id;
        }

        #endregion

        #region Methods

        #region Customer workflow

        /// <summary>
        /// Sends 'New customer' notification message to a store owner
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual int SendCustomerTestimonialMail(Td_Testimonials testimonial, int[] pictureid, int languageId)
        {
            if (testimonial == null)
                throw new ArgumentNullException("No testominial Found");
            var store = _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplate = GetActiveMessageTemplate("Testimonial.SendTestimonialEmail", store.Id);
            if (messageTemplate == null)
                return 0;

            //email account
            var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

            var tokens = new List<Token>();
            var customer = _customerService.GetCustomerById(testimonial.CustomerId);
            if (customer != null)
                _messageTokenProvider.AddCustomerTokens(tokens, customer);

            _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);
            _TestimonialTokenProvider.AddTestimonialTokens(tokens, testimonial, pictureid, languageId);

           
            _eventPublisher.MessageTokensAdded(messageTemplate, tokens);


            var toEmail = emailAccount.Email;
            
            var toName = emailAccount.DisplayName;
            return SendNotification(messageTemplate, emailAccount,
                languageId, tokens,
                toEmail, toName,null,null,testimonial.Email,null);
        }


        #endregion


        #endregion
    }
}
