using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Owin;
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

        private static async Task<ClaimsPrincipal> ReadPrincipal(string token)
        {

            var session = await ApiDependencies.ClientsSessionsRepository.GetAsync(token);
            if (session == null)
                return null;


            return  new ClaimsPrincipal(LykkeIdentity.Create(session.ClientId));
        }


        private static async Task<string> ReadRequest(IOwinRequest request)
        {
            if (request.Method == "GET")
                return null;

            var sr = new StreamReader(request.Body);
            return await sr.ReadToEndAsync();
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

                    var principal = await ReadPrincipal(values[1]);
                    ctx.Authentication.User = principal;

                    if (principal != null)
                    {

                        var request = await ReadRequest(ctx.Request);

                        await
                            ApiDependencies.RequestsLog.WriteAsync(principal.Identity.Name, "["+ctx.Request.Method+"]"+ctx.Request.Uri.PathAndQuery,
                                request, null);


                    }
                        

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
