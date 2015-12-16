
namespace Wallet_Api.Models
{
    public class PinSecurityCheckResultModel
    {
        public bool Passed { get; set; }
    }

    public class IsPinSecurityEnabledResultModel
    {
        public bool IsEnabled { get; set; }
    }
}