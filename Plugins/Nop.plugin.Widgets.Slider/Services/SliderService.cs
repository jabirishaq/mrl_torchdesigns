using Nop.Core;
using Nop.Core.Data;
using Nop.Plugin.Widgets.Slider.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.Slider.Services
{
    public partial class SliderService : ISliderService
    {
        private readonly IRepository<SliderRecord> _sliderRepository;

        public SliderService(IRepository<SliderRecord> sliderRepository)
        {
            this._sliderRepository = sliderRepository;
        }

        public SliderRecord GetSliderById(int id)
        {
            if (id == 0)
                return null;
            return _sliderRepository.GetById(id);
        }

        public void InsertSlider(SliderRecord record)
        {
            if (record == null)
                throw new ArgumentNullException("record");

            _sliderRepository.Insert(record);
        }

        public void UpdateSlider(SliderRecord record)
        {
            if (record == null)
                throw new ArgumentNullException("record");

            _sliderRepository.Update(record);
        }

        public void DeleteSlider(SliderRecord record)
        {
            if (record == null)
                throw new ArgumentNullException("record");
            _sliderRepository.Delete(record);
        }

        public IPagedList<SliderRecord> GetAllSliders(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = from sliders in _sliderRepository.Table
                        orderby sliders.Id
                        select sliders;
            var records = new PagedList<SliderRecord>(query, pageIndex, pageSize);
            return records;
        }

        public virtual IList<SliderRecord> GetAllActiveSliders()
        {
            var query = from sliders in _sliderRepository.Table
                        where sliders.Published orderby sliders.DisplayOrder
                        select sliders;

            return query.ToList();
        }

        //public virtual IList<SliderRecord> GetHomepageEvents(int EventQuntityOnHomepage)
        //{
        //    var query = (from events in _sliderRepository.Table orderby events.StartDateUtc
        //                where events.EndDateUtc >= DateTime.Now && events.Published
        //                select events).Take(EventQuntityOnHomepage);

        //    return query.ToList();
        //}
    }
}
