﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Media;
using Nop.Services.Catalog;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Tax;
using Nop.Services.Orders;
using Nop.Services.Stores;
using Nop.Plugin.TorchDesign.MegaSearch.Models;

namespace Nop.Plugin.TorchDesign.MegaSearch.Models
{
    //here we have some methods shared between controllers
    public static class ControllerExtensions
    {
        public static IList<ProductSpecificationModel> PrepareProductSpecificationModel(this Controller controller,
            IWorkContext workContext,
            ISpecificationAttributeService specificationAttributeService,
            ICacheManager cacheManager,
            Product product)
        {
            if (product == null)
                throw new ArgumentNullException("product");

            string cacheKey = string.Format(ModelCacheEventConsumer.PRODUCT_SPECS_MODEL_KEY, product.Id, workContext.WorkingLanguage.Id);
            return cacheManager.Get(cacheKey, () =>
            {
                var model = specificationAttributeService.GetProductSpecificationAttributesByProductId(product.Id, null, true)
                   .Select(psa =>
                   {
                       return new ProductSpecificationModel()
                       {
                           SpecificationAttributeId = psa.SpecificationAttributeOption.SpecificationAttributeId,
                           SpecificationAttributeName = psa.SpecificationAttributeOption.SpecificationAttribute.GetLocalized(x => x.Name),
                           SpecificationAttributeOption = !String.IsNullOrEmpty(psa.CustomValue) ? psa.CustomValue : psa.SpecificationAttributeOption.GetLocalized(x => x.Name),
                       };
                   }).ToList();
                return model;
            });
        }

        public static IEnumerable<ProductOverviewModel> PrepareProductOverviewModels(this Controller controller,
            IWorkContext workContext,
            IStoreContext storeContext,
            ICategoryService categoryService,
            IProductService productService,
            ISpecificationAttributeService specificationAttributeService,
            IPriceCalculationService priceCalculationService,
            IPriceFormatter priceFormatter,
            IPermissionService permissionService,
            ILocalizationService localizationService,
            ITaxService taxService,
            ICurrencyService currencyService,
            IPictureService pictureService,
            IWebHelper webHelper,
            ICacheManager cacheManager,
            CatalogSettings catalogSettings,
            MediaSettings mediaSettings,  
            IOrderReportService orderReportService,
            IAclService aclService,
            IStoreMappingService storeMappingService,
            IEnumerable<Product> products,
            bool preparePriceModel = true, bool preparePictureModel = true,
            int? productThumbPictureSize = null, bool prepareSpecificationAttributes = false,
            bool forceRedirectionAfterAddingToCart = false
         )
        {
            if (products == null)
                throw new ArgumentNullException("products");



            //load and cache report
            var report = cacheManager.Get(string.Format(ModelCacheEventConsumer.HOMEPAGE_BESTSELLERS_IDS_KEY, storeContext.CurrentStore.Id),
                () =>
                    orderReportService.BestSellersReport(storeId: storeContext.CurrentStore.Id,
                    pageSize: catalogSettings.NumberOfBestsellersOnHomepage));


            //load products
            var bestsellerproducts = productService.GetProductsByIds(report.Select(x => x.ProductId).ToArray());
            //ACL and store mapping
            bestsellerproducts = bestsellerproducts.Where(p => aclService.Authorize(p) && storeMappingService.Authorize(p)).ToList();
           
            bool bestseller = true;
            if (bestsellerproducts.Count == 0)
                { 
                bestseller = false; 
               }

            var recentlyaddedproducts = productService.SearchProducts(
                storeId: storeContext.CurrentStore.Id,
                visibleIndividuallyOnly: true,
                orderBy: ProductSortingEnum.CreatedOn,
                pageSize: catalogSettings.RecentlyAddedProductsNumber);

            var models = new List<ProductOverviewModel>();
            foreach (var product in products)
            {
                
                var model = new ProductOverviewModel()
                {

                   Id = product.Id,
                    Name = product.GetLocalized(x => x.Name),
                    ShortDescription = product.GetLocalized(x => x.ShortDescription),
                    FullDescription = product.GetLocalized(x => x.FullDescription),
                    SeName = product.GetSeName(),
                    Manifacturepartno=product.ManufacturerPartNumber,
                    
                };
             
                if (bestseller)
                {

                    foreach (var bestsall in recentlyaddedproducts)
                    {
                        if (product.Id == bestsall.Id)
                        {
                            model.IsBestSellers = true;
                            break;
                        }
                        else
                        {
                            model.IsBestSellers = false;
                        }
                    }
                   
                }

                foreach (var recent in recentlyaddedproducts)
                {
                    if (product.Id == recent.Id)
                    {
                        model.IsNewProduct = true;
                        break;
                    }
                    else
                    {
                        model.IsNewProduct = false;
                    }
                }
                //price
                if (preparePriceModel)
                {
                    #region Prepare product price

                    var priceModel = new ProductOverviewModel.ProductPriceModel()
                    {
                        ForceRedirectionAfterAddingToCart = forceRedirectionAfterAddingToCart
                    };

                    switch (product.ProductType)
                    {
                        case ProductType.GroupedProduct:
                            {
                                #region Grouped product

                                var associatedProducts = productService.GetAssociatedProducts(product.Id, storeContext.CurrentStore.Id);

                                switch (associatedProducts.Count)
                                {
                                    case 0:
                                        {
                                            //no associated products
                                            priceModel.OldPrice = null;
                                            priceModel.Price = null;
                                            priceModel.DisableBuyButton = true;
                                            priceModel.DisableWishlistButton = true;
                                            priceModel.AvailableForPreOrder = false;
                                        }
                                        break;
                                    default:
                                        {
                                            //we have at least one associated product
                                            priceModel.DisableBuyButton = true;
                                            priceModel.DisableWishlistButton = true;
                                            priceModel.AvailableForPreOrder = false;

                                            if (permissionService.Authorize(StandardPermissionProvider.DisplayPrices))
                                            {
                                                //find a minimum possible price
                                                decimal? minPossiblePrice = null;
                                                Product minPriceProduct = null;
                                                foreach (var associatedProduct in associatedProducts)
                                                {
                                                    //calculate for the maximum quantity (in case if we have tier prices)
                                                    var tmpPrice = priceCalculationService.GetFinalPrice(associatedProduct,
                                                        workContext.CurrentCustomer, decimal.Zero, true, int.MaxValue);
                                                    if (!minPossiblePrice.HasValue || tmpPrice < minPossiblePrice.Value)
                                                    {
                                                        minPriceProduct = associatedProduct;
                                                        minPossiblePrice = tmpPrice;
                                                    }
                                                }
                                                if (minPriceProduct != null && !minPriceProduct.CustomerEntersPrice)
                                                {
                                                    if (minPriceProduct.CallForPrice)
                                                    {
                                                        priceModel.OldPrice = null;
                                                        priceModel.Price = localizationService.GetResource("Products.CallForPrice");
                                                    }
                                                    else if (minPossiblePrice.HasValue)
                                                    {
                                                        //calculate prices
                                                        decimal taxRate = decimal.Zero;
                                                        decimal finalPriceBase = taxService.GetProductPrice(minPriceProduct, minPossiblePrice.Value, out taxRate);
                                                        decimal finalPrice = currencyService.ConvertFromPrimaryStoreCurrency(finalPriceBase, workContext.WorkingCurrency);

                                                        priceModel.OldPrice = null;
                                                        priceModel.Price = String.Format(localizationService.GetResource("Products.PriceRangeFrom"), priceFormatter.FormatPrice(finalPrice));

                                                    }
                                                    else
                                                    {
                                                        //Actually it's not possible (we presume that minimalPrice always has a value)
                                                        //We never should get here
                                                        Debug.WriteLine(string.Format("Cannot calculate minPrice for product #{0}", product.Id));
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //hide prices
                                                priceModel.OldPrice = null;
                                                priceModel.Price = null;
                                            }
                                        }
                                        break;
                                }

                                #endregion
                            }
                            break;
                        case ProductType.SimpleProduct:
                        default:
                            {
                                #region Simple product

                                //add to cart button
                                priceModel.DisableBuyButton = product.DisableBuyButton ||
                                    !permissionService.Authorize(StandardPermissionProvider.EnableShoppingCart) ||
                                    !permissionService.Authorize(StandardPermissionProvider.DisplayPrices);

                                //add to wishlist button
                                priceModel.DisableWishlistButton = product.DisableWishlistButton ||
                                    !permissionService.Authorize(StandardPermissionProvider.EnableWishlist) ||
                                    !permissionService.Authorize(StandardPermissionProvider.DisplayPrices);
                                //pre-order
                                if (product.AvailableForPreOrder)
                                {
                                    priceModel.AvailableForPreOrder = !product.PreOrderAvailabilityStartDateTimeUtc.HasValue ||
                                        product.PreOrderAvailabilityStartDateTimeUtc.Value >= DateTime.UtcNow;
                                    priceModel.PreOrderAvailabilityStartDateTimeUtc = product.PreOrderAvailabilityStartDateTimeUtc;
                                }

                                //prices
                                if (permissionService.Authorize(StandardPermissionProvider.DisplayPrices))
                                {
                                    //calculate for the maximum quantity (in case if we have tier prices)
                                    decimal minPossiblePrice = priceCalculationService.GetFinalPrice(product,
                                        workContext.CurrentCustomer, decimal.Zero, true, int.MaxValue);
                                    if (!product.CustomerEntersPrice)
                                    {
                                        if (product.CallForPrice)
                                        {
                                            //call for price
                                            priceModel.OldPrice = null;
                                            priceModel.Price = localizationService.GetResource("Products.CallForPrice");
                                        }
                                        else
                                        {
                                            //calculate prices
                                            decimal taxRate = decimal.Zero;
                                            decimal oldPriceBase = taxService.GetProductPrice(product, product.OldPrice, out taxRate);
                                            decimal finalPriceBase = taxService.GetProductPrice(product, minPossiblePrice, out taxRate);

                                            decimal oldPrice = currencyService.ConvertFromPrimaryStoreCurrency(oldPriceBase, workContext.WorkingCurrency);
                                            decimal finalPrice = currencyService.ConvertFromPrimaryStoreCurrency(finalPriceBase, workContext.WorkingCurrency);

                                            //do we have tier prices configured?
                                            var tierPrices = new List<TierPrice>();
                                            if (product.HasTierPrices)
                                            {
                                                tierPrices.AddRange(product.TierPrices
                                                    .OrderBy(tp => tp.Quantity)
                                                    .ToList()
                                                    .FilterByStore(storeContext.CurrentStore.Id)
                                                    .FilterForCustomer(workContext.CurrentCustomer)
                                                    .RemoveDuplicatedQuantities());
                                            }
                                            //When there is just one tier (with  qty 1), 
                                            //there are no actual savings in the list.
                                            bool displayFromMessage = tierPrices.Count > 0 &&
                                                !(tierPrices.Count == 1 && tierPrices[0].Quantity <= 1);
                                            if (displayFromMessage)
                                            {
                                                priceModel.OldPrice = null;
                                                priceModel.Price = String.Format(localizationService.GetResource("Products.PriceRangeFrom"), priceFormatter.FormatPrice(finalPrice));
                                            }
                                            else
                                            {
                                                if (finalPriceBase != oldPriceBase && oldPriceBase != decimal.Zero)
                                                {
                                                    priceModel.OldPrice = priceFormatter.FormatPrice(oldPrice);
                                                    priceModel.Price = priceFormatter.FormatPrice(finalPrice);
                                                }
                                                else
                                                {
                                                    priceModel.OldPrice = null;
                                                    priceModel.Price = priceFormatter.FormatPrice(finalPrice);
                                                }
                                            }


                                            //property for German market
                                            //we display tax/shipping info only with "shipping enabled" for this product
                                            //we also ensure this it's not free shipping
                                            priceModel.DisplayTaxShippingInfo = catalogSettings.DisplayTaxShippingInfoProductBoxes
                                                && product.IsShipEnabled &&
                                                !product.IsFreeShipping;
                                        }
                                    }
                                }
                                else
                                {
                                    //hide prices
                                    priceModel.OldPrice = null;
                                    priceModel.Price = null;
                                }

                                #endregion
                            }
                            break;
                    }

                    model.ProductPrice = priceModel;

                    #endregion
                }

                //picture
                if (preparePictureModel)
                {
                    #region Prepare product picture

                    //If a size has been set in the view, we use it in priority
                    int pictureSize = productThumbPictureSize.HasValue ? productThumbPictureSize.Value : mediaSettings.ProductThumbPictureSize;
                    //prepare picture model
                    var defaultProductPictureCacheKey = string.Format(ModelCacheEventConsumer.PRODUCT_DEFAULTPICTURE_MODEL_KEY, product.Id, pictureSize, true, workContext.WorkingLanguage.Id, webHelper.IsCurrentConnectionSecured(), storeContext.CurrentStore.Id);
                    model.DefaultPictureModel = cacheManager.Get(defaultProductPictureCacheKey, () =>
                    {
                        var picture = pictureService.GetPicturesByProductId(product.Id, 1).FirstOrDefault();
                        var pictureModel = new PictureModel()
                        {
                            ImageUrl = pictureService.GetPictureUrl(picture, pictureSize),
                            FullSizeImageUrl = pictureService.GetPictureUrl(picture),
                            Title = string.Format(localizationService.GetResource("Media.Product.ImageLinkTitleFormat"), model.Name),
                            AlternateText = string.Format(localizationService.GetResource("Media.Product.ImageAlternateTextFormat"), model.Name)
                        };
                        return pictureModel;
                    });

                    #endregion
                }

                //specs
                if (prepareSpecificationAttributes)
                {
                    model.SpecificationAttributeModels = PrepareProductSpecificationModel(controller, workContext,
                         specificationAttributeService, cacheManager, product);
                }

                //reviews
                model.ReviewOverviewModel = new ProductReviewOverviewModel()
                {   
                    ProductId = product.Id,
                    RatingSum = product.ApprovedRatingSum,
                    TotalReviews = product.ApprovedTotalReviews,
                    AllowCustomerReviews = product.AllowCustomerReviews
                    
                };

                models.Add(model);
            }
            return models;
        }
    }
}