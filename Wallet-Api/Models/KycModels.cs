namespace Wallet_Api.Models
{

    public class KycModelStatusResponseModel
    {
        public string KycStatus { get; set; }
    }

    public class KycDocumentsModel
    {
        public string Type { get; set; }
        public string Ext { get; set; }
        public string Data { get; set; }
    }

}