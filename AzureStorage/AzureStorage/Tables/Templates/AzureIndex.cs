using System;
using System.Threading.Tasks;
using Common;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureStorage.Tables.Templates
{
    public class AzureIndex : TableEntity, IAzureIndex
    {
        public AzureIndex()
        {

        }

        public AzureIndex(string partitionKey, string rowKey, string primaryPartitionKey, string primaryRowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
            PrimaryPartitionKey = primaryPartitionKey;
            PrimaryRowKey = primaryRowKey;
        }

        public AzureIndex(string partitionKey, string rowKey, ITableEntity tableEntity)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
            PrimaryPartitionKey = tableEntity.PartitionKey;
            PrimaryRowKey = tableEntity.RowKey;

        }



        public string PrimaryPartitionKey { get; set; }
        public string PrimaryRowKey { get; set; }


        public static AzureIndex Create(string partitionKey, string rowKey, ITableEntity tableEntity)
        {
            return new AzureIndex
            {
                PartitionKey = partitionKey,
                RowKey = rowKey,
                PrimaryPartitionKey = tableEntity.PartitionKey,
                PrimaryRowKey = tableEntity.RowKey
            };
        }
    }

    public class AzureDoubleIndex : TableEntity, IAzureDoubleIndex
    {
        public string PrimaryPartitionKey { get; set; }
        public string PrimaryRowKey { get; set; }
        public string PrimaryPartitionKey2 { get; set; }
        public string PrimaryRowKey2 { get; set; }

        public static AzureDoubleIndex Create(string partitionKey, string rowKey, ITableEntity tableEntity,
            ITableEntity tableEntity2)
        {
            return new AzureDoubleIndex
            {
                PartitionKey = partitionKey,
                RowKey = rowKey,
                PrimaryPartitionKey = tableEntity.PartitionKey,
                PrimaryRowKey = tableEntity.RowKey,
                PrimaryPartitionKey2 = tableEntity2.PartitionKey,
                PrimaryRowKey2 = tableEntity2.RowKey
            };
        }

    }


    public static class AzureIndexUtils
    {

        public async static Task<T> GetDataAsync<T>(this INoSQLTableStorage<T> tableStorage, IAzureIndex index) where T : class, ITableEntity, new()
        {
            if (index == null)
                return null;

            return await tableStorage.GetDataAsync(index.PrimaryPartitionKey, index.PrimaryRowKey);
        }

        public async static Task<T> GetDataAsync<T>(this INoSQLTableStorage<AzureIndex> indexTableStorage, string indexPartitionKey, string indexRowKey, INoSQLTableStorage<T> tableStorage) where T : class, ITableEntity, new()
        {
            var indexEntity = await indexTableStorage.GetDataAsync(indexPartitionKey, indexRowKey);
            return await tableStorage.GetDataAsync(indexEntity);
        }




        public async static Task<T> ReplaceAsync<T>(this INoSQLTableStorage<AzureIndex> indexTableStorage, string indexPartitionKey, string indexRowKey, INoSQLTableStorage<T> tableStorage, Func<T,T> action) where T : class, ITableEntity, new()
        {
            var indexEntity = await indexTableStorage.GetDataAsync(indexPartitionKey, indexRowKey);
            return await tableStorage.ReplaceAsync(indexEntity, action);
        }


        public async static Task<T> ReplaceAsync<T>(this INoSQLTableStorage<T> tableStorage, IAzureIndex index, Func<T, T> action) where T : class, ITableEntity, new()
        {
            if (index == null)
                return null;

            return await tableStorage.ReplaceAsync(index.PrimaryPartitionKey, index.PrimaryRowKey, action);
        }


        /// <summary>
        /// Delete index and entity
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="indexTableStorage">Table storage with indexes</param>
        /// <param name="indexPartitionKey">Index partition key</param>
        /// <param name="indexRowKey">Index row key</param>
        /// <param name="tableStorage">Table storage with entities</param>
        /// <returns>Deleted entity</returns>
        public static async Task<T> DeleteAsync<T>(this INoSQLTableStorage<AzureIndex> indexTableStorage,
            string indexPartitionKey, string indexRowKey, INoSQLTableStorage<T> tableStorage)
            where T : class, ITableEntity, new()
        {
            var indexEntity = await indexTableStorage.DeleteAsync(indexPartitionKey, indexRowKey);

            if (indexEntity == null)
                return null;
            return await tableStorage.DeleteAsync(indexEntity);
        }


    }
}
