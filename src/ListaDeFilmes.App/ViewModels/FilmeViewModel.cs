using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ListaDeFilmes.App.ViewModels
{
    public class FilmeViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "É necessário colocar o {0}")] //obrigatorio
        [StringLength(80, MinimumLength = 3, ErrorMessage = "O tamanho do {0} deve ser entre {2} e {1} caracteres!")]   //  0 = nome, 2 = tamanho minimo, 1 = tamanho máximo
        public string Nome { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH':'mm':'ss}")]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]   //formato para Data de Nascimento
        [Display(Name = "Data do Cadastro")]
        public DateTime? DataCadastro { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "É necessário colocar uma {0}")] //obrigatorio
        [Display(Name = "Classificação", Prompt = "Ex: 9, 12, 16, 18 ou Livre")] //Propt = Espaço Reservado - placeholder
        [StringLength(5, MinimumLength = 1, ErrorMessage = "O tamanho do {0} deve ser entre {2} e {1} caracteres!")]   //  0 = nome, 2 = tamanho minimo, 1 = tamanho máximo
        public string Classificacao { get; set; }

        public int? Ano { get; set; }

        [Display(Name = "Comentários")]
        public string Comentarios { get; set; }

        [Display(Name = "Imagem do Produto")]
        //[Ignore]
        public IFormFile ImagemUpload { get; set; }

        public string Imagem { get; set; }

        //[Moeda]  //[Moeda] é um DataAnnotations Personalizado
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public decimal Valor { get; set; }

        [DisplayName("Ativo?")]
        public bool Ativo { get; set; }


        // Um Filme pode ter vários Atores (Relacionamneto muitos para muitos)
        public ICollection<FilmeAtorViewModel> FilmesAtores { get; set; }

        // Um Filme só pode ter um Genero (Relacionamento um para muitos)
        [Display(Name = "Gênero")] //Espaço Reservado - placeholder
        public GeneroViewModel Genero { get; set; }


        // Um Filme só pode ter um Diretor (Relacionamneto um para muitos)
        public DiretorViewModel Diretor { get; set; }


        // Um Filme só pode ter um Produtora (Relacionamneto um para muitos)
        public ProdutoraViewModel Produtora { get; set; }


        //public FilmeViewModel()
        //{

        //}

        //public FilmeViewModel(Guid id, string nome, string classificacao, int? ano, string comentarios, GeneroViewModel genero, DiretorViewModel diretor, ProdutoraViewModel produtora)
        //{
        //    Id = id;
        //    Nome = nome;
        //    Classificacao = classificacao;
        //    Ano = ano;
        //    Comentarios = comentarios;
        //    Genero = genero;
        //    Diretor = diretor;
        //    Produtora = produtora;
        //}

        //public FilmeViewModel(string nome, string classificacao, int? ano, string comentarios, GeneroViewModel genero, DiretorViewModel diretor, ProdutoraViewModel produtora)
        //{
        //    Nome = nome;
        //    Classificacao = classificacao;
        //    Ano = ano;
        //    Comentarios = comentarios;
        //    Genero = genero;
        //    Diretor = diretor;
        //    Produtora = produtora;
        //}

        //public void AddFilmeAtores(FilmeAtorViewModel filmeAtor)
        //{
        //    FilmesAtores.Add(filmeAtor);
        //}

        //public void RemoverFilmeAtores(FilmeAtorViewModel filme)
        //{
        //    FilmesAtores.Remove(filme);
        //}


        ////Buscar ultimos cadastrados
        //public double UltimosCadastrados(DateTime inicio, DateTime final)
        //{
        //    return Genero.Filmes.Where(g => g.DataCadastro >= inicio && g.DataCadastro <= final).Sum(g => g.Genero.Filmes.Count());
        //}

        //Buscar por lançamentos  -- criar


        //Buscar por ano do filme   -- criar

    }
}
