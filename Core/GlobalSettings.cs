namespace Core
{

    public static class GlobalSettings
    {
        public const int Mt4TimeOffset = 2;

        /*
        public static IEnumerable<FinanceInstrument> GetFinanceInstruments(string currency)
        {
            return FinanceInstruments.Values.Where(itm => itm.Base == currency || itm.Quoting == currency);
        }

        public static FinanceInstrument FindInstument(string curFrom, string curTo)
        {
            foreach (var fi in FinanceInstruments.Values)
            {
                if (fi.Base == curFrom && fi.Quoting == curTo)
                    return fi;

                if (fi.Quoting == curFrom && fi.Base == curTo)
                    return fi;


            }

            return null;
        }
        */
    }
}
