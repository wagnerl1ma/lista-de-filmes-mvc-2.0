using ListaDeFilmes.Business.Notificacoes;

namespace ListaDeFilmes.Business.Interfaces
{
    public interface INotificador
    {
        bool TemNotificacao();
        IReadOnlyList<Notificacao> ObterNotificacoes();
        void Handle(Notificacao notificacao);
    }
}
