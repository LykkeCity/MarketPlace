using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Owin.Extensions;
using Owin;

namespace Wallet_Api
{
    public static class LykkeAuthMiddlware
    {
        public class LykkeIdentity : IIdentity
        {
            public string Name { get; private set; }
            public string AuthenticationType { get; private set; }
            public bool IsAuthenticated { get; private set; }
            public DateTime Created { get; private set; }

            public static LykkeIdentity Create(string clientId)
            {
                return new LykkeIdentity
                {
                    Name = clientId,
                    AuthenticationType = "Token",
                    Created = DateTime.UtcNow,
                    IsAuthenticated = true
                };
            }
        }

        private static readonly Dictionary<string, ClaimsPrincipal> PrincipalsCache = new Dictionary<string, ClaimsPrincipal>();

        private  static readonly ReaderWriterLockSlim ReaderWriterLockSlim = new ReaderWriterLockSlim();

        private static async Task<ClaimsPrincipal> ReadPrincipal(string token)
        {
            ReaderWriterLockSlim.EnterReadLock();
            try
            {
                if (PrincipalsCache.ContainsKey(token))
                    return PrincipalsCache[token];
            }
            finally
            {
                ReaderWriterLockSlim.ExitReadLock();
            }

            var session = await ApiDependencies.ClientsSessionsRepository.GetAsync(token);
            if (session == null)
                return null;


            ReaderWriterLockSlim.EnterWriteLock();
            try
            {
                if (PrincipalsCache.ContainsKey(token))
                    return PrincipalsCache[token];

                var principal =
                    new ClaimsPrincipal(LykkeIdentity.Create(session.ClientId));

                PrincipalsCache.Add(token, principal);

                return principal;

            }
            finally
            {
                ReaderWriterLockSlim.ExitWriteLock();
            }


        }

        public static void ConfigureLykkeAuth(this IAppBuilder app)
        {
            app.Use(async (ctx, next) =>
            {
                try
                {
                    var header = ctx.Request.Headers["Authorization"];

                    if (string.IsNullOrEmpty(header))
                        return;

                    var values = header.Split(' ');

                    if (values[0].Length < 2)
                        return;

                    if (values[0] != "Bearer")
                        return;

                    if (string.IsNullOrEmpty(values[1]))
                        return;

                    ctx.Authentication.User = await ReadPrincipal(values[1]);
         

                }
                finally
                {
                    await next();
                }
            });

            app.UseStageMarker(PipelineStage.PreHandlerExecute);
        }

     
    }
}
