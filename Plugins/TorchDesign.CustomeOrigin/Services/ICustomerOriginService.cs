using Nop.Core;
using Nop.Plugin.TorchDesign.CustomerOrigin.Domain;
using System;
using System.Collections.Generic;

namespace Nop.Plugin.TorchDesign.CustomerOrigin.Services
{
    public partial interface ICustomerOriginService
    {

        IList<Td_CustomerOriginAnswer> GetCustomerOriginAnswerByOptionid(int id);

        void InsertCustomerOriginAnswer(Td_CustomerOriginAnswer record);

        void UpdateCustomerOriginAnswer(Td_CustomerOriginAnswer record);

        void DeleteCustomerOriginAnswer(Td_CustomerOriginAnswer record);

        IPagedList<Td_CustomerOriginAnswer> GetAllCustomerOriginAnswer(int pageIndex = 0, int pageSize = int.MaxValue);

        IList<Td_CustomerOriginAnswer> Findbydate(DateTime? createdFromUtc = null, DateTime? createdToUtc = null,int id=0);




        Td_CustomerOrigin GetCustomerOriginById(int id);

        void InsertCustomerOrigin(Td_CustomerOrigin record);

        void UpdateCustomerOrigin(Td_CustomerOrigin record);

        void DeleteCustomerOrigin(Td_CustomerOrigin record);

        IPagedList<Td_CustomerOrigin> GetAllCustomerOrigin(int pageIndex = 0, int pageSize = int.MaxValue);

        Td_CustomerOriginAnswer CustomerExistOrNotInOriginTable(int customerid);


    }
}
