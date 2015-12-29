namespace Wallet_Api.Models
{

    public class GetKycStatusRespModel
    {
        public string KycStatus { get; set; }
        public ApiPersonalDataModel PersonalData { get; set; }


        public static GetKycStatusRespModel Create(string kycStatus, ApiPersonalDataModel personalData)
        {
            return new GetKycStatusRespModel
            {
                KycStatus = kycStatus,
                PersonalData = personalData
            };
        }
    }

    public class KycDocumentsModel
    {
        public string Type { get; set; }
        public string Ext { get; set; }
        public string Data { get; set; }
    }


    public class PostKycDocumentRespModel
    {
        public ApiPersonalDataModel PersonalData { get; set; }

        public static PostKycDocumentRespModel Create(ApiPersonalDataModel personalData)
        {
            return new PostKycDocumentRespModel
            {
                PersonalData = personalData
            };
        }
    }

    public class PostKycStatusRespModel
    {
        public ApiPersonalDataModel PersonalData { get; set; }

        public static PostKycStatusRespModel Create(ApiPersonalDataModel personalData)
        {
            return new PostKycStatusRespModel
            {
                PersonalData = personalData
            };
        }
    }
}