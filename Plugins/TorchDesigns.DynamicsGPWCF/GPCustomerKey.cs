using Nop.Plugin.TorchDesigns.DynamicsGPWCF.DynamicsGP;
using Nop.Core.Domain.Orders;

namespace Nop.Plugin.TorchDesigns.DynamicsGPWCF
{
    class GPCustomerKey
    {
        CustomerKey _oCustomerKey;
        GreatPlains _greatPlains;

        /// <summary>
        /// Constructor creates a customer key based upon the billing address of the provided order.
        /// </summary>
        /// <param name="order">nopCommerce Order</param>
        public GPCustomerKey(GreatPlains greatPlains, Order order)
        {
            this._greatPlains = greatPlains;
            createCustomerKey(order);
        }

        /// <summary>
        /// Creates a new Customer Key that is not already in use.  The key is based upon the Customer's Billing Address.
        /// </summary>
        /// <param name="order">nopCommerce Order</param>
        public void createCustomerKey(Order order)
        {
            // Init
            CustomerKey customerKey = new CustomerKey();
            string strCustomerKey = "";
            string strLastName = order.BillingAddress.LastName.ToUpper().Trim();
            string strFirstName = order.BillingAddress.FirstName.ToUpper().Trim();

            // If the key is already in use, add a suffix
            try
            {
                int iSuffix = 0;
                string strSuffix = "";
                int iMaxLength = 13;
                DynamicsGP.Customer gpCustomer = null;
                do
                {
                    // If the last name is longer than 13 characters, shorten it to make room for the first initial.
                    if (strLastName.Length > iMaxLength)
                    {
                        strLastName = strLastName.Substring(0, iMaxLength);
                    }
                    strCustomerKey = strLastName + "-" + strFirstName;

                    // If the key will be longer than 15 characters, shorten it to stay within Great Plains field limitations
                    if (strCustomerKey.Length > (iMaxLength + 2))
                    {
                        strCustomerKey = strCustomerKey.Substring(0, (iMaxLength + 2));
                    }
                    strCustomerKey = strCustomerKey + strSuffix;
                    customerKey.Id = strCustomerKey;

                    gpCustomer = _greatPlains.GetService().GetCustomerByKey(customerKey, _greatPlains.GpContext);
                    if (gpCustomer != null)
                    {
                        iSuffix++;
                        iMaxLength = 13 - iSuffix.ToString().Trim().Length;
                        strSuffix = iSuffix.ToString().Trim();
                    }
                } while (gpCustomer != null);
            }
            catch 
            {
                // Do nothing - Not finding the key is actually the desired response.
            }
            Key = customerKey;
        }

        /// <summary>
        /// Great Plains Customer Key
        /// </summary>
        public CustomerKey Key
        {
            get
            {
                return _oCustomerKey;
            }
            set
            {
                _oCustomerKey = value;
            }
        }
    }
}
