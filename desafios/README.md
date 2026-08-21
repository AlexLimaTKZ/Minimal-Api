# 🧪 Desafios

Faça os desafios **sem abrir as soluções primeiro**. O objetivo não é digitar rápido; é identificar o que você ainda não consegue explicar.

## 01 — Primeira API

Crie uma Minimal API do zero com:

- `GET /` → uma mensagem;
- `GET /status` → JSON com `online: true`;
- `GET /saudacao/{nome}` → usa o nome da rota.

Você concluiu quando consegue explicar `CreateBuilder`, `Build`, `MapGet` e `Run`.

## 02 — CRUD em memória

Crie uma API de tarefas com:

```text
GET    /tarefas
GET    /tarefas/{id}
POST   /tarefas
PUT    /tarefas/{id}
DELETE /tarefas/{id}
```

Use uma lista em memória e códigos HTTP coerentes.

## 03 — DTO e validação

Crie um DTO de entrada que não permita ao cliente definir o id. Rejeite título vazio com `400`.

## 04 — Persistência

Evolua o CRUD para EF Core. Explique a diferença entre `DbContext`, `DbSet`, `Add` e `SaveChangesAsync`.

## 05 — Autenticação e autorização

Proteja pelo menos uma rota. Demonstre os três resultados:

```text
sem autenticação → 401
autenticado sem permissão → 403
autenticado com permissão → sucesso
```

## 06 — Testes

Escreva testes para sucesso, recurso inexistente e entrada inválida.

## Regra Feynman

Ao terminar cada desafio, grave ou escreva uma explicação de 60 segundos respondendo:

> “O que acontece desde a requisição chegar até a resposta sair?”

As soluções comentadas estão em [solucoes/README.md](solucoes/README.md).
