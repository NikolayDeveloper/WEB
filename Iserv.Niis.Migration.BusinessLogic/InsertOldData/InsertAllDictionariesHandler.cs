using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Migration.BusinessLogic.Abstract;
using Iserv.Niis.Migration.BusinessLogic.Services.NewNiis;
using Iserv.Niis.Migration.BusinessLogic.Services.OldNiis;
using Iserv.OldNiis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Entities.Dictionaries.Location;
using Iserv.Niis.Business.Helpers;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Iserv.Niis.Migration.BusinessLogic.InsertOldData
{
    public class InsertAllDictionariesHandler : BaseHandler
    {
        private readonly static string PathApi = System.Environment.CurrentDirectory;

        private readonly OldNiisDictionaryService _oldNiisDictionaryService;
        private readonly NewNiisDictionaryService _newNiisDictionaryService;

        public InsertAllDictionariesHandler(
            NiisWebContextMigration context,
            OldNiisDictionaryService oldNiisDictionaryService,
            NewNiisDictionaryService newNiisDictionaryService) : base(context)
        {
            _oldNiisDictionaryService = oldNiisDictionaryService;
            _newNiisDictionaryService = newNiisDictionaryService;
        }


        public void Execute()
        {
            MigrateDicDivisions();
            Console.WriteLine("MigrateDicDivisions migrated");
            MigrateDicRoutes();
            Console.WriteLine("MigrateDicRoutes migrated");
            MigrateDicDepartmentTypes();
            Console.WriteLine("MigrateDicDepartmentTypes migrated");
            MigrateDicDepartmens();
            Console.WriteLine("MigrateDicDepartmens migrated");
            MigrateDicPositions();
            Console.WriteLine("MigrateDicPositions migrated");
            MigrateDicContinents();
            Console.WriteLine("MigrateDicContinents migrated");
            MigrateDicCountries();
            Console.WriteLine("MigrateDicCountries migrated");
            MigrateDicProtectionDocTypes();
            Console.WriteLine("MigrateDicProtectionDocTypes migrated");
            MigrateDicReceiveTypes();
            Console.WriteLine("MigrateDicReceiveTypes migrated");
            MigrateDicCustomerTypes();
            Console.WriteLine("MigrateDicCustomerTypes migrated");
            MigrateDicOnlineRequisitionStatuses();
            Console.WriteLine("MigrateDicOnlineRequisitionStatuses migrated");
            MigrateIntegrationConServiceStatuses();
            Console.WriteLine("MigrateIntegrationConServiceStatuses migrated");
            MigrateDicRouteStages();
            Console.WriteLine("MigrateDicRouteStages migrated");
            MigrateDicRequestStatuses();
            Console.WriteLine("MigrateDicRequestStatuses migrated");
            MigrateDicProtectionDocStatuses();
            Console.WriteLine("MigrateDicProtectionDocStatuses migrated");
            MigrateDicContractStatuses();
            Console.WriteLine("MigrateDicContractStatuses migrated");
            MigrateDicTypeTrademarks();
            Console.WriteLine("MigrateDicTypeTrademarks migrated");
            MigrateDicSelectionAchieveTypes();
            Console.WriteLine("MigrateDicSelectionAchieveTypes migrated");
            MigrateDicConventionTypes();
            Console.WriteLine("MigrateDicConventionTypes migrated");
            MigrateDicBeneficiaryTypes();
            Console.WriteLine("MigrateDicBeneficiaryTypes migrated");
            MigrateDicDocumentClassifications();
            Console.WriteLine("MigrateDicDocumentClassifications migrated");
            MigrateDocumentTemplateFiles();
            Console.WriteLine("MigrateDocumentTemplateFiles migrated");
            MigrateDicDocumentTypes();
            Console.WriteLine("MigrateDicDocumentTypes migrated");
            MigrateDicPaymentStatuses();
            Console.WriteLine("MigrateDicPaymentStatuses migrated");
            MigrateDicColorTZs();
            Console.WriteLine("MigrateDicColorTZs migrated");
            MigrateDicCustomerRoles();
            Console.WriteLine("MigrateDicCustomerRoles migrated");
            MigrateDicICFEMs();
            Console.WriteLine("MigrateDicICFEMs migrated");
            MigrateDicEarlyRegTypes();
            Console.WriteLine("MigrateDicEarlyRegTypes migrated");
            MigrateDicICGSs();
            Console.WriteLine("MigrateDicICGSs migrated");
            MigrateDicICISs();
            Console.WriteLine("MigrateDicICISs migrated");
            MigrateDicIPCs();
            Console.WriteLine("MigrateDicIPCs migrated");
            MigrateDicProtectionDocSubTypes();
            Console.WriteLine("MigrateDicProtectionDocSubTypes migrated");
            MigrateDicApplicantTypes();
            Console.WriteLine("MigrateDicApplicantTypes migrated");
            MigrateDicConsiderationTypes();
            Console.WriteLine("MigrateDicConsiderationTypes migrated");
            MigrateDicLocationTypes();
            Console.WriteLine("MigrateDicLocationTypes migrated");
            MigrateDicLocations();
            Console.WriteLine("MigrateDicLocations migrated");
            MigrateProtectionDocAttornies();
            Console.WriteLine("MigrateProtectionDocAttornies migrated");
            MigrateDicRedefinitionTypes();
            Console.WriteLine("MigrateDicRedefinitionTypes migrated");
            MigrateDicTariffs();
            Console.WriteLine("MigrateDicTariffs migrated");
            MigrateDicDetailICGSs();
            Console.WriteLine("MigrateDicDetailICGSs migrated");
            MigrateDicSendTypes();
            Console.WriteLine("MigrateDicSendTypes migrated");
            MigrateRouteStadeOrders();
            Console.WriteLine("MigrateRouteStadeOrders migrated");
            MigrateAvailabilityCorrespondences();
            Console.WriteLine("AvailabilityCorrespondences migrated");
            MigrateDicAddresses();
            Console.WriteLine("DicAddresses migrated");
        }

        #region Private Methods

        private void MigrateDicDivisions()
        {
            var isAnyDicDivisions = _newNiisDictionaryService.IsAnyDictionaryByEntityName(nameof(DicDivision));

            if (isAnyDicDivisions == false)
            {
                var dicDivisions = _oldNiisDictionaryService.GetDicDivisions();
                ActionTransaction(() => _newNiisDictionaryService.CreateRangeDictionaries(dicDivisions));
            }
        }

        private void MigrateDicRoutes()
        {
            var isAnyDicRoutes = _newNiisDictionaryService.IsAnyDictionaryByEntityName(nameof(DicRoute));

            if (isAnyDicRoutes == false)
            {
                var dicRoutes = _oldNiisDictionaryService.GetDicRoutes();
                ActionTransaction(() => _newNiisDictionaryService.CreateRangeDictionaries(dicRoutes));
            }
        }

        private void MigrateDicDepartmentTypes()
        {
            var isAnyDicDepartmentTypes = _newNiisDictionaryService.IsAnyDictionaryByEntityName(nameof(DicDepartmentType));

            if (isAnyDicDepartmentTypes == false)
            {
                var departmentTypes = _oldNiisDictionaryService.GetDicDepartmentTypes();
                ActionTransaction(() => _newNiisDictionaryService.CreateRangeDictionaries(departmentTypes));
            }
        }

        private void MigrateDicDepartmens()
        {
            var isAnyDicDepartments = _newNiisDictionaryService.IsAnyDictionaryByEntityName(nameof(DicDepartment));

            if (isAnyDicDepartments == false)
            {
                var departments = _oldNiisDictionaryService.GetDicDepartments();
                ActionTransaction(() => _newNiisDictionaryService.CreateRangeDictionaries(departments));
            }
        }

        private void MigrateDicOnlineRequisitionStatuses()
        {
            var isAnyDicStatuses = _newNiisDictionaryService.IsAnyDictionaryByEntityName(nameof(DicOnlineRequisitionStatus));

            if (isAnyDicStatuses == false)
            {
                var statuses = _oldNiisDictionaryService.GetDicOnlineRequisitionStatuses();
                ActionTransaction(() => _newNiisDictionaryService.CreateRangeDictionaries(statuses));
            }
        }

        private void MigrateDicPositions()
        {
            var isAnyDicPositions = _newNiisDictionaryService.IsAnyDictionaryByEntityName(nameof(DicPosition));

            if (isAnyDicPositions == false)
            {
                var positions = _oldNiisDictionaryService.GetDicPositions();
                ActionTransaction(() => _newNiisDictionaryService.CreateRangeDictionaries(positions));
            }
        }

        private void MigrateDicContinents()
        {
            var isAnyDicContinents = _newNiisDictionaryService.IsAnyDictionaryByEntityName(nameof(DicContinent));

            if (isAnyDicContinents == false)
            {
                var continents = _oldNiisDictionaryService.GetDicContinents();
                ActionTransaction(() => _newNiisDictionaryService.CreateRangeDictionaries(continents));
            }
        }

        private void MigrateDicCountries()
        {
            var isAnyDicCountries = _newNiisDictionaryService.IsAnyDictionaryByEntityName(nameof(DicCountry));

            if (isAnyDicCountries == false)
            {
                var countries = _oldNiisDictionaryService.GetDicCountries();
                ActionTransaction(() => _newNiisDictionaryService.CreateRangeDictionaries(countries));
            }
        }

        private void MigrateDicProtectionDocTypes()
        {
            var isAnyDicProtectionDocTypes = _newNiisDictionaryService.IsAnyDictionaryByEntityName(nameof(DicProtectionDocType));
            if (isAnyDicProtectionDocTypes == false)
            {
                var protectionDocTypes = _oldNiisDictionaryService.GetDicProtectionDocTypes();
                ActionTransaction(() => _newNiisDictionaryService.CreateRangeDictionaries(protectionDocTypes));
            }
        }

        private void MigrateDicReceiveTypes()
        {
            var isAnyDicReceiveTypes = _newNiisDictionaryService.IsAnyDictionaryByEntityName(nameof(DicReceiveType));
            if (isAnyDicReceiveTypes == false)
            {
                var dicReceiveTypes = _oldNiisDictionaryService.GetDicReceiveTypes();
                ActionTransaction(() => _newNiisDictionaryService.CreateRangeDictionaries(dicReceiveTypes));
            }
        }

        private void MigrateDicCustomerTypes()
        {
            var isAnyDicCustomerTypes = _newNiisDictionaryService.IsAnyDictionaryByEntityName(nameof(DicCustomerType));

            if (isAnyDicCustomerTypes == false)
            {
                var types = _oldNiisDictionaryService.GetDicCustomerTypes();
                ActionTransaction(() => _newNiisDictionaryService.CreateRangeDictionaries(types));
            }
        }

        private void MigrateIntegrationConServiceStatuses()
        {
            var statuses = _oldNiisDictionaryService.GetIntegrationConServiceStatuses();
            ActionTransaction(() => _newNiisDictionaryService.AddRangeIntegrationConServiceStatus(statuses));
        }

        private void MigrateDicRouteStages()
        {
            var isAnyDicRouteStages = _newNiisDictionaryService.IsAnyDictionaryByEntityName(nameof(DicRouteStage));

            if (isAnyDicRouteStages == false)
            {
                var stages = _oldNiisDictionaryService.GetDicRouteStages();
                ActionTransaction(() => _newNiisDictionaryService.CreateRangeDictionaries(stages));
            }
            else
            {
                var stages = _oldNiisDictionaryService.GetDicRouteStages();
                ActionTransaction(() => _newNiisDictionaryService.UpdateRangeDicRouteStages(stages));
            }
        }

        private void MigrateDicRequestStatuses()
        {
            var isAnyDicRequestStatuses = _newNiisDictionaryService.IsAnyDictionaryByEntityName(nameof(DicRequestStatus));

            if (isAnyDicRequestStatuses == false)
            {
                var statuses = _oldNiisDictionaryService.GetDicRequestStatuses();
                ActionTransaction(() => _newNiisDictionaryService.CreateRangeDictionaries(statuses));
            }
        }

        private void MigrateDicProtectionDocStatuses()
        {
            var isAnyDicProtectionDocStatuses = _newNiisDictionaryService.IsAnyDictionaryByEntityName(nameof(DicProtectionDocStatus));

            if (isAnyDicProtectionDocStatuses == false)
            {
                var statuses = _oldNiisDictionaryService.GetDicProtectionDocStatuses();
                ActionTransaction(() => _newNiisDictionaryService.CreateRangeDictionaries(statuses));
            }
        }

        private void MigrateDicContractStatuses()
        {
            var isAnyDicDicContractStatuses = _newNiisDictionaryService.IsAnyDictionaryByEntityName(nameof(DicContractStatus));

            if (isAnyDicDicContractStatuses == false)
            {
                var statuses = _oldNiisDictionaryService.GetDicContractStatuses();
                ActionTransaction(() => _newNiisDictionaryService.CreateRangeDictionaries(statuses));
            }
        }

        private void MigrateDicTypeTrademarks()
        {
            var isAnyDicTypeTrademarks = _newNiisDictionaryService.IsAnyDictionaryByEntityName(nameof(DicTypeTrademark));

            if (isAnyDicTypeTrademarks == false)
            {
                var types = _oldNiisDictionaryService.GetDicTypeTrademarks();
                ActionTransaction(() => _newNiisDictionaryService.CreateRangeDictionaries(types));
            }
        }

        private void MigrateDicSelectionAchieveTypes()
        {
            var isAnyDicSelectionAchieveTypes = _newNiisDictionaryService.IsAnyDictionaryByEntityName(nameof(DicSelectionAchieveType));

            if (isAnyDicSelectionAchieveTypes == false)
            {
                var types = _oldNiisDictionaryService.GetDicSelectionAchieveTypes();
                ActionTransaction(() => _newNiisDictionaryService.CreateRangeDictionaries(types));
            }
        }

        private void MigrateDicConventionTypes()
        {
            var isAnyDicConventionTypes = _newNiisDictionaryService.IsAnyDictionaryByEntityName(nameof(DicConventionType));

            if (isAnyDicConventionTypes == false)
            {
                var types = _oldNiisDictionaryService.GetDicConventionTypes();
                ActionTransaction(() => _newNiisDictionaryService.CreateRangeDictionaries(types));
            }
        }

        private void MigrateDicBeneficiaryTypes()
        {
            var isAnyDicBeneficiaryTypes = _newNiisDictionaryService.IsAnyDictionaryByEntityName(nameof(DicBeneficiaryType));

            if (isAnyDicBeneficiaryTypes == false)
            {
                var types = _oldNiisDictionaryService.GetDicBeneficiaryTypes();
                ActionTransaction(() => _newNiisDictionaryService.CreateRangeDictionaries(types));
            }
        }

        private void MigrateDicPaymentStatuses()
        {
            var isAnyDicPaymentStatuses = _newNiisDictionaryService.IsAnyDictionaryByEntityName(nameof(DicPaymentStatus));

            if (isAnyDicPaymentStatuses == false)
            {
                var statuses = _oldNiisDictionaryService.GetDicPaymentStatuses();
                ActionTransaction(() => _newNiisDictionaryService.CreateRangeDictionaries(statuses));
            }
        }

        private void MigrateDicColorTZs()
        {
            var isAnyDicColorTZs = _newNiisDictionaryService.IsAnyDictionaryByEntityName(nameof(DicColorTZ));

            if (isAnyDicColorTZs == false)
            {
                var colors = _oldNiisDictionaryService.GetDicColorTZs();
                ActionTransaction(() => _newNiisDictionaryService.CreateRangeDictionaries(colors));
            }
        }

        private void MigrateDicCustomerRoles()
        {
            var isAnyDicCustomerRoles = _newNiisDictionaryService.IsAnyDictionaryByEntityName(nameof(DicCustomerRole));

            if (isAnyDicCustomerRoles == false)
            {
                var roles = _oldNiisDictionaryService.GetDicCustomerRoles();
                ActionTransaction(() => _newNiisDictionaryService.CreateRangeDictionaries(roles));
            }
        }

        private void MigrateDicICFEMs()
        {
            var isAnyDicICFEMs = _newNiisDictionaryService.IsAnyDictionaryByEntityName(nameof(DicICFEM));

            if (isAnyDicICFEMs == false)
            {
                var icfems = _oldNiisDictionaryService.GetDicICFEMs();
                ActionTransaction(() => _newNiisDictionaryService.CreateRangeDictionaries(icfems));
            }
        }

        private void MigrateDicEarlyRegTypes()
        {
            var isAnyDicEarlyRegTypes = _newNiisDictionaryService.IsAnyDictionaryByEntityName(nameof(DicEarlyRegType));

            if (isAnyDicEarlyRegTypes == false)
            {
                var types = _oldNiisDictionaryService.GetDicEarlyRegTypes();
                ActionTransaction(() => _newNiisDictionaryService.CreateRangeDictionaries(types));
            }
        }

        private void MigrateDicICGSs()
        {
            var isAnyDicICGSs = _newNiisDictionaryService.IsAnyDictionaryByEntityName(nameof(DicICGS));

            if (isAnyDicICGSs == false)
            {
                var icgs = _oldNiisDictionaryService.GetDicICGSs();
                ActionTransaction(() => _newNiisDictionaryService.CreateRangeDictionaries(icgs));
            }
        }

        private void MigrateDicICISs()
        {
            var isAnyDicICISs = _newNiisDictionaryService.IsAnyDictionaryByEntityName(nameof(DicICIS));

            if (isAnyDicICISs == false)
            {
                var iciss = _oldNiisDictionaryService.GetDicICISs();
                ActionTransaction(() => _newNiisDictionaryService.CreateRangeDictionaries(iciss));
            }
        }

        private void MigrateDicDocumentClassifications()
        {
            var isAnyDicDocumentClassifications = _newNiisDictionaryService.IsAnyDictionaryByEntityName(nameof(DicDocumentClassification));
            if (isAnyDicDocumentClassifications == false)
            {
                var dicDocumentClassifications = _oldNiisDictionaryService.GetDicDocumentClassifications();
                ActionTransaction(() => _newNiisDictionaryService.CreateRangeDictionaries(dicDocumentClassifications));
            }
        }

        public void MigrateDocumentTemplateFiles()
        {
            var isAnyDocumentTemplateFiles = _newNiisDictionaryService.IsAnyDocumentTemplateFiles();
            if (isAnyDocumentTemplateFiles == false)
            {
                ActionTransaction(() => DocumentTemplateHelper.CreateTemplateEntities(NewNiisContext, PathApi));
            }
        }

        public void MigrateDicDocumentTypes()
        {
            var isAnyDicDocumentTypes = _newNiisDictionaryService.IsAnyDictionaryByEntityName(nameof(DicDocumentType));
            if (isAnyDicDocumentTypes == false)
            {
                var dicDocumentTypes = _oldNiisDictionaryService.GetDicDocumentTypes();
                ActionTransaction(() => _newNiisDictionaryService.CreateRangeDictionaries(dicDocumentTypes));
            }
        }

        public void MigrateDicIPCs()
        {
            var isAnyDicIPCs = _newNiisDictionaryService.IsAnyDictionaryByEntityName(nameof(DicIPC));
            if (isAnyDicIPCs == false)
            {
                var ipcs = _oldNiisDictionaryService.GetDicIPCs();
                ActionTransaction(() => _newNiisDictionaryService.CreateRangeDictionaries(ipcs));
            }
        }


        private void MigrateDicProtectionDocSubTypes()
        {
            var isAnyDicProtectionDocSubTypes = _newNiisDictionaryService.IsAnyDictionaryByEntityName(nameof(DicProtectionDocSubType));
            if (isAnyDicProtectionDocSubTypes == false)
            {
                var dicProtectionDocSubTypes = _oldNiisDictionaryService.GetDicProtectionDocSubTypes();
                ActionTransaction(() => _newNiisDictionaryService.CreateRangeDictionaries(dicProtectionDocSubTypes));
            }
        }

        private void MigrateDicApplicantTypes()
        {
            var isAnyDicApplicantTypes = _newNiisDictionaryService.IsAnyDictionaryByEntityName(nameof(DicApplicantType));
            if (isAnyDicApplicantTypes == false)
            {
                var dicApplicantTypes = _oldNiisDictionaryService.GetDicApplicantTypes();
                ActionTransaction(() => _newNiisDictionaryService.CreateRangeDictionaries(dicApplicantTypes));
            }
        }

        private void MigrateDicConsiderationTypes()
        {
            var isAnyDicConsiderationTypes = _newNiisDictionaryService.IsAnyDictionaryByEntityName(nameof(DicConsiderationType));
            if (isAnyDicConsiderationTypes == false)
            {
                var dicConsiderationTypes = _oldNiisDictionaryService.GetDicConsiderationTypes();
                ActionTransaction(() => _newNiisDictionaryService.CreateRangeDictionaries(dicConsiderationTypes));
            }
        }

        private void MigrateDicLocationTypes()
        {
            var isAnyDicLocationTypes = _newNiisDictionaryService.IsAnyDictionaryByEntityName(nameof(DicLocationType));
            if (isAnyDicLocationTypes == false)
            {
                var dicLocationTypes = _oldNiisDictionaryService.GetDicLocationTypes();
                ActionTransaction(() => _newNiisDictionaryService.CreateRangeDictionaries(dicLocationTypes));
            }
        }

        private void MigrateDicLocations()
        {
            var isAnyDicLocations = _newNiisDictionaryService.IsAnyDictionaryByEntityName(nameof(DicLocation));
            if (isAnyDicLocations == false)
            {
                var dicLocations = _oldNiisDictionaryService.GetDicLocations();
                ActionTransaction(() => _newNiisDictionaryService.CreateRangeDictionaries(dicLocations));
            }
        }

        private void MigrateProtectionDocAttornies()
        {
            var isAnyProtectionDocAttornies = _newNiisDictionaryService.IsAnyProtectionDocAttornies();
            if (isAnyProtectionDocAttornies == false)
            {
                var protectionDocAttornies = _oldNiisDictionaryService.GetProtectionDocAttornies();
                ActionTransaction(() => _newNiisDictionaryService.CreateRangeProtectionDocAttornies(protectionDocAttornies));
            }
        }

        private void MigrateDicRedefinitionTypes()
        {
            var isAnyDicRedefinitionTypes = _newNiisDictionaryService.IsAnyDictionaryByEntityName(nameof(DicRedefinitionType));
            if (isAnyDicRedefinitionTypes == false)
            {
                var dicRedefinitionTypes = _oldNiisDictionaryService.GetDicRedefinitionTypes();
                ActionTransaction(() => _newNiisDictionaryService.CreateRangeDictionaries(dicRedefinitionTypes));
            }
        }

        private void MigrateDicTariffs()
        {
            var isAnyDicTariffs = _newNiisDictionaryService.IsAnyDictionaryByEntityName(nameof(DicTariff));
            if (isAnyDicTariffs == false)
            {
                var dicTariffs = _oldNiisDictionaryService.GetDicTariffs();

                ActionTransaction(() => _newNiisDictionaryService.CreateRangeDictionaries(dicTariffs));
            }
        }

        private void MigrateDicDetailICGSs()
        {
            var isAnyDicDetailICGSs = _newNiisDictionaryService.IsAnyDictionaryByEntityName(nameof(DicDetailICGS));
            if (isAnyDicDetailICGSs == false)
            {
                var sqlInsertDicDetailICGSs = File.ReadAllText(Directory.GetCurrentDirectory() + "/InsertDicDetailICGSs.sql");
                ActionTransaction(() => {
                    NewNiisContext.Database.SetCommandTimeout(600);
                    NewNiisContext.Database.ExecuteSqlCommand(sqlInsertDicDetailICGSs);
                });
            }
        }

        private void MigrateDicSendTypes()
        {
            var isAnyDicSendTypes = _newNiisDictionaryService.IsAnyDictionaryByEntityName(nameof(DicSendType));
            if (isAnyDicSendTypes == false)
            {
                var dicSendTypes = _oldNiisDictionaryService.GetDicSendTypes();
                ActionTransaction(() => _newNiisDictionaryService.CreateRangeDictionaries(dicSendTypes));
            }
        }

        private void MigrateRouteStadeOrders()
        {
            var isAnyDicRouteStadeOrders = _newNiisDictionaryService.IsAnyRouteStateOrders();
            if (isAnyDicRouteStadeOrders == false)
            {
                var roteStageOrders = _oldNiisDictionaryService.GetRouteStageOrders();
                ActionTransaction(() => _newNiisDictionaryService.CreateRangeRouteStageOrders(roteStageOrders));
            }
        }
        private void MigrateAvailabilityCorrespondences()
        {
            var isAnyAvailabilityCorrespondences = _newNiisDictionaryService.IsAnyAvailabilityCorrespondences();
            if (isAnyAvailabilityCorrespondences == false)
            {
                var availabilityCorrespondences = _oldNiisDictionaryService.GetAvailabilityCorrespondences();
                ActionTransaction(() => _newNiisDictionaryService.CreateRangeAvailabilityCorrespondences(availabilityCorrespondences));
            }
        }

        private void MigrateDicAddresses()
        {
            var isAnyDicAddress = _newNiisDictionaryService.IsAnyDictionaryByEntityName(nameof(DicAddress));
            if (isAnyDicAddress == false)
            {
                var adresses = _oldNiisDictionaryService.GetDicAddresses();
                ActionTransaction(() => _newNiisDictionaryService.CreateRangeDictionaries(adresses));
            }
        }

        #endregion
    }
}
