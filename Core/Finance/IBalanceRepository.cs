using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Finance
{
    public interface ITraderBalance
    {
        string TraderId { get; }
        string Currency { get; }
        double Amount  { get; }

    }


    public interface IBalanceRepository
    {

        Task<IEnumerable<ITraderBalance>> GetAsync(string traderId);

        Task ChangeBalanceAsync(string traderId, string currency, double delta);


    }
}
