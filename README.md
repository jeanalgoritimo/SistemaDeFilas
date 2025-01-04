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

