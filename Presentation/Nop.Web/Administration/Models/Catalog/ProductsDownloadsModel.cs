using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Models.Catalog
{
    public partial class ProductsDownloadsModel : BaseNopModel
    {
        public int Id { get; set; }
        [NopResourceDisplayName("admin.download.uploadfile")]
        [UIHint("Download")]
        public int DownloadsId { get; set; }

       [NopResourceDisplayName("Admin.Catalog.Products.Pictures.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

         [NopResourceDisplayName("admin.catalog.productreviews.fields.title")]
        public string DownloadTitle { get; set; }
         [NopResourceDisplayName("admin.catalog.categories.fields.description")]
         public string DownloadDescription { get; set; }

         //public int ClonePictureid { get; set; }
    }
}