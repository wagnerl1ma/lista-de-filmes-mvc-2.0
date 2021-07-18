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
using Microsoft.AspNetCore.Http;
using System.IO;

namespace ListaDeFilmes.App.Controllers
{
    public class FilmesController : BaseController
    {
        private readonly IFilmeRepository _filmeRepository;
        private readonly IGeneroRepository _generoRepository;
        private readonly IMapper _mapper;

        public FilmesController(IFilmeRepository filmeRepository, IGeneroRepository generoRepository, IMapper mapper)
        {
            _filmeRepository = filmeRepository;
            _generoRepository = generoRepository;
            _mapper = mapper;
        }

        // GET: Filmes
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<FilmeViewModel>>(await _filmeRepository.ObterFilmesGeneros()));
        }

        // GET: Filmes/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var filmeViewModel = await ObterFilmePreenchido(id);

            if (filmeViewModel == null)
            {
                return NotFound();
            }

            return View(filmeViewModel);
        }

        // GET: Filmes/Create
        public async Task<IActionResult> Create()
        {
            var filmeViewModel = await PopularGeneros(new FilmeViewModel());
            return View(filmeViewModel);
        }

        // POST: Filmes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FilmeViewModel filmeViewModel)
        {
            filmeViewModel = await PopularGeneros(filmeViewModel);

            if (!ModelState.IsValid)
            {
                return View(filmeViewModel);
            }

            var imgPrefixo = Guid.NewGuid() + "_";

            if (!await UploadArquivo(filmeViewModel.ImagemUpload, imgPrefixo))
            {
                return View(filmeViewModel);
            }

            filmeViewModel.Imagem = imgPrefixo + filmeViewModel.ImagemUpload.FileName;

            await _filmeRepository.Adicionar(_mapper.Map<Filme>(filmeViewModel));

            //if (!OperacaoValida())
            //{
            //    return View(filmeViewModel);
            //}

            return RedirectToAction(nameof(Index));
        }

        //GET: Filmes/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var filmeViewModel = await ObterFilmePreenchido(id);

            if (id == null)
            {
                return NotFound();
            }
            
            return View(filmeViewModel);
        }

        //POST: Filmes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, FilmeViewModel filmeViewModel)
        {
            if (id != filmeViewModel.Id)
            {
                return NotFound();
            }

            var filmeAtualizacao = await ObterFilmePreenchido(id);

            filmeViewModel.Genero = filmeAtualizacao.Genero;
            filmeViewModel.Imagem = filmeAtualizacao.Imagem;

            if (!ModelState.IsValid)
            {
                return View(filmeViewModel); 
            }

            if (filmeViewModel.ImagemUpload != null)
            {
                var imgPrefixo = Guid.NewGuid() + "_";
                if (!await UploadArquivo(filmeViewModel.ImagemUpload, imgPrefixo))
                {
                    return View(filmeViewModel);
                }

                filmeAtualizacao.Imagem = imgPrefixo + filmeViewModel.ImagemUpload.FileName;
            }

            filmeAtualizacao.Nome = filmeViewModel.Nome;
            filmeAtualizacao.Classificacao = filmeViewModel.Classificacao;
            filmeAtualizacao.Ano = filmeViewModel.Ano;
            //filmeAtualizacao.Comentarios = filmeViewModel.Comentarios;
            filmeAtualizacao.Valor = filmeViewModel.Valor;
            filmeAtualizacao.Ativo = filmeViewModel.Ativo;


            await _filmeRepository.Atualizar(_mapper.Map<Filme>(filmeAtualizacao));

            //if (!OperacaoValida())
            //{
            //    return View(filmeViewModel);
            //}

            return RedirectToAction(nameof(Index));
        }

        //GET: Filmes/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            var filme = await ObterFilmePreenchido(id);

            if (filme == null)
            {
                return NotFound();
            }

            return View(filme);
        }

        //POST: Filmes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var filme = await ObterFilmePreenchido(id);

            if (filme == null)
            {
                return NotFound();
            }

            await _filmeRepository.Remover(id);

            //if (!OperacaoValida())
            //{
            //    return View(filme);
            //}

            //TempData["Sucesso"] = "Produto excluido com sucesso!";

            return RedirectToAction(nameof(Index));
        }


        private async Task<FilmeViewModel> ObterFilmePreenchido(Guid id)
        {
            var produto = _mapper.Map<FilmeViewModel>(await _filmeRepository.ObterFilmeGenero(id));
            produto.Generos = _mapper.Map<IEnumerable<GeneroViewModel>>(await _generoRepository.ObterTodos());

            return produto;
        }

        private async Task<FilmeViewModel> PopularGeneros(FilmeViewModel filme)
        {
            filme.Generos = _mapper.Map<IEnumerable<GeneroViewModel>>(await _generoRepository.ObterTodos());
            return filme;
        }

        private async Task<bool> UploadArquivo(IFormFile arquivo, string imgPrefixo)
        {
            if (arquivo.Length <= 0) return false;

            //irá gravar a imagem nesse caminho
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", imgPrefixo + arquivo.FileName);

            if (System.IO.File.Exists(path))
            {
                ModelState.AddModelError(string.Empty, "Já existe um arquivo com este nome!");
                return false;
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            return true;
        }
    }
}
