Amazon SQS .NET Core Example
This project demonstrates how to interact with Amazon Simple Queue Service (SQS) using AWS SDK for .NET Core. It shows how to send and receive messages between two SQS queues. The program also uses a custom Person class for serializing and sending data as JSON.

Requirements
.NET SDK (Core or later)
AWS SDK for .NET
AWS Account with access to SQS
appsettings.json for AWS credentials
Setup
Prerequisites
Install .NET SDK: https://dotnet.microsoft.com/download
Install AWS SDK for .NET:
bash
Copiar código
dotnet add package AWSSDK.SQS
dotnet add package Microsoft.Extensions.Configuration
dotnet add package Microsoft.Extensions.Configuration.Json
AWS Credentials
Before running the program, ensure your AWS credentials are configured in appsettings.json or as environment variables.

Create a file named appsettings.json in the root directory and add your AWS credentials:

json
Copiar código
{
  "AWS": {
    "AccessKey": "your-access-key",
    "SecretKey": "your-secret-key"
  }
}
Running the Program
Open your terminal and navigate to the project directory.
Run the application:
bash
Copiar código
dotnet run
Code Overview
This application performs the following steps:

Configuration Setup:

Reads AWS access credentials and other configurations from appsettings.json using Microsoft.Extensions.Configuration.
Queue Management:

The program checks if the specified SQS queue exists, or creates a new one if it doesn't. Two queues are used in this example: FilasAwsDotnetCore1 and FilasAwsDotnetCore.
Sending Messages:

It sends a serialized Person object as a JSON string to the first queue.
Receiving Messages:

The program enters a loop, repeatedly checking the second queue for new messages, processing them, and deleting them after successful processing.
Custom Class:

The Person class contains properties Name, Age, and Email. This class is serialized to JSON and sent as the message body in the queue.
Code Explanation
AmazonSQSClient: Interacts with the SQS service to send and receive messages.
SendMessageAsync: Sends a message to an SQS queue.
ReceiveMessagesAsync: Receives messages from a queue, processes them, and deletes them after processing.
GetOrCreateQueueAsync: Checks if the queue exists, creates it if necessary, and returns the queue's URL.
Example Output
bash
Copiar código
Verificando ou criando a fila: FilasAwsDotnetCore1
Enviando mensagem: {"Name":"John Doe","Age":30,"Email":"john.doe@example.com"}
Mensagem enviada com sucesso!
Verificando ou criando a fila: FilasAwsDotnetCore
Aguardando mensagens...
Mensagem recebida da segunda fila: {"Name":"John Doe","Age":30,"Email":"john.doe@example.com"}
Mensagem deletada.
Loop terminado da segunda fila.
Additional Configuration
AWS Region: The AWS region is set to us-east-1 in the code, but you can change it according to your AWS SQS region.
Message Visibility: This code uses a long polling technique (WaitTimeSeconds = 5) to reduce unnecessary requests to SQS.
Important Notes
Ensure that the AWS credentials you use have the necessary permissions to interact with SQS.
You may modify the message body and queue names based on your application needs.
