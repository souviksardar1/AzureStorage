namespace Blob.SDK;
public class UploadToBlob
{
    public static async Task UploadCsvDataToBlob<T>(string storageAccConnectionString, string containerName, string fileName, IEnumerable<T> stats)
    {
        Console.WriteLine($"Uploating records to CSV file {fileName}");
        BlobServiceClient blobServiceClient = new BlobServiceClient(storageAccConnectionString);
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        containerClient.CreateIfNotExists();
        BlobClient blob = containerClient.GetBlobClient(fileName);
        using (var writer = new StringWriter())
        using (var csv = new CsvWriter(writer, CultureInfo.CurrentCulture))
        {
            csv.WriteRecords<T>(stats);
            string csvContent = writer.ToString();
            Stream streamToUploadToBlob = CommonOperation.GenerateStreamFromString(csvContent);
            streamToUploadToBlob.Position = 0;
            Stream finalStream = streamToUploadToBlob;
            await blob.UploadAsync(finalStream, true);
        }
        Console.WriteLine($"Records uploaded to CSV file {fileName}");
    }
}
