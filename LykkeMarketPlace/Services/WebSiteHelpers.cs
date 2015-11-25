namespace LykkeMarketPlace.Services
{
    public static class WebSiteHelpers
    {

        public static string MoneyToStr(this double src)
        {
            return src.ToString("0.00");
        }
    }
}