# 05 — Parâmetros e dados de entrada

Uma API pode receber dados de lugares diferentes. Os três mais comuns para esta trilha são **rota**, **query string** e **body**.

## 1. Route parameter

```text
GET /veiculos/42
```

A rota pode declarar um espaço reservado:

```csharp
app.MapGet("/veiculos/{id}", (int id) =>
{
    return Results.Ok(new { id });
});
```

```text
/veiculos/42
           ↓
         id = 42
```

Use quando o valor identifica diretamente o recurso.

## 2. Query string

```text
GET /veiculos?marca=Honda&ano=2024
```

```csharp
app.MapGet("/veiculos", (string? marca, int? ano) =>
{
    return Results.Ok(new { marca, ano });
});
```

É útil para filtros e opções.

## 3. Body

Para criar um objeto, normalmente enviamos JSON no corpo:

```json
{
  "marca": "Honda",
  "modelo": "Civic",
  "ano": 2024,
  "cor": "Preto"
}
```

Se existir uma classe compatível:

```csharp
public class Veiculo
{
    public int Id { get; set; }
    public string Marca { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
}
```

podemos receber:

```csharp
app.MapPost("/veiculos", (Veiculo veiculo) =>
{
    return Results.Ok(veiculo);
});
```

ASP.NET Core lê o JSON e monta o objeto C#.

## 🧒 Feynman: endereço, observação e pacote

```text
rota  → parte do endereço
query → observações anexadas ao pedido
body  → pacote de dados enviado
```

## ❓ Perguntas Feynman

- Onde colocaria o id de um veículo específico?
- Onde colocaria um filtro opcional de cor?
- Onde enviaria todos os dados de um veículo novo?

## 🎯 Desafio

Crie `GET /ola/{nome}?formal=true` e retorne uma saudação diferente quando `formal` for verdadeiro.
