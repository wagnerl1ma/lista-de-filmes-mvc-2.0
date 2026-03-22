using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ListaDeFilmes.App.Areas.Identity.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        // TODO: [EMAIL] Injetar IEmailSender aqui quando implementar o servico de e-mail
        // private readonly IEmailSender _emailSender;

        public ForgotPasswordModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required(ErrorMessage = "O campo E-mail é obrigatório.")]
            [EmailAddress(ErrorMessage = "O campo E-mail não é um endereço de e-mail válido.")]
            [Display(Name = "E-mail")]
            public string Email { get; set; } = string.Empty;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                // TODO: [EMAIL] Descomentar e implementar quando o servico de e-mail estiver pronto
                // 1. Gerar token de reset:
                //    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                //    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                //
                // 2. Montar o link de callback:
                //    var callbackUrl = Url.Page("/Account/ResetPassword", pageHandler: null,
                //        values: new { area = "Identity", code }, protocol: Request.Scheme);
                //
                // 3. Enviar o e-mail:
                //    await _emailSender.SendEmailAsync(Input.Email, "Redefinir senha",
                //        $"Redefina sua senha <a href='{callbackUrl}'>clicando aqui</a>.");

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}
