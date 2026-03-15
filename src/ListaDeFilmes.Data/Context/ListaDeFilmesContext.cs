using ListaDeFilmes.Business.Models;
using Microsoft.EntityFrameworkCore;

namespace ListaDeFilmes.Data.Context
{
    public class ListaDeFilmesContext : DbContext
    {
        public ListaDeFilmesContext(DbContextOptions options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<Ator> Atores { get; set; }
        public DbSet<Diretor> Diretores { get; set; }
        public DbSet<Filme> Filmes { get; set; }
        public DbSet<FilmeAtor> FilmesAtores { get; set; }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Produtora> Produtoras { get; set; }


        //Apenas para STRING: caso esquecer de passar o tamanho da string nas mappings/configurations, o código abaixo irá criar o campo no banco de dados com o tamanho "varchar(100)"
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<string>()
                .HaveColumnType("varchar(100)");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //registra todas as mappings/configurations de uma vez só, ao inves de escrever uma por uma
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ListaDeFilmesContext).Assembly);

            //Impedindo a exclusão por cascata
            //foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            base.OnModelCreating(modelBuilder);
        }
    }
}
