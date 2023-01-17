using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Nop.Web.Framework.Localization;
using System.Collections.Generic;

namespace Nop.Plugin.TorchDesign.Testimonials.Models
{
    public class TestimonialsModel : BaseNopEntityModel 
    {
        public TestimonialsModel()
        {
            Picturelist = new List<TestimonialsPicture>();
        }
       
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
        public string Message { get; set; }
        public bool IsPublished { get; set; }
        public DateTime CreateOn { get; set; }
        public bool Appprovebymail { get; set; }
        public string ApproveValue { get; set; }
        public IList<TestimonialsPicture> Picturelist { get; set; }
        public int filterby { get; set; }
        public bool IsRejected { get; set; }
        public string DifaultPicture { get; set; }


        //public virtual ICollection<Testimonials_Category_Mapping> TestimonialsMapping
        //{
        //    get { return Testimonialscategorysmap ?? (Testimonialscategorysmap = new List<Testimonials_Category_Mapping>()); }
        //    protected set { Testimonialscategorysmap = value; }
        //}
    }

    public class TestimonialsPicture : BaseNopModel
    {
        public int Id { get; set; }
        [UIHint("MultiPicture")]
        public int PictureId { get; set; }
        public string PictureUrl { get; set; }
        public string FullSizePicture { get; set; }

    }
}
