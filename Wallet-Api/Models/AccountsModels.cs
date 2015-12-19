using System.Collections.Generic;

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


    public class RestrictedCountriesResponseModel
    {
        public IEnumerable<string> RestrictedCountires { get; set; } 
    }


    public class AccountRegistrationModel
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactPhone { get; set; }
        public string Password { get; set; }
        public string ClientInfo { get; set; }
    }

    public class AuthenticateModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ClientInfo { get; set; }
    }

    public class AuthenticateResponseModel
    {
        public string KycStatus { get; set; }
        public bool PinIsEntered { get; set; }
        public string Token { get; set; }
    }

    public class GetRegistrationStatusResponseModel
    {
        public string KycStatus { get; set; }
        public bool PinIsEntered { get; set; }
    }


    public class PinSecurityChangeModel
    {
        public string Pin { get; set; }
    }

    public class AccountsRegistrationResponseModel
    {
        public string Token { get; set; }
        
    }

    public class GetRegistrationStateModel
    {
        public string KycStatus { get; set; }
    }

}