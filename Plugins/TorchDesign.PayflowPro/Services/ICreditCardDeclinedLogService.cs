using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Plugin.TorchDesign.PayflowPro.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.TorchDesign.PayflowPro.Services
{
    public partial interface ICreditCardDeclinedLogService
    {
        void InsertCreditCardDeclinedLog(Td_CreditCardDeclinedLog td_CreditCardDeclinedLog);

        void UpdateCreditCardDeclinedLog(Td_CreditCardDeclinedLog td_CreditCardDeclinedLog);

        void DeleteCreditCardDeclinedLog(Td_CreditCardDeclinedLog td_CreditCardDeclinedLog);

        Td_CreditCardDeclinedLog GetCreditCardDeclinedLogById(int td_CreditCardDeclinedLogId);

        IPagedList<Td_CreditCardDeclinedLog> SearchTd_CreditCardDeclinedLogs(int customerId = 0, DateTime? createdOn = null, int pageIndex = 0, int pageSize = int.MaxValue);

        void AddIPToBannedList(string ipAddress, Customer customer);
    }
}
