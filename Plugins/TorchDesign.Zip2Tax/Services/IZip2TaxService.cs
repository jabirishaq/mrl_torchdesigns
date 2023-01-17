using Nop.Core;
using Nop.Plugin.TorchDesign.Zip2Tax.Domain;
using System;
using System.Collections.Generic;

namespace Nop.Plugin.TorchDesign.Zip2Tax.Services
{
    public partial interface IZip2TaxService
    {

        Td_Zip2TaxCounter GetZip2TaxCounterById(int id);

        Td_Zip2TaxCounter GetCounterByZipcode(string Zipcode);

        //IList<Td_Zip2TaxCounter> GetAllZip2TaxCounterlist();

        void InsertZip2TaxCounter(Td_Zip2TaxCounter record);

        void UpdateZip2TaxCounter(Td_Zip2TaxCounter record);

        void DeleteZip2TaxCounter(Td_Zip2TaxCounter record);

        IPagedList<Td_Zip2TaxCounter> GetAllZip2TaxCounter(int pageIndex = 0, int pageSize = int.MaxValue);

        IPagedList<Td_Zip2TaxCounter> SearchCall(DateTime? createdFromUtc = null, DateTime? createdToUtc = null, int pageIndex = 0, int pageSize = int.MaxValue);

        string GetCounty(string Zipcode);


        Td_Zip2Tax GetZip2TaxById(int id);

        Td_Zip2Tax GetAvailableTaxDetailRowByZipcode(string Zipcode);

        decimal GetAvailableTaxDetailByZipcode(string Zipcode,string stateCode = null, bool isFromConfigure = false);

      
       

        //IList<Td_Zip2Tax> GetAllZip2Taxlist();

        void InsertZip2Tax(Td_Zip2Tax record);

        void UpdateZip2Tax(Td_Zip2Tax record);

        void DeleteZip2Tax(Td_Zip2Tax record);

        IPagedList<Td_Zip2Tax> GetAllZip2Tax(int pageIndex = 0, int pageSize = int.MaxValue);

        string GetCountyNameUsingZip2Tax(string zipCode);


    }
}
