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
using Microsoft.AspNetCore.Authorization;
using ListaDeFilmes.App.Extensions;

namespace ListaDeFilmes.App.Controllers
{
    [Authorize]
    public class FilmesController : BaseController
    {
        private readonly IFilmeRepository _filmeRepository;
        private readonly IFilmeService _filmeService;
        private readonly IGeneroRepository _generoRepository;
        private readonly IMapper _mapper;

        public FilmesController(IFilmeRepository filmeRepository,
                                IGeneroRepository generoRepository,
                                IFilmeService filmeService,
                                IMapper mapper,
                                INotificador notificador) : base(notificador)
        {
            _filmeRepository = filmeRepository;
            _generoRepository = generoRepository;
            _filmeService = filmeService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [Route("lista-de-filmes")]
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<FilmeViewModel>>(await _filmeRepository.ObterFilmesGeneros()));
        }

        [AllowAnonymous]
        [Route("detalhes-do-filme/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var filmeViewModel = await ObterFilmePreenchido(id);

            if (filmeViewModel == null)
            {
                return NotFound();
            }

            return View(filmeViewModel);
        }

        [AllowAnonymous]
        [Route("detalhes-do-filme-modal/{id:guid}")]
        public async Task<IActionResult> DetailsModal(Guid id)
        {
            var filmeViewModel = await ObterFilmePreenchido(id);

            if (filmeViewModel == null)
            {
                return NotFound();
            }

            return PartialView("_DetailsModal", filmeViewModel);
        }

        [ClaimsAuthorize("Filmes", "Adicionar")]
        [Route("novo-filme")]
        public async Task<IActionResult> Create()
        {
            var filmeViewModel = await PopularGeneros(new FilmeViewModel());
            return View(filmeViewModel);
        }

        [HttpPost]
        [ClaimsAuthorize("Filmes", "Adicionar")]
        [ValidateAntiForgeryToken]
        [Route("novo-filme")]
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

            await _filmeService.Adicionar(_mapper.Map<Filme>(filmeViewModel));

            if (!OperacaoValida())
            {
                return View(filmeViewModel);
            }

            return RedirectToAction(nameof(Index));
        }


        [ClaimsAuthorize("Filmes", "Editar")]
        [Route("editar-filme/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var filmeViewModel = await ObterFilmePreenchido(id);

            if (id == null)
            {
                return NotFound();
            }
            
            return View(filmeViewModel);
        }

        [HttpPost]
        [ClaimsAuthorize("Filmes", "Editar")]
        [ValidateAntiForgeryToken]
        [Route("editar-filme/{id:guid}")]
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

            filmeAtualizacao.GeneroId = filmeViewModel.GeneroId;
            filmeAtualizacao.Nome = filmeViewModel.Nome;
            filmeAtualizacao.Classificacao = filmeViewModel.Classificacao;
            filmeAtualizacao.Ano = filmeViewModel.Ano;
            filmeAtualizacao.Comentarios = filmeViewModel.Comentarios;
            filmeAtualizacao.Valor = filmeViewModel.Valor;
            filmeAtualizacao.Ativo = filmeViewModel.Ativo;


            await _filmeService.Atualizar(_mapper.Map<Filme>(filmeAtualizacao));
            //await _filmeRepository.Atualizar(_mapper.Map<Filme>(filmeAtualizacao));

            if (!OperacaoValida())
            {
                return View(filmeAtualizacao);
            }

            return RedirectToAction(nameof(Index));
        }

        //[ClaimsAuthorize("Filmes", "Editar")]
        [Route("editar-filme-modal/{id:guid}")]
        public async Task<IActionResult> EditarFilmeModal(Guid id)
        {
            var filmeViewModel = await ObterFilmePreenchido(id);

            if (filmeViewModel == null)
            {
                return NotFound();
            }

            return PartialView("_EditarFilme", filmeViewModel);
        }

        //[ClaimsAuthorize("Filmes", "Editar")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("editar-filme-modal/{id:guid}")]
        public async Task<IActionResult> EditarFilmeModal(Guid id, FilmeViewModel filmeViewModel)
        {
            if (id != filmeViewModel.Id)
            {
                return NotFound();
            }

            var filmeAtualizacao = await ObterFilmePreenchido(id);

            filmeViewModel.Genero = filmeAtualizacao.Genero;
            filmeViewModel.Imagem = filmeAtualizacao.Imagem;

            //Ignorando esses dois campos da ModelState para ser válida.
            ModelState.Remove("Nome");
            ModelState.Remove("Documento");

            if (!ModelState.IsValid)
            {
                return PartialView("_EditarFilme", filmeViewModel);
            }

            if (filmeViewModel.ImagemUpload != null)
            {
                var imgPrefixo = Guid.NewGuid() + "_";
                if (!await UploadArquivo(filmeViewModel.ImagemUpload, imgPrefixo))
                {
                    return PartialView("_EditarFilme", filmeViewModel);
                }

                filmeAtualizacao.Imagem = imgPrefixo + filmeViewModel.ImagemUpload.FileName;
            }

            filmeAtualizacao.Nome = filmeViewModel.Nome;
            filmeAtualizacao.Classificacao = filmeViewModel.Classificacao;
            filmeAtualizacao.Ano = filmeViewModel.Ano;
            //filmeAtualizacao.Comentarios = filmeViewModel.Comentarios;
            filmeAtualizacao.Valor = filmeViewModel.Valor;
            filmeAtualizacao.Ativo = filmeViewModel.Ativo;


            await _filmeService.Atualizar(_mapper.Map<Filme>(filmeAtualizacao));

            if (!OperacaoValida())
            {
                return PartialView("_EditarFilme", filmeViewModel);
            }

            var url = Url.Action("ObterFilmesParaModal", "Filmes", new { id = filmeViewModel.Id});
            return Json(new { success = true, url });
        }

        [Route("modal-filme")]
        public async Task<IActionResult> ObterFilmesParaModal()
        {
            var filmes = _mapper.Map<IEnumerable<FilmeViewModel>>(await _filmeRepository.ObterFilmesGeneros());

            if (filmes == null)
            {
                return NotFound();
            }

            return PartialView("_ListaFilmes", filmes);
        }

        [ClaimsAuthorize("Filmes", "Excluir")]
        [Route("excluir-filme/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var filme = await ObterFilmePreenchido(id);

            if (filme == null)
            {
                return NotFound();
            }

            return View(filme);
        }

        
        [HttpPost, ActionName("Delete")]
        [ClaimsAuthorize("Filmes", "Excluir")]
        [ValidateAntiForgeryToken]
        [Route("excluir-filme/{id:guid}")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var filme = await ObterFilmePreenchido(id);

            if (filme == null)
            {
                return NotFound();
            }

            await _filmeService.Remover(id);

            if (!OperacaoValida())
            {
                return View(filme);
            }

            TempData["Sucesso"] = "Filme excluido com sucesso!";

            return RedirectToAction(nameof(Index));
        }


        [Route("obter-filme-genero/{id:guid}")]
        private async Task<FilmeViewModel> ObterFilmePreenchido(Guid id)
        {
            var filme = _mapper.Map<FilmeViewModel>(await _filmeRepository.ObterFilmeGenero(id));
            filme.Generos = _mapper.Map<IEnumerable<GeneroViewModel>>(await _generoRepository.ObterTodos());

            return filme;
        }

        [Route("popular-filme-generos/{id:guid}")]
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
