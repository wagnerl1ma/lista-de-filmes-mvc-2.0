using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace ListaDeFilmes.App.Areas.Identity.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ForgotPasswordModel(UserManager<IdentityUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
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
                if (user == null)
                {
                    // Não revelar que o usuário não existe
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(
                    Input.Email,
                    "Redefinir senha - Minha Lista de Filmes",
                    BuildResetEmailHtml(HtmlEncoder.Default.Encode(callbackUrl!)));

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }

        private static string BuildResetEmailHtml(string callbackUrl)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset=""utf-8"" />
</head>
<body style=""margin: 0; padding: 0; background-color: #001520; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;"">
    <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""background-color: #001520; padding: 40px 0;"">
        <tr>
            <td align=""center"">
                <table role=""presentation"" width=""520"" cellspacing=""0"" cellpadding=""0"" style=""background-color: #073642; border-radius: 12px; overflow: hidden;"">
                    <tr>
                        <td style=""padding: 30px 40px 20px; text-align: center; border-bottom: 2px solid #b58900;"">
                            <h1 style=""color: #b58900; margin: 0; font-size: 22px;"">Minha Lista de Filmes</h1>
                        </td>
                    </tr>
                    <tr>
                        <td style=""padding: 30px 40px;"">
                            <h2 style=""color: #ffffff; margin: 0 0 16px; font-size: 20px;"">Redefinir sua senha</h2>
                            <p style=""color: #93a1a1; font-size: 15px; line-height: 1.6; margin: 0 0 24px;"">
                                Recebemos uma solicitacao para redefinir a senha da sua conta.
                                Clique no botao abaixo para criar uma nova senha.
                            </p>
                            <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"">
                                <tr>
                                    <td align=""center"" style=""padding: 8px 0 24px;"">
                                        <a href=""{callbackUrl}""
                                           style=""display: inline-block; background-color: #b58900; color: #001520; text-decoration: none; padding: 14px 32px; border-radius: 6px; font-weight: bold; font-size: 15px;"">
                                            Redefinir Senha
                                        </a>
                                    </td>
                                </tr>
                            </table>
                            <p style=""color: #657b83; font-size: 13px; line-height: 1.5; margin: 0;"">
                                Se voce nao solicitou a redefinicao de senha, ignore este e-mail.
                                Sua senha permanecera inalterada.
                            </p>
                            <p style=""color: #657b83; font-size: 12px; line-height: 1.5; margin: 16px 0 0; word-break: break-all;"">
                                Se o botao nao funcionar, copie e cole este link no navegador:<br/>
                                <a href=""{callbackUrl}"" style=""color: #b58900;"">{callbackUrl}</a>
                            </p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>";
        }
    }
}
