using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Plugin.TorchDesign.Zip2Tax.Domain;
using System.Net;
using System.Xml.Linq;
using Nop.Services.Logging;
using System.Net.Http;

namespace Nop.Plugin.TorchDesign.Zip2Tax.Services
{
    public partial class Zip2TaxService : IZip2TaxService
    {
        private readonly IRepository<Td_Zip2TaxCounter> _Zip2TaxCounter;
        private readonly IRepository<Td_Zip2Tax> _Zip2Tax;
        private readonly TorchDesignZip2TaxSettings _zipsetting;
        private readonly ILogger _logger;

        public Zip2TaxService(IRepository<Td_Zip2TaxCounter> Zip2TaxCounter, IRepository<Td_Zip2Tax> Zip2Tax, TorchDesignZip2TaxSettings zipsetting,
                              ILogger logger)
        {
            this._Zip2TaxCounter = Zip2TaxCounter;
            this._Zip2Tax = Zip2Tax;
            this._zipsetting = zipsetting;
            this._logger = logger;
        }

        public Td_Zip2TaxCounter GetZip2TaxCounterById(int id)
        {
            if (id == 0)
                return null;
            return _Zip2TaxCounter.GetById(id);
        }

        public Td_Zip2TaxCounter GetCounterByZipcode(string Zipcode)
        {
            if (Zipcode.Length < 1)
                throw new ArgumentNullException("A valid Zip Code is required.");

            var query = from a in _Zip2TaxCounter.Table where a.Zipcode == Zipcode select a;
            return query.LastOrDefault();
        }

        public virtual IPagedList<Td_Zip2TaxCounter> SearchCall(DateTime? createdFromUtc = null, DateTime? createdToUtc = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _Zip2TaxCounter.Table;
            if (createdFromUtc.HasValue)
                query = query.Where(o => createdFromUtc.Value <= o.CallDate);
            if (createdToUtc.HasValue)
                query = query.Where(o => createdToUtc.Value >= o.CallDate);
            query = query.OrderByDescending(o => o.CallDate);

            return new PagedList<Td_Zip2TaxCounter>(query, pageIndex, pageSize);


        }

        public void InsertZip2TaxCounter(Td_Zip2TaxCounter record)
        {
            if (record == null)
                throw new ArgumentNullException("record");

            _Zip2TaxCounter.Insert(record);
        }

        public void UpdateZip2TaxCounter(Td_Zip2TaxCounter record)
        {
            if (record == null)
                throw new ArgumentNullException("record");

            _Zip2TaxCounter.Update(record);
        }

        public void DeleteZip2TaxCounter(Td_Zip2TaxCounter record)
        {
            if (record == null)
                throw new ArgumentNullException("record");
            _Zip2TaxCounter.Delete(record);
        }

        public IPagedList<Td_Zip2TaxCounter> GetAllZip2TaxCounter(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = from a in _Zip2TaxCounter.Table
                        orderby a.Id
                        select a;
            var records = new PagedList<Td_Zip2TaxCounter>(query, pageIndex, pageSize);
            return records;
        }

        //public virtual IList<Td_Zip2TaxCounter> GetAllZip2TaxCounterlist()
        //{
        //    var query = from sliders in _Zip2TaxCounter.Table     select sliders;

        //    return query.ToList();
        //}


        public Td_Zip2Tax GetZip2TaxById(int id)
        {
            if (id == 0)
                return null;
            return _Zip2Tax.GetById(id);
        }

        public void InsertZip2Tax(Td_Zip2Tax record)
        {
            if (record == null)
                throw new ArgumentNullException("record");

            _Zip2Tax.Insert(record);
        }

        // Get County by Zip Code
        public string GetCounty(string zipCode)
        {
            if (zipCode.Length < 1)
                throw new ArgumentNullException("Enter Zipcode");

            var query = from a in _Zip2Tax.Table where a.Zipcode == zipCode select a.County;
            return query.SingleOrDefault();
        }

        public void UpdateZip2Tax(Td_Zip2Tax record)
        {
            if (record == null)
                throw new ArgumentNullException("record");

            _Zip2Tax.Update(record);
        }

        public void DeleteZip2Tax(Td_Zip2Tax record)
        {
            if (record == null)
                throw new ArgumentNullException("record");
            _Zip2Tax.Delete(record);
        }

        public IPagedList<Td_Zip2Tax> GetAllZip2Tax(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = from a in _Zip2Tax.Table
                        orderby a.Id
                        select a;
            var records = new PagedList<Td_Zip2Tax>(query, pageIndex, pageSize);
            return records;
        }

        public Td_Zip2Tax GetAvailableTaxDetailRowByZipcode(string Zipcode)
        {
            if (Zipcode.Length < 1)
                throw new ArgumentNullException("Enter Zipcode");

            var query = from a in _Zip2Tax.Table where a.Zipcode == Zipcode select a;
            return query.SingleOrDefault();
        }


        //------Calculate Taxrate From Zipcode--------------------------------------------------------------------------

        public decimal GetAvailableTaxDetailByZipcode(string Zipcode, string stateCode = null, bool isFromConfigure = false)
        {
            decimal taxr = decimal.Zero;
            var validZipcode = from a in _Zip2TaxCounter.Table where a.Zipcode == Zipcode && (!a.IsValideZipcode) select a;
            var validZipcodeResult = validZipcode.FirstOrDefault();

            var ZipcodeList = from a in _Zip2Tax.Table
                              where a.Zipcode == Zipcode
                              select a;

            var ZipcodeResult = ZipcodeList.FirstOrDefault();
            if (validZipcodeResult != null && ZipcodeResult == null)
            {
                return taxr;
            }

            var expirredate = DateTime.UtcNow.AddHours((_zipsetting.UpdateTimeStemp * -1));
            //var query = from a in _Zip2Tax.Table where a.Zipcode == Zipcode && a.ModifyOn > expirredate select a;
            var query = from a in _Zip2Tax.Table where a.Zipcode == Zipcode && a.ModifyOn > DateTime.UtcNow select a;
            var result1 = query.SingleOrDefault();
            if (result1 != null)
            {
                decimal.TryParse(result1.TaxRate, out taxr);
            }
            else
            {
                if (!string.IsNullOrEmpty(stateCode))
                {
                    var allowedStateIds = _zipsetting.AllowedStateIds;
                    if (!string.IsNullOrWhiteSpace(allowedStateIds))
                    {
                        string[] stateIds = allowedStateIds.Split(',').Select(x=>x.ToLower()).ToArray();
                        if (stateIds.Contains(stateCode))
                            taxr = GetLatestTaxRate(Zipcode);
                    }
                }

                if (isFromConfigure)
                {
                    taxr = GetLatestTaxRate(Zipcode);
                }
            }
            return taxr;
        }

        //-------------------------Get Online Taxrate--------------------------------------------------------
        public decimal GetLatestTaxRate(string zipCode)
        {
            decimal taxRate = 0;
            string username = _zipsetting.UserName;
            string password = _zipsetting.Password;
            //string cmdText = "exec z2t_lookup '" + zipCode + "', '" + username + "', '" + password + "'";
            string url = _zipsetting.Zip2TaxApiUrl + "?username=" + username + "&password=" + password + "&zip=" + zipCode;
            //string url = "";
            string strxml;
            if (!String.IsNullOrEmpty(_zipsetting.UserName) && !String.IsNullOrEmpty(_zipsetting.Password) && !String.IsNullOrEmpty(_zipsetting.Zip2TaxApiUrl))
            {
                try
                {
                    _logger.Information("Zip2tax Api Called");
                    using (WebClient client = new WebClient())
                    {
                        //Download the xml document as a string
                        client.Headers.Add(HttpRequestHeader.Accept, "application/xml");
                        strxml = client.DownloadString(url);
                    }
                    
                    XDocument doc = XDocument.Parse(strxml);
                    XElement generalElement = doc.Element("z2tLookup").Element("errorInfo");

                    String errorInfo = generalElement.Element("errorMessage").Value;
                    _logger.Information("Zip2tax Error info" + errorInfo);
                    if (errorInfo.Equals("Success"))
                    {

                        XElement generalInnerElement =
                            doc.Element("z2tLookup").Element("addressInfo").Element("addresses").Element("address");

                        XElement generalInnerTaxElement =
                            generalInnerElement.Element("salesTax");

                        string tax = generalInnerTaxElement.Element("taxRate").Value;
                        string county = generalInnerElement.Element("county").Value;
                        decimal.TryParse(tax, out taxRate);


                        //insert Counter Data in Table
                        Td_Zip2TaxCounter counterinsert = new Td_Zip2TaxCounter();
                        counterinsert.CallDate = DateTime.UtcNow;
                        counterinsert.Zipcode = zipCode;
                        counterinsert.TaxRate = taxRate.ToString();
                        InsertZip2TaxCounter(counterinsert);

                        //Update Existing TaxInformation Data in Table
                        var Ziptablerecod = GetAvailableTaxDetailRowByZipcode(zipCode);
                        if (Ziptablerecod == null)
                        {
                            Td_Zip2Tax taxmodel = new Td_Zip2Tax();
                            taxmodel.CreateOn = DateTime.UtcNow;
                            taxmodel.Zipcode = zipCode;
                            taxmodel.TaxRate = taxRate.ToString();
                            taxmodel.County = county;
                            taxmodel.Taxcategoryid = 0;
                            //taxmodel.ModifyOn = DateTime.UtcNow;
                            taxmodel.ModifyOn = DateTime.UtcNow.AddDays(-DateTime.UtcNow.Day + 1).AddMonths(1);
                            InsertZip2Tax(taxmodel);
                        }
                        else
                        {
                            var zip2TaxById = GetZip2TaxById(Ziptablerecod.Id);
                            //zip2TaxById.ModifyOn = DateTime.UtcNow;
                            zip2TaxById.ModifyOn = DateTime.UtcNow.AddDays(-DateTime.UtcNow.Day + 1).AddMonths(1);
                            zip2TaxById.TaxRate = taxRate.ToString();
                            if (string.IsNullOrEmpty(zip2TaxById.County))
                                zip2TaxById.County = county;
                            UpdateZip2Tax(zip2TaxById);
                        }


                        return taxRate;
                    }
                    else
                    {
                        //insert Counter Data in Table
                        Td_Zip2TaxCounter counterinsert = new Td_Zip2TaxCounter();
                        counterinsert.CallDate = DateTime.UtcNow;
                        counterinsert.Zipcode = zipCode;
                        counterinsert.TaxRate = decimal.Zero.ToString();
                        counterinsert.IsValideZipcode = false;
                        InsertZip2TaxCounter(counterinsert);
                        // decimal.TryParse(_zipsetting.DefaultTaxRate, out taxRate);
                        return _zipsetting.DefaultTaxRate;
                    }
                }

                catch (Exception em)
                {
                    Td_Zip2TaxCounter counterinsert = new Td_Zip2TaxCounter();
                    counterinsert.CallDate = DateTime.UtcNow;
                    counterinsert.Zipcode = zipCode;
                    counterinsert.TaxRate = "0";
                    InsertZip2TaxCounter(counterinsert);
                    //decimal.TryParse(_zipsetting.DefaultTaxRate, out taxRate);
                    _logger.Error("Zip2Tax - "+ em.Message, em);
                    return _zipsetting.DefaultTaxRate;
                }
            }
            else
            {
                return _zipsetting.DefaultTaxRate;
            }
        }

        #region Get county using zip2tax if its null in table

        public virtual string GetCountyNameUsingZip2Tax(string zipCode)
        {
            if (string.IsNullOrEmpty(zipCode))
            {
                _logger.Error("ZipCode is null or empty while calling GetCountyNameUsingZip2Tax() method");
                throw new ArgumentNullException(zipCode);
            }

            decimal taxRate = 0;
            string username = _zipsetting.UserName;
            string password = _zipsetting.Password;
            //string cmdText = "exec z2t_lookup '" + zipCode + "', '" + username + "', '" + password + "'";
            string url = _zipsetting.Zip2TaxApiUrl + "?username=" + username + "&password=" + password + "&zip=" + zipCode;
            //string url = "";
            string strxml;
            if (!String.IsNullOrEmpty(_zipsetting.UserName) && !String.IsNullOrEmpty(_zipsetting.Password) && !String.IsNullOrEmpty(_zipsetting.Zip2TaxApiUrl))
            {
                try
                {
                    using (WebClient client = new WebClient())
                    {
                        //Download the xml document as a string
                        client.Headers.Add(HttpRequestHeader.Accept, "application/xml");
                        strxml = client.DownloadString(url);
                    }
                    XDocument doc = XDocument.Parse(strxml);
                    XElement generalElement = doc.Element("z2tLookup").Element("errorInfo");

                    String errorInfo = generalElement.Element("errorMessage").Value;

                    if (errorInfo.Equals("Success"))
                    {

                        XElement generalInnerElement =
                            doc.Element("z2tLookup").Element("addressInfo").Element("addresses").Element("address");

                        XElement generalInnerTaxElement =
                            generalInnerElement.Element("salesTax").Element("rateInfo");

                        string tax = generalInnerTaxElement.Element("taxRate").Value;
                        string county = generalInnerElement.Element("county").Value;
                        decimal.TryParse(tax, out taxRate);


                        //insert Counter Data in Table
                        Td_Zip2TaxCounter counterinsert = new Td_Zip2TaxCounter();
                        counterinsert.CallDate = DateTime.UtcNow;
                        counterinsert.Zipcode = zipCode;
                        counterinsert.TaxRate = taxRate.ToString();
                        InsertZip2TaxCounter(counterinsert);

                        //Update Existing TaxInformation Data in Table
                        var Ziptablerecod = GetAvailableTaxDetailRowByZipcode(zipCode);
                        if (Ziptablerecod == null)
                        {
                            Td_Zip2Tax taxmodel = new Td_Zip2Tax();
                            taxmodel.CreateOn = DateTime.UtcNow;
                            taxmodel.Zipcode = zipCode;
                            taxmodel.TaxRate = taxRate.ToString();
                            taxmodel.County = county;
                            taxmodel.Taxcategoryid = 0;
                            taxmodel.ModifyOn = DateTime.UtcNow;
                            InsertZip2Tax(taxmodel);
                        }
                        else
                        {
                            var zip2TaxById = GetZip2TaxById(Ziptablerecod.Id);
                            zip2TaxById.ModifyOn = DateTime.UtcNow;
                            zip2TaxById.TaxRate = taxRate.ToString();
                            if (string.IsNullOrEmpty(zip2TaxById.County))
                                zip2TaxById.County = county;
                            UpdateZip2Tax(zip2TaxById);
                        }


                        return county;
                    }
                    else
                    {
                        var errorMessage = !string.IsNullOrEmpty(errorInfo) ? errorInfo : string.Empty;
                        _logger.Error("Error occurred while getting latest tax from Zip2Tax in GetCountyNameUsingZip2Tax() method. Error message : " + errorMessage);
                        return string.Empty;
                    }
                }

                catch (Exception ex)
                {
                    _logger.Error("Error occurred while getting latest tax from Zip2Tax in GetCountyNameUsingZip2Tax() method. Error message : " + ex.Message, ex);
                    return string.Empty; ;
                }
            }
            else
            {
                _logger.Error("Username, Password or in Zip2TaxApiUrl is null while getting latest tax from Zip2tax in GetCountyNameUsingZip2Tax() method.");
                return string.Empty;
            }
        }

        #endregion
    }
}
