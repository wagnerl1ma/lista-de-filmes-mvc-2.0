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

namespace ListaDeFilmes.App.Controllers
{
    public class GenerosController : BaseController
    {
        private readonly IGeneroRepository _generoRepository;
        private readonly IMapper _mapper;

        public GenerosController(IGeneroRepository generoRepository, IMapper mapper)
        {
            _generoRepository = generoRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<GeneroViewModel>>(await _generoRepository.ObterTodos()));
           // return View(await _generoRepository.ObterTodos());
        }

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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GeneroViewModel generoViewModel)
        {
            if (ModelState.IsValid)
            {
                var genero = _mapper.Map<Genero>(generoViewModel);
                await _generoRepository.Adicionar(genero);

                return RedirectToAction(nameof(Index));
            }

            return View(generoViewModel);
        }

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
        [ValidateAntiForgeryToken]
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
            await _generoRepository.Atualizar(genero);

            return RedirectToAction(nameof(Index));
        }

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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var generoViewModel = await _generoRepository.ObterPorId(id);

            if (generoViewModel == null)
            {
                return NotFound();
            }

            await _generoRepository.Remover(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
