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
        public async Task<IActionResult> Index(CancellationToken ct = default)
        {
            return View(_mapper.Map<IEnumerable<FilmeViewModel>>(await _filmeRepository.ObterFilmesGeneros(ct)));
        }

        [AllowAnonymous]
        [Route("detalhes-do-filme/{id:guid}")]
        public async Task<IActionResult> Details(Guid id, CancellationToken ct = default)
        {
            var filmeViewModel = await ObterFilmePreenchido(id, ct);

            if (filmeViewModel == null)
            {
                return NotFound();
            }

            return View(filmeViewModel);
        }

        [AllowAnonymous]
        [Route("detalhes-do-filme-modal/{id:guid}")]
        public async Task<IActionResult> DetailsModal(Guid id, CancellationToken ct = default)
        {
            var filmeViewModel = await ObterFilmePreenchido(id, ct);

            if (filmeViewModel == null)
            {
                return NotFound();
            }

            return PartialView("_DetailsModal", filmeViewModel);
        }

        [ClaimsAuthorize("Filmes", "Adicionar")]
        [Route("novo-filme")]
        public async Task<IActionResult> Create(CancellationToken ct = default)
        {
            var filmeViewModel = await PopularGeneros(new FilmeViewModel(), ct);
            return View(filmeViewModel);
        }

        [HttpPost]
        [ClaimsAuthorize("Filmes", "Adicionar")]
        [Route("novo-filme")]
        public async Task<IActionResult> Create(FilmeViewModel filmeViewModel, CancellationToken ct = default)
        {
            filmeViewModel = await PopularGeneros(filmeViewModel, ct);

            if (!ModelState.IsValid)
            {
                return View(filmeViewModel);
            }

            var imgPrefixo = Guid.NewGuid() + "_";

            if (!await UploadArquivo(filmeViewModel.ImagemUpload, imgPrefixo, ct))
            {
                return View(filmeViewModel);
            }

            filmeViewModel.Imagem = imgPrefixo + filmeViewModel.ImagemUpload!.FileName;

            await _filmeService.Adicionar(_mapper.Map<Filme>(filmeViewModel), ct);

            if (!OperacaoValida())
            {
                return View(filmeViewModel);
            }

            return RedirectToAction(nameof(Index));
        }


        [ClaimsAuthorize("Filmes", "Editar")]
        [Route("editar-filme/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, CancellationToken ct = default)
        {
            var filmeViewModel = await ObterFilmePreenchido(id, ct);

            if (filmeViewModel == null)
            {
                return NotFound();
            }

            return View(filmeViewModel);
        }

        [HttpPost]
        [ClaimsAuthorize("Filmes", "Editar")]
        [Route("editar-filme/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, FilmeViewModel filmeViewModel, CancellationToken ct = default)
        {
            if (id != filmeViewModel.Id)
            {
                return NotFound();
            }

            var filmeAtualizacao = await ObterFilmePreenchido(id, ct);

            filmeViewModel.Genero = filmeAtualizacao.Genero;
            filmeViewModel.Imagem = filmeAtualizacao.Imagem;

            if (!ModelState.IsValid)
            {
                return View(filmeViewModel);
            }

            if (filmeViewModel.ImagemUpload != null)
            {
                var imgPrefixo = Guid.NewGuid() + "_";
                if (!await UploadArquivo(filmeViewModel.ImagemUpload, imgPrefixo, ct))
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


            await _filmeService.Atualizar(_mapper.Map<Filme>(filmeAtualizacao), ct);
            //await _filmeRepository.Atualizar(_mapper.Map<Filme>(filmeAtualizacao));

            if (!OperacaoValida())
            {
                return View(filmeAtualizacao);
            }

            return RedirectToAction(nameof(Index));
        }

        //[ClaimsAuthorize("Filmes", "Editar")]
        [Route("editar-filme-modal/{id:guid}")]
        public async Task<IActionResult> EditarFilmeModal(Guid id, CancellationToken ct = default)
        {
            var filmeViewModel = await ObterFilmePreenchido(id, ct);

            if (filmeViewModel == null)
            {
                return NotFound();
            }

            return PartialView("_EditarFilme", filmeViewModel);
        }

        //[ClaimsAuthorize("Filmes", "Editar")]
        [HttpPost]
        [Route("editar-filme-modal/{id:guid}")]
        public async Task<IActionResult> EditarFilmeModal(Guid id, FilmeViewModel filmeViewModel, CancellationToken ct = default)
        {
            if (id != filmeViewModel.Id)
            {
                return NotFound();
            }

            var filmeAtualizacao = await ObterFilmePreenchido(id, ct);

            filmeViewModel.Genero = filmeAtualizacao.Genero;
            filmeViewModel.Imagem = filmeAtualizacao.Imagem;

            //Ignorando esses dois campos da ModelState para ser válida.
            ModelState.Remove("Nome");

            if (!ModelState.IsValid)
            {
                return PartialView("_EditarFilme", filmeViewModel);
            }

            if (filmeViewModel.ImagemUpload != null)
            {
                var imgPrefixo = Guid.NewGuid() + "_";
                if (!await UploadArquivo(filmeViewModel.ImagemUpload, imgPrefixo, ct))
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


            await _filmeService.Atualizar(_mapper.Map<Filme>(filmeAtualizacao), ct);

            if (!OperacaoValida())
            {
                return PartialView("_EditarFilme", filmeViewModel);
            }

            var url = Url.Action("ObterFilmesParaModal", "Filmes", new { id = filmeViewModel.Id});
            return Json(new { success = true, url });
        }

        [Route("modal-filme")]
        public async Task<IActionResult> ObterFilmesParaModal(CancellationToken ct = default)
        {
            var filmes = _mapper.Map<IEnumerable<FilmeViewModel>>(await _filmeRepository.ObterFilmesGeneros(ct));

            if (filmes == null)
            {
                return NotFound();
            }

            return PartialView("_ListaFilmes", filmes);
        }

        [ClaimsAuthorize("Filmes", "Excluir")]
        [Route("excluir-filme/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct = default)
        {
            var filme = await ObterFilmePreenchido(id, ct);

            if (filme == null)
            {
                return NotFound();
            }

            return View(filme);
        }


        [HttpPost, ActionName("Delete")]
        [ClaimsAuthorize("Filmes", "Excluir")]
        [Route("excluir-filme/{id:guid}")]
        public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken ct = default)
        {
            var filme = await ObterFilmePreenchido(id, ct);

            if (filme == null)
            {
                return NotFound();
            }

            await _filmeService.Remover(id, ct);

            if (!OperacaoValida())
            {
                return View(filme);
            }

            TempData["Sucesso"] = "Filme excluido com sucesso!";

            return RedirectToAction(nameof(Index));
        }


        private async Task<FilmeViewModel> ObterFilmePreenchido(Guid id, CancellationToken ct = default)
        {
            var filme = _mapper.Map<FilmeViewModel>(await _filmeRepository.ObterFilmeGenero(id, ct));
            filme.Generos = _mapper.Map<IEnumerable<GeneroViewModel>>(await _generoRepository.ObterTodos(ct));

            return filme;
        }

        private async Task<FilmeViewModel> PopularGeneros(FilmeViewModel filme, CancellationToken ct = default)
        {
            filme.Generos = _mapper.Map<IEnumerable<GeneroViewModel>>(await _generoRepository.ObterTodos(ct));
            return filme;
        }

        private async Task<bool> UploadArquivo(IFormFile? arquivo, string imgPrefixo, CancellationToken ct = default)
        {
            if (arquivo == null || arquivo.Length <= 0) return false;

            //irá gravar a imagem nesse caminho
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", imgPrefixo + arquivo.FileName);

            if (System.IO.File.Exists(path))
            {
                ModelState.AddModelError(string.Empty, "Já existe um arquivo com este nome!");
                return false;
            }

            await using var stream = new FileStream(path, FileMode.Create);
            await arquivo.CopyToAsync(stream, ct);

            return true;
        }
    }
}
