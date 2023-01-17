using Nop.Core;
using Nop.Plugin.TorchDesign.FAQ.Domain;
using System.Collections.Generic;

namespace Nop.Plugin.TorchDesign.FAQ.Services
{
    public partial interface IFrequentlyAskedQService
    {

        FrequentlyAskedQ GetFAQById(int id);

        IList<FrequentlyAskedQ> GetFAQByCategoryId(int id);

        void InsertFAQ(FrequentlyAskedQ record);

        void UpdateFAQ(FrequentlyAskedQ record);

        void DeleteFAQ(FrequentlyAskedQ record);
        
        IPagedList<FrequentlyAskedQ> GetAllFAQs(int pageIndex = 0, int pageSize = int.MaxValue);
        IPagedList<FrequentlyAskedQ> GetAllActiveFAQs(int pageIndex = 0, int pageSize = int.MaxValue);


        FAQ_Category GetFAQCategoryById(int id);

        void InsertFAQCategory(FAQ_Category record);

        void UpdateFAQCategory(FAQ_Category record);

        void DeleteFAQCategory(FAQ_Category record);

        IPagedList<FAQ_Category> GetAllFAQCategory(int pageIndex = 0, int pageSize = int.MaxValue);

        IList<FrequentlyAskedQ> GetAllFAQWithoutCategotyMapping();

        FAQ_Category_Mapping GetFAQCategoryMappingById(int id);

        void InsertFAQCategoryMapping(FAQ_Category_Mapping record);

        void UpdateFAQCategoryMapping(FAQ_Category_Mapping record);

        void DeleteFAQCategoryMapping(FAQ_Category_Mapping record);

        IList<FAQ_Category_Mapping> GetAllFAQCategoryMappingByFaqId(int faqid);

        IPagedList<FAQ_Category_Mapping> GetAllFAQCategoryMapping(int pageIndex = 0, int pageSize = int.MaxValue);

        bool CategoryExistOrNot(int categoryid,int faqid);

       
    }
}
