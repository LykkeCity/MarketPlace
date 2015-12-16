using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureRepositories;
using Common.IocContainer;
using LkeServices;
using MockServices;
using RepositoriesMock;

namespace WebApiTests.Mocks
{
    public static class EnvironmentCreator
    {

        public static IoC CreateEnvironment()
        {
            var ioc = new IoC();
             ioc.BindAzureReposInMemForTests();
             ioc.BinMockAzureTests();
             ioc.BindLykkeWalletServices();
             ioc.BindMockServices();
            return ioc;
        }
    }
}
