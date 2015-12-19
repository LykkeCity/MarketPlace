using System.Threading.Tasks;

namespace Core
{
    public interface IIpGeolocationData
    {

        string CountryCode { get; }

        string Isp { get; }

        string Ip { get; }

        string City { get; }

        string Region { get; }

        string Longitude { get; }

        string Latitude { get; }
    }

    public interface ISrvIpGetLocation
    {

        IIpGeolocationData GetData(string ip);

        /// <summary>
        /// Получить информацию о стране и городе по IP
        /// </summary>
        /// <param name="ip">ip</param>
        /// <returns></returns>
        Task<IIpGeolocationData> GetDataAsync(string ip);
    }
}
