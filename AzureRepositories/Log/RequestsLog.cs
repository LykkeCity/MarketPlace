using System;
using System.Threading.Tasks;
using AzureStorage;
using Core;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureRepositories.Log
{
    public class RequestLogItem : TableEntity
    {
        public static string GeneratePartitionKey(string userId)
        {
            return userId;
        }

        public DateTime DateTime { get; set; }
        public string Url { get; set; }
        public string Request { get; set; }

        public string Response { get; set; }

        private const int MaxFieldSize = 1024 * 4;

        public static RequestLogItem Create(string userId, string url, string request, string response)
        {
            if (request?.Length > MaxFieldSize)
                request = request.Substring(0, MaxFieldSize);

            return new RequestLogItem
            {
                PartitionKey = GeneratePartitionKey(userId),
                Url = url,
                Request = request,
                Response = response,
                DateTime = DateTime.UtcNow
            };
        }
    }

    public class RequestsLog : IRequestsLog
    {
        private readonly INoSQLTableStorage<RequestLogItem> _tableStorage;

        public RequestsLog(INoSQLTableStorage<RequestLogItem> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public Task WriteAsync(string userId, string url, string request, string response)
        {
            var newEntity = RequestLogItem.Create(userId, url, request, response);
            return _tableStorage.InsertAndGenerateRowKeyAsDateTimeAsync(newEntity, newEntity.DateTime);
        }
    }
}
