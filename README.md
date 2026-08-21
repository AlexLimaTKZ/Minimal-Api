# 🚀 Minimal API com C# — do zero ao projeto completo

Este repositório é uma **trilha prática para aprender ASP.NET Core Minimal API com C#**, construída para quem quer entender o que está fazendo — e não apenas copiar código.

A abordagem usa a técnica de **Feynman**: primeiro explicamos a ideia em linguagem simples, depois criamos um modelo mental, só então escrevemos o código e, no fim, você tenta explicar com suas próprias palavras.

> Se você nunca criou uma API, comece pela **[Aula 00 — Comece aqui](aulas/00-comece-aqui.md)**.

## 🎯 O que você vai construir

Ao longo da trilha, uma API de veículos evolui por etapas:

```text
Olá Mundo
   ↓
Endpoints HTTP
   ↓
CRUD em memória
   ↓
DTO + validação
   ↓
Injeção de dependência
   ↓
Entity Framework Core
   ↓
MySQL + migrations
   ↓
Swagger/OpenAPI
   ↓
JWT + roles
   ↓
Arquitetura em camadas
   ↓
Testes automatizados
```

No final, você entenderá como o projeto completo em `src/` funciona e por que ele foi organizado dessa forma.

## 🧠 Como estudar com Feynman

Em cada aula, siga este ciclo:

```text
1. Entenda o problema
        ↓
2. Explique de forma simples
        ↓
3. Veja o modelo mental
        ↓
4. Escreva o código
        ↓
5. Explique o código sem olhar
        ↓
6. Faça o desafio
```

Se você não consegue explicar uma parte sem usar palavras complicadas, volte nela. Essa dificuldade mostra exatamente onde ainda existe uma lacuna.

## 🗺️ Trilha completa

| # | Aula | O que você aprende |
|---|---|---|
| 00 | [Comece aqui](aulas/00-comece-aqui.md) | ambiente, pré-requisitos e como estudar |
| 01 | [O que é uma API?](aulas/01-o-que-e-uma-api.md) | cliente, servidor, API e request/response |
| 02 | [Primeira Minimal API](aulas/02-primeira-minimal-api.md) | `WebApplication`, `MapGet` e `Run` |
| 03 | [HTTP sem complicação](aulas/03-http-sem-complicacao.md) | métodos, status codes e JSON |
| 04 | [Endpoints](aulas/04-entendendo-endpoints.md) | rota, método e handler |
| 05 | [Parâmetros e dados de entrada](aulas/05-parametros-e-dados-de-entrada.md) | route, query e body |
| 06 | [CRUD em memória](aulas/06-crud-em-memoria.md) | GET, POST, PUT e DELETE sem banco |
| 07 | [DTOs e validação](aulas/07-dtos-e-validacao.md) | contratos de entrada e dados válidos |
| 08 | [Injeção de dependência](aulas/08-injecao-de-dependencia.md) | DI, interfaces e serviços |
| 09 | [Entity Framework Core](aulas/09-entity-framework-core.md) | ORM, `DbContext` e `DbSet` |
| 10 | [MySQL e migrations](aulas/10-mysql-e-migrations.md) | persistência e evolução do schema |
| 11 | [Swagger e OpenAPI](aulas/11-swagger-openapi.md) | documentação e teste dos endpoints |
| 12 | [Autenticação JWT](aulas/12-autenticacao-jwt.md) | login, token e Bearer |
| 13 | [Roles e autorização](aulas/13-roles-e-autorizacao.md) | autenticação x autorização e policies |
| 14 | [Arquitetura em camadas](aulas/14-arquitetura-em-camadas.md) | Api, Application, Domain e Infrastructure |
| 15 | [Testes automatizados](aulas/15-testes-automatizados.md) | unitários, integração e `dotnet test` |

## 🧪 Aprender fazendo

Depois das aulas, use os [desafios](desafios/README.md). As soluções ficam separadas em `desafios/solucoes/` para você tentar antes de consultar.

Quando terminar, faça o **[projeto final guiado](projeto-final/README.md)** e construa a API novamente sem seguir um tutorial linha por linha.

## 📦 Exemplos pequenos

A pasta `exemplos/` contém versões reduzidas para estudar uma ideia sem carregar toda a complexidade do projeto final:

- `01-hello-world`: a menor Minimal API possível;
- `02-crud-memoria`: CRUD completo sem banco de dados.

## 🏗️ Projeto final existente

O código em `src/` representa uma evolução mais próxima de um projeto real:

```text
src/
├── MinimalApi.Api
├── MinimalApi.Application
├── MinimalApi.Domain
└── MinimalApi.Infrastructure
```

Não comece por ele se você for iniciante. Primeiro construa a versão simples e entenda cada degrau; depois a arquitetura completa deixa de parecer “mágica”.

## ▶️ Executando o projeto completo

O projeto atual usa **.NET 9**.

```bash
dotnet restore
dotnet build
dotnet run --project src/MinimalApi.Api
```

Para os recursos que usam MySQL/JWT, configure os segredos localmente conforme a [Aula 10](aulas/10-mysql-e-migrations.md). O repositório não deve armazenar senhas ou chaves reais.

Para executar os testes:

```bash
dotnet test
```

## 🔐 Segurança importante

Nunca publique senha de banco, token ou chave JWT no GitHub. Este projeto usa configuração vazia no `appsettings.json`; valores locais devem ser fornecidos por **User Secrets** ou variáveis de ambiente.

> Se uma credencial já foi publicada no histórico Git, removê-la do arquivo atual não é suficiente: ela deve ser **rotacionada**.

## 📚 Documentação oficial

- [.NET](https://dotnet.microsoft.com/)
- [ASP.NET Core Minimal APIs](https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis)
- [Entity Framework Core](https://learn.microsoft.com/ef/core/)
- [ASP.NET Core Authentication](https://learn.microsoft.com/aspnet/core/security/authentication/)

---

**Objetivo do repositório:** você deve terminar a trilha conseguindo explicar, sem decorar, o caminho completo de uma requisição HTTP até o banco de dados e de volta ao cliente.
