using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Plugin.TorchDesign.CustomerOrigin.Domain;
using System.Net;
using System.Xml.Linq;

namespace Nop.Plugin.TorchDesign.CustomerOrigin.Services
{
    public partial class CustomerOriginService : ICustomerOriginService
    {
        private readonly IRepository<Td_CustomerOriginAnswer> _CustomerOriginAnswer;
        private readonly IRepository<Td_CustomerOrigin> _CustomerOrigin;
        private readonly TorchDesignCustomerOriginSettings _originsetting;


        public CustomerOriginService(IRepository<Td_CustomerOriginAnswer> CustomerOriginAnswer, IRepository<Td_CustomerOrigin> CustomerOrigin, TorchDesignCustomerOriginSettings originsetting)
        {
            this._CustomerOriginAnswer = CustomerOriginAnswer;
            this._CustomerOrigin = CustomerOrigin;
            this._originsetting = originsetting;
        }

        public IList<Td_CustomerOriginAnswer> GetCustomerOriginAnswerByOptionid(int id)
        {
            if (id == 0)
                return null;
            var query = (from a in _CustomerOriginAnswer.Table
                        where a.Optionid == id
                         select a).ToList(); ;
            return query;
        }

      

        public virtual IList<Td_CustomerOriginAnswer> Findbydate(DateTime? createdFromUtc = null, DateTime? createdToUtc = null,int id=0)
        {
            var query = _CustomerOriginAnswer.Table.Where(x=> id == x.Optionid);
            if (createdFromUtc.HasValue)
                query = query.Where(o => createdFromUtc.Value <= o.GivenOn);
            if (createdToUtc.HasValue)
                query = query.Where(o => createdToUtc.Value >= o.GivenOn);
            query = query.OrderByDescending(o => o.GivenOn);

            return query.ToList();


        }




        public void InsertCustomerOriginAnswer(Td_CustomerOriginAnswer record)
        {
            if (record == null)
                throw new ArgumentNullException("record");

            _CustomerOriginAnswer.Insert(record);
        }

        public void UpdateCustomerOriginAnswer(Td_CustomerOriginAnswer record)
        {
            if (record == null)
                throw new ArgumentNullException("record");

            _CustomerOriginAnswer.Update(record);
        }

        public void DeleteCustomerOriginAnswer(Td_CustomerOriginAnswer record)
        {
            if (record == null)
                throw new ArgumentNullException("record");
            _CustomerOriginAnswer.Delete(record);
        }

        public IPagedList<Td_CustomerOriginAnswer> GetAllCustomerOriginAnswer(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = from a in _CustomerOriginAnswer.Table
                        orderby a.Id
                        select a;
            var records = new PagedList<Td_CustomerOriginAnswer>(query, pageIndex, pageSize);
            return records;
        }

      










        public Td_CustomerOrigin GetCustomerOriginById(int id)
        {
            if (id == 0)
                return null;
            return _CustomerOrigin.GetById(id);
        }

        public void InsertCustomerOrigin(Td_CustomerOrigin record)
        {
            if (record == null)
                throw new ArgumentNullException("record");

            _CustomerOrigin.Insert(record);
        }


        public void UpdateCustomerOrigin(Td_CustomerOrigin record)
        {
            if (record == null)
                throw new ArgumentNullException("record");

            _CustomerOrigin.Update(record);
        }

        public void DeleteCustomerOrigin(Td_CustomerOrigin record)
        {
            if (record == null)
                throw new ArgumentNullException("record");
            _CustomerOrigin.Delete(record);
        }

        public IPagedList<Td_CustomerOrigin> GetAllCustomerOrigin(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = from a in _CustomerOrigin.Table
                        orderby a.Id
                        select a;
            var records = new PagedList<Td_CustomerOrigin>(query, pageIndex, pageSize);
            return records;
        }

        public Td_CustomerOriginAnswer CustomerExistOrNotInOriginTable(int customerid)
        {
            var query = (from a in _CustomerOriginAnswer.Table
                         where a.CustomerId == customerid
                         orderby a.GivenOn descending
                         select a).FirstOrDefault();
            
            return query;
         
        }
     
    }
}
