
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public interface IBackOfficeUser
    {
        string Id { get; }
        bool IsAdmn { get; }
    }

    public class BackOfficeUser : IBackOfficeUser
    {
        public string Id { get; set; }
        public bool IsAdmn { get; set; }


        public static BackOfficeUser CreateDefaultAdminUser(string id)
        {
            return new BackOfficeUser
            {
                Id = id,
                IsAdmn = true
            };
        }

    }

    public interface IBackOfficeUsersRepository
    {
        Task CreateAsync(IBackOfficeUser backOfficeUser, string password);

        Task<IBackOfficeUser> AuthenticateAsync(string username, string password);

        Task<bool> UserExists(string id);
        Task ChangePasswordAsync(string id, string newPassword);
    }
}
