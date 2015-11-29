﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using BackOffice.Services;
using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Owin;

namespace BackOffice
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

