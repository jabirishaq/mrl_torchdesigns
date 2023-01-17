using System.Data.Entity;
using Nop.Core.Infrastructure;

namespace Nop.Plugin.Widgets.TorchDesign_StoreLocator.Data
{
    public class EfStartUpTask : IStartupTask
    {
        public void Execute()
        {
            //It's required to set initializer to null (for SQL Server Compact).
            //otherwise, you'll get something like "The model backing the 'your context name' context has changed since the database was created. Consider using Code First Migrations to update the database"
            Database.SetInitializer<StoreLocatorObjctContext>(null);
           
        }

        public int Order
        {
            //ensure that this task is run first 
            get { return 0; }
        }
    }
}
