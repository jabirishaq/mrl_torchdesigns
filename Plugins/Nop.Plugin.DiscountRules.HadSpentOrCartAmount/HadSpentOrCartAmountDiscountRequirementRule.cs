using System;
using System.Linq;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Core.Plugins;
using Nop.Services.Configuration;
using Nop.Services.Discounts;
using Nop.Services.Localization;
using Nop.Services.Orders;
using Nop.Core.Domain.Payments;

namespace Nop.Plugin.DiscountRules.HadSpentOrCartAmount
{
    public partial class HadSpentOrCartAmountDiscountRequirementRule : BasePlugin, IDiscountRequirementRule
    {
        private readonly ISettingService _settingService;
        private readonly IOrderService _orderService;

        public HadSpentOrCartAmountDiscountRequirementRule(ISettingService settingService, 
            IOrderService orderService)
        {
            this._settingService = settingService;
            this._orderService = orderService;
        }

        /// <summary>
        /// Check discount requirement
        /// </summary>
        /// <param name="request">Object that contains all information required to check the requirement (Current customer, discount, etc)</param>
        /// <returns>true - requirement is met; otherwise, false</returns>
        public bool CheckRequirement(CheckDiscountRequirementRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (request.DiscountRequirement == null)
                throw new NopException("Discount requirement is not set");

            var spentAmountRequirement = _settingService.GetSettingByKey<decimal>(string.Format("DiscountRequirement.HadSpentOrCartAmount-{0}", request.DiscountRequirement.Id));

            if (spentAmountRequirement == decimal.Zero)
                return true;

            if (request.Customer == null)
                return false;
            var orders = _orderService.SearchOrders(storeId: request.Store.Id, 
                customerId: request.Customer.Id).Where(x=> x.PaymentStatus == PaymentStatus.Paid || x.PaymentStatus == PaymentStatus.Authorized);
               
            decimal spentAmount = orders.Sum(o => o.OrderTotal);
            decimal price = spentAmount;
            var cartitem = request.Customer.ShoppingCartItems.Where(x=>x.ShoppingCartType==ShoppingCartType.ShoppingCart).ToList();
            foreach(var item in cartitem)
            {              
               price= price + (item.Quantity * item.Product.Price);
            }

            if (price > spentAmountRequirement)
            {
                return true;
            }
             else 
            {
                return false;
            }
           
        }

        /// <summary>
        /// Get URL for rule configuration
        /// </summary>
        /// <param name="discountId">Discount identifier</param>
        /// <param name="discountRequirementId">Discount requirement identifier (if editing)</param>
        /// <returns>URL</returns>
        public string GetConfigurationUrl(int discountId, int? discountRequirementId)
        {
            //configured in RouteProvider.cs
            string result = "Plugins/DiscountRulesHadSpentOrCartAmount/Configure/?discountId=" + discountId;
            if (discountRequirementId.HasValue)
                result += string.Format("&discountRequirementId={0}", discountRequirementId.Value);
            return result;
        }

        public override void Install()
        {
            //locales
            this.AddOrUpdatePluginLocaleResource("Plugins.DiscountRules.HadSpentOrCartAmount.Fields.Amount", "Required spent amount");
            this.AddOrUpdatePluginLocaleResource("Plugins.DiscountRules.HadSpentOrCartAmount.Fields.Amount.Hint", "Discount will be applied if customer has spent/purchased x.xx amount.");
            base.Install();
        }

        public override void Uninstall()
        {
            //locales
            this.DeletePluginLocaleResource("Plugins.DiscountRules.HadSpentOrCartAmount.Fields.Amount");
            this.DeletePluginLocaleResource("Plugins.DiscountRules.HadSpentOrCartAmount.Fields.Amount.Hint");
            base.Uninstall();
        }
    }
}