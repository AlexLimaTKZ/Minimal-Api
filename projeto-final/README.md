# 🎓 Projeto final — API de Garagem

## Missão

Construa uma API de veículos **sem seguir as aulas linha por linha**. Use a documentação apenas para consultar conceitos quando travar.

A meta é provar que você consegue reconstruir o raciocínio.

## Requisitos do veículo

```text
Id
Marca
Modelo
Ano
Cor
```

## Nível 1 — HTTP e CRUD em memória

Implemente:

```text
GET    /veiculos
GET    /veiculos/{id}
POST   /veiculos
PUT    /veiculos/{id}
DELETE /veiculos/{id}
```

Critérios:

- códigos HTTP coerentes;
- 404 para id inexistente;
- id controlado pelo servidor;
- JSON de entrada e saída compreensível.

## Nível 2 — DTO e validação

O cliente não deve enviar tudo que a entidade possui. Crie DTOs quando houver benefício e rejeite dados inválidos.

## Nível 3 — Persistência

Substitua a lista em memória por EF Core + MySQL.

Você deve conseguir explicar:

```text
DbContext
DbSet
migration
SaveChangesAsync
connection string
```

## Nível 4 — Documentação

Adicione OpenAPI/Swagger e teste manualmente os endpoints.

## Nível 5 — Login

Implemente um fluxo de autenticação que emita JWT. Para projeto real, use armazenamento seguro de senha; não use texto puro.

## Nível 6 — Autorização

Defina pelo menos dois níveis de permissão. Exemplo:

```text
Editor → visualizar
Admin  → visualizar + criar + alterar + excluir
```

## Nível 7 — Arquitetura

Somente depois da API funcionar, reorganize em responsabilidades claras. Compare sua proposta com `src/`.

Não crie camadas apenas porque o projeto de referência possui camadas. Explique qual problema cada separação resolve.

## Nível 8 — Testes

Cubra pelo menos:

- criação válida;
- entrada inválida;
- consulta inexistente;
- rota protegida sem token;
- permissão insuficiente;
- exclusão bem-sucedida.

## Entrega Feynman

Além do código, escreva `EXPLICACAO.md` respondendo, com suas palavras:

1. O que é uma API?
2. O que ocorre quando chega `POST /veiculos`?
3. Qual a diferença entre DTO e entidade?
4. Para que serve o `DbContext`?
5. O que uma migration faz?
6. Qual a diferença entre autenticação e autorização?
7. Por que existem camadas no seu projeto?
8. O que seus testes protegem?

## Critério de conclusão

Você concluiu de verdade quando consegue desenhar este fluxo e explicar cada seta:

```text
Cliente
  ↓ HTTP
API / Endpoint
  ↓
Application / regra de uso
  ↓
Domain
  ↓
Infrastructure / EF Core
  ↓
MySQL
  ↓
resposta volta pelo caminho inverso
```

Se alguma seta ainda parece mágica, volte à aula correspondente. Isso é Feynman funcionando: a dificuldade aponta a lacuna.
