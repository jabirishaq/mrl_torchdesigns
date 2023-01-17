using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core.Domain.Orders;
using Nop.Plugin.TorchDesigns.DynamicsGPWCF.DynamicsGP;
using Nop.Core.Infrastructure;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Logging;
using Nop.Core.Domain.Common;
using Nop.Services.Tax;

namespace Nop.Plugin.TorchDesigns.DynamicsGPWCF
{
    public class GPCustomer
    {
        GreatPlains _greatPlains = null;
        CustomerKey _customerKey = null;
        DynamicsGP.Customer _gpCustomer = null;
        string _customerName = "";
        readonly ILogger _logger;

        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ICustomerService _customerService;
        private readonly IAddressService _addressService;
        private readonly ITaxService _taxService;
        private readonly TorchDesignsDynamicsGPWCFSettings _torchDesignsDynamicsGPSettings;

        /// <summary>
        /// Constructor:  Finds or creates a Great Plains GPCustomer based upon the provided nopCommerce order
        /// </summary>
        /// <param name="greatPlains">Great Plains Service</param>
        /// <param name="order">NopCommerce Order</param>
        /// <param name="genericAttributeService">Generic Attribute Service</param>
        /// <param name="customerService">Customer Service</param>
        /// <param name="logger">Logger Service</param>
        /// <param name="addressService">Address Service</param>
        /// <param name="taxService">Tax Service</param>
        public GPCustomer(GreatPlains greatPlains, Order order, IGenericAttributeService genericAttributeService, ICustomerService customerService, ILogger logger, IAddressService addressService,
            ITaxService taxService)
        {
            this._genericAttributeService = genericAttributeService;
            this._customerService = customerService;
            this._addressService = addressService;
            this._taxService = taxService;
            this._logger = logger;
            this._torchDesignsDynamicsGPSettings = EngineContext.Current.Resolve<TorchDesignsDynamicsGPWCFSettings>();

            _greatPlains = greatPlains;
            findOrCreateCustomer(order);

        }

        /// <summary>
        /// Create a new GPCustomer based upon the billing details of an order
        /// </summary>
        /// <param name="order">NopCommerce Order</param>
        /// <returns>Great Plains CustomerKey for the newly created GPCustomer</returns>
        private DynamicsGP.CustomerKey createCustomer(Order order)
        {
            CustomerKey custKey = null;
            Nop.Plugin.TorchDesigns.DynamicsGPWCF.DynamicsGP.Customer gpCustomer = null;

            gpCustomer = convertCustomer(order);

            try
            {
                // Create the Customer
                Policy gpCreateCustomerPolicy = _greatPlains.GetService().GetPolicyByOperation("CreateCustomer", _greatPlains.GpContext);
                _greatPlains.GetService().CreateCustomer(gpCustomer, _greatPlains.GpContext, gpCreateCustomerPolicy);

                if (gpCustomer != null)
                {
                    custKey = gpCustomer.Key;
                    _gpCustomer = gpCustomer;
                }
            }
            catch (Exception ex)
            {
                _logger.InsertLog(Core.Domain.Logging.LogLevel.Error, ex.Message, ex.InnerException.ToString(), null);
            }

            return custKey;
        }

        /// <summary>
        /// Converts the billing details from an order into a new Great Plains GPCustomer object
        /// </summary>
        /// <param name="order">NopCommerce Order</param>
        /// <returns>Great Plains Customer</returns>
        private Nop.Plugin.TorchDesigns.DynamicsGPWCF.DynamicsGP.Customer convertCustomer(Order order)
        {
            DynamicsGP.Customer gpCustomer = null;
            Nop.Core.Domain.Customers.Customer nopCustomer = order.Customer;
            // Initialize
            gpCustomer = new Nop.Plugin.TorchDesigns.DynamicsGPWCF.DynamicsGP.Customer();
            string LastName = (order != null && order.BillingAddress != null && !string.IsNullOrEmpty(order.BillingAddress.LastName)) ? order.BillingAddress.LastName.ToUpper() : string.Empty;
            string FirstName = (order != null && order.BillingAddress != null && !string.IsNullOrEmpty(order.BillingAddress.FirstName)) ? order.BillingAddress.FirstName.ToUpper() : string.Empty;
            string Company = (order != null && order.BillingAddress != null && !string.IsNullOrEmpty(order.BillingAddress.Company)) ? order.BillingAddress.Company.ToUpper() : string.Empty;

            try
            {
                // Name
                if (String.IsNullOrWhiteSpace(Company) != true)
                {
                    if ((Company.Trim().ToUpper() != "NONE")
                        & (Company.Trim().ToUpper() != "NA")
                        & (Company.Trim().ToUpper() != "N/A"))
                    {
                        Name = Company;
                    }
                }

                if (String.IsNullOrWhiteSpace(Name) == true)
                {
                    Name = FirstName + " " + LastName;
                }

                gpCustomer.Name = GPName.CleanName(Name);

                // Customer Key
                CustomerKey gpCustomerKey = null;
                gpCustomerKey = new GPCustomerKey(_greatPlains, order).Key;
                gpCustomer.Key = gpCustomerKey;

                // Add the GPCustomer id from NopCommerce to the notes on the order in GP
                gpCustomer.Notes = "NOPCOMMERCE CUSTOMER ID: " + nopCustomer.Id.ToString().Trim();

                // Create Address List
                new GPAddresses(_addressService, _taxService, _greatPlains, _logger).AddMissingAddresses(gpCustomer, order);

                // Set Default Addresses
                gpCustomer.ShipToAddressKey = new CustomerAddressKey();
                gpCustomer.ShipToAddressKey.Id = "SHIP TO";
                gpCustomer.ShipToAddressKey.CustomerKey = gpCustomerKey;

                gpCustomer.BillToAddressKey = new CustomerAddressKey();
                gpCustomer.BillToAddressKey.Id = "BILL TO";
                gpCustomer.BillToAddressKey.CustomerKey = gpCustomerKey;

                gpCustomer.DefaultAddressKey = new CustomerAddressKey();
                gpCustomer.DefaultAddressKey.Id = "BILL TO";
                gpCustomer.DefaultAddressKey.CustomerKey = gpCustomerKey;

                // Payment Terms
                gpCustomer.PaymentTermsKey = new PaymentTermsKey();
                gpCustomer.PaymentTermsKey.Id = "DUE UPON RECEIPT";
                //TODO: Allow Customization of Payment Terms Key

                // Price Level
                gpCustomer.PriceLevelKey = new PriceLevelKey();
                gpCustomer.PriceLevelKey.Id = "HOME2";

                // Currency Key
                gpCustomer.CurrencyKey = new CurrencyKey();
                gpCustomer.CurrencyKey.ISOCode = _torchDesignsDynamicsGPSettings.CurrencyKeyISOCode;

                // Credit Limit
                gpCustomer.CreditLimit = new CustomerCreditLimit();
                gpCustomer.CreditLimit.Item = (CreditLimitSpecialAmount)CreditLimitSpecialAmount.NoCredit;

                // Maximum Writeoff
                gpCustomer.MaximumWriteoff = new MaximumWriteoff();
                gpCustomer.MaximumWriteoff.Item = (MaximumWriteoffSpecialAmount)MaximumWriteoffSpecialAmount.Unlimited;

                // Priority
                gpCustomer.Priority = 1;
            }
            catch (Exception ex)
            {
                gpCustomer = null;
                Exception e = ex;
            }

            return gpCustomer;
        }

        /// <summary>
        /// Finds or Creates a Great Plains Customer Object based upon the billing details from the provided NopCommerce order
        /// </summary>
        /// <param name="order">NopCommerce Order</param>
        private void findOrCreateCustomer(Order order)
        {
            // Try to find an existing GPCustomer reference in the database
            _customerKey = findCustomerWithReference(order.Customer);

            //Attempt Match on Name and Phone Number
            if (_customerKey == null)
            {
                _customerKey = GetCustomerMatchByPhone(order);
            }

            // Attempt to match by address 
            DynamicsGP.CustomerSummary[] gpCustomerSummary;

            // Find all name matches
            if (_customerKey == null)
            {
                try
                {
                    CustomerCriteria gpCustomerCriteria = new CustomerCriteria();
                    CompanyAddressCriteria gpCompanyAddressCriteria = new CompanyAddressCriteria();

                    LikeRestrictionOfString nameCriteria = new LikeRestrictionOfString();
                    nameCriteria.Like = order.BillingAddress.FirstName.Trim().ToUpper() + " " + order.BillingAddress.LastName.Trim().ToUpper();
                    gpCustomerCriteria.Name = nameCriteria;

                    gpCustomerSummary = _greatPlains.GetService().GetCustomerList(gpCustomerCriteria, _greatPlains.GpContext);

                }
                catch (Exception ex)
                {
                    _logger.InsertLog(Core.Domain.Logging.LogLevel.Information, ex.Message, ex.InnerException.ToString(), null);
                    gpCustomerSummary = null;
                }

                // Try to find an existing GPCustomer
                if (gpCustomerSummary != null)
                {
                    List<DynamicsGP.Customer> customerList = GetCustomerList(gpCustomerSummary);

                    //Attempt Match on Name and Address - Country
                    if (_customerKey == null)
                    {
                        _customerKey = GetCustomerMatchByAddress(customerList, order);
                    }

                }
            }

            // If the GPCustomer was not found, create the GPCustomer
            if (_customerKey == null)
            {
                _customerKey = createCustomer(order);
            }
            // If the GPCustomer was not just created, update the gpAddresses.
            else
            {
                updateCustomer(_customerKey, order);
            }

            // If the GPCustomer key was successfully identified, store it in the database for future use.
            if (_customerKey != null)
            {
                updateCustomerReference(order.Customer, _customerKey);
                _customerService.UpdateCustomer(order.Customer);
            }

        }

        /// <summary>
        /// Creates a Great Plains Customer List based upon an array of CustomerSummary objects
        /// </summary>
        /// <param name="customerSummary">Array of CustomerSummary objects</param>
        /// <returns>List of Great Plains Customers</returns>
        private List<DynamicsGP.Customer> GetCustomerList(CustomerSummary[] customerSummary)
        {
            List<DynamicsGP.Customer> customerList = new List<DynamicsGP.Customer>();

            // Iterate through the customers to find a matching phone number
            foreach (CustomerSummary summary in customerSummary)
            {
                customerList.Add(_greatPlains.GetService().GetCustomerByKey(summary.Key, _greatPlains.GpContext));
            }

            return customerList;
        }

        /// <summary>
        /// Get the GPCustomer key for a GPCustomer that has a matching address
        /// </summary>
        /// <param name="customerList">List of customers to compare</param>
        /// <param name="order">The order to compare to the list.  The billing address from the order will be used.</param>
        /// <returns></returns>
        private CustomerKey GetCustomerMatchByAddress(List<DynamicsGP.Customer> customerList, Order order)
        {
            CustomerKey customerKey = null;

            foreach (DynamicsGP.Customer customer in customerList)
            {
                CustomerAddressKey customerAddressKey = null;
                GPAddresses addresses = new GPAddresses(_addressService, _taxService, _greatPlains, _logger);

                customerAddressKey = addresses.GetMatchingAddressKey(customer, order.BillingAddress, "", order);
                if (customerAddressKey != null)
                {
                    customerKey = customer.Key;
                    break;
                }

            }

            return customerKey;
        }

        /// <summary>
        /// Get a GP Customer that has the same name and phone number as the provided Order's billing address
        /// </summary>
        /// <param name="order">NopCommerce Order</param>
        /// <returns></returns>
        private CustomerKey GetCustomerMatchByPhone(Order order)
        {
            CustomerKey gpCustKey = null;
            DynamicsGP.CustomerSummary[] gpCustomerSummary;
            string LastName = (order != null && order.BillingAddress != null && !string.IsNullOrEmpty(order.BillingAddress.LastName)) ? order.BillingAddress.LastName.Trim().ToUpper() : string.Empty;
            string FirstName = (order != null && order.BillingAddress != null && !string.IsNullOrEmpty(order.BillingAddress.FirstName)) ? order.BillingAddress.FirstName.Trim().ToUpper() : string.Empty;
            string PhoneNumber = (order != null && order.BillingAddress != null && !string.IsNullOrEmpty(order.BillingAddress.PhoneNumber)) ? order.BillingAddress.PhoneNumber.Trim().ToUpper() : string.Empty;
            // Try to find the GPCustomer matching Last Name and Phone Number
            try
            {
                CustomerCriteria gpCustomerCriteria = new CustomerCriteria();
                CompanyAddressCriteria gpCompanyAddressCriteria = new CompanyAddressCriteria();

                LikeRestrictionOfString nameCriteria = new LikeRestrictionOfString();
                nameCriteria.Like = order.BillingAddress.FirstName.Trim().ToUpper() + " " + order.BillingAddress.LastName.Trim().ToUpper();
                gpCustomerCriteria.Name = nameCriteria;

                LikeRestrictionOfString phoneCriteria = new LikeRestrictionOfString();
                phoneCriteria.Like = GPPhoneNumber.Convert(PhoneNumber);
                gpCustomerCriteria.PhoneNumber = phoneCriteria;

                gpCustomerSummary = _greatPlains.GetService().GetCustomerList(gpCustomerCriteria, _greatPlains.GpContext);

            }
            catch (Exception ex)
            {
                _logger.InsertLog(Core.Domain.Logging.LogLevel.Information, ex.Message, ex.InnerException.ToString(), null);
                gpCustomerSummary = null;
            }

            // If too few/too many matches, null the object
            if (gpCustomerSummary != null)
            {
                if (gpCustomerSummary.Count() != 1)
                {
                    gpCustomerSummary = null;
                }
                else
                {
                    gpCustKey = gpCustomerSummary[0].Key;
                }
            }

            return gpCustKey;
        }

        /// <summary>
        /// Finds a GPCustomer using a known cross reference
        /// </summary>
        /// <param name="oNopCustomer"></param>
        /// <returns></returns>
        private CustomerKey findCustomerWithReference(Nop.Core.Domain.Customers.Customer oNopCustomer)
        {
            // Init
            CustomerKey gpCustKey = null;

            IList<GenericAttribute> attributes = _genericAttributeService.GetAttributesForEntity(oNopCustomer.Id, "Customer");
            foreach (GenericAttribute attribute in attributes)
            {
                if (attribute.Key == "gp_customerkey")
                {
                    gpCustKey = new CustomerKey();
                    gpCustKey.Id = attribute.Value.Trim();
                }
            }

            return gpCustKey;
        }

        /// <summary>
        /// Update the customer to include the new addresses that were not previously found on this customer
        /// </summary>
        /// <param name="customerKey">Great Plains Customer Key for the customer to be updated</param>
        /// <param name="order">nopCommerce Order - The billing and shipping addresses from this order will be added to the customer if the addresses do not already exist</param>
        private void updateCustomer(CustomerKey customerKey, Order order)
        {
            DynamicsGP.Customer gpCustomer = null;

            try
            {
                //var customers = _greatPlains.GetService().GetCustomerList(new CustomerCriteria(), _greatPlains.GpContext);
                try
                {
                    gpCustomer = _greatPlains.GetService().GetCustomerByKey(customerKey, _greatPlains.GpContext);
                    _gpCustomer = gpCustomer;
                }
                catch (Exception)
                {
                    var genricAttributesOfCustomer = _genericAttributeService.GetAttributesForEntity(order.Customer.Id, "Customer");
                    foreach (var genricAttribute in genricAttributesOfCustomer)
                    {
                        if (genricAttribute.Key == "gp_customerkey")
                        {
                            _genericAttributeService.DeleteAttribute(genricAttribute);
                        }
                    }

                    createCustomer(order);
                }

                if (gpCustomer != null)
                {
                    // Add the missing gpAddresses to the order
                    GPAddresses gpAddresses = new GPAddresses(_addressService, _taxService, _greatPlains, _logger);
                    gpAddresses.AddMissingAddresses(gpCustomer, order);
                }
                // If I can't update the GPCustomer, I will create a new one.
                else
                {
                    createCustomer(order);
                }
            }
            catch (Exception ex)
            {
                //createCustomer(order);
                _logger.Error("CreateCustomer", ex);
            }

            // Update the Customer
            try
            {
                if (gpCustomer != null)
                {
                    Policy oPolicy = _greatPlains.GetService().GetPolicyByOperation("UpdateCustomer", _greatPlains.GpContext);
                    _greatPlains.GetService().UpdateCustomer(gpCustomer, _greatPlains.GpContext, oPolicy);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("UpdateCustomer", ex);
            }
        }

        /// <summary>
        /// Update or Add the nopCommerce to Great Plains key cross reference.
        /// </summary>
        /// <param name="iNopCustomerId">NopCommerce Customer Id</param>
        /// <param name="strGpCustomerId">Great Plains Customer Key</param>
        private void updateCustomerReference(Nop.Core.Domain.Customers.Customer customer, CustomerKey key)
        {
            _genericAttributeService.SaveAttribute<string>(customer, "gp_customerkey", key.Id.Trim(), 0);
        }

        #region Properties

        public DynamicsGP.Customer Customer
        {
            get
            {
                return _gpCustomer;
            }
            set
            {
                _gpCustomer = value;
            }
        }

        public CustomerKey CustKey
        {
            get
            {
                return _customerKey;
            }
            set
            {
                _customerKey = value;
            }
        }

        public string Name
        {
            get
            {
                return _customerName;
            }
            set
            {
                _customerName = value.ToUpper();
            }
        }



        #endregion
    }
}
