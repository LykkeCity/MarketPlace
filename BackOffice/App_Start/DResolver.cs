using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using AzureRepositories;
using BackOfficeAzureRepositories;
using Common.IocContainer;
using Common.Log;
using Core;

namespace BackOffice
{

    public static class Settings
    {

        public static string ConnectionString
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["ConnectionString"];
                }
                catch (Exception)
                {

                    return "UseDevelopmentStorage=true";
                }
            }
        }

    }

    public static class Dependencies
    {

        private static void CreateAdminUser(IoC ioc)
        {
            var usersRepo = ioc.GetObject<IBackOfficeUsersRepository>();

            var adminUser = usersRepo.UserExists("admin").Result;

            if (!adminUser)

                usersRepo.CreateAsync(BackOfficeUser.CreateDefaultAdminUser("admin"), "123").Wait();
        }

        public static IDependencyResolver CreateDepencencyResolver()
        {
            var dr = new MyDependencyResolver();
            var log = new LogToConsole();
            dr.IoC.Register<ILog>(log);

#if DEBUG
            dr.IoC.BindAzureReposInMem();
            dr.IoC.BindBackOfficeRepositoriesInMemory();

#else
            AzureRepoBinder.BindAzureRepositories(dr.IoC, Settings.ConnectionString, log);
            dr.IoC.BindBackOfficeRepositories(Settings.ConnectionString, log);
#endif
            CreateAdminUser(dr.IoC);
            return dr;
        }

    }



    public class MyDependencyResolver : IDependencyResolver
    {

        public readonly IoC IoC = new IoC();

        public void RegisterSingleTone<T, TI>() where TI : T
        {
            IoC.RegisterSingleTone<T, TI>();
        }

        public void RegisterSingleTone<T, TI>(TI instance) where TI : T
        {
            IoC.Register<T>(instance);
        }

        public T GetType<T>() where T : class
        {
            var result = IoC.GetObject<T>();
            return result;
        }

        public object GetService(Type serviceType)
        {
            var result = IoC.CreateInstance(serviceType);

            return result;
        }

        private readonly object[] _nullData = new object[0];
        public IEnumerable<object> GetServices(Type serviceType)
        {
            var result = IoC.CreateInstance(serviceType);
            return result == null ? _nullData : new[] { result };
        }

    }
}