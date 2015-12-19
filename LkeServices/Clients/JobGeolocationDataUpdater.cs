using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Core;
using Core.Clients;
using Core.EventLogs;

namespace LkeServices.Clients
{
    public class JobGeolocationDataUpdater : TimerPeriod, IRegistrationConsumer
    {
        private readonly ISrvIpGetLocation _srvIpGetLocation;
        private readonly IRegistrationLogs _registrationLogs;
        private readonly IPersonalDataRepository _personalDataRepository;

        public class RegistrationEvent
        {
            public IClientAccount ClientAccount { get; set; }
            public string Ip { get; set; }
            public string RegId { get; set; }
        }

        private readonly Queue<RegistrationEvent> _queue = new Queue<RegistrationEvent>(); 

        public Task ConsumeRegistration(IClientAccount account, string ip, string regId)
        {
            lock(_queue)
                _queue.Enqueue(new RegistrationEvent
                {
                    ClientAccount = account,
                    Ip = ip,
                    RegId = regId
                });


            return Task.FromResult(0);
        }

        public JobGeolocationDataUpdater(ISrvIpGetLocation srvIpGetLocation, IRegistrationLogs registrationLogs, 
            IPersonalDataRepository personalDataRepository,
            ILog log) : base("JobGeolocationReader", 5000, log)
        {
            _srvIpGetLocation = srvIpGetLocation;
            _registrationLogs = registrationLogs;
            _personalDataRepository = personalDataRepository;
        }


        private RegistrationEvent GetEvent()
        {
            lock (_queue)
                return _queue.Count == 0 ? null : _queue.Dequeue();
        }

        protected override async Task Execute()
        {
            var evnt = GetEvent();

            while (evnt != null)
            {

                var geo = await _srvIpGetLocation.GetDataAsync(evnt.Ip);
                await _registrationLogs.UpdateGeolocationDataAsync(evnt.RegId, geo.CountryCode, geo.City, geo.Isp);

                await
                    _personalDataRepository.UpdateGeolocationDataAsync(evnt.ClientAccount.Id, geo.CountryCode, geo.City);

                evnt = GetEvent();
            }
        }
    }
}
