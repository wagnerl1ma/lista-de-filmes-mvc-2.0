# TODO - Minha Lista de Filmes

Lista de pendencias e melhorias planejadas para o projeto.

---

## Funcionalidades Pendentes

### Envio de E-mail para Recuperacao de Senha ✅ Concluido
- [x] Escolher provedor de e-mail — **MailKit (SMTP generico)**
- [x] Criar `EmailSettings.cs` e `EmailSender.cs` em `App/Extensions/`
- [x] Registrar `EmailSettings` e `IEmailSender` no DI em `Configurations/IdentityConfig.cs`
- [x] Adicionar secao `EmailSettings` no `appsettings.json`
- [x] Proteger `appsettings.Development.json` e `appsettings.Staging.json` no `.gitignore`
- [x] Corrigir bug em `ForgotPassword.cshtml.cs` (`IsEmailConfirmedAsync` sempre retornava false)
- [x] Atualizar `ForgotPassword.cshtml.cs` para gerar token e enviar e-mail HTML estilizado
- [x] Criar paginas `ResetPassword.cshtml/.cs` e `ResetPasswordConfirmation.cshtml/.cs`
- [x] Testar o fluxo completo: esqueceu senha -> e-mail -> link -> nova senha -> login

**Credenciais SMTP:** configurar em `appsettings.Staging.json` (nao commitado) com SenderEmail, Username e Password (App Password do Gmail)

---

### CRUD de Diretor
- [ ] Implementar entidade, service, repository, views (seguir checklist do CLAUDE.md)
- [ ] Tornar `DiretorId` obrigatorio em `Filme.cs` (atualmente nullable)

### CRUD de Produtora
- [ ] Implementar entidade, service, repository, views (seguir checklist do CLAUDE.md)
- [ ] Tornar `ProdutoraId` obrigatorio em `Filme.cs` (atualmente nullable)

---

## Testes E2E (Selenium + xUnit)

Projeto: `src/ListaDeFilmes.Tests.E2E/`

### Pendencias
- [ ] Corrigir/validar o `LoginAsAdmin()` — login via JavaScript pode falhar dependendo de validacoes client-side
- [ ] Executar todos os 19 testes do fluxo completo e corrigir falhas
- [ ] Atualizar o `msedgedriver.exe` em `drivers/` quando o Edge for atualizado (versao atual: 147)
- [ ] Remover credenciais hardcoded do `FluxoCompletoTests.cs` e usar User Secrets ou variavel de ambiente
- [ ] Adicionar testes para o fluxo de "Esqueceu a senha" (envio de e-mail ja implementado)

### Como executar
1. Iniciar a aplicacao (`Ctrl+F5` no Visual Studio ou `dotnet run --project src/ListaDeFilmes.App`)
2. Rodar os testes: `dotnet test src/ListaDeFilmes.Tests.E2E`
3. Para debugar: Test Explorer > botao direito em `FluxoCompletoTests` > Debug Tests

---

## Infraestrutura

### CI/CD
- [ ] Configurar pipelines em `.github/workflows/`
- [ ] Build automatico em PRs
- [ ] Rodar testes automaticamente

---

*Ultima atualizacao: 2026-03-29*
