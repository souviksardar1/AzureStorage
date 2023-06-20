using AzureBlobCommunication.Models;
using Blob.SDK;

public class Program
{
    static string woContainerName = "workorder-container";
    static string manufacturerContainerName = "manufacturer-container";
    static string azStorageConnectionString = "DefaultEndpoints=https;AccountName=souviksardar;AccountKey=xxxxxxgE39H5AtttttfB1YsyyyyyyqoLoaz7NppG+uuuuuuu=;EndpointSuffx=core.windows.net";
    public static async Task Main()
    {

        var workorderDatafromAzStorage = await ReadBlobHandler.ReadCsvDataFromBlob<Workorder>(azStorageConnectionString, woContainerName, "Workorder.csv");
        var manufacturerDatafromAzStorage = await ReadBlobHandler.ReadCsvDataFromBlob<Manufacturer>(azStorageConnectionString, manufacturerContainerName, "Manufacturer.csv");

        #region Append blob
        Console.WriteLine("Start : Append Blob operation is starting");


        if (workorderDatafromAzStorage == null || !workorderDatafromAzStorage.Any())
        {
            await FeedDummyDataUpdateBlob(typeof(Workorder));
        }
        else
        {
            await ReadOrUpdateWorkorderInAppendBlob();
        }
        Console.WriteLine("End : Append Blob operation is completed");
        #endregion End Append blob

        #region Block blog

        Console.WriteLine("Start : Blob operation is starting");


        if (manufacturerDatafromAzStorage == null || !manufacturerDatafromAzStorage.Any())
        {
            await FeedDummyDataUpdateBlob(typeof(Manufacturer));
        }
        else
        {
            await ReadOrUpdateWorkorderInBlob();
        }
        Console.WriteLine("End : Blob operation is completed");

        #endregion End Block blog

    }

    public async static Task FeedDummyDataUpdateBlob(Type t)
    {
        await Task.CompletedTask;

        var workorder = new List<Workorder>();
        var manufacturer = new List<Manufacturer>();
        if (t.Name == "Workorder")
        {
            for (int i = 1; i < 10; i++)
            {
                workorder.Add(new Workorder { Id = "Workorder" + i });
            }
            await UploadToAppendBlobHandler.UploadCsvDataToAppendBlob<Workorder>(azStorageConnectionString, woContainerName, "Workorder.csv", workorder);
        }

        if (t.Name == "Manufacturer")
        {
            for (int i = 1; i < 8; i++)
            {
                manufacturer.Add(new Manufacturer { Id = i, Name = "Manufacturer" + i });
            }
            await UploadToBlobHandler.UploadCsvDataToBlob<Manufacturer>(azStorageConnectionString, manufacturerContainerName, "Manufacturer.csv", manufacturer);
        }
    }
    public async static Task ReadOrUpdateWorkorderInAppendBlob()
    {
        var tasks = new List<Workorder>();

        for (int i = 0; i < 5; i++)
        {
            tasks.Add(new Workorder { Id = "Task" + i });
        }
        await UploadToAppendBlobHandler.UploadCsvDataToAppendBlob<Workorder>(azStorageConnectionString, woContainerName, "Workorder.csv", tasks);
    }

    public async static Task ReadOrUpdateWorkorderInBlob()
    {
        var manufacturer = new List<Manufacturer>();

        for (int i = 0; i < 3; i++)
        {
            manufacturer.Add(new Manufacturer { Id = i, Name = "Car - " + i });
        }
        await UploadToBlobHandler.UploadCsvDataToBlob<Manufacturer>(azStorageConnectionString, manufacturerContainerName, "Manufacturer.csv", manufacturer);
    }
}

