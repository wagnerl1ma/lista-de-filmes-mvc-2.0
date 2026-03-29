# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

---

## Visão Geral

Aplicação web de gerenciamento de lista de filmes pessoal. Permite cadastrar filmes
com gênero, diretor, produtora e elenco, com controle de acesso por claims.

**Stack:**
- ASP.NET Core MVC (.NET 8)
- Entity Framework Core 9.0
- SQL Server (LocalDB em desenvolvimento)
- ASP.NET Core Identity (autenticação/autorização)
- FluentValidation 11.x
- AutoMapper 12.x

---

## Comandos Essenciais

```bash
# Build da solução completa
dotnet build ListaDeFilmes.sln

# Executar a aplicação
dotnet run --project src/ListaDeFilmes.App

# Rodar todos os testes
dotnet test src/ListaDeFilmes.Tests

# Rodar um teste específico pelo nome do método
dotnet test src/ListaDeFilmes.Tests --filter "FullyQualifiedName~AdicionarFilme_FilmeValido"

# Rodar testes de uma classe específica
dotnet test src/ListaDeFilmes.Tests --filter "ClassName~FilmeServiceTests"

# Adicionar migration
dotnet ef migrations add <NomeDaMigration> --project src/ListaDeFilmes.Data --startup-project src/ListaDeFilmes.App

# Aplicar migrations manualmente
dotnet ef database update --project src/ListaDeFilmes.Data --startup-project src/ListaDeFilmes.App
```

**Connection string:** configurar em `appsettings.Development.json`
**User Secrets:** ativo apenas em ambiente `Production` (ver `Program.cs`)

---

## Estrutura de Pastas

```
src/
├── ListaDeFilmes.App/        # Apresentação: Controllers, Views, ViewModels, TagHelpers
├── ListaDeFilmes.Business/   # Domínio: Models, Services, Interfaces, Notificações
├── ListaDeFilmes.Data/       # Dados: DbContext, Repositories, Mappings, Migrations
└── ListaDeFilmes.Tests/      # Testes: xUnit + Moq + FluentAssertions
```

### App
- `Controllers/` — Controllers MVC
- `ViewModels/` — ViewModels por entidade
- `Views/<Entidade>/` — Razor views (Index, Create, Edit, Delete, Details)
- `Extensions/` — Extensões de validação e autorização
- `Configurations/` — DependencyInjectionConfig, IdentityConfig, MvcConfig, GlobalizationConfig
- `TagHelpers/` — Custom tag helpers de autorização por claims
- `AutoMapper/` — Profiles do AutoMapper

### Business
- `Models/` — Entidades de domínio (herdam de `Entity.cs`)
- `Models/Validations/` — Classes FluentValidation por entidade
- `Services/` — Serviços de negócio (herdam de `BaseService`)
- `Interfaces/` — Contratos de service e repository
- `Notificacoes/` — `Notificacao.cs` e `Notificador.cs` (pattern Observer)

### Data
- `Context/` — `ListaDeFilmesContext.cs`
- `Repository/` — Repository genérico base + repositories especializados
- `Mappings/` — Fluent API mappings por entidade
- `Migrations/` — Migrations do EF Core

---

## Padrões Arquiteturais

### Fluxo de uma operação
```
Controller → Service → Repository → DbContext
                ↓
           FluentValidation
                ↓
           Notificador (Observer)
```

- Services validam com FluentValidation e notificam erros via `INotificador`
- Services **não lançam exceções** para erros de negócio — usam `Notificar()`
- Controllers verificam `OperacaoValida()` antes de redirecionar
- Repositories usam `.AsNoTracking()` por padrão em todas as queries

### Repository Pattern
- `IRepository<TEntity>` — interface genérica com CRUD
- `Repository<TEntity>` — implementação base
- `I<Entidade>Repository` / `<Entidade>Repository` — especializações com queries customizadas

### Notificações
```csharp
// No service:
Notificar("Mensagem de erro");

// No controller:
if (!OperacaoValida()) return View(viewModel);
```

### AutoMapper
- Sempre bidirecional com `.ReverseMap()`
- Configurado como `Profile` em `App/AutoMapper/`
- Registrado em `DependencyInjectionConfig`

---

## Convenções de Código

### Nomenclatura
| Tipo | Padrão | Exemplo |
|------|--------|---------|
| Controller | `<Entidade>Controller` | `FilmesController` |
| ViewModel | `<Entidade>ViewModel` | `FilmeViewModel` |
| Service | `<Entidade>Service` | `FilmeService` |
| Interface Service | `I<Entidade>Service` | `IFilmeService` |
| Repository | `<Entidade>Repository` | `FilmeRepository` |
| Interface Repository | `I<Entidade>Repository` | `IFilmeRepository` |
| Validation | `<Entidade>Validation` | `FilmeValidation` |
| Mapping | `<Entidade>Mapping` | `FilmeMapping` |
| Partial View | `_<NomeParcial>` | `_EditarFilme` |
| Campo privado | `_fieldName` | `_filmeRepository` |

### Views CRUD padrão
Toda entidade com CRUD completo terá: `Index`, `Create`, `Edit`, `Delete`, `Details`

---

## Convenções de Banco de Dados

| Item | Padrão | Exemplo |
|------|--------|---------|
| Tabela | `Tb_<Entidade>` | `Tb_Filmes` |
| Chave estrangeira | `Id_<Entidade>` | `Id_Genero` |
| Coluna | `<Campo>_<Entidade>` | `Nome_Filmes`, `Classificacao_Filme` |
| String padrão | `varchar(100)` | — |
| Decimal | `decimal(18,2)` | — |

**Sempre usar NoTracking** nas queries de leitura — o DbContext está configurado com
`QueryTrackingBehavior.NoTracking` como padrão.

---

## Autorização

Autorização **baseada em Claims**, não em Roles.

```csharp
[ClaimsAuthorize("Filmes", "Adicionar")]
public async Task<IActionResult> Create(FilmeViewModel filmeViewModel) { ... }
```

**Recursos atuais:** `Filmes`, `Generos`
**Ações:** `Adicionar`, `Editar`, `Excluir`, `Visualizar`

**TagHelpers de autorização:**
- `ApagaElementoByClaimTagHelper` — remove o elemento do HTML se sem permissão
- `DesabilitaLinkByClaimTagHelper` — desabilita link se sem permissão

---

## Email (Reset de Senha)

Usa **MailKit** via implementação de `IEmailSender` do Identity.

Credenciais configuradas via `appsettings.json` (seção `EmailSettings`) — preencher localmente, nunca commitar valores reais.

`EmailSender` é registrado como `Transient` em `IdentityConfig.cs` (diferente dos services de negócio que são `Scoped`).

---

## Testes

Projeto criado em `src/ListaDeFilmes.Tests` com xUnit + Moq + FluentAssertions.

**Estrutura dos testes:**
```
ListaDeFilmes.Tests/
├── Notificacoes/     — testes do Notificador
├── Services/         — testes de services (repositórios mockados com Moq)
└── Validations/      — testes de FluentValidation diretamente
```

**Padrão de método:** `Metodo_Cenario_ResultadoEsperado`
Exemplo: `AdicionarFilme_FilmeSemNome_DeveNotificarErro`

**Padrão de classe de teste (Services):**
```csharp
public class FilmeServiceTests
{
    private readonly Mock<IFilmeRepository> _filmeRepositoryMock = new();
    private readonly Notificador _notificador = new();
    private readonly FilmeService _service;

    public FilmeServiceTests()
    {
        _service = new FilmeService(_filmeRepositoryMock.Object, _notificador);
    }
}
```

---

## Checklist: Como Adicionar uma Nova Entidade

Siga esta ordem ao implementar uma nova entidade completa:

**Business:**
- [ ] `Models/NovaEntidade.cs` — herdar de `Entity.cs` (Id é `Guid`)
- [ ] `Interfaces/INovaEntidadeRepository.cs`
- [ ] `Interfaces/INovaEntidadeService.cs`
- [ ] `Services/NovaEntidadeService.cs` — herdar de `BaseService`
- [ ] `Models/Validations/NovaEntidadeValidation.cs` — FluentValidation

**Data:**
- [ ] `Mappings/NovaEntidadeMapping.cs` — Fluent API (seguir convenção de nomes de tabela/coluna)
- [ ] `Repository/NovaEntidadeRepository.cs`
- [ ] Adicionar `DbSet<NovaEntidade>` em `ListaDeFilmesContext.cs`
- [ ] `dotnet ef migrations add` + `dotnet ef database update`

**App:**
- [ ] `ViewModels/NovaEntidadeViewModel.cs`
- [ ] AutoMapper profile com `.ReverseMap()`
- [ ] `Controllers/NovaEntidadeController.cs` com `[ClaimsAuthorize]`
- [ ] Views: `Index`, `Create`, `Edit`, `Delete`, `Details`
- [ ] Registrar Service e Repository como `Scoped` em `DependencyInjectionConfig`

---

## Git

- **Branch por feature:** `feat/<descricao-curta>`
- **Commits:** Conventional Commits em português
  - `feat: adiciona crud de produtoras`
  - `fix: corrige validação de ano do filme`
  - `refactor: extrai lógica de upload de imagem para service`

---

## TODOs Conhecidos

- `DiretorId` e `ProdutoraId` em `Filme.cs` estão nullable (`?`) temporariamente — tornar obrigatórios quando os CRUDs de Diretor e Produtora forem finalizados
- Implementar CRUD completo de Diretor e Produtora
- Configurar pipelines CI/CD em `.github/workflows/`
