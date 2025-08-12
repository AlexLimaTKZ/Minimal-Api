# minimal-api

Este projeto é uma API mínima desenvolvida com ASP.NET Core, projetada para fornecer um backend leve e eficiente para aplicações web e móveis. Ele visa simplificar o desenvolvimento de APIs, focando na entrega rápida de funcionalidades essenciais e na facilidade de manutenção.

## Instalação e Configuração

Para configurar e executar este projeto localmente, siga os passos abaixo:

### Pré-requisitos

*   [.NET SDK](https://dotnet.microsoft.com/download) (versão 8.0 ou superior)
*   Um servidor MySQL em execução

### Passos

1.  **Clone o repositório:**
    ```bash
    git clone <URL_DO_SEU_REPOSITORIO>
    cd minimal-api
    ```
2.  **Restaure as dependências:**
    ```bash
    dotnet restore
    ```
3.  **Configure a conexão com o banco de dados:**
    Abra o arquivo `src/MinimalApi.Api/appsettings.json` (e `appsettings.Development.json` para ambiente de desenvolvimento) e atualize a string de conexão do MySQL:
    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Server=localhost;Port=3306;Database=your_database_name;Uid=your_username;Pwd=your_password;"
      },
      // ... outras configurações
    }
    ```
    Certifique-se de que o banco de dados `your_database_name` exista ou crie-o.

4.  **Aplique as migrações do banco de dados (Entity Framework Core):**
    Navegue até o diretório do projeto da API:
    ```bash
    cd src/MinimalApi.Api
    ```
    Execute as migrações:
    ```bash
    dotnet ef database update
    ```
    Se você precisar criar uma nova migração:
    ```bash
    dotnet ef migrations add NomeDaSuaMigracao
    ```
    (Certifique-se de ter a ferramenta `dotnet ef` instalada: `dotnet tool install --global dotnet-ef`)

5.  **Construa o projeto:**
    Navegue de volta para a raiz do projeto:
    ```bash
    cd ../..
    dotnet build
    ```

6.  **Execute a aplicação:**
    ```bash
    dotnet run --project src/MinimalApi.Api
    ```
    A API estará disponível em `https://localhost:<porta_gerada_automaticamente>`.

## Documentação da API

Após iniciar a aplicação, a documentação interativa da API (Swagger/OpenAPI) estará disponível no seguinte endereço:

`https://localhost:<porta_gerada_automaticamente>/swagger`

Você pode usar esta interface para explorar os endpoints disponíveis, testar requisições e entender a estrutura das respostas.

## Tecnologias Utilizadas

*   **ASP.NET Core Minimal API:** Framework para construção de APIs web leves e de alta performance.
*   **C#:** Linguagem de programação.
*   **.NET 8.0 / 9.0:** Plataforma de desenvolvimento.
*   **Entity Framework Core:** ORM (Object-Relational Mapper) para interação com o banco de dados.
*   **MySQL:** Sistema de gerenciamento de banco de dados relacional.

## Como Realizar Testes

Os testes unitários e de integração estão localizados no projeto `tests/MinimalApi.Tests`. Para executá-los:

1.  Navegue até a raiz do projeto.
2.  Execute o comando:
    ```bash
    dotnet test
    ```

## Como Contribuir

Agradecemos o seu interesse em contribuir! Para contribuir com este projeto, siga os passos abaixo:

1.  Faça um fork deste repositório.
2.  Crie uma nova branch para sua feature ou correção de bug (`git checkout -b feature/sua-feature` ou `bugfix/sua-correcao`).
3.  Faça suas alterações e certifique-se de que os testes passem.
4.  Faça commit de suas alterações (`git commit -m 'feat: Adiciona nova feature'`).
5.  Envie sua branch para o seu fork (`git push origin feature/sua-feature`).
6.  Abra um Pull Request para a branch `main` deste repositório.

## Links

Aqui estão alguns links úteis para criar um projeto como este:

-   [.NET SDK](https://dotnet.microsoft.com/download)
-   [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-8.0)
-   [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
-   [MySQL Official Website](https://www.mysql.com/)
-   [Pomelo.EntityFrameworkCore.MySql GitHub](https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql)
-   [JWT (JSON Web Tokens) Official Website](https://jwt.io/)

## Autor

Alex S. Lima
