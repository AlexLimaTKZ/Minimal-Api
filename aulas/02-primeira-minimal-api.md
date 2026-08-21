# 02 — Sua primeira Minimal API

## 🤔 O problema

Queremos criar um programa que fique esperando requisições HTTP e responda quando alguém acessar `/`.

## 1. Crie o projeto

```bash
dotnet new web -n MinhaPrimeiraApi
cd MinhaPrimeiraApi
```

Abra `Program.cs` e deixe assim:

```csharp
var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/", () => "Olá, mundo!");

app.Run();
```

Execute:

```bash
dotnet run
```

O terminal mostrará a URL local da aplicação.

## 🧒 Feynman: linha por linha

### `CreateBuilder`

```csharp
var builder = WebApplication.CreateBuilder(args);
```

Pense no `builder` como a mesa onde preparamos as peças que a aplicação poderá usar.

### `Build`

```csharp
var app = builder.Build();
```

Agora pegamos as configurações preparadas e construímos a aplicação.

### `MapGet`

```csharp
app.MapGet("/", () => "Olá, mundo!");
```

Em português:

> Quando alguém fizer um pedido **GET** na porta `/`, devolva `Olá, mundo!`.

### `Run`

```csharp
app.Run();
```

É o equivalente a dizer:

> Ligue o servidor e fique esperando pedidos.

## 🧠 Modelo mental

```text
WebApplication.CreateBuilder
          ↓
       Build
          ↓
registrar endpoints
          ↓
        Run
```

Quando alguém acessa a URL:

```text
Navegador → GET / → MapGet → "Olá, mundo!"
```

## ❓ Perguntas Feynman

- O que aconteceria se removêssemos `app.Run()`?
- O que `/` representa?
- Por que usamos `MapGet` e não apenas um método C# comum?

## 🎯 Desafio

Crie mais uma rota:

```text
GET /sobre
```

Ela deve retornar uma frase sobre você ou sobre o projeto.
