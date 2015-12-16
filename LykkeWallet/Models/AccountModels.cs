
namespace LykkeWallet.Models
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

    public class SignUpViewModel
    {
        public string Email { get; set; }
    }

    public class SignUpModel
    {
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string PasswordAgain { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}