# 07 — DTOs e validação

## 🤔 O problema

Nem sempre queremos receber ou devolver a entidade inteira.

Imagine um usuário com:

```text
Id
Nome
Email
SenhaHash
Perfil
DataCriacao
```

Para login, precisamos apenas de:

```text
Email
Senha
```

## 🧒 Feynman: DTO é um envelope

A entidade é a ficha completa guardada pelo sistema. O DTO é um envelope preparado para uma viagem específica.

```text
Entidade completa
      ↓ seleciona somente o necessário
DTO de Login
├── Email
└── Senha
```

DTO significa **Data Transfer Object**.

Exemplo:

```csharp
public record CriarVeiculoDto(
    string Marca,
    string Modelo,
    int Ano,
    string Cor
);
```

Note que o cliente não envia `Id`; o servidor pode gerar esse valor.

## Validação

Receber JSON não significa aceitar qualquer coisa.

```csharp
public class Veiculo
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Marca { get; set; } = string.Empty;
}
```

Data Annotations descrevem regras simples.

No projeto completo há validação antes de salvar e retorno de `Results.ValidationProblem(...)` quando os dados não são válidos.

## 🧠 Modelo mental

```text
JSON recebido
    ↓
DTO / modelo de entrada
    ↓
validação
   ↙  ↘
 inválido  válido
   ↓        ↓
400        continuar
```

## Regra importante

Validação de formato e regra de negócio não são exatamente a mesma coisa.

```text
"Marca é obrigatória" → validação de entrada
"veículo não pode ser vendido duas vezes" → regra de negócio
```

## ❓ Perguntas Feynman

- Por que não enviar a entidade inteira no login?
- Qual vantagem existe em o servidor controlar o `Id`?
- O que deve acontecer quando um campo obrigatório não é enviado?

## 🎯 Desafio

Crie um `CriarVeiculoDto` e adapte seu POST em memória para recebê-lo.
