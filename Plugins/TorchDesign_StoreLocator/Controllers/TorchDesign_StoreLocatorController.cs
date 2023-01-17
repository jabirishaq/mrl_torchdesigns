using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Nop.Core;
using Nop.Plugin.Widgets.TorchDesign_StoreLocator.Services;
using Nop.Plugin.Widgets.TorchDesign_StoreLocator.Domain;
using Nop.Plugin.Widgets.TorchDesign_StoreLocator.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Kendoui;
using Nop.Web.Framework.Mvc;
using Nop.Services.Stores;
using System.Xml.Linq;
using Nop.Data;
using System.Data;
using Nop.Core.Data;
using Nop.Plugin.Widgets.TorchDesign_StoreLocator.Data;
using System.Drawing;
using System.IO;
using System.Web;
using Nop.Core.Infrastructure;

namespace Nop.Plugin.Widgets.TorchDesign_StoreLocator.Controllers
{

    public class TorchDesign_StoreLocatorController : BasePluginController
    {
        private readonly IDbContext _dbContext;
        private readonly IStoreLocatorService _StorelocatorService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly ISettingService _settingService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly IDataProvider _dataProvider;
        private readonly StoreLocatorObjctContext _storeLocatorContext;
        private readonly StoreLocatorSettings _storelocatorsetting;

        public TorchDesign_StoreLocatorController(IDbContext dbContext, IStoreLocatorService StorelocatorService, ILanguageService languageService,
            ILocalizationService localizationService, ILocalizedEntityService localizedEntityService,
            ISettingService settingService, IStoreService storeService, IWorkContext workContext,
            IDataProvider dataProvider, StoreLocatorObjctContext storeLocatorContext, StoreLocatorSettings storelocatorsetting)
        {
            this._dbContext = dbContext;
            this._StorelocatorService = StorelocatorService;
            this._languageService = languageService;
            this._localizationService = localizationService;
            this._localizedEntityService = localizedEntityService;
            this._settingService = settingService;
            this._storeService = storeService;
            this._workContext = workContext;
            this._dataProvider = dataProvider;
            this._storeLocatorContext = storeLocatorContext;
            this._storelocatorsetting = storelocatorsetting;
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            //little hack here
            //always set culture to 'en-US' (Telerik has a bug related to editing decimal values in other cultures). Like currently it's done for admin area in Global.asax.cs
            CommonHelper.SetTelerikCulture();

            base.Initialize(requestContext);
        }

        #region Configure

        public ActionResult AddPopup()
        {
            var model = new ConfigurationModel();


            model.AvailableOptions.Add(new SelectListItem() { Text = "Lowe's", Value = "Lowe's" });
            model.AvailableOptions.Add(new SelectListItem() { Text = "Other", Value = "Other" });
            model.DistanceFromAddress = 0;

            return View("~/Plugins/Widgets.TorchDesign_StoreLocator/Views/TorchDesign_StoreLocator/AddPopup.cshtml", model);
        }


        [HttpPost]
        public ActionResult AddPopup(string btnId, string formId, ConfigurationModel model)
        {
            if(model.StoreType == "Lowe's" && model.StoreNumber <= 0)
            { 
                ModelState.AddModelError("", "Please Enter Store Number Greater Then Zero");
            }
          
            if (ModelState.IsValid)
            {
                StoreLocator Sobj = null;
                if (model.StoreType == "Other")
                {
                    Sobj = new StoreLocator
                    {
                        Id = model.Id,
                        StoreNumber = model.StoreNumber,
                        StoreName = model.StoreName,
                        StoreType = model.StoreType,
                        Address = model.Address,
                        City = model.City,
                        CountryCode = model.CountryCode,
                        PhoneNumber = model.PhoneNumber,
                        Region = model.Region,
                        PostalCode = model.PostalCode,
                        Latitude = model.Latitude,
                        Longitude = model.Longitude,
                        DistanceFromAddress=0
                    };
                }
                else
                {

                    Sobj = new StoreLocator
                    {
                        Id = model.Id,
                        StoreNumber = model.StoreNumber,
                        StoreName = null,
                        StoreType = model.StoreType,
                        Address = model.Address,
                        City = model.City,
                        CountryCode = model.CountryCode,
                        PhoneNumber = model.PhoneNumber,
                        Region = model.Region,
                        PostalCode = model.PostalCode,
                        Latitude = model.Latitude,
                        Longitude = model.Longitude,
                        DistanceFromAddress=0
                    };
                }

                // If the Longitude and Latitude are undefined
                if (Sobj.Latitude == 0 && Sobj.Longitude == 0)
                {
                    // Get Longitude and Latitude from Google
                    string strFullAddress = Sobj.Address + ";" + Sobj.City + ", " + Sobj.Region + " " + Sobj.PostalCode;
                    var oGoogleResults = GetGeocodingSearchResults(strFullAddress.Trim());
                    var status = oGoogleResults.Element("status").Value;
                    if (status == "OK")
                    {
                        Sobj.Latitude = Convert.ToDecimal(oGoogleResults.Element("result").Element("geometry").Element("location").Element("lat").Value);
                        Sobj.Longitude = Convert.ToDecimal(oGoogleResults.Element("result").Element("geometry").Element("location").Element("lng").Value);
                    }
                }

                // Add store to repository
                _StorelocatorService.InsertStoreLocation(Sobj);

                ViewBag.RefreshPage = true;
                ViewBag.btnId = btnId;
                ViewBag.formId = formId;
                model.AvailableOptions.Add(new SelectListItem() { Text = "Lowe's", Value = "Lowe's" });
                model.AvailableOptions.Add(new SelectListItem() { Text = "Other", Value = "Other" });

            }

            model.AvailableOptions.Add(new SelectListItem() { Text = "Lowe's", Value = "Lowe's" });
            model.AvailableOptions.Add(new SelectListItem() { Text = "Other", Value = "Other" });
            model.DistanceFromAddress = 0;
            return View("~/Plugins/Widgets.TorchDesign_StoreLocator/Views/TorchDesign_StoreLocator/AddPopup.cshtml", model);

        }

        public static XElement GetGeocodingSearchResults(string address)
        {
            // Use the Google Geocoding service to get information about the user-entered address
            // See http://code.google.com/apis/maps/documentation/geocoding/index.html for more info...
            //var url = String.Format("http://maps.google.com/maps/api/geocode/xml?address={0}&sensor=false&zoom=12", HttpUtility.UrlEncode(address));
            var googleAPISetting = EngineContext.Current.Resolve<StoreLocatorSettings>();
            var url = string.Format("https://maps.googleapis.com/maps/api/geocode/xml?address={0}&zoom=12&key={1}", HttpUtility.UrlEncode(address), googleAPISetting.GoogleAPIKey);

            // Load the XML into an XElement object (whee, LINQ to XML!)
            var results = XElement.Load(url);

            // Check the status
            var status = results.Element("status").Value;
            if (status != "OK" && status != "ZERO_RESULTS")
                // Whoops, something else was wrong with the request...
                throw new ApplicationException("There was an error with Google's Geocoding Service: " + status);

            return results;
        }

        public ActionResult EditPopup(int id)
        {

            var Sobj = _StorelocatorService.GetStoreLocationById(id);
            if (Sobj == null)
                //No record found with the specified id
                return RedirectToAction("Configure");

            var model = new ConfigurationModel
            {
                Id = Sobj.Id,
                StoreNumber = Sobj.StoreNumber,
                StoreName = Sobj.StoreName,
                StoreType = Sobj.StoreType,
                Address = Sobj.Address,
                City = Sobj.City,
                CountryCode = Sobj.CountryCode,
                PhoneNumber = Sobj.PhoneNumber,
                Region = Sobj.Region,
                PostalCode = Sobj.PostalCode,
                Latitude = Sobj.Latitude,
                Longitude = Sobj.Longitude,
                DistanceFromAddress=0

            };
            if (Sobj.StoreType == "Lowe's")
            {
                model.AvailableOptions.Add(new SelectListItem() { Text = "Lowe's", Value = "Lowe's" });
                model.AvailableOptions.Add(new SelectListItem() { Text = "Other", Value = "Other" });

            }
            else
            {
                model.AvailableOptions.Add(new SelectListItem() { Text = "Other", Value = "Other" });
                model.AvailableOptions.Add(new SelectListItem() { Text = "Lowe's", Value = "Lowe's" });

            }
            return View("~/Plugins/Widgets.TorchDesign_StoreLocator/Views/TorchDesign_StoreLocator/EditPopup.cshtml", model);

        }

        [HttpPost]
        public ActionResult EditPopup(string btnId, string formId, ConfigurationModel model)
        {
            if (model.StoreType == "Lowe's" && model.StoreNumber <= 0)
            {
                ModelState.AddModelError("", "Please Enter Store Number Greater Then Zero");
            }
            if (ModelState.IsValid)
            {

                var obj = _StorelocatorService.GetStoreLocationById(model.Id);
                if (obj == null)
                    //No record found with the specified id
                    return RedirectToAction("Configure");

                if (model.StoreType == "Other")
                {
                    obj.Id = model.Id;
                    obj.StoreNumber = model.StoreNumber;
                    obj.StoreName = model.StoreName;
                    obj.StoreType = model.StoreType;
                    obj.Address = model.Address;
                    obj.City = model.City;
                    obj.CountryCode = model.CountryCode;
                    obj.PhoneNumber = model.PhoneNumber;
                    obj.Region = model.Region;
                    obj.PostalCode = model.PostalCode;
                    obj.Latitude = model.Latitude;
                    obj.Longitude = model.Longitude;
                    obj.DistanceFromAddress = 0;
                }
                else
                {
                    obj.Id = model.Id;
                    obj.StoreNumber = model.StoreNumber;
                    obj.StoreName = null;
                    obj.StoreType = model.StoreType;
                    obj.Address = model.Address;
                    obj.City = model.City;
                    obj.CountryCode = model.CountryCode;
                    obj.PhoneNumber = model.PhoneNumber;
                    obj.Region = model.Region;
                    obj.PostalCode = model.PostalCode;
                    obj.Latitude = model.Latitude;
                    obj.Longitude = model.Longitude;
                    obj.DistanceFromAddress = 0;
                }

                // If the Longitude and Latitude are undefined
                if (obj.Latitude == 0 && obj.Longitude == 0)
                {
                    // Get Longitude and Latitude from Google
                    string strFullAddress = obj.Address + ";" + obj.City + ", " + obj.Region + " " + obj.PostalCode;
                    var oGoogleResults = GetGeocodingSearchResults(strFullAddress.Trim());
                    var status = oGoogleResults.Element("status").Value;
                    if (status == "OK")
                    {
                        obj.Latitude = Convert.ToDecimal(oGoogleResults.Element("result").Element("geometry").Element("location").Element("lat").Value);
                        obj.Longitude = Convert.ToDecimal(oGoogleResults.Element("result").Element("geometry").Element("location").Element("lng").Value);
                    }
                }

                _StorelocatorService.UpdateStoreLocation(obj);
                //UpdateLocales(obj, model);
                ViewBag.RefreshPage = true;
                ViewBag.btnId = btnId;
                ViewBag.formId = formId;
            }
            model.AvailableOptions.Add(new SelectListItem() { Text = "Lowe's", Value = "Lowe's" });
            model.AvailableOptions.Add(new SelectListItem() { Text = "Other", Value = "Other" });

            return View("~/Plugins/Widgets.TorchDesign_StoreLocator/Views/TorchDesign_StoreLocator/EditPopup.cshtml", model);
        }

        [HttpPost]
        public ActionResult DeleteStrLocation(int id)
        {
            var deleteFAQ = _StorelocatorService.GetStoreLocationById(id);
            if (deleteFAQ == null)
                throw new ArgumentException("No FAQ found with the specified Id");
            _StorelocatorService.DeleteStoreLocation(deleteFAQ);
            return new NullJsonResult();
        }

        [AdminAuthorize]
         public ActionResult Configure()
        {
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var storelocatoreset = _settingService.LoadSetting<StoreLocatorSettings>(storeScope);
            var model = new ConfigurationModel();
            model.Description = storelocatoreset.Description;
            model.GoogleAPIKey = storelocatoreset.GoogleAPIKey;
            if (storeScope > 0)
            {
                model.Description_Override = _settingService.SettingExists(storelocatoreset, x => x.Description, storeScope);
            }
            return View("~/Plugins/Widgets.TorchDesign_StoreLocator/Views/TorchDesign_StoreLocator/Configure.cshtml", model);

        }

        [HttpPost]
        [AdminAuthorize]
         public ActionResult Configure(ConfigurationModel model)
        {
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var storelocatoreset = _settingService.LoadSetting<StoreLocatorSettings>(storeScope);

            storelocatoreset.Description = model.Description;
            storelocatoreset.GoogleAPIKey = model.GoogleAPIKey;

            if (model.Description_Override || storeScope == 0)
            {
                _settingService.SaveSetting(storelocatoreset, x => x.Description, storeScope, false);
                _settingService.SaveSetting(storelocatoreset, x => x.GoogleAPIKey, storeScope, false);
            }
            else if (storeScope > 0)
            {
                _settingService.DeleteSetting(storelocatoreset, x => x.Description, storeScope);
                _settingService.DeleteSetting(storelocatoreset, x => x.GoogleAPIKey, storeScope);
            }
            _settingService.ClearCache();

            return Configure();
            //return View("~/Plugins/Widgets.TorchDesign_StoreLocator/Views/TorchDesign_StoreLocator/Configure.cshtml", model);
        }

        [HttpPost]
        public ActionResult StoreList(DataSourceRequest command, ConfigurationModel model)
        {
            var Storelocations = _StorelocatorService.GetAllStoreLocations(command.Page - 1, command.PageSize);
            var StoreModel = Storelocations.Select(x =>
            {
                var m = new ConfigurationModel()
                {
                    Id = x.Id,
                    StoreNumber = x.StoreNumber,
                    StoreName = x.StoreName,
                    StoreType = x.StoreType,
                    Address = x.Address,
                    City = x.City,
                    CountryCode = x.CountryCode,
                    PhoneNumber = x.PhoneNumber,
                    Region = x.Region,
                    PostalCode = x.PostalCode,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude

                };
                return m;
            })
              .ToList();
            var gridModel = new DataSourceResult
            {
                Data = StoreModel,
                Total = Storelocations.TotalCount
            };

            return Json(gridModel);
        }
        #endregion


        #region Public Info
        public ActionResult PublicInfo()
        {
          var model = new PublicInfoModel();
            model.resultfound = false;
            model.storefound = true;
            model.getdirecrtion = false;
            //model.radius = "25";
            model.Radiusoption.Add(new SelectListItem() { Text = "5", Value = "5" });
            model.Radiusoption.Add(new SelectListItem() { Text = "10", Value = "10" });
            model.Radiusoption.Add(new SelectListItem() { Text = "25", Value = "25" });
            model.Radiusoption.Add(new SelectListItem() { Text = "50", Value = "50" });
            model.Radiusoption.Add(new SelectListItem() { Text = "100", Value = "100" });
            model.Description = _storelocatorsetting.Description;
            model.SearchStoreOption.Add(new SelectListItem() { Text = "Lowe's Only", Value = "0" });
            model.SearchStoreOption.Add(new SelectListItem() { Text = "All Stores", Value = "1" });

            return View("~/Plugins/Widgets.TorchDesign_StoreLocator/Views/TorchDesign_StoreLocator/PublicInfo.cshtml", model);
        }

        [HttpPost]
       
        public ActionResult PublicInfo(PublicInfoModel model)
        {
            model.Description = _storelocatorsetting.Description;
            if (!String.IsNullOrEmpty(model.SearchContant))
            {
                var results = GoogleMapsAPIHelpersCS.GetGeocodingSearchResults(model.SearchContant.Trim());
                var resultCount = results.Elements("result").Count();

                // How many results did we get back?
                if (resultCount == 0)
                {
                    model.getdirecrtion = false;
                    model.resultfound = false;
                    model.storefound = false;
                    model.Radiusoption.Add(new SelectListItem() { Text = "5", Value = "5" });
                    model.Radiusoption.Add(new SelectListItem() { Text = "10", Value = "10" });
                    model.Radiusoption.Add(new SelectListItem() { Text = "25", Value = "25" });
                    model.Radiusoption.Add(new SelectListItem() { Text = "50", Value = "50" });
                    model.Radiusoption.Add(new SelectListItem() { Text = "100", Value = "100" });

                    model.SearchStoreOption.Add(new SelectListItem() { Text = "Lowe's Only", Value = "0" });
                    model.SearchStoreOption.Add(new SelectListItem() { Text = "All Stores", Value = "1" });

                    //Eep, no results!
                    return View("~/Plugins/Widgets.TorchDesign_StoreLocator/Views/TorchDesign_StoreLocator/PublicInfo.cshtml", model);
                }
                else if (resultCount == 1)
                {
                    //Got back just one result, show the stores that match the address search
                    Session["addurl"] = Server.UrlEncode(results.Element("result").Element("formatted_address").Value);
                    //MapModel mapmodel = new MapModel();
                    //mapmodel.Addressurl = Server.UrlEncode(results.Element("result").Element("formatted_address").Value);
                    //  return View("~/Plugins/Widgets.TorchDesign_StoreLocator/Views/TorchDesign_StoreLocator/MapDisplay.cshtml");

                    string _strAddress = Session["addurl"].ToString();
                    if (!string.IsNullOrEmpty(_strAddress))
                    {
                        IList<PublicInfoModel> Searchresult = new List<PublicInfoModel>();
                        // Get the lat/long info about the address
                        //var results1 = GoogleMapsAPIHelpersCS.GetGeocodingSearchResults(_strAddress);

                        //   StoreLocatorControl.SearchText = _strAddress;

                        // Set the latitude and longtitude parameters based on the address being searched on
                        var lat = results.Element("result").Element("geometry").Element("location").Element("lat").Value;
                        var lng = results.Element("result").Element("geometry").Element("location").Element("lng").Value;
                        decimal range = 0;
                        string selectedstoretype;
                        if (model.SearchStoreOptionId == 0)
                        {
                            selectedstoretype = "Lowe's";
                        }
                        else
                        {
                            selectedstoretype = null;
                        }

                        //decimal radius = 25;
                        //decimal.TryParse(model.radius, out radius);
                        //if (radius <= 0)
                        //{
                        //    range = 25;
                        //}
                        //else
                        //{
                        //    range = radius;
                        //}
                        range = model.radius;
                        decimal latValue = 0;
                        decimal.TryParse(lat, out latValue);
                        decimal lngValue = 0;
                        decimal.TryParse(lng, out lngValue);
                        //decimal milesValue = 0;
                        var stores = GetDistance(latValue, lngValue, range, selectedstoretype);
                        if (stores.Count() > 0)
                        {
                            GetStoreList(model, stores, lat, lng);
                           
                        }
                        else
                        {
                            if (range == 5)
                            {   range=10;
                                var stores1 = GetDistance(latValue, lngValue, range, selectedstoretype);
                                if (stores1.Count() > 0)
                                {
                                    GetStoreList(model, stores1, lat, lng);
                                    model.radius=10;
                                }
                                else
                                {
                                    range = 25;
                                    var stores2 = GetDistance(latValue, lngValue, range, selectedstoretype);
                                    if (stores2.Count() > 0)
                                    {
                                        GetStoreList(model, stores2, lat, lng);
                                         model.radius=25;
                                    }
                                    else 
                                    {
                                        range = 50;
                                        var stores3 = GetDistance(latValue, lngValue, range, selectedstoretype);
                                        if (stores3.Count() > 0)
                                        {
                                            GetStoreList(model, stores3, lat, lng);
                                             model.radius=50;
                                        }
                                        else
                                        {
                                            range = 100;
                                            var stores4 = GetDistance(latValue, lngValue, range, selectedstoretype);
                                            if (stores4.Count() > 0)
                                            {
                                                GetStoreList(model, stores4, lat, lng);
                                                 model.radius=100;
                                            }
                                            else 
                                            {

                                                model.storefound = false;
                                                model.getdirecrtion = false;
                                            }
                                        
                                        }

                                    }

                                }
                            }
                            else if (range == 10)
                            {
                                range = 25;
                                var stores1 = GetDistance(latValue, lngValue, range, selectedstoretype);
                                if (stores1.Count() > 0)
                                {
                                    GetStoreList(model, stores1, lat, lng);
                                     model.radius=25;
                                }
                                else
                                {
                                    range = 50;
                                    var stores2 = GetDistance(latValue, lngValue, range, selectedstoretype);
                                    if (stores2.Count() > 0)
                                    {
                                        GetStoreList(model, stores2, lat, lng);
                                         model.radius=50;
                                    }
                                    else
                                    {
                                        range = 100;
                                        var stores3 = GetDistance(latValue, lngValue, range, selectedstoretype);
                                        if (stores3.Count() > 0)
                                        {
                                            GetStoreList(model, stores3, lat, lng);
                                             model.radius=100;
                                        }
                                         else
                                          {

                                                model.storefound = false;
                                                model.getdirecrtion = false;
                                          }
                                     
                                    }

                                }

                            }
                            else if (range == 25)
                            {
                                range = 50;
                                var stores1 = GetDistance(latValue, lngValue, range, selectedstoretype);
                                if (stores1.Count() > 0)
                                {
                                    GetStoreList(model, stores1, lat, lng);
                                     model.radius=50;
                                }
                                else
                                {
                                    range = 100;
                                    var stores2 = GetDistance(latValue, lngValue, range, selectedstoretype);
                                    if (stores2.Count() > 0)
                                    {
                                        GetStoreList(model, stores2, lat, lng);
                                         model.radius=100;
                                    }
                                    else
                                    {
                                        model.storefound = false;
                                        model.getdirecrtion = false;
                                    }

                                }
                            }
                            else if (range == 50)
                            {
                                range = 100;
                                var stores1 = GetDistance(latValue, lngValue, range, selectedstoretype);
                                if (stores1.Count() > 0)
                                {
                                    GetStoreList(model, stores1, lat, lng);
                                     model.radius=100;
                                }
                                else
                                {
                                        model.storefound = false;
                                        model.getdirecrtion = false;
                                    
                                }
                            }
                            else if (range == 100)
                            {
                                model.storefound = false;
                                model.getdirecrtion = false;
                            }
                        }

                        model.Radiusoption.Add(new SelectListItem() { Text = "5", Value = "5", Selected = model.radius == 5});
                        model.Radiusoption.Add(new SelectListItem() { Text = "10", Value = "10", Selected = model.radius == 10});
                        model.Radiusoption.Add(new SelectListItem() { Text = "25", Value = "25", Selected = model.radius == 25});
                        model.Radiusoption.Add(new SelectListItem() { Text = "50", Value = "50", Selected = model.radius == 50});
                        model.Radiusoption.Add(new SelectListItem() { Text = "100", Value = "100", Selected = model.radius == 100});
                       
                        model.SearchStoreOption.Add(new SelectListItem() { Text = "Lowe's Only", Value = "0" });
                        model.SearchStoreOption.Add(new SelectListItem() { Text = "All Stores", Value = "1" });
                        return View("~/Plugins/Widgets.TorchDesign_StoreLocator/Views/TorchDesign_StoreLocator/PublicInfo.cshtml", model);
                    }

                }
                else
                {
                    //Got back multiple results - We need to ask the user which address they mean to use...
                    var matches = (from result in results.Elements("result")
                                   let formatted_address = result.Element("formatted_address").Value
                                   select formatted_address).ToList();
                    foreach (var item in matches)
                    {
                        model.AvailableLocation.Add(Server.UrlEncode(item));

                    }
                    model.resultfound = false;
                    model.getdirecrtion = false;
                    model.storefound = true;
                    model.Radiusoption.Add(new SelectListItem() { Text = "5", Value = "5" });
                    model.Radiusoption.Add(new SelectListItem() { Text = "10", Value = "10" });
                    model.Radiusoption.Add(new SelectListItem() { Text = "25", Value = "25" });
                    model.Radiusoption.Add(new SelectListItem() { Text = "50", Value = "50" });
                    model.Radiusoption.Add(new SelectListItem() { Text = "100", Value = "100" });

                    model.SearchStoreOption.Add(new SelectListItem() { Text = "Lowe's Only", Value = "0" });
                    model.SearchStoreOption.Add(new SelectListItem() { Text = "All Stores", Value = "1" });
                }
            }
            else 
            {
                model.getdirecrtion = false;
                model.resultfound = false;
                model.storefound = false;
                model.Radiusoption.Add(new SelectListItem() { Text = "5", Value = "5" });
                model.Radiusoption.Add(new SelectListItem() { Text = "10", Value = "10" });
                model.Radiusoption.Add(new SelectListItem() { Text = "25", Value = "25" });
                model.Radiusoption.Add(new SelectListItem() { Text = "50", Value = "50" });
                model.Radiusoption.Add(new SelectListItem() { Text = "100", Value = "100" });

                model.SearchStoreOption.Add(new SelectListItem() { Text = "Lowe's Only", Value = "0" });
                model.SearchStoreOption.Add(new SelectListItem() { Text = "All Stores", Value = "1" });
            }
            return View("~/Plugins/Widgets.TorchDesign_StoreLocator/Views/TorchDesign_StoreLocator/PublicInfo.cshtml", model);
        }

        public ActionResult Getlet(string id, string address)
        {
            int sid=Convert.ToInt32(id);
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var storelocatoreset = _settingService.LoadSetting<StoreLocatorSettings>(storeScope);
            var store = _StorelocatorService.GetStoreLocationById(sid);
            PublicInfoModel model = new PublicInfoModel();
            model.sourcestoreaddress = address;
            model.GoogleMapAPIKey = storelocatoreset.GoogleAPIKey;
            model.destinationstoreaddress =store.Address+","+store.City+"-"+store.PostalCode;
            model.getdirecrtion = true;
            model.resultfound = false;
            model.storefound = false;
            return View("~/Plugins/Widgets.TorchDesign_StoreLocator/Views/TorchDesign_StoreLocator/Getlet.cshtml", model);
        }


        public ActionResult GetModifiedImage(int imageid)
        {
            Image image = Image.FromFile(Path.Combine(Server.MapPath("/Plugins/Widgets.TorchDesign_StoreLocator/Content/img/"), "Location_marker.png"));
         
            using (Graphics g = Graphics.FromImage(image))
            {
                // do something with the Graphics (eg. write "Hello World!")
                string text = imageid.ToString();

                // Create font and brush.
                Font drawFont = new Font("helvetica", 19, FontStyle.Bold);
                SolidBrush drawBrush = new SolidBrush(Color.White);

                // Create point for upper-left corner of drawing.
                PointF stringPoint = new PointF(19, 3);

                g.DrawString(text, drawFont, drawBrush, stringPoint);
            }

            MemoryStream ms = new MemoryStream();

            image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            return File(ms.ToArray(), "Location_marker.png");
        }

        public ActionResult GetModifiedImageRed(int imageid)
        {
            Image image = Image.FromFile(Path.Combine(Server.MapPath("/Plugins/Widgets.TorchDesign_StoreLocator/Content/img/"), "red.png"));

            using (Graphics g = Graphics.FromImage(image))
            {
                // do something with the Graphics (eg. write "Hello World!")
                string text = imageid.ToString();

                // Create font and brush.
                Font drawFont = new Font("helvetica", 19, FontStyle.Bold);
                SolidBrush drawBrush = new SolidBrush(Color.Black);

                // Create point for upper-left corner of drawing.
                PointF stringPoint = new PointF(7, 5);

                g.DrawString(text, drawFont, drawBrush, stringPoint);
            }

            MemoryStream ms = new MemoryStream();

            image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            return File(ms.ToArray(), "Location_marker.png");
        }

        public IList<StoreLocator> GetDistance(decimal lati,decimal longi, decimal radius,string storetypes)
        {

            var longitude = _dataProvider.GetParameter();
            longitude.ParameterName = "Longitude";
            longitude.Value = longi;
            longitude.DbType = DbType.Decimal;

            var latitude = _dataProvider.GetParameter();
            latitude.ParameterName = "Latitude";
            latitude.Value = lati;
            latitude.DbType = DbType.Decimal;

            var miles = _dataProvider.GetParameter();
            miles.ParameterName = "Miles";
            miles.Value = radius;
            miles.DbType = DbType.Decimal;

            var storetype = _dataProvider.GetParameter();
            storetype.ParameterName = "StoreType";
            storetype.Value = storetypes;
            storetype.DbType = DbType.String;

            var stores = _storeLocatorContext.ExecuteStoredProcedureList<StoreLocator>("GetStoresByLatLanWithMiles", longitude, latitude, miles, storetype);

            return stores;
        }
        public void GetStoreList(PublicInfoModel model, IList<StoreLocator> stores,string lat,string lng)
        {
            model.storefound = true;

            var locations = new List<string>();
            var infoWindowContents = new List<string>();
            var count = 1;

            foreach (var item in stores)
            {
                model.Availablestores.Add(new Models.Availablestores()
                {
                    Id = item.Id,
                    StoreNumber=item.StoreNumber,
                    StoreName = item.StoreName,
                    Address = item.Address,
                    City = item.City,
                    Region = item.Region,
                    CountryCode = item.CountryCode,
                    PostalCode = item.PostalCode,
                    PhoneNumber = item.PhoneNumber,
                    StoreType = item.StoreType,
                    Latitude = item.Latitude,
                    Longitude = item.Longitude,
                    DistanceFromAddress = Math.Round(item.DistanceFromAddress, 1, MidpointRounding.AwayFromZero)
                });

                var imageColorCallingaction = "GetModifiedImage";
                if (item.StoreType == "Other")
                {
                    imageColorCallingaction = "GetModifiedImageRed";
                }

                locations.Add(string.Format(
                  @"{{ 
                                    title: ""Store #{0}"", 
                                    position: new google.maps.LatLng({1}, {2}),
                                    icon: ""{3}/{4}""
                                }}",
                     item.Id,
                     item.Latitude,
                     item.Longitude,
                     imageColorCallingaction,
                     count
                  )
                );
                infoWindowContents.Add(string.Format(
                  @"{{ 
                                    content: ""<div class=\""infoWindow\""><b>Store #{0}</b><br />{1}<br />{2}, {3} {4}</div>""
                                }}",
                     item.StoreNumber,
                     item.Address,
                     item.City,
                     item.Region,
                     item.PostalCode
                  )
                );
                count++;

            }
            model.resultfound = true;
            var locationsJson = "[" + string.Join(",", locations.ToArray()) + "]";
            var overlayContentsJson = "[" + string.Join(",", infoWindowContents.ToArray()) + "]";

            model.firstLatitude = lat;
            model.firstLongitude = lng;
            model.locationsjson = locationsJson;
            model.infoWindowContentsjson = overlayContentsJson;

            model.getdirecrtion = false;
        }
        #endregion
    }
}
