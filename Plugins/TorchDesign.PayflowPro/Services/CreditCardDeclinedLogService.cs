using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Plugin.TorchDesign.PayflowPro.Domain;
using Nop.Core.Data;
using Nop.Services.Logging;
using Nop.Core.Domain.Customers;
using Nop.Data;
using Nop.Core.Plugins;
using Nop.Core.Caching;

namespace Nop.Plugin.TorchDesign.PayflowPro.Services
{
    public partial class CreditCardDeclinedLogService : ICreditCardDeclinedLogService
    {
        #region Fields

        private readonly IRepository<Td_CreditCardDeclinedLog> _tdCreditCardDeclinedLogRepository;
        private readonly ILogger _logger;
        private readonly IDbContext _dbContext;
        private readonly IPluginFinder _pluginFinder;

        #endregion

        #region Constructor

        public CreditCardDeclinedLogService(IRepository<Td_CreditCardDeclinedLog> tdCreditCardDeclinedLogRepository, ILogger logger,
                                             IDbContext dbContext, IPluginFinder pluginFinder)
        {
            _tdCreditCardDeclinedLogRepository = tdCreditCardDeclinedLogRepository;
            _logger = logger;
            _dbContext = dbContext;
            _pluginFinder = pluginFinder;
        }

        #endregion

        #region Methods

        public void DeleteCreditCardDeclinedLog(Td_CreditCardDeclinedLog td_CreditCardDeclinedLog)
        {
            if (td_CreditCardDeclinedLog == null)
                throw new ArgumentNullException("td_CreditCardDeclinedLog");

            _tdCreditCardDeclinedLogRepository.Delete(td_CreditCardDeclinedLog);
        }

        public Td_CreditCardDeclinedLog GetCreditCardDeclinedLogById(int td_CreditCardDeclinedLogId)
        {
            if (td_CreditCardDeclinedLogId == 0)
                return null;

            return _tdCreditCardDeclinedLogRepository.GetById(td_CreditCardDeclinedLogId);
        }

        public void InsertCreditCardDeclinedLog(Td_CreditCardDeclinedLog td_CreditCardDeclinedLog)
        {
            if (td_CreditCardDeclinedLog == null)
                throw new ArgumentNullException("td_CreditCardDeclinedLog");

            _tdCreditCardDeclinedLogRepository.Insert(td_CreditCardDeclinedLog);
        }

        public IPagedList<Td_CreditCardDeclinedLog> SearchTd_CreditCardDeclinedLogs(int customerId = 0, DateTime? createdOn = default(DateTime?), int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _tdCreditCardDeclinedLogRepository.Table;

            if (customerId > 0)
                query = query.Where(t => t.CustomerId == customerId);
            if (createdOn.HasValue)
                query = query.Where(t => t.CreatedOn.Equals(createdOn.Value));

            query = query.OrderByDescending(t => t.CreatedOn);
            return new PagedList<Td_CreditCardDeclinedLog>(query, pageIndex, pageSize);
        }

        public void UpdateCreditCardDeclinedLog(Td_CreditCardDeclinedLog td_CreditCardDeclinedLog)
        {
            if (td_CreditCardDeclinedLog == null)
                throw new ArgumentNullException("td_CreditCardDeclinedLog");

            _tdCreditCardDeclinedLogRepository.Update(td_CreditCardDeclinedLog);
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
