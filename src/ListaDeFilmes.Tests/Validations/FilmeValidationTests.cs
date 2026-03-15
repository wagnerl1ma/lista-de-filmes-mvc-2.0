using FluentAssertions;
using ListaDeFilmes.Business.Models;
using ListaDeFilmes.Business.Models.Validations;

namespace ListaDeFilmes.Tests.Validations
{
    public class FilmeValidationTests
    {
        private readonly FilmeValidation _validation = new();

        private static Filme CriarFilmeValido() => new()
        {
            Nome = "Matrix",
            Classificacao = "14",
            Comentarios = "Filme de ficao cientifica",
            Valor = 29.90m,
            Ativo = true
        };

        [Fact]
        public void FilmeValidation_NomeSemPreenchimento_DeveRetornarErro()
        {
            var filme = CriarFilmeValido();
            filme.Nome = string.Empty;

            var resultado = _validation.Validate(filme);

            resultado.IsValid.Should().BeFalse();
            resultado.Errors.Should().Contain(e => e.PropertyName == "Nome");
        }

        [Fact]
        public void FilmeValidation_NomeComTamanhoValido_DeveRetornarSucesso()
        {
            var filme = CriarFilmeValido();

            var resultado = _validation.Validate(filme);

            resultado.IsValid.Should().BeTrue();
        }

        [Fact]
        public void FilmeValidation_ClassificacaoExcedeTamanho_DeveRetornarErro()
        {
            var filme = CriarFilmeValido();
            filme.Classificacao = "123456";

            var resultado = _validation.Validate(filme);

            resultado.IsValid.Should().BeFalse();
            resultado.Errors.Should().Contain(e => e.PropertyName == "Classificacao");
        }

        [Fact]
        public void FilmeValidation_ValorMenorOuIgualZero_DeveRetornarErro()
        {
            var filme = CriarFilmeValido();
            filme.Valor = 0;

            var resultado = _validation.Validate(filme);

            resultado.IsValid.Should().BeFalse();
            resultado.Errors.Should().Contain(e => e.PropertyName == "Valor");
        }

        [Fact]
        public void FilmeValidation_ComentariosSemPreenchimento_DeveRetornarErro()
        {
            var filme = CriarFilmeValido();
            filme.Comentarios = string.Empty;

            var resultado = _validation.Validate(filme);

            resultado.IsValid.Should().BeFalse();
            resultado.Errors.Should().Contain(e => e.PropertyName == "Comentarios");
        }
    }
}
