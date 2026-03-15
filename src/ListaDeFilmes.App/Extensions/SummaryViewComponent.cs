using ListaDeFilmes.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ListaDeFilmes.App.Extensions
{
    public class SummaryViewComponent : ViewComponent
    {
        private readonly INotificador _notificador;

        public SummaryViewComponent(INotificador notificador)
        {
            _notificador = notificador;
        }

        public IViewComponentResult Invoke()
        {
            var notificacoes = _notificador.ObterNotificacoes();

            foreach (var c in notificacoes)
                ViewData.ModelState.AddModelError(string.Empty, c.Mensagem);

            return View();
        }
    }
}
