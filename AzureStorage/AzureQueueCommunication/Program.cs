using AzureQueueCommunication.Data;
using Queue.SDK;
using System.Text.Json;

public class Program
{
    static string queueName = "sensor-queue";
    public static async Task Main()
    {
        await SendMessageAsync();
        //await ReceiveMessagesAsync();
        await ReceiveAndDeleteMessagesAsync();
        await DeleteQueueAsync();
    }
    public static async Task SendMessageAsync()
    {
        try
        {
            ModelForQueue model = new() { Id = 5, Name = "Souvik Sardar", Location = "India" };
            var dataToSend = JsonSerializer.Serialize(model);
            QueueMessageOperationHandler ss = new QueueMessageOperationHandler(queueName);
            var sendResponse = await ss.SendAsync(dataToSend);
            var statusCode = sendResponse.GetRawResponse().Status;

            if (statusCode == 201)
            {
                Console.WriteLine($"Message has been sent sucessfully with id: {sendResponse.Value.MessageId}");
            }
            else
            {
                Console.WriteLine("There is an error while sending the message to storage queue");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Execption in SendMessage() {ex.StackTrace}");
        }

    }
    public static async Task ReceiveMessagesAsync()
    {
        try
        {
            QueueMessageOperationHandler ss = new QueueMessageOperationHandler(queueName);
            var rcvResponse = await ss.ReceiveAsync<Dictionary<string, string>>();
            if (rcvResponse.Item2 == 200)
            {
                foreach (var msg in rcvResponse.Item1)
                {
                    var deserializeResponse = JsonSerializer.Deserialize<ModelForQueue>(msg.Value);
                    Console.WriteLine($"Received data of messageId '{msg.Key}' ==> Id= {deserializeResponse.Id}, Name= {deserializeResponse.Name}, Location = {deserializeResponse.Location}");
                }
            }
            else
            {
                Console.WriteLine("Issue while receiving message from Queue");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Execption in ReceiveMessagesAsync() {ex.StackTrace}");
        }
    }
    public static async Task ReceiveAndDeleteMessagesAsync()
    {
        try
        {
            QueueMessageOperationHandler ss = new QueueMessageOperationHandler(queueName);
            var rcvResponse = await ss.ReceiveAndDeleteAsync<Dictionary<string, string>>();
            if (rcvResponse.Item2 == 200)
            {
                foreach (var msg in rcvResponse.Item1)
                {
                    var deserializeResponse = JsonSerializer.Deserialize<ModelForQueue>(msg.Value);
                    Console.WriteLine($"Received data of messageId '{msg.Key}' ==> Id= {deserializeResponse.Id}, Name= {deserializeResponse.Name}, Location = {deserializeResponse.Location}");
                }
            }
            else
            {
                Console.WriteLine("Issue while receiving message from Queue");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Execption in ReceiveAndDeleteMessagesAsync() {ex.StackTrace}");
        }
    }
    public static async Task DeleteQueueAsync()
    {
        try
        {
            QueueMessageOperationHandler ss = new QueueMessageOperationHandler(queueName);
            var deleteData = await ss.DeleteQueueAsync();
            if (deleteData)
            {
                Console.WriteLine($"'{queueName}' queue is deleted from Azure!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Execption in DeleteQueueAsync() {ex.StackTrace}");
        }

    }
}