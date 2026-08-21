# 08 — Injeção de dependência

## 🤔 O problema

Um endpoint precisa executar uma tarefa de negócio. Se ele mesmo criar todas as ferramentas de que precisa, fica difícil trocar implementações e testar.

## 🧒 Feynman: a oficina

Um mecânico precisa de uma chave de fenda.

Sem injeção:

```text
Mecânico → fabrica a própria ferramenta → usa
```

Com injeção:

```text
Oficina → entrega a ferramenta → Mecânico usa
```

No ASP.NET Core, a “oficina” é o **container de serviços**.

## Registrando um serviço

```csharp
builder.Services.AddScoped<IAdministradorServico, AdministradorServico>();
```

Leia como:

> Quando alguém pedir `IAdministradorServico`, entregue uma instância de `AdministradorServico` durante esta requisição.

## Recebendo no endpoint

```csharp
app.MapPost("/login", async (
    LoginDTO login,
    IAdministradorServico servico) =>
{
    var admin = await servico.GetAdministradorByEmailAndSenha(
        login.Email,
        login.Senha);

    return admin is null
        ? Results.Unauthorized()
        : Results.Ok();
});
```

O ASP.NET percebe o parâmetro de serviço e fornece a implementação registrada.

## Lifetimes

Os três nomes que você mais verá:

```text
Transient → cria frequentemente uma nova instância
Scoped    → uma instância por requisição HTTP
Singleton → uma instância para a aplicação inteira
```

`DbContext` costuma ser `Scoped`.

## 🧠 Por que interface?

```text
Endpoint → IAdministradorServico ← AdministradorServico
```

O endpoint conhece o **contrato**, não precisa conhecer todos os detalhes da implementação.

Isso ajuda em testes e substituições.

## ❓ Perguntas Feynman

- Quem cria o `AdministradorServico`?
- Por que isso é melhor do que `new AdministradorServico(...)` dentro do endpoint?
- O que significa `Scoped`?

## 🎯 Desafio

Crie `ISaudacaoServico` e `SaudacaoServico`, registre com `AddScoped` e injete em `GET /saudacao/{nome}`.
