using ListaDeFilmes.Business.Interfaces;
using ListaDeFilmes.Business.Models;
using ListaDeFilmes.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ListaDeFilmes.Data.Repository
{
    //abstract = nao pode ser instanciada, só posso usa-la se for Herdada
    //Entity, new() = Significa que eu posso instanciar essa Entidade
    //todos os métodos estão como virtual para usar o override(sobescrever) quando for necessário.
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
    {
        protected readonly ListaDeFilmesContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;

        protected Repository(ListaDeFilmesContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
        {
            //AsNoTracking é ideal para consulta, o mesmo não faz o rastreio de todo o objeto, sendo assim é mais performatico
            return await _dbSet.AsNoTracking().Where(predicate).ToListAsync(ct);
        }

        public virtual async Task<TEntity?> ObterPorId(Guid id, CancellationToken ct = default)
        {
            return await _dbSet.FindAsync(new object[] { id }, ct);
        }

        public virtual async Task<List<TEntity>> ObterTodos(CancellationToken ct = default)
        {
            return await _dbSet.ToListAsync(ct);
        }

        public virtual Task Adicionar(TEntity entity, CancellationToken ct = default)
        {
            _dbSet.Add(entity);
            return Task.CompletedTask;
        }

        public virtual Task Atualizar(TEntity entity, CancellationToken ct = default)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }

        public virtual Task Remover(Guid id, CancellationToken ct = default)
        {
            //Remove do banco sem precisar fazer a busca no banco.
            //ex: Não é necessário usar o metodo Find para encontrar o id, e devolver o objeto para remover. Com o "new TEntity" você passa somente o Id para remover.
            // E isso é possível porque todo mundo herda de TEntity

            _dbSet.Remove(new TEntity { Id = id });
            return Task.CompletedTask;
        }

        public async Task<int> SaveChanges(CancellationToken ct = default)
        {
            return await _dbContext.SaveChangesAsync(ct);
        }

        public virtual async Task<bool> IdExiste(Guid id, CancellationToken ct = default)
        {
            return await _dbSet.AnyAsync(x => x.Id == id, ct);
        }

        public async ValueTask DisposeAsync()
        {
            //IAsyncDisposable serve para liberar memória de forma assíncrona
            if (_dbContext != null)
                await _dbContext.DisposeAsync();
        }
    }
}
