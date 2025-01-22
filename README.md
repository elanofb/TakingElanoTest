# Projeto TakingElano

Este projeto é configurado para rodar e testar serviços, infraestrutura e testes automatizados para a empresa **Taking**, configurado por **Elano Barreto** para o teste técnico. O objetivo deste guia é fornecer instruções objetivas para configurar, rodar e testar o projeto.

---

## Swagger

Acesse a documentação Swagger através do link:
- **Swagger UI**: [http://localhost:5078/swagger/index.html](http://localhost:5078/swagger/index.html)

---

## Configuração Inicial

### Rodar o Projeto

Execute o seguinte comando para rodar o projeto:
```bash
dotnet run --project TakingElanoApi
```

---

## Estrutura do Projeto

### Criando a Estrutura
1. Criar bibliotecas e domínios do projeto:
    ```bash
    dotnet new classlib -n TakingElano.Domain
    ```
2. Adicionar uma nova classe de migração EF:
    ```bash
    dotnet ef migrations add InitialCreate --project TakingElano.Infrastructure --startup-project TakingElano.API
    ```
3. Listar as migrações:
    ```bash
    dotnet ef migrations list --project TakingElano.Infrastructure --startup-project TakingElano.API
    ```
4. Atualizar o banco de dados:
    ```bash
    dotnet ef database update --project TakingElano.Infrastructure --startup-project TakingElano.API
    ```

---

## Testes

### Projetos de Teste
1. Criar projetos de testes:
    ```bash
    dotnet new xunit --name TakingElano.UnitTests
    dotnet new xunit --name TakingElano.IntegrationTests
    ```
2. Adicionar os projetos de teste à solução:
    ```bash
    dotnet sln add TakingElano.UnitTests/TakingElano.UnitTests.csproj
    dotnet sln add TakingElano.IntegrationTests/TakingElano.IntegrationTests.csproj
    ```
3. Referenciar bibliotecas nos testes:
    - UnitTests:
        ```bash
        dotnet add TakingElano.UnitTests reference TakingElano.Application
        dotnet add TakingElano.UnitTests reference TakingElano.Domain
        ```
    - IntegrationTests:
        ```bash
        dotnet add TakingElano.IntegrationTests reference TakingElano.API
        dotnet add TakingElano.IntegrationTests reference TakingElano.Infrastructure
        ```

---

## RabbitMQ

Adicionar dependências para suporte ao RabbitMQ:
```bash
dotnet add TakingElano.Infrastructure package RabbitMQ.Client
```
Para uma versão específica:
```bash
dotnet add TakingElano.Infrastructure package RabbitMQ.Client --version 6.5.0
```
Outros pacotes necessários:
```bash
dotnet add TakingElano.Infrastructure package Microsoft.Extensions.Hosting
```
Segue uma evidência da criação das Queues no Rabbit
![image](https://github.com/user-attachments/assets/6f34c271-0516-4e56-b14c-6b03b3be5b96)

---

## Configuração de Componentes

### API

Adicionar dependências e referências para o projeto API:
```bash
dotnet add TakingElano.API reference TakingElano.Domain
```
```bash
dotnet add TakingElano.API reference TakingElano.Application
```
```bash
dotnet add TakingElano.API reference TakingElano.CrossCutting
```
```bash
dotnet add TakingElano.API reference TakingElano.Infra
```
Dependências:
```bash
dotnet add TakingElano.API package Microsoft.AspNetCore.Mvc
```
```bash
dotnet add package Microsoft.EntityFrameworkCore.Design
```

### Infraestrutura

Adicionar pacotes e referências para infraestrutura:
```bash
dotnet add TakingElano.Infrastructure reference TakingElano.Domain
```
```bash
dotnet add TakingElano.Infrastructure package Microsoft.EntityFrameworkCore
```
```bash
dotnet add TakingElano.Infrastructure package Microsoft.EntityFrameworkCore.SqlServer
```
Atualizar banco de dados:
```bash
dotnet ef database update --project TakingElano.Infrastructure --startup-project TakingElanoApi
```

### Application

Adicionar referência ao domínio:
```bash
dotnet add TakingElano.Application reference TakingElano.Domain
```

### CrossCutting

Adicionar referências e pacotes:
```bash
dotnet add TakingElano.CrossCutting reference TakingElano.Application
```
Pacotes Serilog:
```bash
dotnet add TakingElano.CrossCutting package Serilog
```
```bash
dotnet add TakingElano.CrossCutting package Serilog.Extensions.Logging
```
```bash
dotnet add TakingElano.CrossCutting package Serilog.Sinks.Console
```
```bash
dotnet add package Serilog.Sinks.File
```

---

### Testes Unitários

Adicionar pacotes úteis para testes unitários:
```bash
dotnet add TakingElano.UnitTests package FluentAssertions
```
```bash
dotnet add TakingElano.UnitTests package Bogus
```
```bash
dotnet add TakingElano.UnitTests package NSubstitute
```
Outros pacotes comuns:
```bash
dotnet add package Moq
```
```bash
dotnet add package xunit
```
```bash
dotnet add package xunit.runner.visualstudio
```

### Testes de Integração

Adicionar pacotes úteis para testes de integração:
```bash
dotnet add TakingElano.IntegrationTests package Testcontainers
```
```bash
dotnet add TakingElano.IntegrationTests package FluentAssertions
```
```bash
dotnet add TakingElano.IntegrationTests package Microsoft.AspNetCore.Mvc.Testing
```
```bash
dotnet add TakingElano.IntegrationTests package Microsoft.EntityFrameworkCore
```
```bash
dotnet add TakingElano.IntegrationTests package Microsoft.EntityFrameworkCore.InMemory
```

Testcontainers específicos:
```bash
dotnet add package Testcontainers.RabbitMq
```
```bash
dotnet add package Testcontainers.MsSql
```
```bash
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
```

---

## EF Core e Migrações

### Instalação do EF Core
```bash
dotnet tool install --global dotnet-ef
```
Verificar versão instalada:
```bash
dotnet ef --version
```
Adicionar pacote de design:
```bash
dotnet add package Microsoft.EntityFrameworkCore.Design
```

### Criar e Atualizar Migrações
Criar uma nova migração:
```bash
dotnet ef migrations add InitialCreate
```
Atualizar o banco de dados:
```bash
dotnet ef database update
```

---

## Docker
Para verificar filas no RabbitMQ:
```bash
docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:management
```
```bash
docker exec -it rabbitmq rabbitmqctl list_queues
```

