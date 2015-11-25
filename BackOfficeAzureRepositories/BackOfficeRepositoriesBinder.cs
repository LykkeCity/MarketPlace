using AzureStorage.Tables;
using BackOfficeAzureRepositories.Users;
using Common.IocContainer;
using Common.Log;
using Core;

namespace BackOfficeAzureRepositories
{
    public static class BackOfficeRepositoriesBinder
    {

        public static void BindBackOfficeRepositories(this IoC ioc, string connString, ILog log)
        {
            ioc.Register<IBackOfficeUsersRepository>(
                new BackOfficeUsersRepository(new AzureTableStorage<BackOfficeUserEntity>(connString, "BackOfficeUsers",
                    log)));
        }



        public static void BindBackOfficeRepositoriesInMemory(this IoC ioc)
        {
            ioc.Register<IBackOfficeUsersRepository>(
                new BackOfficeUsersRepository(new NoSqlTableInMemory<BackOfficeUserEntity>()));
        }

    }

}
