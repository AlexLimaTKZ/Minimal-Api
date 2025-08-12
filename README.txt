# Refatoração e Melhorias em uma API Minimalista .NET

Este `README.md` detalha as modificações e melhorias implementadas em um projeto de API minimalista .NET, transformando-o em uma aplicação mais robusta, organizada e testável.

## 1. Configuração do Modelo de Dados para Veículos

Inicialmente, o projeto não possuía um modelo de dados para veículos. Foi criado um novo modelo (`Veiculo`) e integrado ao contexto do banco de dados.

### O que foi feito:

- **Criação da Entidade `Veiculo`**: Um novo arquivo `Veiculo.cs` foi adicionado em `Dominio/Entidades` (posteriormente movido para `src/MinimalApi.Domain/Entidades`). Esta entidade define as propriedades de um veículo, como `Marca`, `Modelo`, `Ano` e `Cor`, utilizando `Data Annotations` para validação básica e mapeamento para o banco de dados.

    ```csharp
    // Exemplo de Veiculo.cs
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace MinimalApi.Domain.Entidades
    {
        public class Veiculo
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }

            [Required]
            [StringLength(100)]
            public string Marca { get; set; } = default!;

            // ... outras propriedades
        }
    }
    ```

- **Atualização do `DbContexto`**: O arquivo `Infraestrutura/DB/DbContexto.cs` (posteriormente movido para `src/MinimalApi.Infrastructure/DB`) foi modificado para incluir um `DbSet<Veiculo>`, permitindo que o Entity Framework Core gerencie a tabela de veículos no banco de dados.

    ```csharp
    // Exemplo de DbContexto.cs
    using Microsoft.EntityFrameworkCore;
    using MinimalApi.Domain.Entidades;

    namespace MinimalApi.Infrastructure.DB
    {
        public class DbContexto : DbContext
        {
            public DbContexto(DbContextOptions<DbContexto> options) : base(options) { }

            public DbSet<Administrador> Administradores { get; set; } = default!;
            public DbSet<Veiculo> Veiculos { get; set; } = default!;

            // ... OnModelCreating
        }
    }
    ```

### Como foi feito (Comandos):

Após a criação da entidade e a atualização do `DbContexto`, foram utilizados os seguintes comandos do .NET CLI para gerar e aplicar as migrações do banco de dados:

```bash

# Adiciona uma nova migração (ex: VeiculoMigration)
dotnet ef migrations add VeiculoMigration

# Aplica as migrações ao banco de dados
dotnet ef database update
```

## 2. Configuração da Documentação da API com Swagger

Para facilitar o consumo e a compreensão da API, o Swagger (OpenAPI) foi integrado ao projeto, fornecendo uma interface interativa para testar os endpoints.

### O que foi feito:

- **Instalação do Pacote**: O pacote NuGet `Swashbuckle.AspNetCore` foi adicionado ao projeto.
- **Configuração em `Program.cs`**: As seguintes linhas foram adicionadas ao `Program.cs` (posteriormente movido para `src/MinimalApi.Api/Program.cs`):
    -   `builder.Services.AddEndpointsApiExplorer();` e `builder.Services.AddSwaggerGen();` para registrar os serviços do Swagger.
    -   `app.UseSwagger();` e `app.UseSwaggerUI();` para habilitar o middleware do Swagger na pipeline de requisições HTTP, especialmente em ambiente de desenvolvimento.

### Como foi feito:

```csharp
// Em Program.cs
var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ... outros serviços

var app = builder.Build();

// Configura o pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// ... outros middlewares
```

Após a execução da aplicação, a documentação interativa do Swagger pode ser acessada em `http://localhost:<porta>/swagger`.

## 3. Criação de Endpoints CRUD para Veículos

Foram implementados endpoints para as operações básicas de CRUD (Create, Read, Update, Delete) para a entidade `Veiculo`.

### O que foi feito:

- **Endpoints HTTP**: Foram adicionados métodos `MapPost`, `MapGet`, `MapPut` e `MapDelete` no `Program.cs` para lidar com as requisições HTTP correspondentes:
    -   `POST /veiculos`: Cria um novo veículo.
    -   `GET /veiculos`: Lista todos os veículos.
    -   `GET /veiculos/{id}`: Busca um veículo específico por ID.
    -   `PUT /veiculos/{id}`: Atualiza um veículo existente.
    -   `DELETE /veiculos/{id}`: Remove um veículo.

- **Injeção de Dependência**: O `DbContexto` foi injetado nos handlers dos endpoints para interagir com o banco de dados.

### Como foi feito (Exemplo POST):

```csharp
// Em Program.cs
app.MapPost("/veiculos", async (Veiculo veiculo, DbContexto db) =>
{
    // ... validação (ver próxima seção)
    db.Veiculos.Add(veiculo);
    await db.SaveChangesAsync();
    return Results.Created($"/veiculos/{veiculo.Id}", veiculo);
});
```

## 4. Implementação de Validação de Dados

Para garantir a integridade dos dados, a validação foi adicionada aos endpoints de criação e atualização de veículos.

### O que foi feito:

- **Validação com `Data Annotations`**: As propriedades da entidade `Veiculo` já possuíam `Data Annotations` (`[Required]`, `[StringLength]`).
- **Verificação Manual**: Nos handlers dos endpoints `POST /veiculos` e `PUT /veiculos/{id}`, foi adicionada uma verificação manual usando `System.ComponentModel.DataAnnotations.Validator.TryValidateObject`.
- **Retorno de Erros**: Em caso de falha na validação, um `Results.ValidationProblem` é retornado, fornecendo detalhes sobre os campos inválidos.

### Como foi feito (Exemplo POST com validação):

```csharp
// Em Program.cs
using System.ComponentModel.DataAnnotations; // Adicionado

app.MapPost("/veiculos", async (Veiculo veiculo, DbContexto db) =>
{
    var validationContext = new ValidationContext(veiculo, serviceProvider: null, items: null);
    var validationResults = new List<ValidationResult>();

    if (!Validator.TryValidateObject(veiculo, validationContext, validationResults, validateAllProperties: true))
    {
        return Results.ValidationProblem(validationResults.ToDictionary(vr => vr.MemberNames.First(), vr => new string[] { vr.ErrorMessage ?? "Invalid" }));
    }

    db.Veiculos.Add(veiculo);
    await db.SaveChangesAsync();
    return Results.Created($"/veiculos/{veiculo.Id}", veiculo);
});
```

## 5. Autenticação e Autorização com JWT

Um sistema de autenticação e autorização baseado em JSON Web Tokens (JWT) foi implementado para proteger os endpoints da API e controlar o acesso com base em perfis de usuário.

### O que foi feito:

- **Instalação do Pacote**: O pacote NuGet `Microsoft.AspNetCore.Authentication.JwtBearer` foi adicionado.
- **Configuração dos Serviços JWT**: No `Program.cs`, os serviços de autenticação JWT foram configurados, incluindo a validação do emissor, audiência, tempo de vida e chave de assinatura do token.
- **Configuração em `appsettings.json`**: Uma nova seção `Jwt` foi adicionada ao `appsettings.json` para armazenar a chave secreta, o emissor e a audiência do JWT.

    ```json
    // Exemplo em appsettings.json
    "Jwt": {
      "Key": "SuaChaveSecretaAquiQueDeveSerLongaESegura",
      "Issuer": "seuservidor.com",
      "Audience": "suaaplicacao.com"
    }
    ```

- **Middleware de Autenticação/Autorização**: `app.UseAuthentication();` e `app.UseAuthorization();` foram adicionados ao pipeline de requisições HTTP no `Program.cs`.
- **Endpoint de Login**: O endpoint `POST /login` foi modificado para:
    -   Autenticar o usuário (verificando e-mail e senha no banco de dados).
    -   Gerar um JWT contendo `Claims` (como ID do usuário, e-mail e `Perfil` - Admin/Editor).
    -   Retornar o JWT para o cliente.

    **⚠️ NOTA DE SEGURANÇA CRÍTICA**: A implementação atual verifica senhas em texto simples. Em um ambiente de produção, **É IMPERATIVO** que as senhas sejam armazenadas e verificadas usando técnicas de hashing e salting (ex: `BCrypt.Net` ou `ASP.NET Core Identity`).

- **Políticas de Autorização**: Foram definidas políticas de autorização (`AdminPolicy` e `EditorPolicy`) baseadas em roles (`Admin` e `Editor`).
- **Proteção de Endpoints**: Os endpoints de veículos foram protegidos usando `.RequireAuthorization()`:
    -   `POST /veiculos`, `PUT /veiculos/{id}`, `DELETE /veiculos/{id}`: Requerem `AdminPolicy`.
    -   `GET /veiculos`, `GET /veiculos/{id}`: Requerem `EditorPolicy` (que inclui `Admin`).

## 6. Refatoração do Projeto para Arquitetura em Camadas

O projeto foi refatorado para seguir uma arquitetura em camadas (Domain, Application, Infrastructure, API), promovendo a separação de responsabilidades, manutenibilidade e testabilidade.

### O que foi feito:

- **Estrutura de Pastas e Projetos**: A solução foi organizada em novas pastas (`src`, `tests`) e projetos (`.csproj`):
    -   `src/MinimalApi.Api`: Contém o ponto de entrada da aplicação (endpoints, `Program.cs`).
    -   `src/MinimalApi.Domain`: Contém as entidades de domínio (`Administrador`, `Veiculo`), DTOs (`LoginDTO`) e interfaces de domínio (`IAdministradorServico`).
    -   `src/MinimalApi.Application`: Contém a lógica de negócio e serviços de aplicação (`AdministradorServico`).
    -   `src/MinimalApi.Infrastructure`: Contém a lógica de acesso a dados (`DbContexto`, Migrações).
    -   `tests/MinimalApi.Tests`: Contém todos os testes automatizados.

- **Movimentação de Arquivos e Atualização de Namespaces**: Todos os arquivos existentes foram movidos para seus respectivos novos projetos, e seus `namespaces` foram atualizados para refletir a nova estrutura (ex: `MinimalApi.Dominio.Entidades` para `MinimalApi.Domain.Entidades`).

- **Referências de Projeto**: As dependências entre os projetos foram estabelecidas (ex: `MinimalApi.Api` referencia `MinimalApi.Application` e `MinimalApi.Infrastructure`).

- **Injeção de Dependência Atualizada**: O `Program.cs` em `MinimalApi.Api` foi atualizado para registrar e injetar corretamente os serviços e contextos de banco de dados das novas camadas.

### Como foi feito (Comandos):

```bash

# Criação de diretórios
mkdir src tests

# Criação e adição de projetos à solução
dotnet new web -n MinimalApi.Api -o src/MinimalApi.Api && dotnet sln add src/MinimalApi.Api/MinimalApi.Api.csproj
dotnet new classlib -n MinimalApi.Domain -o src/MinimalApi.Domain && dotnet sln add src/MinimalApi.Domain/MinimalApi.Domain.csproj
dotnet new classlib -n MinimalApi.Application -o src/MinimalApi.Application && dotnet sln add src/MinimalApi.Application/MinimalApi.Application.csproj
dotnet new classlib -n MinimalApi.Infrastructure -o src/MinimalApi.Infrastructure && dotnet sln add src/MinimalApi.Infrastructure/MinimalApi.Infrastructure.csproj
dotnet new xunit -n MinimalApi.Tests -o tests/MinimalApi.Tests && dotnet sln add tests/MinimalApi.Tests/MinimalApi.Tests.csproj

# Exemplo de adição de referências (outras foram feitas de forma similar)
dotnet add src/MinimalApi.Api/MinimalApi.Api.csproj reference src/MinimalApi.Application/MinimalApi.Application.csproj

# Movimentação de arquivos (exemplo para API, outros foram feitos de forma similar)
del src\MinimalApi.Api\Program.cs src\MinimalApi.Api\appsettings.json src\MinimalApi.Api\appsettings.Development.json
move Program.cs src\MinimalApi.Api\ && move appsettings.json src\MinimalApi.Api\ && move appsettings.Development.json src\MinimalApi.Api\ && move Properties\launchSettings.json src\MinimalApi.Api\Properties\
del Properties\.gitignore && rmdir Properties

# Atualização de namespaces (exemplo para Administrador.cs, outros foram feitos de forma similar)
# Substituição de 'namespace MinimalApi.Dominio.Entidades' por 'namespace MinimalApi.Domain.Entidades'
```

## 7. Criação de Testes Automatizados

Foram adicionados testes automatizados para garantir a correção e a robustez da aplicação, cobrindo diferentes camadas.

### O que foi feito:

- **Testes de Unidade para Modelos**: Criado `tests/MinimalApi.Tests/Domain/AdministradorTests.cs` para verificar o comportamento básico da entidade `Administrador`.

- **Testes de Persistência**: Criado `tests/MinimalApi.Tests/Infrastructure/DbContextoTests.cs` para testar as operações de CRUD com o `DbContexto` usando um banco de dados em memória (`Microsoft.EntityFrameworkCore.InMemory`), garantindo que a interação com o ORM funcione como esperado.

- **Testes de Requisição (Integração de API)**: Criados `tests/MinimalApi.Tests/Api/AuthEndpointsTests.cs` e `tests/MinimalApi.Tests/Api/VeiculoEndpointsTests.cs`.
    -   Utilizam `Microsoft.AspNetCore.Mvc.Testing` para simular um servidor HTTP real em memória, permitindo testar os endpoints da API de forma completa.
    -   Incluem testes para o endpoint de login (geração de JWT) e para as operações CRUD de veículos, verificando autenticação, autorização e validação de dados.

### Como foi feito (Comandos):

```bash

# Adição de pacotes NuGet para testes (ex: Microsoft.EntityFrameworkCore.InMemory, Microsoft.AspNetCore.Mvc.Testing)
dotnet add tests/MinimalApi.Tests/MinimalApi.Tests.csproj package Microsoft.EntityFrameworkCore.InMemory
dotnet add tests/MinimalApi.Tests/MinimalApi.Tests.csproj package Microsoft.AspNetCore.Mvc.Testing

# Criação de diretórios para organização dos testes
mkdir tests\MinimalApi.Tests\Domain tests\MinimalApi.Tests\Infrastructure tests\MinimalApi.Tests\Api

# Criação dos arquivos de teste (conteúdo detalhado acima)
# write_file tests/MinimalApi.Tests/Domain/AdministradorTests.cs
# write_file tests/MinimalApi.Tests/Infrastructure/DbContextoTests.cs
# write_file tests/MinimalApi.Tests/Api/AuthEndpointsTests.cs
# write_file tests/MinimalApi.Tests/Api/VeiculoEndpointsTests.cs
```

### Como Executar os Testes:

Para executar todos os testes automatizados, abra um terminal na raiz do projeto e execute:

```bash

dotnet test tests/MinimalApi.Tests/MinimalApi.Tests.csproj
```

## Conclusão

Este projeto evoluiu de uma API minimalista simples para uma aplicação com uma arquitetura mais robusta, com separação de responsabilidades, documentação interativa, segurança aprimorada e uma suíte de testes abrangente. As mudanças visam facilitar a manutenção, o desenvolvimento de novas funcionalidades e a garantia da qualidade do código.
