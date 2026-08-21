# 01 — O que é uma API?

## 🤔 O problema

Dois programas precisam conversar. Um aplicativo de celular, por exemplo, precisa pedir dados a um servidor sem saber como o servidor guarda esses dados.

## 🧒 Feynman: a analogia do restaurante

Pense em um restaurante:

```text
Cliente → Garçom → Cozinha
```

O cliente não entra na cozinha para buscar comida. Ele faz um pedido ao garçom, que leva a solicitação e traz a resposta.

Na web:

```text
Frontend → API → sistema/banco
```

A API é uma **porta de comunicação com regras bem definidas**.

## 🧠 Modelo mental

Uma conversa básica possui duas partes:

```text
REQUEST  → “quero alguma coisa”
RESPONSE ← “aqui está o resultado”
```

Exemplo:

```text
GET /veiculos
      ↓
API procura os veículos
      ↓
200 OK
[
  { "id": 1, "marca": "Honda" }
]
```

## API não é banco de dados

A API pode consultar um banco, arquivo ou outro serviço, mas ela não é nenhum deles.

```text
Cliente
  ↓
API
  ├── banco de dados
  ├── serviço externo
  └── regras da aplicação
```

## REST, em linguagem simples

Em uma API REST, recursos costumam ser representados por URLs e manipulados com métodos HTTP.

```text
/veiculos → coleção de veículos
/veiculos/10 → veículo de id 10
```

Nas próximas aulas veremos os métodos HTTP que dizem **qual ação** queremos realizar.

## ❓ Perguntas Feynman

Tente responder sem olhar:

- Por que o frontend não precisa saber como o banco funciona?
- O que é request?
- O que é response?
- Por que a analogia do garçom faz sentido?

## 🎯 Desafio

Escolha um aplicativo que você usa e desenhe:

```text
Aplicativo → API → servidor → resposta
```

Pense em uma informação que o app precisa buscar.
