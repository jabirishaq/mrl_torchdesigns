using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.Catalog;

namespace Nop.Services.Catalog
{
    /// <summary>
    /// Category service interface
    /// </summary>
    public partial interface IQrCodeService
    {
        QrCode GetQrCodeById(int id);

       void InsertQrCode(QrCode record);

        void UpdateQrCode(QrCode record);

        void DeleteQrCode(QrCode record);

        IPagedList<QrCode> GetAllQrCodeWithotCount(int pageIndex = 0, int pageSize = int.MaxValue);

        IPagedList<QrCodeDiaplay> GetAllQrCode(int pageIndex = 0, int pageSize = int.MaxValue);
       
    }
}
