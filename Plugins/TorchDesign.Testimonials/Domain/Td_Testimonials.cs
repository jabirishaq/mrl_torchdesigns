using Nop.Core;
using Nop.Core.Domain.Localization;
using System;
using System.Collections.Generic;

namespace Nop.Plugin.TorchDesign.Testimonials.Domain
{
    /// <summary>
    /// Represents a Testimonials record
    /// </summary>
    public partial class Td_Testimonials : BaseEntity, ILocalizedEntity
    {

        //private ICollection<Testimonials_Category_Mapping> Testimonialscategorysmap;

        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
        public string Message { get; set; }
    
        public bool IsPublished { get; set; }
        public bool IsRejected { get; set; }
        public DateTime CreateOn { get; set; }

       
        //public virtual ICollection<Testimonials_Category_Mapping> TestimonialsMapping
        //{
        //    get { return Testimonialscategorysmap ?? (Testimonialscategorysmap = new List<Testimonials_Category_Mapping>()); }
        //    protected set { Testimonialscategorysmap = value; }
        //}
    }
}
