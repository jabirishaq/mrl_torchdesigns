using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Nop.Services.Topics;
using Nop.Web.Models.SystemDesignHelp;
using Nop.Core.Domain.SystemDesignHelp;
using Nop.Services.Localization;
using Nop.Services.Directory;
using Nop.Core;
using Nop.Core.Domain.Localization;
using Nop.Services.Messages;
using Nop.Services.Media;
using System.IO;
using System.Web;
using Nop.Core.Domain.Directory;
using Nop.Web.Framework.UI.Captcha;

namespace Nop.Web.Controllers
{
    public partial class SystemDesignHelpController : BasePublicController
    {
        #region Fields

        private readonly ITopicService _topicService;
        private readonly IPictureService _pictureservice;
        private readonly ILanguageService _languageService;
        private readonly ICurrencyService _currencyService;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IWebHelper _webHelper;
        private readonly LocalizationSettings _localizationSettings;
        private readonly IWorkflowMessageService _workflowmessageservice;
        private readonly IDownloadService _downloadServices;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly CaptchaSettings _captchaSettings;


        #endregion

        #region Constructors

        public SystemDesignHelpController(IPictureService pictureservice,
            ITopicService topicService,
            ILanguageService languageService,
            ICurrencyService currencyService,
            ILocalizationService localizationService,
            IWorkContext workContext,
            IStoreContext storeContext,
            IWebHelper webHelper,
            LocalizationSettings localizationSettings,
            IWorkflowMessageService workflowmessageservice,
            IDownloadService downloadServices,
            IStateProvinceService stateProvinceService,
            CaptchaSettings captchaSettings)
        {
            this._pictureservice = pictureservice;
            this._topicService = topicService;
            this._languageService = languageService;
            this._currencyService = currencyService;
            this._localizationService = localizationService;
            this._workContext = workContext;
            this._storeContext = storeContext;
            this._localizationSettings = localizationSettings;
            this._workflowmessageservice = workflowmessageservice;
            this._downloadServices = downloadServices;
            this._webHelper = webHelper;
            this._stateProvinceService = stateProvinceService;
            this._captchaSettings = captchaSettings; 
        }

        #endregion

        #region Methods

        public ActionResult Step1()
        {
            return View();
        }

        public ActionResult Step2()
        {
            SystemDesignHelpModel model = new SystemDesignHelpModel();

            // Get the USA Continental and USA Other State Lists
            IList<StateProvince> states = _stateProvinceService.GetStateProvincesByCountryId(237,false);
            IList<StateProvince> otherStates = _stateProvinceService.GetStateProvincesByCountryId(600, false);

            // Merge the two lists of states into one.
            foreach(StateProvince state in otherStates)
            {
                states.Add(state);
            }
            
            model.AvailableStates.Add(new SelectListItem() { Text = "Select State", Value = "0" });

            // Sort the list of states
            states = states.OrderBy(r => r.Name).ToList();

            foreach(StateProvince state in states)
            {
                model.AvailableStates.Add(new SelectListItem { Text = state.Name, Value = state.Name });
            }
            //model.AvailableStates.Add(new SelectListItem { Text = "Mid-Atlantic", Value = "Northeast" });
            //model.AvailableStates.Add(new SelectListItem { Text = "Northeast", Value = "Northeast" });
            //model.AvailableStates.Add(new SelectListItem { Text = "Midwest", Value = "Midwest" });
            //model.AvailableStates.Add(new SelectListItem { Text = "Southwest", Value = "Southwest" });
            //model.AvailableStates.Add(new SelectListItem { Text = "West", Value = "West" });


            model.AvailableSoilType.Add(new SelectListItem { Text = "Loam", Value = "Loam" });
            model.AvailableSoilType.Add(new SelectListItem { Text = "Clay", Value = "Clay" });
            model.AvailableSoilType.Add(new SelectListItem { Text = "Sand", Value = "Sand", Selected = true });


            model.AvailableCityWell.Add(new SelectListItem { Text = "City", Value = "City", Selected = true });
            model.AvailableCityWell.Add(new SelectListItem { Text = "Well", Value = "Well" });

            //model.AvailableWaterPressure.Add(new SelectListItem { Text = "Low", Value = "Low", Selected = true });
            //model.AvailableWaterPressure.Add(new SelectListItem { Text = "Medium", Value = "Medium" });
            //model.AvailableWaterPressure.Add(new SelectListItem { Text = "High", Value = "High" });

            model.AvailableHaveFaucets.Add(new SelectListItem { Text = "1", Value = "1", Selected = true });
            model.AvailableHaveFaucets.Add(new SelectListItem { Text = "2", Value = "2" });
            model.AvailableHaveFaucets.Add(new SelectListItem { Text = "3", Value = "3" });
            model.AvailableHaveFaucets.Add(new SelectListItem { Text = "4", Value = "4" });


            model.AvailableUseFaucets.Add(new SelectListItem { Text = "1", Value = "1", Selected = true });
            model.AvailableUseFaucets.Add(new SelectListItem { Text = "2", Value = "2" });
            model.AvailableUseFaucets.Add(new SelectListItem { Text = "3", Value = "3" });
            model.AvailableUseFaucets.Add(new SelectListItem { Text = "4", Value = "4" });

            model.AvailableDrippers.Add(new SelectListItem { Text = "Drippers", Value = "Drippers" });
            model.AvailableDrippers.Add(new SelectListItem { Text = "Micro Sprays", Value = "Micro Sprays" , Selected = true });
            model.AvailableDrippers.Add(new SelectListItem { Text = "Both", Value = "Both" });


            return View(model);
        }

        [HttpPost]
        [CaptchaValidator]
        public ActionResult Step2(SystemDesignHelpModel model, HttpPostedFileBase file, bool captchaValid)
        {
            //validate CAPTCHA
            if (_captchaSettings.Enabled && !captchaValid)
            {
                ModelState.AddModelError("", _captchaSettings.GetWrongCaptchaMessage(_localizationService));
            }

            if (ModelState.IsValid)
            {
                if (file != null)
                {

                    if (file.ContentLength > 0)
                    {
                        SystemDesignHelp smodel = new SystemDesignHelp();
                        smodel.CityWellId = model.CityWellId;
                        smodel.CountryId = model.StateId;
                        smodel.DrippersId = model.DrippersId;
                        smodel.Email = model.Email;
                        smodel.FirstName = (!String.IsNullOrEmpty(model.FirstName)) ? (model.FirstName) : "";
                        smodel.HaveFaucetsId = model.HaveFaucetsId;
                        smodel.LastName = (!String.IsNullOrEmpty(model.LastName)) ? (model.LastName) : "";
                        //smodel.PictureUrl = _pictureservice.GetPictureUrl(model.PictureId);
                        //var Download = _downloadServices.GetDownloadById(model.PictureId);
                        //string Downloadurl = "";
                        //string storeUrl = _webHelper.GetStoreLocation();
                        //if (Download.DownloadGuid != null)
                        //{
                        //    Downloadurl = string.Format("{0}download/ShowImage/?downloadGuid={1}", storeUrl, Download.DownloadGuid);
                        //}

                        //smodel.FirstName = fi.FileName;
                        //smodel.filestream = model.PictureId.InputStream;
                        smodel.PSI = model.PSI != null ? model.PSI : "";
                        smodel.Second = model.Second != null ? model.Second : "";
                        smodel.SoilTypeId = model.SoilTypeId;
                        smodel.UseFaucetsId = model.UseFaucetsId;


                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/Content/images/SystemHelpAttachments/"), fileName);
                        file.SaveAs(path);
                        smodel.PictureUrl = path;

                        _workflowmessageservice.SendSystemDesignHelpNotification(smodel, _localizationSettings.DefaultAdminLanguageId);
                        return RedirectToAction("Step3");
                    }
                    else
                    {
                        return RedirectToAction("Step2");
                    }
                }

                return RedirectToAction("Step2");
            }
            return View(model);
        }

        public ActionResult Step3()
        {
            return View();
        }

        #endregion
    }
}
