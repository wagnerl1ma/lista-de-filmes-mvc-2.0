using ListaDeFilmes.Business.Interfaces;
using ListaDeFilmes.Business.Models;
using ListaDeFilmes.Business.Models.Validations;

namespace ListaDeFilmes.Business.Services
{
    public class GeneroService : BaseService, IGeneroService
    {
        private readonly IGeneroRepository _generoRepository;

        public GeneroService(IGeneroRepository generoRepository, INotificador notificador) : base(notificador)
        {
            _generoRepository = generoRepository;
        }
        public async Task Adicionar(Genero genero, CancellationToken ct = default)
        {
            //se a Validação não for valida, retorna a notificação e nao faz a adição
            if (!ExecutarValidacao(new GeneroValidation(), genero)) return;

            await _generoRepository.Adicionar(genero, ct);
            await _generoRepository.SaveChanges(ct);
        }

        public async Task Atualizar(Genero genero, CancellationToken ct = default)
        {
            //se a Validação não for valida, retorna e nao faz a atualização
            if (!ExecutarValidacao(new GeneroValidation(), genero)) return;

            await _generoRepository.Atualizar(genero, ct);
            await _generoRepository.SaveChanges(ct);
        }

        public async Task Remover(Guid id, CancellationToken ct = default)
        {
            await _generoRepository.Remover(id, ct);
            await _generoRepository.SaveChanges(ct);
        }

        public async ValueTask DisposeAsync()
        {
            //? = Se ele existir faça o Dispose, se nao exister não faça
            if (_generoRepository != null) await _generoRepository.DisposeAsync();
        }
    }
}
