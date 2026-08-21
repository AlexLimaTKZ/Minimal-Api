# 15 — Testes automatizados

## 🧒 Feynman: um robô fazendo perguntas

Um teste automatizado é como um robô que repete uma pergunta ao seu código e verifica se a resposta continua correta.

```text
Teste pergunta:
"GET /veiculos/999 retorna 404?"
          ↓
executa a API
          ↓
compara resultado
          ↓
✅ ou ❌
```

## Por que testar?

Sem testes:

```text
mudança → esperança de que nada quebrou
```

Com testes:

```text
mudança → suíte executa → feedback objetivo
```

## Unitário x integração

Modelo simplificado:

```text
Teste unitário
→ uma unidade isolada

Teste de integração
→ várias peças trabalhando juntas
```

Para APIs, testes de endpoint frequentemente verificam roteamento, serialização, autenticação e comportamento HTTP em conjunto.

## Executando o projeto atual

```bash
dotnet test
```

Há testes em `tests/MinimalApi.Tests`, inclusive para autenticação e endpoints de veículos.

## O padrão Arrange, Act, Assert

```text
Arrange → prepare o cenário
Act     → execute a ação
Assert  → confira o resultado
```

Exemplo conceitual:

```csharp
// Arrange
var idInexistente = 999;

// Act
var response = await client.GetAsync($"/veiculos/{idInexistente}");

// Assert
Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
```

## O que testar primeiro?

Priorize comportamentos importantes:

- sucesso do CRUD;
- recurso inexistente;
- entrada inválida;
- endpoint protegido sem token;
- role sem permissão;
- comportamento de login.

## ❓ Perguntas Feynman

- O que um teste automatizado compra para o time?
- Qual a função de Arrange, Act e Assert?
- Por que testar apenas o “caminho feliz” é insuficiente?

## 🎯 Desafio

Escolha um endpoint e escreva três cenários em português antes de escrever qualquer código de teste: sucesso, entrada inválida e recurso inexistente.
