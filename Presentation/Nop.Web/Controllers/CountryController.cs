using System;
using System.Linq;
using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Web.Infrastructure.Cache;
using System.Collections.Generic;
using Nop.Core.Domain.Common;

namespace Nop.Web.Controllers
{
    public partial class CountryController : BasePublicController
    {
        #region Fields

        private readonly ICountryService _countryService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        private readonly ICacheManager _cacheManager;
        private readonly CommonSettings _commonSettings;
        #endregion

        #region Constructors

        public CountryController(ICountryService countryService,
            IStateProvinceService stateProvinceService,
            ILocalizationService localizationService,
            IWorkContext workContext,
            ICacheManager cacheManager, CommonSettings commonSettings)
        {
            this._countryService = countryService;
            this._stateProvinceService = stateProvinceService;
            this._localizationService = localizationService;
            this._workContext = workContext;
            this._cacheManager = cacheManager;
            this._commonSettings = commonSettings;
        }

        #endregion

        #region States / provinces

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetStatesByCountryId(string countryId, bool addEmptyStateIfRequired)
        {
            //this action method gets called via an ajax request
            if (String.IsNullOrEmpty(countryId))
                throw new ArgumentNullException("countryId");

            string cacheKey = string.Format(ModelCacheEventConsumer.STATEPROVINCES_BY_COUNTRY_MODEL_KEY, countryId, addEmptyStateIfRequired, _workContext.WorkingLanguage.Id);
            var cacheModel = _cacheManager.Get(cacheKey, () =>
            {
                var country = _countryService.GetCountryById(Convert.ToInt32(countryId));
                var states = _stateProvinceService.GetStateProvincesByCountryId(country != null ? country.Id : 0).ToList();
                var result = (from s in states
                              select new { id = s.Id, name = s.GetLocalized(x => x.Name) })
                              .ToList();

                if (addEmptyStateIfRequired && result.Count == 0)
                    result.Insert(0, new { id = 0, name = _localizationService.GetResource("Address.OtherNonUS") });
                return result;

            });

            return Json(cacheModel, JsonRequestBehavior.AllowGet);
        }





        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetStatesByCountryIdCustome(string countryId, bool addEmptyStateIfRequired)
        {
            //this action method gets called via an ajax request
            if (String.IsNullOrEmpty(countryId))
                throw new ArgumentNullException("countryId");

            string cacheKey = string.Format(ModelCacheEventConsumer.STATEPROVINCES_BY_COUNTRY_MODEL_KEY, countryId, addEmptyStateIfRequired, _workContext.WorkingLanguage.Id);
            var cacheModel = _cacheManager.Get(cacheKey, () =>
            {

                if (countryId == int.MaxValue.ToString())
                {
                    
                    var allStates = _stateProvinceService.GetAllStateProvincesOfUSA();

                    var result = (from s in allStates
                                  select new { id = s.Id, name = s.GetLocalized(x => x.Name) })
                              .ToList();

                    if (addEmptyStateIfRequired && result.Count == 0)
                        result.Insert(0, new { id = 0, name = _localizationService.GetResource("Address.OtherNonUS") });

                    result.Insert(0, new { id = 0, name = _localizationService.GetResource("Address.SelectState") });
                    return result;
                }
                else
                {
                    var country = _countryService.GetCountryById(Convert.ToInt32(countryId));
                    if (country != null)
                    {
                        var states = _stateProvinceService.GetStateProvincesByCountryId(country != null ? country.Id : 0).ToList();
                        var result = (from s in states
                                      select new { id = s.Id, name = s.GetLocalized(x => x.Name) })
                                 .ToList();

                        if (addEmptyStateIfRequired && result.Count == 0)
                            result.Insert(0, new { id = 0, name = _localizationService.GetResource("Address.OtherNonUS") });

                        result.Insert(0, new { id = 0, name = _localizationService.GetResource("Address.SelectState") });
                        return result;
                    }
                    else { return null; }

                }



            });

            return Json(cacheModel, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
