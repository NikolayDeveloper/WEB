using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Iserv.Niis.ExternalServices.DataAccess.EntityFramework.Migrations
{
    [DbContext(typeof(NiisIntegrationContext))]
    internal class NiisIntegrationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Iserv.Niis.ExternalServices.Domain.Entities.IntegrationBulletin", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<DateTimeOffset>("Date")
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("GETDATE()");

                b.Property<string>("Note");

                b.Property<DateTimeOffset>("PublishDate");

                b.Property<bool>("Sent")
                    .ValueGeneratedOnAdd()
                    .HasDefaultValue(false);

                b.HasKey("Id");

                b.ToTable("IntegrationBulletins");
            });

            modelBuilder.Entity("Iserv.Niis.ExternalServices.Domain.Entities.IntegrationCalcCustomer", b =>
            {
                b.Property<int>("ActionId");

                b.Property<bool>("SecondPass");

                b.Property<int>("CustomerId");

                b.HasKey("ActionId", "SecondPass", "CustomerId");

                b.ToTable("IntegrationCalcCustomers");
            });

            modelBuilder.Entity("Iserv.Niis.ExternalServices.Domain.Entities.IntegrationCalcHistory", b =>
            {
                b.Property<int>("ActionId");

                b.Property<int>("HistoryId");

                b.Property<int>("EntityType");

                b.HasKey("ActionId", "HistoryId");

                b.ToTable("IntegrationCalcHistories");
            });

            modelBuilder.Entity("Iserv.Niis.ExternalServices.Domain.Entities.IntegrationCalcLink", b =>
            {
                b.Property<int>("ActionId");

                b.Property<int>("PatentId");

                b.Property<int>("CustomerId");

                b.Property<int>("LinkId");

                b.Property<bool>("SecondPass");

                b.HasKey("ActionId", "PatentId", "CustomerId", "LinkId", "SecondPass");

                b.ToTable("IntegrationCalcLinks");
            });

            modelBuilder.Entity("Iserv.Niis.ExternalServices.Domain.Entities.IntegrationCalcPatent", b =>
            {
                b.Property<int>("ActionId");

                b.Property<int>("PatentId");

                b.HasKey("ActionId", "PatentId");

                b.ToTable("IntegrationCalcPatents");
            });

            modelBuilder.Entity("Iserv.Niis.ExternalServices.Domain.Entities.IntegrationHistory", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<int?>("CustomerId");

                b.Property<string>("CustomerName");

                b.Property<string>("CustomerRnn");

                b.Property<string>("CustomerXin");

                b.Property<DateTimeOffset>("HistoryDate");

                b.Property<int>("HistoryEntityType");

                b.Property<int>("HistoryGRActionType");

                b.Property<int?>("LinkId");

                b.Property<int?>("LinkType");

                b.Property<int?>("PatentDocType");

                b.Property<int?>("PatentEndReason");

                b.Property<DateTimeOffset?>("PatentGRRegDate");

                b.Property<int?>("PatentId");

                b.Property<string>("PatentName");

                b.Property<DateTimeOffset?>("PatentPublicDate");

                b.Property<string>("PatentPublicNumber");

                b.Property<DateTimeOffset?>("PatentSrokEndDate");

                b.Property<int?>("PatentType");

                b.HasKey("Id");

                b.ToTable("IntegrationHistories");
            });

            modelBuilder.Entity("Iserv.Niis.ExternalServices.Domain.Entities.IntegrationHistorySent", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<DateTimeOffset>("Date")
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("GETDATE()");

                b.HasKey("Id");

                b.ToTable("IntegrationHistorySents");
            });

            modelBuilder.Entity("Iserv.Niis.ExternalServices.Domain.Entities.IntegrationLogAction", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<DateTimeOffset?>("ActionDate")
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("GETDATE()");

                b.Property<int>("ActionTypeId");

                b.Property<int?>("BinListId");

                b.Property<DateTimeOffset?>("DateFrom");

                b.Property<DateTimeOffset?>("DateTo");

                b.Property<int?>("DigitalSignatureId");

                b.Property<int?>("HistoryId");

                b.Property<int?>("RnnListId");

                b.Property<int?>("SystemInfoAnswerId");

                b.Property<int?>("SystemInfoQueryId");

                b.HasKey("Id");

                b.ToTable("IntegrationLogActions");
            });

            modelBuilder.Entity("Iserv.Niis.ExternalServices.Domain.Entities.IntegrationLogDigitalSignature", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<string>("Bin");

                b.Property<bool>("CertificateIsCorrect");

                b.Property<string>("CheckNote");

                b.Property<string>("Company");

                b.Property<string>("EMail");

                b.Property<string>("Iin");

                b.Property<DateTimeOffset?>("PeriodFrom");

                b.Property<DateTimeOffset?>("PeriodTo");

                b.Property<string>("Person");

                b.Property<byte[]>("RawData");

                b.Property<bool>("SignatureIsValid");

                b.HasKey("Id");

                b.ToTable("IntegrationLogDigitalSignatures");
            });

            modelBuilder.Entity("Iserv.Niis.ExternalServices.Domain.Entities.IntegrationLogString", b =>
            {
                b.Property<int>("Id");

                b.Property<int>("Index");

                b.Property<string>("Value");

                b.HasKey("Id", "Index");

                b.ToTable("IntegrationLogStrings");
            });

            modelBuilder.Entity("Iserv.Niis.ExternalServices.Domain.Entities.IntegrationLogSystemInfo", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<string>("AdditionalInfo");

                b.Property<string>("ChainId");

                b.Property<DateTimeOffset?>("MessageDate");

                b.Property<string>("MessageId");

                b.Property<string>("Sender");

                b.Property<string>("StatusCode");

                b.Property<string>("StatusMessageKz");

                b.Property<string>("StatusMessageRu");

                b.HasKey("Id");

                b.ToTable("IntegrationLogSystemInfos");
            });

            modelBuilder.Entity("Iserv.Niis.ExternalServices.Domain.Entities.IntegrationMonitorLog", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<DateTimeOffset>("DbDateTime")
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("GETDATE()");

                b.Property<bool>("Error");

                b.Property<string>("Note");

                b.HasKey("Id");

                b.ToTable("IntegrationMonitorLogs");
            });

            modelBuilder.Entity("Iserv.Niis.ExternalServices.Domain.Entities.LogAction", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<DateTimeOffset?>("DbDateTime")
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("GETDATE()");

                b.Property<string>("Note");

                b.Property<string>("Project");

                b.Property<int?>("SystemInfoAnswerId");

                b.Property<int?>("SystemInfoQueryId");

                b.Property<string>("Type");

                b.HasKey("Id");

                b.ToTable("LogActions");
            });

            modelBuilder.Entity("Iserv.Niis.ExternalServices.Domain.Entities.LogSystemInfo", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<string>("AdditionalInfo");

                b.Property<string>("ChainId");

                b.Property<DateTimeOffset?>("DbDateTime")
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("GETDATE()");

                b.Property<DateTimeOffset?>("MessageDate");

                b.Property<string>("MessageId");

                b.Property<string>("Sender");

                b.Property<string>("StatusCode");

                b.Property<string>("StatusMessageKz");

                b.Property<string>("StatusMessageRu");

                b.HasKey("Id");

                b.ToTable("LogSystemInfos");
            });

            modelBuilder.Entity("Iserv.Niis.ExternalServices.Domain.Entities.Reference", b =>
            {
                b.Property<int>("Id");

                b.Property<string>("Type");

                b.Property<int?>("ParentId");

                b.Property<string>("Ru");

                b.HasKey("Id", "Type");

                b.ToTable("References");
            });
        }
    }
}