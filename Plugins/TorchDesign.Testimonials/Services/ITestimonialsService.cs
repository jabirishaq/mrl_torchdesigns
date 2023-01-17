using Nop.Core;
using Nop.Plugin.TorchDesign.Testimonials.Domain;
using System.Collections.Generic;

namespace Nop.Plugin.TorchDesign.Testimonials.Services
{
    public partial interface ITestimonialservice
    {

        Td_Testimonials GetTestimonialsById(int id);

        void InsertTestimonials(Td_Testimonials record);

        void UpdateTestimonials(Td_Testimonials record);

        void DeleteTestimonials(Td_Testimonials record);
        
        IPagedList<Td_Testimonials> GetAllTestimonials(int pageIndex = 0, int pageSize = int.MaxValue);
        IPagedList<Td_Testimonials> GetAllActiveTestimonials(int pageIndex = 0, int pageSize = int.MaxValue);
        IPagedList<Td_Testimonials> GetAllRejectedTestimonials(int pageIndex = 0, int pageSize = int.MaxValue);
        IPagedList<Td_Testimonials> GetAllDeactiveTestimonials(int pageIndex = 0, int pageSize = int.MaxValue);
        IList<Td_Testimonials_Pictures> GetAllTestimonialsPictures();
        IList<Td_Testimonials_Pictures> GetAllTestimonialsPicturesByTestimonialId(int TestimonialId);
        void InsertTestimonialsPicture(Td_Testimonials_Pictures record);
        void DeleteTestimonialsPicture(Td_Testimonials_Pictures record);

        
    }
}
