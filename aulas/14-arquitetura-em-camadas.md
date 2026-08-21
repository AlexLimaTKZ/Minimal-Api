# 14 — Arquitetura em camadas

## 🤔 Por que só agora?

No início, colocar tudo em um `Program.cs` ajuda a enxergar o fluxo. Quando a aplicação cresce, responsabilidades começam a se misturar.

Agora faz sentido separar.

O projeto final usa:

```text
MinimalApi.Api
MinimalApi.Application
MinimalApi.Domain
MinimalApi.Infrastructure
```

## 🧒 Feynman: restaurante organizado

```text
API            → garçom: recebe e devolve pedidos
Application    → gerente/cozinheiro: coordena casos de uso
Domain         → receitas e regras: essência do negócio
Infrastructure → equipamentos/fornecedores: banco e detalhes técnicos
```

É uma analogia, não uma definição formal, mas ajuda a lembrar as responsabilidades.

## Domain

Deve concentrar conceitos do negócio, entidades e contratos que não precisam conhecer HTTP ou MySQL.

## Application

Orquestra ações da aplicação. Um serviço de administrador, por exemplo, descreve uma operação usada pelo endpoint.

## Infrastructure

Contém detalhes externos: EF Core, `DbContext`, migrations e implementações ligadas a persistência.

## Api

É a porta HTTP: configura serviços, middlewares, autenticação e mapeia endpoints.

## Dependências como setas

Um bom exercício é perguntar quem conhece quem:

```text
Api ───────→ Application
 │              ↓
 └────────→ Infrastructure
                ↓
              Domain
```

O objetivo não é decorar o desenho, mas impedir que regras importantes fiquem acopladas a detalhes que mudam facilmente.

## Quando não separar?

Uma Minimal API com três endpoints pode ficar muito bem em poucos arquivos. Arquitetura é ferramenta, não troféu.

Use separação quando ela reduz confusão e melhora manutenção/teste.

## ❓ Perguntas Feynman

- Em qual camada você esperaria encontrar `DbContext`?
- A entidade `Veiculo` deveria depender de Swagger?
- Por que começamos simples antes de mostrar quatro projetos?

## 🎯 Desafio

Percorra `src/` e classifique cinco arquivos reais pela responsabilidade de sua camada. Se algum arquivo parecer estar na camada errada, anote o motivo.
