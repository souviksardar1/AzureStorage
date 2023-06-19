using Azure;
using Azure.Storage.Queues.Models;

namespace Queue.SDK
{
    public class QueueMessageOperation
    {
        private Setup setup;
        public QueueMessageOperation(string q)
        {
            setup = new Setup(q);
        }
        public async Task<Response<SendReceipt>> SendAsync(string data)
        {
           var client = await setup.ClientConfigurer();
           var response =  await client.SendMessageAsync(data);
            return response;
        }

        public async Task<(T, int)> ReceiveAsync<T>()
        {
            Dictionary<string, string> receivedResponses = new Dictionary<string, string>();
            var client = await setup.ClientConfigurer();
            var rowResponse = await client.ReceiveMessagesAsync(maxMessages: 20);
            var messages = rowResponse.Value;

            foreach (var msg in messages)
            {
                receivedResponses.Add(msg.MessageId, msg.MessageText);
            }
            return ((T)Convert.ChangeType(receivedResponses, typeof(T)), rowResponse.GetRawResponse().Status);
        }

        public async Task<(T, int)> ReceiveAndDeleteAsync<T>()
        {
            Dictionary<string, string> receivedResponses = new Dictionary<string, string>();
            var client = await setup.ClientConfigurer();
            var rowResponse = await client.ReceiveMessagesAsync(maxMessages: 20);
            var messages = rowResponse.Value;

            for (int i = 0; i < messages.Count(); i++)
            {
                receivedResponses.Add(messages[i].MessageId, messages[i].MessageText);

                await client.DeleteMessageAsync(messages[i].MessageId, messages[i].PopReceipt);
                Console.WriteLine($"MessageId - '{messages[i].MessageId}' is deleted from queue storage");
            }
            return ((T)Convert.ChangeType(receivedResponses, typeof(T)), rowResponse.GetRawResponse().Status);
        }

        public async Task<Response<bool>> DeleteQueueAsync()
        {
            var client = await setup.ClientConfigurer();
            return await client.DeleteIfExistsAsync();
        }
    }
}
