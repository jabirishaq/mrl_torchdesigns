using Nop.Web.Framework;

namespace Nop.Plugin.DiscountRules.HadSpentOrCartAmount.Models
{
    public class RequirementModel
    {
        [NopResourceDisplayName("Plugins.DiscountRules.HadSpentOrCartAmount.Fields.Amount")]
        public decimal SpentAmount { get; set; }

        public int DiscountId { get; set; }

        public int RequirementId { get; set; }
    }
}