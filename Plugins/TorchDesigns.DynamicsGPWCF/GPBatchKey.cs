using System;
using Nop.Plugin.TorchDesigns.DynamicsGPWCF.DynamicsGP;


namespace Nop.Plugin.TorchDesigns.DynamicsGPWCF
{
    public static class GPBatchKey
    {
        /// <summary>
        /// Creates a Great Plains BatchKey based on today's date
        /// </summary>
        /// <returns>Great Plains BatchKey</returns>
        public static BatchKey GetBatchKey()
        {
            // Init
            BatchKey oBatchKey = new BatchKey();
            int iMonth = DateTime.Now.Month;
            int iDay = DateTime.Now.Day;
            int iYear = DateTime.Now.Year;

            // Build Batch Key
            //string strBatchKey = "NOP" + iYear.ToString().Trim() + iMonth.ToString().Trim() + iDay.ToString().Trim(); // TODO: Configurable Prefix
            string strBatchKey = "NEW ORDER";

            // Set Batch Key
            oBatchKey.Id = strBatchKey;
            oBatchKey.CreatedDateTime = DateTime.Now.Date;
            oBatchKey.Source = "SALES TRANSACTION ENTRY";

            return oBatchKey;
        }
    }
}
