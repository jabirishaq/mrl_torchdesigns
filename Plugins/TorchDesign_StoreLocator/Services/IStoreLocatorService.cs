using Nop.Core;
using Nop.Plugin.Widgets.TorchDesign_StoreLocator.Domain;
using System.Collections.Generic;

namespace Nop.Plugin.Widgets.TorchDesign_StoreLocator.Services
{
    public partial interface IStoreLocatorService
    {

        StoreLocator GetStoreLocationById(int id);

        void InsertStoreLocation(StoreLocator record);

        void UpdateStoreLocation(StoreLocator record);

        void DeleteStoreLocation(StoreLocator record);

        IPagedList<StoreLocator> GetAllStoreLocations(int pageIndex = 0, int pageSize = int.MaxValue);

    }
}
