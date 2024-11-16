using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ListaDeFilmes.App.ViewModels
{
    public class DiretorViewModel
    {
        [Key]
        public Guid Id { get; set; }
        public string Nome { get; set; }

        public ICollection<FilmeViewModel> Filmes { get; set; }

        //public DiretorViewModel()
        //{

        //}

        //public DiretorViewModel(string nome)
        //{
        //    Nome = nome;
        //}

        //public DiretorViewModel(Guid id, string nome)
        //{
        //    Id = id;
        //    Nome = nome;
        //}
    }
}
