using Iserv.Niis.Migration.BusinessLogic.Abstract;
using Iserv.Niis.Migration.BusinessLogic.Models;
using Iserv.OldNiis.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Iserv.Niis.Migration.BusinessLogic.Services.OldNiis
{
    public class OldNiisDocumentDataService : BaseService
    {
        private readonly OldNiisFileContext _fileContext;

        public OldNiisDocumentDataService(OldNiisFileContext fileContext)
        {
            _fileContext = fileContext;
            _fileContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public AttachmentFileInfo GetDocumentData(int documentId)
        {
            return _fileContext.DdDocumentDatas
                .Where(d => d.U_ID == documentId && d.ANY_DATA != null)
                .Select(d => new AttachmentFileInfo
                {
                    Id = (int)d.U_ID,
                    DateCreate = d.date_create ?? DateTimeOffset.Now,
                    DateUpdate = d.stamp ?? DateTimeOffset.Now,
                    FileName = d.SYS_ANY_DATA,
                    File = d.ANY_DATA
                })
                .SingleOrDefault();
        }

        public List<AttachmentFileInfo> GetDdDocumentDatas(List<int> docIds)
        {
            return _fileContext.DdDocumentDatas
                .AsNoTracking()
                .Where(d => docIds.Contains((int)d.U_ID) && d.ANY_DATA != null && d.ANY_DATA.Length > 0)
                .Select(d => new AttachmentFileInfo
                {
                    Id = (int)d.U_ID,
                    DateCreate = d.date_create ?? DateTimeOffset.Now,
                    DateUpdate = d.stamp ?? DateTimeOffset.Now,
                    FileName = d.SYS_ANY_DATA,
                    File = d.ANY_DATA
                })
                .ToList();
        }
    }
}
