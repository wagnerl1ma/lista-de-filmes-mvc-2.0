using FluentAssertions;
using ListaDeFilmes.Business.Interfaces;
using ListaDeFilmes.Business.Models;
using ListaDeFilmes.Business.Notificacoes;
using ListaDeFilmes.Business.Services;
using Moq;

namespace ListaDeFilmes.Tests.Services
{
    public class FilmeServiceTests
    {
        private readonly Mock<IFilmeRepository> _filmeRepositoryMock = new();
        private readonly Mock<IGeneroRepository> _generoRepositoryMock = new();
        private readonly Notificador _notificador = new();
        private readonly FilmeService _service;

        public FilmeServiceTests()
        {
            _service = new FilmeService(_filmeRepositoryMock.Object, _generoRepositoryMock.Object, _notificador);
        }

        private static Filme CriarFilmeValido() => new()
        {
            Nome = "Matrix",
            Classificacao = "14",
            Comentarios = "Filme de ficao cientifica",
            Valor = 29.90m,
            Ativo = true
        };

        [Fact]
        public async Task AdicionarFilme_FilmeValido_DeveChamarRepositorio()
        {
            var filme = CriarFilmeValido();

            await _service.Adicionar(filme);

            _filmeRepositoryMock.Verify(r => r.Adicionar(filme, It.IsAny<CancellationToken>()), Times.Once);
            _filmeRepositoryMock.Verify(r => r.SaveChanges(It.IsAny<CancellationToken>()), Times.Once);
            _notificador.TemNotificacao().Should().BeFalse();
        }

        [Fact]
        public async Task AdicionarFilme_FilmeInvalido_DeveNotificarErro()
        {
            var filme = CriarFilmeValido();
            filme.Nome = string.Empty;

            await _service.Adicionar(filme);

            _filmeRepositoryMock.Verify(r => r.Adicionar(It.IsAny<Filme>(), It.IsAny<CancellationToken>()), Times.Never);
            _notificador.TemNotificacao().Should().BeTrue();
        }

        [Fact]
        public async Task AdicionarFilme_FilmeValido_DevePreencherDataCadastro()
        {
            var filme = CriarFilmeValido();

            await _service.Adicionar(filme);

            filme.DataCadastro.Should().NotBeNull();
            filme.DataCadastro!.Value.Date.Should().Be(DateTime.Now.Date);
        }

        [Fact]
        public async Task RemoverFilme_IdExistente_DeveChamarRepositorio()
        {
            var id = Guid.NewGuid();

            await _service.Remover(id);

            _filmeRepositoryMock.Verify(r => r.Remover(id, It.IsAny<CancellationToken>()), Times.Once);
            _filmeRepositoryMock.Verify(r => r.SaveChanges(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
