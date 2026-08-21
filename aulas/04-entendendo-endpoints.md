# 04 — Entendendo endpoints

## 🧠 A fórmula

Um endpoint de Minimal API pode ser entendido assim:

```text
MÉTODO HTTP + ROTA + HANDLER
```

Exemplo:

```csharp
app.MapGet("/veiculos", () => "Lista de veículos");
```

Separando:

```text
MapGet       → método HTTP GET
/veiculos    → rota
() => ...    → código executado quando a rota é chamada
```

## 🧒 Feynman: portas numeradas

Imagine um corredor com portas:

```text
/           → recepção
/veiculos   → setor de veículos
/login      → setor de autenticação
```

A rota diz **em qual porta bater**. O método HTTP diz **o que você quer fazer naquela porta**.

Assim, estes dois endpoints podem compartilhar a mesma rota:

```csharp
app.MapGet("/veiculos", () => "listar");
app.MapPost("/veiculos", () => "criar");
```

Mesmo endereço, ações diferentes.

## Retornando resultados HTTP

Minimal APIs oferecem helpers:

```csharp
return Results.Ok(dados);
return Results.NotFound();
return Results.Created("/veiculos/1", veiculo);
return Results.NoContent();
```

Eles deixam explícito qual resposta HTTP será enviada.

## Handler com mais de uma linha

```csharp
app.MapGet("/saudacao/{nome}", (string nome) =>
{
    var mensagem = $"Olá, {nome}!";
    return Results.Ok(mensagem);
});
```

## ❓ Perguntas Feynman

- O que diferencia `/veiculos` com GET de `/veiculos` com POST?
- O que é o handler?
- Por que `Results.NotFound()` comunica melhor a intenção do que retornar apenas `null`?

## 🎯 Desafio

Crie `/status` que responda `200 OK` com um objeto contendo `online = true`.
