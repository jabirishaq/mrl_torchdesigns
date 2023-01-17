using System;
using Nop.Web.Framework;
using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Localization;
using System.Web.Mvc;
using System.Collections.Generic;

namespace Nop.Plugin.TorchDesign.FAQ.Models
{
    public partial class CategoriesModel : BaseNopEntityModel
    {
        public CategoriesModel()
        {

            AvailableQandAByCategory = new List<QandAModel>();

        }

        public IList<QandAModel> AvailableQandAByCategory { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.FAQ.Category")]
        public int CategoryId { get; set; }
       

        [NopResourceDisplayName("Plugins.Widgets.FAQ.Category")]
        [AllowHtml]
        public string CategoryName { get; set; }

        public bool Active { get; set; }

        public int DisplayOrder { get; set; }  

    }
}

