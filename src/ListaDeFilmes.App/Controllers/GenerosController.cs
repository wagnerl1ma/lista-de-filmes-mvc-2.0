using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ListaDeFilmes.App.Data;
using ListaDeFilmes.App.ViewModels;
using ListaDeFilmes.Business.Interfaces;
using AutoMapper;
using ListaDeFilmes.Business.Models;
using Microsoft.AspNetCore.Authorization;
using ListaDeFilmes.App.Extensions;

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

        [AllowAnonymous]
        [Route("lista-de-generos")]
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<GeneroViewModel>>(await _generoRepository.ObterTodos()));
           // return View(await _generoRepository.ObterTodos());
        }

        [AllowAnonymous]
        [Route("detalhes-do-genero/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var generoViewModel = _mapper.Map<GeneroViewModel>(await _generoRepository.ObterPorId(id));
            //var generoViewModel = await _generoRepository.ObterPorId(id);

            if (generoViewModel == null)
            {
                return NotFound();
            }

            return View(generoViewModel);
        }

        [AllowAnonymous]
        [Route("detalhes-do-genero-modal/{id:guid}")]
        public async Task<IActionResult> DetailsModal(Guid id)
        {
            var generoViewModel = _mapper.Map<GeneroViewModel>(await _generoRepository.ObterPorId(id));
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
        public async Task<IActionResult> Create(GeneroViewModel generoViewModel)
        {
            if (ModelState.IsValid)
            {
                var genero = _mapper.Map<Genero>(generoViewModel);
                await _generoService.Adicionar(genero);

            }

            if (!OperacaoValida())
            {
                return View(generoViewModel);
            }

            return RedirectToAction(nameof(Index));
        }

        [ClaimsAuthorize("Generos", "Editar")]
        [Route("editar-genero/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var generoViewModel = _mapper.Map<GeneroViewModel>(await _generoRepository.ObterPorId(id));
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
        public async Task<IActionResult> Edit(Guid id, GeneroViewModel generoViewModel)
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
            await _generoService.Atualizar(genero);

            if (!OperacaoValida())
            {
                return View(generoViewModel);
            }

            return RedirectToAction(nameof(Index));
        }

        [ClaimsAuthorize("Generos", "Excluir")]
        [Route("excluir-genero/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            //var generoViewModel = await _generoRepository.ObterPorId(id);
            var generoViewModel = _mapper.Map<GeneroViewModel>(await _generoRepository.ObterPorId(id));

            if (generoViewModel == null)
            {
                return NotFound();
            }

            return View(generoViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ClaimsAuthorize("Generos", "Excluir")]
        [Route("excluir-genero/{id:guid}")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var generoViewModel = await _generoRepository.ObterPorId(id);

            if (generoViewModel == null)
            {
                return NotFound();
            }

            await _generoService.Remover(id);

            if (!OperacaoValida())
            {
                return View(generoViewModel);
            }

            TempData["Sucesso"] = "Gênero excluido com sucesso!";

            return RedirectToAction(nameof(Index));
        }
    }
}
