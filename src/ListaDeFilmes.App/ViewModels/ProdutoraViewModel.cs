using System.ComponentModel.DataAnnotations;

namespace ListaDeFilmes.App.ViewModels
{
    public class ProdutoraViewModel
    {
        [Key]
        public Guid Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public List<FilmeViewModel> Filmes { get; set; } = new List<FilmeViewModel>();

        //public ProdutoraViewModel()
        //{

        //}

        //public ProdutoraViewModel(string nome)
        //{
        //    Nome = nome;
        //}

        //public ProdutoraViewModel(Guid id, string nome)
        //{
        //    Id = id;
        //    Nome = nome;
        //}
    }
}
