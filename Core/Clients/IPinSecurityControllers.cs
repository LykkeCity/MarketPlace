using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Clients
{

    public interface IPinSecurityRepository
    {
        Task SaveAsync(string clientId, string pin);
        Task<bool> CheckAsync(string clientId, string pin);
    }

}
