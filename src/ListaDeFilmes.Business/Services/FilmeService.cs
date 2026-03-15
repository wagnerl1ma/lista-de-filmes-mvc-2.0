using ListaDeFilmes.Business.Interfaces;
using ListaDeFilmes.Business.Models;
using ListaDeFilmes.Business.Models.Validations;

namespace ListaDeFilmes.Business.Services
{
    public class FilmeService : BaseService, IFilmeService
    {
        private readonly IFilmeRepository _filmeRepository;
        private readonly IGeneroRepository _generoRepository;

        public FilmeService(IFilmeRepository filmeRepository, IGeneroRepository generoRepository, INotificador notificador) : base(notificador)
        {
            _filmeRepository = filmeRepository;
            _generoRepository = generoRepository;
        }

        public async Task Adicionar(Filme filme, CancellationToken ct = default)
        {
            //se a Validação não for valida, retorna a notificação e nao faz a adição
            if (!ExecutarValidacao(new FilmeValidation(), filme)) return;

            filme.DataCadastro = DateTime.Now;

            await _filmeRepository.Adicionar(filme, ct);
            await _filmeRepository.SaveChanges(ct);
        }

        public async Task Atualizar(Filme filme, CancellationToken ct = default)
        {
            if (!ExecutarValidacao(new FilmeValidation(), filme)) return;

            if (filme.Genero == null || !ExecutarValidacao(new GeneroValidation(), filme.Genero)) return;

            filme.Genero = await _generoRepository.ObterPorId(filme.GeneroId, ct);

            await _filmeRepository.Atualizar(filme, ct);
            await _filmeRepository.SaveChanges(ct);
        }

        public async Task Remover(Guid id, CancellationToken ct = default)
        {
            await _filmeRepository.Remover(id, ct);
            await _filmeRepository.SaveChanges(ct);
        }

        //public async Task<Filme> ObterFilmePreenchido(Guid id)
        //{
        //    var filme = await _filmeRepository.ObterFilmeGenero(id);
        //    filme.Genero = await _generoRepository.ObterPorId(filme.GeneroId);

        //    return filme;
        //}

        public async ValueTask DisposeAsync()
        {
            //? = Se ele existir faça o Dispose, se nao exister não faça
            if (_filmeRepository != null) await _filmeRepository.DisposeAsync();
            if (_generoRepository != null) await _generoRepository.DisposeAsync();
        }
    }
}
