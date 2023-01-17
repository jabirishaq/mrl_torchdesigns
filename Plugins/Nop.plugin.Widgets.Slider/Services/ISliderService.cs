using Nop.Core;
using Nop.Plugin.Widgets.Slider.Domain;
using System;
using System.Collections.Generic;

namespace Nop.Plugin.Widgets.Slider.Services
{
    public partial interface ISliderService
    {
        SliderRecord GetSliderById(int id);

        void InsertSlider(SliderRecord record);

        void UpdateSlider(SliderRecord record);

        void DeleteSlider(SliderRecord record);

        IPagedList<SliderRecord> GetAllSliders(int pageIndex = 0, int pageSize = int.MaxValue);

        IList<SliderRecord> GetAllActiveSliders();

        //IList<SliderRecord> GetHomepageEvents(int Quntityonhomepage);
    }
}
