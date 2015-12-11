namespace Wallet_Api.Models
{
    public class AccountExistResultModel
    {
        public bool IsEmailRegistered { get; set; }
    }

    public class CheckDocumentsToUploadModel
    {
        public bool IdCard { get; set; }
        public bool ProofOfAddress { get; set; }
        public bool Selfie { get; set; }

    }
}