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

        // GET: Generos
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<GeneroViewModel>>(await _generoRepository.ObterTodos()));
           // return View(await _generoRepository.ObterTodos());
        }

        // GET: Generos/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var generoViewModel = await _generoRepository.ObterPorId(id);

            if (generoViewModel == null)
            {
                return NotFound();
            }

            return View(generoViewModel);
        }

        // GET: Generos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Generos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Generos/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var generoViewModel = await _generoRepository.ObterPorId(id);

            if (generoViewModel == null)
            {
                return NotFound();
            }

            return View(generoViewModel);
        }

        // POST: Generos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Generos/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            var generoViewModel = await _generoRepository.ObterPorId(id);

            if (generoViewModel == null)
            {
                return NotFound();
            }

            return View(generoViewModel);
        }

        // POST: Generos/Delete/5
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
