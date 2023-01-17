using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Plugin.Widgets.TorchDesign_Support.Domain;
using Nop.Services.Localization;

namespace Nop.Plugin.Widgets.TorchDesign_Support.Services
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class SuportTopicExtensions
    {
        /// <summary>
        /// Finds a related product item by specified identifiers
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="productId1">The first product identifier</param>
        /// <param name="productId2">The second product identifier</param>
        /// <returns>Related product</returns>
		 public static SupportTopicRelatedProduct FindRelatedProduct(this IList<SupportTopicRelatedProduct> source,
            int supportTopicId, int productId)
        {
			  foreach (SupportTopicRelatedProduct relatedProduct in source)
				  if (relatedProduct.SupportTopicId == supportTopicId && relatedProduct.ProductId == productId)
                    return relatedProduct;
            return null;
        }

		 public static SupportDownloadRelatedProduct FindRelatedProduct(this IList<SupportDownloadRelatedProduct> source,
			  int supportDownloadId, int productId)
		 {
			 foreach (SupportDownloadRelatedProduct relatedProduct in source)
				 if (relatedProduct.SupportDownloadId == supportDownloadId && relatedProduct.ProductId == productId)
					 return relatedProduct;
			 return null;
		 }
		 /// <summary>
		 /// Finds a related product item by specified identifiers
		 /// </summary>
		 /// <param name="source">Source</param>
		 /// <param name="productId1">The first product identifier</param>
		 /// <param name="productId2">The second product identifier</param>
		 /// <returns>Related product</returns>
		 public static SupportVideoRelatedProduct FindRelatedProduct(this IList<SupportVideoRelatedProduct> source,
				int supportVideoId, int productId)
		 {
			 foreach (SupportVideoRelatedProduct relatedProduct in source)
				 if (relatedProduct.SupportVideoId == supportVideoId && relatedProduct.ProductId == productId)
					 return relatedProduct;
			 return null;
		 }

 
    }
}
