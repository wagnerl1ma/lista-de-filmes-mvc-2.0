using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ListaDeFilmes.App.ViewModels
{
    public class FilmeAtorViewModel
    {
        public FilmeViewModel Filme { get; set; }
        public Guid FilmeId { get; set; }

        public AtorViewModel Ator { get; set; }
        public Guid AtorId { get; set; }

        //public FilmeAtorViewModel()
        //{

        //}

        //public FilmeAtorViewModel(FilmeViewModel filme, AtorViewModel ator)
        //{
        //    Filme = filme;
        //    Ator = ator;
        //}

        //public FilmeAtorViewModel(Guid filmeId, FilmeViewModel filme, Guid atorId, AtorViewModel ator)
        //{
        //    FilmeId = filmeId;
        //    Filme = filme;
        //    AtorId = atorId;
        //    Ator = ator;
        //}
    }
}
