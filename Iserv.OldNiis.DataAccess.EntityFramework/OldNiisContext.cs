using Iserv.OldNiis.Domain.Entities;
using Iserv.OldNiis.Domain.Entities.Others;
using Iserv.OldNiis.Domain.Entities.References;
using Microsoft.EntityFrameworkCore;
using NIIS.DBConverter.Entities.AccountingData;
using NIIS.DBConverter.Entities.DBObjects;
using NIIS.DBConverter.Entities.Documents;
using NIIS.DBConverter.Entities.Others;
using NIIS.DBConverter.Entities.Patent;
using NIIS.DBConverter.Entities.Payments;
using NIIS.DBConverter.Entities.References;

namespace Iserv.OldNiis.DataAccess.EntityFramework
{
    public class OldNiisContext : DbContext
    {
        public OldNiisContext(DbContextOptions options) : base(options) {
            Database.SetCommandTimeout(600);
        }

        public virtual DbSet<SPMain> MainRef { get; set; }
        public virtual DbSet<CLDocument> Documents { get; set; }
        public virtual DbSet<CLTemplate> Templates { get; set; }
        public virtual DbSet<DDWorktype> Worktypes { get; set; }
        public virtual DbSet<CLStage> Stages { get; set; }
        public virtual DbSet<CLDepartment> Departments { get; set; }
        public virtual DbSet<SPTypePatent> TypePatents { get; set; }
        public virtual DbSet<SPIIType> IITypes { get; set; }
        public virtual DbSet<SPTEarlyType> EarlyTypes { get; set; }
        public virtual DbSet<SPTCustomerPatent> CustomerPatents { get; set; }
        public virtual DbSet<SPTTypeConsid> ConsidTypes { get; set; }
        public virtual DbSet<SPTVidTariff> Tariffs { get; set; }
        public virtual DbSet<SPTRequestType> RequestTypes { get; set; }
        public virtual DbSet<CLLocation> Locations { get; set; }
        public virtual DbSet<IntegrationConServiceStatus> IntegrationConServiceStatuses { get; set; }

        public virtual DbSet<CLIPC> IPCs { get; set; }
        public virtual DbSet<CLIPC2> IPC2s { get; set; }
        public virtual DbSet<CLTMICFEM> tmICFEMs { get; set; }
        public virtual DbSet<CLICIS> ICISes { get; set; }
        public virtual DbSet<CLTMICGS> ICGSes { get; set; }
        public virtual DbSet<SSGroup> Groups { get; set; }
        public virtual DbSet<SSUser> Users { get; set; }
        public virtual DbSet<STTableClassification> TableClassifications { get; set; }
        public virtual DbSet<tbPatentAttorneys> PatentAttorneyses { get; set; }
        public virtual DbSet<tbAvailabilityCorrespondence> AvailabilityCorrespondences { get; set; }

        public virtual DbSet<RfIPC> RfIpcs { get; set; }
        public virtual DbSet<RfICIS> RfIciss { get; set; }
        public virtual DbSet<RfTmIcgs> RfTmIcgses { get; set; }
        public virtual DbSet<RfTmIcfem> RfTmIcfems { get; set; }

        public virtual DbSet<SPTPatSubt> SptPatSubts { get; set; }
        public virtual DbSet<TbReferences> TbReferences { get; set; }
        public virtual DbSet<TbGeneratedQueryExpDep> TbGeneratedQueryExpDeps { get; set; }

        #region Documents

        public virtual DbSet<DDOutProp> DDOutProps { get; set; }
        public virtual DbSet<RFMessageDocument> RFMessageDocuments { get; set; }
        public virtual DbSet<DDDocument> DDDocuments { get; set; }
        public virtual DbSet<WTPTWorkoffice> WTPTWorkoffices { get; set; }
        public virtual DbSet<RFDocumentExecutor> RFDocumentExecutors { get; set; }
        public virtual DbSet<DdInfo> DdInfos { get; set; }
        public virtual DbSet<TbDocumentUsersSignature> DocumentUsersSignaturies { get; set; }

        #endregion

        #region Customers

        public virtual DbSet<WTCustomer> WtCustomers { get; set; }
        public virtual DbSet<WTAddress> WtAddresses { get; set; }
        public virtual DbSet<RfCustomer> RfCustomers { get; set; }
        public virtual DbSet<DDCustomer> DdCustomers { get; set; }

        #endregion

        #region DBObjects

        public virtual DbSet<StColumn> StColumns { get; set; }
        public virtual DbSet<STColumnType> StColumnTypes { get; set; }
        public virtual DbSet<STColumnKind> StColumnKinds { get; set; }
        public virtual DbSet<SPTemplateTable> SpTemplateTables { get; set; }
        public virtual DbSet<RFTemplateAttrib> RfTemplateAttribs { get; set; }

        #endregion

        #region Others

        public virtual DbSet<RFStagePosition> RfStagePositions { get; set; }
        public virtual DbSet<RFStageNext> RfStageNexts { get; set; }
        public virtual DbSet<VCLDocumentHistory> VClDocumentHistories { get; set; }
        public virtual DbSet<RfStageDocument> RfStageDocuments { get; set; }
        public virtual DbSet<RfTmIColorTm> RfTmIColorTms { get; set; }
        public virtual DbSet<ClTmIColor> ClTmIColors { get; set; }
        public virtual DbSet<tbGeneratedInvoicesNumbers> TbGeneratedInvoicesNumbers { get; set; }
        #endregion

        #region Patents

        public virtual DbSet<BtBasePatent> BtBasePatents { get; set; }
        public virtual DbSet<WtPtEarlyReg> WtPtEarlyRegs { get; set; }
        public virtual DbSet<DdEarlyReg> DdEarlyRegs { get; set; }
        public virtual DbSet<WtPtRedefine> WtPtRedefines { get; set; }
        public virtual DbSet<RfPatPatDk> RfPatPatDks { get; set; }

        #endregion

        #region Payments

        public virtual DbSet<WtPlPayment> WtPlPayments { get; set; }
        public virtual DbSet<WtPlPaymentUse> WtPlPaymentUses { get; set; }
        public virtual DbSet<WtPlFixpayment> WtPlFixpayments { get; set; }
        public virtual DbSet<DdPaymentData> DdPaymentData { get; set; }

        #endregion

        public virtual DbSet<TbCounter> TbCounters { get; set; }
    }
}
