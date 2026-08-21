# 09 — Entity Framework Core

## 🤔 O problema

Até agora nossa lista de veículos mora na memória. Ao reiniciar o servidor, tudo desaparece.

Precisamos guardar os dados em um lugar persistente.

## 🧒 Feynman: um tradutor entre C# e banco

Bancos relacionais entendem SQL. Nosso código trabalha com objetos C#.

O Entity Framework Core (EF Core) atua como tradutor:

```text
Objetos C#
   ↓
Entity Framework Core
   ↓
SQL
   ↓
Banco de dados
```

Ele é um **ORM** (Object-Relational Mapper).

## DbContext

Pense no `DbContext` como a central de conversa com o banco.

```csharp
public class DbContexto : DbContext
{
    public DbContexto(DbContextOptions<DbContexto> options)
        : base(options) { }

    public DbSet<Veiculo> Veiculos { get; set; } = default!;
}
```

## DbSet

Um `DbSet<Veiculo>` representa a coleção de veículos que o EF consegue consultar e modificar.

Modelo mental:

```text
DbContexto
   └── Veiculos (DbSet)
          ↓
      tabela no banco
```

## Consultando

```csharp
var veiculos = await db.Veiculos.ToListAsync();
```

Em termos simples:

> EF, busque todos os veículos e transforme o resultado em objetos C#.

## Inserindo

```csharp
db.Veiculos.Add(veiculo);
await db.SaveChangesAsync();
```

`Add` informa a intenção. `SaveChangesAsync` envia as alterações pendentes ao banco.

## Buscando por id

```csharp
var veiculo = await db.Veiculos.FindAsync(id);
```

## 🧠 Fluxo completo

```text
GET /veiculos
     ↓
endpoint recebe DbContexto
     ↓
db.Veiculos.ToListAsync()
     ↓
EF gera consulta
     ↓
Banco responde
     ↓
EF cria objetos C#
     ↓
API devolve JSON
```

## ❓ Perguntas Feynman

- Por que o EF Core é chamado de ORM?
- Qual diferença existe entre `DbContext` e `DbSet`?
- Por que `Add` sozinho não basta para persistir?

## 🎯 Desafio

Abra `src/MinimalApi.Infrastructure` e localize o `DbContexto`. Explique com suas palavras o papel de cada `DbSet`.
