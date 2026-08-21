# ✅ Soluções comentadas

> Use somente depois de tentar. Uma solução é uma referência, não a única forma correta.

## Desafio 01

Uma versão mínima:

```csharp
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Olá!");
app.MapGet("/status", () => Results.Ok(new { online = true }));
app.MapGet("/saudacao/{nome}", (string nome) =>
    Results.Ok(new { mensagem = $"Olá, {nome}!" }));

app.Run();
```

Pergunta de conferência: se você trocar `MapGet` por `MapPost`, o que muda para o cliente?

## Desafio 02

Compare sua implementação com `exemplos/02-crud-memoria`. O essencial é que você consiga justificar:

```text
POST   → 201 ao criar
GET id → 404 quando não encontra
PUT    → 404 ou 204
DELETE → 404 ou 204
```

## Desafio 03

Exemplo de contrato:

```csharp
public record CriarTarefaDto(string Titulo);
```

Validação simples no endpoint:

```csharp
if (string.IsNullOrWhiteSpace(dto.Titulo))
    return Results.BadRequest(new { erro = "Titulo é obrigatório" });
```

O ponto pedagógico é perceber que o cliente não controla todos os campos internos.

## Desafio 04

A sequência conceitual esperada é:

```text
entidade → DbSet → DbContext → provider → banco
```

Para criar:

```csharp
db.Tarefas.Add(tarefa);
await db.SaveChangesAsync();
```

`Add` muda o estado rastreado pelo contexto; `SaveChangesAsync` persiste.

## Desafio 05

Confira se sua explicação distingue:

```text
Authentication = identidade
Authorization  = permissão
```

O endpoint protegido deve usar `RequireAuthorization()` ou uma policy equivalente.

## Desafio 06

Estruture seus testes como:

```text
Arrange → cenário
Act     → chamada
Assert  → resultado
```

Não copie os testes existentes sem entendê-los. Abra `tests/MinimalApi.Tests/Api` e compare os cenários somente depois de escrever os seus.
