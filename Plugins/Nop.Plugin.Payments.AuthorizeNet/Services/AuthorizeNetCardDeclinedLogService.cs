using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.Customers;
using Nop.Core.Plugins;
using Nop.Data;
using Nop.Plugin.Payments.AuthorizeNet.Domain;
using Nop.Services.Logging;
using System;
using System.Linq;

namespace Nop.Plugin.Payments.AuthorizeNet.Services
{
    public partial class AuthorizeNetCardDeclinedLogService : IAuthorizeNetCardDeclinedLogService
    {
        #region Fields

        private readonly IRepository<Td_AuthorizeNet_DeclinedCard_Log> _tdAuthorizeNetDeclinedCardLogRepository;
        private readonly ILogger _logger;
        private readonly IDbContext _dbContext;
        private readonly IPluginFinder _pluginFinder;

        #endregion

        #region Constructor

        public AuthorizeNetCardDeclinedLogService(IRepository<Td_AuthorizeNet_DeclinedCard_Log> tdAuthorizeNetDeclinedCardLogRepository, ILogger logger,
                                             IDbContext dbContext, IPluginFinder pluginFinder)
        {
            _tdAuthorizeNetDeclinedCardLogRepository = tdAuthorizeNetDeclinedCardLogRepository;
            _logger = logger;
            _dbContext = dbContext;
            _pluginFinder = pluginFinder;
        }

        #endregion

        #region Methods

        public virtual void DeleteAuthorizeNetCardDeclinedLog(Td_AuthorizeNet_DeclinedCard_Log td_AuthorizeNet_DeclinedCard_Log)
        {
            if (td_AuthorizeNet_DeclinedCard_Log == null)
                throw new ArgumentNullException("td_AuthorizeNet_DeclinedCard_Log");

            _tdAuthorizeNetDeclinedCardLogRepository.Delete(td_AuthorizeNet_DeclinedCard_Log);
        }

        public virtual Td_AuthorizeNet_DeclinedCard_Log GetAuthorizeNetCardDeclinedLogById(int td_AuthorizeNet_DeclinedCard_LogId)
        {
            if (td_AuthorizeNet_DeclinedCard_LogId == 0)
                return null;

            return _tdAuthorizeNetDeclinedCardLogRepository.GetById(td_AuthorizeNet_DeclinedCard_LogId);
        }

        public virtual void InsertAuthorizeNetCardDeclinedLog(Td_AuthorizeNet_DeclinedCard_Log td_AuthorizeNet_DeclinedCard_Log)
        {
            if (td_AuthorizeNet_DeclinedCard_Log == null)
                throw new ArgumentNullException("td_AuthorizeNet_DeclinedCard_Log");

            _tdAuthorizeNetDeclinedCardLogRepository.Insert(td_AuthorizeNet_DeclinedCard_Log);
        }

        public virtual IPagedList<Td_AuthorizeNet_DeclinedCard_Log> SearchAuthorizeNetCardDeclinedLogs(int customerId = 0, DateTime? createdOn = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _tdAuthorizeNetDeclinedCardLogRepository.Table;

            if (customerId > 0)
                query = query.Where(t => t.CustomerId == customerId);
            if (createdOn.HasValue)
                query = query.Where(t => t.CreatedOn.Equals(createdOn.Value));

            query = query.OrderByDescending(t => t.CreatedOn);
            return new PagedList<Td_AuthorizeNet_DeclinedCard_Log>(query, pageIndex, pageSize);
        }

        public virtual void UpdateAuthorizeNetCardDeclinedLog(Td_AuthorizeNet_DeclinedCard_Log td_AuthorizeNet_DeclinedCard_Log)
        {
            if (td_AuthorizeNet_DeclinedCard_Log == null)
                throw new ArgumentNullException("td_AuthorizeNet_DeclinedCard_Log");

            _tdAuthorizeNetDeclinedCardLogRepository.Update(td_AuthorizeNet_DeclinedCard_Log);
        }

        public virtual void AddIPToBannedList(string ipAddress, Customer customer)
        {
            var plugin = _pluginFinder.GetPluginDescriptorBySystemName("FoxNetSoft.Plugin.Misc.IPFilter", true);
            if (plugin.Installed)
            {
                if (customer == null)
                    throw new ArgumentNullException("customer");

                if (string.IsNullOrEmpty(ipAddress))
                    _logger.Error("IPAddress is null or empty while adding to banned list.", null, customer);

                _dbContext.ExecuteSqlCommand(string.Format("Insert into FNS_IPFilter_IP (StoreId,IpAddress,Allowed,CreatedOnUtc) values ({0},'{1}',{2},'{3}'); ", 0, ipAddress, 0, DateTime.UtcNow));

                var cacheManager = new MemoryCacheManager();
                cacheManager.Clear();
            }
            else
            {
                _logger.Error("Couldn't add IPaddress to banned list as FoxNetSoft IPFilter (Country, region, anti hacker) plugin is not installed.");
            }
        }

        #endregion
    }
}
