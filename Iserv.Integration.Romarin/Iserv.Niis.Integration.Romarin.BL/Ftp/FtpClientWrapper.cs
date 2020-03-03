using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FluentFTP;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Business.Implementations;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Integration;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Integration.Romarin.BL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Core;
using Serilog.Core.Enrichers;
using Serilog.Sinks.MSSqlServer;

namespace Iserv.Niis.Integration.Romarin.BL.Ftp
{
    public class FtpClientWrapper
    {
        private const string KZ = "KZ";
        private const string formatyyyyMMdd = "yyyyMMdd";
        private readonly string _connectionString;
        private readonly FtpClient _ftpClient;
        private readonly int _mainExecutorId;
        private readonly string _path;

        private NiisWebContext _context;
        private INumberGenerator _numberGenerator;
        private ZipArchive _zipArchive;

        public FtpClientWrapper(string host, string userId, string password, string path, string connectionString,
            int mainExecutorId, string logConnectionString = null)
        {
            _ftpClient = new FtpClient(host, new NetworkCredential(userId, password));
            _path = path;
            _connectionString = connectionString;
            var columnOptions = new ColumnOptions();
            _mainExecutorId = mainExecutorId;
            Log.Logger = new LoggerConfiguration()
                .WriteTo.MSSqlServer(logConnectionString ?? connectionString, "IntegrationRomarinLog",
                    columnOptions: columnOptions, autoCreateSqlTable: true)
                .CreateLogger();
        }

        public void ProcessFiles()
        {
            //try
            //{

            #region Process File

            InitContext();

            Debug.WriteLine("Begin: " + DateTime.Now);
            try
            {
                _ftpClient.Connect();
                Log.Information("Соединение с Ftp сервером");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                Log.Error(e, "Соединение с Ftp сервером. Ошибка");
                Log.CloseAndFlush();
                throw;
            }

            foreach (var item in _ftpClient.GetListing(_path).OrderBy(f => f.Created))
            {
                Debug.WriteLine("Iteration start: " + DateTime.Now);

                // if this is a file
                if (item.Type == FtpFileSystemObjectType.File)
                {
                    if (_context.IntegrationRomarinFiles.Any(irf => irf.FileName == item.Name))
                    {
                        continue;
                    }

                    var outStream = new MemoryStream();

                    var isDownload = false;
                    // Скачивание архива
                    for (var i = 0; i < 3; i++)
                    {
                        try
                        {
                            Debug.WriteLine("Download Zip start: " + DateTime.Now);
                            isDownload = _ftpClient.Download(outStream, item.FullName);
                            Debug.WriteLine("Download Zip end: " + DateTime.Now);

                            Log.ForContext(new ILogEventEnricher[]
                            {
                                new PropertyEnricher("File", item.Name),
                                new PropertyEnricher("Type", IntegrationLogType.File),
                                new PropertyEnricher("Status", IntegrationConst.Statuses.Downloaded)
                            }).Information("Файл {File} скачен", item.Name);
                            break;
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e.ToString());

                            Log.ForContext(new ILogEventEnricher[]
                            {
                                new PropertyEnricher("File", item.Name),
                                new PropertyEnricher("Status", IntegrationConst.Statuses.Error)
                            }).Error(e, "Скачивания файла {File}. Ошибка ", item.FullName);
                            isDownload = false;
                            Thread.Sleep(2000);
                        }
                    }

                    if (isDownload)
                    {
                        // найти xml
                        _zipArchive = new ZipArchive(outStream, ZipArchiveMode.Read);
                        var xmlEntry = _zipArchive.Entries.FirstOrDefault(ent => ent.Name.EndsWith(".xml"));
                        if (xmlEntry != null)
                        {
                            var xmlStream = xmlEntry.Open();

                            var serializer = new XmlSerializer(typeof(RomarinDto));
                            var romarinDto = (RomarinDto) serializer.Deserialize(xmlStream);

                            // Сохранить МТЗ
                            var operationCount = 0;
                            foreach (var markgr in romarinDto.MARKGRS)
                            {
                                Log.Logger = Log.ForContext("File", item.Name);
                                ProcessItem(markgr, null, _context);

                                if (++operationCount >= 500)
                                {
                                    operationCount = 0;
                                    _context.SaveChanges();
                                    InitContext();
                                }
                            }

                            _context.IntegrationRomarinFiles.Add(new IntegrationRomarinFile
                            {
                                FileName = item.Name,
                                TimeStamp = DateTimeOffset.Now,
                                Status = IntegrationConst.Statuses.Done
                            });
                            _context.SaveChanges();
                            InitContext();

                            Log.ForContext(new[]
                            {
                                new PropertyEnricher("File", item.Name),
                                new PropertyEnricher("Type", IntegrationLogType.File),
                                new PropertyEnricher("Status", IntegrationConst.Statuses.Done)
                            }).Information("Файл {File} импортирован");
                        }
                    }
                    else
                    {
                        Log.ForContext(new ILogEventEnricher[]
                        {
                            new PropertyEnricher("File", item.Name),
                            new PropertyEnricher("Status", IntegrationConst.Statuses.Error)
                        }).Warning("Файл {File} не скачен", item.Name);
                    }
                }

                Debug.WriteLine("Iteration end: " + DateTime.Now);
            }

            _ftpClient.Disconnect();
            Debug.WriteLine("End: " + DateTime.Now);

            #endregion

            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.ToString());
            //    Log.Error(e, "ошибка");
            //}
            _context.SaveChanges();
            Log.CloseAndFlush();
        }

        /// <summary>
        ///     Обработка информации о МТЗ
        /// </summary>
        /// <param name="markgr">Данные из xml</param>
        /// <param name="loopState"></param>
        /// <param name="localContext">Конекст БД</param>
        /// <returns></returns>
        private NiisWebContext ProcessItem(MARKGR markgr, ParallelLoopState loopState, NiisWebContext localContext)
        {
            //foreach (MARKGR markgr in romarinDto.MARKGRS)
            //{

            // Only for KZ
            var isKz = markgr.EXN?.Any(e =>
                e.DESPG != null && e.DESPG.DCPCD.Contains(KZ) || e.DESPG2 != null && e.DESPG2.DCPCD.Contains(KZ));
            isKz = isKz.HasValue && isKz.Value || markgr.LIN != null && markgr.LIN.Any(l => l.DCPCD.Contains(KZ));
            if (!isKz.Value)
            {
                return localContext;
            }

            var protectionDocIcfems = new List<DicIcfemProtectionDocRelation>();
            var protectionDocIcgs = new List<ICGSProtectionDoc>();

            // Получить статус
            // Охрана
            var statusCode = "D.TZ";
            if (markgr.RFNP.Any() || markgr.RFNT.Any() || markgr.EXN != null && markgr.EXN.Any() || markgr.ENN.Any())
            {
                //Обработка заявки
                statusCode = "W";
            }

            if (markgr.R18NT.Any())
            {
                // Окончательный отказ
                statusCode = "03.10";
            }

            // Заявка
            var request = localContext.Requests.FirstOrDefault(r => r.RequestNum == markgr.INTREGN.Trim())
                          ?? new Request
                          {
                              Id = 0,
                              DateCreate = DateTimeOffset.Now,
                              UserId = _mainExecutorId
                          };

            if (request.Id == 0)
            {
                localContext.Requests.Add(request);
            }
            else
            {
                localContext.Requests.Update(request);
            }

            //try
            //{

            #region Convert Data

            // 2 (21) Регистрационный номер заявки
            LogMsg(request.RequestNum, markgr.INTREGN.Trim(),
                $"(21) Регистрационный номер заявки {markgr.INTREGN.Trim()}");
            request.RequestNum = markgr.INTREGN.Trim();

            // 1 (54) Название на иностранном языке
            LogMsg(request.NameEn, markgr.CURRENT?.IMAGE.TEXT?.Trim(),
                $"(54) Название на иностранном языке Заявка № {request.RequestNum}");
            request.NameEn = markgr.CURRENT?.IMAGE.TEXT?.Trim();


            // 3 (22) Дата подачи заявки
            LogMsg(request.RequestDate?.ToString(), DateFromString(markgr.INTREGD)?.ToString(),
                $"(22) Дата подачи заявки Заявка № {request.RequestNum}");
            request.RequestDate = DateFromString(markgr.INTREGD);

            // 5 (891) Дата распространения на РК
            var exn = markgr.EXN?.FirstOrDefault(e => e.DESPG?.DCPCD != null && e.DESPG.DCPCD.Contains(KZ)
                                                      || e.DESPG2?.DCPCD != null && e.DESPG2.DCPCD.Contains(KZ));
            LogMsg(request.DistributionDate?.ToString(), DateFromString(markgr.INTREGD)?.ToString(),
                $"(891) Дата распространения на РК Заявка № {request.RequestNum}");
            request.DistributionDate = DateFromString(exn?.REGEDAT);

            // 6 Патентообладатель
            if (markgr.CURRENT?.HOLGR?.ADDRESS != null)
            {
                ProcessPatentHolder(localContext, markgr, request);
            }

            if (markgr.CURRENT != null)
            {
                // 7 Изображение
                ProcessImage(localContext, markgr, request);

                // 8 Транслитерация
                LogMsg(request.Transliteration, markgr.CURRENT.MARTRAN?.Trim(),
                    $"Транслитерация Заявка № {request.RequestNum}");
                request.Transliteration = markgr.CURRENT.MARTRAN?.Trim();

                // 9 МКИЭТЗ
                if (markgr.CURRENT.VIENNAGR != null)
                {
                    // только детальный уровень
                    ProcessIcfem(localContext, markgr, request, statusCode, protectionDocIcfems);
                }

                // 10 МКТУ
                if (markgr.CURRENT.BASICGS != null)
                {
                    ProcessIcgs(localContext, markgr, request, statusCode, protectionDocIcgs);
                }
            }

            // 11 (511) МКТУ- Описание отказа по классу 
            if (markgr.LIN != null && markgr.LIN.Any(l => l.DCPCD.Contains(KZ)))
            {
                ProcessIcgsRefuse(localContext, markgr, request, statusCode, protectionDocIcgs);
            }

            if (markgr.CURRENT != null)
            {
                // 12 (591) Цвета 
                if (markgr.CURRENT.COLCLAGR != null)
                {
                    var colors = new[]
                    {
                        markgr.CURRENT.COLCLAGR.COLCLAEN, markgr.CURRENT.COLCLAGR.COLCLAES,
                        markgr.CURRENT.COLCLAGR.COLCLAFR
                    }.FirstOrDefault(ls => ls != null);

                    var colorStr = string.Empty;
                    foreach (var color in colors)
                    {
                        colorStr += color.Trim() + " ";
                    }

                    LogMsg(request.RomarinColor, colorStr, $"(591) Цвета Заявка № {request.RequestNum}");
                    request.RomarinColor = colorStr;
                }

                // 13 Приоритетные данные
                if (markgr.CURRENT.PRIGR != null)
                {
                    var country = localContext.DicCountries.FirstOrDefault(dc =>
                        dc.Code.Trim().ToLower() == markgr.CURRENT.PRIGR.PRICP.Trim().ToLower());
                    if (country == null)
                    {
                        Log.Logger.Error("Приоритетные данные. Страна не найдена: {0}. {1}",
                            markgr.CURRENT.PRIGR.PRICP.Trim().ToLower(), $"Заявка № {request.RequestNum}");
                    }
                    else
                    {
                        var dicEarlyType = localContext.DicEarlyRegTypes.FirstOrDefault(dr => dr.Code == "30 - 300");
                        var requestEarly = localContext.RequestEarlyRegs.FirstOrDefault(rer =>
                                               rer.RegNumber == markgr.CURRENT.PRIGR.PRIAPPD.Trim())
                                           ?? new RequestEarlyReg
                                           {
                                               Id = 0,
                                               DateCreate = DateTimeOffset.Now,
                                               RegNumber = markgr.CURRENT.PRIGR.PRIAPPD.Trim(),
                                               EarlyRegType = dicEarlyType
                                           };
                        if (requestEarly.Id == 0)
                        {
                            localContext.RequestEarlyRegs.Add(requestEarly);
                        }
                        else
                        {
                            localContext.RequestEarlyRegs.Update(requestEarly);
                        }

                        requestEarly.RegCountry = country;
                        requestEarly.RegCountryId = country.Id;
                        //requestEarly.PriorityDate = DateFromString(markgr.CURRENT.PRIGR?.PRIAPPD);
                        requestEarly.Request = request;

                        var priorityData = new[]
                        {
                            markgr.CURRENT.PRIGR?.TEXTEN,
                            markgr.CURRENT.PRIGR?.TEXTES,
                            markgr.CURRENT.PRIGR?.TEXTFR
                        }.FirstOrDefault(txn => !string.IsNullOrEmpty(txn));

                        LogMsg(requestEarly.ITMRawPriorityData, priorityData,
                            $"Приоритетные данные Заявка № {request.RequestNum}");
                        requestEarly.ITMRawPriorityData = priorityData;
                    }
                }

                // 14 (526) Дискламация
                var disclaimerRu = new[]
                {
                    markgr.CURRENT.DISCLAIMGR?.DISCLAIMEREN,
                    markgr.CURRENT.DISCLAIMGR?.DISCLAIMERES,
                    markgr.CURRENT.DISCLAIMGR?.DISCLAIMERFR
                }.FirstOrDefault(str => !string.IsNullOrEmpty(str));

                LogMsg(request.DisclaimerRu, disclaimerRu, $"Дискламация Заявка № {request.RequestNum}");
                request.DisclaimerRu = disclaimerRu;
            }

            // 15 Статус
            request.Status = localContext.DicRequestStatuses.FirstOrDefault(drs => drs.Code == statusCode);
            LogMsg(null, request.Status?.NameRu, $"Статус Заявка № {request.RequestNum}");

            // 16 Тип ОД
            var protectionDocType = localContext.DicProtectionDocTypes.FirstOrDefault(t => t.Code == "ITM");
            request.ProtectionDocType = protectionDocType;

            // Охранный документ
            if (statusCode == "D.TZ")
            {
                //todo! Данный статус заявки в системе в данный момент не используется, не понятно как должно работать
                /*var protectionDoc =
                    localContext.ProtectionDocs.FirstOrDefault(r => r.GosNumber == markgr.INTREGN.Trim())
                    ?? new ProtectionDoc
                    {
                        DateCreate = DateTimeOffset.Now
                    };
                protectionDoc.Request = request;

                protectionDoc.NameEn = request.NameEn;
                protectionDoc.GosNumber = request.RequestNum;
                protectionDoc.GosDate = request.RequestDate;
                // 4 (180) Срок действия ОД
                LogMsg(protectionDoc.ValidDate?.ToString(), DateFromString(markgr.EXPDATE)?.ToString(),
                    $"(180) Срок действия ОД Заявка № {request.RequestNum}");
                protectionDoc.ValidDate = DateFromString(markgr.EXPDATE);
                protectionDoc.DistributionDate = request.DistributionDate;
                protectionDoc.Transliteration = request.Transliteration;
                protectionDoc.Type = request.ProtectionDocType;
                protectionDoc.DisclaimerRu = request.DisclaimerRu;

                protectionDoc.Icfems = new List<DicIcfemProtectionDocRelation>();
                foreach (var icfem in protectionDocIcfems)
                {
                    if (localContext.DicIcfemProtectionDocRelations.All(icf =>
                        icf.DicIcfemId != icfem.DicIcfemId && icf.ProtectionDocId != protectionDoc.Id))
                    {
                        protectionDoc.Icfems.Add(icfem);
                    }
                }

                protectionDoc.IcgsProtectionDocs = new List<ICGSProtectionDoc>();
                foreach (var icgspd in protectionDocIcgs)
                {
                    if (localContext.ICGSProtectionDocs.All(icf =>
                        icf.IcgsId != icgspd.Id && icf.ProtectionDocId != protectionDoc.Id))
                    {
                        protectionDoc.IcgsProtectionDocs.Add(icgspd);
                    }
                }*/
            }

            //
            if (int.TryParse(request.RequestNum, out var reqNumber))
            {
                if (reqNumber >= 1316814)
                {
                    // Ожидание срока рассмотрения заявки
                    var dicRouteStage = localContext.DicRouteStages.FirstOrDefault(dr => dr.Code == "TMI03.1");
                    // Начальник управления экспертизы международных заявок на товарные знаки
                    if (dicRouteStage != null)
                    {
                        var requestWorkflow =
                            localContext.RequestWorkflows.FirstOrDefault(rw => rw.OwnerId == request.Id);
                        if (requestWorkflow == null)
                        {
                            var workFlow = new RequestWorkflow
                            {
                                CurrentStageId = dicRouteStage.Id,
                                DateCreate = DateTimeOffset.Now,
                                CurrentUserId = _mainExecutorId,
                                RouteId = dicRouteStage.RouteId,
                                IsComplete = dicRouteStage.IsLast,
                                IsSystem = dicRouteStage.IsSystem,
                                IsMain = dicRouteStage.IsMain,
                                Owner = request
                            };
                            localContext.RequestWorkflows.Add(workFlow);
                            localContext.SaveChanges();
                            request.CurrentWorkflowId = workFlow.Id;
                        }
                    }
                }
                else
                {
                    // Публикация ВОИС
                    var dicRouteStage = localContext.DicRouteStages.FirstOrDefault(dr => dr.Code == "TMI03.3.4.5");
                    if (dicRouteStage != null)
                    {
                        var requestWorkflow =
                            localContext.RequestWorkflows.FirstOrDefault(rw => rw.OwnerId == request.Id);
                        if (requestWorkflow == null)
                        {
                            var workFlow = new RequestWorkflow
                            {
                                CurrentStageId = dicRouteStage.Id,
                                DateCreate = DateTimeOffset.Now,
                                CurrentUserId = _mainExecutorId,
                                RouteId = dicRouteStage.RouteId,
                                IsComplete = dicRouteStage.IsLast,
                                IsSystem = dicRouteStage.IsSystem,
                                IsMain = dicRouteStage.IsMain,
                                Owner = request
                            };
                            localContext.RequestWorkflows.Add(workFlow);
                            localContext.SaveChanges();
                            request.CurrentWorkflowId = workFlow.Id;
                        }
                    }

                    // Начальник управления экспертизы международных заявок на товарные знаки
                }
            }

            // 17 By office
            request.AdditionalDocs = new List<AdditionalDoc>();

            // 17.1 Registration
            if (markgr.ENN != null)
            {
                ProcessAdditionalDocs(markgr.ENN, AdditionalDocType.Registration, localContext, ref request);
            }

            // 17.2 Subsequent designation
            if (markgr.EXN != null)
            {
                ProcessAdditionalDocs(markgr.EXN, AdditionalDocType.SubsequentDesignation, localContext, ref request);
            }

            // 17.3 Continuation of effect
            if (markgr.CENS != null)
            {
                ProcessAdditionalDocs(markgr.CENS, AdditionalDocType.ContinuationOfEffect, localContext, ref request);
            }

            // 17.4 Total provisional refusal of protection
            if (markgr.RFNT != null)
            {
                ProcessAdditionalDocs(markgr.RFNT, AdditionalDocType.TotalProvisionalRefusalOfProtection, localContext,
                    ref request);
            }

            // 17.5 Statement indicating that the mark is protected for all the goods and services requested
            if (markgr.FINV != null)
            {
                ProcessAdditionalDocs(markgr.FINV, AdditionalDocType.StatementIndicatingThatTheMarkIsProtected,
                    localContext, ref request);
            }

            // 17.6 Renewal
            if (markgr.REN != null)
            {
                ProcessAdditionalDocs(markgr.REN, AdditionalDocType.Renewal, localContext, ref request);
            }

            // 17.7 Limitation
            if (markgr.LIN != null)
            {
                ProcessAdditionalDocs(markgr.LIN, AdditionalDocType.Limitation, localContext, ref request);
            }

            // 17.8 Statement of grant of protection made under Rule 18ter(1)
            if (markgr.GP18N != null)
            {
                ProcessAdditionalDocs(markgr.GP18N, AdditionalDocType.StatementOfGrantOfProtectionMadeUnderRule,
                    localContext, ref request);
            }

            // 17.9 Ex Officio examination completed but opposition or observations by third parties still possible, under Rule 18bis(1)
            if (markgr.ISN != null)
            {
                ProcessAdditionalDocs(markgr.ISN, AdditionalDocType.ExOfficioExaminationCompleted, localContext,
                    ref request);
            }

            // 17.10 Confirmation of total provisional refusal under Rule 18ter(3)
            if (markgr.R18NT != null)
            {
                ProcessAdditionalDocs(markgr.R18NT, AdditionalDocType.ConfirmationOfTotalProvisionalRefusal,
                    localContext, ref request);
            }

            // 17.11 Partial invalidation
            if (markgr.INNP != null)
            {
                ProcessAdditionalDocs(markgr.INNP, AdditionalDocType.PartialInvalidation, localContext, ref request);
            }

            // 17.12 Statement indicating the goods and services for which protection of the mark is granted under Rule 18ter(2)(ii)
            if (markgr.R18NP != null)
            {
                ProcessAdditionalDocs(markgr.R18NP, AdditionalDocType.StatementIndicatingTheGoodsAndServices,
                    localContext, ref request);
            }

            // 17.13 Further statement under Rule 18ter(4) indicating that protection of the mark is granted for all the goods and services requested
            if (markgr.FDNV != null)
            {
                ProcessAdditionalDocs(markgr.FDNV, AdditionalDocType.FurtherStatementUnderRule, localContext,
                    ref request);
            }

            // 17.14 Opposition possible after the 18 months time limit
            if (markgr.OPN != null)
            {
                ProcessAdditionalDocs(markgr.OPN, AdditionalDocType.OppositionPossibleAfterTimeLimit, localContext,
                    ref request);
            }

            // 17.15 Statement of grant of protection following a provisional refusal under Rule 18ter(2)(i)
            if (markgr.R18NV != null)
            {
                ProcessAdditionalDocs(markgr.R18NV,
                    AdditionalDocType.StatementOfGrantOfProtectionFollowingProvisionalRefusal, localContext,
                    ref request);
            }

            // 17.16 Partial provisional refusal of protection
            if (markgr.RFNP != null)
            {
                ProcessAdditionalDocs(markgr.RFNP,
                    AdditionalDocType.StatementOfGrantOfProtectionFollowingProvisionalRefusal, localContext,
                    ref request);
            }

            _numberGenerator.GenerateBarcode(request);

            #endregion

            //}
            //catch (Exception e)
            //{
            //    Debug.WriteLine(e);
            //}

            //localContext.ProtectionDocs.Add(protectionDoc);

            return localContext;
        }

        /// <summary>
        ///     Обработка Патентообладателя
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="markgr"></param>
        /// <param name="request"></param>
        private void ProcessPatentHolder(NiisWebContext localContext, MARKGR markgr, Request request)
        {
            if (!localContext.RequestCustomers.Any(rc =>
                rc.RequestId == request.Id && rc.Customer.Xin == markgr.CURRENT.HOLGR.CLID.Trim()))
            {
                // адрес
                var sb = new StringBuilder();
                foreach (var addrl in markgr.CURRENT.HOLGR.ADDRESS.ADDRL)
                {
                    sb.Append(addrl);
                    sb.Append(" ");
                }

                var address = sb.ToString().Trim();

                // название
                sb = new StringBuilder();
                foreach (var nl in markgr.CURRENT.HOLGR.NAME.NAMEL)
                {
                    sb.Append(nl.Trim());
                    sb.Append(" ");
                }

                var namel = sb.ToString().Trim();

                var patentHolderRole =
                    localContext.DicCustomerRoles.FirstOrDefault(dcr => dcr.NameRu == "Патентообладатель");
                // не определено
                var patentHolderType = localContext.DicCustomerTypes.FirstOrDefault(dct => dct.Code == "4");

                var patentHolder =
                    localContext.DicCustomers.FirstOrDefault(dc => dc.Xin == markgr.CURRENT.HOLGR.CLID.Trim())
                    ?? new DicCustomer
                    {
                        DateCreate = DateTimeOffset.Now
                    };

                // страна
                var country = localContext.DicCountries
                    .FirstOrDefault(
                        dc => dc.Code.ToLower() == markgr.CURRENT.HOLGR.ADDRESS.COUNTRY.ToLower().Trim());

                if (patentHolder.Id == 0)
                {
                    LogMsg(patentHolder.Xin, markgr.CURRENT.HOLGR.CLID.Trim(), "Патентообладатель");
                    patentHolder.Xin = markgr.CURRENT.HOLGR.CLID.Trim();
                }

                LogMsg(patentHolder.Address, address, "Патентообладатель. Адрес");
                patentHolder.Address = address;

                LogMsg(patentHolder.NameEn, namel, "Патентообладатель. Название");
                patentHolder.NameEnLong = namel;
                patentHolder.NameEn = namel;
                patentHolder.NameRuLong = namel;
                patentHolder.NameRu = namel;

                LogMsg(patentHolder.Country?.NameEn, country?.NameEn, "Патентообладатель. Страна");
                patentHolder.CountryId = country?.Id;
                patentHolder.Country = country;
                patentHolder.Type = patentHolderType;

                localContext.RequestCustomers.Add(new RequestCustomer
                {
                    Request = request,
                    Customer = patentHolder,
                    CustomerRole = patentHolderRole,
                    DateCreate = DateTimeOffset.Now
                });
            }
        }

        /// <summary>
        ///     Обработка изображения
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="markgr"></param>
        /// <param name="request"></param>
        private void ProcessImage(NiisWebContext localContext, MARKGR markgr, Request request)
        {
            var fileNameFromXml = (markgr.CURRENT.IMAGE.NAME + "." + markgr.CURRENT.IMAGE.TYPE).ToLower().Trim();
            var imageZip = _zipArchive.Entries.FirstOrDefault(ent =>
                ent.Name.ToLower().Contains(fileNameFromXml)
                || ent.Name.ToLower().Contains(markgr.CURRENT.IMAGE.NAME.ToLower().Trim())
                || fileNameFromXml.Contains(ent.Name.ToLower()));
            if (imageZip != null)
            {
                var ms = new MemoryStream();
                for (var i = 0; i < 3; i++)
                {
                    try
                    {
                        var imgStream = imageZip.Open();
                        imgStream.CopyTo(ms);

                        LogMsg(null, fileNameFromXml, $"Изображение. Заявка {request.RequestNum}");
                        request.Image = ms.ToArray();
                        break;
                    }
                    catch (Exception e)
                    {
                        Log.Logger.Error(e, $"Изображение. Заявка {request.RequestNum}");
                        Thread.Sleep(1000);
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(request.NameEn)
                    || !string.IsNullOrEmpty(request.NameRu)
                    || !string.IsNullOrEmpty(request.NameKz))
                {
                    var lu = new LogoUpdater();
                    request.IsImageFromName = true;
                    lu.Update(request);

                    var logoText = string.Join(Environment.NewLine, request.NameRu, request.NameKz, request.NameEn);
                    LogMsg(null, logoText, $"Изображение. Сформировано из названия. Заявка {request.RequestNum}");
                }
            }
        }

        /// <summary>
        ///     ОБработка МКИЭТЗ
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="markgr"></param>
        /// <param name="request"></param>
        /// <param name="statusCode"></param>
        /// <param name="protectionDocIcfems"></param>
        private void ProcessIcfem(NiisWebContext localContext, MARKGR markgr, Request request, string statusCode,
            IList<DicIcfemProtectionDocRelation> protectionDocIcfems)
        {
            foreach (var v in markgr.CURRENT.VIENNAGR.VIECLA3)
            {
                var icefIcfem =
                    localContext.DicICFEMs.FirstOrDefault(cf => cf.Code.Replace(".", "").Trim() == v.Trim());
                if (icefIcfem != null)
                {
                    if (localContext.DicIcfemRequestRelations.All(icf =>
                        icf.DicIcfemId != icefIcfem.Id && icf.RequestId != request.Id))
                    {
                        LogMsg(null, icefIcfem.NameRu, "МКИЭТЗ");
                        request.Icfems.Add(new DicIcfemRequestRelation
                        {
                            DicIcfemId = icefIcfem.Id,
                            DicIcfem = icefIcfem
                        });
                        if (statusCode == "D.TZ")
                        {
                            protectionDocIcfems.Add(new DicIcfemProtectionDocRelation
                            {
                                DicIcfemId = icefIcfem.Id,
                                DicIcfem = icefIcfem
                            });
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     МКТУ
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="markgr"></param>
        /// <param name="request"></param>
        /// <param name="statusCode"></param>
        /// <param name="protectionDocIcgs"></param>
        private void ProcessIcgs(NiisWebContext localContext, MARKGR markgr, Request request, string statusCode,
            IList<ICGSProtectionDoc> protectionDocIcgs)
        {
            foreach (var gsgr in markgr.CURRENT.BASICGS.GSGRs)
            {
                var icgs = localContext.DicICGSs.FirstOrDefault(di => di.NameEn.Contains(gsgr.NICCLAI.Trim()));
                if (icgs != null)
                {
                    var gstrStr = new[] {gsgr.GSTERMEN, gsgr.GSTERMES, gsgr.GSTERMFR}.FirstOrDefault(str =>
                        !string.IsNullOrEmpty(str)) ?? string.Empty;
                    var icgsObj = localContext.ICGSRequests.FirstOrDefault(icf =>
                        icf.IcgsId == icgs.Id && icf.RequestId == request.Id);
                    if (icgsObj == null)
                    {
                        var ir = new ICGSRequest
                        {
                            IcgsId = icgs.Id,
                            Icgs = icgs
                        };

                        LogMsg(ir.Description, gstrStr, "МКТУ");
                        ir.ClaimedDescription = ir.Description = gstrStr;
                        request.ICGSRequests.Add(ir);
                    }
                    else
                    {
                        LogMsg(icgsObj.Description, gstrStr, "МКТУ");
                        icgsObj.ClaimedDescription = icgsObj.Description = gstrStr;
                    }

                    if (statusCode == "D.TZ")
                    {
                        protectionDocIcgs.Add(new ICGSProtectionDoc
                        {
                            DateCreate = DateTimeOffset.Now,
                            ClaimedDescription = gstrStr,
                            Description = gstrStr
                        });
                    }
                }
            }
        }

        /// <summary>
        ///     (511) МКТУ- Описание отказа по классу
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="markgr"></param>
        /// <param name="request"></param>
        /// <param name="statusCode"></param>
        /// <param name="protectionDocIcgs"></param>
        private void ProcessIcgsRefuse(NiisWebContext localContext, MARKGR markgr, Request request, string statusCode,
            IList<ICGSProtectionDoc> protectionDocIcgs)
        {
            foreach (var lin in markgr.LIN.Where(l => l.DCPCD.Contains(KZ)))
            {
                foreach (var limto in lin.LIMTO)
                {
                    var icgs = localContext.DicICGSs.FirstOrDefault(di => di.NameEn.Contains(limto.NICCLAI.Trim()));
                    if (icgs != null)
                    {
                        var gsterStr = new[] {limto.GSTERMEN, limto.GSTERMES, limto.GSTERMFR}.FirstOrDefault(str =>
                            !string.IsNullOrEmpty(str)) ?? string.Empty;
                        var icgsObj = localContext.ICGSRequests.FirstOrDefault(icf =>
                            icf.IcgsId != icgs.Id && icf.RequestId != request.Id);
                        if (icgsObj == null)
                        {
                            var ir = new ICGSRequest
                            {
                                IcgsId = icgs.Id,
                                Icgs = icgs,
                                IsRefused = true
                            };
                            LogMsg(ir.Description, gsterStr, "(511) МКТУ- Описание отказа по классу");
                            ir.ClaimedDescription = ir.Description = gsterStr;
                            request.ICGSRequests.Add(ir);
                        }
                        else
                        {
                            LogMsg(icgsObj.Description, gsterStr, "(511) МКТУ- Описание отказа по классу");
                            icgsObj.Description = icgsObj.ClaimedDescription = gsterStr;
                        }

                        if (statusCode == "D.TZ")
                        {
                            protectionDocIcgs.Add(new ICGSProtectionDoc
                            {
                                DateCreate = DateTimeOffset.Now,
                                ClaimedDescription = gsterStr,
                                Description = gsterStr,
                                IsNegative = false
                            });
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Обработка
        /// </summary>
        /// <param name="additionalDocTags">Список тегов с дополнительными документами</param>
        /// <param name="code">код документа</param>
        /// <param name="request">Заявка</param>
        private void ProcessAdditionalDocs(IEnumerable<IAdditionalDocTag> additionalDocTags, string code,
            NiisWebContext localContext, ref Request request)
        {
            foreach (var additionalDocTag in additionalDocTags)
            {
                var pubdate = DateFromString(additionalDocTag.PUBDATE);
                var notdate = DateFromString(additionalDocTag.NOTDATE);
                var regedat = DateFromString(additionalDocTag.REGEDAT);
                var regrdat = DateFromString(additionalDocTag.REGRDAT);

                var addDoc = new AdditionalDoc
                {
                    Code = code,
                    DateCreate = DateTimeOffset.Now,
                    GazetteReference = additionalDocTag.GAZNO?.Trim(),
                    PublicationDate = pubdate,
                    NotificationDate = notdate,
                    IntRegisterRegnDate = regrdat,
                    IntRegisterEffectiveDate = regedat
                };

                LogMsg(null, addDoc.GazetteReference,
                    AdditionalDocType.Descriptions[code] +
                    $" Gazette reference of publication. Заявка № {request.RequestNum}");
                LogMsg(null, pubdate?.ToString(),
                    AdditionalDocType.Descriptions[code] + $" Date of publication Заявка. № {request.RequestNum}");
                LogMsg(null, notdate?.ToString(),
                    AdditionalDocType.Descriptions[code] + $" Notification Date Заявка. № {request.RequestNum}");
                LogMsg(null, regrdat?.ToString(),
                    AdditionalDocType.Descriptions[code] +
                    $" Date of recordal in the International Register Заявка. № {request.RequestNum}");
                LogMsg(null, regedat?.ToString(),
                    AdditionalDocType.Descriptions[code] +
                    $" Effective date of modification The date that a transaction recorded in the international register in respect of a given international registration has effect. Заявка. № {request.RequestNum}");

                if (additionalDocTag.INTOFF != null)
                {
                    var country = localContext.DicCountries
                        .FirstOrDefault(c => c.Code.ToLower().Trim() == additionalDocTag.INTOFF.ToLower().Trim());
                    addDoc.OfficeOfOriginCountry = country;

                    LogMsg(null, addDoc.OfficeOfOriginCountry?.Code,
                        AdditionalDocType.Descriptions[code] +
                        $" Office of Origin Code The two letter country code (WIPO ST3.) that is used to identify the Office of Origin Заявка. № {request.RequestNum}");
                }

                request.AdditionalDocs.Add(addDoc);
            }
        }

        private void InitContext()
        {
            //_context?.Dispose();

            var builder = new DbContextOptionsBuilder<NiisWebContext>();
            builder.UseSqlServer(_connectionString);
            _context = new NiisWebContext(builder.Options);
            _numberGenerator = new NumberGenerator(_context);
        }

        private DateTimeOffset? DateFromString(string dateStr)
        {
            return !string.IsNullOrEmpty(dateStr?.Trim())
                ? DateTime.ParseExact(dateStr, formatyyyyMMdd, CultureInfo.InvariantCulture)
                : (DateTime?) null;
        }

        private void LogMsg(string fieldValue, string fieldNewValue, string msg)
        {
            if (!string.IsNullOrEmpty(fieldValue) && fieldValue == fieldNewValue ||
                string.IsNullOrEmpty(fieldValue) && string.IsNullOrEmpty(fieldNewValue))
            {
                return;
            }

            Log.ForContext(new[]
            {
                new PropertyEnricher("Status",
                    string.IsNullOrEmpty(fieldValue)
                        ? IntegrationConst.Statuses.New
                        : IntegrationConst.Statuses.Changed),
                new PropertyEnricher("Type", IntegrationLogType.Event),
                new PropertyEnricher("OldValue", fieldValue),
                new PropertyEnricher("NewValue", fieldNewValue)
            }).Information(msg);
        }
    }
}