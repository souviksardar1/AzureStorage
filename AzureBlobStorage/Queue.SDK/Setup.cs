using Azure.Storage.Queues;

namespace Queue.SDK
{
    public class Setup
    {
        private readonly string _queueName;
        private readonly string connectionString = "DefaultEndpointProtcol=https;AccontName=toragebysardar;AccontKey=EO7EJeeeiBR+HcrNT7qqqZ1ycyyyyuySMGLuq4bHci4Ay+yyyy9hA=;EndpoiSuffix=core.window.net";

        public Setup(string queueName)
        {
            _queueName = queueName;

        }

        public async Task<QueueClient> ClientConfigurer()
        {
            var client = new QueueClient(connectionString, _queueName);

            if (!await client.ExistsAsync())
            {
                await client.CreateAsync();
                Console.WriteLine("New queue created successfully because it didn't exist.");
            }
            return client;
        }
    }
}