using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Owin;

namespace Wallet_Api
{
    public static class LocalizationModule
    {
        public const string LangCookie = "Language";



        public static void SetThread(string langId)
        {

           // Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(langId);
            //Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        private static void DetectLanguage(IOwinRequest request)
        {
            var lang = request.Cookies[LangCookie];
            SetThread(lang);
        }

        public static IAppBuilder Localize(this IAppBuilder app, PipelineStage stage = PipelineStage.PreHandlerExecute)
        {
            if (app == null)
            {
                throw new ArgumentNullException("app");
            }

            app.Use(
                (ctx, next) =>
                {

#if DEBUG
#else
                    if (!ctx.Request.IsSecure)
                    {
                        string url = "https://" + ctx.Request.Uri.Host + ctx.Request.Uri.PathAndQuery;
                        ctx.Response.Redirect(url);
                    }

#endif


                    DetectLanguage(ctx.Request);

                    return next.Invoke();
                });

            app.UseStageMarker(stage);

            return app;
        }

    }
}