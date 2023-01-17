using System.Collections.Generic;
using Nop.Plugin.Widgets.TorchDesign_Support.Domain;

namespace Nop.Plugin.Widgets.TorchDesign_Support.Services
{
    /// <summary>
	 /// Created by Nick
    /// Support Topic service interface
    /// </summary>
    public partial interface ISupportService
    {
        

       
        /// <summary>
        /// Gets all Steps
        /// </summary>	       
        /// <returns>Steps</returns>
		  //IList<STopicSteps> GetAllSTopicSteps();

        /// <summary>
        /// Inserts a Stopic Step
        /// </summary>
        /// <param name="topic">Topic</param>
		  //void InsertSTopicStep(STopicSteps topic);
		 IList<SupportCategory> GetAllSupportCategory();
         IList<SupportCategory> GetAllSupportCategoryByProductCategoryId(int productCategoryId);
         SupportCategory GetSupportCategoryById(int supportcategoryId);
		 void InsertSupportCategory(SupportCategory supportcategory);
		 void UpdateSupportCategory(SupportCategory supportcategory);
         void DeleteSupportCategory(SupportCategory supportcategory);
         //void DeleteSupportTopicSupportCategory(SupportTopicSupportCategory supporttopicsupportcategory);

		 IList<SupportTopic> GetAllSupportTopics();
         IList<SupportTopic> GetAllSupportTopicBySupportCategoryId(int supportCategoryId);
         IList<SupportTopicSupportCategory> GetAllSupportTopicSupportCategoryBySupportCategoryId(int supportCategoryId);
         IList<SupportTopic> GetAllSupportTopicByProductCategoryFilter(int supportCategoryId=0, int productcategoryId = 0);
         IList<SupportCategory> GetAllSupportCategoryBySupporttopicFilter(int supportCategoryId = 0, int productcategoryId = 0);
         IList<SupportTopic> GetAllSupportTopicsByProductCategoryFilter(int productcategoryId = 0);
         IList<SupportTopic> GetAllSupportTopicsByProductIdFilter(int productid = 0);
         IList<SupportCategory> GetAllSupportCategoryByProductIdFilter(int productid = 0);
         SupportTopic GetSupportTopicById(int supportTopicId);
		 void InsertSupportTopic(SupportTopic supporttopic);
		 void UpdateSupportTopic(SupportTopic supporttopic);
		 void DeleteSupportTopic(SupportTopic supportTopic);

         IList<SupportTopicStep> GetAllSupportTopicStep();
         IList<SupportTopicStep> GetAllSupportTopicStepBySupportTopicId(int suporttopicid);
         SupportTopicStep GetSupportTopicStepById(int supportTopicstepId);
		 void InsertSupportTopicStep(SupportTopicStep supportTopicStep);
         void UpdateSupportTopicStep(SupportTopicStep supportTopicStep);
         void DeleteSupportTopicStep(SupportTopicStep supportTopicStep);
		        
         IList<SupportTopicRelatedProduct> GetRelatedProductsBySupportTopicId(int supportTopicId);
         IList<SupportTopicRelatedProduct> GetSupportTopicRelatedProductsByIds(int[] relatedProductIds);
		 SupportTopicRelatedProduct GetSupportTopicRelatedProductById(int relatedProductId);
         void InsertSupportTopicRelatedProduct(SupportTopicRelatedProduct relatedProduct);
         void DeleteSupportTopicRelatedProduct(SupportTopicRelatedProduct relatedProduct);
		 
         IList<SupportDownload> GetAllSupportDownloads();
         IList<SupportDownload> GetSupportDownloadByProductId(int productId);
         IList<SupportDownload> GetSupportDownloadByCategoryId(int categoryId);
         IList<SupportDownload> GetSupportDownloadByCaegoryIdsAndProductid(int[] categoryIds = null, int productid = 0);
         IList<SupportDownload> GetAllSupportDownloadByProductCategoryId(int productCategoryId);
         IList<SupportDownload> GetAllSupportDownloadByProductCategoryIds(int productid, IList<int> categoryIds = null);
         SupportDownload GetSupportDownloadById(int supportDownloadId);
		 void InsertSupportDownload(SupportDownload supportdownload);
		 void UpdateSupportDownload(SupportDownload supportdownload);
         void DeleteSupportDownload(SupportDownload supportdownload);

		 IList<SupportDownloadRelatedProduct> GetSupportDownloadRelatedProductsBySupportDownloadId(int supportDownloadId);
         IList<SupportDownloadRelatedProduct> GetSupportDownloadRelatedProductsByIds(int[] relatedProductIds);
         IList<SupportDownloadRelatedProduct> GetRelatedProductsBySupportDownloadId(int supportDownloadId);
         SupportDownloadRelatedProduct GetSupportDownloadRelatedProductById(int relatedProductId);
         void InsertSupportDownloadRelatedProduct(SupportDownloadRelatedProduct relatedProduct);
         void DeleteSupportDownloadRelatedProduct(SupportDownloadRelatedProduct relatedProduct);
		 
		 
		 void DeleteSupportDownloadProductCategory(SupportDownloadProductCategory supportDownloadProductCategory);
		 void DeleteSupportTopicProductCategory(SupportTopicProductCategory supportTopicProductCategory);
         void DeleteSupportCategoryProductCategory(SupportCategoryProductCategory SupportCategoryProductCategory);
		 void DeleteSupportTopicSupportCategory(SupportTopicSupportCategory supportTopicSupportCategory);
		
		 #region Support Video

         IList<SupportVideo> GetAllSupportVideos();
         IList<SupportVideo> GetAllSupportVideoByProductCategoryId(int productCategoryId);

         IList<SupportVideo> GetAllSupportVideoByProductCategoryIds(int productid, IList<int> categoryIds = null);
		 SupportVideo GetSupportVideoById(int supportVideoId);
         void InsertSupportVideo(SupportVideo supportvideo);
		 void UpdateSupportVideo(SupportVideo supportVideo);
         void DeleteSupportVideo(SupportVideo supportVideo);
         IList<SupportVideo> GetSupportVideoByProductId(int productId);
         IList<SupportVideo> GetSupportVideoByCategoryId(int categoryId);

		 IList<SupportVideoRelatedProduct> GetSupportVideoRelatedProductsBySupportVideoId(int supportVideoId);
         IList<SupportVideoRelatedProduct> GetSupportVideoRelatedProductsByIds(int[] relatedProductIds);
         IList<SupportVideoRelatedProduct> GetRelatedProductsBySupportVideoId(int supportVideoId);
         SupportVideoRelatedProduct GetSupportVideoRelatedProductById(int relatedProductId);
         //IList<SupportVideo> GetAllSupportVideoBySupportCategoryId(int supportCategoryId);
         void InsertSupportVideoRelatedProduct(SupportVideoRelatedProduct relatedProduct);
         void DeleteSupportVideoRelatedProduct(SupportVideoRelatedProduct relatedProduct);
		
		 	
		 void DeleteSupportVideoProductCategory(SupportVideoProductCategory supportVideoProductCategory);
		 #endregion
      
    }
}
