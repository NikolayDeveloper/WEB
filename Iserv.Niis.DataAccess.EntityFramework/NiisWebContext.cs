using Iserv.Niis.DataAccess.EntityFramework.Infrastructure;
using Iserv.Niis.Domain.Entities;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.AutoNotificationDocumentGeneration;
using Iserv.Niis.Domain.Entities.AutoPaymentInvoiceGeneration;
using Iserv.Niis.Domain.Entities.AutoRouteStages;
using Iserv.Niis.Domain.Entities.BibliographicData;
using Iserv.Niis.Domain.Entities.Bulletin;
using Iserv.Niis.Domain.Entities.Calendar;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Entities.Dictionaries.Location;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.Integration;
using Iserv.Niis.Domain.Entities.IntellectualProperty;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Domain.Entities.Notification;
using Iserv.Niis.Domain.Entities.Other;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Entities.Rules;
using Iserv.Niis.Domain.Entities.Security;
using Iserv.Niis.Domain.Entities.Settings;
using Iserv.Niis.Domain.Entities.System;
using Iserv.Niis.Domain.Entities.Workflow;
using Iserv.Niis.Domain.EntitiesFile;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework
{
    public class NiisWebContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, IdentityUserClaim<int>,
        IdentityUserRole<int>, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public NiisWebContext(DbContextOptions<NiisWebContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder
                .MapModelsSpecificProperties()
                .MapForeignKeysOnCascadeDeleteBehavior()
                .MapRestrictionsForBasicPropertiesOfDictionary();
        }
               
        #region DBSets

        public DbSet<ClaimConstant> ClaimConstants { get; set; }

        public DbSet<AvailabilityCorrespondence> AvailabilityCorrespondences { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<CustomerAttorneyInfo> CustomerAttorneyInfos { get; set; }
        public DbSet<GeneratedInvoiceNumber> GeneratedInvoiceNumbers { get; set; }
        public DbSet<SystemCounter> SystemCounter { get; set; }
        public DbSet<SystemSettings> SystemSettings { get; set; }
        public DbSet<GeneratedQueryExpDep> GeneratedQueryExpDeps { get; set; }
        public DbSet<GridPrintSetting> GridPrintSettings { get; set; }
        public DbSet<IntellectualProperty> IntellectualProperties { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<RouteStageOrder> RouteStageOrders { get; set; }
        public DbSet<SettingGridOption> SettingGridOptions { get; set; }
        public DbSet<Signature> Signatures { get; set; }
        public DbSet<PaymentInvoiceGenerationRule> PaymentInvoiceGenerationRules { get; set; }
        public DbSet<PaymentInvoiceGenerationByPetitionRule> PaymentInvoiceByPetitionRules { get; set; }
        public DbSet<AutoGenerateNotificationDocumentByPetitionAndPaymentRule> NotificationDocumentByPetitionAndPaymentRules { get; set; }
        public DbSet<AutoGenerateNotificationDocumentByStageRule> NotificationDocumentByStageRules { get; set; }
        public DbSet<PaymentInvoiceChargingRule> PaymentInvoiceChargingRules { get; set; }


        #region Request

        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestInfo> RequestInfos { get; set; }
        public DbSet<ICGSRequest> ICGSRequests { get; set; }
        public DbSet<ICISRequest> ICISRequests { get; set; }
        public DbSet<IPCRequest> IPCRequests { get; set; }
        public DbSet<RequestCustomer> RequestCustomers { get; set; }
        public DbSet<RequestProtectionDocSimilar> RequestProtectionDocSimilarities { get; set; }
        public DbSet<RequestNotificationStatus> RequestNotificationStatuses { get; set; }
        public DbSet<ExpertSearchSimilar> ExpertSearchSimilarities { get; set; }
        public DbSet<RequestConventionInfo> RequestConventionInfos { get; set; }
        public DbSet<AdditionalDoc> AdditionalDocs { get; set; }

        #endregion

        #region ProtectionDoc

        public DbSet<ProtectionDoc> ProtectionDocs { get; set; }
        public DbSet<ProtectionDocEarlyReg> ProtectionDocEarlyRegs { get; set; }
        public DbSet<RequestEarlyReg> RequestEarlyRegs { get; set; }
        public DbSet<ProtectionDocProtectionDocRelation> ProtectionDocParents { get; set; }
        public DbSet<ProtectionDocRedefine> ProtectionDocRedefines { get; set; }
        public DbSet<ICGSProtectionDoc> ICGSProtectionDocs { get; set; }
        public DbSet<ICISProtectionDoc> ICISProtectionDocs { get; set; }
        public DbSet<IPCProtectionDoc> IpcProtectionDocs { get; set; }
        public DbSet<ProtectionDocCustomer> ProtectionDocCustomers { get; set; }
        public DbSet<ContractCustomer> ContractCustomers { get; set; }
        public DbSet<ProtectionDocConventionInfo> ProtectionDocConventionInfos { get; set; }
        public DbSet<ProtectionDocInfo> ProtectionDocInfos { get; set; }

        #endregion

        #region Payment

        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentInvoice> PaymentInvoices { get; set; }
        public DbSet<PaymentRegistryData> PaymentRegistryDatas { get; set; }
        public DbSet<PaymentUse> PaymentUses { get; set; }
        public DbSet<PaymentExecutor> PaymentExecutors { get; set; }
        public DbSet<RequiredPayment> RequiredPayments { get; set; }
        public DbSet<PaymentCharge> PaymentCharges { get; set; }

        #endregion

        #region Integration

        public DbSet<IntegrationConPackage> IntegrationConPackages { get; set; }
        public DbSet<IntegrationConPackageState> IntegrationConPackageStates { get; set; }
        public DbSet<IntegrationConPackageType> IntegrationConPackageTypes { get; set; }
        public DbSet<IntegrationConServiceStatus> IntegrationConServiceStatuses { get; set; }
        public DbSet<IntegrationEGovPay> IntegrationEGovPays { get; set; }
        public DbSet<IntegrationRequisition> IntegrationRequisitions { get; set; }
        public DbSet<IntegrationLogRefList> IntegrationLogRefLists { get; set; }
        public DbSet<IntegrationNiisRefTariff> IntegrationNiisRefTariffs { get; set; }
        public DbSet<IntegrationPaymentCalc> IntegrationPaymentCalcs { get; set; }
        public DbSet<Statement> Statements { get; set; }
        public DbSet<IntegrationStatus> IntegrationStatuses { get; set; }
        public DbSet<IntegrationDocument> IntegrationDocuments { get; set; }

        public DbSet<IntegrationRomarinLog> IntegrationRomarinLogs { get; set; }
        public DbSet<IntegrationRomarinFile> IntegrationRomarinFiles { get; set; }
        #endregion

        #region Document

        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentContent> DocumentContents { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<DocumentAccessPermissions> DocumentAccessRoles { get; set; }
        public DbSet<DocumentCustomer> DocumentCustomers { get; set; }
        public DbSet<DocumentEarlyReg> DocumentEarlyRegs { get; set; }
        public DbSet<DocumentExecutor> DocumentExecutors { get; set; }
        public DbSet<DocumentProperty> DocumentProperties { get; set; }
        public DbSet<DocumentUserSignature> DocumentUserSignatures { get; set; }
        public DbSet<DocumentWorkflow> DocumentWorkflows { get; set; }
        public DbSet<RequestWorkflow> RequestWorkflows { get; set; }
        public DbSet<WorkflowTaskQueue> WorkflowTaskQueues { get; set; }
        public DbSet<NotificationTaskQueue> NotificationTaskQueues { get; set; }
        public DbSet<ProtectionDocWorkflow> ProtectionDocWorkflows { get; set; }
        public DbSet<ProtectionDocParallelWorkflow> ProtectionDocParallelWorkflows { get; set; }
        public DbSet<ContractWorkflow> ContractWorkflows { get; set; }
        public DbSet<RequestDocument> RequestsDocuments { get; set; }
        public DbSet<DocumentTemplateFile> DocumentTemplateFiles { get; set; }
        public DbSet<ProtectionDocDocument> ProtectionDocDocuments { get; set; }
        public DbSet<ContractDocument> ContractsDocuments { get; set; }
        public DbSet<DocumentUserInput> DocumentUserInputs { get; set; }
        public DbSet<DocumentNotificationStatus> DocumentNotificationStatuses { get; set; }

        public DbSet<AutoRouteStage> AutoRouteStages { get; set; }
        public DbSet<AutoRouteStageEvent> AutoRouteStageEvents { get; set; }
        public DbSet<AutoRouteStageViewer> AutoRouteStageViewers { get; set; }
        public DbSet<DocumentWorkflowViewer> DocumentWorkflowViewers { get; set; }

        #endregion

        #region Dictionaries

        public DbSet<DicAddress> DicAddresses { get; set; }
        public DbSet<DicApplicantType> DicApplicantTypes { get; set; }
        public DbSet<DicColorTZ> DicColorTZs { get; set; }
        public DbSet<DicConsiderationType> DicConsiderationTypes { get; set; }
        public DbSet<DicContinent> DicContinents { get; set; }
        public DbSet<DicContractCategory> DicContractCategories { get; set; }
        public DbSet<DicContractStatus> DicContractStatuses { get; set; }
        public DbSet<DicConventionType> DicConventionTypes { get; set; }
        public DbSet<DicCountry> DicCountries { get; set; }
        public DbSet<DicCustomer> DicCustomers { get; set; }
        public DbSet<DicCustomerRole> DicCustomerRoles { get; set; }
        public DbSet<DicCustomerType> DicCustomerTypes { get; set; }
        public DbSet<DicDepartment> DicDepartments { get; set; }
        public DbSet<DicDepartmentType> DicDepartmentTypes { get; set; }
        public DbSet<DicDivision> DicDivisions { get; set; }
        public DbSet<DicPaymentStatus> DicPaymentStatuses { get; set; }
        public DbSet<DicDocumentStatus> DicDocumentStatuses { get; set; }
        public DbSet<DicPositionType> DicPositionTypes { get; set; }
        public DbSet<DicStageExpirationByDocType> DicStageExpirationByDocTypes { get; set; }
        public DbSet<DicDocumentClassification> DicDocumentClassifications { get; set; }
        public DbSet<DicDocumentType> DicDocumentTypes { get; set; }
        public DbSet<DicEarlyRegType> DicEarlyRegTypes { get; set; }
        public DbSet<DicEntityAccessType> DicEntityAccessTypes { get; set; }
        public DbSet<DicICFEM> DicICFEMs { get; set; }
        public DbSet<DicICGS> DicICGSs { get; set; }
        public DbSet<DicDetailICGS> DicDetailICGSs { get; set; }
        public DbSet<DicICIS> DicICISs { get; set; }
        public DbSet<DicIntellectualPropertyStatus> DicIntellectualPropertyStatuses { get; set; }
        public DbSet<DicIPC> DicIPCs { get; set; }
        public DbSet<DicLocation> DicLocations { get; set; }
        public DbSet<DicLocationType> DicLocationTypes { get; set; }
        public DbSet<DicLogType> DicLogTypes { get; set; }
        public DbSet<DicOnlineRequisitionStatus> DicOnlineRequisitionStatuses { get; set; }
        public DbSet<DicPosition> DicPositions { get; set; }
        public DbSet<DicProtectionDocBulletinType> DicProtectionDocBulletinTypes { get; set; }
        public DbSet<DicProtectionDocStatus> DicProtectionDocStatuses { get; set; }
        public DbSet<DicRequestStatus> DicRequestStatuses { get; set; }
        public DbSet<DicProtectionDocSubType> DicProtectionDocSubTypes { get; set; }
        public DbSet<DicProtectionDocTMType> DicProtectionDocTMTypes { get; set; }
        public DbSet<DicProtectionDocType> DicProtectionDocTypes { get; set; }
        public DbSet<DicReceiveType> DicReceiveTypes { get; set; }
        public DbSet<DicRedefinitionDocumentType> DicRedefinitionDocumentTypes { get; set; }
        public DbSet<DicRedefinitionType> DicRedefinitionTypes { get; set; }
        public DbSet<DicRequisitionFeedType> DicRequisitionFeedTypes { get; set; }
        public DbSet<DicRoute> DicRoutes { get; set; }
        public DbSet<DicRouteStage> DicRouteStages { get; set; }
        public DbSet<DicSendType> DicSendTypes { get; set; }
        public DbSet<DicTariff> DicTariffs { get; set; }
        public DbSet<DicEventType> DicEventTypes { get; set; }
        public DbSet<DicTypeTrademark> DicTypeTrademarks { get; set; }
        public DbSet<DicNotificationStatus> DicNotificationStatuses { get; set; }
        public DbSet<DicContactInfoType> DicContactInfoTypes { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
        public DbSet<DicBeneficiaryType> DicBeneficiaryTypes { get; set; }
        public DbSet<DicSelectionAchieveType> DicSelectionAchieveTypes { get; set; }
        public DbSet<DicDocumentTypeGroup> DicDocumentTypeGroups { get; set; }
        public DbSet<DicDocumentTypeGroupType> DicDocumentTypeGroupTypes { get; set; }
        public DbSet<DicTariffProtectionDocType> DicTariffProtectionDocTypes { get; set; }
        public DbSet<DicContractType> DicContractTypes { get; set; }
        public DbSet<DicBiblioChangeType> DicBiblioChangeTypes { get; set; }
        public DbSet<MadeChange> MadeChanges { get; set; }
        public DbSet<RequestAutoRouteStageExecutor> RequestAutoRouteStageExecutors { get; set; }
        public DbSet<LogRecord> LogRecords { get; set; }

        #endregion

        #region ManyToMany

        public DbSet<DocumentDocumentRelation> DocumentDocumentRelations { get; set; }
        public DbSet<DicColorTZProtectionDocRelation> DicColorTzProtectionDocRelations { get; set; }
        public DbSet<DicColorTZRequestRelation> DicColorTzRequestRelations { get; set; }
        public DbSet<DicIcfemProtectionDocRelation> DicIcfemProtectionDocRelations { get; set; }
        public DbSet<DicIcfemRequestRelation> DicIcfemRequestRelations { get; set; }
        public DbSet<ContractProtectionDocRelation> ContractProtectionDocRelations { get; set; }
        public DbSet<ContractRequestRelation> ContractRequestRelations { get; set; }
        public DbSet<ContractRequestICGSRequestRelation> ContractRequestICGSRequestRelations { get; set; }

        public DbSet<ContractProtectionDocICGSProtectionDocRelation> ContractProtectionDocICGSProtectionDocRelations
        {
            get;
            set;
        }

        public DbSet<RequestRequestRelation> RequestRequestRelations { get; set; }
        public DbSet<UserRouteStageRelation> UserRouteStageRelations { get; set; }
        public DbSet<UserIcgsRelation> UserIcgsRelations { get; set; }
        public DbSet<RoleRouteStageRelation> RoleRouteStageRelations { get; set; }
        public DbSet<RoleProtectionDocTypeRelation> RoleProtectionDocTypeRelations { get; set; }

        public DbSet<DicDocumentTypeDicProtectionDocTypeRelation> DicDocumentTypeDicProtectionDocTypeRelations
        {
            get;
            set;
        }

        public DbSet<ContractNotificationStatus> ContractNotificationStatuses { get; set; }
        public DbSet<DicBiblioChangeTypeDicRouteStageRelation> DicBiblioChangeTypeDicRouteStageRelations { get; set; }

        /// <summary>
        /// Связи между статусами заявок и маршрутами.
        /// </summary>
        public DbSet<DicRequestStatusRoute> DicRequestStatusesRoutes { get; set; }

        /// <summary>
        /// Связи между статусами охранных документов и маршрутами.
        /// </summary>
        public DbSet<DicProtectionDocStatusRoute> DicProtectionDocStatusesRoutes { get; set; }
        #endregion

        #region Calendar

        public DbSet<Event> Events { get; set; }

        #endregion

        #region Bulletin

        public DbSet<BulletinSection> BulletinSections { get; set; }

        #endregion

        public DbSet<SearchViewEntity> SearchViewEntities { get; set; }
        public DbSet<SearchRequestViewEntity> SearchRequestViewEntities { get; set; }
        public DbSet<SearchProtectionDocViewEntity> SearchProtectionDocViewEntities { get; set; }
        public DbSet<SearchContractViewEntity> SearchContractViewEntities { get; set; }
        public DbSet<ExpertSearchViewEntity> ExpertSearchViewEntities { get; set; }
        public DbSet<UsersTasksCountsEntity> UsersTasksCountsEntities { get; set; }

        public DbSet<DocumentLink> DocumentLinks { get; set; }
        public DbSet<DicRouteStagePerformer> DicRouteStagePerformers { get; set; }

        /// <summary>
        /// Настройки пользователя.
        /// </summary>
        public DbSet<UserSetting> UserSettings { get; set; }

        #endregion
    }
}