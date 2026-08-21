# 11 — Swagger e OpenAPI

## 🧒 Feynman: o cardápio da API

Imagine chegar a um restaurante sem saber o que pode pedir. Uma API sem documentação causa problema parecido.

OpenAPI descreve as operações disponíveis. Swagger UI usa essa descrição para criar uma interface navegável e testável.

```text
API
├── GET /veiculos
├── POST /veiculos
└── DELETE /veiculos/{id}
       ↓
OpenAPI descreve
       ↓
Swagger UI mostra e permite testar
```

## Configuração usada no projeto

```csharp
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
```

Depois da construção da aplicação:

```csharp
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
```

## Por que somente em Development?

Uma interface de exploração pode ser útil em desenvolvimento, mas a exposição em produção deve ser uma decisão consciente do projeto.

## O Swagger substitui testes?

Não.

```text
Swagger → exploração manual e documentação
Testes  → verificação automática repetível
```

Os dois têm papéis diferentes.

## Melhorando a documentação

Endpoints podem receber nomes, descrições, tipos de resposta e grupos. Quanto melhor o contrato, mais útil fica a documentação gerada.

## ❓ Perguntas Feynman

- Qual diferença existe entre OpenAPI e Swagger UI?
- Por que Swagger não substitui testes?
- O que um consumidor da API ganha com boa documentação?

## 🎯 Desafio

Execute a API em desenvolvimento e, pelo Swagger, identifique método, rota, body esperado e respostas de um endpoint de veículos.
