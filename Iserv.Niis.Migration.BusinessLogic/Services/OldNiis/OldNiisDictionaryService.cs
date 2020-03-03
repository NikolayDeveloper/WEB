using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Entities.Dictionaries.Location;
using Iserv.Niis.Migration.BusinessLogic.Abstract;
using Iserv.Niis.Migration.BusinessLogic.Common;
using Iserv.Niis.Migration.BusinessLogic.Utils;
using Iserv.OldNiis.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Domain.Entities.Integration;
using Iserv.Niis.Domain.Entities.Other;

namespace Iserv.Niis.Migration.BusinessLogic.Services.OldNiis
{
    public class OldNiisDictionaryService : BaseService
    {
        private readonly OldNiisContext _context;
        private readonly DictionaryTypesHelper _dictionaryTypesHelper;

        public OldNiisDictionaryService(
            OldNiisContext context,
            DictionaryTypesHelper dictionaryTypesHelper)
        {
            _context = context;
            _dictionaryTypesHelper = dictionaryTypesHelper;
        }

        public List<int> GetCountriesIds()
        {
            return _context.Locations.Where(l => l.Id == RefMainTypeId.Country).Select(l => l.Id).ToList();
        }

        public List<int> GetContinentsIds()
        {
            return _context.Locations.Where(l => l.Id == RefMainTypeId.Continent).Select(l => l.Id).ToList();
        }

        public List<DicDivision> GetDicDivisions()
        {
            var divisions = _context.Departments
                .AsNoTracking()
                .Where(d => d.TypeId == SPMainId.Division)
                .OrderBy(d => d.Id)
                .ToList();

            return divisions.Select(d => new DicDivision
            {
                Id = d.Id,
                Code = d.Code,
                NameRu = d.NameRu,
                NameKz = d.NameKz,
                NameEn = d.NameEn,
                IsMonitoring = CustomConverter.StringToBool(d.IsMonitoring),
            }).ToList();
        }

        public List<DicRoute> GetDicRoutes()
        {
            var routes = _context.Worktypes.AsNoTracking().OrderBy(r => r.Id).ToList();

            return routes.Select(r => new DicRoute
            {
                Id = r.Id,
                Code = r.Code,
                Description = r.Description,
                NameEn = r.NameEn,
                NameKz = r.NameKz,
                NameRu = r.NameRu
            }).ToList();
        }

        public List<DicDepartmentType> GetDicDepartmentTypes()
        {
            var departmentTypes = _context.MainRef
                .AsNoTracking()
                .Where(d => d.TypeId == SPMainId.DepartamentType)
                .OrderBy(d => d.Id)
                .ToList();

            return departmentTypes.Select(d => new DicDepartmentType
            {
                Id = d.Id,
                Code = d.Code,
                Description = d.Description,
                NameEn = d.NameEn,
                NameKz = d.NameKz,
                NameRu = d.NameRu
            }).ToList();
        }

        public List<DicDepartment> GetDicDepartments()
        {
            var departmentTypeIds = new[] { RefMainTypeId.Department, RefMainTypeId.Office, RefMainTypeId.Branch };

            var departments = _context.Departments
                .AsNoTracking()
                .Where(d => departmentTypeIds.Contains(d.TypeId))
                .OrderBy(d => d.Id);

            return departments.Select(d => new DicDepartment
            {
                Id = d.Id,
                DepartmentTypeId = d.TypeId,
                DivisionId = d.ParentId ?? 0,
                Code = d.Code,
                NameEn = d.NameEn,
                NameKz = d.NameKz,
                NameRu = d.NameRu,
                TNameRu = d.TNameRu,
                IsMonitoring = CustomConverter.StringToBool(d.IsMonitoring)
            }).ToList();
        }

        public List<DicPosition> GetDicPositions()
        {
            var positions = _context.Departments
                .AsNoTracking()
                .Where(d => d.TypeId == SPMainId.Position).
                OrderBy(p => p.Id);

            return positions.Select(p => new DicPosition
            {
                Id = p.Id,
                DepartmentId = p.ParentId,
                Code = p.Code,
                PositionTypeId = 1,
                NameEn = p.NameEn,
                NameKz = p.NameKz,
                NameRu = p.NameRu,
                IsMonitoring = CustomConverter.StringToBool(p.IsMonitoring)
            }).ToList();
        }

        public List<DicContinent> GetDicContinents()
        {
            var continents = _context.Locations
                .AsNoTracking()
                .Where(l => l.TypeId == RefMainTypeId.Continent)
                .OrderBy(l => l.Id)
                .ToList();

            return continents.Select(c => new DicContinent
            {
                Id = c.Id,
                Code = c.Code,
                NameEn = c.NameEn,
                NameKz = c.NameKz,
                NameRu = c.NameRu,
                ParentId = c.ParentId,
                Order = c.OrderId,
            }).ToList();
        }

        public List<DicCountry> GetDicCountries()
        {
            var countries = _context.Locations
                .AsNoTracking()
                .Where(l => l.TypeId == RefMainTypeId.Country)
                .OrderBy(l => l.Id)
                .ToList();

            return countries.Select(c => new DicCountry
            {
                Id = c.Id,
                Code = c.Code,
                NameEn = c.NameEn,
                NameKz = c.NameKz,
                NameRu = c.NameRu,
                ContinentId = c.ParentId,
                Order = c.OrderId
            }).ToList();
        }

        public List<DicCustomerType> GetDicCustomerTypes()
        {
            var customerTypes = _context.MainRef
                .AsNoTracking()
                .Where(t => t.TypeId == RefMainTypeId.CustomerType)
                .OrderBy(t => t.Id)
                .ToList();

            return customerTypes.Select(t => new DicCustomerType
            {
                Id = t.Id,
                Code = t.Code,
                NameEn = t.NameEn,
                NameKz = t.NameKz,
                NameRu = t.NameRu,
                Description = t.Description,
            }).ToList();
        }

        public (List<DicCustomer> customers, List<CustomerAttorneyInfo> customerAttorneyInfos) GetDicCustomersAndCustomerAttorneyInfos(int packageSize, int lastId)
        {
            var oldCustomers = _context.WtCustomers
                .AsNoTracking()
                .OrderBy(c => c.Id)
                .Where(c => c.Id != 0 && c.Id > lastId)
                .Take(packageSize)
                .ToList();

            var oldCountriesIds = oldCustomers.Where(d => d.CountryId != null).Select(d => d.CountryId.GetValueOrDefault(0)).Distinct().ToList();
            var countries = _dictionaryTypesHelper.GetCountryIds(oldCountriesIds);

            var customers = oldCustomers.Select(c => new DicCustomer
            {
                Id = c.Id,
                TypeId = c.TypeId,
                IsSMB = CustomConverter.StringToNullableBool(c.flIsSmb),
                Xin = c.flXIN,
                NameEn = c.CusNameMlEn,
                NameEnLong = c.CusNameMlEnLong,
                NameKz = c.CusNameMlKz,
                NameKzLong = c.CusNameMlKzLong,
                NameRu = c.CusNameMlRu,
                NameRuLong = c.CusNameMlRuLong,
                ShortDocContent = c.flShortDocContent,
                Rnn = c.RTN,
                ContactName = c.ContactFace,
                CountryId = countries.Any(d => d == c.CountryId) ? c.CountryId : null,
                Email = c.EMail,
                PhoneFax = c.Fax,
                ApplicantsInfo = c.flApplicantsInfo,
                CertificateNumber = c.flCertificateNumber,
                CertificateSeries = c.flCertificateSeries,
                NotaryName = c.flNotaryName,
                Opf = c.flOpf,
                PowerAttorneyDateIssue = c.flPowerAttorneyDateIssue,
                PowerAttorneyFullNum = c.flPowerAttorneyFullNum,
                RegDate = c.flRegDate,
                JurRegNumber = c.JurRegNumber,
                Login = c.Login,
                Password = c.Password_,
                Phone = c.Phone,
                Subscript = c.Subscript,
            }).ToList();

            var customerAttorneyInfos = oldCustomers.Select(c => new CustomerAttorneyInfo
            {
                RegCode = c.AttCode,
                DateBeginStop = c.AttDateBeginStop,
                DateCard = c.AttDateCard,
                DateDisCard = c.AttDateDiscard,
                DateEndStop = c.AttDateEndStop,
                Education = c.AttEducation,
                Language = c.AttLang,
                PaymentOrder = c.AttPlatpor,
                PublicRedefine = c.AttPublicRedefine,
                Redefine = c.AttRedefine,
                SomeDate = c.AttSomeDate,
                FieldOfKnowledge = c.AttSphereKnow,
                FieldOfActivity = c.AttSphereWork,
                GovReg = c.AttStatReg,
                GovRegDate = c.AttStatRegDate,
                WorkPlace = c.AttWorkPlace,
                CustomerId = c.Id,
            }).ToList();

            return (customers, customerAttorneyInfos);
        }

        public int GetDicCustomersCount()
        {
            return _context.WtCustomers
                .AsNoTracking()
                .OrderBy(c => c.Id)
                .Where(c => c.Id != 0)
                .Count();
        }

        public List<DicProtectionDocType> GetDicProtectionDocTypes()
        {
            var patentTypes = _context.TypePatents
                .AsNoTracking()
                .ToList();

            var dicRoutes = GetDicRoutes();

            var protectionDocTypes = patentTypes.Select(p => new DicProtectionDocType
            {
                Id = p.Id,
                Code = p.Code,
                DepatmentId = p.DepartmentId,
                NameRu = p.NameRu,
                NameEn = p.NameEn,
                NameKz = p.NameKz,
                Description = p.Description,
                DkCode = p.DKCode,
                DocTypeText = p.DocTypeText,
                DocTypeTextKz = p.DocTYpeTextKz,
                RouteId = _dictionaryTypesHelper.GetDicRouteIdForDicProtectionDocType(p.Code, dicRoutes)
            }).ToList();

            // Добавляем устаревшие типы для истории

            var lastId = protectionDocTypes.Last().Id;
            protectionDocTypes.Add(new DicProtectionDocType
            {
                Id = ++lastId,
                Code = DicProtectionDocTypeCodes.ProtectionDocTypeTrademarkGenerallyKnownCode,
                NameRu = "Заявка на признание ТЗ общеизвестным",
                RouteId = _dictionaryTypesHelper.GetDicRouteIdForDicProtectionDocType(DicProtectionDocTypeCodes.ProtectionDocTypeTrademarkGenerallyKnownCode, dicRoutes),
            });
            protectionDocTypes.Add(new DicProtectionDocType
            {
                Id = ++lastId,
                Code = DicProtectionDocTypeCodes.ProtectionDocTypeEapoCode,
                NameRu = "Заявка на ИЗ_ЕАПО",
                RouteId = _dictionaryTypesHelper.GetDicRouteIdForDicProtectionDocType(DicProtectionDocTypeCodes.ProtectionDocTypeEapoCode, dicRoutes),
            });
            protectionDocTypes.Add(new DicProtectionDocType
            {
                Id = ++lastId,
                Code = DicProtectionDocTypeCodes.ProtectionDocTypeRstCode,
                NameRu = "Заявка на ИЗ_PCT",
                RouteId = _dictionaryTypesHelper.GetDicRouteIdForDicProtectionDocType(DicProtectionDocTypeCodes.ProtectionDocTypeRstCode, dicRoutes),
            });

            return protectionDocTypes;
        }

        public List<IntegrationConServiceStatus> GetIntegrationConServiceStatuses()
        {
            var statuses = _context.IntegrationConServiceStatuses.AsNoTracking().OrderBy(s => s.Id).ToList();

            return statuses.Select(s => new IntegrationConServiceStatus
            {
                Id = s.Id,
                Code = s.Code,
                NameEn = s.NameEn,
                NameRu = s.NameRu,
                NameKz = s.NameKz,
                DateCreate = DateTimeOffset.Now,
                DateUpdate = DateTimeOffset.Now,
            }).ToList();
        }

        public List<DicRouteStage> GetDicRouteStages()
        {
            var stages = _context.Stages
                .AsNoTracking()
                .OrderBy(s => s.Id)
                .ToList();
            return stages.Select(s => new DicRouteStage
            {
                Id = s.Id,
                Code = s.Code,
                OnlineRequisitionStatusId = s.OnlineStatusId,
                Interval = s.Interval,
                IsFirst = CustomConverter.StringToBool(s.IsFirst),
                IsLast = CustomConverter.StringToBool(s.IsLast),
                IsMultiUser = CustomConverter.StringToBool(s.IsMultyUser),
                IsReturnable = CustomConverter.StringToNullableBool(s.IsReturning),
                IsSystem = CustomConverter.StringToBool(s.IsSystem),
                NameRu = s.NameRu,
                NameEn = s.NameEn,
                NameKz = s.NameKz,
                Description = s.Description,
                RouteId = s.WorktypeId,
                ExpirationType = Domain.Enums.ExpirationType.CalendarDay,
                ExpirationValue = (short?)s.IntervalDays,
            }).ToList();
        }

        public List<DicOnlineRequisitionStatus> GetDicOnlineRequisitionStatuses()
        {
            var onlineStatuses = _context.MainRef
                .AsNoTracking()
                .Where(s => s.TypeId == RefMainTypeId.OnlineRequestionStatus)
                .OrderBy(s => s.Id).ToList();

            return onlineStatuses.Select(s => new DicOnlineRequisitionStatus
            {
                Id = s.Id,
                Code = s.Code,
                NameEn = s.NameEn,
                NameKz = s.NameKz,
                NameRu = s.NameRu,
                Description = s.Description
            }).ToList();
        }

        public List<DicReceiveType> GetDicReceiveTypes()
        {
            var receiveTypes = _context.MainRef
                .AsNoTracking()
                .Where(d => d.TypeId == RefMainTypeId.ReceiveType)
                .ToList();

            return receiveTypes.Select(r => new DicReceiveType
            {
                Id = r.Id,
                Code = r.Code,
                NameRu = r.NameRu,
                NameEn = r.NameEn,
                NameKz = r.NameKz,
                Description = r.Description
            }).ToList();
        }

        public List<DicRequestStatus> GetDicRequestStatuses()
        {
            var requestStatuses = _context.MainRef
                .AsNoTracking()
                .Where(s => s.TypeId == RefMainTypeId.PatentStatus)
                .OrderBy(s => s.Id)
                .ToList();

            return requestStatuses.Select(s => new DicRequestStatus
            {
                Id = s.Id,
                Code = s.Code,
                Description = s.Description,
                NameEn = s.NameEn,
                NameKz = s.NameKz,
                NameRu = s.NameRu
            }).ToList();
        }

        public List<DicProtectionDocStatus> GetDicProtectionDocStatuses()
        {
            var requestStatuses = _context.MainRef
                .AsNoTracking()
                .Where(s => s.TypeId == RefMainTypeId.PatentStatus)
                .OrderBy(s => s.Id)
                .ToList();

            return requestStatuses.Select(s => new DicProtectionDocStatus
            {
                Id = s.Id,
                Code = s.Code,
                Description = s.Description,
                NameEn = s.NameEn,
                NameKz = s.NameKz,
                NameRu = s.NameRu
            }).ToList();
        }

        public List<DicContractStatus> GetDicContractStatuses()
        {
            var requestStatuses = _context.MainRef
                .AsNoTracking()
                .Where(s => s.TypeId == RefMainTypeId.PatentStatus)
                .OrderBy(s => s.Id)
                .ToList();

            return requestStatuses.Select(s => new DicContractStatus
            {
                Id = s.Id,
                Code = s.Code,
                Description = s.Description,
                NameEn = s.NameEn,
                NameKz = s.NameKz,
                NameRu = s.NameRu
            }).ToList();
        }

        public List<DicTypeTrademark> GetDicTypeTrademarks()
        {
            return new List<DicTypeTrademark>
            {
                new DicTypeTrademark { Code = "01", NameRu = "Буквенный" },
                new DicTypeTrademark { Code = "02", NameRu = "Голографический" },
                new DicTypeTrademark { Code = "03", NameRu = "Звуковой" },
                new DicTypeTrademark { Code = "04", NameRu = "Изобразительный " },
                new DicTypeTrademark { Code = "05", NameRu = "Комбинированный" },
                new DicTypeTrademark { Code = "06", NameRu = "Обонятельный" },
                new DicTypeTrademark { Code = "07", NameRu = "Объемный" },
                new DicTypeTrademark { Code = "08", NameRu = "Словесный" },
                new DicTypeTrademark { Code = "09", NameRu = "Цифровой" },
                new DicTypeTrademark { Code = "10", NameRu = "Прочие" }
            };
        }

        public List<DicSelectionAchieveType> GetDicSelectionAchieveTypes()
        {
            return new List<DicSelectionAchieveType>
            {
                new DicSelectionAchieveType { Code = "01", NameRu = "Растениеводство"},
                new DicSelectionAchieveType { Code = "02", NameRu = "Животноводство"},
                new DicSelectionAchieveType { Code = "03", NameRu = "Сорта винограда, древесных декоративных, плодовых и лесных культур, в том числе их подвоев" },
            };
        }

        public List<DicBeneficiaryType> GetDicBeneficiaryTypes()
        {
            return new List<DicBeneficiaryType>
            {
                new DicBeneficiaryType { Code = "SMB", NameRu = "Субъекты малого и среднего бизнеса" },
                new DicBeneficiaryType { Code = "VET", NameRu = "Участники Великой отечественной войны, инвалиды, учащиеся общеобразовательных школ и колледжей, студенты высших учебных заведений, пенсионеры по возрасту и выслуге лет" },
            };
        }

        public List<DicConventionType> GetDicConventionTypes()
        {
            var types = _context.IITypes.AsNoTracking().OrderBy(t => t.Id).ToList();

            return types.Select(t => new DicConventionType
            {
                Id = t.Id,
                Code = t.Code,
                NameEn = t.NameEn,
                NameKz = t.NameKz,
                NameRu = t.NameRu,
                Description = t.Description
            }).ToList();
        }

        public List<DicPaymentStatus> GetDicPaymentStatuses()
        {
            return new List<DicPaymentStatus>
            {
                new DicPaymentStatus { Id = 1, Code = "notpaid", NameEn = "Not paid", NameRu = "Не оплачено"},
                new DicPaymentStatus { Id = 2, Code = "credited", NameEn = "Credited", NameRu = "Зачтено"},
                new DicPaymentStatus { Id = 3, Code = "charged", NameEn = "Charged", NameRu = "Списано"},
            };
        }

        public List<DicColorTZ> GetDicColorTZs()
        {
            var colors = _context.ClTmIColors.AsNoTracking().OrderBy(c => c.U_ID).ToList();

            return colors.Select(c => new DicColorTZ
            {
                Id = c.U_ID,
                Code = c.CODE,
                NameEn = c.NAME_ML_EN,
                NameKz = c.NAME_ML_KZ,
                NameRu = c.NAME_ML_RU
            }).ToList();
        }

        public List<DicCustomerRole> GetDicCustomerRoles()
        {
            var roles = _context.CustomerPatents.AsNoTracking().OrderBy(r => r.Id).ToList();

            return roles.Select(r => new DicCustomerRole
            {
                Id = r.Id,
                Code = r.Code,
                NameEn = r.NameEn,
                NameKz = r.NameKz,
                NameRu = r.NameRu
            }).ToList();
        }

        public List<DicICFEM> GetDicICFEMs()
        {
            var icfems = _context.tmICFEMs.AsNoTracking().OrderBy(i => i.Id).ToList();

            return icfems.Select(i => new DicICFEM
            {
                Id = i.Id,
                Code = i.Code,
                NameEn = i.NameEn,
                NameKz = i.NameKz,
                NameRu = i.NameRu,
                ParentId = i.ParentId
            }).ToList();
        }

        public List<DicEarlyRegType> GetDicEarlyRegTypes()
        {
            var types = _context.EarlyTypes.AsNoTracking().OrderBy(t => t.Id).ToList();

            return types.Select(t => new DicEarlyRegType
            {
                Id = t.Id,
                Code = t.Code,
                NameEn = t.NameEn,
                NameKz = t.NameKz,
                NameRu = t.NameRu,
                ProtectionDocTypeId = t.TypeId
            }).ToList();
        }

        public List<DicICGS> GetDicICGSs()
        {
            var icgs = _context.ICGSes.AsNoTracking().OrderBy(i => i.Id).ToList();

            var anotherIcgsId = new[] { 2, 50, 51 };

            return icgs.Select(i => new DicICGS
            {
                Id = i.Id,
                Code = anotherIcgsId.Contains(i.Id) ? i.Code : i.Code + "0000",
                NameEn = i.NameEn,
                NameKz = i.NameKz,
                NameRu = i.NameRu,
                Description = i.FullDesc,
                DescriptionShort = i.ShortDesc,
            }).ToList();
        }

        public List<DicICIS> GetDicICISs()
        {
            var icis = _context.ICISes.AsNoTracking().OrderBy(i => i.Id).ToList();

            return icis.Select(i => new DicICIS
            {
                Id = i.Id,
                Code = i.Code,
                NameEn = i.NameEn,
                NameKz = i.NameKz,
                NameRu = i.NameRu,
                Description = i.Description,
                ParentId = i.ParentId,
                RevisionNumber = i.RevisionId
            }).ToList();
        }

        public List<DicDocumentClassification> GetDicDocumentClassifications()
        {
            var documentClassifications = _context.Templates
                .AsNoTracking()
                .ToList();
            return documentClassifications.Select(d => new DicDocumentClassification
            {
                Id = d.Id,
                ParentId = d.ParentId,
                Code = d.Code,
                NameRu = d.NameRu,
                NameEn = d.NameEn,
                NameKz = d.NameKz,
                Description = d.Description
            }).ToList();
        }

        public List<DicDocumentType> GetDicDocumentTypes()
        {
            var documentTypes = _context.Documents
                .AsNoTracking()
                .ToList();

            return documentTypes.Select(d => new DicDocumentType
            {
                Id = d.Id,
                Code = d.Code,
                NameRu = d.NameRu,
                NameEn = d.NameEn,
                NameKz = d.NameKz,
                Description = d.Description,
                ClassificationId = d.DocumentClassificationId,
                RouteId = d.WorktypeId,
                IsRequireSigning = CustomConverter.StringToNullableBool(d.IsSigningRequire),
                IsUnique = CustomConverter.StringToNullableBool(d.IsUnique),
                Order = d.Order,
                // TemplateFileId = d.Template != null ? d.Id : default(int?),
            }).ToList();
        }

        public List<DicIPC> GetDicIPCs()
        {
            var ipcs = _context.IPC2s.AsNoTracking().OrderBy(i => i.Id).ToList();

            return ipcs.Select(i => new DicIPC
            {
                Id = i.Id,
                Code = i.Code,
                NameEn = i.NameEn,
                NameKz = i.NameKz,
                NameRu = i.NameRu,
                Description = i.Description,
                ParentId = i.ParentId,
                RevisionNumber = i.RevisionId,
                Kind = i.Kind,
                EntryLevel = i.EntryLevel,
                EntryType = i.EntryType,
            }).ToList();
        }

        public List<DicProtectionDocSubType> GetDicProtectionDocSubTypes()
        {
            var oldProtectionDocSubTypes = _context.SptPatSubts
                .AsNoTracking()
                .ToList();


            var protectionDocSubTypes = oldProtectionDocSubTypes.Select(p => new DicProtectionDocSubType
            {
                Id = p.Id,
                Code = p.Code,
                NameRu = p.NameRu,
                NameEn = p.NameEn,
                NameKz = p.NameKz,
                TypeId = p.TypeId.Value,
                S1 = p.S1,
                S1Kz = p.S1Kz,
                S2 = p.S2,
                S2Kz = p.S2Kz
            }).ToList();

            protectionDocSubTypes.ForEach(p =>
            {
                switch (p.Id)
                {
                    case SPTPatSubtId.UsefulModelEurasianRequest:
                        p.Code = DicProtectionDocSubtypeCodes.EurasianRequestUsefulModel;
                        break;
                    case SPTPatSubtId.UsefulModelNearAbroad:
                        p.Code = DicProtectionDocSubtypeCodes.NearAbroadUsefulModel;
                        break;
                    case SPTPatSubtId.UsefulModelForeign:
                        p.Code = DicProtectionDocSubtypeCodes.ForeignUsefulModel;
                        break;
                    case SPTPatSubtId.UsefulModelEurasianPatent:
                        p.Code = DicProtectionDocSubtypeCodes.EurasianPatentUsefulModel;
                        break;
                    case SPTPatSubtId.UsefulModelInternational:
                        p.Code = DicProtectionDocSubtypeCodes.InternationalUsefulModel;
                        break;
                    case SPTPatSubtId.UsefulModelNational:
                        p.Code = DicProtectionDocSubtypeCodes.NationalUsefulModel;
                        break;
                }
            });

            var lastId = protectionDocSubTypes.Last().Id;

            protectionDocSubTypes.Add(new DicProtectionDocSubType
            {
                Id = ++lastId,
                TypeId = SPTypePatentId.IndustrialSample,
                Code = "03_Industrial_Sample",
                NameRu = "Ближнее зарубежъе"
            });
            protectionDocSubTypes.Add(new DicProtectionDocSubType
            {
                Id = ++lastId,
                TypeId = SPTypePatentId.IndustrialSample,
                Code = "04_Industrial_Sample",
                NameRu = "Иностранная"
            });
            protectionDocSubTypes.Add(new DicProtectionDocSubType
            {
                Id = ++lastId,
                TypeId = SPTypePatentId.IndustrialSample,
                Code = "05_Industrial_Sample",
                NameRu = "Национальная"
            });

            protectionDocSubTypes.Add(new DicProtectionDocSubType
            {
                Id = ++lastId,
                TypeId = SPTypePatentId.TradeMark,
                Code = "03_Trade_Mark",
                NameRu = "Ближнее зарубежъе"
            });
            protectionDocSubTypes.Add(new DicProtectionDocSubType
            {
                Id = ++lastId,
                TypeId = SPTypePatentId.TradeMark,
                Code = "04_Trade_Mark",
                NameRu = "Иностранная"
            });

            protectionDocSubTypes.Add(new DicProtectionDocSubType
            {
                Id = ++lastId,
                TypeId = SPTypePatentId.NameOfOrigin,
                Code = "03_Name_of_Origin",
                NameRu = "Ближнее зарубежъе",
            });
            protectionDocSubTypes.Add(new DicProtectionDocSubType
            {
                Id = ++lastId,
                TypeId = SPTypePatentId.NameOfOrigin,
                Code = "04_Name_of_Origin",
                NameRu = "Иностранная",
            });
            protectionDocSubTypes.Add(new DicProtectionDocSubType
            {
                Id = ++lastId,
                TypeId = SPTypePatentId.NameOfOrigin,
                Code = "05_Name_of_Origin",
                NameRu = "Национальная",
            });

            protectionDocSubTypes.Add(new DicProtectionDocSubType
            {
                Id = ++lastId,
                TypeId = SPTypePatentId.SelectionAchieve,
                Code = "03_Selection_Achieve",
                NameRu = "Ближнее зарубежъе",
            });
            protectionDocSubTypes.Add(new DicProtectionDocSubType
            {
                Id = ++lastId,
                TypeId = SPTypePatentId.SelectionAchieve,
                Code = "04_Selection_Achieve",
                NameRu = "Иностранная",
            });
            protectionDocSubTypes.Add(new DicProtectionDocSubType
            {
                Id = ++lastId,
                TypeId = SPTypePatentId.SelectionAchieve,
                Code = "05_Selection_Achieve",
                NameRu = "Национальная",
            });

            return protectionDocSubTypes;
        }

        public List<DicApplicantType> GetDicApplicantTypes()
        {
            var dicApplicantTypes = new List<DicApplicantType>
            {
                new DicApplicantType
                {
                    Code ="1",
                    NameRu ="Физическое лицо"
                },
                  new DicApplicantType
                {
                    Code ="2",
                    NameRu ="Юридическое лицо"
                },
                    new DicApplicantType
                {
                    Code ="3",
                    NameRu ="Субъекты малого и среднего бизнеса-резиденты"
                },
                      new DicApplicantType
                {
                    Code ="4",
                    NameRu ="Участники Великой отечественной войны, инвалиды, учащиеся общеобразовательных школ и колледжей, студенты высших учебных заведений."
                },
            };

            return dicApplicantTypes;
        }

        public List<DicConsiderationType> GetDicConsiderationTypes()
        {
            var considerationTypes = _context.ConsidTypes
                .AsNoTracking()
                .ToList();

            return considerationTypes.Select(c => new DicConsiderationType
            {
                Id = c.Id,
                ProtectionDocTypeId = c.TypeId,
                Code = c.Code,
                NameRu = c.NameRu,
                NameEn = c.NameEn,
                NameKz = c.NameKz,
                Description = c.Description
            }).ToList();
        }

        public List<DicLocationType> GetDicLocationTypes()
        {
            var locationTypes = _context.MainRef
                .AsNoTracking()
                .Where(d => d.TypeId == RefMainTypeId.LocationType)
                .ToList();

            return locationTypes.Select(l => new DicLocationType
            {
                Id = l.Id,
                Code = l.Code,
                NameRu = l.NameRu,
                NameEn = l.NameEn,
                NameKz = l.NameKz,
                Description = l.Description
            }).ToList();
        }

        public List<DicLocation> GetDicLocations()
        {
            var locations = _context.Locations
                .AsNoTracking()
                .Where(l => new int[] { SPMainId.City, SPMainId.Region, SPMainId.Area }.Contains(l.TypeId))
                .ToList();

            var locationIds = locations.Select(l => l.Id).ToList();

            return locations.Select(l => new DicLocation
            {
                Id = l.Id,
                TypeId = l.TypeId,
                Code = l.Code,
                ParentId = locationIds.Contains(l.ParentId ?? 0) ? l.ParentId : null,
                Order = l.OrderId,
                StatId = l.StatId,
                StatParentId = l.StatParentId,
                NameRu = l.NameRu,
                NameEn = l.NameEn,
                NameKz = l.NameKz
            }).ToList();
        }

        public List<ProtectionDocAttorney> GetProtectionDocAttornies()
        {
            var protectionDocAttornies = _context.PatentAttorneyses
                .AsNoTracking()
                .ToList();

            return protectionDocAttornies.Select(p => new ProtectionDocAttorney
            {
                Id = p.Id,
                DateCreate = DateTimeOffset.Now,
                DateUpdate = DateTimeOffset.Now,
                LocationId = p.LocationId,
                CountryId = p.CountryId,
                NameFirst = p.Firstname,
                NameLast = p.Lastname,
                NameMiddle = p.Middlename,
                Address = p.Address,
                CertificateDate = p.CertDate,
                CertificateNumber = p.CertNum,
                Description = p.Note,
                Email = p.EMail,
                Fax = p.Fax,
                IsActive = CustomConverter.IntToBool(p.Active),
                Job = p.Job,
                KnowledgeArea = p.KnowledgeArea,
                Language = p.Language,
                OPS = p.Ops,
                Phone = p.Phone,
                RevalidNote = p.RevalidNote,
                WebSite = p.WebSite,
                XIN = p.IIN
            }).ToList();
        }

        public List<DicRedefinitionType> GetDicRedefinitionTypes()
        {
            var dicRedefinitionTypes = _context.MainRef
                .AsNoTracking()
                .Where(d => d.TypeId == RefMainTypeId.RedefinitionType)
                .ToList();

            return dicRedefinitionTypes.Select(d => new DicRedefinitionType
            {
                Id = d.Id,
                Code = d.Code,
                NameRu = d.NameRu,
                NameEn = d.NameEn,
                NameKz = d.NameKz,
                Description = d.Description,
            }).ToList();
        }

        public List<DicTariff> GetDicTariffs()
        {
            var tariffs = _context.Tariffs
                .AsNoTracking()
                .ToList();

            var result = tariffs.Select(d => new DicTariff
            {
                Id = d.Id,
                Code = d.Code,
                NameRu = d.NameRu,
                NameEn = d.NameEn,
                NameKz = d.NameKz,
                Description = d.Description,
                TariffProtectionDocTypes = d.TypeId > 0 ?
                    new[] {
                        new DicTariffProtectionDocType {
                            TariffId = d.Id,
                            ProtectionDocTypeId = d.TypeId.GetValueOrDefault(0)
                        }
                    }
                    : new List<DicTariffProtectionDocType>().ToArray(),
                Price = (decimal)d.Price,
                Limit = d.TLimit,
            }).ToList();

            return result;
        }

        public List<DicSendType> GetDicSendTypes()
        {
            var sendTypes = _context.MainRef
                .AsNoTracking()
                .Where(t => t.TypeId == RefMainTypeId.SendType)
                .OrderBy(t => t.Id)
                .ToList();

            return sendTypes.Select(t => new DicSendType
            {
                Id = t.Id,
                Code = t.Code,
                Description = t.Description,
                NameEn = t.NameEn,
                NameKz = t.NameKz,
                NameRu = t.NameRu
            }).ToList();
        }

        public List<RouteStageOrder> GetRouteStageOrders()
        {
            var stageOrders = _context.RfStageNexts.AsNoTracking().OrderBy(s => s.Id).ToList();

            return stageOrders.Select(s => new RouteStageOrder
            {
                Id = s.Id,
                CurrentStageId = s.StageId,
                NextStageId = s.NextStageId,
                IsAutomatic = CustomConverter.StringToBool(s.IsAutomatic),
                IsParallel = CustomConverter.StringToBool(s.IsParallel),
                IsReturn = CustomConverter.StringToBool(s.IsReturn),
            }).ToList();
        }

        public List<AvailabilityCorrespondence> GetAvailabilityCorrespondences()
        {
            var documentTypeIds = _dictionaryTypesHelper.GetDocumentTypeIds();

            var availabilityCorrespondences = _context.AvailabilityCorrespondences
                .Where(a => documentTypeIds.Contains(a.CorId))
                .AsNoTracking()
                .ToList();

            return availabilityCorrespondences.Select(a => new AvailabilityCorrespondence
            {
                Id = a.Id,
                DateCreate = DateTimeOffset.Now,
                DateUpdate = DateTimeOffset.Now,
                DocumentTypeId = a.CorId,
                ProtectionDocTypeId = a.PatentType,
                RouteStageId = a.StageId,
                Status = a.Status
            }).ToList();
        }

        public List<DicAddress> GetDicAddresses()
        {
            var addresses = _context.WtAddresses.AsNoTracking().Where(a => a.Id != 0).OrderBy(a => a.Id).ToList();

            var countriesIds = _context.Locations
                .AsNoTracking()
                .Where(l => l.TypeId == RefMainTypeId.Country)
                .OrderBy(l => l.Id)
                .Select(l => l.Id)
                .ToList();

            var continentsIds = _context.Locations
                .AsNoTracking()
                .Where(l => l.TypeId == RefMainTypeId.Continent)
                .OrderBy(l => l.Id)
                .Select(l => l.Id)
                .ToList();

            return addresses.Select(a => new DicAddress
            {
                Id = a.Id,
                NameEn = a.AddressMlEn,
                NameKz = a.AddressMlKz,
                NameRu = a.AddressMllRu,
                PostCode = a.PostBox,
                CountryId = countriesIds.Contains(a.LocationId ?? -1) ? a.LocationId : null,
                ContinentId = continentsIds.Contains(a.LocationId ?? -1) ? a.LocationId : null,
            }).ToList();
        }
    }
}
