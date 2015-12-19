using AzureRepositories.Accounts;
using AzureRepositories.Assets;
using AzureRepositories.BackOffice;
using AzureRepositories.Clients;
using AzureRepositories.EventLogs;
using AzureRepositories.Finance;
using AzureRepositories.Kyc;
using AzureRepositories.Orders;
using AzureStorage.Blob;
using AzureStorage.Tables;
using AzureStorage.Tables.Templates.Index;
using Common.Log;

namespace AzureRepositories
{
    public static class AzureRepoFactories
    {

        public static BalanceRepository CreateBalanceRepository(string connstring, ILog log)
        {
            return new BalanceRepository(new AzureTableStorage<TraderBalanceEntity>(connstring, "AccountBalances", log));
        }


        public static IdentityGenerator CreateIdentityGenerator(string connstring, ILog log)
        {
            return new IdentityGenerator(new AzureTableStorage<IdentityEntity>(connstring, "Setup", log));
        }



        public static OrdersRepository CreateOrdersRepository(string connstring, ILog log)
        {
            return new OrdersRepository(new AzureTableStorage<OrderEntity>(connstring, "Orders", log));
        }


        public static ClientSettingsRepository CreateTraderSettingsRepository(string connString, ILog log)
        {
            return new ClientSettingsRepository(new AzureTableStorage<ClientSettingsEntity>(connString, "TraderSettings", log));
        }


        public static BrowserSessionsRepository CreateBrowserSessionsRepository(string connString, ILog log)
        {
            return new BrowserSessionsRepository(new AzureTableStorage<BrowserSessionEntity>(connString, "BrowserSessions", log));
        }


        public static class Accounts
        {
            public static AccountsRepository CreateAccountsRepository(string connString, ILog log)
            {
                return new AccountsRepository(new AzureTableStorage<AccountEntity>(connString, "Accounts", log));
            }
        }

        public static class EventLogs
        {
            public static RegistrationLogs CreateRegistrationLogs(string connecionString, ILog log)
            {
                return new RegistrationLogs(new AzureTableStorage<RegistrationLogEventEntity>(connecionString, "LogRegistrations", log));
            }
        }


        public static class Clients
        {
            public static ClientsRepository CreateTradersRepository(string connstring, ILog log)
            {
                const string tableName = "Traders";
                return new ClientsRepository(
                    new AzureTableStorage<ClientAccountEntity>(connstring, tableName, log),
                    new AzureTableStorage<AzureIndex>(connstring, tableName, log));
            }

            public static ClientSessionsRepository CreateClientSessionsRepository(string connstring, ILog log)
            {
                return new ClientSessionsRepository(
                    new AzureTableStorage<ClientSessionEntity>(connstring, "Sessions", log));
            }

            public static PersonalDataRepository CreatePersonalDataRepository(string connString, ILog log)
            {
                return new PersonalDataRepository(new AzureTableStorage<PersonalDataEntity>(connString, "PersonalData", log));
            }

            public static KycRepository CreateKycRepository(string connString, ILog log)
            {
                return new KycRepository(new AzureTableStorage<KycEntity>(connString, "KycStatuses", log));
            }



            public static KycDocumentsRepository CreateKycDocumentsRepository(string connString, ILog log)
            {
                return new KycDocumentsRepository(new AzureTableStorage<KycDocumentEntity>(connString, "KycDocuments", log));
            }

            public static KycDocumentsScansRepository CreatKycDocumentsScansRepository(string connString)
            {
                return new KycDocumentsScansRepository(new AzureBlobStorage(connString));
            }

            public static KycUploadsLog CreateKycUploadsLog(string connString, ILog log)
            {
                return
                    new KycUploadsLog(new AzureTableStorage<KycUploadsLogItemEntity>(connString, "KycUploadsLog", log));
            }

            public static PinSecurityRepository CreatePinSecurityRepository(string connString, ILog log)
            {
                return new PinSecurityRepository(new AzureTableStorage<PinSecurityEntity>(connString, "ClientPins", log));
            }


        }


        public static class BackOffice
        {
            public static MenuBadgesRepository CreateMenuBadgesRepository(string connecionString, ILog log)
            {
                return new MenuBadgesRepository(new AzureTableStorage<MenuBadgeEntity>(connecionString, "MenuBadges", log));
            }

        }

        private const string TableNameDictionaries = "Dictionaries";
        public static class Dictionaries
        {
            public static AssetsRepository CreateAssetsRepository(string connstring, ILog log)
            {
                return new AssetsRepository(new AzureTableStorage<AssetEntity>(connstring, TableNameDictionaries, log));
            }

            public static AssetPairsRepository CreateAssetPairsRepository(string connString, ILog log)
            {
                return new AssetPairsRepository(new AzureTableStorage<AssetPairEntity>(connString, TableNameDictionaries, log));
            }
        }

    }

}
