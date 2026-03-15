using System.Linq.Expressions;
using ListaDeFilmes.Business.Models;

namespace ListaDeFilmes.Business.Interfaces
{
    public interface IRepository<TEntity> : IAsyncDisposable where TEntity : Entity         //IDisposable serve para liberar memória
    {
        Task Adicionar(TEntity entity, CancellationToken ct = default);

        Task<TEntity?> ObterPorId(Guid id, CancellationToken ct = default);

        Task<List<TEntity>> ObterTodos(CancellationToken ct = default);

        Task Atualizar(TEntity entity, CancellationToken ct = default);

        Task Remover(Guid id, CancellationToken ct = default);

        Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default);  //busca por qualquer parametro atraves de expressões lambda

        Task<int> SaveChanges(CancellationToken ct = default);  // int porque retorna o numero de linhas afetadas

        Task<bool> IdExiste(Guid id, CancellationToken ct = default); //verifica se o id existe

    }
}
