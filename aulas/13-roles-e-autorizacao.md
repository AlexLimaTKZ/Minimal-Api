# 13 — Roles e autorização

Duas perguntas diferentes:

```text
Autenticação → Quem é você?
Autorização  → O que você pode fazer?
```

Essa distinção elimina grande parte da confusão sobre segurança em APIs.

## 🧒 Feynman: crachá da empresa

Seu crachá prova que você trabalha na empresa. Isso autentica você.

Mas o crachá também pode permitir acesso apenas a determinadas salas. Isso é autorização.

## Roles

No projeto usamos papéis como:

```text
Admin
Editor
```

Um token pode conter uma claim de role.

## Policies

Policies transformam regras de acesso em nomes claros:

```csharp
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy",
        policy => policy.RequireRole("Admin"));

    options.AddPolicy("EditorPolicy",
        policy => policy.RequireRole("Editor", "Admin"));
});
```

Depois:

```csharp
app.MapDelete("/veiculos/{id}", ...)
   .RequireAuthorization("AdminPolicy");
```

## 🧠 Interpretação

```text
GET /veiculos
   ↓
EditorPolicy
   ↓
Editor ou Admin podem entrar

DELETE /veiculos/5
   ↓
AdminPolicy
   ↓
somente Admin
```

## 401 x 403

```text
401 → não provou quem é
403 → provou quem é, mas não tem acesso
```

## ❓ Perguntas Feynman

- Por que autenticação não garante autorização?
- O que uma policy ganha em relação a espalhar `if (role == ...)` por todo código?
- Quando você esperaria 401 e quando 403?

## 🎯 Desafio

Crie uma policy fictícia `LeitorPolicy` e defina, em palavras, quais endpoints ela deveria acessar.
