# 12 — Autenticação com JWT

## 🤔 O problema

Alguns endpoints não podem aceitar qualquer pessoa. Primeiro precisamos saber **quem está fazendo a requisição**.

Isso é autenticação.

## 🧒 Feynman: a pulseira do evento

```text
Pessoa chega
    ↓
mostra credencial
    ↓
porteiro verifica
    ↓
recebe uma pulseira
    ↓
usa a pulseira nas próximas entradas
```

Na API:

```text
POST /login
    ↓
email + senha
    ↓
servidor verifica
    ↓
JWT
    ↓
cliente envia Bearer TOKEN nas próximas requisições
```

## O que é JWT?

JWT significa JSON Web Token. É uma string assinada que pode carregar informações chamadas **claims**.

Exemplos de claims:

```text
id
email
role
expiração
```

## Importante: JWT não é senha

O token serve para representar uma autenticação já realizada. Senhas precisam ser tratadas de forma segura e **nunca devem ser armazenadas em texto puro em produção**.

O código existente neste projeto contém uma simplificação histórica de autenticação. Use-a para entender o fluxo, não como receita pronta de segurança para produção.

## Configurando autenticação

O projeto usa `AddAuthentication().AddJwtBearer(...)` para ensinar ao ASP.NET como validar tokens recebidos.

Depois:

```csharp
app.UseAuthentication();
app.UseAuthorization();
```

## Header Bearer

O cliente envia:

```text
Authorization: Bearer eyJhbGciOi...
```

O middleware valida assinatura, emissor, audiência e expiração conforme a configuração.

## 🧠 Fluxo

```text
request com token
      ↓
Authentication Middleware
      ↓ válido?
   não ↙   ↘ sim
   401     cria usuário autenticado/claims
                ↓
             endpoint
```

## ❓ Perguntas Feynman

- O que acontece antes de um cliente receber o token?
- Por que o servidor valida assinatura e expiração?
- JWT é a mesma coisa que criptografar a senha?

## 🎯 Desafio

Desenhe o fluxo login → token → endpoint protegido sem consultar esta página.
