using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Plugin.TorchDesign.Testimonials.Domain;

namespace Nop.Plugin.TorchDesign.Testimonials.Services
{
    public partial class Testimonialservice : ITestimonialservice
    {
         private readonly IRepository<Td_Testimonials> _TestimonialsRepository;
      
         private readonly IRepository<Td_Testimonials_Pictures> _TestimonialsPicturesRepository;


         public Testimonialservice(IRepository<Td_Testimonials> TestimonialsRepository, IRepository<Td_Testimonials_Pictures> TestimonialsPicturesRepository)
        {
            this._TestimonialsRepository = TestimonialsRepository;
            this._TestimonialsPicturesRepository = TestimonialsPicturesRepository;
           
        }

        public Td_Testimonials GetTestimonialsById(int id)
        {
            if (id == 0)
                return null;
            return _TestimonialsRepository.GetById(id);
        }

        public IList<Td_Testimonials_Pictures> GetAllTestimonialsPictures()
        {
            var query = (from p in _TestimonialsPicturesRepository.Table
                        join t in _TestimonialsRepository.Table on p.TestimonialsId equals t.Id
                        where t.IsPublished
                         orderby p.Id
                         select p).ToList();

            return query;

        }

        public void InsertTestimonials(Td_Testimonials record)
        {
            if (record == null)
                throw new ArgumentNullException("record");

            _TestimonialsRepository.Insert(record);
        }

        public void UpdateTestimonials(Td_Testimonials record)
        {
            if (record == null)
                throw new ArgumentNullException("record");

            _TestimonialsRepository.Update(record);
        }

        public void DeleteTestimonials(Td_Testimonials record)
        {
            if (record == null)
                throw new ArgumentNullException("record");
            _TestimonialsRepository.Delete(record);
        }

        public IPagedList<Td_Testimonials> GetAllTestimonials(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = from a in _TestimonialsRepository.Table
                        orderby a.CreateOn descending
                        select a;
            var records = new PagedList<Td_Testimonials>(query, pageIndex, pageSize);
            return records;
        }

        public IPagedList<Td_Testimonials> GetAllActiveTestimonials(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = from a in _TestimonialsRepository.Table
                        where a.IsPublished
                        orderby a.CreateOn descending
                        select a;
            var records = new PagedList<Td_Testimonials>(query, pageIndex, pageSize);
            return records;
        }

        public IPagedList<Td_Testimonials> GetAllRejectedTestimonials(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = from a in _TestimonialsRepository.Table
                        where a.IsRejected
                        orderby a.CreateOn descending
                        select a;
            var records = new PagedList<Td_Testimonials>(query, pageIndex, pageSize);
            return records;
        }
        public IPagedList<Td_Testimonials> GetAllDeactiveTestimonials(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = from a in _TestimonialsRepository.Table
                        where (a.IsPublished==false && a.IsRejected==false)
                        orderby a.CreateOn descending
                        select a;
            var records = new PagedList<Td_Testimonials>(query, pageIndex, pageSize);
            return records;
        }

        public void InsertTestimonialsPicture(Td_Testimonials_Pictures record)
        {
            if (record == null)
                throw new ArgumentNullException("record");

            _TestimonialsPicturesRepository.Insert(record);
        }

        public void DeleteTestimonialsPicture(Td_Testimonials_Pictures record)
        {
            if (record == null)
                throw new ArgumentNullException("record");

            _TestimonialsPicturesRepository.Delete(record);
        }

        public IList<Td_Testimonials_Pictures> GetAllTestimonialsPicturesByTestimonialId(int TestimonialId)
        {
            var query = from a in _TestimonialsPicturesRepository.Table
                        where a.TestimonialsId==TestimonialId
                        select a;
          
            return  query.ToList();
        }

        
            }
}
