using System;
using Core.EventLogs;

namespace BackOffice.Areas.EventLogs.Models
{
    public class EvntLogsRegsIndexViewModel
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }

    public class GetEvntLogsRegsModel
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }


    public class GetEvntLogsRegsViewModel
    {
        public IRegistrationLogEvent[] Events { get; set; }
    }
}