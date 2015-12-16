using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Clients
{
    public interface IClientSession
    {
        string ClientId { get; }
        string Token { get; }
        DateTime Registered { get; }
        DateTime LastAction { get; }
    }

    public interface IClientsSessionsRepository
    {
        Task SaveAsync(string clientId, string token);
        Task<IClientSession> GetAsync(string token);
        Task<IEnumerable<IClientSession>> GetByClientAsync(string clientId);
    }

}
