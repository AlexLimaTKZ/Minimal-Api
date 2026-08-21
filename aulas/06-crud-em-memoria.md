# 06 — CRUD completo em memória

## 🎯 Por que sem banco primeiro?

Queremos aprender API antes de aprender banco. Uma `List<Veiculo>` é suficiente para enxergar o fluxo HTTP inteiro.

CRUD significa:

```text
Create → criar
Read   → ler
Update → atualizar
Delete → apagar
```

## Modelo

```csharp
public class Veiculo
{
    public int Id { get; set; }
    public string Marca { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public int Ano { get; set; }
    public string Cor { get; set; } = string.Empty;
}
```

Crie a lista:

```csharp
var veiculos = new List<Veiculo>();
var proximoId = 1;
```

## CREATE — POST

```csharp
app.MapPost("/veiculos", (Veiculo veiculo) =>
{
    veiculo.Id = proximoId++;
    veiculos.Add(veiculo);
    return Results.Created($"/veiculos/{veiculo.Id}", veiculo);
});
```

## READ — GET

```csharp
app.MapGet("/veiculos", () => Results.Ok(veiculos));
```

```csharp
app.MapGet("/veiculos/{id}", (int id) =>
{
    var veiculo = veiculos.FirstOrDefault(v => v.Id == id);
    return veiculo is null ? Results.NotFound() : Results.Ok(veiculo);
});
```

## UPDATE — PUT

```csharp
app.MapPut("/veiculos/{id}", (int id, Veiculo entrada) =>
{
    var veiculo = veiculos.FirstOrDefault(v => v.Id == id);
    if (veiculo is null) return Results.NotFound();

    veiculo.Marca = entrada.Marca;
    veiculo.Modelo = entrada.Modelo;
    veiculo.Ano = entrada.Ano;
    veiculo.Cor = entrada.Cor;

    return Results.NoContent();
});
```

## DELETE

```csharp
app.MapDelete("/veiculos/{id}", (int id) =>
{
    var veiculo = veiculos.FirstOrDefault(v => v.Id == id);
    if (veiculo is null) return Results.NotFound();

    veiculos.Remove(veiculo);
    return Results.NoContent();
});
```

## 🧠 O fluxo inteiro

```text
Cliente
   ↓ request
Endpoint
   ↓
Lista em memória
   ↓
Resultado
   ↓ response
Cliente
```

A lista desaparece quando o servidor reinicia. Isso é proposital. Na Aula 09 substituiremos essa memória temporária por persistência real.

## ❓ Perguntas Feynman

- Por que o POST retorna 201?
- Por que precisamos procurar o veículo antes de PUT/DELETE?
- Por que os dados somem quando a aplicação reinicia?

## 🎯 Desafio

Implemente um filtro `GET /veiculos?marca=...` sem consultar a solução.

Veja também o exemplo executável em `exemplos/02-crud-memoria`.
