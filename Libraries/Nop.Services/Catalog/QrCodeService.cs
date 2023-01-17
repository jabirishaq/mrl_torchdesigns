using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Stores;
using Nop.Services.Events;
using Nop.Services.Security;
using Nop.Services.Stores;

namespace Nop.Services.Catalog
{
    /// <summary>
    /// Category service
    /// </summary>
    public partial class QrCodeService : IQrCodeService
    {
        

        #region Fields
        private readonly IRepository<QrCode> _qrcodeRepository;
        #endregion
        
        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
   
        /// <param name="catalogSettings">QrCode</param>
        public QrCodeService(IRepository<QrCode> qrcodeRepository)
        {
            this._qrcodeRepository = qrcodeRepository;
        }

        #endregion

        #region Methods

        public QrCode GetQrCodeById(int id)
        {
            if (id == 0)
                return null;
            return _qrcodeRepository.GetById(id);
        }

     
        public void InsertQrCode(QrCode record)
        {
            if (record == null)
                throw new ArgumentNullException("record");

            _qrcodeRepository.Insert(record);
        }

        public void UpdateQrCode(QrCode record)
        {
            if (record == null)
                throw new ArgumentNullException("record");

            _qrcodeRepository.Update(record);
        }

        public void DeleteQrCode(QrCode record)
        {
            if (record == null)
                throw new ArgumentNullException("record");
            _qrcodeRepository.Delete(record);
        }

        public IPagedList<QrCodeDiaplay> GetAllQrCode(int pageIndex = 0, int pageSize = int.MaxValue)
        {


            var query = from a in _qrcodeRepository.Table
                        group a by a.QrCodeName into g
                        select new QrCodeDiaplay()
                        {
                            Count = g.Count(),
                            QrCodeName = g.Key,
                            id=g.FirstOrDefault().Id,
                            QrCodeUrl= g.FirstOrDefault().QrCodeUrl
                        };
            query = query.OrderBy(x => x.QrCodeName);
          
            var records = new PagedList<QrCodeDiaplay>(query, pageIndex, pageSize);
            return records;
        }

        public IPagedList<QrCode> GetAllQrCodeWithotCount(int pageIndex = 0, int pageSize = int.MaxValue)
        {

            var query = from pt in _qrcodeRepository.Table
                        orderby pt.Id
                        select pt;
            var records = new PagedList<QrCode>(query, pageIndex, pageSize);
            return records;
        }

    
        #endregion
    }
}
