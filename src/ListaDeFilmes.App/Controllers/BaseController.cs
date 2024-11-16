using ListaDeFilmes.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ListaDeFilmes.App.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly INotificador _notificador;

        protected BaseController(INotificador notificador)
        {
            _notificador = notificador;
        }

        protected bool OperacaoValida()
        {
            return !_notificador.TemNotificacao();
        }
    }
}
