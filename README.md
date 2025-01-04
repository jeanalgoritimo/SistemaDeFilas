Amazon SQS with .NET Core  
Este exemplo demonstra como integrar o Amazon SQS (Simple Queue Service) com uma aplicação em .NET Core. Ele inclui a criação de filas, envio de mensagens, e leitura de mensagens de uma fila SQS.

Pré-requisitos
Conta AWS: Certifique-se de que você tem uma conta AWS ativa.
Credenciais AWS: Você precisa das chaves de acesso da AWS (AWS:AccessKey e AWS:SecretKey). Estas devem ser configuradas no arquivo appsettings.json para autenticação.
.NET SDK: O projeto foi desenvolvido usando o .NET 5.0 ou superior.
Biblioteca AWS SDK: O SDK da AWS para .NET é necessário para se comunicar com o SQS.
Para instalar o SDK da AWS, execute:

bash
Copiar código
dotnet add package AWSSDK.SQS
Arquivo appsettings.json
Crie o arquivo appsettings.json na raiz do seu projeto e adicione suas credenciais da AWS:

json
Copiar código
{
  "AWS": {
    "AccessKey": "SUA_ACESSO_KEY",
    "SecretKey": "SUA_SECRET_KEY"
  }
}
Estrutura do Projeto
Este exemplo usa Amazon SQS para enviar e receber mensagens. Aqui está um resumo do código:

Criação e Obtenção de Filas SQS: A fila é criada se ela não existir, ou o código obtém a URL de uma fila existente.

Envio de Mensagens: O código envia uma mensagem serializada (JSON) contendo informações de uma pessoa.

Recebimento e Processamento de Mensagens: A aplicação recebe até 5 mensagens por vez, processa e exclui as mensagens após o processamento.

Código Principal
Definição de Classe Person
csharp
Copiar código
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
}
A classe Person é usada para criar um objeto que será convertido em JSON e enviado para o SQS.

Função Principal Main
csharp
Copiar código
static async Task Main(string[] args)
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

    Configuration = builder.Build();
    var awsAccessKeyId = Configuration["AWS:AccessKey"];
    var awsSecretAccessKey = Configuration["AWS:SecretKey"];
    var region = RegionEndpoint.GetBySystemName("us-east-1");
    var sqsClient = new AmazonSQSClient(RegionEndpoint.USEast1);

    // Criar ou obter a URL da fila
    var queueUrl = await GetOrCreateQueueAsync(sqsClient, QueueName);

    var person = new Person
    {
        Name = "John Doe",
        Age = 30,
        Email = "john.doe@example.com"
    };

    string jsonString = JsonSerializer.Serialize(person);

    // Enviar uma mensagem para a fila
    await SendMessageAsync(sqsClient, queueUrl, jsonString);

    // Receber mensagens da fila
    while (true)
    {
        queueUrl = await GetOrCreateQueueAsync(sqsClient, QueueName1);
        await ReceiveMessagesAsync(sqsClient, queueUrl);

        Thread.Sleep(1000);
        if (receiveMessage)
            break;
    }

    Console.WriteLine("Loop terminado da segunda fila.");
}
Funções Auxiliares
Função para Criar ou Obter a Fila
csharp
Copiar código
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
Função para Enviar Mensagens
csharp
Copiar código
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
Função para Receber Mensagens
csharp
Copiar código
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
Como Executar
Configure suas credenciais AWS no arquivo appsettings.json conforme mencionado.

Compile o projeto usando o comando:

bash
Copiar código
dotnet build
Execute o projeto com o comando:

bash
Copiar código
dotnet run
O código enviará uma mensagem de um objeto Person para a fila SQS e aguardará mensagens da segunda fila, processando-as e deletando-as após o processamento.


