using ListaDeFilmes.Business.Models;

namespace ListaDeFilmes.Business.Interfaces
{
    public interface IGeneroRepository : IRepository<Genero>
    {
        Task<List<Genero>> GetGenerosAsync(CancellationToken ct = default);
    }
}
