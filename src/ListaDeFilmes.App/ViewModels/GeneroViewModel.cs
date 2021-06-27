using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ListaDeFilmes.App.ViewModels
{
    public class GeneroViewModel
    {
        [Key]
        public Guid Id { get; set; }

        public string Nome { get; set; }

        public ICollection<FilmeViewModel> Filmes { get; set; } = new List<FilmeViewModel>();   //= O new List<Filme> garante que a lista seja instanciada  //Relacionamento: um genero tem em vários filmes


        //public GeneroViewModel()
        //{

        //}

        //public GeneroViewModel(string nome)
        //{
        //    Nome = nome;
        //}

        //public GeneroViewModel(Guid id, string nome)
        //{
        //    Id = id;
        //    Nome = nome;
        //}

        //public void AddFilme(FilmeViewModel filme)
        //{
        //    Filmes.Add(filme);
        //}
    }
}
