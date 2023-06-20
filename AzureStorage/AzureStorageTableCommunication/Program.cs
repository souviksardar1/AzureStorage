using AzureStorageTable.SDK;
using AzureStorageTable.SDK.FwkModels;

public class Program
{
    static string tableName = "tablebysouvik1";
    public static async Task Main()
    {
        await InsertData();
        await GetAllData();
    }

    public static async Task InsertData()
    {
        try
        {
            TableOperationHandler ss = new TableOperationHandler(tableName);
            Dictionary<string, object> dicData = new();
            dicData["Name"] = "Souvik Sardar";

            var output = await ss.InsertOneAsync("Name", Guid.NewGuid().ToString(), dicData);
            if (output == 204) // success but no content
            {
                Console.WriteLine($"Data is inserted into Azure storage table");
            }
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Execption in InsertDataAsync() {ex.StackTrace}");
        }

    }


    public static async Task GetAllData()
    {
        try
        {
            TableOperationHandler handler = new TableOperationHandler(tableName);
            var data = await handler.GetAllDataAsync();
            foreach (var d in data)
            {
                Console.WriteLine($"Id : {d.Id}, Name= {d.Name}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Execption in GetAllData() {ex.StackTrace}");
        }

    }
}