namespace Nop.Core.Domain.Orders
{
    /// <summary>
    /// Represents an order status enumeration
    /// </summary>
    public enum OrderStatus : int
    {
        /// <summary>
        /// Pending
        /// </summary>
        Pending = 10,
        /// <summary>
        /// Processing
        /// </summary>
        Processing = 20,
        /// <summary>
        /// Complete
        /// </summary>
        Complete = 30,
        /// <summary>
        /// Cancelled
        /// </summary>
        Cancelled = 40,
         /// <summary>
        /// Transmitted to Great Plains
        /// </summary>
        Transmitted = 25
    }
}
