﻿using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.TorchDesign.CustomerOrigin.Models
{
    public partial class CheckoutProgressModel : BaseNopModel
    {
        public CheckoutProgressStep CheckoutProgressStep { get; set; }
    }

    public enum CheckoutProgressStep
    {
        Cart,
        Address,
        Shipping,
        Payment,
        Confirm,
        Complete
    }
}