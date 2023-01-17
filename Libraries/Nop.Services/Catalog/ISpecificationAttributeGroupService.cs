using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.Catalog;

namespace Nop.Services.Catalog
{
    /// <summary>
    /// Specification attribute service interface
    /// </summary>
    public partial interface ISpecificationAttributeGroupService
    {

        #region Product specification attribute Group


        SpecificationAttributeGroup GetSpecificationGroupById(int id);

        void InsertSpecificationGroup(SpecificationAttributeGroup record);

        void UpdateSpecificationGroup(SpecificationAttributeGroup record);

        void DeleteSpecificationGroup(SpecificationAttributeGroup record);

        IPagedList<SpecificationAttributeGroup> GetAllSpecificationGroup(int pageIndex = 0, int pageSize = int.MaxValue);
        #endregion
    }
}
