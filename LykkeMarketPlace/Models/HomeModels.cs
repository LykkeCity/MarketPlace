
using Core.Traders;

namespace LykkeMarketPlace.Models
{

    public class SignInDialogViewModel
    {
        public string Email { get; set; }
    }

    public class SignInModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class SignUpModel : ITrader
    {
        public string Id => null;
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordAgain { get; set; }
    }
}