namespace LykkeWallet.Services
{
    public static class WebSiteHelpers
    {

        public const string AppCaption = "Lykke wallet";

        public const string MainContentDiv = "#pamain";

        public static string MoneyToStr(this double src)
        {
            return src.ToString("0.00");
        }
    }
}