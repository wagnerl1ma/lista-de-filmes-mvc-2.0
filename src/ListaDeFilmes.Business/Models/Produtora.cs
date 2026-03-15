namespace ListaDeFilmes.Business.Models
{
    public class Produtora : Entity
    {
        public string Nome { get; set; } = string.Empty;

        public List<Filme> Filmes { get; set; } = new List<Filme>();

        //public Produtora()
        //{

        //}

        //public Produtora(string nome)
        //{
        //    Nome = nome;
        //}

        //public Produtora(Guid id, string nome)
        //{
        //    Id = id;
        //    Nome = nome;
        //}
    }
}