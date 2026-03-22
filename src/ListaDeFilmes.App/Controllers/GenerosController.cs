using AutoMapper;
using ListaDeFilmes.App.Extensions;
using ListaDeFilmes.App.ViewModels;
using ListaDeFilmes.Business.Interfaces;
using ListaDeFilmes.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ListaDeFilmes.App.Controllers
{
    [Authorize]
    public class GenerosController : BaseController
    {
        private readonly IGeneroRepository _generoRepository;
        private readonly IGeneroService _generoService;
        private readonly IMapper _mapper;

        public GenerosController(IGeneroRepository generoRepository,
                                 IGeneroService generoService,
                                 IMapper mapper,
                                 INotificador notificador) : base(notificador)
        {
            _generoRepository = generoRepository;
            _generoService = generoService;
            _mapper = mapper;
        }

        [Route("lista-de-generos")]
        public async Task<IActionResult> Index(CancellationToken ct = default)
        {
            return View(_mapper.Map<IEnumerable<GeneroViewModel>>(await _generoRepository.ObterTodos(ct)));
           // return View(await _generoRepository.ObterTodos());
        }

        [Route("detalhes-do-genero/{id:guid}")]
        public async Task<IActionResult> Details(Guid id, CancellationToken ct = default)
        {
            var generoViewModel = _mapper.Map<GeneroViewModel>(await _generoRepository.ObterPorId(id, ct));
            //var generoViewModel = await _generoRepository.ObterPorId(id);

            if (generoViewModel == null)
            {
                return NotFound();
            }

            return View(generoViewModel);
        }

        [Route("detalhes-do-genero-modal/{id:guid}")]
        public async Task<IActionResult> DetailsModal(Guid id, CancellationToken ct = default)
        {
            var generoViewModel = _mapper.Map<GeneroViewModel>(await _generoRepository.ObterPorId(id, ct));
            //var generoViewModel = await _generoRepository.ObterPorId(id);

            if (generoViewModel == null)
            {
                return NotFound();
            }

            return PartialView("_DetailsModal", generoViewModel);
        }

        [ClaimsAuthorize("Generos", "Adicionar")]
        [Route("novo-genero")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ClaimsAuthorize("Generos", "Adicionar")]
        [Route("novo-genero")]
        public async Task<IActionResult> Create(GeneroViewModel generoViewModel, CancellationToken ct = default)
        {
            if (ModelState.IsValid)
            {
                var genero = _mapper.Map<Genero>(generoViewModel);
                await _generoService.Adicionar(genero, ct);

            }

            if (!OperacaoValida())
            {
                return View(generoViewModel);
            }

            return RedirectToAction(nameof(Index));
        }

        [ClaimsAuthorize("Generos", "Editar")]
        [Route("editar-genero/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, CancellationToken ct = default)
        {
            var generoViewModel = _mapper.Map<GeneroViewModel>(await _generoRepository.ObterPorId(id, ct));
            //var generoViewModel = await _generoRepository.ObterPorId(id);

            if (generoViewModel == null)
            {
                return NotFound();
            }

            return View(generoViewModel);
        }

        [HttpPost]
        [ClaimsAuthorize("Generos", "Editar")]
        [Route("editar-genero/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, GeneroViewModel generoViewModel, CancellationToken ct = default)
        {
            if (id != generoViewModel.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(generoViewModel);
            }

            var genero = _mapper.Map<Genero>(generoViewModel);
            await _generoService.Atualizar(genero, ct);

            if (!OperacaoValida())
            {
                return View(generoViewModel);
            }

            return RedirectToAction(nameof(Index));
        }

        [ClaimsAuthorize("Generos", "Excluir")]
        [Route("excluir-genero/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct = default)
        {
            //var generoViewModel = await _generoRepository.ObterPorId(id);
            var generoViewModel = _mapper.Map<GeneroViewModel>(await _generoRepository.ObterPorId(id, ct));

            if (generoViewModel == null)
            {
                return NotFound();
            }

            return View(generoViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ClaimsAuthorize("Generos", "Excluir")]
        [Route("excluir-genero/{id:guid}")]
        public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken ct = default)
        {
            var generoViewModel = await _generoRepository.ObterPorId(id, ct);

            if (generoViewModel == null)
            {
                return NotFound();
            }

            await _generoService.Remover(id, ct);

            if (!OperacaoValida())
            {
                return View(generoViewModel);
            }

            TempData["Sucesso"] = "Gênero excluido com sucesso!";

            return RedirectToAction(nameof(Index));
        }
    }
}
