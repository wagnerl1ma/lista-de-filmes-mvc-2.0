# TODO - Minha Lista de Filmes

Lista de pendencias e melhorias planejadas para o projeto.

---

## Funcionalidades Pendentes

### Envio de E-mail para Recuperacao de Senha
- [ ] Escolher provedor de e-mail (SendGrid, SMTP, Mailgun, etc.)
- [ ] Criar uma implementacao de `IEmailSender` (ex: `EmailSender.cs` em `Extensions/`)
- [ ] Registrar o servico no DI em `Configurations/IdentityConfig.cs`
- [ ] Adicionar configuracoes de e-mail no `appsettings.json` (ou User Secrets para credenciais)
- [ ] Atualizar `ForgotPassword.cshtml.cs` para gerar token e enviar e-mail com link de reset
- [ ] Criar as paginas `ResetPassword.cshtml` e `ResetPasswordConfirmation.cshtml`
- [ ] Testar o fluxo completo: esqueceu senha -> e-mail -> link -> nova senha -> login

**Arquivos que precisam ser alterados:**
- `src/ListaDeFilmes.App/Configurations/IdentityConfig.cs` — registrar `IEmailSender`
- `src/ListaDeFilmes.App/Areas/Identity/Pages/Account/ForgotPassword.cshtml.cs` — gerar token e enviar e-mail
- `appsettings.json` / User Secrets — adicionar configuracoes do provedor de e-mail

**Referencia:** ver comentarios `// TODO: [EMAIL]` no codigo

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
- [ ] Adicionar testes para o fluxo de "Esqueceu a senha" (apos implementar envio de e-mail)

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

*Ultima atualizacao: 2026-03-22*
