using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Http;
using System.Web.Http.Dependencies;
using AzureRepositories;
using AzureRepositories.Log;
using AzureStorage.Tables;
using Common.IocContainer;
using Common.Log;
using Core.Clients;
using LkeServices;
using MockServices;
using RepositoriesMock;

namespace Wallet_Api
{
    public static class ApiDependencies
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


        public static Func<object, string> GetIdentity;


        public static IClientsSessionsRepository ClientsSessionsRepository { get; private set; }

        public static IDependencyResolver Create()
        {
            var result = new MyDependencyResolver();

#if DEBUG
            var log = new LogToConsole();
            result.IoC.BindAzureReposInMem();
            result.IoC.BinMockAzureDebug();

            #else
            var log = new LogToTable(new AzureTableStorage<LogEntity>(Settings.ConnectionString, "LogApi", null));

            result.IoC.BindAzureRepositories(Settings.ConnectionString, Settings.ConnectionString, log);
                      result.IoC.BindMockAzureRepositories(Settings.ConnectionString, log);
            #endif

            result.IoC.BindLykkeWalletServices();
            result.IoC.BindMockServices();

            GetIdentity = ctr =>
            {
                var ctx = ctr as ApiController;
                return ctx?.User.Identity.Name;
            };

            ClientsSessionsRepository = result.IoC.GetObject<IClientsSessionsRepository>();

            log.WriteInfo("ApiDependencies", "Create", "Create", "Create");

            return result;
        }

        public class MyDependencyResolver : IDependencyResolver
        {
            public class MyDependencyScope : IDependencyScope
            {
                private readonly IoC _ioc;

                public MyDependencyScope(IoC ioc)
                {
                    _ioc = ioc;
                }

                public void Dispose()
                {
                }

                public object GetService(Type serviceType)
                {
                    var result = _ioc.CreateInstance(serviceType);

                    return result;
                }
                internal static readonly object[] NullData = new object[0];
                public IEnumerable<object> GetServices(Type serviceType)
                {
                    var result = _ioc.CreateInstance(serviceType);
                    return result == null ? NullData : new[] { result };
                }
            }


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

      
            public IEnumerable<object> GetServices(Type serviceType)
            {
                var result = IoC.CreateInstance(serviceType);
                return result == null ? MyDependencyScope.NullData : new[] { result };
            }


            private MyDependencyScope _myDependencyScope;

            public IDependencyScope BeginScope()
            {
                return _myDependencyScope ?? (_myDependencyScope = new MyDependencyScope(IoC));
            }

            public void Dispose()
            {
             
            }
        }

    }
}