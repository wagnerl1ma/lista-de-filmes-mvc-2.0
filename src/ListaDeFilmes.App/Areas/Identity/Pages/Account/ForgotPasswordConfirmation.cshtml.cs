using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ListaDeFilmes.App.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordConfirmationModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
