using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Plugin.Payments.AuthorizeNet.Domain;
using System;

namespace Nop.Plugin.Payments.AuthorizeNet.Services
{
    public partial interface IAuthorizeNetCardDeclinedLogService
    {
        void InsertAuthorizeNetCardDeclinedLog(Td_AuthorizeNet_DeclinedCard_Log td_AuthorizeNet_DeclinedCard_Log);

        void UpdateAuthorizeNetCardDeclinedLog(Td_AuthorizeNet_DeclinedCard_Log td_AuthorizeNet_DeclinedCard_Log);

        void DeleteAuthorizeNetCardDeclinedLog(Td_AuthorizeNet_DeclinedCard_Log td_AuthorizeNet_DeclinedCard_Log);

        Td_AuthorizeNet_DeclinedCard_Log GetAuthorizeNetCardDeclinedLogById(int td_AuthorizeNet_DeclinedCard_LogId);

        IPagedList<Td_AuthorizeNet_DeclinedCard_Log> SearchAuthorizeNetCardDeclinedLogs(int customerId = 0, DateTime? createdOn = null, int pageIndex = 0, int pageSize = int.MaxValue);

        void AddIPToBannedList(string ipAddress, Customer customer);
    }
}
