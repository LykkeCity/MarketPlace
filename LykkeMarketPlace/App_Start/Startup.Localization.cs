using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using LykkeMarketPlace.Services;
using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Owin;

namespace LykkeMarketPlace
{
    public static class LocalizationModule
    {

        private static void DetectLanguage(IOwinRequest request)
        {
            var lang = request.Cookies[ControllerLangExtention.LangCookie];
            ControllerLangExtention.SetThread(lang);
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
                    DetectLanguage(ctx.Request);

                    return next.Invoke();
                });

            app.UseStageMarker(stage);

            return app;
        }

    }


}

