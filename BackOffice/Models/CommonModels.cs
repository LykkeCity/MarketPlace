namespace BackOffice.Models
{

    public static class WebSiteConstants
    {
        public const string PersonalAreaDiv = "#pamain";
    }


    public interface IPersonalAreaDialog
    {
        string Caption { get; set; }
        string Width { get; set; }
    }

    public interface IFindClientViewModel
    {
        /// <summary>
        /// Where should I Do Search request
        /// </summary>
        string RequestUrl { get; }
        /// <summary>
        /// Where should I put result Html
        /// </summary>
        string Div { get; }
    }

}