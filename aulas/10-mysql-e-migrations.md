# 10 — MySQL, migrations e segredos

## 🤔 O problema

O EF Core sabe trabalhar com objetos, mas ainda precisamos escolher **onde** os dados serão persistidos e como o esquema do banco evolui.

Neste projeto usamos MySQL.

## 🧒 Feynman: migration é uma instrução de reforma

Imagine que sua classe ganhou uma propriedade `Cor`.

O código mudou, mas a tabela antiga ainda não tem essa coluna.

```text
Classe C# mudou
      ↓
Migration registra a mudança
      ↓
Database Update aplica a mudança
      ↓
Banco fica compatível
```

## Ferramenta do EF

```bash
dotnet tool install --global dotnet-ef
```

Se já estiver instalada:

```bash
dotnet ef --version
```

## Criando migration

A partir da configuração correta dos projetos:

```bash
dotnet ef migrations add NomeDaMudanca \
  --project src/MinimalApi.Infrastructure \
  --startup-project src/MinimalApi.Api
```

Aplicando:

```bash
dotnet ef database update \
  --project src/MinimalApi.Infrastructure \
  --startup-project src/MinimalApi.Api
```

## 🔐 Não coloque senha no GitHub

O `appsettings.json` deste repositório não contém credenciais reais.

Use User Secrets em desenvolvimento:

```bash
cd src/MinimalApi.Api
dotnet user-secrets init
```

Depois:

```bash
dotnet user-secrets set "ConnectionStrings:mysql" "Server=localhost;Database=minimal_api;Uid=SEU_USUARIO;Pwd=SUA_SENHA;"
dotnet user-secrets set "Jwt:Key" "UMA_CHAVE_LOCAL_LONGA_E_SEGURA"
```

Confira apenas as chaves cadastradas quando necessário:

```bash
dotnet user-secrets list
```

> Não cole valores reais em issues, prints, commits ou Pull Requests.

## Conexão no código

No projeto completo:

```csharp
builder.Services.AddDbContext<DbContexto>(options =>
{
    options.UseMySql(
        builder.Configuration.GetConnectionString("mysql"),
        ServerVersion.AutoDetect(
            builder.Configuration.GetConnectionString("mysql")));
});
```

O `IConfiguration` reúne valores de fontes como `appsettings`, User Secrets e variáveis de ambiente.

## ❓ Perguntas Feynman

- Por que mudar uma classe não muda automaticamente um banco já existente?
- O que uma migration representa?
- Por que senha não deve ficar no Git?

## 🎯 Desafio

Adicione uma propriedade de treino a uma entidade em um projeto descartável, gere a migration e leia o arquivo gerado antes de aplicá-lo.
