
namespace Wallet_Api.Models
{
    public class OkResponseModel
    {
        public string result { get; set; }
        public static readonly OkResponseModel Instance = new OkResponseModel {result = "ok"};
    }

    public class FailResponseModel
    {
        public string result { get; set; }
        public string message { get; set; }

        public static FailResponseModel Create(string message)
        {
            return new FailResponseModel
            {
                result = "fail",
                message = message
            };
        }

    }


    public class FailFieldModel
    {
        public string result { get; set; }
        public string field { get; set; }
        public string message { get; set; }

        public static FailFieldModel Create(string field, string message)
        {
            return new FailFieldModel
            {
                result = "fail",
                field = field,
                message = message
            };
        }
    }
}