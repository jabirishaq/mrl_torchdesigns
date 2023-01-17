using System;
using Nop.Plugin.TorchDesigns.DynamicsGPWCF.DynamicsGP;
using Nop.Core.Domain.Orders;
using Nop.Services.Configuration;
using Nop.Services.Logging;
using Nop.Services.Orders;
using Nop.Services.Shipping;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Tax;
using System.Net;
using Nop.Core.Infrastructure;

namespace Nop.Plugin.TorchDesigns.DynamicsGPWCF
{
    public class GreatPlains
    {
        Uri _serviceAddress; // Dynamics GP Web Service URI
        bool _bClientCertEnabled = false; // True when Client Cert authentication is enabled
        bool _bEnabled = false; // True when the Great Plains interface is enabled
        string _strClientCertPath; // Path to the Client Certificate
        string _strClientCertKey;  // Password for the Client Certs private key
        string _strErrorMessage; // Error Message
        int? _iCompanyKey = null; // Company Key

        private Company[] _companyList;
        private Context _context;
        readonly ISettingService _settingService;
        readonly ILogger _logger;
        readonly IOrderService _orderService;
        readonly IShippingService _shippingService;
        readonly IGenericAttributeService _genericAttributeService;
        readonly ICustomerService _customerService;
        readonly ITaxService _taxService;
        readonly IAddressService _addressService;
        private readonly TorchDesignsDynamicsGPWCFSettings _torchDesignsDynamicsGPSettings;

        public GreatPlains(ISettingService settingService, ILogger logger, IOrderService orderService, IShippingService shippingService, IGenericAttributeService genericAttributeService, 
            ICustomerService customerService, ITaxService taxService, IAddressService _addressService)
        {
            _settingService = settingService;
            _orderService = orderService;
            _shippingService = shippingService;
            _genericAttributeService = genericAttributeService;
            _customerService = customerService;
            _taxService = taxService;
            _logger = logger;
            _context = new Context();
            _torchDesignsDynamicsGPSettings = EngineContext.Current.Resolve<TorchDesignsDynamicsGPWCFSettings>();

            BindData();
        }

        /// <summary>
        /// Retrieves saved values from the database and binds them to this instance.
        /// </summary>
        private void BindData()
        {
            string strErrorMessage = "<p>GreatPlains.BindData:  Could not connect to Great Plains.</p>";

            try
            {
                //string clientCertEnabled = GetProperty("gp_clientcertenabled", false).Trim();
                //strErrorMessage += "<p>gp_clientcertenabled=" + clientCertEnabled + "</p>";
               
                string enabled = GetProperty("gp_enabled", false).Trim();
                strErrorMessage += "<p>gp_enabled=" + enabled + "</p>";

                string webServiceAddress = _torchDesignsDynamicsGPSettings.WebServiceAddress;// GetProperty("gp_webserviceaddress", false);
                strErrorMessage += "<p>gp_webserviceaddress=" + webServiceAddress + "</p>";
                //if (!String.IsNullOrEmpty(clientCertEnabled))
                //{
                //    ClientCertEnabled = Boolean.Parse(clientCertEnabled);
                //}
                if (!String.IsNullOrEmpty(enabled))
                {
                    Enabled = Boolean.Parse(enabled);
                }
                //ClientCertPath = GetProperty("gp_clientcertpath", false);
                //strErrorMessage += "<p>gp_clientcertpath=" + ClientCertPath + "</p>";

                //ClientCertKey = GetProperty("gp_clientcertkey", false);//TODO Encrypt the ClientCertKey when it is stored
                //strErrorMessage += "<p>gp_clientcertkey=" + ClientCertKey + "</p>";

                if (string.IsNullOrEmpty(webServiceAddress) == false)
                {
                    WebServiceAddress = new Uri(webServiceAddress);
                }

                string companyKey = _torchDesignsDynamicsGPSettings.CompanyKey;// GetProperty("gp_companykey", false);
                strErrorMessage += "<p>gp_companykey=" + companyKey + "</p>";
                if (String.IsNullOrWhiteSpace(companyKey) == false)
                {
                    CompanyKey = Convert.ToInt32(companyKey);
                }
                GetCompanyList();
            }
            catch (Exception ex)
            {
                _logger.Information(strErrorMessage, ex, null);
                _logger.InsertLog(Core.Domain.Logging.LogLevel.Error, ex.Message, strErrorMessage,null);
            }
        }

        #region Properties

        /// <summary>
        /// True when the ClientCert will be used for authentication
        /// </summary>
        public bool ClientCertEnabled
        {
            get
            {
                return _bClientCertEnabled;
            }
            set
            {
                if (_bClientCertEnabled != value)
                {
                    _bClientCertEnabled = value;
                    UpdateProperty("ClientCertEnabled", value.ToString(), false);
                }
            }
        }

        /// <summary>
        /// The local path to the Client Certificate
        /// </summary>
        public string ClientCertPath
        {
            get
            {
                return _strClientCertPath;
            }
            set
            {
                string newValue = value.Trim();

                if (_strClientCertPath != newValue && String.IsNullOrWhiteSpace(value) == false)
                {
                    _strClientCertPath = newValue;
                    UpdateProperty("ClientCertPath", newValue, false);
                }
            }

        }

        /// <summary>
        /// Private Key Password for the Client Certificate
        /// </summary>
        public string ClientCertKey
        {
            get
            {
                return _strClientCertKey;
            }
            set
            {
                if (_strClientCertKey != value && String.IsNullOrWhiteSpace(value) == false)
                {
                    _strClientCertKey = value;
                    UpdateProperty("ClientCertKey", value, true);
                }
            }
        }   

        public int? CompanyKey
        {
            get
            {
                return _iCompanyKey;
            }
            set
            {
                if (_iCompanyKey != value)
                {
                    _iCompanyKey = value;
                    UpdateProperty("CompanyKey", value.ToString(), false);
                }
            }
        }

        public Company[] CompanyList
        {
            get
            {
                return _companyList;
            }
        }

        public Context GpContext
        {
            get
            {
                return _context;
            }
        }

        /// <summary>
        /// True when the Dynamics GP interface is enabled.
        /// </summary>
        public bool Enabled
        {
            get
            {
                return _bEnabled;
            }
            set
            {
                if (_bEnabled != value)
                {
                    _bEnabled = value;
                    UpdateProperty("Enabled", value.ToString(), false);
                }
            }
        }

        public string ErrorMessage
        {
            get
            {
                return _strErrorMessage;
            }
            set
            {
                _strErrorMessage = value;
            }
        }

        /// <summary>
        /// Dynamics GP Web Service URI
        /// </summary>
        public Uri WebServiceAddress
        {
            get
            {
                return _serviceAddress;
            }
            set
            {
                if (value != null)
                {
                    if (_serviceAddress != value)
                    {
                        _serviceAddress = value;
                        UpdateProperty("WebServiceAddress", value.ToString().Trim(), false);
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// Submit a Sales Order to Great Plains.  During this process the GPCustomer will be updated or added as needed.
        /// </summary>
        /// <param name="CustomerOrder"></param>
        /// <returns></returns>
        public void SubmitSalesOrder(Order CustomerOrder)
        {
            try
            {
                // Initialize
                GPSalesOrder salesOrder = new GPSalesOrder(_orderService, _shippingService, _genericAttributeService, _customerService, _logger,_taxService, _addressService, this);
                salesOrder.SubmitSalesOrder(CustomerOrder);

            }
            catch (Exception ex)
            {
                _logger.Error("GreatPlains.SubmitSalesOrder", ex,null);
            }
        }

        private string GetCompanyList()
        {
            string message = "";

            if (String.IsNullOrWhiteSpace(WebServiceAddress.ToString()) == false)
            {
                _logger.Information("webServiceAddress = " + WebServiceAddress.ToString());
                try
                {
                    CompanyCriteria companyCriteria = new CompanyCriteria();
                    _companyList = GetService().GetCompanyList(companyCriteria, new Context());
                    message = _companyList[0].Name;
                }
                catch (Exception ex)
                {
                    string strErrorMessage = "<p>GreatPlains.GetCompanyList:  Could not connect to Great Plains.</p>";
                    strErrorMessage += "<p>gp_clientcertenabled=" + ClientCertEnabled + "</p>";
                    strErrorMessage += "<p>gp_enabled=" + Enabled + "</p>";
                    strErrorMessage += "<p>gp_webserviceaddress=" + WebServiceAddress.ToString() + "</p>";
                    strErrorMessage += "<p>gp_clientcertpath=" + ClientCertPath + "</p>";
                    strErrorMessage += "<p>gp_clientcertkey=" + ClientCertKey + "</p>";
                    strErrorMessage += "<p>gp_companykey=" + CompanyKey + "</p>";
                    strErrorMessage += "<p>" + ex.InnerException + "</p>";

                    _logger.Information(strErrorMessage, ex, null);
                    _logger.InsertLog(Core.Domain.Logging.LogLevel.Error, ex.Message, strErrorMessage, null);
                }
            }
            return message;
        }

        /// <summary>
        /// Get a connection to the defined Great Plains Web Service
        /// </summary>
        /// <returns></returns>
        public Nop.Plugin.TorchDesigns.DynamicsGPWCF.DynamicsGP.DynamicsGP GetService()
        {
            try
            {
                // Create Service
                DynamicsGP.DynamicsGP gpService = new DynamicsGP.DynamicsGP();

                // Set the web service address
                gpService.Url = _torchDesignsDynamicsGPSettings.WebServiceAddress;
                // Set Credentials

                var password = string.Empty;
                var base64EncodedBytes = System.Convert.FromBase64String(_torchDesignsDynamicsGPSettings.Password);
                password = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
                NetworkCredential credentials = new NetworkCredential(_torchDesignsDynamicsGPSettings.UserName, password, _torchDesignsDynamicsGPSettings.Domain);
                gpService.Credentials = credentials;

                // Set the Organization Name
                if (_iCompanyKey != null)
                {
                    CompanyKey key = new CompanyKey();
                    key.Id = CompanyKey.Value;
                    _context.OrganizationKey = (OrganizationKey)key;
                }

                return gpService;
            }
            catch (Exception ex)
            {
                _logger.Error("Error in GreatPlains.cs -> GetService() . Error message: " + ex.Message);
                return null;
            }
           
        }

        /// <summary>
        /// Get a property
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Encrypted"></param>
        /// <returns></returns>
        private string GetProperty(string Key, bool Encrypted)
        {
            // Initialize
            string Value = _settingService.GetSettingByKey<string>(Key);
            
            if (Encrypted && String.IsNullOrEmpty(Value) == false)
            {
                Value = Crypto.DecryptStringAES(Value, Key.Trim() + "ref1ned");
            }

            return Value;
        }

        /// <summary>
        /// Updates a Configuration Variable
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        private void UpdateProperty(string Key, string Value, bool Encrypted)
        {
            if (Encrypted && String.IsNullOrEmpty(Value) == false)
            {
                Value = Crypto.EncryptStringAES(Value, Key.Trim() + "ref1ned");
            }

            _settingService.SetSetting<string>(Key, Value, 0, true);

        }

    }
}
