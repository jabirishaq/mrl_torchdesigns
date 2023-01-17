using System;
using System.Linq;
using Nop.Core.Domain.Orders;
using Nop.Plugin.TorchDesigns.DynamicsGPWCF.DynamicsGP;

namespace Nop.Plugin.TorchDesigns.DynamicsGPWCF
{
    public static class GPProductList
    {
        public static SalesOrderLine[] GetProductList(Order oNopOrder, SalesOrder oGpSalesOrder)
        {
            SalesOrderLine[] oProductList = new SalesOrderLine[oNopOrder.OrderItems.Count()];
            int i = 0;

            foreach (OrderItem p in oNopOrder.OrderItems)
            {
                // Description
                oProductList[i] = new SalesOrderLine();
                
                // Key
                oProductList[i].ItemKey = new ItemKey();
                oProductList[i].ItemKey.Id = p.Product.Sku;

                // Item Price
                oProductList[i].UnitPrice = new MoneyAmount();
                oProductList[i].UnitPrice.DecimalDigits = 2;  // TODO Configurable
                oProductList[i].UnitPrice.Currency = "USD"; // TODO Configurable

                // Discount
                if (p.DiscountAmountExclTax != decimal.Zero) // With Discount
                {
                    oProductList[i].Discount = new MoneyPercentChoice();
                    oProductList[i].Discount.Item = new MoneyAmount();
                    ((MoneyAmount)oProductList[i].Discount.Item).Currency = "USD";
                    ((MoneyAmount)oProductList[i].Discount.Item).DecimalDigits = 2;
                    ((MoneyAmount)oProductList[i].Discount.Item).Value = Decimal.Round(p.DiscountAmountExclTax,2);
                    oProductList[i].UnitPrice.Value = Decimal.Round(p.PriceExclTax + p.DiscountAmountExclTax,2);
                }
                else // Without Discount
                {
                    oProductList[i].UnitPrice.Value = p.PriceExclTax;
                }

                // Quantity
                oProductList[i].Quantity = new Quantity();
                oProductList[i].Quantity.Value = p.Quantity;

                // Shipping (Set these equal to the sales document)
                oProductList[i].ShippingMethodKey = oGpSalesOrder.ShippingMethodKey;
                oProductList[i].ShipToAddressKey = oGpSalesOrder.ShipToAddressKey;
                //oProductList[i].RequestedShipDate =  new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);
                //oProductList[i].ActualShipDate= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);
                //oProductList[i].FulfillDate= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);
                
                i++;
            }

            return oProductList;
        }
    }
}
