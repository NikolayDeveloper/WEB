using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Migration.BusinessLogic.Models;
using Iserv.Niis.Migration.BusinessLogic.Utils;
using Iserv.Niis.Migration.Common;
using Iserv.OldNiis.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Iserv.Niis.Migration.Host
{
    public class MigrateAttachments
    {
        private readonly AttachmentFileHelper _attachmentFileHelper;
        private readonly AppConfiguration _appConfiguration;
        private readonly OldNiisFileContext _fileContext;

        private NiisWebContextMigration _context;

        public MigrateAttachments(AppConfiguration appConfiguration,
            AttachmentFileHelper attachmentFileHelper,
            OldNiisFileContext fileContext)
        {
            _attachmentFileHelper = attachmentFileHelper;
            _appConfiguration = appConfiguration;
            _fileContext = fileContext;

            OptionsBuilder = new DbContextOptionsBuilder<NiisWebContextMigration>();
            OptionsBuilder.UseSqlServer(_appConfiguration.NiisConnectionString);
        }

        #region Fields and Properties

        private static DbContextOptionsBuilder OptionsBuilder = null;


        private readonly string RequestDataFilePath = @"./LastMigratedAttachmentRequestId.txt";
        private readonly string ContractDataFilePath = @"./LastMigratedAttachmentContractId.txt";
        private readonly string DocumentDataFilePath = @"./LastMigratedAttachmentDocumentId.txt";
        private int LastMigratedRequestId => int.Parse(File.ReadAllText(RequestDataFilePath));
        private int LastMigratedContractId => int.Parse(File.ReadAllText(ContractDataFilePath));
        private int LastMigratedDocumentId => int.Parse(File.ReadAllText(DocumentDataFilePath));
        private static int MigratedFilesCount = 0;

        private static Stopwatch StopwatchRequestFiels = Stopwatch.StartNew();

        #endregion

        public void MigrateRequestAttachments()
        {
            int lastRequestId = 0;
            using (_context = new NiisWebContextMigration(OptionsBuilder.Options))
            {
                lastRequestId = _context.Requests.OrderByDescending(r => r.Id).First().Id;
            }

            while (lastRequestId > LastMigratedRequestId)
            {
                using (_context = new NiisWebContextMigration(OptionsBuilder.Options))
                {
                    var requests = _context.Requests
                        .Where(r => r.Id > LastMigratedRequestId && r.MainAttachmentId.HasValue == false)
                        .OrderBy(r => r.Id)
                        .Take(_appConfiguration.PackageSizeForFile)
                        .ToList();

                    if (requests.Any() == false)
                    {
                        break;
                    }

                    try
                    {
                        _attachmentFileHelper.AttachFilesToRequests(requests, _context, ref MigratedFilesCount);

                        File.WriteAllText(RequestDataFilePath, requests.OrderBy(r => r.Id).Last().Id.ToString());
                    }
                    catch (Exception ex)
                    {
                        var logErrorText = $"Ошибка при загрузки файлов для заявки с Id: {string.Join(",", requests.Select(r => r.Id))}. текст ошибки" + ex.Message;

                        Log.LogError(logErrorText);
                        Console.WriteLine(logErrorText);

                        if (ex.InnerException != null)
                            Console.WriteLine(ex.InnerException);
                    }

                    Console.Write($"\rМигрировано {MigratedFilesCount} файлов. Затрачено времени: {StopwatchRequestFiels.Elapsed}");
                }
            }
        }

        public void MigrateContractAttachments()
        {
            int lastContractId = 0;
            MigratedFilesCount = 0;
            using (_context = new NiisWebContextMigration(OptionsBuilder.Options))
            {
                lastContractId = _context.Contracts.OrderByDescending(r => r.Id).First().Id;
            }

            while (lastContractId > LastMigratedContractId)
            {
                using (var _context = new NiisWebContextMigration(OptionsBuilder.Options))
                {
                    var contracts = _context.Contracts
                        .Where(r => r.Id > LastMigratedContractId && r.MainAttachmentId.HasValue == false)
                        .OrderBy(r => r.Id)
                        .Take(_appConfiguration.PackageSizeForFile)
                        .ToList();

                    if (contracts.Any() == false)
                    {
                        break;
                    }

                    foreach (var contract in contracts)
                    {
                        try
                        {
                            var mainAttachmentId = _attachmentFileHelper.GetMainAttachmentId(contract.Id, _context);
                            if (mainAttachmentId.HasValue == true)
                            {
                                contract.MainAttachmentId = mainAttachmentId;
                                _context.SaveChanges();

                                MigratedFilesCount++;
                            }

                            File.WriteAllText(ContractDataFilePath, contracts.Last().Id.ToString());
                        }
                        catch (Exception ex)
                        {
                            var logErrorText = $"Ошибка при загрузки файла для договоров с Id: {contract.Id}. текст ошибки" + ex.Message;

                            Log.LogError(logErrorText);
                            Console.WriteLine(logErrorText);

                            if (ex.InnerException != null)
                                Console.WriteLine(ex.InnerException);
                        }
                    }
                    Console.Write($"\rМигрировано {MigratedFilesCount} файлов. Затрачено времени: {StopwatchRequestFiels.Elapsed}");

                }
            }
        }

        public void MigrateDocumentAttachments()
        {
            int lastDocumentId = 0;
            MigratedFilesCount = 0;
            using (_context = new NiisWebContextMigration(OptionsBuilder.Options))
            {
                lastDocumentId = _context.Documents.OrderByDescending(r => r.Id).First().Id;
            }

            while (lastDocumentId > LastMigratedDocumentId)
            {
                using (var _context = new NiisWebContextMigration(OptionsBuilder.Options))
                {
                    var documents = _context.Documents
                        .Where(r => r.Id > LastMigratedDocumentId && r.MainAttachmentId.HasValue == false)
                        .OrderBy(r => r.Id)
                        .Take(_appConfiguration.PackageSizeForFile)
                        .ToList();

                    if (documents.Any() == false)
                    {
                        break;
                    }

                    try
                    {

                        _attachmentFileHelper.AttachFilesToDocuments(documents, _context, ref MigratedFilesCount);

                        File.WriteAllText(DocumentDataFilePath, documents.Last().Id.ToString());
                    }
                    catch (Exception ex)
                    {
                        var logErrorText = $"Ошибка при загрузки файла для документов с Id: {string.Join(",", documents.Select(r => r.Id))}. текст ошибки" + ex.Message;

                        Log.LogError(logErrorText);
                        Console.WriteLine(logErrorText);

                        Console.WriteLine(ex.Message + ex.StackTrace);
                        if (ex.InnerException != null)
                            Console.WriteLine(ex.InnerException.Message + ex.StackTrace);
                    }

                    Console.Write($"\rМигрировано {MigratedFilesCount} файлов. Затрачено времени: {StopwatchRequestFiels.Elapsed}");
                }
            }
        }
    }
}
