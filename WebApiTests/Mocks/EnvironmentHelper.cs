using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.IocContainer;
using Core.Clients;
using Wallet_Api;

namespace WebApiTests.Mocks
{
    public static class EnvironmentHelper
    {
        public static IClientAccount RegisterClient(this IoC ioc, string email, string password = "123")
        {
            var clientAccount = new ClientAccount
            {
                Email = email,
                Phone = "+7999222444555",
                Registered = DateTime.UtcNow
            };
            return ioc.GetObject<IClientAccountsRepository>().RegisterAsync(clientAccount, password).Result;
        }


        public static void SetIdentity(this IClientAccount account)
        {
            ApiDependencies.GetIdentity = o => account.Id;
        }

    }
}
