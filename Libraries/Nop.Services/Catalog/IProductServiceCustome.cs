using System;
using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Orders;

namespace Nop.Services.Catalog
{
    /// <summary>
    /// Product service
    /// </summary>
    public partial interface IProductServiceCustome
    {
        #region Products


        /// <summary>
        /// SearchProductsByLinq
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="categoryIds">Category identifiers</param>
        /// <param name="manufacturerId">Manufacturer identifier; 0 to load all records</param>
        /// <param name="partno">Part Number</param>
        /// <param name="storeId">Store identifier; 0 to load all records</param>
        /// <param name="vendorId">Vendor identifier; 0 to load all records</param>
        /// <param name="warehouseId">Warehouse identifier; 0 to load all records</param>
        /// <param name="parentGroupedProductId">Parent product identifier (used with grouped products); 0 to load all records</param>
        /// <param name="productType">Product type; 0 to load all records</param>
        /// <param name="visibleIndividuallyOnly">A values indicating whether to load only products marked as "visible individually"; "false" to load all records; "true" to load "visible individually" only</param>
        /// <param name="featuredProducts">A value indicating whether loaded products are marked as featured (relates only to categories and manufacturers). 0 to load featured products only, 1 to load not featured products only, null to load all products</param>
        /// <param name="priceMin">Minimum price; null to load all records</param>
        /// <param name="priceMax">Maximum price; null to load all records</param>
        /// <param name="productTagId">Product tag identifier; 0 to load all records</param>
        /// <param name="keywords">Keywords</param>
        /// <param name="searchDescriptions">A value indicating whether to search by a specified "keyword" in product descriptions</param>
        /// <param name="searchSku">A value indicating whether to search by a specified "keyword" in product SKU</param>
        /// <param name="searchProductTags">A value indicating whether to search by a specified "keyword" in product tags</param>
        /// <param name="languageId">Language identifier (search for text searching)</param>
        /// <param name="filteredSpecs">Filtered product specification identifiers</param>
        /// <param name="orderBy">Order by</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Products</returns>
        IPagedList<Product> SearchProductsByLinq(
            int pageIndex = 0,
            int pageSize = int.MaxValue,
            IList<int> categoryIds = null,
            int manufacturerId = 0,
            string partno = null,
            int storeId = 0,
            int vendorId = 0,
            int warehouseId = 0,
            int parentGroupedProductId = 0,
            ProductType? productType = null,
            bool visibleIndividuallyOnly = false,
            bool? featuredProducts = null,
            decimal? priceMin = null,
            decimal? priceMax = null,
            int productTagId = 0,
            string keywords = null,
            bool searchDescriptions = false,
            bool searchSku = true,
            bool searchProductTags = false,
            int languageId = 0,
            IList<int> filteredSpecs = null,
            ProductSortingEnum orderBy = ProductSortingEnum.Position,
            bool showHidden = false);






        IPagedList<Product> SearchProductsusinglinq(
          out IList<int> filterableSpecificationAttributeOptionIds,
          bool loadFilterableSpecificationAttributeOptionIds = false,
          int pageIndex = 0,
          int pageSize = int.MaxValue,
          IList<int> categoryIds = null,
          int manufacturerId = 0,
          string partno = null,
          int storeId = 0,
          int vendorId = 0,
          int warehouseId = 0,
          int parentGroupedProductId = 0,
          ProductType? productType = null,
          bool visibleIndividuallyOnly = false,
          bool? featuredProducts = null,
          decimal? priceMin = null,
          decimal? priceMax = null,
          int productTagId = 0,
          string keywords = null,
          bool searchDescriptions = false,
          bool searchSku = true,
          bool searchProductTags = false,
          int languageId = 0,
          IList<int> filteredSpecs = null,
          ProductSortingEnum orderBy = ProductSortingEnum.Position,
          bool showHidden = false);

        /// <summary>
        /// Gets a product by PartNumber
        /// </summary>
        /// <param name="sku">PartNumber</param>
        /// <returns>Product</returns>
        Product GetProductByPartnumber(string partnumber);

        ProductPicture GetProductPictureByPictureId(int PictureId);



        void InsertProductCustomeField(ProductCustomeField product);

        void UpdateProductCustomeField(ProductCustomeField product);

        ProductCustomeField GetCustomeFieldByProductId(int productId);


        #endregion
        
        #region Product Vedios


        void DeleteProductVideo(ProductVideo productvideo);

        IList<ProductVideo> GetProductVideoByProductId(int productId);

        ProductVideo GetProductVideoById(int productvideoId);

        ProductVideo GetProductVideoByPictureId(int pictureid);

        void InsertProductVideo(ProductVideo productvideo);

        void UpdateProductVideo(ProductVideo productvideo);

        #endregion

        #region Product Download


        void DeleteProductDownload(ProductDownload productdownload);

        IList<ProductDownload> GetProductDownloadByProductId(int productId);

        ProductDownload GetProductDownloadById(int productdownloadId);

        //ProductVideo GetProductVideoByPictureId(int pictureid);

        void InsertProductDownload(ProductDownload productdownload);

        void UpdateProductDownload(ProductDownload productdownload);

        #endregion

        #region Product In Box

        void DeleteProductFromBox(ProductInBox product);

        IList<ProductInBox> GetProductFromBoxByPerantProductId(int parentproductid);

        ProductInBox GetProductFromBoxById(int id);

        ProductInBox GetProductFromBoxByInBoxProductId(int inboxproductid);

        void InsertProductInBox(ProductInBox product);

        void UpdateProductInBox(ProductInBox product);

        #endregion
    }
}
