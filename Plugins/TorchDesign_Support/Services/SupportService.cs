using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core.Data;
using Nop.Services.Events;
using Nop.Core.Caching;
using Nop.Plugin.Widgets.TorchDesign_Support.Domain;



namespace Nop.Plugin.Widgets.TorchDesign_Support.Services
{
    /// <summary>
    /// Created by Nick
    /// Topic service
    /// </summary>
    public partial class SupportService : ISupportService
    {

        #region Constants
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : support category ID
        /// </remarks>
        private const string SupportCATEGORIES_BY_ID_KEY = "Nop.supportcategory.id-{0}";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : support category ID
        /// </remarks>
        private const string SupportTOPICS_BY_ID_KEY = "Nop.supporttopic.id-{0}";

        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string SupportCATEGORIES_PATTERN_KEY = "Nop.supportcategory.";


        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string SupportDOWNLOADS_PATTERN_KEY = "Nop.supportdownload.";


        private const string SupportTOPICSTEP_BY_ID_KEY = "Nop.supporttopicstep.id-{0}";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : support category ID
        /// </remarks>
        private const string SupportDOWNLOADS_BY_ID_KEY = "Nop.supportdownload.id-{0}";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : support Video 
        /// </remarks>
        private const string SupportVideo_BY_ID_KEY = "Nop.supportvideo.id-{0}";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : support category ID
        /// </remarks>
        private const string SupportVIDEOS_BY_ID_KEY = "Nop.supportvideo.id-{0}";



        #endregion

        #region Fields

        private readonly IRepository<SupportTopicProductCategory> _supportTopicProductCategoryRepository;
        private readonly IRepository<SupportCategoryProductCategory> _supportCategoryProductCategoryRepository;
        private readonly IRepository<SupportTopicSupportCategory> _supportTopicSupportCategoryRepository;
        private readonly IRepository<SupportCategory> _supportCategoryRepository;
        private readonly IRepository<SupportTopic> _supportTopicRepository;
        private readonly IRepository<SupportTopicStep> _supportTopicStepRepository;
        private readonly IRepository<SupportTopicRelatedProduct> _relatedProductRepository;

        private readonly IRepository<SupportDownload> _supportDownloadRepository;
        private readonly IRepository<SupportDownloadProductCategory> _supportDownloadProductCategoryRepository;
        private readonly IRepository<SupportDownloadRelatedProduct> _supportDownloadRelatedProductRepository;
        private readonly IRepository<SupportVideo> _supportVideoRepository;
        private readonly IRepository<SupportVideoProductCategory> _supportVideoProductCategoryRepository;
        private readonly IRepository<SupportVideoRelatedProduct> _supportVideoRelatedProductRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;
        //SupportCategory

        #endregion

        #region Ctor

        public SupportService(IRepository<SupportTopicProductCategory> SupportTopicProductCategoryRepository, IRepository<SupportCategoryProductCategory> supportCategoryProductCategoryRepository,
             IRepository<SupportTopicSupportCategory> SupportTopicSupportCategoryRepository,
             IRepository<SupportCategory> SupportCategoryRepository,
            IRepository<SupportTopic> SupportTopicRepository,
            IRepository<SupportTopicStep> SupportTopicStepRepository,
             IRepository<SupportTopicRelatedProduct> RelatedProductRepository,
            IRepository<SupportDownload> SupportDownloadRepository,
            IRepository<SupportDownloadProductCategory> SupportDownloadProductCategoryRepository,
            IRepository<SupportDownloadRelatedProduct> SupportDownloadRelatedProductRepository,
            IRepository<SupportVideo> supportVideoRepository,
            IRepository<SupportVideoProductCategory> supportVideoProductCategoryRepository,
            IRepository<SupportVideoRelatedProduct> supportVideoRelatedProductRepository,
             IEventPublisher eventPublisher,
            ICacheManager cacheManager)
        {
            this._supportTopicProductCategoryRepository = SupportTopicProductCategoryRepository;
            this._supportTopicSupportCategoryRepository = SupportTopicSupportCategoryRepository;
            this._supportCategoryProductCategoryRepository = supportCategoryProductCategoryRepository;
            this._supportCategoryRepository = SupportCategoryRepository;
            this._supportTopicRepository = SupportTopicRepository;
            this._supportDownloadRepository = SupportDownloadRepository;
            this._supportTopicStepRepository = SupportTopicStepRepository;
            this._relatedProductRepository = RelatedProductRepository;
            this._supportDownloadProductCategoryRepository = SupportDownloadProductCategoryRepository;
            this._supportDownloadRelatedProductRepository = SupportDownloadRelatedProductRepository;
            this._supportVideoRepository = supportVideoRepository;
            this._supportVideoProductCategoryRepository = supportVideoProductCategoryRepository;
            this._supportVideoRelatedProductRepository = supportVideoRelatedProductRepository;
            this._eventPublisher = eventPublisher;
            this._cacheManager = cacheManager;
        }

        #endregion

        #region Methods







        // /// <summary>
        // /// Gets all Support topic steps
        // /// </summary>        
        // /// <returns>Support Topic Steps</returns>
        //public virtual IList<STopicSteps> GetAllSTopicSteps()
        // {
        //	 var query = _STopicStepsRepository.Table;
        //	  query = query.OrderBy(t => t.Id);


        //			query = from t in query
        //					  select t;

        //	  return query.ToList();
        // }

        // /// <summary>
        // /// Inserts a support topic step
        // /// </summary>
        // /// <param name="topic">Support Topic STep</param>
        //public virtual void InsertSTopicStep(STopicSteps topicstep)
        // {
        //	 if (topicstep == null)
        //		 throw new ArgumentNullException("STopicSteps");

        //	 _STopicStepsRepository.Insert(topicstep);

        //	  //event notification
        //	 _eventPublisher.EntityInserted(topicstep);
        // }


        public virtual IList<SupportCategory> GetAllSupportCategory()
        {
            return _supportCategoryRepository.Table.ToList();
        }

        /// <summary>
        /// Get all support category by product category id
        /// </summary>
        /// <returns>support category</returns>
        public virtual IList<SupportCategory> GetAllSupportCategoryByProductCategoryId(int productCategoryId)
        {

            var supportCategoriyMapping = _supportCategoryProductCategoryRepository.Table;

            if (productCategoryId > 0)
                supportCategoriyMapping = supportCategoriyMapping.Where(x => x.CategoryId == productCategoryId);

            //var supportCategories = from s in _supportCategoryRepository.Table
            //                            where query.Select(x=>x.SupportCategoryId).Contains(s.Id)
            //                            select s;
            //return supportCategories.ToList();

            var q = (from scm in supportCategoriyMapping
                     join sc in _supportCategoryRepository.Table on scm.SupportCategoryId equals sc.Id
                     select sc);

            return q.ToList();

        }


        /// <summary>
        /// Get all support category by product category id
        /// </summary>
        /// <returns>support category</returns>
        public virtual IList<SupportTopic> GetAllSupportTopicBySupportCategoryId(int supportCategoryId)
        {

            //var supportTopicMapping = _supportTopicSupportCategoryRepository.Table;

            //if (supportCategoryId > 0)
            //    supportTopicMapping = supportTopicMapping.Where(x => x.SupportCategoryId == supportCategoryId);

            //var supportCategories = from s in _supportCategoryRepository.Table
            //                            where query.Select(x=>x.SupportCategoryId).Contains(s.Id)
            //                            select s;
            //return supportCategories.ToList();
            var query = from p in _supportTopicRepository.Table
                        from pc in p.SupportTopicSupportCategory.Where(pc => supportCategoryId.Equals(pc.SupportCategoryId))
                        select p;

            //var q = (from scm in supportTopicMapping
            //         join sc in _supportTopicRepository.Table on scm.SupportCategoryId equals sc.Id
            //         select sc);

            return query.ToList();

        }
        public virtual IList<SupportTopicSupportCategory> GetAllSupportTopicSupportCategoryBySupportCategoryId(int supportCategoryId)
        {
            var query = from p in _supportTopicSupportCategoryRepository.Table
                        where p.SupportCategoryId == supportCategoryId
                        select p;

            return query.ToList();

        }
        public virtual IList<SupportTopic> GetAllSupportTopicByProductCategoryFilter(int supportCategoryId = 0, int productcategoryId = 0)
        {

            //var query = from p in _supportTopicRepository.Table
            //            from pc in p.SupportTopicSupportCategory.Where(pc => supportCategoryId.Equals(pc.SupportCategoryId))
            //            select p;

            //if (productcategoryId > 0)
            //{
            //    query = from c in query
            //            from cc in c.SupportTopicProductCategories.Where(cc => cc.CategoryId.Equals(productcategoryId))
            //            select c;
            //}
            var query = from c in _supportTopicRepository.Table
                        from cc in c.SupportTopicProductCategories.Where(cc => cc.CategoryId.Equals(productcategoryId))
                        select c;
            if (supportCategoryId > 0)
            {
                query = from c in query
                        from cc in c.SupportTopicSupportCategory.Where(cc => cc.SupportCategoryId.Equals(supportCategoryId))
                        select c;
            }
            return query.ToList();
        }
        public virtual IList<SupportCategory> GetAllSupportCategoryBySupporttopicFilter(int supportCategoryId = 0, int productcategoryId = 0)
        {

            var query = (from c in _supportTopicRepository.Table
                         from cc in c.SupportTopicProductCategories.Where(cc => cc.CategoryId.Equals(productcategoryId))
                         select c.Id).ToArray().Distinct();


            var query3 = (from rp in _supportCategoryRepository.Table
                          from p in rp.SupportTopicSupportCategory.Select(x => x.SupportTopicId).Intersect(query)
                          select rp).Distinct();



            return query3.ToList();
        }
        public virtual IList<SupportTopic> GetAllSupportTopicsByProductCategoryFilter(int productcategoryId = 0)
        {

            var query = (from c in _supportTopicRepository.Table
                         from cc in c.SupportTopicProductCategories.Where(cc => cc.CategoryId.Equals(productcategoryId))
                         select c).ToArray().Distinct();

            return query.ToList();
        }

        public virtual IList<SupportCategory> GetAllSupportCategoryByProductIdFilter(int productid = 0)
        {

            var query = (from c in _supportTopicRepository.Table
                         join p in _relatedProductRepository.Table on c.Id equals p.SupportTopicId
                         where p.ProductId == productid
                         select c.Id).ToArray().Distinct();



            var query3 = from rp in _supportCategoryRepository.Table
                         from p in rp.SupportTopicSupportCategory.Select(x => x.SupportTopicId).Intersect(query)
                         select rp;



            return query3.ToList();
        }

        public virtual IList<SupportTopic> GetAllSupportTopicsByProductIdFilter(int productid = 0)
        {

            var query = (from c in _supportTopicRepository.Table
                         join p in _relatedProductRepository.Table on c.Id equals p.SupportTopicId
                         where p.ProductId == productid
                         select c).ToArray().Distinct();

            return query.ToList();
        }


        /// Gets a SupportCategory
        /// </summary>
        /// <param name="categoryId">SupportCategory identifier</param>
        /// <returns>SupportCategory</returns>
        public virtual SupportCategory GetSupportCategoryById(int supportcategoryId)
        {
            if (supportcategoryId == 0)
                return null;

            string key = string.Format(SupportCATEGORIES_BY_ID_KEY, supportcategoryId);
            return _cacheManager.Get(key, () => _supportCategoryRepository.GetById(supportcategoryId));
        }

        /// <summary>
        /// Inserts SupportCategory
        /// </summary>
        /// <param name="category">SupportCategory</param>
        public virtual void InsertSupportCategory(SupportCategory supportcategory)
        {
            if (supportcategory == null)
                throw new ArgumentNullException("SupportCategory");

            _supportCategoryRepository.Insert(supportcategory);

            //cache
            _cacheManager.RemoveByPattern(SupportCATEGORIES_PATTERN_KEY);


            //event notification
            _eventPublisher.EntityInserted(supportcategory);
        }

        /// <summary>
        /// Updates the support category
        /// </summary>
        /// <param name="supportcategory">Support Category</param>
        public virtual void UpdateSupportCategory(SupportCategory supportcategory)
        {
            if (supportcategory == null)
                throw new ArgumentNullException("supportcategory");

            //validate category hierarchy
            var supportCategory = GetSupportCategoryById(supportcategory.Id);

            _supportCategoryRepository.Update(supportCategory);

            //cache
            _cacheManager.RemoveByPattern(SupportCATEGORIES_PATTERN_KEY);


            //event notification
            _eventPublisher.EntityUpdated(supportCategory);
        }

        public virtual void DeleteSupportCategory(SupportCategory supportcategory)
        {
            if (supportcategory == null)
                throw new ArgumentNullException("supportcategory");

            _supportCategoryRepository.Delete(supportcategory);

            _eventPublisher.EntityDeleted(supportcategory);
        }

        public virtual IList<SupportTopic> GetAllSupportTopics()
        {
            return _supportTopicRepository.Table.ToList();
        }

        /// <summary>
        /// Gets a SupportTopic
        /// </summary>
        /// <param name="categoryId">SupportCategory identifier</param>
        /// <returns>SupportCategory</returns>
        public virtual SupportTopic GetSupportTopicById(int supportTopicId)
        {
            if (supportTopicId == 0)
                return null;

            string key = string.Format(SupportTOPICS_BY_ID_KEY, supportTopicId);
            return _cacheManager.Get(key, () => _supportTopicRepository.GetById(supportTopicId));
        }

        /// <summary>
        /// Inserts SupportTopic
        /// </summary>
        /// <param name="supporttopic">SupportTopic</param>
        public virtual void InsertSupportTopic(SupportTopic supporttopic)
        {
            if (supporttopic == null)
                throw new ArgumentNullException("supporttopic");

            _supportTopicRepository.Insert(supporttopic);

            //cache
            _cacheManager.RemoveByPattern(SupportTOPICS_BY_ID_KEY);


            //event notification
            _eventPublisher.EntityInserted(supporttopic);
        }

        /// <summary>
        /// Updates the support topic
        /// </summary>
        /// <param name="supporttopic">Support Topic</param>
        public virtual void UpdateSupportTopic(SupportTopic supporttopic)
        {
            if (supporttopic == null)
                throw new ArgumentNullException("supporttopic");

            //validate category hierarchy
            var supportCategory = GetSupportCategoryById(supporttopic.Id);

            _supportTopicRepository.Update(supporttopic);

            //cache
            _cacheManager.RemoveByPattern(SupportTOPICS_BY_ID_KEY);


            //event notification
            _eventPublisher.EntityUpdated(supporttopic);
        }

        /// <summary>
        /// Deletes a support topic
        /// </summary>
        /// <param name="topic">Topic</param>
        public virtual void DeleteSupportTopic(SupportTopic supportTopic)
        {
            if (supportTopic == null)
                throw new ArgumentNullException("supportTopic");

            _supportTopicRepository.Delete(supportTopic);

            //event notification
            _eventPublisher.EntityDeleted(supportTopic);
        }
        //public virtual void DeleteSupportTopicSupportCategory(SupportTopicSupportCategory supporttopicsupportcategory)
        //{
        //    if (supporttopicsupportcategory == null)
        //        throw new ArgumentNullException("supporttopicsupportcategory");

        //    _supportTopicSupportCategoryRepository.Delete(supporttopicsupportcategory);

        //    //event notification
        //    _eventPublisher.EntityDeleted(supporttopicsupportcategory);
        //}

        public virtual SupportTopicStep GetSupportTopicStepById(int supportTopicstepId)
        {
            if (supportTopicstepId == 0)
                return null;

            return _supportTopicStepRepository.GetById(supportTopicstepId);

        }
        public IList<SupportTopicStep> GetAllSupportTopicStep()
        {
            var query = (from p in _supportTopicStepRepository.Table
                         orderby p.DisplayOrder
                         select p).ToList();

            return query;

        }

        public IList<SupportTopicStep> GetAllSupportTopicStepBySupportTopicId(int suporttopicid)
        {
            var query = (from p in _supportTopicStepRepository.Table
                         where p.SupportTopicId == suporttopicid
                         orderby p.DisplayOrder
                         select p).ToList();

            return query;

        }
        /// <summary>
        /// Inserts SupportTopic
        /// </summary>
        /// <param name="supporttopic">SupportTopic</param>
        public virtual void InsertSupportTopicStep(SupportTopicStep supportTopicStep)
        {
            if (supportTopicStep == null)
                throw new ArgumentNullException("supportTopicStep");

            _supportTopicStepRepository.Insert(supportTopicStep);

            //cache
            _cacheManager.RemoveByPattern(SupportTOPICSTEP_BY_ID_KEY);


            //event notification
            _eventPublisher.EntityInserted(supportTopicStep);
        }
        public virtual void UpdateSupportTopicStep(SupportTopicStep supportTopicStep)
        {
            if (supportTopicStep == null)
                throw new ArgumentNullException("supporttopic");

            //validate category hierarchy
            var supportCategory = GetSupportCategoryById(supportTopicStep.Id);

            _supportTopicStepRepository.Update(supportTopicStep);

            //cache
            _cacheManager.RemoveByPattern(SupportTOPICS_BY_ID_KEY);


            //event notification
            _eventPublisher.EntityUpdated(supportTopicStep);
        }
        public virtual void DeleteSupportTopicStep(SupportTopicStep supportTopicStep)
        {
            if (supportTopicStep == null)
                throw new ArgumentNullException("supportTopic");

            _supportTopicStepRepository.Delete(supportTopicStep);

            //event notification
            _eventPublisher.EntityDeleted(supportTopicStep);
        }


        /// <summary>
        /// Gets a related product collection by product identifier
        /// </summary>
        /// <param name="productId1">The first product identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Related product collection</returns>
        public virtual IList<SupportTopicRelatedProduct> GetRelatedProductsBySupportTopicId(int supportTopicId)
        {
            var query = from rp in _relatedProductRepository.Table
                        join p in _supportTopicRepository.Table on rp.SupportTopicId equals p.Id
                        where rp.SupportTopicId == supportTopicId
                        orderby rp.DisplayOrder
                        select rp;
            var relatedProducts = query.ToList();

            return relatedProducts;
        }

        /// <summary>
        /// Inserts a related product
        /// </summary>
        /// <param name="relatedProduct">Related product</param>
        public virtual void InsertSupportTopicRelatedProduct(SupportTopicRelatedProduct relatedProduct)
        {
            if (relatedProduct == null)
                throw new ArgumentNullException("relatedProduct");

            _relatedProductRepository.Insert(relatedProduct);

            //event notification
            _eventPublisher.EntityInserted(relatedProduct);
        }

        /// <summary>
        /// Gets a related product
        /// </summary>
        /// <param name="relatedProductId">Related product identifier</param>
        /// <returns>Related product</returns>
        public virtual SupportTopicRelatedProduct GetSupportTopicRelatedProductById(int relatedProductId)
        {
            if (relatedProductId == 0)
                return null;

            return _relatedProductRepository.GetById(relatedProductId);
        }


        public virtual IList<SupportTopicRelatedProduct> GetSupportTopicRelatedProductsByIds(int[] relatedProductIds)
        {
            if (relatedProductIds == null || relatedProductIds.Length == 0)
                return new List<SupportTopicRelatedProduct>();

            var query = from rp in _relatedProductRepository.Table
                        where relatedProductIds.Contains(rp.Id)
                        select rp;
            var relatedProducts = query.ToList();
            //sort by passed identifiers
            var sortedProducts = new List<SupportTopicRelatedProduct>();
            foreach (int id in relatedProductIds)
            {
                var relatedProduct = relatedProducts.Find(x => x.Id == id);
                if (relatedProduct != null)
                    sortedProducts.Add(relatedProduct);
            }
            return sortedProducts;

        }
        /// <summary>
        /// Deletes a related product
        /// </summary>
        /// <param name="relatedProduct">Related product</param>
        public virtual void DeleteSupportTopicRelatedProduct(SupportTopicRelatedProduct relatedProduct)
        {
            if (relatedProduct == null)
                throw new ArgumentNullException("relatedProduct");

            _relatedProductRepository.Delete(relatedProduct);

            //event notification
            _eventPublisher.EntityDeleted(relatedProduct);
        }


        public virtual IList<SupportDownload> GetAllSupportDownloads()
        {
            return _supportDownloadRepository.Table.OrderBy(x => x.DisplayOrder).ToList();
        }

        /// <summary>
        /// Inserts SupportDownload
        /// </summary>
        /// <param name="supportcategory">SupportDownload</param>
        public virtual void InsertSupportDownload(SupportDownload supportdownload)
        {
            if (supportdownload == null)
                throw new ArgumentNullException("supportdownload");

            _supportDownloadRepository.Insert(supportdownload);

            //cache
            _cacheManager.RemoveByPattern(SupportDOWNLOADS_PATTERN_KEY);


            //event notification
            _eventPublisher.EntityInserted(supportdownload);
        }

        /// <summary>
        /// Update SupportDownload
        /// </summary>
        /// <param name="supportcategory">SupportDownload</param>
        public virtual void UpdateSupportDownload(SupportDownload supportdownload)
        {
            if (supportdownload == null)
                throw new ArgumentNullException("supportdownload");

            _supportDownloadRepository.Update(supportdownload);

            //cache
            _cacheManager.RemoveByPattern(SupportDOWNLOADS_PATTERN_KEY);


            //event notification
            _eventPublisher.EntityUpdated(supportdownload);
        }

        public virtual void DeleteSupportDownload(SupportDownload supportdownload)
        {
            if (supportdownload == null)
                throw new ArgumentNullException("supportdownload");

            _supportDownloadRepository.Delete(supportdownload);

            _eventPublisher.EntityDeleted(supportdownload);
        }

        /// <summary>
        /// Gets a SupportDownload
        /// </summary>
        /// <param name="supportDownloadId">SupportDownload identifier</param>
        /// <returns>SupportCategory</returns>
        public virtual SupportDownload GetSupportDownloadById(int supportDownloadId)
        {
            if (supportDownloadId == 0)
                return null;

            string key = string.Format(SupportDOWNLOADS_BY_ID_KEY, supportDownloadId);
            return _cacheManager.Get(key, () => _supportDownloadRepository.GetById(supportDownloadId));
        }

        /// <summary>
        /// Gets a related product collection by product identifier
        /// </summary>
        /// <param name="productId1">The first product identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Related product collection</returns>
        public virtual IList<SupportDownloadRelatedProduct> GetSupportDownloadRelatedProductsBySupportDownloadId(int supportDownloadId)
        {
            var query = from rp in _supportDownloadRelatedProductRepository.Table
                        join p in _supportDownloadRepository.Table on rp.SupportDownloadId equals p.Id
                        where rp.SupportDownloadId == supportDownloadId
                        orderby rp.DisplayOrder
                        select rp;
            var relatedProducts = query.ToList();

            return relatedProducts;
        }

        /// <summary>
        /// Gets a related product
        /// </summary>
        /// <param name="relatedProductId">Related product identifier</param>
        /// <returns>Related product</returns>
        public virtual SupportDownloadRelatedProduct GetSupportDownloadRelatedProductById(int relatedProductId)
        {
            if (relatedProductId == 0)
                return null;

            return _supportDownloadRelatedProductRepository.GetById(relatedProductId);
        }

        public virtual IList<SupportDownloadRelatedProduct> GetSupportDownloadRelatedProductsByIds(int[] relatedProductIds)
        {
            if (relatedProductIds == null || relatedProductIds.Length == 0)
                return new List<SupportDownloadRelatedProduct>();

            var query = from rp in _supportDownloadRelatedProductRepository.Table
                        where relatedProductIds.Contains(rp.Id)
                        select rp;
            var relatedProducts = query.ToList();
            //sort by passed identifiers
            var sortedProducts = new List<SupportDownloadRelatedProduct>();
            foreach (int id in relatedProductIds)
            {
                var relatedProduct = relatedProducts.Find(x => x.Id == id);
                if (relatedProduct != null)
                    sortedProducts.Add(relatedProduct);
            }
            return sortedProducts;
        }
        /// <summary>
        /// Deletes a related product
        /// </summary>
        /// <param name="relatedProduct">Related product</param>
        public virtual void DeleteSupportDownloadRelatedProduct(SupportDownloadRelatedProduct relatedProduct)
        {
            if (relatedProduct == null)
                throw new ArgumentNullException("relatedProduct");

            _supportDownloadRelatedProductRepository.Delete(relatedProduct);

            //event notification
            _eventPublisher.EntityDeleted(relatedProduct);
        }

        /// <summary>
        /// Gets a related product collection by product identifier
        /// </summary>
        /// <param name="productId1">The first product identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Related product collection</returns>
        public virtual IList<SupportDownloadRelatedProduct> GetRelatedProductsBySupportDownloadId(int supportDownloadId)
        {
            var query = from rp in _supportDownloadRelatedProductRepository.Table
                        join p in _supportDownloadRepository.Table on rp.SupportDownloadId equals p.Id
                        where rp.SupportDownloadId == supportDownloadId
                        orderby rp.DisplayOrder
                        select rp;
            var relatedProducts = query.ToList();

            return relatedProducts;
        }
        /// <summary>
        /// Inserts a related product
        /// </summary>
        /// <param name="relatedProduct">Related product</param>
        public virtual void InsertSupportDownloadRelatedProduct(SupportDownloadRelatedProduct relatedProduct)
        {
            if (relatedProduct == null)
                throw new ArgumentNullException("relatedProduct");

            _supportDownloadRelatedProductRepository.Insert(relatedProduct);

            //event notification
            _eventPublisher.EntityInserted(relatedProduct);
        }

        public virtual void DeleteSupportDownloadProductCategory(SupportDownloadProductCategory supportDownloadProductCategory)
        {
            if (supportDownloadProductCategory == null)
                throw new ArgumentNullException("supportDownloadProductCategory");

            _supportDownloadProductCategoryRepository.Delete(supportDownloadProductCategory);


            //event notification
            _eventPublisher.EntityDeleted(supportDownloadProductCategory);
        }

        public virtual void DeleteSupportTopicProductCategory(SupportTopicProductCategory supportTopicProductCategory)
        {
            if (supportTopicProductCategory == null)
                throw new ArgumentNullException("supportTopicProductCategory");


            _supportTopicProductCategoryRepository.Delete(supportTopicProductCategory);


            //event notification
            _eventPublisher.EntityDeleted(supportTopicProductCategory);
        }
        public virtual void DeleteSupportCategoryProductCategory(SupportCategoryProductCategory SupportCategoryProductCategory)
        {
            if (SupportCategoryProductCategory == null)
                throw new ArgumentNullException("supportTopicProductCategory");


            _supportCategoryProductCategoryRepository.Delete(SupportCategoryProductCategory);


            //event notification
            _eventPublisher.EntityDeleted(SupportCategoryProductCategory);
        }
        public virtual void DeleteSupportTopicSupportCategory(SupportTopicSupportCategory supportTopicSupportCategory)
        {
            if (supportTopicSupportCategory == null)
                throw new ArgumentNullException("supportTopicSupportCategory");


            _supportTopicSupportCategoryRepository.Delete(supportTopicSupportCategory);


            //event notification
            _eventPublisher.EntityDeleted(supportTopicSupportCategory);
        }

        #endregion

        #region Support Video

        public virtual void InsertSupportVideo(SupportVideo supportvideo)
        {
            if (supportvideo == null)
                throw new ArgumentNullException("supportvideo");
            _supportVideoRepository.Insert(supportvideo);

            //cache
            _cacheManager.RemoveByPattern(SupportVideo_BY_ID_KEY);


            //event notification
            _eventPublisher.EntityInserted(supportvideo);
        }

        /// <summary>
        /// Gets a SupportTopic
        /// </summary>
        /// <param name="categoryId">SupportCategory identifier</param>
        /// <returns>SupportCategory</returns>
        public virtual SupportVideo GetSupportVideoById(int supportVideoId)
        {
            if (supportVideoId == 0)
                return null;

            string key = string.Format(SupportVideo_BY_ID_KEY, supportVideoId);
            return _cacheManager.Get(key, () => _supportVideoRepository.GetById(supportVideoId));
        }

        public virtual IList<SupportVideo> GetAllSupportVideos()
        {
            return _supportVideoRepository.Table.ToList();
        }

        /// <summary>
        /// Gets a related product collection by product identifier
        /// </summary>
        /// <param name="productId1">The first product identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Related product collection</returns>
        public virtual IList<SupportVideoRelatedProduct> GetSupportVideoRelatedProductsBySupportVideoId(int supportVideoId)
        {
            var query = from rp in _supportVideoRelatedProductRepository.Table
                        join p in _supportVideoRepository.Table on rp.SupportVideoId equals p.Id
                        where rp.SupportVideoId == supportVideoId
                        orderby rp.DisplayOrder
                        select rp;
            var relatedProducts = query.ToList();

            return relatedProducts;
        }

        /// <summary>
        /// Gets a related product
        /// </summary>
        /// <param name="relatedProductId">Related product identifier</param>
        /// <returns>Related product</returns>
        public virtual SupportVideoRelatedProduct GetSupportVideoRelatedProductById(int relatedProductId)
        {
            if (relatedProductId == 0)
                return null;

            return _supportVideoRelatedProductRepository.GetById(relatedProductId);
        }
        public virtual IList<SupportVideoRelatedProduct> GetSupportVideoRelatedProductsByIds(int[] relatedProductIds)
        {
            if (relatedProductIds == null || relatedProductIds.Length == 0)
                return new List<SupportVideoRelatedProduct>();

            var query = from rp in _supportVideoRelatedProductRepository.Table
                        where relatedProductIds.Contains(rp.Id)
                        select rp;
            var relatedProducts = query.ToList();
            //sort by passed identifiers
            var sortedProducts = new List<SupportVideoRelatedProduct>();
            foreach (int id in relatedProductIds)
            {
                var relatedProduct = relatedProducts.Find(x => x.Id == id);
                if (relatedProduct != null)
                    sortedProducts.Add(relatedProduct);
            }
            return sortedProducts;
        }

        public virtual IList<SupportVideo> GetSupportVideoByProductId(int productId)
        {

            var query = (from p in _supportVideoRepository.Table
                         from pc in p.SupportVideosRelatedProducts.Where(pc => pc.ProductId.Equals(productId))
                         select p).ToList();
            return query.ToList();
        }

        public virtual IList<SupportVideo> GetSupportVideoByCategoryId(int categoryId)
        {

            var query = (from p in _supportVideoRepository.Table
                         from pc in p.SupportVideoProductCategories.Where(pc => pc.CategoryId.Equals(categoryId))
                         select p).ToList();
            return query.ToList();
        }


        public virtual IList<SupportDownload> GetSupportDownloadByProductId(int productId)
        {

            var query = (from p in _supportDownloadRepository.Table
                         from pc in p.SupportDownloadRelatedProducts.Where(pc => pc.ProductId.Equals(productId))
                         select p).ToList();
            return query.ToList();
        }

        public virtual IList<SupportDownload> GetSupportDownloadByCategoryId(int categoryId)
        {

            var query = (from p in _supportDownloadRepository.Table
                         from pc in p.SupportDownloadProductCategories.Where(pc => pc.CategoryId.Equals(categoryId))
                         select p).ToList();
            return query.ToList();
        }

        public virtual IList<SupportDownload> GetSupportDownloadByCaegoryIdsAndProductid(int[] categoryIds = null, int productid = 0)
        {
            var categoryIdList = new List<int>(); 

            if (categoryIds != null)
                categoryIdList = categoryIds.ToList();

            var query = (from p in _supportDownloadRepository.Table
                         from pc in p.SupportDownloadProductCategories.Where(pc => categoryIdList.Contains(pc.CategoryId))
                         select p).ToList();


            var query1 = (from p in _supportDownloadRepository.Table
                          from pc in p.SupportDownloadRelatedProducts.Where(pc => pc.ProductId.Equals(productid))
                          select p).ToList();
            var result = query.Concat(query).Concat(query1).ToList().OrderBy(x => x.Id).Distinct().ToList();

            return result;
        }

        public virtual IList<SupportDownload> GetAllSupportDownloadByProductCategoryId(int productCategoryId)
        {

            var suppordownloadMapping = _supportDownloadProductCategoryRepository.Table;

            if (productCategoryId > 0)
                suppordownloadMapping = suppordownloadMapping.Where(x => x.CategoryId == productCategoryId);

            //var supportCategories = from s in _supportCategoryRepository.Table
            //                            where query.Select(x=>x.SupportCategoryId).Contains(s.Id)
            //                            select s;
            //return supportCategories.ToList();

            var q = (from scm in suppordownloadMapping
                     join sc in _supportDownloadRepository.Table on scm.SupportDownloadId equals sc.Id
                     select sc);

            return q.ToList();

        }

        public virtual IList<SupportDownload> GetAllSupportDownloadByProductCategoryIds(int productid, IList<int> categoryIds = null)
        {
            categoryIds = categoryIds ?? new List<int>();
            var Cquery = (from p in _supportDownloadRepository.Table
                          from pc in p.SupportDownloadProductCategories.Where(pc => categoryIds.Contains(pc.CategoryId))
                          select p).ToList();

            var Pquery = (from p in _supportDownloadRepository.Table
                          from dc in p.SupportDownloadRelatedProducts.Where(dc => dc.ProductId.Equals(productid))
                          select p).ToList();

            Cquery.AddRange(Pquery);
            var result = Cquery.Distinct();
            return result.ToList();

        }

        public virtual IList<SupportVideo> GetAllSupportVideoByProductCategoryId(int productCategoryId)
        {

            var supportvideoMapping = _supportVideoProductCategoryRepository.Table;

            if (productCategoryId > 0)
                supportvideoMapping = supportvideoMapping.Where(x => x.CategoryId == productCategoryId);

            //var supportCategories = from s in _supportCategoryRepository.Table
            //                            where query.Select(x=>x.SupportCategoryId).Contains(s.Id)
            //                            select s;
            //return supportCategories.ToList();

            var q = (from scm in supportvideoMapping
                     join sc in _supportVideoRepository.Table on scm.SupportVideoId equals sc.Id
                     select sc);

            return q.ToList();

        }

        public virtual IList<SupportVideo> GetAllSupportVideoByProductCategoryIds(int productid, IList<int> categoryIds = null)
        {
            categoryIds = categoryIds ?? new List<int>();
            var Cquery = (from p in _supportVideoRepository.Table
                          from pc in p.SupportVideoProductCategories.Where(pc => categoryIds.Contains(pc.CategoryId))
                          select p).ToList();

            var Pquery = (from p in _supportVideoRepository.Table
                          from dc in p.SupportVideosRelatedProducts.Where(dc => dc.ProductId.Equals(productid))
                          select p).ToList();
            Cquery.AddRange(Pquery);
            var result = Cquery.Distinct();
            return result.ToList();

        }
        //public virtual IList<SupportVideo> GetAllSupportVideoBySupportCategoryId(int supportCategoryId)
        //{


        //    var query = from p in _supportVideoRepository.Table
        //                from pc in p.SupportVideoSupportCategory.Where(pc => supportCategoryId.Equals(pc.SupportCategoryId))
        //                select p;

        //    //var q = (from scm in supportTopicMapping
        //    //         join sc in _supportTopicRepository.Table on scm.SupportCategoryId equals sc.Id
        //    //         select sc);

        //    return query.ToList();

        //}

        /// <summary>
        /// Deletes a related product
        /// </summary>
        /// <param name="relatedProduct">Related product</param>
        public virtual void DeleteSupportVideoRelatedProduct(SupportVideoRelatedProduct relatedProduct)
        {
            if (relatedProduct == null)
                throw new ArgumentNullException("relatedProduct");

            _supportVideoRelatedProductRepository.Delete(relatedProduct);

            //event notification
            _eventPublisher.EntityDeleted(relatedProduct);
        }

        /// <summary>
        /// Gets a related product collection by product identifier
        /// </summary>
        /// <param name="productId1">The first product identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Related product collection</returns>
        public virtual IList<SupportVideoRelatedProduct> GetRelatedProductsBySupportVideoId(int supportVideoId)
        {
            var query = from rp in _supportVideoRelatedProductRepository.Table
                        join p in _supportVideoRepository.Table on rp.SupportVideoId equals p.Id
                        where rp.SupportVideoId == supportVideoId
                        orderby rp.DisplayOrder
                        select rp;
            var relatedProducts = query.ToList();

            return relatedProducts;
        }

        /// <summary>
        /// Inserts a related product
        /// </summary>
        /// <param name="relatedProduct">Related product</param>
        public virtual void InsertSupportVideoRelatedProduct(SupportVideoRelatedProduct relatedProduct)
        {
            if (relatedProduct == null)
                throw new ArgumentNullException("relatedProduct");
            _supportVideoRelatedProductRepository.Insert(relatedProduct);

            //event notification
            _eventPublisher.EntityInserted(relatedProduct);
        }

        /// <summary>
        /// Updates the support video
        /// </summary>
        /// <param name="supporttopic">Support Video</param>
        public virtual void UpdateSupportVideo(SupportVideo supportvideo)
        {
            if (supportvideo == null)
                throw new ArgumentNullException("supportvideo");

            //validate category hierarchy
            var supportCategory = GetSupportCategoryById(supportvideo.Id);
            _supportVideoRepository.Update(supportvideo);

            //cache
            _cacheManager.RemoveByPattern(SupportVIDEOS_BY_ID_KEY);


            //event notification
            _eventPublisher.EntityUpdated(supportvideo);
        }

        /// <summary>
        /// Deletes a support topic
        /// </summary>
        /// <param name="topic">Topic</param>
        public virtual void DeleteSupportVideo(SupportVideo supportVideo)
        {
            if (supportVideo == null)
                throw new ArgumentNullException("supportVideo");
            _supportVideoRepository.Delete(supportVideo);

            //event notification
            _eventPublisher.EntityDeleted(supportVideo);
        }

        public virtual void DeleteSupportVideoProductCategory(SupportVideoProductCategory supportVideoProductCategory)
        {
            if (supportVideoProductCategory == null)
                throw new ArgumentNullException("supportVideoProductCategory");

            _supportVideoProductCategoryRepository.Delete(supportVideoProductCategory);


            //event notification
            _eventPublisher.EntityDeleted(supportVideoProductCategory);
        }


        #endregion
    }
}
