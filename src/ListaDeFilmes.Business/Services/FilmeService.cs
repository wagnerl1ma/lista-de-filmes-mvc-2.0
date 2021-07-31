using ListaDeFilmes.Business.Interfaces;
using ListaDeFilmes.Business.Models;
using ListaDeFilmes.Business.Models.Validations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ListaDeFilmes.Business.Services
{
    public class FilmeService : BaseService, IFilmeService
    {
        private readonly IFilmeRepository _filmeRepository;

        public FilmeService(IFilmeRepository filmeRepository, INotificador notificador) : base(notificador)
        {
            _filmeRepository = filmeRepository;
        }

        public async Task Adicionar(Filme filme)
        {
            //se a Validação não for valida, retorna a notificação e nao faz a adição
            if (!ExecutarValidacao(new FilmeValidation(), filme)
                || !ExecutarValidacao(new GeneroValidation(), filme.Genero)) return;

            await _filmeRepository.Adicionar(filme);
        }

        public Task Atualizar(Filme filme)
        {
            throw new NotImplementedException();
        }

        public Task AtualizarGenero(Genero genero)
        {
            throw new NotImplementedException();
        }

        public Task Remover(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
