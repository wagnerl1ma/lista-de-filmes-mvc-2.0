using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ListaDeFilmes.App.ViewModels
{
    public class AtorViewModel
    {
        [Key]
        public Guid Id { get; set; }

        public string Nome { get; set; }

        // Um Ator pode ter vários Filmes (Relacionamneto muitos para muitos)
        public ICollection<FilmeAtorViewModel> FilmesAtores { get; set; } //= new List<FilmeAtor>();     

        //public AtorViewModel()
        //{

        //}

        //public AtorViewModel(string nome)
        //{
        //    Nome = nome;
        //}

        //public AtorViewModel(Guid id, string nome)
        //{
        //    Id = id;
        //    Nome = nome;
        //}
    }
}
