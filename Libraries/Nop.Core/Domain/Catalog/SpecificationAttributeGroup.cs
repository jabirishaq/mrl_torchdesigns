namespace Nop.Core.Domain.Catalog
{
    /// <summary>
    /// Represents a related product
    /// </summary>
    public partial class SpecificationAttributeGroup : BaseEntity
    {
        /// <summary>
        /// Gets or sets the name of group 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }
    }

}
