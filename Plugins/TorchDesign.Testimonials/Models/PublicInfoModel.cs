using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Nop.Web.Framework.Localization;
using System.Collections.Generic;
using FluentValidation.Attributes;


namespace Nop.Plugin.TorchDesign.Testimonials.Models
{
    public class PublicInfoModel : BaseNopEntityModel 
    {
        public TestimonialListModel testiMonialListModel { get; set; }
        public AddTestiMonialModel addTestiMonialModel { get; set; }

        //public virtual ICollection<Testimonials_Category_Mapping> TestimonialsMapping
        //{
        //    get { return Testimonialscategorysmap ?? (Testimonialscategorysmap = new List<Testimonials_Category_Mapping>()); }
        //    protected set { Testimonialscategorysmap = value; }
        //}
    }

    public class TestimonialListModel : BaseNopModel
    {
        public TestimonialListModel()
        {
            AvailableTestomonials = new List<TestimonialsList>();
        }

        public int pageindex { get; set; }
        public IList<TestimonialsList> AvailableTestomonials { get; set; }
    }

    [Validator(typeof(PublicInfoValidator))]
    public class AddTestiMonialModel : BaseNopModel
    {
        [NopResourceDisplayName("plugins.torchdesign.testimonial.customername")]
        public string CustomerName { get; set; }
        [NopResourceDisplayName("plugins.torchdesign.testimonial.email_address")]
        public string Email { get; set; }
        [NopResourceDisplayName("plugins.torchdesign.testimonial.location")]
        [StringLength(50, ErrorMessage = "Maximum 50 characters allowed")]
        public string Location { get; set; }
        [NopResourceDisplayName("plugins.torchdesign.testimonial.from_message")]
        public string Message { get; set; }
        public bool IsPublished { get; set; }
        public DateTime CreateOn { get; set; }
        [UIHint("MultiPicture")]
        public int PictureId { get; set; }
        public bool IsInserted { get; set; }
    }

    public class TestimonialsList : BaseNopModel
    {
        public TestimonialsList()
        {
            PictureList = new List<PictureList>();
          
        }
        public int tId { get; set; }
        public string CustomerName { get; set; }
        public string Location { get; set; }
        public string Message { get; set; }
        public IList<PictureList> PictureList { get; set; }
        public string DifaultPicture { get; set; }

    }

    public class PictureList : BaseNopModel
    {
        public int PictureId { get; set; }

        public string PictureUrl { get; set; }

        public string FullSizeImageUrl { get; set; }

    }
}
