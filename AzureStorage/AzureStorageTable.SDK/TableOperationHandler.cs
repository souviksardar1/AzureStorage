using AzureStorageTable.SDK.FwkModels;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.WindowsAzure.Storage.Table;
using MoreLinq;
using TableOperation = Microsoft.Azure.Cosmos.Table.TableOperation;
using TableResult = Microsoft.Azure.Cosmos.Table.TableResult;

namespace AzureStorageTable.SDK
{
    public class TableOperationHandler
    {
        private Setup setup;
        public TableOperationHandler(string table)
        {
            setup = new Setup(table);
        }
        public async Task<int> InsertOneAsync(string partitionKey, string rowKey, Dictionary<string, object> data)
        {
            var client = await setup.ClientConfigurer();

            var tableEntity = new Azure.Data.Tables.TableEntity(partitionKey, rowKey) {
                {
                    data.FirstOrDefault().Key,
                    data.FirstOrDefault().Value
                }
            };
            var result = client.AddEntity(tableEntity);
            return result.Status;
        }

        public async Task<IEnumerable<PersonModel>> GetAllDataAsync()
        {
            IList<PersonModel> personsList = new List<PersonModel>();
            var client = await setup.ClientConfigurer();
            var persons = client.QueryAsync<PersonModel>(filter: "", maxPerPage: 10);
            await foreach (var person in persons)
            {
                personsList.Add(person);
            }
            return personsList;
        }
    }
}
