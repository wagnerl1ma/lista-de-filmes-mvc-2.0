using FluentAssertions;
using ListaDeFilmes.Business.Notificacoes;

namespace ListaDeFilmes.Tests.Notificacoes
{
    public class NotificadorTests
    {
        [Fact]
        public void Handle_NovaNotificacao_DeveAdicionarNaLista()
        {
            var notificador = new Notificador();

            notificador.Handle(new Notificacao("Erro de teste"));

            notificador.ObterNotificacoes().Should().HaveCount(1);
            notificador.ObterNotificacoes()[0].Mensagem.Should().Be("Erro de teste");
        }

        [Fact]
        public void TemNotificacao_SemNotificacoes_DeveRetornarFalse()
        {
            var notificador = new Notificador();

            notificador.TemNotificacao().Should().BeFalse();
        }

        [Fact]
        public void TemNotificacao_ComNotificacoes_DeveRetornarTrue()
        {
            var notificador = new Notificador();
            notificador.Handle(new Notificacao("Erro"));

            notificador.TemNotificacao().Should().BeTrue();
        }
    }
}
