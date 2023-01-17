using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Plugin.TorchDesign.MegaSearch.Domain;
using System.Net;
using System.Xml.Linq;

namespace Nop.Plugin.TorchDesign.MegaSearch.Services
{
    public partial class MegaSearchService : IMegaSearchService
    {
        //private readonly IRepository<Td_Zip2TaxCounter> _Zip2TaxCounter;
        //private readonly IRepository<Td_MegaSearch> _Zip2Tax;
        //private readonly TorchDesignMegaSearchSettings _megasearchsetting;


        //public MegaSearchService(IRepository<Td_Zip2TaxCounter> Zip2TaxCounter, IRepository<Td_MegaSearch> Zip2Tax, TorchDesignMegaSearchSettings zipsetting)
        //{
        //    this._Zip2TaxCounter = Zip2TaxCounter;
        //    this._Zip2Tax = Zip2Tax;
        //    this._megasearchsetting = zipsetting;
        //}

        //public Td_Zip2TaxCounter GetZip2TaxCounterById(int id)
        //{
        //    if (id == 0)
        //        return null;
        //    return _Zip2TaxCounter.GetById(id);
        //}

        //public Td_Zip2TaxCounter GetCounterByZipcode(string Zipcode)
        //{
        //    if (Zipcode.Length < 1)
        //        throw new ArgumentNullException("Enter Zipcode");

        //    var query = from a in _Zip2TaxCounter.Table where a.Zipcode == Zipcode select a;
        //    return query.LastOrDefault();
        //}

        //public virtual IPagedList<Td_Zip2TaxCounter> SearchCall(DateTime? createdFromUtc = null, DateTime? createdToUtc = null, int pageIndex = 0, int pageSize = int.MaxValue)
        //{
        //    var query = _Zip2TaxCounter.Table;
        //    if (createdFromUtc.HasValue)
        //        query = query.Where(o => createdFromUtc.Value <= o.CallDate);
        //    if (createdToUtc.HasValue)
        //        query = query.Where(o => createdToUtc.Value >= o.CallDate);
        //    query = query.OrderByDescending(o => o.CallDate);

        //    return new PagedList<Td_Zip2TaxCounter>(query, pageIndex, pageSize);


        //}




        //public void InsertZip2TaxCounter(Td_Zip2TaxCounter record)
        //{
        //    if (record == null)
        //        throw new ArgumentNullException("record");

        //    _Zip2TaxCounter.Insert(record);
        //}

        //public void UpdateZip2TaxCounter(Td_Zip2TaxCounter record)
        //{
        //    if (record == null)
        //        throw new ArgumentNullException("record");

        //    _Zip2TaxCounter.Update(record);
        //}

        //public void DeleteZip2TaxCounter(Td_Zip2TaxCounter record)
        //{
        //    if (record == null)
        //        throw new ArgumentNullException("record");
        //    _Zip2TaxCounter.Delete(record);
        //}

        //public IPagedList<Td_Zip2TaxCounter> GetAllZip2TaxCounter(int pageIndex = 0, int pageSize = int.MaxValue)
        //{
        //    var query = from a in _Zip2TaxCounter.Table
        //                orderby a.Id
        //                select a;
        //    var records = new PagedList<Td_Zip2TaxCounter>(query, pageIndex, pageSize);
        //    return records;
        //}

        ////public virtual IList<Td_Zip2TaxCounter> GetAllZip2TaxCounterlist()
        ////{
        ////    var query = from sliders in _Zip2TaxCounter.Table     select sliders;

        ////    return query.ToList();
        ////}











        //public Td_MegaSearch GetZip2TaxById(int id)
        //{
        //    if (id == 0)
        //        return null;
        //    return _Zip2Tax.GetById(id);
        //}

        //public void InsertZip2Tax(Td_MegaSearch record)
        //{
        //    if (record == null)
        //        throw new ArgumentNullException("record");

        //    _Zip2Tax.Insert(record);
        //}


        //public void UpdateZip2Tax(Td_MegaSearch record)
        //{
        //    if (record == null)
        //        throw new ArgumentNullException("record");

        //    _Zip2Tax.Update(record);
        //}

        //public void DeleteZip2Tax(Td_MegaSearch record)
        //{
        //    if (record == null)
        //        throw new ArgumentNullException("record");
        //    _Zip2Tax.Delete(record);
        //}

        //public IPagedList<Td_MegaSearch> GetAllZip2Tax(int pageIndex = 0, int pageSize = int.MaxValue)
        //{
        //    var query = from a in _Zip2Tax.Table
        //                orderby a.Id
        //                select a;
        //    var records = new PagedList<Td_MegaSearch>(query, pageIndex, pageSize);
        //    return records;
        //}

        //public Td_MegaSearch GetAvailableTaxDetailRowByZipcode(string Zipcode)
        //{
        //    if (Zipcode.Length < 1)
        //        throw new ArgumentNullException("Enter Zipcode");

        //    var query = from a in _Zip2Tax.Table where a.Zipcode == Zipcode select a;
        //    return query.SingleOrDefault();
        //}
        ////------Calculate Taxrate From Zipcode--------------------------------------------------------------------------

       
      
    }
}
