using System;
using Nop.Plugin.TorchDesigns.DynamicsGPWCF.DynamicsGP;
using Nop.Services.Tax;
using Nop.Plugin.TorchDesign.Zip2Tax;
using Nop.Core.Infrastructure;
using Nop.Services.Logging;

namespace Nop.Plugin.TorchDesigns.DynamicsGPWCF
{
    class GPAddress
    {
        CustomerAddress _oAddress = null; // Great Plains Address Object
        CustomerAddressKey _oCustomerAddressKey = null;  // Customer Address Key
        readonly ITaxService _taxService;
        private readonly TorchDesignsDynamicsGPWCFSettings _torchDesignsDynamicsGPSettings;
        private readonly ILogger _logger;
        /// <summary>
        /// Converts a nopCommerce Address into a Great Plains Address
        /// </summary>
        /// <param name="oAddress">nopCommerce Address</param>
        /// <param name="oAddressKey">Great Plains Address Key</param>
        public GPAddress(Nop.Core.Domain.Common.Address oAddress,
            CustomerAddressKey oAddressKey,
            ITaxService taxService,
            Core.Domain.Orders.Order nopOrder)
        {
            // Init
            CustAddressKey = oAddressKey;
            _taxService = taxService;
            this._torchDesignsDynamicsGPSettings = EngineContext.Current.Resolve<TorchDesignsDynamicsGPWCFSettings>();
            this._logger = EngineContext.Current.Resolve<ILogger>();

            // Create Great Plains Address Object
            createAddress(oAddress, nopOrder);


        }

        #region Utilities

        protected string ConvertCountyToTaxScheduleId(string county)
        {
            string countyAsTaxScheduleId = string.Empty;
            var splittedCounty = county.ToUpper().Split(' ');
            if (splittedCounty.Length == 1)
            {
                countyAsTaxScheduleId = splittedCounty[0];
            }
            else
            {
                for (int i = 0; i < splittedCounty.Length; i++)
                {
                    // As per Task we added this condition for convert county name which is "SAINT" to "ST" only. not for others county
                    if (i == 0 && splittedCounty[i] == "SAINT")
                    {
                        string splittedString = splittedCounty[i];
                        countyAsTaxScheduleId = splittedString[0].ToString() + splittedString[splittedString.Length - 1].ToString();
                    }
                    else
                    {
                        countyAsTaxScheduleId = i == 0 ? splittedCounty[i] : countyAsTaxScheduleId + " " + splittedCounty[i];
                    }
                }
            }
            return countyAsTaxScheduleId;
        }

        #endregion


        /// <summary>
        /// Creates a Great Plains Address based  a nopCommerce Address
        /// </summary>upon
        /// <param name="oAddress">nopCommerce Address</param>
        private void createAddress(Nop.Core.Domain.Common.Address oAddress, Core.Domain.Orders.Order nopOrder)
        {
            //_logger.Information("Create Address OAddress = " + oAddress.Address1 + oAddress.ZipPostalCode);
            // Init
            CustomerAddress gpAddress = new CustomerAddress();

            // Address Key
            gpAddress.Key = CustAddressKey;

            // Contact Name
            gpAddress.ContactPerson = GPName.CleanName(oAddress.FirstName.Trim() + " " + oAddress.LastName).ToUpper();

            // Address
            gpAddress.Line1 = (oAddress != null && !string.IsNullOrEmpty(oAddress.Address1)) ? oAddress.Address1.ToUpper() : string.Empty;
            gpAddress.Line2 = (oAddress != null && !string.IsNullOrEmpty(oAddress.Address2)) ? oAddress.Address2.ToUpper() : string.Empty;
            if (!String.IsNullOrWhiteSpace(oAddress.City))
            {
                gpAddress.City = oAddress.City.ToUpper();
            }
            if (oAddress.StateProvince != null)
            {
                if (!String.IsNullOrWhiteSpace(oAddress.StateProvince.Abbreviation))
                {
                    gpAddress.State = oAddress.StateProvince.Abbreviation.ToUpper();
                }
            }
            if (!String.IsNullOrWhiteSpace(oAddress.ZipPostalCode))
            {
                gpAddress.PostalCode = oAddress.ZipPostalCode.ToUpper();
            }
            gpAddress.CountryRegion = (oAddress != null && oAddress.Country != null && !string.IsNullOrEmpty(oAddress.Country.ThreeLetterIsoCode)) ? oAddress.Country.ThreeLetterIsoCode.ToUpper() : string.Empty;

            // Added condition as per given task - When publishing to GP, the addresses are coming across with a country "USA" , but the Country Code "US" is not coming across 
            if (oAddress.Country != null && !string.IsNullOrEmpty(oAddress.Country.ThreeLetterIsoCode) && oAddress.Country.ThreeLetterIsoCode.ToLower().Equals("usa"))
            {
                var countryRegionCodeKey = !string.IsNullOrEmpty(oAddress.Country.TwoLetterIsoCode) ? oAddress.Country.TwoLetterIsoCode : string.Empty;
                gpAddress.CountryRegionCodeKey = new CountryRegionCodeKey();
                gpAddress.CountryRegionCodeKey.Id = countryRegionCodeKey;
            }
            // Phone Number
            PhoneNumber gpPhone = new PhoneNumber();
            gpPhone.Value = GPPhoneNumber.Convert(oAddress.PhoneNumber);
            gpAddress.Phone1 = gpPhone;

            // Fax Number
            if (oAddress.FaxNumber != null)
            {
                if (String.IsNullOrWhiteSpace(oAddress.FaxNumber.ToString()) == false)
                {
                    PhoneNumber gpFax = new PhoneNumber();
                    gpFax.Value = GPPhoneNumber.Convert(oAddress.FaxNumber);
                    gpAddress.Fax = gpFax;
                }
            }
            // Sales Person ID
            gpAddress.SalespersonKey = new SalespersonKey();
            gpAddress.SalespersonKey.Id = _torchDesignsDynamicsGPSettings.SalespersonKeyId; // TODO: Add to Configuration

            // Territory ID
            gpAddress.SalesTerritoryKey = new SalesTerritoryKey();
            gpAddress.SalesTerritoryKey.Id = _torchDesignsDynamicsGPSettings.SalesTerritoryKeyId; // TODO: Add to Configuration

            // Tax Schedule
            TorchDesignZip2TaxPlugin zip2TaxProvider = (TorchDesignZip2TaxPlugin)_taxService.LoadActiveTaxProvider();
            gpAddress.TaxScheduleKey = new TaxScheduleKey();
            CalculateTaxRequest oTaxRequest = new CalculateTaxRequest();
            oTaxRequest.Address = oAddress;

            //If there is sales tax, it should use county name to determine tax schedule id
            if (nopOrder.OrderTax > decimal.Zero)
            {
                //For customers in Florida, the TaxSchedule Id should be the name of the county. Example:  Saint Lucie would become ST LUCIE
                //if (oAddress.StateProvince != null && oAddress.StateProvince.Name.Trim().ToLower().Equals("florida"))
                //{
                    string countyAsStrTaxScheduleId = zip2TaxProvider.GetCounty(oAddress.ZipPostalCode.ToString().Trim());
                    if (countyAsStrTaxScheduleId == null || countyAsStrTaxScheduleId == "")
                    {
                        var county = zip2TaxProvider.GetCountyNameUsingZip2TaxIfNull(oAddress.ZipPostalCode.ToString().Trim());
                        if (!string.IsNullOrEmpty(county))
                        {
                            county = county.ToUpper();
                            countyAsStrTaxScheduleId = ConvertCountyToTaxScheduleId(county);
                        }
                        else
                        {
                            countyAsStrTaxScheduleId = _torchDesignsDynamicsGPSettings.TaxScheduleId; // TODO: Add to Configuration
                        }
                    }
                    else
                    {
                        //string taxScheduleId = string.Empty;
                        string county = countyAsStrTaxScheduleId.ToUpper();
                        countyAsStrTaxScheduleId = ConvertCountyToTaxScheduleId(county);
                    }
                    gpAddress.TaxScheduleKey.Id = countyAsStrTaxScheduleId;

                //}
                //else
                //{
                //    //Default Tax Schedule Id for customers outside of florida
                //    string strTaxScheduleId = _torchDesignsDynamicsGPSettings.TaxScheduleIdForCustomersOutsideFlorida;
                //    if (string.IsNullOrEmpty(strTaxScheduleId))
                //    {
                //        strTaxScheduleId = _torchDesignsDynamicsGPSettings.TaxScheduleId; // TODO: Add to Configuration
                //    }
                //    gpAddress.TaxScheduleKey.Id = strTaxScheduleId;
                //}
            }
            else
            {
                // if there is no sales tax, then it should use the default key
                gpAddress.TaxScheduleKey.Id = _torchDesignsDynamicsGPSettings.TaxScheduleId;
            }
           
            GreatPlainsAddress = gpAddress;
        }

        #region Properties

        public CustomerAddressKey CustAddressKey
        {
            get
            {
                return _oCustomerAddressKey;
            }
            set
            {
                _oCustomerAddressKey = value;
            }
        }

        public CustomerAddress GreatPlainsAddress
        {
            get
            {
                return _oAddress;
            }
            set
            {
                _oAddress = value;
            }
        }

        #endregion

    }
}
