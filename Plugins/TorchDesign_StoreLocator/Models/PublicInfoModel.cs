using Nop.Web.Framework.Mvc;
using System.Web.Mvc;
using System.Collections.Generic;

namespace Nop.Plugin.Widgets.TorchDesign_StoreLocator.Models
{
    public class PublicInfoModel : BaseNopEntityModel 
    {
        public PublicInfoModel ()
	{
        AvailableLocation = new List<string>();
        Availablestores = new List<Availablestores>();
        SearchStoreOption = new List<SelectListItem>();
        Radiusoption = new List<SelectListItem>();
        
	}
        public string Description { get; set; }
        public string SearchContant { get; set; }
        public int radius { get; set; }
        public IList<Availablestores> Availablestores { get; set; }
        public IList<string> AvailableLocation { get; set; }
        public bool resultfound { get; set; }
        public string firstLatitude { get; set; }
        public string firstLongitude { get; set; }
        public string locationsjson { get; set; }
        public string infoWindowContentsjson { get; set; }
       
        public string sourcestoreaddress { get; set; }
        public string destinationstoreaddress { get; set; }
        public bool getdirecrtion { get; set; }

        public bool storefound { get; set; }
        public int SearchStoreOptionId { get; set;}
        public IList<SelectListItem> SearchStoreOption { get; set; }
        public IList<SelectListItem> Radiusoption { get; set; }
        //public string CurrentLatitude { get; set; }
        //public string CurrentLongitude { get; set; }
        public string GoogleMapAPIKey { get; set; }
            
    }
    public class Availablestores : BaseNopEntityModel
    {
        public int StoreNumber { get; set; }
        public string StoreName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string CountryCode { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string StoreType { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public double DistanceFromAddress { get; set; }
    }
}
