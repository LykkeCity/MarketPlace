
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


}