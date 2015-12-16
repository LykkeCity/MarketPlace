using System.Threading.Tasks;

namespace Core.Clients
{
    /// <summary>
    /// Service, which makes SMS confirmation
    /// </summary>
    public interface ISrvSmsConfirmator
    {
        /// <summary>
        /// Send Sms confirmation to Client
        /// </summary>
        /// <param name="clientId">client Id</param>
        /// <returns></returns>
        Task SendSmsAsync(string clientId);

        /// <summary>
        /// Check Last Valid SMS code
        /// </summary>
        /// <param name="clientId">clientId</param>
        /// <param name="smsCode">sms code Id</param>
        /// <returns></returns>
        Task<bool> CheckSmsConfirmation(string clientId, string smsCode);
    }

}
