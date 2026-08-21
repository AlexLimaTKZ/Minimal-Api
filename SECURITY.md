# 🔐 Segurança

Este repositório é educacional, mas boas práticas de segurança também fazem parte do aprendizado.

## Segredos

Nunca versione:

- senha de banco de dados;
- chave JWT;
- token de API;
- arquivo `.env` com valores reais;
- credenciais de produção.

Use **User Secrets** durante o desenvolvimento ou variáveis de ambiente em outros ambientes.

```bash
cd src/MinimalApi.Api
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:mysql" "SUA_CONNECTION_STRING"
dotnet user-secrets set "Jwt:Key" "SUA_CHAVE_LONGA_E_SEGURA"
```

## Credencial já publicada

Se um segredo já apareceu em um commit, simplesmente apagar a linha atual não torna aquele segredo seguro: o histórico Git pode continuar contendo o valor. **Rotacione/revogue a credencial exposta.**

## Senhas de usuários

A implementação histórica de login deste projeto ainda demonstra comparação de senha de maneira simplificada para fins de estudo. Isso **não é adequado para produção**.

Em um sistema real, utilize um algoritmo próprio para armazenamento de senhas, como os mecanismos fornecidos pelo ASP.NET Core Identity/PasswordHasher ou uma solução equivalente com hash e salt.

## Relatos

Não publique credenciais reais em Issues ou Pull Requests. Ao relatar um problema, substitua segredos por valores fictícios.
