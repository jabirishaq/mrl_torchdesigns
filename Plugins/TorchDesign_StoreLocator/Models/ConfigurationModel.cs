using System;
using Nop.Web.Framework;
using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Localization;
using System.Web.Mvc;
using System.Collections.Generic;

namespace Nop.Plugin.Widgets.TorchDesign_StoreLocator.Models
{
    public partial class ConfigurationModel : BaseNopEntityModel
    {
        public ConfigurationModel()
        {

            AvailableOptions = new List<SelectListItem>();

        }


        [NopResourceDisplayName("Plugins.Widgets.Storelocator.StoreNumber")]
        //[AllowHtml]
        //[Range(0, int.MaxValue, ErrorMessage = "Store Number Mustbe Positive Number And More Then Zero.")]
        public int StoreNumber { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.Storelocator.Description")]
        [AllowHtml]
        public string Description { get; set; }
        public bool Description_Override { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.Storelocator.GoogleAPIKey")]
        [AllowHtml]
        public string GoogleAPIKey { get; set; }


        [StringLength(50, ErrorMessage = " In StoreName Maximum 50 characters are allowed")]
        [NopResourceDisplayName("Plugins.Widgets.Storelocator.StoreName")]
        [AllowHtml]
        public string StoreName { get; set; }

         [StringLength(100, ErrorMessage = "In Address Maximum 100 characters are allowed")]
        [NopResourceDisplayName("Plugins.Widgets.Storelocator.Address")]
        [AllowHtml]
        public string Address { get; set; }

         [StringLength(50, ErrorMessage = "In City Maximum 50 characters are allowed")]
        [NopResourceDisplayName("Plugins.Widgets.Storelocator.City")]
        [AllowHtml]
        public string City { get; set; }

         [StringLength(50, ErrorMessage = "In Region Maximum 50 characters are allowed")]
        [NopResourceDisplayName("Plugins.Widgets.Storelocator.Region")]
        [AllowHtml]
        public string Region { get; set; }

         [StringLength(2, ErrorMessage = "In CountryCode Maximum 2 characters are allowed")]
        [NopResourceDisplayName("Plugins.Widgets.Storelocator.CountryCode")]
        [AllowHtml]
        public string CountryCode { get; set; }

         [StringLength(10, ErrorMessage = "In PostalCode Maximum 10 characters are allowed")]
        [NopResourceDisplayName("Plugins.Widgets.Storelocator.PostelCode")]
        [AllowHtml]
        public string PostalCode { get; set; }

         [StringLength(14, ErrorMessage = "In PhoneNumber Maximum 14 characters are allowed")]
        [NopResourceDisplayName("Plugins.Widgets.Storelocator.PhoneNumber")]
        [AllowHtml]
        public string PhoneNumber { get; set; }

         [StringLength(20, ErrorMessage = "In StoreType Maximum 20 characters are allowed")]
        [NopResourceDisplayName("Plugins.Widgets.Storelocator.StoreType")]
        [AllowHtml]
        public string StoreType { get; set; }

      
        [NopResourceDisplayName("Plugins.Widgets.Storelocator.Latitude")]
        [AllowHtml]
        public decimal Latitude { get; set; }

      
        [NopResourceDisplayName("Plugins.Widgets.Storelocator.Longitude")]
        [AllowHtml]
        public decimal Longitude { get; set; }

        public double DistanceFromAddress { get; set; }

        public IList<SelectListItem> AvailableOptions { get; set; }

    }

}

