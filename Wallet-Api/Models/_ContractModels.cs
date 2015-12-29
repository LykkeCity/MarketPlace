
using Core.Clients;

namespace Wallet_Api.Models
{
    public class ApiPersonalDataModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }

    public class ApiWalletAssetModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Balance { get; set; }
        public string Symbol { get; set; }
    }

    public class ApiBankCardModel
    {
        public string Id { get; set; }
        public char Type { get; set; }
        public string LastDigits { get; set; }
    }

    public static class DomainToContractConverter
    {
        public static ApiPersonalDataModel ConvertToApiModel(this IPersonalData src)
        {
            if (src == null)
                return null;

            return new ApiPersonalDataModel
            {
                FullName = src.FullName,
                Email = src.Email,
                Phone = src.ContactPhone
            };
        }
    }
}