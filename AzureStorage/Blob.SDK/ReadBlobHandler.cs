namespace Blob.SDK;
public static class ReadBlobHandler
{
    public async static Task<List<T>> ReadCsvDataFromBlob<T>(string storageAccConnectionString, string containerName, string fileName)
    {
        BlobServiceClient blobServiceClient = new BlobServiceClient(storageAccConnectionString);
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        BlobClient blob = containerClient.GetBlobClient(fileName);

        List<T> data;
        if (blob.Exists())
        {
            Console.WriteLine($"Reading records in CSV file {fileName}");

            using (var memoryStream = new MemoryStream())
            {
                await blob.DownloadToAsync(memoryStream);
                memoryStream.Position = 0;
                using (var reader = new StreamReader(memoryStream))
                {
                    using (var csv = new CsvReader(reader, CultureInfo.CurrentCulture))
                    {
                        data = csv.GetRecords<T>().ToList();
                    }
                }
            }
            Console.WriteLine($"Read all records in CSV file {fileName}");
            return data;
        }
        Console.WriteLine($"Requested CSV file {fileName} is not exist");
        return null;
    }
}