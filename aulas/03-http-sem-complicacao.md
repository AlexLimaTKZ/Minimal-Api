# 03 — HTTP sem complicação

## 🧒 Feynman: verbos para conversar

Se a API é uma recepção, HTTP fornece palavras padronizadas para dizer o que queremos.

```text
GET    → quero ler
POST   → quero criar
PUT    → quero substituir/atualizar
DELETE → quero apagar
```

Esses são **métodos HTTP**.

## Exemplo com veículos

```text
GET    /veiculos      → listar
GET    /veiculos/5    → buscar o id 5
POST   /veiculos      → criar
PUT    /veiculos/5    → atualizar o id 5
DELETE /veiculos/5    → remover o id 5
```

## Status code = resumo da resposta

A resposta HTTP possui um código que indica o resultado.

| Código | Significado simples |
|---|---|
| `200 OK` | deu certo |
| `201 Created` | algo foi criado |
| `204 No Content` | deu certo, sem corpo de resposta |
| `400 Bad Request` | o cliente enviou dados inválidos |
| `401 Unauthorized` | precisa se autenticar |
| `403 Forbidden` | está autenticado, mas não tem permissão |
| `404 Not Found` | recurso não encontrado |
| `500 Internal Server Error` | erro inesperado no servidor |

## JSON

APIs frequentemente enviam objetos em JSON:

```json
{
  "id": 1,
  "marca": "Honda",
  "modelo": "Civic"
}
```

Pense em JSON como uma forma de escrever dados em texto para que sistemas diferentes consigam entendê-los.

## Request e response completos

```text
REQUEST
POST /veiculos
Content-Type: application/json

{ "marca": "Honda", "modelo": "Civic" }

             ↓

RESPONSE
201 Created

{ "id": 1, "marca": "Honda", "modelo": "Civic" }
```

## ❓ Perguntas Feynman

- Por que GET e POST não significam a mesma coisa?
- Qual a diferença entre 401 e 403?
- Por que JSON é útil?

## 🎯 Desafio

Escolha os métodos corretos para: cadastrar usuário, listar produtos, apagar comentário e alterar endereço.
