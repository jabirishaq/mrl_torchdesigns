using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.Catalog;
using Nop.Services.Events;

namespace Nop.Services.Catalog
{
    /// <summary>
    /// SpecificationAttributeGroup Service
    /// </summary>
    public partial class SpecificationAttributeGroupService : ISpecificationAttributeGroupService
    {
      
        #region Fields
        
      
        private readonly IRepository<SpecificationAttributeGroup> _spacificationattributeGrouprepository;
      
        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="specificationAttributeRepository">Specification attribute repository</param>
        /// <param name="specificationAttributeOptionRepository">Specification attribute option repository</param>
        /// <param name="spacificationattributeGrouprepository">Product specification attribute repository</param>
        /// <param name="eventPublisher">Event published</param>
        public SpecificationAttributeGroupService(IRepository<SpecificationAttributeGroup> spacificationattributeGrouprepository)
        {
            _spacificationattributeGrouprepository = spacificationattributeGrouprepository;
         
        }

        #endregion

        #region Methods

    

        #region Product specification attribute Group

        /// <summary>
        /// Deletes a product specification attribute mapping
        /// </summary>
        /// <param name="specificationattributegroup">Product specification attribute</param>
        public virtual void DeleteSpecificationGroup(SpecificationAttributeGroup specificationattributegroup)
        {
            if (specificationattributegroup == null)
                throw new ArgumentNullException("specificationattributegroup");

            _spacificationattributeGrouprepository.Delete(specificationattributegroup);

        }


        public IPagedList<SpecificationAttributeGroup> GetAllSpecificationGroup(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = from a in _spacificationattributeGrouprepository.Table
                        orderby a.Id
                        select a;
            var records = new PagedList<SpecificationAttributeGroup>(query, pageIndex, pageSize);
            return records;
        }


        /// <summary>
        /// Gets a product specification attribute mapping 
        /// </summary>
        /// <param name="specificationattributegroupId">Product specification attribute mapping identifier</param>
        /// <returns>Product specification attribute mapping</returns>
        public virtual SpecificationAttributeGroup GetSpecificationGroupById(int specificationattributegroupId)
        {
            if (specificationattributegroupId == 0)
                return null;
            
            return _spacificationattributeGrouprepository.GetById(specificationattributegroupId);
        }

        /// <summary>
        /// Inserts a product specification attribute mapping
        /// </summary>
        /// <param name="specificationattributegroup">Product specification attribute mapping</param>
        public virtual void InsertSpecificationGroup(SpecificationAttributeGroup specificationattributegroup)
        {
            if (specificationattributegroup == null)
                throw new ArgumentNullException("specificationattributegroup");

            _spacificationattributeGrouprepository.Insert(specificationattributegroup);

          }

        /// <summary>
        /// Updates the product specification attribute mapping
        /// </summary>
        /// <param name="specificationattributegroup">Product specification attribute mapping</param>
        public virtual void UpdateSpecificationGroup(SpecificationAttributeGroup specificationattributegroup)
        {
            if (specificationattributegroup == null)
                throw new ArgumentNullException("specificationattributegroup");

            _spacificationattributeGrouprepository.Update(specificationattributegroup);

        }

        #endregion

        #endregion
    }
}
