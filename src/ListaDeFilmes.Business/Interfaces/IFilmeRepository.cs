using ListaDeFilmes.Business.Models;

namespace ListaDeFilmes.Business.Interfaces
{
    public interface IFilmeRepository : IRepository<Filme>
    {
        Task<List<Filme>> PesquisaNomeFilme(string nomeFilme, CancellationToken ct = default);

        Task<List<Filme>> ObterFilmesGeneros(CancellationToken ct = default);

        Task<Filme?> ObterFilmeGenero(Guid id, CancellationToken ct = default);
    }
}
