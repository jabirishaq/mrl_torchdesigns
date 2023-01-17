using System.Collections.Generic;
using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Admin.Validators.Catalog;
using Nop.Web.Framework;
using Nop.Web.Framework.Localization;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Models.Catalog
{
    public partial class SpecificationGroupModel : BaseNopEntityModel
    {
        public string Name { get; set; }

        public int DisplayOrder { get; set; }
    }

   }