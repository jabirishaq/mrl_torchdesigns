using System;
using Nop.Web.Framework;
using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Localization;
using System.Web.Mvc;
using System.Collections.Generic;

namespace Nop.Plugin.TorchDesign.FAQ.Models
{
    public partial class FAQ_Model : BaseNopEntityModel
    {
        public FAQ_Model()
        {

            AllCategories = new List<CategoriesModel>();
            RemainedQandA = new List<QandAModel>();

        }

       
        public string Title { get; set; }
        public bool Title_OverrideForStore { get; set; }

        
        public int DisplayOption { get; set; }
        public bool DisplayOption_OverrideForStore { get; set; }
        
        public IList<CategoriesModel> AllCategories { get; set; }

        public IList<QandAModel> RemainedQandA { get; set; }

       
    }
}

