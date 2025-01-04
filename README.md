Amazon SQS with .NET Core Example
This project demonstrates how to interact with Amazon SQS using AWS SDK for .NET. It shows how to send, receive, and delete messages in SQS queues.

Prerequisites
Before running the project, ensure the following:

AWS Account: You need an AWS account with access to Amazon SQS.
AWS SDK for .NET: This project uses the Amazon.SQS NuGet package. You can install it via the following command:
bash
Copiar código
dotnet add package AWSSDK.SQS
AppSettings Configuration: The appsettings.json file must contain your AWS credentials, as shown below:
appsettings.json Example:
json
Copiar código
{
  "AWS": {
    "AccessKey": "your-access-key-id",
    "SecretKey": "your-secret-access-key"
  }
}
Setup
Install Dependencies: In your project folder, run:

bash
Copiar código
dotnet restore
Configure AWS Credentials: Add your AWS:AccessKey and AWS:SecretKey to the appsettings.json file. Make sure to replace "your-access-key-id" and "your-secret-access-key" with your actual AWS credentials.

Run the Program: You can now run the application using the following command:

bash
Copiar código
dotnet run
Code Overview
Main Workflow
Configuration: The program loads AWS credentials and configuration from the appsettings.json file using Microsoft.Extensions.Configuration.

Queue Creation: The program checks if the queues (FilasAwsDotnetCore1 and FilasAwsDotnetCore) exist, and if not, it creates them.

Sending Messages: A Person object is created and serialized into a JSON string, then sent to the queue using SendMessageAsync.

Receiving Messages: The program listens for incoming messages in the queue and processes them when they arrive. After processing a message, it is deleted from the queue.

Classes and Methods
Person Class: A simple class with Name, Age, and Email properties, used to send serialized data in the queue.

GetOrCreateQueueAsync: Ensures that the SQS queue exists, or creates it if not.

SendMessageAsync: Sends a message (serialized object) to the specified queue.

ReceiveMessagesAsync: Polls the queue for incoming messages and processes them.

Sample Output
css
Copiar código
Verificando ou criando a fila: FilasAwsDotnetCore1
Enviando mensagem: {"Name":"John Doe","Age":30,"Email":"john.doe@example.com"}
Mensagem enviada com sucesso!
Aguardando mensagens...
Mensagem recebida da segunda fila: {"Name":"John Doe","Age":30,"Email":"john.doe@example.com"}
Mensagem deletada.
Loop terminado da segunda fila.
Notes
The program runs in a loop, continuously checking and receiving messages from FilasAwsDotnetCore.
The program stops once a message is successfully received and deleted.
If you want to modify the behavior or add more functionality (such as sending messages to multiple queues), feel free to adjust the code accordingly.
