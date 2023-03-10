using Nop.Core.Configuration;

namespace Nop.Plugin.TorchDesign.PayflowPro
{
    public class TorchDesignPayflowProSettings : ISettings
    {
        public TransactMode TransactMode { get; set; }
        public bool UseSandbox { get; set; }
        public string ApiAccountName { get; set; }
        public string ApiAccountPassword { get; set; }
        public string Partner { get; set; }
        public string Vendor { get; set; }
        public string Signature { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether to "additional fee" is specified as percentage. true - percentage, false - fixed value.
        /// </summary>
        public bool AdditionalFeePercentage { get; set; }
        /// <summary>
        /// Additional fee
        /// </summary>
        public decimal AdditionalFee { get; set; }
    }
}
