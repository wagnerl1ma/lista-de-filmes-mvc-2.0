using FluentAssertions;
using ListaDeFilmes.Business.Models;
using ListaDeFilmes.Business.Models.Validations;

namespace ListaDeFilmes.Tests.Validations
{
    public class GeneroValidationTests
    {
        private readonly GeneroValidation _validation = new();

        [Fact]
        public void GeneroValidation_NomeSemPreenchimento_DeveRetornarErro()
        {
            var genero = new Genero { Nome = string.Empty };

            var resultado = _validation.Validate(genero);

            resultado.IsValid.Should().BeFalse();
            resultado.Errors.Should().Contain(e => e.PropertyName == "Nome");
        }

        [Fact]
        public void GeneroValidation_NomeComTamanhoValido_DeveRetornarSucesso()
        {
            var genero = new Genero { Nome = "Acao" };

            var resultado = _validation.Validate(genero);

            resultado.IsValid.Should().BeTrue();
        }

        [Fact]
        public void GeneroValidation_NomeExcedeTamanhoMaximo_DeveRetornarErro()
        {
            var genero = new Genero { Nome = new string('A', 51) };

            var resultado = _validation.Validate(genero);

            resultado.IsValid.Should().BeFalse();
            resultado.Errors.Should().Contain(e => e.PropertyName == "Nome");
        }
    }
}
