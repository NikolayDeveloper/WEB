using Iserv.Niis.Migration.BusinessLogic.InsertOldData;
using Iserv.Niis.Migration.BusinessLogic.Utils;
using Iserv.Niis.Migration.Common;
using System;
using System.Threading.Tasks;

namespace Iserv.Niis.Migration.Host
{
    public class MigrateAllData
    {
        private readonly InsertAllDictionariesHandler _insertAllDictionariesHandler;
        private readonly InsertDicCustomersHandler _insertDicCustomersHandler;
        private readonly InsertSecurityUsersHandler _insertSecurityUsersHandler;
        private readonly InsertRequestsHandler _insertRequestsHandler;
        private readonly InsertDocumentsHandler _insertDocumentsHandler;
        private readonly InsertContractsHandler _insertContractsHandler;
        private readonly InsertProtectionDocsHandler _insertProtectionDocsHandler;
        private readonly PrepareMigrationHandler _prepareMigrationHandler;
        private readonly InsertPaymentsHandler _insertPaymentsHandler;
        private readonly InsertMainEntityRelationsHandler _insertMainEntityRelationsHandler;
        private readonly MigrateAttachments _migrateAttachments;

        public MigrateAllData(
            InsertAllDictionariesHandler insertAllDictionariesHandler,
            InsertDicCustomersHandler insertDicCustomersHandler,
            InsertSecurityUsersHandler insertSecurityUsersHandler,
            InsertRequestsHandler insertRequestsHandler,
            InsertDocumentsHandler insertDocumentsHandler,
            InsertProtectionDocsHandler insertProtectionDocsHandler,
            InsertContractsHandler insertContractsHandler,
            PrepareMigrationHandler prepareMigrationHandler,
            InsertPaymentsHandler insertPaymentsHandler,
            InsertMainEntityRelationsHandler insertMainEntityRelationsHandler,
            MigrateAttachments migrateAttachments)
        {
            _insertAllDictionariesHandler = insertAllDictionariesHandler;
            _insertDicCustomersHandler = insertDicCustomersHandler;
            _insertSecurityUsersHandler = insertSecurityUsersHandler;
            _insertRequestsHandler = insertRequestsHandler;
            _insertDocumentsHandler = insertDocumentsHandler;
            _insertProtectionDocsHandler = insertProtectionDocsHandler;
            _insertContractsHandler = insertContractsHandler;
            _prepareMigrationHandler = prepareMigrationHandler;
            _insertPaymentsHandler = insertPaymentsHandler;
            _insertMainEntityRelationsHandler = insertMainEntityRelationsHandler;
            _migrateAttachments = migrateAttachments;
        }

        public void StartMigrate()
        {
            Console.WriteLine("Start Migration!");

            try
            {
                PrepareMigration();
                MigrateDictionaries();
                MigrateDicCustomers();
                MigrateUsers();
                MigrateRequests();
                MigrateContracts();
                MigrateProtectionDocs();
                MigratePayments();
                MigrateDocuments();
                MigrateMainEntityRelations();

                Console.WriteLine($"\r\nСтарт миграции файлов для заявок");
                _migrateAttachments.MigrateRequestAttachments();

                Console.WriteLine($"\r\nСтарт миграции файлов для заявок");
                _migrateAttachments.MigrateRequestAttachments();

                Console.WriteLine($"\r\nСтарт миграции файлов для договоров");
                _migrateAttachments.MigrateContractAttachments();

                Console.WriteLine($"\r\nСтарт миграции файлов для Документов");
                _migrateAttachments.MigrateDocumentAttachments();

                ExecuteScriptsAfterMigrations();
            }
            catch (Exception ex)
            {
                Log.LogError(ex);

                Console.WriteLine(ex.Message + ex.StackTrace);
                Console.ReadLine();
            }

            Console.WriteLine("\r\nMigration complete!");

            Console.ReadLine();
        }

        #region Private methods


        private void PrepareMigration()
        {
            Console.WriteLine("Prepare to migration");
            _prepareMigrationHandler.Execute();
            Console.WriteLine("Prepare to migration completed");
        }

        private void MigrateDictionaries()
        {
            Console.WriteLine("Start migrate dictionaries");
            _insertAllDictionariesHandler.Execute();
            Console.WriteLine("Migration of dictionaries complete");
        }
        private void MigrateDicCustomers()
        {
            Console.WriteLine("Start migrate DicCustomers");
            _insertDicCustomersHandler.Execute();
            Console.WriteLine("Migration of DicCustomers complete");
        }
        private void MigrateUsers()
        {
            Console.WriteLine("Start migrate security entities");
            _insertSecurityUsersHandler.Execute();
            Console.WriteLine("Migration security entities complete");
        }

        private void MigrateRequests()
        {
            Console.WriteLine("Start migrate requests");
            _insertRequestsHandler.Execute();
            Console.WriteLine("Migration of requests complete");
        }

        private void MigrateContracts()
        {
            Console.WriteLine("Start migrate Contracts");
            _insertContractsHandler.Migrate();
            Console.WriteLine("Migration of Contracts complete");
        }

        private void MigrateProtectionDocs()
        {
            Console.WriteLine("Start migrate protectionDocs");
            _insertProtectionDocsHandler.Execute();
            Console.WriteLine("Migration of protectionDocs complete");
        }

        private void MigratePayments()
        {
            Console.WriteLine("Start migrate payments");
            _insertPaymentsHandler.Execute();
            Console.WriteLine("Migration of payments complete");
        }

        private void MigrateDocuments()
        {
            Console.WriteLine("Start migrate documents");
            _insertDocumentsHandler.Execute();
            Console.WriteLine("Migration of documents complete");
        }

        private void MigrateMainEntityRelations()
        {
            Console.WriteLine("Start migrate MainEntityRelations");
            _insertMainEntityRelationsHandler.Execute();
            Console.WriteLine("Migration of MainEntityRelations complete");
        }


        private void ExecuteScriptsAfterMigrations()
        {
            Console.WriteLine("Start ExecuteScriptsAfterMigration");
            _prepareMigrationHandler.ExecuteScriptsByPath("./UpdateDatabaseAfterMigrationScripts");
            Console.WriteLine("End ExecuteScriptsAfterMigration");
        }

        #endregion
    }
}
