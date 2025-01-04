using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
class Program
{
    private const string QueueName = "FilasAwsDotnetCore1";
    private const string QueueName1 = "FilasAwsDotnetCore";
    private static bool receiveMessage = false;
    public static IConfiguration Configuration { get; set; }
	public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
    }
    
    static async Task Main(string[] args)
    {
		var builder = new ConfigurationBuilder()
		   .SetBasePath(Directory.GetCurrentDirectory())  // This sets the base path to the current directory
		   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

		// Build the configuration
		Configuration = builder.Build();
		var awsAccessKeyId =  Configuration["AWS:AccessKey"];
        var awsSecretAccessKey =  Configuration["AWS:SecretKey"];
        var region = RegionEndpoint.GetBySystemName("us-east-1");
        var sqsClient = new AmazonSQSClient(RegionEndpoint.USEast1);
      

        //// Criar ou obter a URL da fila
        var queueUrl = await GetOrCreateQueueAsync(sqsClient, QueueName);

         
        var person = new Person
        {
            Name = "John Doe",
            Age = 30,
            Email = "john.doe@example.com"
        };

        string jsonString = JsonSerializer.Serialize(person);
         
        //// Enviar uma mensagem para a fila
        await SendMessageAsync(sqsClient, queueUrl, jsonString);

        while (true)
        {
            //// Criar ou obter a URL da fila
             queueUrl = await GetOrCreateQueueAsync(sqsClient, QueueName1);

            //// Enviar uma mensagem para a fila
            //await SendMessageAsync(sqsClient, queueUrl, "Minha primeira mensagem no SQS !");

            // Receber mensagens da fila
            await ReceiveMessagesAsync(sqsClient, queueUrl);

            Thread.Sleep(1000);
            if (receiveMessage)
                break;
        }
        Console.WriteLine("Loop terminado da segunda fila.");

        // Receber mensagens da fila
        //await ReceiveMessagesAsync(sqsClient, queueUrl);
    }

    private static async Task<string> GetOrCreateQueueAsync(IAmazonSQS sqsClient, string queueName)
    {
        Console.WriteLine($"Verificando ou criando a fila: {queueName}");
        var createQueueRequest = new CreateQueueRequest
        {
            QueueName = queueName
        };
        var createQueueResponse = await sqsClient.CreateQueueAsync(createQueueRequest);
        return createQueueResponse.QueueUrl;
    }

    private static async Task SendMessageAsync(IAmazonSQS sqsClient, string queueUrl, string message)
    {
        Console.WriteLine($"Enviando mensagem: {message}");
        var sendMessageRequest = new SendMessageRequest
        {
            QueueUrl = queueUrl,
            MessageBody = message
        };
        await sqsClient.SendMessageAsync(sendMessageRequest);
        Console.WriteLine("Mensagem enviada com sucesso!");
    }
    private static async Task ReceiveMessagesAsync(IAmazonSQS sqsClient, string queueUrl)
    {
        Console.WriteLine("Aguardando mensagens...");
        var receiveMessageRequest = new ReceiveMessageRequest
        {
            QueueUrl = queueUrl,
            MaxNumberOfMessages = 5,
            WaitTimeSeconds = 5
        };
        var receiveMessageResponse = await sqsClient.ReceiveMessageAsync(receiveMessageRequest);

        foreach (var message in receiveMessageResponse.Messages)
        {
            Console.WriteLine($"Mensagem recebida da segunda fila: {message.Body}");

             
            // Processar mensagem...

            // Deletar mensagem da fila após o processamento
            await sqsClient.DeleteMessageAsync(queueUrl, message.ReceiptHandle);
            Console.WriteLine("Mensagem deletada.");
            receiveMessage = true; 
        }
    }
	 
}

