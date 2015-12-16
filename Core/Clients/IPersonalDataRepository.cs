using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Clients
{
    public interface IPersonalData
    {
        DateTime Regitered { get; }
        string Id { get; }
        string Email { get; }
        string FirstName { get; }
        string LastName { get; }
        string Country { get; }
        string Zip { get; }
        string City { get; }
        string Address { get; }
        string ContactPhone { get; }
    }

    public class PersonalData : IPersonalData
    {
        public DateTime Regitered { get; set; }
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string ContactPhone { get; set; }

        public static PersonalData Create(IClientAccount src, string firstname, string lastname)
        {
            return new PersonalData
            {
                Id = src.Id,
                Email = src.Email,
                ContactPhone = src.Phone,
                Regitered = src.Registered,
                FirstName = firstname,
                LastName = lastname
            };
        }
    }

    public interface IPersonalDataRepository
    {
        Task<IPersonalData> GetAsync(string id);
        Task<IEnumerable<IPersonalData>> GetAsync(IEnumerable<string> id);
        Task SaveAsync(IPersonalData personalData);
        Task<IPersonalData> ScanAndFindAsync(Func<IPersonalData, bool> func);

        Task UpdateAsync(IPersonalData personalData);
    }

}
