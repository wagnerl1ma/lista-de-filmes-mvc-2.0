using ListaDeFilmes.Business.Models;

namespace ListaDeFilmes.Business.Interfaces
{
    public interface IGeneroService : IAsyncDisposable
    {
        Task Adicionar(Genero fornecedor, CancellationToken ct = default);
        Task Atualizar(Genero fornecedor, CancellationToken ct = default);
        Task Remover(Guid id, CancellationToken ct = default);

        //Task AtualizarGenero(Genero genero);
    }
}
