using ListaDeFilmes.Business.Models;

namespace ListaDeFilmes.Business.Interfaces
{
    public interface IFilmeService : IAsyncDisposable
    {
        Task Adicionar(Filme fornecedor, CancellationToken ct = default);
        Task Atualizar(Filme fornecedor, CancellationToken ct = default);
        Task Remover(Guid id, CancellationToken ct = default);
        //Task<Filme> ObterFilmePreenchido(Guid id);
    }
}
