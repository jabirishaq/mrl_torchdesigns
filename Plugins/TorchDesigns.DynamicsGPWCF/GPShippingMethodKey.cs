using System;
using Nop.Core.Domain.Orders;
using Nop.Plugin.TorchDesigns.DynamicsGPWCF.DynamicsGP;

namespace Nop.Plugin.TorchDesigns.DynamicsGPWCF
{
    public static class GPShippingMethodKey
    {
        /// <summary>
        /// Converts a NopCommerce Shipping Method into a Great Plains ShippingMethodKey
        /// </summary>
        /// <param name="strNopShippingMethod">The shipping method specified in NopCommerce</param>
        /// <returns>Great Plains ShippingMethodKey</returns>
        public static ShippingMethodKey GetGpShippingMethodKey(Order nopOrder)
        {
            // TODO Make this a mappable field using the database
            ShippingMethodKey oKey = new ShippingMethodKey();

            String strNopShippingMethod = nopOrder.ShippingMethod.ToUpper().Trim();
            String strNopShippingModule = nopOrder.ShippingRateComputationMethodSystemName.ToUpper().Trim();

            oKey.Id = "";

            //Added this condition as per added task "When the user selects free shipping, it should export to GP as 'Flat Rate'."
            if (nopOrder.OrderShippingExclTax == decimal.Zero)
                oKey.Id = "FLAT RATE";
            else
            {
                if (strNopShippingModule == "SHIPPING.UPS")
                {
                    if (strNopShippingMethod.ToUpper().Trim() == "UPS WORLDWIDE EXPEDITED")
                    {
                        oKey.Id = "UPS WORLDWIDE";
                    }
                    else if (strNopShippingMethod.ToUpper().Trim() == "UPS 3 DAY SELECT")
                    {
                        oKey.Id = "UPS 3DAY SELECT";
                    }
                    else
                    {
                        oKey.Id = strNopShippingMethod;
                    }
                }
                else if (strNopShippingModule == "SHIPPING.USPS")
                {
                    if(strNopShippingMethod.Contains("INTERNATIONAL"))
                    {
                        if (strNopShippingMethod.Contains("EXPRESS"))
                        {
                            oKey.Id = "USPS EXPRE INTL";
                        }
                        else if (strNopShippingMethod.Contains("PRIORITY"))
                        {
                            oKey.Id = "USPS PRIOR INTL";
                        }
                        else if (strNopShippingMethod.Contains("FIRST"))
                        {
                            oKey.Id = "USPS FIRST INTL";
                        }
                        else
                        {
                            oKey.Id = strNopShippingMethod;
                        }
                    }
                    else
                    {
                        if(strNopShippingMethod.Contains("EXPRESS"))
                        {
                            oKey.Id = "USPS EXPRESS";
                        }
                        else if (strNopShippingMethod.Contains("PRIORITY"))
                        {
                            oKey.Id = "USPS PRIORITY";
                        }
                        else
                        {
                            oKey.Id = strNopShippingMethod;
                        }

                    }
                }
                else if (strNopShippingModule == "SHIPPING.FIXEDRATE")
                {
                    oKey.Id = "FLAT RATE";
                }

            }

            return oKey;
        }
    }
}
