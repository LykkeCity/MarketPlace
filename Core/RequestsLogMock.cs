using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class RequestsLogMock : IRequestsLog
    {
        public Task WriteAsync(string userId, string url, string request, string response)
        {
            return Task.FromResult(0);
        }
    }
}
