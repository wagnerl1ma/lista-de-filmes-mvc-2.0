using ListaDeFilmes.Business.Interfaces;
using ListaDeFilmes.Business.Models;
using ListaDeFilmes.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ListaDeFilmes.Data.Repository
{
    public class GeneroRepository : Repository<Genero>, IGeneroRepository
    {
        public GeneroRepository(ListaDeFilmesContext context) : base(context) { }

        public async Task<List<Genero>> GetGenerosAsync(CancellationToken ct = default)
        {
            return await _dbContext.Generos.AsNoTracking().OrderBy(x => x.Nome).ToListAsync(ct);
        }
    }
}
