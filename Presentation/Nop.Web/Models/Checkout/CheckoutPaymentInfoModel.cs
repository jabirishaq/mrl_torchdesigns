using System.Web.Routing;
using Nop.Web.Framework.Mvc;
using System.Collections.Generic;

namespace Nop.Web.Models.Checkout
{
    public partial class CheckoutPaymentInfoModel : BaseNopModel
    {
        public CheckoutPaymentInfoModel()
        {
            Warnings = new List<string>();
        }
        public string PaymentInfoActionName { get; set; }
        public string PaymentInfoControllerName { get; set; }
        public RouteValueDictionary PaymentInfoRouteValues { get; set; }

        public IList<string> Warnings { get; set; }

        /// <summary>
        /// Used on one-page checkout page
        /// </summary>
        public bool DisplayOrderTotals { get; set; }

        /// <summary>
        /// Gets or sets the identifier value whether reCaptcha should be displayed or not
        /// </summary>
        public bool DisplayCaptcha { get; set; }
    }
}