namespace Blob.SDK;
public class UploadToAppendBlob
{
    public static async Task UploadCsvDataToAppendBlob<T>(string storageAccConnectionString, string containerName, string fileName, IEnumerable<T> stats)
    {
        Console.WriteLine($"Uploating records to CSV file {fileName}");
        BlobServiceClient blobServiceClient = new BlobServiceClient(storageAccConnectionString);
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        containerClient.CreateIfNotExists();
        Stream streamToUploadToBlob;
        AppendBlobClient appendBlobClient = containerClient.GetAppendBlobClient(fileName);
        appendBlobClient.CreateIfNotExists();
        var blockSize = appendBlobClient.GetProperties().Value.ContentLength;
        using (var writer = new StringWriter())
        using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.CurrentCulture) { HasHeaderRecord = blockSize > 0 ? false : true }))
        {
            csv.WriteRecords<T>(stats);
            string csvContent = writer.ToString();
            streamToUploadToBlob = CommonOperation.GenerateStreamFromString(csvContent);
            var maxBlockSize = appendBlobClient.AppendBlobMaxAppendBlockBytes;

            var buffer = new byte[maxBlockSize];

            if (streamToUploadToBlob.Length <= maxBlockSize)
            {
                appendBlobClient.AppendBlock(streamToUploadToBlob);
            }
            else
            {
                var bytesLeft = (streamToUploadToBlob.Length - streamToUploadToBlob.Position);

                while (bytesLeft > 0)
                {
                    if (bytesLeft >= maxBlockSize)
                    {
                        buffer = new byte[maxBlockSize];
                        await streamToUploadToBlob.ReadAsync
                            (buffer, 0, maxBlockSize);
                    }
                    else
                    {
                        buffer = new byte[bytesLeft];
                        await streamToUploadToBlob.ReadAsync
                            (buffer, 0, Convert.ToInt32(bytesLeft));
                    }

                    appendBlobClient.AppendBlock(new MemoryStream(buffer));

                    bytesLeft = (streamToUploadToBlob.Length - streamToUploadToBlob.Position);

                }

            }
        }
        Console.WriteLine($"Records uploaded to CSV file {fileName}");
    }
}
