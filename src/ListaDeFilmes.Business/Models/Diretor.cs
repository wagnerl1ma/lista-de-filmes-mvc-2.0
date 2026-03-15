namespace ListaDeFilmes.Business.Models
{
    public class Diretor : Entity
    {
        public string Nome { get; set; } = string.Empty;

        public ICollection<Filme> Filmes { get; set; } = new List<Filme>();

        //public Diretor()
        //{

        //}

        //public Diretor(string nome)
        //{
        //    Nome = nome;
        //}

        //public Diretor(Guid id, string nome)
        //{
        //    Id = id;
        //    Nome = nome;
        //}
    }
}