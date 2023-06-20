using Azure.Data.Tables;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Azure.Cosmos.Table;
using CloudStorageAccount = Microsoft.Azure.Cosmos.Table.CloudStorageAccount;

namespace AzureStorageTable.SDK
{
    public class Setup
    {
        private readonly string storageConnectionString = "DefaultEdpointPotocol=http;AcountNam=storsouvik;AcontKey=h7EO7EJzTQiBRuttrrreee+yyrqq4qfZ1yc1yH9AjG66I0+eb4yP41l99994Ay+ASte33wwss;EndpointSuffix=core.window.net";
        private readonly string AccountName = "storbysovik";
        private readonly string AccountKey = "h7geeeeeefZ1yc1yH9AjGgggg+ewwyP41luWzLk8esdffkwwAy+wwte=";
        private string _azureTableName;

        public Setup(string tableName)
        {
            _azureTableName = tableName;
        }
        public async Task<TableClient> ClientConfigurer()
        {
            var serviceClient = new TableServiceClient(storageConnectionString);
            var tableClient = serviceClient.GetTableClient(_azureTableName);
            var tableExistanceStatus = tableClient.CreateIfNotExists();
            if (tableExistanceStatus.GetRawResponse().Status == 201)
            {
                Console.WriteLine($"New table named '{_azureTableName}' is created in Azure bcs it didn't exist.");
            }
            await Task.CompletedTask;
            return tableClient;
        }
    }
}