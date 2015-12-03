using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Web.Mvc;
using AzureRepositories;
using Common.IocContainer;
using Common.Log;
using Core.Kyc;
using LkeServices;
using MockServices;
using RepositoriesMock;


namespace LykkeWallet
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

        public static IPEndPoint FeedIpEndPoint
        {
            get
            {
                try
                {
                    var strings = ConfigurationManager.AppSettings["FeedIp"].Split(':');
                    return new IPEndPoint(IPAddress.Parse(strings[0]), int.Parse(strings[1]));
                }
                catch (Exception)
                {

                    return new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8090);
                }
            }
        }

    }




    public static class Dependencies
    {
        public static IKycRepository KycRepository { get; private set; }

        private static void PopulateDependencies(this IoC ioc)
        {
            KycRepository = ioc.GetObject<IKycRepository>();
        }

        public static IDependencyResolver CreateDepencencyResolver()
        {

            var dr = new MyDependencyResolver();
            var log = new LogToConsole();
            dr.IoC.Register<ILog>(log);

            #if DEBUG
            dr.IoC.BindAzureReposInMem();
            dr.IoC.BinMockAzureDebug();
            #else
            AzureRepoBinder.BindAzureRepositories(dr.IoC, Settings.ConnectionString, log);
            dr.IoC.BindMockAzureRepositories(Settings.ConnectionString, log);
            #endif

            dr.IoC.BindMockServices();
            dr.IoC.BindLykkeWalletServices();
            dr.IoC.StartTraderPortalServices();

            dr.IoC.PopulateDependencies();
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