namespace Nop.Core.Domain.Orders
{
    /// <summary>
    /// Represents an "order by country" report line
    /// </summary>
    public partial class OrderByStateReportLine
    {
        /// <summary>
        /// Country identifier; null for unknow country
        /// </summary>
        public int? StateProvinceId { get; set; }

        /// <summary>
        /// Gets or sets the number of orders
        /// </summary>
        public int TotalOrders { get; set; }

        /// <summary>
        /// Gets or sets the order total summary
        /// </summary>
        public decimal SumOrders { get; set; }
    }
}
