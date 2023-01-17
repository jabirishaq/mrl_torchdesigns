using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Plugin.Widgets.TorchDesign_StoreLocator.Domain;

namespace Nop.Plugin.Widgets.TorchDesign_StoreLocator.Services
{
    public partial class StoreLocatorService : IStoreLocatorService
    {
         private readonly IRepository<StoreLocator> _StorelocatorRepository;


         public StoreLocatorService(IRepository<StoreLocator> StorelocatorRepository)
        {
            this._StorelocatorRepository = StorelocatorRepository;
           
        }

         public StoreLocator GetStoreLocationById(int id)
        {
            if (id == 0)
                return null;
            return _StorelocatorRepository.GetById(id);
        }

         public void InsertStoreLocation(StoreLocator record)
        {
            if (record == null)
                throw new ArgumentNullException("record");

            _StorelocatorRepository.Insert(record);
        }

        public void UpdateStoreLocation(StoreLocator record)
        {
            if (record == null)
                throw new ArgumentNullException("record");

            _StorelocatorRepository.Update(record);
        }

        public void DeleteStoreLocation(StoreLocator record)
        {
            if (record == null)
                throw new ArgumentNullException("record");
            _StorelocatorRepository.Delete(record);
        }

        public IPagedList<StoreLocator> GetAllStoreLocations(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = from a in _StorelocatorRepository.Table
                        orderby a.Id
                        select a;
            var records = new PagedList<StoreLocator>(query, pageIndex, pageSize);
            return records;
        }

        ////public virtual IList<StoreLocator> G(int id)
        ////{
        ////    var query = from stores in _StorelocatorRepository.Table
        ////                where stores.Id == id
        ////                select stores;

        ////    return query.ToList();
        ////}
      
    }
}
