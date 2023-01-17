using System;
using System.Collections.Generic;
using Nop.Core.Domain.Orders;
using Nop.Plugin.TorchDesigns.DynamicsGPWCF.DynamicsGP;
using Nop.Services.Common;
using Nop.Services.Tax;
using Nop.Services.Logging;
using Nop.Plugin.TorchDesign.Zip2Tax;
using Nop.Core.Infrastructure;

namespace Nop.Plugin.TorchDesigns.DynamicsGPWCF
{
    public partial class GPAddresses
    {
        private readonly IAddressService _addressservice;
        private readonly ITaxService _taxService;
        private readonly ILogger _logger;
        GreatPlains _greatPlains;
        private readonly TorchDesignsDynamicsGPWCFSettings _torchDesignsDynamicsGPSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="addressservice">nopCommerce Address Service</param>
        /// <param name="taxService">nopCommerce Tax Service</param>
        /// <param name="greatPlains">Great Plains Interface</param>
        /// <param name="logger">nopCommerce Logger Service</param>
        public GPAddresses(IAddressService addressservice, ITaxService taxService, GreatPlains greatPlains, ILogger logger)
        {
            this._addressservice = addressservice;
            this._taxService = taxService;
            this._greatPlains = greatPlains;
            this._logger = logger;
            this._torchDesignsDynamicsGPSettings = EngineContext.Current.Resolve<TorchDesignsDynamicsGPWCFSettings>();
        }

        /// <summary>
        /// Adds the missing shipping and/or billing addresses from a nopCommerce Order to the provided customer
        /// </summary>
        /// <param name="gpCustomer">Great Plains Customer</param>
        /// <param name="order">nopCommerce Order</param>
        public void AddMissingAddresses(DynamicsGP.Customer gpCustomer, Order order)
        {
            //_logger.Information("Add Missing Addresses");
            // If it is missing, add the billing address
            if (GetMatchingAddressKeyForOrder(gpCustomer, order.BillingAddress, "BILL TO", order) == null)
            {
                AddAddressToCustomer(gpCustomer, order.BillingAddress, "BILL TO", order);
            }

            // If it is missing, add the shipping address
            if (GetMatchingAddressKeyForOrder(gpCustomer, order.ShippingAddress, "SHIP TO", order) == null)
            {
                AddAddressToCustomer(gpCustomer, order.ShippingAddress, "SHIP TO", order);
            }
        }

        /// <summary>
        /// Adds the specified address to the provided Great Plains Customer
        /// </summary>
        /// <param name="gpCustomer">Great Plains Customer</param>
        /// <param name="nopAddress">nopCommerce Address</param>
        /// <param name="strPrefix">Great Plains Address Key Prefix</param>
        private void AddAddressToCustomer(DynamicsGP.Customer gpCustomer, Core.Domain.Common.Address nopAddress, string strPrefix, Order nopOrder)
        {
            //_logger.Information("Add Addresse to customer " + strPrefix);

            // Get Last Address Key
            string strLastAddressKey = GetLastAddressKey(gpCustomer, strPrefix);
            int iLastAddressNumber = 0;
            string newAddressKey = "";

            // Increment Address Key by 1
            if (strLastAddressKey == "")
            {
                newAddressKey = strPrefix;
            }
            else if (strLastAddressKey == strPrefix)
            {
                newAddressKey = strPrefix + " 1";
            }
            else
            {
                iLastAddressNumber = Convert.ToInt32(strLastAddressKey.Replace(strPrefix, "").Trim()) + 1;
                newAddressKey = strPrefix + " " + iLastAddressNumber.ToString().Trim();
            }

            // Create CustomerAddressKey object
            CustomerAddressKey gpAddressKey = new CustomerAddressKey();
            gpAddressKey.Id = newAddressKey;
            gpAddressKey.CustomerKey = gpCustomer.Key;

            GPAddress gpAddress = new GPAddress(nopAddress, gpAddressKey, _taxService, nopOrder);

            // Add the Address to the Array
            List<CustomerAddress> gpCustomerAddresses = new List<CustomerAddress>();
            if (gpCustomer.Addresses != null)
            {
                foreach (CustomerAddress address in gpCustomer.Addresses)
                {
                    gpCustomerAddresses.Add(address);
                }
            }
            gpCustomerAddresses.Add(gpAddress.GreatPlainsAddress);

            gpCustomer.Addresses = gpCustomerAddresses.ToArray();
        }

        /// <summary>
        /// Return the last key name on the GPCustomer that is currently using this prefix.
        /// </summary>
        /// <param name="strPrefix">Prefix for the key like SHIP TO or BILL TO</param>
        /// <returns></returns>
        private string GetLastAddressKey(DynamicsGP.Customer customer, string strPrefix)
        {
            string strLastAddressKey = "";
            int iMaxNumber = 0;

            if (customer.Addresses != null)
            {
                foreach (CustomerAddress address in customer.Addresses)
                {
                    if (address.Key.Id.StartsWith(strPrefix) == true)
                    {
                        if (address.Key.Id == strPrefix)
                        {
                            if (iMaxNumber == 0)
                            {
                                strLastAddressKey = strPrefix;
                            }
                        }
                        else
                        {
                            int iTestNumber = Convert.ToInt32(address.Key.Id.Replace(strPrefix, "").Trim());
                            if (iTestNumber > iMaxNumber)
                            {
                                iMaxNumber = iTestNumber;
                                strLastAddressKey = address.Key.Id;
                            }
                        }
                    }
                }
            }

            return strLastAddressKey;
        }

        public CustomerAddressKey GetMatchingAddressKeyForOrder(DynamicsGP.Customer gpCustomer, Nop.Core.Domain.Common.Address nopAddress, string strPrefix, Order nopOrder)
        {
            CustomerAddressKey gpCustomerAddressKey = null;

            //_logger.Information("Get Matching Address = " + nopAddress.Address1 + nopAddress.ZipPostalCode);
            if (gpCustomer.Addresses != null)
            {
                _logger.Information("gpCustomer.Addresses count : " + gpCustomer.Addresses.Length);


                    foreach (DynamicsGP.CustomerAddress gpAddress in gpCustomer.Addresses)
                    {
                    
                    bool addressMatch = true;
                    //_logger.Information("address company" + nopAddress.Company +"=" + gpAddress.Name + "address firstname" + nopAddress.FirstName + " " + nopAddress.LastName + "=" + gpAddress.Name
                    //    + "address address1" + nopAddress.Address1 + "=" + gpAddress.Line1 + "address address2" + nopAddress.Address2 + "=" + gpAddress.Line2
                    //    + "address city" + nopAddress.City + "=" + gpAddress.City + "address Sate" + nopAddress.StateProvince.Abbreviation + "=" + gpAddress.State +
                    //    "address Zip" + nopAddress.ZipPostalCode + "=" + gpAddress.PostalCode + "address key" + strPrefix + "=" + gpAddress.Key.Id);
                   
                    //if (!StringCompare(nopAddress.Company, gpAddress.Name))
                    //    {
                    //        addressMatch = false;
                    //    }
                    //    else 
                        if (!StringCompare((nopAddress.FirstName.ToUpper().Trim() + " " + nopAddress.LastName.ToUpper().Trim()), gpAddress.ContactPerson))
                        {
                            addressMatch = false;
                        }
                        else if (!StringCompare(nopAddress.Address1, gpAddress.Line1))
                        {
                            addressMatch = false;
                        }
                        else if (!StringCompare(nopAddress.Address2, gpAddress.Line2))
                        {
                            addressMatch = false;
                        }
                        else if (!StringCompare(nopAddress.City, gpAddress.City))
                        {
                            addressMatch = false;
                        }
                        else if (nopAddress.StateProvince != null && !string.IsNullOrEmpty(nopAddress.StateProvince.Abbreviation) && (!StringCompare(nopAddress.StateProvince.Abbreviation, gpAddress.State)))
                        {
                            addressMatch = false;
                        }
                        else if (!StringCompare(nopAddress.ZipPostalCode, gpAddress.PostalCode))
                        {
                            addressMatch = false;
                        }
                        else if (!string.IsNullOrEmpty(strPrefix))
                        {
                        if (!gpAddress.Key.Id.ToUpper().Trim().StartsWith(strPrefix.ToUpper().Trim()))
                        {
                                addressMatch = false;
                            }
                        }

                        if (addressMatch)
                        {
                            //_logger.Information("NopAddresses Match with GPAddress");
                            gpCustomerAddressKey = gpAddress.Key;
                            //If there is sales tax, it should use county name to determine tax schedule id
                            gpAddress.TaxScheduleKey = new TaxScheduleKey();
                            AddTaxScheduleKeyIfTaxIsApplicable(gpAddress, nopOrder, nopAddress);
                            break;
                        }
                }
            }
            //_logger.Information("Matching Address gpCustomerAddressKey = ");
            return gpCustomerAddressKey;
        }


        /// <summary>
        /// Returns the Great Plains Address Key that matches the provided nopCommerce Address for a particular customer
        /// </summary>
        /// <param name="gpCustomer">Great Plains Customer</param>
        /// <param name="nopAddress">nopCommerce Address</param>
        /// <param name="strPrefix">Address Key Prefix</param>
        /// <returns></returns>
        public CustomerAddressKey GetMatchingAddressKey(DynamicsGP.Customer gpCustomer, Nop.Core.Domain.Common.Address nopAddress, string strPrefix, Order nopOrder)
        {
            //_logger.Information("Get Matching Address = " + gpCustomer.Name);
            CustomerAddressKey gpCustomerAddressKey = null;
            if (gpCustomer.Addresses != null)
            {
                foreach (DynamicsGP.CustomerAddress gpAddress in gpCustomer.Addresses)
                {
                    bool bMatch = true;
                    // Company
                    if (StringCompare(nopAddress.Company, gpAddress.Name) == false)
                    {
                        bMatch = false;
                    }
                    else if (StringCompare((nopAddress.FirstName.ToUpper().Trim() + " " + nopAddress.LastName.ToUpper().Trim()), gpAddress.ContactPerson) == false)
                    {
                        bMatch = false;
                    }
                    else if (StringCompare(nopAddress.Address1, gpAddress.Line1) == false)
                    {
                        bMatch = false;
                    }
                    else if (StringCompare(nopAddress.Address2, gpAddress.Line2) == false)
                    {
                        bMatch = false;
                    }
                    else if (StringCompare(nopAddress.City, gpAddress.City) == false)
                    {
                        bMatch = false;
                    }
                    else if (nopAddress.StateProvince != null && !string.IsNullOrWhiteSpace(nopAddress.StateProvince.Abbreviation) && (StringCompare(nopAddress.StateProvince.Abbreviation, gpAddress.State) == false))
                    {
                        bMatch = false;
                    }
                    else if (StringCompare(nopAddress.ZipPostalCode, gpAddress.PostalCode) == false)
                    {
                        bMatch = false;
                    }

                    // Check the prefix if it is provided
                    else if (String.IsNullOrWhiteSpace(strPrefix) == false)
                    {
                        if (gpAddress.Key.Id.ToUpper().Trim().StartsWith(strPrefix.ToUpper().Trim()) == false)
                        {
                            bMatch = false;
                        }
                    }

                    if (bMatch == true)
                    {
                        gpCustomerAddressKey = gpAddress.Key;
                        //If there is sales tax, it should use county name to determine tax schedule id
                        gpAddress.TaxScheduleKey = new TaxScheduleKey();
                        AddTaxScheduleKeyIfTaxIsApplicable(gpAddress, nopOrder, nopAddress);
                        break;
                    }
                }
            }

            return gpCustomerAddressKey;
        }

        /// <summary>
        /// Determines if the two provided strings can be considered a match.  This also handles nulls and blank values.
        /// </summary>
        /// <param name="string1">First String to Compare</param>
        /// <param name="string2">Second String to Compare</param>
        /// <returns>bool: True if the strings are considered a match</returns>
        private bool StringCompare(string string1, string string2)
        {
            bool bMatch = true;
            if ((String.IsNullOrEmpty(string1) == false) && (String.IsNullOrEmpty(string2) == false))
            {
                if (string1.ToUpper().Trim() != string2.ToUpper().Trim())
                {
                    bMatch = false;
                }
            }
            else if ((String.IsNullOrEmpty(string1) == true) && (String.IsNullOrEmpty(string2) == true))
            {
                bMatch = true;
            }
            else if ((String.IsNullOrEmpty(string1) == true) || (String.IsNullOrEmpty(string2) == true))
            {
                bMatch = false;
            }

            return bMatch;
        }

        protected void AddTaxScheduleKeyIfTaxIsApplicable(DynamicsGP.CustomerAddress gpAddress, Order nopOrder, Nop.Core.Domain.Common.Address nopAddress)
        {
            if (nopOrder.OrderTax > decimal.Zero)
            {
                TorchDesignZip2TaxPlugin zip2TaxProvider = (TorchDesignZip2TaxPlugin)_taxService.LoadActiveTaxProvider();
                //if (nopAddress.StateProvince != null && nopAddress.StateProvince.Name.Trim().ToLower().Equals("florida"))
                //{
                    string countyAsStrTaxScheduleId = zip2TaxProvider.GetCounty(nopAddress.ZipPostalCode.ToString().Trim());
                    if (countyAsStrTaxScheduleId == null || countyAsStrTaxScheduleId == "")
                    {
                        var county = zip2TaxProvider.GetCountyNameUsingZip2TaxIfNull(nopAddress.ZipPostalCode.ToString().Trim());
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
        }

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
                    if (i == 0)
                    {
                        string splittedString = splittedCounty[i];
                        countyAsTaxScheduleId = splittedString[0].ToString() + splittedString[splittedString.Length - 1].ToString();
                    }
                    else
                    {
                        countyAsTaxScheduleId = countyAsTaxScheduleId + " " + splittedCounty[i];
                    }
                }
            }
            return countyAsTaxScheduleId;
        }
    }
}
