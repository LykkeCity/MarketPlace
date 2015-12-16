using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.BackOffice
{

    public static class MenuBadges
    {
        public const string Kyc = "KYC";
    }

    public interface IMenuBadge
    {
        string Id { get; }
        string Value { get; }
    }

    public interface IMenuBadgesRepository
    {
        Task SaveBadgeAsync(string id, string value);
        Task RemoveBadgeAsync(string id);
        Task<IEnumerable<IMenuBadge>> GetBadesAsync();
    }
}
