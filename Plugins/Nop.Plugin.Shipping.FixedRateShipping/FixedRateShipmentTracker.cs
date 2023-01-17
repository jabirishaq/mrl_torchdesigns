using Nop.Services.Shipping.Tracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Shipping.FixedRateShipping
{
    public class FixedRateShipmentTracker : IShipmentTracker
    {
        public IList<ShipmentStatusEvent> GetShipmentEvents(string trackingNumber)
        {
            throw new NotImplementedException();
        }

        /// Gets a url for a page to show tracking info (third party tracking page).
        /// </summary>
        /// <param name="trackingNumber">The tracking number to track.</param>
        /// <returns>A url to a tracking page.</returns>
        public virtual string GetUrl(string trackingNumber)
        {
            if (trackingNumber.StartsWith("1Z"))
            {
                string url = "http://wwwapps.ups.com/WebTracking/track?track=yes&trackNums={0}";
                url = string.Format(url, trackingNumber);
                return url;
            }
            else
            {
                string url = "http://trkcnfrm1.smi.usps.com/PTSInternetWeb/InterLabelInquiry.do?origTrackNum={0}";
                url = string.Format(url, trackingNumber);
                return url;
            }
        }

        public bool IsMatch(string trackingNumber)
        {
            throw new NotImplementedException();
        }
    }
}
