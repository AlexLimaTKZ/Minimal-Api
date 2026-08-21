# 00 — Comece aqui

## 🎯 Objetivo

Preparar seu ambiente e entender como esta trilha funciona. Você **não precisa conhecer ASP.NET Core**.

É suficiente ter noções básicas de C#: variável, `if`, método, classe e lista.

## 🧒 Feynman: o que vamos fazer?

Imagine que você vai construir uma pequena recepção. Alguém chega, faz um pedido e recebe uma resposta.

```text
Pessoa → pedido → recepção → resposta → pessoa
```

Na web:

```text
Cliente → requisição HTTP → API → resposta HTTP → Cliente
```

Todo o curso é uma evolução dessa ideia.

## 🧰 Instale o necessário

Você precisa de:

- .NET SDK compatível com o projeto (a referência atual é .NET 9);
- editor como VS Code, Visual Studio ou Rider;
- Git, se quiser versionar seus exercícios;
- MySQL apenas a partir da Aula 10.

Confira o .NET:

```bash
dotnet --version
```

## 📁 Crie uma pasta de treino

Não comece alterando o projeto final. Crie um espaço para reproduzir as aulas:

```bash
mkdir minimal-api-estudos
cd minimal-api-estudos
```

## 🧠 Regra da trilha

Antes de avançar, responda três perguntas:

1. Qual problema esta aula resolveu?
2. O que cada parte principal do código faz?
3. Eu conseguiria explicar isso para alguém que não programa?

Se a resposta 3 for “não”, releia usando palavras mais simples.

## 🗺️ Ordem recomendada

```text
API → HTTP → endpoints → CRUD → DTO → DI → banco → JWT → arquitetura → testes
```

Essa ordem é proposital. Banco de dados e autenticação são úteis, mas escondem o conceito fundamental se aparecem cedo demais.

## 🧪 Mini desafio

Explique em uma frase, sem usar a palavra “endpoint”:

> O que você acha que uma API faz?

Guarde sua resposta. Na Aula 01 você vai compará-la com o modelo que construiremos.

## ✅ Checklist

- [ ] `dotnet --version` funciona;
- [ ] tenho um editor;
- [ ] entendo que cliente envia um pedido e servidor devolve uma resposta;
- [ ] vou tentar explicar cada aula com minhas próprias palavras.
