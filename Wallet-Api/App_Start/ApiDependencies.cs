using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Http.Dependencies;
using AzureRepositories;
using Common.IocContainer;
using Common.Log;
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

        public static IDependencyResolver Create()
        {
            var result = new MyDependencyResolver();
            var log = new LogToConsole();
#if DEBUG
            result.IoC.BindAzureReposInMem();
            result.IoC.BinMockAzureDebug();


#else
                        result.IoC.BindAzureRepositories(Settings.ConnectionString, Settings.ConnectionString, log);
                      result.IoC.BindMockAzureRepositories(Settings.ConnectionString, log);
#endif

            result.IoC.BindLykkeWalletServices();
            result.IoC.BindMockServices();
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