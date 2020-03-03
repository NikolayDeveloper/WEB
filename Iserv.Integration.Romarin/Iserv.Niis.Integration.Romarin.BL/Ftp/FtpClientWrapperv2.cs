using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using FluentFTP;
using Iserv.Niis.Integration.Romarin.Domain;
using Iserv.Niis.Integration.Romarin.Domain.DbContext;
using Microsoft.EntityFrameworkCore;


namespace Iserv.Niis.Integration.Romarin.BL.Ftp
{
    public class FtpClientWrapperv2
    {
        private FtpClient _ftpClient;
        private string _path;
        private const string KZ = "KZ";
        private const string AT = "AT";
        private const string BG = "BG";
        private const string HR = "HR";
        private const string formatyyyyMMdd = "yyyyMMdd";

        private NiisContext _context;
        private ZipArchive _zipArchive = null;

        public FtpClientWrapper(string host, string userId, string password, string path)
        {
            _ftpClient = new FtpClient(host, new NetworkCredential(userId, password));
            _path = path;
        }

        public void Get()
        {
            try
            {
                InitContext();

                _context.Database.EnsureCreated();
                Debug.WriteLine("Begin: " + DateTime.Now);
                try
                {
                    _ftpClient.Connect();
                    _context.IntegrationRecords.Add(new IntegrationRecord()
                    {
                        EventName = "Соединение с Ftp сервером",
                        Description = string.Empty,
                        DateTimeStamp = DateTime.Now
                    });
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                    _context.IntegrationRecords.Add(new IntegrationRecord()
                    {
                        EventName = "Соединение с Ftp сервером. Ошибка",
                        Description = e.ToString(),
                        DateTimeStamp = DateTime.Now
                    });
                    return;
                }
                
                foreach (FtpListItem item in _ftpClient.GetListing(_path)) {
                    Debug.WriteLine("Iteration start: " + DateTime.Now);
                    _context.IntegrationRecords.Add(new IntegrationRecord()
                    {
                        EventName = "Старт итерации",
                        Description = item.Name,
                        DateTimeStamp = DateTime.Now
                    });

                    IntegrationFile file = new IntegrationFile();
                    file.ArchiveName = item.FullName;
                    file.DateTimeStamp = DateTime.Now;
                    
                    // if this is a file
                    if (item.Type == FtpFileSystemObjectType.File)
                    {
                        //Task<bool> isDownloadTask = _ftpClient.DownloadFileAsync("..\\Files\\" + item.Name, item.FullName);
                        MemoryStream outStream = new MemoryStream();
                        //Task<bool> isDownloadTask = _ftpClient.DownloadAsync(outStream, item.FullName);
                        //isDownloadTask.ContinueWith()
                        
                        bool isDownload = false;
                        // Скачивание архива
                        for (int i = 0; i < 3; i++)
                        {
                            try
                            {
                                Debug.WriteLine("Download Zip start: " + DateTime.Now);
                                isDownload =  _ftpClient.Download(outStream, item.FullName);
                                Debug.WriteLine("Download Zip end: " + DateTime.Now);
                                _context.IntegrationRecords.Add(new IntegrationRecord()
                                {
                                    EventName = "Скачивания архива",
                                    Description = item.Name,
                                    DateTimeStamp = DateTime.Now
                                });
                                break;
                            }
                            catch (Exception e)
                            {
                                Debug.WriteLine(e.ToString());
                                _context.IntegrationRecords.Add(new IntegrationRecord()
                                {
                                    EventName = "Скачивания архива. Ошибка",
                                    Description = e.ToString(),
                                    DateTimeStamp = DateTime.Now
                                });
                            }
                            Thread.Sleep(5000);
                        }
                        if (isDownload)
                        {
                            _context.IntegrationRecords.Add(new IntegrationRecord()
                            {
                                EventName = "Успешное скачен",
                                Description = item.Name,
                                DateTimeStamp = DateTime.Now
                            });

                            file.Description = "Файл успешно скачен";
                            file.Status = FileStatuses.Downloaded;

                            // найти xml
                            _zipArchive = new ZipArchive(outStream, ZipArchiveMode.Read);
                            var xmlEntry = _zipArchive.Entries.FirstOrDefault(ent => ent.Name.EndsWith(".xml"));
                            if (xmlEntry != null)
                            {
                                var xmlStream = xmlEntry.Open();

                                XmlSerializer serializer = new XmlSerializer(typeof(RomarinDto));
                                RomarinDto romarinDto = (RomarinDto) serializer.Deserialize(xmlStream);

                                // Сохранить МТЗ
                                int operationCount = 0;
                                foreach (var markgr in romarinDto.MARKGRS)
                                {
                                    ProcessItem(markgr, null, _context);
                                    if (++operationCount >= 500)
                                    {
                                        Debug.WriteLine("SaveChanges start: " + DateTime.Now);
                                        operationCount = 0;
                                        _context.SaveChanges();
                                        Debug.WriteLine("SaveChanges end: " + DateTime.Now);
                                        InitContext();
                                    }
                                }

                                file.Description = "Данные сохранены";
                                file.Status = FileStatuses.Success;
                                if (operationCount > 0)
                                {
                                    _context.IntegrationFiles.Add(file);
                                    Debug.WriteLine("SaveChanges start: " + DateTime.Now);
                                    _context.SaveChanges();
                                    Debug.WriteLine("SaveChanges end: " + DateTime.Now);
                                    InitContext();
                                }
                                // SaveMtz(romarinDto.MARKGRS);
                            }
                        }
                        else
                        {
                            file.Description = "Файл скачен с ошибкой";
                            file.Status = FileStatuses.Error;
                            _context.SaveChanges();
                        }
                    }

                    Debug.WriteLine("Iteration end: " + DateTime.Now);
                    _context.IntegrationRecords.Add(new IntegrationRecord()
                    {
                        EventName = "Завершение итерации",
                        Description = item.FullName,
                        DateTimeStamp = DateTime.Now
                    });
                }

                _ftpClient.Disconnect();
                Debug.WriteLine("End: " + DateTime.Now);
                _context.IntegrationRecords.Add(new IntegrationRecord()
                {
                    EventName = "Соединение с Ftp сервером закрыто",
                    Description = string.Empty,
                    DateTimeStamp = DateTime.Now
                });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                _context.IntegrationRecords.Add(new IntegrationRecord()
                {
                    EventName = "Общая ошибка",
                    Description = e.ToString(),
                    DateTimeStamp = DateTime.Now
                });
            }
            _context.SaveChanges();
        }

        private void SaveMtz(List<MARKGR> romarinDtoMarkgrs)
        {
            Parallel.ForEach(romarinDtoMarkgrs,
                new ParallelOptions() {MaxDegreeOfParallelism = 8},
                () => new NiisContext(),
                ProcessItem,
                (context) => context.Dispose());
        }

        private NiisContext ProcessItem(MARKGR markgr, ParallelLoopState loopState, NiisContext localContext)
        {
            //foreach (MARKGR markgr in romarinDto.MARKGRS)
            //{

            // Only for KZ
            bool? isKz = markgr.EXN?.Any(e =>
                e.DESPG != null && e.DESPG.DCPCD.Contains(KZ) || e.DESPG2 != null && e.DESPG2.DCPCD.Contains(KZ));
            isKz = isKz.HasValue && isKz.Value || markgr.LIN != null && markgr.LIN.Any(l => l.DCPCD.Contains(KZ));
            if (!isKz.HasValue || !isKz.Value)
            {
                return localContext;
            }

            TzTable tzTable = new TzTable();

            #region Convert Data   

            // 1
            tzTable.Name1 = markgr.CURRENT?.IMAGE.TEXT;

            // 2
            tzTable.RegNumberSecurityDocumentNumber2 = markgr.INTREGN;

            // 3
            tzTable.DateOfApplicationDateOfTitle3 = !string.IsNullOrEmpty(markgr.INTREGD)
                ? DateTime.ParseExact(markgr.INTREGD, formatyyyyMMdd, CultureInfo.InvariantCulture)
                : (DateTime?) null;

            // 4
            tzTable.Validity4 = !string.IsNullOrEmpty(markgr.EXPDATE)
                ? DateTime.ParseExact(markgr.EXPDATE, formatyyyyMMdd, CultureInfo.InvariantCulture)
                : (DateTime?) null;


            // 5
            var exn = markgr.EXN?.FirstOrDefault(e =>
                (e.DESPG != null && e.DESPG.DCPCD.Contains(KZ)) || (e.DESPG2 != null && e.DESPG2.DCPCD.Contains(KZ)));
            tzTable.DateOfDistributionRK5 = !string.IsNullOrEmpty(exn?.REGEDAT)
                ? DateTime.ParseExact(exn.REGEDAT, formatyyyyMMdd, CultureInfo.InvariantCulture)
                : (DateTime?) null;

            // 6
            if (markgr.CURRENT?.HOLGR?.ADDRESS != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string addrl in markgr.CURRENT.HOLGR.ADDRESS.ADDRL)
                {
                    sb.Append(addrl);
                    sb.Append(" ");
                }
                var address = sb.ToString().Trim();

                sb = new StringBuilder();
                foreach (string nl in markgr.CURRENT.HOLGR.NAME.NAMEL)
                {
                    sb.Append(nl);
                    sb.Append(" ");
                }
                var namel = sb.ToString().Trim();

                tzTable.Patentee6 = new PatentHolder()
                {
                    Address = new Address()
                    {
                        ADDRL = address,
                        COUNTRY = markgr.CURRENT.HOLGR.ADDRESS.COUNTRY
                    },
                    CLID = markgr.CURRENT.HOLGR.CLID,
                    NOTLANG = markgr.CURRENT.HOLGR.NOTLANG,
                    NameL = namel,
                    ENTNATL = markgr.CURRENT.HOLGR.ENTNATL,
                    NATLTY = markgr.CURRENT.HOLGR.NATLTY,
                };
            }

            if (markgr.CURRENT != null)
            {
                // 7
                var imageZip = _zipArchive.Entries.FirstOrDefault(ent =>
                    (markgr.CURRENT.IMAGE.NAME + "." + markgr.CURRENT.IMAGE.TYPE).ToLower()
                    .Contains(ent.Name.ToLower()));
                if (imageZip != null)
                {
                    var imgStream = imageZip.Open();
                    MemoryStream ms = new MemoryStream();
                    imgStream.CopyTo(ms);

                    tzTable.Image7 = new Image()
                    {
                        Colour = markgr.CURRENT.IMAGE.COLOUR,
                        Type = markgr.CURRENT.IMAGE.TYPE,
                        Text = markgr.CURRENT.IMAGE.TEXT,
                        ImageValue = ms.ToArray()
                    };
                }

                // 8
                tzTable.Transliteration8 = markgr.CURRENT.MARTRAN;

                // 9
                StringBuilder sb = new StringBuilder();
                if (markgr.CURRENT.VIENNAGR != null)
                {
                    foreach (string v in markgr.CURRENT.VIENNAGR.VIECLAI)
                    {
                        sb.Append(v);
                        sb.Append(" ");
                    }
                    var viewclais = sb.ToString();

                    sb = new StringBuilder();
                    foreach (string v in markgr.CURRENT.VIENNAGR.VIECLA3)
                    {
                        sb.Append(v);
                        sb.Append(" ");
                    }
                    var viewcla3S = sb.ToString();
                    tzTable.MKIETZ9 = new MKIETZ()
                    {
                        VIENVER = markgr.CURRENT.VIENNAGR.VIENVER,
                        VIECLAIs = viewclais,
                        VIECLA3s = viewcla3S
                    };
                }


                // 10
                if (markgr.CURRENT.BASICGS != null)
                {
                    tzTable.MKTU10 = new MKTU
                    {
                        NICEVER = markgr.CURRENT.BASICGS.NICEVER,
                        GSGRs = new List<Domain.GSGR>(),
                    };
                    foreach (GSGR gsgr in markgr.CURRENT.BASICGS.GSGRs)
                    {
                        tzTable.MKTU10.GSGRs.Add(new Domain.GSGR()
                        {
                            GSTERMEN = gsgr.GSTERMEN,
                            NICCLAI = gsgr.NICCLAI
                        });
                    }
                }
            }

            // 11
            if (markgr.LIN != null && markgr.LIN.Any(l => l.DCPCD.Contains(KZ)))
            {
                foreach( var lin in markgr.LIN.Where(l => l.DCPCD.Contains(KZ)))
                {
                    DateTime? notdate = string.IsNullOrEmpty(lin.NOTDATE)
                        ? (DateTime?)null
                        : DateTime.ParseExact(lin.NOTDATE, formatyyyyMMdd, CultureInfo.InvariantCulture);
                    DateTime? pubdate = string.IsNullOrEmpty(lin.PUBDATE)
                        ? (DateTime?)null
                        : DateTime.ParseExact(lin.PUBDATE, formatyyyyMMdd, CultureInfo.InvariantCulture);
                    DateTime? regedat = string.IsNullOrEmpty(lin.PUBDATE)
                        ? (DateTime?)null
                        : DateTime.ParseExact(lin.REGEDAT, formatyyyyMMdd, CultureInfo.InvariantCulture);
                    DateTime? regrdat = string.IsNullOrEmpty(lin.PUBDATE)
                        ? (DateTime?)null
                        : DateTime.ParseExact(lin.REGRDAT, formatyyyyMMdd, CultureInfo.InvariantCulture);

                    tzTable.MKTUDescription11 = new MKTUDescription
                    {
                        GAZNO = lin.GAZNO,
                        NOTDATE = notdate,
                        PUBDATE = pubdate,
                        REGEDAT = regedat,
                        REGRDAT = regrdat,
                        Limtos = new List<Domain.LIMTO>()
                    };
                    foreach (LIMTO limto in lin.LIMTO)
                    {
                        tzTable.MKTUDescription11.Limtos.Add(new Domain.LIMTO()
                        {
                            NICCLAI = limto.NICCLAI,
                            GSTERMEN = limto.GSTERMEN
                        });
                    }
                }
            }


            if (markgr.CURRENT != null)
            {
                // 12
                tzTable.Colors12 = markgr.CURRENT?.COLCLAGR?.COLCLAEN;

                // 13
                DateTime? priappd = null;
                if (!string.IsNullOrEmpty(markgr.CURRENT.PRIGR?.PRIAPPD))
                {
                    priappd = DateTime.ParseExact(markgr.CURRENT.PRIGR.PRIAPPD, formatyyyyMMdd,
                        CultureInfo.InvariantCulture);
                }

                tzTable.PriorityData13 = new PriorityData()
                {
                    PRICP = markgr.CURRENT.PRIGR?.PRICP,
                    PRIAPPD = priappd,
                    PRIAPPN = markgr.CURRENT.PRIGR?.PRIAPPN,
                    TEXTEN = markgr.CURRENT.PRIGR?.TEXTEN
                };

                // 14
                tzTable.Disclaimer14 = markgr.CURRENT.DISCLAIMGR?.DISCLAIMEREN;
            }

            // 15
            tzTable.Status15 = "";
            // 16
            tzTable.TypeOD16 = "";

            // 17.1
            if (markgr.ENN != null)
            {
                foreach (var enn in markgr.ENN)
                {
                    //DateTime? notdate = string.IsNullOrEmpty(markgr.LIN.NOTDATE) ? (DateTime?)null : DateTime.Parse(markgr.ENN.NOTDATE);
                    DateTime? pubdate = string.IsNullOrEmpty(enn.PUBDATE)
                        ? (DateTime?)null
                        : DateTime.ParseExact(enn.PUBDATE, formatyyyyMMdd, CultureInfo.InvariantCulture);
                    //DateTime? regedat = string.IsNullOrEmpty(markgr.LIN.PUBDATE) ? (DateTime?)null : DateTime.Parse(markgr.LIN.REGEDAT);
                    //DateTime? regrdat = string.IsNullOrEmpty(markgr.LIN.PUBDATE) ? (DateTime?)null : DateTime.Parse(markgr.LIN.REGRDAT);
                    tzTable.Registration171 = new List<Registration>
                    {
                        new Registration
                        {
                            GAZNO = enn.GAZNO,
                            PUBDATE = pubdate
                        }
                    };
                }
            }

            // 17.2
            if (markgr.EXN != null)
            {
                tzTable.SubsequentDesignation172 = new List<SubsequentDesignation>();
                foreach (var exnL in markgr.EXN)
                {
                    DateTime? pubdate = string.IsNullOrEmpty(exnL.PUBDATE)
                        ? (DateTime?) null
                        : DateTime.ParseExact(exnL.PUBDATE, formatyyyyMMdd, CultureInfo.InvariantCulture);

                    tzTable.SubsequentDesignation172.Add(
                        new SubsequentDesignation
                        {
                            GAZNO = exnL.GAZNO,
                            PUBDATE = pubdate,
                        });
                }
            }

            // 17.3
            if (markgr.CENS != null)
            {
                //var cenL = markgr.CENS.FirstOrDefault(e => e.DCPCD != null && e.DCPCD.Contains(BG));
                //if (cenL != null)
                tzTable.ContinuationOfEffect173 = new List<ContinuationOfEffect>();
                foreach (var cenL in markgr.CENS)
                {
                    DateTime? pubdate = string.IsNullOrEmpty(cenL.PUBDATE)
                        ? (DateTime?) null
                        : DateTime.ParseExact(cenL.PUBDATE, formatyyyyMMdd, CultureInfo.InvariantCulture);

                    tzTable.ContinuationOfEffect173.Add(
                        new ContinuationOfEffect()
                        {
                            GAZNO = cenL.GAZNO,
                            PUBDATE = pubdate,
                        }
                    );
                }
            }

            // 17.4
            if (markgr.RFNT != null)
            {
                tzTable.TotalProvisionalRefusalOfProtection174 = new List<TotalProvisionalRefusalOfProtection>();
                foreach (var rfnt in markgr.RFNT)
                {
                    DateTime? pubdate = string.IsNullOrEmpty(rfnt.PUBDATE)
                        ? (DateTime?) null
                        : DateTime.ParseExact(rfnt.PUBDATE, formatyyyyMMdd, CultureInfo.InvariantCulture);

                    tzTable.TotalProvisionalRefusalOfProtection174.Add(new TotalProvisionalRefusalOfProtection()
                    {
                        GAZNO = rfnt.GAZNO,
                        PUBDATE = pubdate
                    });
                }
            }

            // 17.5
            if (markgr.FINV != null)
            {
                tzTable.StatementIndicatingThatTheMarkIsProtected175 =
                    new List<StatementIndicatingThatTheMarkIsProtected>();
                foreach (var finv in markgr.FINV)
                {
                    DateTime? notdate = string.IsNullOrEmpty(finv.NOTDATE)
                        ? (DateTime?)null
                        : DateTime.ParseExact(finv.NOTDATE, formatyyyyMMdd, CultureInfo.InvariantCulture);
                    DateTime? pubdate = string.IsNullOrEmpty(finv.PUBDATE)
                        ? (DateTime?)null
                        : DateTime.ParseExact(finv.PUBDATE, formatyyyyMMdd, CultureInfo.InvariantCulture);
                    DateTime? regedat = string.IsNullOrEmpty(finv.REGEDAT)
                        ? (DateTime?)null
                        : DateTime.ParseExact(finv.REGEDAT, formatyyyyMMdd, CultureInfo.InvariantCulture);
                    DateTime? regrdat = string.IsNullOrEmpty(finv.REGRDAT)
                        ? (DateTime?)null
                        : DateTime.ParseExact(finv.REGRDAT, formatyyyyMMdd, CultureInfo.InvariantCulture);

                    tzTable.StatementIndicatingThatTheMarkIsProtected175.Add(
                        new StatementIndicatingThatTheMarkIsProtected()
                        {
                            GAZNO = finv.GAZNO,
                            PUBDATE = pubdate,
                            NOTDATE = notdate,
                            REGEDAT = regedat,
                            REGRDAT = regrdat,
                            INTOFF = finv.INTOFF
                        });
                }
            }

            // 17.6
            if (markgr.REN != null)
            {
                tzTable.Renewal176 = new List<Renewal>();
                foreach (var ren in markgr.REN)
                {
                    DateTime? pubdate = string.IsNullOrEmpty(ren.PUBDATE)
                        ? (DateTime?) null
                        : DateTime.ParseExact(ren.PUBDATE, formatyyyyMMdd, CultureInfo.InvariantCulture);

                    tzTable.Renewal176.Add(new Renewal()
                    {
                        GAZNO = ren.GAZNO,
                        PUBDATE = pubdate
                    });
                }
            }

            // 17.7
            if (markgr.LIN != null)
            {
                tzTable.Limitation177 = new List<Limitation>();
                foreach (var lin in markgr.LIN)
                {
                    DateTime? pubdate = string.IsNullOrEmpty(lin.PUBDATE)
                        ? (DateTime?)null
                        : DateTime.ParseExact(lin.PUBDATE, formatyyyyMMdd, CultureInfo.InvariantCulture);

                    tzTable.Limitation177.Add(new Limitation()
                    {
                        GAZNO = lin.GAZNO,
                        PUBDATE = pubdate
                    });
                }
            }

            // 17.8
            if (markgr.GP18N != null)
            {
                tzTable.StatementOfGrantOfProtection178 = new List<StatementOfGrantOfProtectionMadeUnderRule>();
                foreach (var gp18n in markgr.GP18N)
                {
                    DateTime? pubdate = string.IsNullOrEmpty(gp18n.PUBDATE)
                        ? (DateTime?) null
                        : DateTime.ParseExact(gp18n.PUBDATE, formatyyyyMMdd, CultureInfo.InvariantCulture);
                    DateTime? regedat = string.IsNullOrEmpty(gp18n.REGEDAT)
                        ? (DateTime?) null
                        : DateTime.ParseExact(gp18n.REGEDAT, formatyyyyMMdd, CultureInfo.InvariantCulture);

                    tzTable.StatementOfGrantOfProtection178.Add(
                        new StatementOfGrantOfProtectionMadeUnderRule()
                        {
                            GAZNO = gp18n.GAZNO,
                            PUBDATE = pubdate,
                            REGEDAT = regedat,
                            INTOFF = gp18n.INTOFF
                        }
                    );
                }
            }

            // 17.9
            if (markgr.ISN != null)
            {
                tzTable.Isn179 = new List<ExOfficioExaminationCompleted>();
                foreach (var isn in markgr.ISN)
                {
                    DateTime? pubdate = string.IsNullOrEmpty(isn.PUBDATE)
                        ? (DateTime?) null
                        : DateTime.ParseExact(isn.PUBDATE, formatyyyyMMdd, CultureInfo.InvariantCulture);
                    DateTime? regedat = string.IsNullOrEmpty(isn.REGEDAT)
                        ? (DateTime?) null
                        : DateTime.ParseExact(isn.REGEDAT, formatyyyyMMdd, CultureInfo.InvariantCulture);

                    tzTable.Isn179.Add(new ExOfficioExaminationCompleted()
                        {
                            GAZNO = isn.GAZNO,
                            PUBDATE = pubdate,
                            REGEDAT = regedat,
                            INTOFF = isn.INTOFF
                        }
                    );
                }
            }

            // 17.10
            if (markgr.R18NT != null)
            {
                tzTable.ConfirmationTotalProvisional1710 = new List<ConfirmationOfTotalProvisionalRefusal>();
                foreach (var r18nt in markgr.R18NT)
                {
                    DateTime? pubdate = string.IsNullOrEmpty(r18nt.PUBDATE)
                        ? (DateTime?) null
                        : DateTime.ParseExact(r18nt.PUBDATE, formatyyyyMMdd, CultureInfo.InvariantCulture);
                    DateTime? regedat = string.IsNullOrEmpty(r18nt.REGEDAT)
                        ? (DateTime?) null
                        : DateTime.ParseExact(r18nt.REGEDAT, formatyyyyMMdd, CultureInfo.InvariantCulture);

                    tzTable.ConfirmationTotalProvisional1710.Add(new ConfirmationOfTotalProvisionalRefusal()
                        {
                            GAZNO = r18nt.GAZNO,
                            PUBDATE = pubdate,
                            REGEDAT = regedat,
                            INTOFF = r18nt.INTOFF
                        }
                    );
                }
            }

            // 17.11
            if (markgr.INNP != null)
            {
                tzTable.PartialInvalidationl1711 = new List<PartialInvalidation>();
                foreach (var innp in markgr.INNP)
                {
                    DateTime? pubdate = string.IsNullOrEmpty(innp.PUBDATE)
                        ? (DateTime?) null
                        : DateTime.ParseExact(innp.PUBDATE, formatyyyyMMdd, CultureInfo.InvariantCulture);
                    DateTime? regedat = string.IsNullOrEmpty(innp.REGEDAT)
                        ? (DateTime?) null
                        : DateTime.ParseExact(innp.REGEDAT, formatyyyyMMdd, CultureInfo.InvariantCulture);

                    tzTable.PartialInvalidationl1711.Add(
                        new PartialInvalidation()
                        {
                            GAZNO = innp.GAZNO,
                            PUBDATE = pubdate,
                            REGEDAT = regedat,
                            INTOFF = innp.INTOFF
                        }
                    );
                }
            }

            // 17.12
            if (markgr.R18NP != null)
            {
                tzTable.R18Np1712 = new List<StatementIndicatingTheGoodsAndServices>();
                foreach (var r18np in markgr.R18NP)
                {
                    DateTime? pubdate = string.IsNullOrEmpty(r18np.PUBDATE)
                        ? (DateTime?) null
                        : DateTime.ParseExact(r18np.PUBDATE, formatyyyyMMdd, CultureInfo.InvariantCulture);
                    //DateTime? regedat = string.IsNullOrEmpty(r18np.REGEDAT) ? (DateTime?)null : DateTime.Parse(r18np.REGEDAT);

                    tzTable.R18Np1712.Add(new StatementIndicatingTheGoodsAndServices()
                    {
                        GAZNO = r18np.GAZNO,
                        PUBDATE = pubdate,
                        //REGEDAT = regedat,
                        INTOFF = r18np.INTOFF
                    });
                }
            }

            // 17.13
            if (markgr.FDNV != null)
            {
                tzTable.Fdnp1713 = new List<FurtherStatementUnderRule>();
                foreach (var fdnv in markgr.FDNV)
                {
                    DateTime? pubdate = string.IsNullOrEmpty(fdnv.PUBDATE)
                        ? (DateTime?) null
                        : DateTime.ParseExact(fdnv.PUBDATE, formatyyyyMMdd, CultureInfo.InvariantCulture);
                    //DateTime? regedat = string.IsNullOrEmpty(r18np.REGEDAT) ? (DateTime?)null : DateTime.Parse(r18np.REGEDAT);

                    tzTable.Fdnp1713.Add(
                        new FurtherStatementUnderRule()
                        {
                            GAZNO = fdnv.GAZNO,
                            PUBDATE = pubdate,
                            //REGEDAT = regedat,
                            INTOFF = fdnv.INTOFF
                        }
                    );
                }
            }

            // 17.14
            if (markgr.OPN != null)
            {
                tzTable.Opn1714 = new List<OppositionPossibleAfterTimeLimit>();
                foreach (var opn in markgr.OPN)
                {
                    DateTime? pubdate = string.IsNullOrEmpty(opn.PUBDATE)
                        ? (DateTime?)null
                        : DateTime.ParseExact(opn.PUBDATE, formatyyyyMMdd, CultureInfo.InvariantCulture);
                    //DateTime? regedat = string.IsNullOrEmpty(r18np.REGEDAT) ? (DateTime?)null : DateTime.Parse(r18np.REGEDAT);

                    tzTable.Opn1714.Add(
                        new OppositionPossibleAfterTimeLimit()
                        {
                            GAZNO = opn.GAZNO,
                            PUBDATE = pubdate,
                            //REGEDAT = regedat,
                            INTOFF = opn.INTOFF
                        }
                     );
                }
            }

            // 17.15
            if (markgr.R18NV != null)
            {
                tzTable.R18Nv1715 = new List<StatementOfGrantOfProtectionFollowingProvisionalRefusal>();
                foreach (var r18Nv in markgr.R18NV)
                {
                    DateTime? pubdate = string.IsNullOrEmpty(r18Nv.PUBDATE)
                        ? (DateTime?)null
                        : DateTime.ParseExact(r18Nv.PUBDATE, formatyyyyMMdd, CultureInfo.InvariantCulture);
                    //DateTime? regedat = string.IsNullOrEmpty(r18np.REGEDAT) ? (DateTime?)null : DateTime.Parse(r18np.REGEDAT);

                    tzTable.R18Nv1715.Add(new StatementOfGrantOfProtectionFollowingProvisionalRefusal()
                    {
                        GAZNO = r18Nv.GAZNO,
                        PUBDATE = pubdate,
                        //REGEDAT = regedat,
                        INTOFF = r18Nv.INTOFF
                    });
                }
            }

            // 17.16
            if (markgr.RFNP != null)
            {
                tzTable.Rfnp1716 = new List<PartialProvisionalRefusalOfProtection>();
                foreach (var frnp in markgr.RFNP)
                {
                    DateTime? pubdate = string.IsNullOrEmpty(frnp.PUBDATE)
                        ? (DateTime?) null
                        : DateTime.ParseExact(frnp.PUBDATE, formatyyyyMMdd, CultureInfo.InvariantCulture);
                    DateTime? regedat = string.IsNullOrEmpty(frnp.REGEDAT)
                        ? (DateTime?) null
                        : DateTime.ParseExact(frnp.REGEDAT, formatyyyyMMdd, CultureInfo.InvariantCulture);
                    DateTime? regrdat = string.IsNullOrEmpty(frnp.REGRDAT)
                        ? (DateTime?) null
                        : DateTime.ParseExact(frnp.REGRDAT, formatyyyyMMdd, CultureInfo.InvariantCulture);

                    tzTable.Rfnp1716.Add(new PartialProvisionalRefusalOfProtection()
                    {
                        GAZNO = frnp.GAZNO,
                        PUBDATE = pubdate,
                        REGEDAT = regedat,
                        REGRDAT = regrdat,
                        INTOFF = frnp.INTOFF
                    });
                }
            }
            #endregion

            localContext.TzTables.Add(tzTable);
            //localContext.SaveChanges();

            //}
            return localContext;
        }

        private NiisContext ProcessItemDomain(MARKGR markgr, ParallelLoopState loopState, NiisContext localContext)
        {
            //foreach (MARKGR markgr in romarinDto.MARKGRS)
            //{

            // Only for KZ
            bool? isKz = markgr.EXN?.Any(e =>
                e.DESPG != null && e.DESPG.DCPCD.Contains(KZ) || e.DESPG2 != null && e.DESPG2.DCPCD.Contains(KZ));
            isKz = isKz.HasValue && isKz.Value || markgr.LIN != null && markgr.LIN.Any(l => l.DCPCD.Contains(KZ));
            if (!isKz.HasValue || !isKz.Value)
            {
                return localContext;
            }


            


            TzTable tzTable = new TzTable();

            #region Convert Data   

            // 1
            tzTable.Name1 = markgr.CURRENT?.IMAGE.TEXT;

            // 2
            tzTable.RegNumberSecurityDocumentNumber2 = markgr.INTREGN;

            // 3
            tzTable.DateOfApplicationDateOfTitle3 = !string.IsNullOrEmpty(markgr.INTREGD)
                ? DateTime.ParseExact(markgr.INTREGD, formatyyyyMMdd, CultureInfo.InvariantCulture)
                : (DateTime?)null;

            // 4
            tzTable.Validity4 = !string.IsNullOrEmpty(markgr.EXPDATE)
                ? DateTime.ParseExact(markgr.EXPDATE, formatyyyyMMdd, CultureInfo.InvariantCulture)
                : (DateTime?)null;


            // 5
            var exn = markgr.EXN?.FirstOrDefault(e =>
                (e.DESPG != null && e.DESPG.DCPCD.Contains(KZ)) || (e.DESPG2 != null && e.DESPG2.DCPCD.Contains(KZ)));
            tzTable.DateOfDistributionRK5 = !string.IsNullOrEmpty(exn?.REGEDAT)
                ? DateTime.ParseExact(exn.REGEDAT, formatyyyyMMdd, CultureInfo.InvariantCulture)
                : (DateTime?)null;

            // 6
            if (markgr.CURRENT?.HOLGR?.ADDRESS != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string addrl in markgr.CURRENT.HOLGR.ADDRESS.ADDRL)
                {
                    sb.Append(addrl);
                    sb.Append(" ");
                }
                var address = sb.ToString().Trim();

                sb = new StringBuilder();
                foreach (string nl in markgr.CURRENT.HOLGR.NAME.NAMEL)
                {
                    sb.Append(nl);
                    sb.Append(" ");
                }
                var namel = sb.ToString().Trim();

                tzTable.Patentee6 = new PatentHolder()
                {
                    Address = new Address()
                    {
                        ADDRL = address,
                        COUNTRY = markgr.CURRENT.HOLGR.ADDRESS.COUNTRY
                    },
                    CLID = markgr.CURRENT.HOLGR.CLID,
                    NOTLANG = markgr.CURRENT.HOLGR.NOTLANG,
                    NameL = namel,
                    ENTNATL = markgr.CURRENT.HOLGR.ENTNATL,
                    NATLTY = markgr.CURRENT.HOLGR.NATLTY,
                };
            }

            if (markgr.CURRENT != null)
            {
                // 7
                var imageZip = _zipArchive.Entries.FirstOrDefault(ent =>
                    (markgr.CURRENT.IMAGE.NAME + "." + markgr.CURRENT.IMAGE.TYPE).ToLower()
                    .Contains(ent.Name.ToLower()));
                if (imageZip != null)
                {
                    var imgStream = imageZip.Open();
                    MemoryStream ms = new MemoryStream();
                    imgStream.CopyTo(ms);

                    tzTable.Image7 = new Image()
                    {
                        Colour = markgr.CURRENT.IMAGE.COLOUR,
                        Type = markgr.CURRENT.IMAGE.TYPE,
                        Text = markgr.CURRENT.IMAGE.TEXT,
                        ImageValue = ms.ToArray()
                    };
                }

                // 8
                tzTable.Transliteration8 = markgr.CURRENT.MARTRAN;

                // 9
                StringBuilder sb = new StringBuilder();
                if (markgr.CURRENT.VIENNAGR != null)
                {
                    foreach (string v in markgr.CURRENT.VIENNAGR.VIECLAI)
                    {
                        sb.Append(v);
                        sb.Append(" ");
                    }
                    var viewclais = sb.ToString();

                    sb = new StringBuilder();
                    foreach (string v in markgr.CURRENT.VIENNAGR.VIECLA3)
                    {
                        sb.Append(v);
                        sb.Append(" ");
                    }
                    var viewcla3S = sb.ToString();
                    tzTable.MKIETZ9 = new MKIETZ()
                    {
                        VIENVER = markgr.CURRENT.VIENNAGR.VIENVER,
                        VIECLAIs = viewclais,
                        VIECLA3s = viewcla3S
                    };
                }


                // 10
                if (markgr.CURRENT.BASICGS != null)
                {
                    tzTable.MKTU10 = new MKTU
                    {
                        NICEVER = markgr.CURRENT.BASICGS.NICEVER,
                        GSGRs = new List<Domain.GSGR>(),
                    };
                    foreach (GSGR gsgr in markgr.CURRENT.BASICGS.GSGRs)
                    {
                        tzTable.MKTU10.GSGRs.Add(new Domain.GSGR()
                        {
                            GSTERMEN = gsgr.GSTERMEN,
                            NICCLAI = gsgr.NICCLAI
                        });
                    }
                }
            }

            // 11
            if (markgr.LIN != null && markgr.LIN.Any(l => l.DCPCD.Contains(KZ)))
            {
                foreach (var lin in markgr.LIN.Where(l => l.DCPCD.Contains(KZ)))
                {
                    DateTime? notdate = string.IsNullOrEmpty(lin.NOTDATE)
                        ? (DateTime?)null
                        : DateTime.ParseExact(lin.NOTDATE, formatyyyyMMdd, CultureInfo.InvariantCulture);
                    DateTime? pubdate = string.IsNullOrEmpty(lin.PUBDATE)
                        ? (DateTime?)null
                        : DateTime.ParseExact(lin.PUBDATE, formatyyyyMMdd, CultureInfo.InvariantCulture);
                    DateTime? regedat = string.IsNullOrEmpty(lin.PUBDATE)
                        ? (DateTime?)null
                        : DateTime.ParseExact(lin.REGEDAT, formatyyyyMMdd, CultureInfo.InvariantCulture);
                    DateTime? regrdat = string.IsNullOrEmpty(lin.PUBDATE)
                        ? (DateTime?)null
                        : DateTime.ParseExact(lin.REGRDAT, formatyyyyMMdd, CultureInfo.InvariantCulture);

                    tzTable.MKTUDescription11 = new MKTUDescription
                    {
                        GAZNO = lin.GAZNO,
                        NOTDATE = notdate,
                        PUBDATE = pubdate,
                        REGEDAT = regedat,
                        REGRDAT = regrdat,
                        Limtos = new List<Domain.LIMTO>()
                    };
                    foreach (LIMTO limto in lin.LIMTO)
                    {
                        tzTable.MKTUDescription11.Limtos.Add(new Domain.LIMTO()
                        {
                            NICCLAI = limto.NICCLAI,
                            GSTERMEN = limto.GSTERMEN
                        });
                    }
                }
            }


            if (markgr.CURRENT != null)
            {
                // 12
                tzTable.Colors12 = markgr.CURRENT?.COLCLAGR?.COLCLAEN;

                // 13
                DateTime? priappd = null;
                if (!string.IsNullOrEmpty(markgr.CURRENT.PRIGR?.PRIAPPD))
                {
                    priappd = DateTime.ParseExact(markgr.CURRENT.PRIGR.PRIAPPD, formatyyyyMMdd,
                        CultureInfo.InvariantCulture);
                }

                tzTable.PriorityData13 = new PriorityData()
                {
                    PRICP = markgr.CURRENT.PRIGR?.PRICP,
                    PRIAPPD = priappd,
                    PRIAPPN = markgr.CURRENT.PRIGR?.PRIAPPN,
                    TEXTEN = markgr.CURRENT.PRIGR?.TEXTEN
                };

                // 14
                tzTable.Disclaimer14 = markgr.CURRENT.DISCLAIMGR?.DISCLAIMEREN;
            }

            // 15
            tzTable.Status15 = "";
            // 16
            tzTable.TypeOD16 = "";

            // 17.1
            if (markgr.ENN != null)
            {
                foreach (var enn in markgr.ENN)
                {
                    //DateTime? notdate = string.IsNullOrEmpty(markgr.LIN.NOTDATE) ? (DateTime?)null : DateTime.Parse(markgr.ENN.NOTDATE);
                    DateTime? pubdate = string.IsNullOrEmpty(enn.PUBDATE)
                        ? (DateTime?)null
                        : DateTime.ParseExact(enn.PUBDATE, formatyyyyMMdd, CultureInfo.InvariantCulture);
                    //DateTime? regedat = string.IsNullOrEmpty(markgr.LIN.PUBDATE) ? (DateTime?)null : DateTime.Parse(markgr.LIN.REGEDAT);
                    //DateTime? regrdat = string.IsNullOrEmpty(markgr.LIN.PUBDATE) ? (DateTime?)null : DateTime.Parse(markgr.LIN.REGRDAT);
                    tzTable.Registration171 = new List<Registration>
                    {
                        new Registration
                        {
                            GAZNO = enn.GAZNO,
                            PUBDATE = pubdate
                        }
                    };
                }
            }

            // 17.2
            if (markgr.EXN != null)
            {
                tzTable.SubsequentDesignation172 = new List<SubsequentDesignation>();
                foreach (var exnL in markgr.EXN)
                {
                    DateTime? pubdate = string.IsNullOrEmpty(exnL.PUBDATE)
                        ? (DateTime?)null
                        : DateTime.ParseExact(exnL.PUBDATE, formatyyyyMMdd, CultureInfo.InvariantCulture);

                    tzTable.SubsequentDesignation172.Add(
                        new SubsequentDesignation
                        {
                            GAZNO = exnL.GAZNO,
                            PUBDATE = pubdate,
                        });
                }
            }

            // 17.3
            if (markgr.CENS != null)
            {
                //var cenL = markgr.CENS.FirstOrDefault(e => e.DCPCD != null && e.DCPCD.Contains(BG));
                //if (cenL != null)
                tzTable.ContinuationOfEffect173 = new List<ContinuationOfEffect>();
                foreach (var cenL in markgr.CENS)
                {
                    DateTime? pubdate = string.IsNullOrEmpty(cenL.PUBDATE)
                        ? (DateTime?)null
                        : DateTime.ParseExact(cenL.PUBDATE, formatyyyyMMdd, CultureInfo.InvariantCulture);

                    tzTable.ContinuationOfEffect173.Add(
                        new ContinuationOfEffect()
                        {
                            GAZNO = cenL.GAZNO,
                            PUBDATE = pubdate,
                        }
                    );
                }
            }

            // 17.4
            if (markgr.RFNT != null)
            {
                tzTable.TotalProvisionalRefusalOfProtection174 = new List<TotalProvisionalRefusalOfProtection>();
                foreach (var rfnt in markgr.RFNT)
                {
                    DateTime? pubdate = string.IsNullOrEmpty(rfnt.PUBDATE)
                        ? (DateTime?)null
                        : DateTime.ParseExact(rfnt.PUBDATE, formatyyyyMMdd, CultureInfo.InvariantCulture);

                    tzTable.TotalProvisionalRefusalOfProtection174.Add(new TotalProvisionalRefusalOfProtection()
                    {
                        GAZNO = rfnt.GAZNO,
                        PUBDATE = pubdate
                    });
                }
            }

            // 17.5
            if (markgr.FINV != null)
            {
                tzTable.StatementIndicatingThatTheMarkIsProtected175 =
                    new List<StatementIndicatingThatTheMarkIsProtected>();
                foreach (var finv in markgr.FINV)
                {
                    DateTime? notdate = string.IsNullOrEmpty(finv.NOTDATE)
                        ? (DateTime?)null
                        : DateTime.ParseExact(finv.NOTDATE, formatyyyyMMdd, CultureInfo.InvariantCulture);
                    DateTime? pubdate = string.IsNullOrEmpty(finv.PUBDATE)
                        ? (DateTime?)null
                        : DateTime.ParseExact(finv.PUBDATE, formatyyyyMMdd, CultureInfo.InvariantCulture);
                    DateTime? regedat = string.IsNullOrEmpty(finv.REGEDAT)
                        ? (DateTime?)null
                        : DateTime.ParseExact(finv.REGEDAT, formatyyyyMMdd, CultureInfo.InvariantCulture);
                    DateTime? regrdat = string.IsNullOrEmpty(finv.REGRDAT)
                        ? (DateTime?)null
                        : DateTime.ParseExact(finv.REGRDAT, formatyyyyMMdd, CultureInfo.InvariantCulture);

                    tzTable.StatementIndicatingThatTheMarkIsProtected175.Add(
                        new StatementIndicatingThatTheMarkIsProtected()
                        {
                            GAZNO = finv.GAZNO,
                            PUBDATE = pubdate,
                            NOTDATE = notdate,
                            REGEDAT = regedat,
                            REGRDAT = regrdat,
                            INTOFF = finv.INTOFF
                        });
                }
            }

            // 17.6
            if (markgr.REN != null)
            {
                tzTable.Renewal176 = new List<Renewal>();
                foreach (var ren in markgr.REN)
                {
                    DateTime? pubdate = string.IsNullOrEmpty(ren.PUBDATE)
                        ? (DateTime?)null
                        : DateTime.ParseExact(ren.PUBDATE, formatyyyyMMdd, CultureInfo.InvariantCulture);

                    tzTable.Renewal176.Add(new Renewal()
                    {
                        GAZNO = ren.GAZNO,
                        PUBDATE = pubdate
                    });
                }
            }

            // 17.7
            if (markgr.LIN != null)
            {
                tzTable.Limitation177 = new List<Limitation>();
                foreach (var lin in markgr.LIN)
                {
                    DateTime? pubdate = string.IsNullOrEmpty(lin.PUBDATE)
                        ? (DateTime?)null
                        : DateTime.ParseExact(lin.PUBDATE, formatyyyyMMdd, CultureInfo.InvariantCulture);

                    tzTable.Limitation177.Add(new Limitation()
                    {
                        GAZNO = lin.GAZNO,
                        PUBDATE = pubdate
                    });
                }
            }

            // 17.8
            if (markgr.GP18N != null)
            {
                tzTable.StatementOfGrantOfProtection178 = new List<StatementOfGrantOfProtectionMadeUnderRule>();
                foreach (var gp18n in markgr.GP18N)
                {
                    DateTime? pubdate = string.IsNullOrEmpty(gp18n.PUBDATE)
                        ? (DateTime?)null
                        : DateTime.ParseExact(gp18n.PUBDATE, formatyyyyMMdd, CultureInfo.InvariantCulture);
                    DateTime? regedat = string.IsNullOrEmpty(gp18n.REGEDAT)
                        ? (DateTime?)null
                        : DateTime.ParseExact(gp18n.REGEDAT, formatyyyyMMdd, CultureInfo.InvariantCulture);

                    tzTable.StatementOfGrantOfProtection178.Add(
                        new StatementOfGrantOfProtectionMadeUnderRule()
                        {
                            GAZNO = gp18n.GAZNO,
                            PUBDATE = pubdate,
                            REGEDAT = regedat,
                            INTOFF = gp18n.INTOFF
                        }
                    );
                }
            }

            // 17.9
            if (markgr.ISN != null)
            {
                tzTable.Isn179 = new List<ExOfficioExaminationCompleted>();
                foreach (var isn in markgr.ISN)
                {
                    DateTime? pubdate = string.IsNullOrEmpty(isn.PUBDATE)
                        ? (DateTime?)null
                        : DateTime.ParseExact(isn.PUBDATE, formatyyyyMMdd, CultureInfo.InvariantCulture);
                    DateTime? regedat = string.IsNullOrEmpty(isn.REGEDAT)
                        ? (DateTime?)null
                        : DateTime.ParseExact(isn.REGEDAT, formatyyyyMMdd, CultureInfo.InvariantCulture);

                    tzTable.Isn179.Add(new ExOfficioExaminationCompleted()
                    {
                        GAZNO = isn.GAZNO,
                        PUBDATE = pubdate,
                        REGEDAT = regedat,
                        INTOFF = isn.INTOFF
                    }
                    );
                }
            }

            // 17.10
            if (markgr.R18NT != null)
            {
                tzTable.ConfirmationTotalProvisional1710 = new List<ConfirmationOfTotalProvisionalRefusal>();
                foreach (var r18nt in markgr.R18NT)
                {
                    DateTime? pubdate = string.IsNullOrEmpty(r18nt.PUBDATE)
                        ? (DateTime?)null
                        : DateTime.ParseExact(r18nt.PUBDATE, formatyyyyMMdd, CultureInfo.InvariantCulture);
                    DateTime? regedat = string.IsNullOrEmpty(r18nt.REGEDAT)
                        ? (DateTime?)null
                        : DateTime.ParseExact(r18nt.REGEDAT, formatyyyyMMdd, CultureInfo.InvariantCulture);

                    tzTable.ConfirmationTotalProvisional1710.Add(new ConfirmationOfTotalProvisionalRefusal()
                    {
                        GAZNO = r18nt.GAZNO,
                        PUBDATE = pubdate,
                        REGEDAT = regedat,
                        INTOFF = r18nt.INTOFF
                    }
                    );
                }
            }

            // 17.11
            if (markgr.INNP != null)
            {
                tzTable.PartialInvalidationl1711 = new List<PartialInvalidation>();
                foreach (var innp in markgr.INNP)
                {
                    DateTime? pubdate = string.IsNullOrEmpty(innp.PUBDATE)
                        ? (DateTime?)null
                        : DateTime.ParseExact(innp.PUBDATE, formatyyyyMMdd, CultureInfo.InvariantCulture);
                    DateTime? regedat = string.IsNullOrEmpty(innp.REGEDAT)
                        ? (DateTime?)null
                        : DateTime.ParseExact(innp.REGEDAT, formatyyyyMMdd, CultureInfo.InvariantCulture);

                    tzTable.PartialInvalidationl1711.Add(
                        new PartialInvalidation()
                        {
                            GAZNO = innp.GAZNO,
                            PUBDATE = pubdate,
                            REGEDAT = regedat,
                            INTOFF = innp.INTOFF
                        }
                    );
                }
            }

            // 17.12
            if (markgr.R18NP != null)
            {
                tzTable.R18Np1712 = new List<StatementIndicatingTheGoodsAndServices>();
                foreach (var r18np in markgr.R18NP)
                {
                    DateTime? pubdate = string.IsNullOrEmpty(r18np.PUBDATE)
                        ? (DateTime?)null
                        : DateTime.ParseExact(r18np.PUBDATE, formatyyyyMMdd, CultureInfo.InvariantCulture);
                    //DateTime? regedat = string.IsNullOrEmpty(r18np.REGEDAT) ? (DateTime?)null : DateTime.Parse(r18np.REGEDAT);

                    tzTable.R18Np1712.Add(new StatementIndicatingTheGoodsAndServices()
                    {
                        GAZNO = r18np.GAZNO,
                        PUBDATE = pubdate,
                        //REGEDAT = regedat,
                        INTOFF = r18np.INTOFF
                    });
                }
            }

            // 17.13
            if (markgr.FDNV != null)
            {
                tzTable.Fdnp1713 = new List<FurtherStatementUnderRule>();
                foreach (var fdnv in markgr.FDNV)
                {
                    DateTime? pubdate = string.IsNullOrEmpty(fdnv.PUBDATE)
                        ? (DateTime?)null
                        : DateTime.ParseExact(fdnv.PUBDATE, formatyyyyMMdd, CultureInfo.InvariantCulture);
                    //DateTime? regedat = string.IsNullOrEmpty(r18np.REGEDAT) ? (DateTime?)null : DateTime.Parse(r18np.REGEDAT);

                    tzTable.Fdnp1713.Add(
                        new FurtherStatementUnderRule()
                        {
                            GAZNO = fdnv.GAZNO,
                            PUBDATE = pubdate,
                            //REGEDAT = regedat,
                            INTOFF = fdnv.INTOFF
                        }
                    );
                }
            }

            // 17.14
            if (markgr.OPN != null)
            {
                tzTable.Opn1714 = new List<OppositionPossibleAfterTimeLimit>();
                foreach (var opn in markgr.OPN)
                {
                    DateTime? pubdate = string.IsNullOrEmpty(opn.PUBDATE)
                        ? (DateTime?)null
                        : DateTime.ParseExact(opn.PUBDATE, formatyyyyMMdd, CultureInfo.InvariantCulture);
                    //DateTime? regedat = string.IsNullOrEmpty(r18np.REGEDAT) ? (DateTime?)null : DateTime.Parse(r18np.REGEDAT);

                    tzTable.Opn1714.Add(
                        new OppositionPossibleAfterTimeLimit()
                        {
                            GAZNO = opn.GAZNO,
                            PUBDATE = pubdate,
                            //REGEDAT = regedat,
                            INTOFF = opn.INTOFF
                        }
                     );
                }
            }

            // 17.15
            if (markgr.R18NV != null)
            {
                tzTable.R18Nv1715 = new List<StatementOfGrantOfProtectionFollowingProvisionalRefusal>();
                foreach (var r18Nv in markgr.R18NV)
                {
                    DateTime? pubdate = string.IsNullOrEmpty(r18Nv.PUBDATE)
                        ? (DateTime?)null
                        : DateTime.ParseExact(r18Nv.PUBDATE, formatyyyyMMdd, CultureInfo.InvariantCulture);
                    //DateTime? regedat = string.IsNullOrEmpty(r18np.REGEDAT) ? (DateTime?)null : DateTime.Parse(r18np.REGEDAT);

                    tzTable.R18Nv1715.Add(new StatementOfGrantOfProtectionFollowingProvisionalRefusal()
                    {
                        GAZNO = r18Nv.GAZNO,
                        PUBDATE = pubdate,
                        //REGEDAT = regedat,
                        INTOFF = r18Nv.INTOFF
                    });
                }
            }

            // 17.16
            if (markgr.RFNP != null)
            {
                tzTable.Rfnp1716 = new List<PartialProvisionalRefusalOfProtection>();
                foreach (var frnp in markgr.RFNP)
                {
                    DateTime? pubdate = string.IsNullOrEmpty(frnp.PUBDATE)
                        ? (DateTime?)null
                        : DateTime.ParseExact(frnp.PUBDATE, formatyyyyMMdd, CultureInfo.InvariantCulture);
                    DateTime? regedat = string.IsNullOrEmpty(frnp.REGEDAT)
                        ? (DateTime?)null
                        : DateTime.ParseExact(frnp.REGEDAT, formatyyyyMMdd, CultureInfo.InvariantCulture);
                    DateTime? regrdat = string.IsNullOrEmpty(frnp.REGRDAT)
                        ? (DateTime?)null
                        : DateTime.ParseExact(frnp.REGRDAT, formatyyyyMMdd, CultureInfo.InvariantCulture);

                    tzTable.Rfnp1716.Add(new PartialProvisionalRefusalOfProtection()
                    {
                        GAZNO = frnp.GAZNO,
                        PUBDATE = pubdate,
                        REGEDAT = regedat,
                        REGRDAT = regrdat,
                        INTOFF = frnp.INTOFF
                    });
                }
            }
            #endregion

            localContext.TzTables.Add(tzTable);
            //localContext.SaveChanges();

            //}
            return localContext;
        }


        private void InitContext()
        {
            _context?.Dispose();
            
            var builder = new DbContextOptionsBuilder<NiisContext>();
            var connectionString = ConfigurationManager.AppSettings["ConnectionString"];
            builder.UseSqlServer(connectionString);
            _context = new NiisContext(builder.Options);
        }
    }
}