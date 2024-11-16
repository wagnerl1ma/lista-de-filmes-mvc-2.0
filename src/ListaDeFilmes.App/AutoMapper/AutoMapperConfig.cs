using AutoMapper;
using ListaDeFilmes.App.ViewModels;
using ListaDeFilmes.Business.Models;

namespace ListaDeFilmes.App.AutoMapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Filme, FilmeViewModel>().ReverseMap();
            CreateMap<Genero, GeneroViewModel>().ReverseMap();
            CreateMap<Diretor, DiretorViewModel>().ReverseMap();
            CreateMap<Ator, AtorViewModel>().ReverseMap();
            CreateMap<Produtora, ProdutoraViewModel>().ReverseMap();
            CreateMap<FilmeAtor, FilmeAtorViewModel>().ReverseMap();
        }
    }
}
