using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Plugin.TorchDesign.FAQ.Domain;

namespace Nop.Plugin.TorchDesign.FAQ.Services
{
    public partial class FrequentlyAskedQService : IFrequentlyAskedQService
    {
         private readonly IRepository<FrequentlyAskedQ> _FAQRepository;
         private readonly IRepository<FAQ_Category> _FAQCategoryRepository;
         private readonly IRepository<FAQ_Category_Mapping> _FaqCatMapping;


         public FrequentlyAskedQService(IRepository<FrequentlyAskedQ> FAQRepository, IRepository<FAQ_Category> FAQCategoryRepository, IRepository<FAQ_Category_Mapping> FaqCatMapping)
        {
            this._FAQRepository = FAQRepository;
            this._FAQCategoryRepository = FAQCategoryRepository;
            this._FaqCatMapping = FaqCatMapping;
        }

        public FrequentlyAskedQ GetFAQById(int id)
        {
            if (id == 0)
                return null;
            return _FAQRepository.GetById(id);
        }

        public IList<FrequentlyAskedQ> GetFAQByCategoryId(int id)
        {
           var query = from p in _FAQRepository.Table
                    from pc in p.FAqMapping.Where(pc => id.Equals(pc.CategoryId))
                    select p;


            //var query = from a in _FAQRepository.Table
            //            join b in _FaqCatMapping.Table on a.Id equals b.FaqId 
            //            where b.Id==id 
            //            orderby a.DisplayOrder 
            //            select a;
                          
            var results = query.ToList();
            return results;
            
        }

        public void InsertFAQ(FrequentlyAskedQ record)
        {
            if (record == null)
                throw new ArgumentNullException("record");

            _FAQRepository.Insert(record);
        }

        public void UpdateFAQ(FrequentlyAskedQ record)
        {
            if (record == null)
                throw new ArgumentNullException("record");

            _FAQRepository.Update(record);
        }

        public void DeleteFAQ(FrequentlyAskedQ record)
        {
            if (record == null)
                throw new ArgumentNullException("record");
            _FAQRepository.Delete(record);
        }

        public IPagedList<FrequentlyAskedQ> GetAllFAQs(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = from a in _FAQRepository.Table
                        orderby a.DisplayOrder
                        select a;
            var records = new PagedList<FrequentlyAskedQ>(query, pageIndex, pageSize);
            return records;
        }

        public IPagedList<FrequentlyAskedQ> GetAllActiveFAQs(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = from a in _FAQRepository.Table
                        where a.IsActive
                        orderby a.DisplayOrder
                        select a;
            var records = new PagedList<FrequentlyAskedQ>(query, pageIndex, pageSize);
            return records;
        }



        public FAQ_Category GetFAQCategoryById(int id)
        {
            if (id == 0)
                return null;
            return _FAQCategoryRepository.GetById(id);
        }

        public void InsertFAQCategory(FAQ_Category record)
        {
            if (record == null)
                throw new ArgumentNullException("record");

            _FAQCategoryRepository.Insert(record);
        }

        public void UpdateFAQCategory(FAQ_Category record)
        {
            if (record == null)
                throw new ArgumentNullException("record");

            _FAQCategoryRepository.Update(record);
        }

        public void DeleteFAQCategory(FAQ_Category record)
        {
            if (record == null)
                throw new ArgumentNullException("record");
            _FAQCategoryRepository.Delete(record);
        }

        public IPagedList<FAQ_Category> GetAllFAQCategory(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = from a in _FAQCategoryRepository.Table
                        orderby a.DisplayOrder
                        select a;
            var records = new PagedList<FAQ_Category>(query, pageIndex, pageSize);
            return records;
        }




        public FAQ_Category_Mapping GetFAQCategoryMappingById(int id)
        {
            if (id == 0)
                return null;
            return _FaqCatMapping.GetById(id);
        }

        public void InsertFAQCategoryMapping(FAQ_Category_Mapping record)
        {
            if (record == null)
                throw new ArgumentNullException("record");

            _FaqCatMapping.Insert(record);
        }

        public void UpdateFAQCategoryMapping(FAQ_Category_Mapping record)
        {
            if (record == null)
                throw new ArgumentNullException("record");

            _FaqCatMapping.Update(record);
        }

        public void DeleteFAQCategoryMapping(FAQ_Category_Mapping record)
        {
            if (record == null)
                throw new ArgumentNullException("record");
            _FaqCatMapping.Delete(record);
        }

        public IList<FAQ_Category_Mapping> GetAllFAQCategoryMappingByFaqId(int faqid)
        {
            var query = from a in _FaqCatMapping.Table where a.FaqId == faqid
                        select a;
            var records = query.ToList();
            return records;
        }

        public IList<FrequentlyAskedQ> GetAllFAQWithoutCategotyMapping()
        {
            var query = _FAQRepository.Table.Where(a => !_FaqCatMapping.Table.Select(b => b.FaqId).Contains(a.Id)).ToList();
            return query;
        }


        public IPagedList<FAQ_Category_Mapping> GetAllFAQCategoryMapping(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = from a in _FaqCatMapping.Table
                        orderby a.DisplayOrder
                        select a;
            var records = new PagedList<FAQ_Category_Mapping>(query, pageIndex, pageSize);
            return records;
        }

        public bool CategoryExistOrNot(int categoryid,int faqid)
        {
            var query = (from a in _FaqCatMapping.Table 
                        where (a.CategoryId== categoryid) && (a.FaqId ==faqid)
                        select a).FirstOrDefault();

            if (query != null)
            {
                return true;
            }
            return false;
        }

        
      
    }
}
