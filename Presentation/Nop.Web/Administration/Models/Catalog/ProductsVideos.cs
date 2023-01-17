using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Models.Catalog
{
    public partial class ProductsVideos : BaseNopModel
    {
        public int Id { get; set; }
        [UIHint("Picture")]
        [NopResourceDisplayName("Admin.Catalog.Products.Pictures.Fields.Picture")]
        public int PictureId { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Products.Pictures.Fields.PictureUrl")]
        public string Pictureurl { get; set; }

         [NopResourceDisplayName("Admin.Catalog.Products.Pictures.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

         [Required]
         [NopResourceDisplayName("Admin.Catalog.Products.Video.Fields.VideoId")]
         [StringLength(50, ErrorMessage = "Maximum 50 characters allowed")]
         public string VideoUrl { get; set; }

         [NopResourceDisplayName("Admin.Catalog.Products.Video.Fields.Tagname")]
         public string Tags { get; set; }

         [Required]
         [NopResourceDisplayName("admin.catalog.productreviews.fields.title")]
         [StringLength(50, ErrorMessage = "Maximum 50 characters allowed")]
         public string VideoTitle { get; set; }

         [NopResourceDisplayName("admin.catalog.categories.fields.description")]
         public string VideoDescription { get; set; }
         public int ClonePictureid { get; set; }
    }
}