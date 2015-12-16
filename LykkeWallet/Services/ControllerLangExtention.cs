using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace LykkeWallet.Services
{
    public static class ControllerLangExtention
    {
        public const string LangCookie = "Lang";
        private const string DefaultLang = "en";

        private static readonly Dictionary<string, string> Localizations = new Dictionary<string, string>
        {
            {"en", "en"},
            {"ru", "ru"},

        };


        public static IEnumerable<string> GetLanguages()
        {
            return Localizations.Keys;
        }



        public static void SetThread(string langId)
        {
            if (string.IsNullOrEmpty(langId))
                langId = DefaultLang;
            else
            langId = Localizations.ContainsKey(langId) ? Localizations[langId] : DefaultLang;

            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(langId);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        public static void SetLanguage(this Controller ctx, string langId)
        {
            langId = langId.ToLower();
            if (!Localizations.ContainsKey(langId))
                langId = DefaultLang;

            ctx.Response.SetCookie(new HttpCookie(LangCookie, langId) {Expires = DateTime.UtcNow.AddYears(5)});
        }
    }
}