using ListaDeFilmes.Business.Interfaces;
using ListaDeFilmes.Business.Models;
using ListaDeFilmes.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ListaDeFilmes.Data.Repository
{
    public class FilmeRepository : Repository<Filme>, IFilmeRepository
    {
        public FilmeRepository(ListaDeFilmesContext context) : base(context) { }

        public async Task<Filme?> ObterFilmeGenero(Guid id, CancellationToken ct = default)
        {
            return await _dbContext.Filmes.AsNoTracking().Include(g => g.Genero).FirstOrDefaultAsync(f => f.Id == id, ct);
        }

        public async Task<List<Filme>> ObterFilmesGeneros(CancellationToken ct = default)
        {
            return await _dbContext.Filmes.AsNoTracking().Include(x => x.Genero).ToListAsync(ct); //instanciando o Genero junto com a lista de filmes
        }

        public async Task<List<Filme>> PesquisaNomeFilme(string nomeFilme, CancellationToken ct = default)
        {
            var query = _dbContext.Filmes.AsNoTracking()
               .Where(x => x.Nome.Contains(nomeFilme))
               .Include(x => x.Genero) //join
               .OrderBy(x => x.Nome);

            return await query.ToListAsync(ct);
        }
    }
}
