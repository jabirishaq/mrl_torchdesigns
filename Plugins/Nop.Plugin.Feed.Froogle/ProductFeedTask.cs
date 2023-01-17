using Nop.Core.Domain.Stores;
using Nop.Core.Plugins;
using Nop.Services.Stores;
using Nop.Services.Tasks;
using System;
using System.Collections.Generic;

namespace Nop.Plugin.Feed.Froogle
{
    /// <summary>
    /// Represents a task sending product to google shopping
    /// </summary>
    public partial class ProductFeedTask : ITask
    {
        private readonly IPluginFinder _pluginFinder;
        private readonly IStoreService _storeService;
        private readonly FroogleSettings _froogleSettings;
        public ProductFeedTask(IPluginFinder pluginFinder, IStoreService storeService,FroogleSettings froogleSettings)
        {
            this._pluginFinder = pluginFinder;
            this._storeService = storeService;
            this._froogleSettings = froogleSettings;
        }

        /// <summary>
        /// Executes a task
        /// </summary>
        public void Execute()
        {
            var pluginDescriptor = _pluginFinder.GetPluginDescriptorBySystemName("PromotionFeed.Froogle");
            if (pluginDescriptor == null)
                throw new Exception("Cannot load the plugin");

            //plugin
            var plugin = pluginDescriptor.Instance() as FroogleService;
            if (plugin == null)
                throw new Exception("Cannot load the plugin");

            var stores = new List<Store>();
            var storeById = _storeService.GetStoreById(_froogleSettings.StoreId);
            if (storeById != null)
                stores.Add(storeById);
            else
                stores.AddRange(_storeService.GetAllStores());

            foreach (var store in stores)
                plugin.GenerateStaticFile(store);
        }
    }
}
