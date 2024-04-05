using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Univesp.CaminhoDoMar.ProjetoIntegradorApplicationCore.Interfaces.Service;
using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.Interfaces.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Univesp.CaminhoDoMar.ProjetoIntegradorWeb.Controllers
{

    public class AccountController : Controller
    {
        protected readonly IIdentityService _identityService;
        protected readonly IUsuarioRepository _usuarioRepository;

        public AccountController(IUsuarioRepository usuarioRepository, IIdentityService identityService)
        {
            _usuarioRepository = usuarioRepository;
            _identityService = identityService;
        }

       
        public async Task Login(string returnUrl = "/")
        {
            var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
                // Indicate here where Auth0 should redirect the user after a login.
                // Note that the resulting absolute Uri must be added to the
                // **Allowed Callback URLs** settings for the app.
                .Build();

            await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        }
        [Authorize]
        public async Task Logout()
        {
            var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
                // Indicate here where Auth0 should redirect the user after a logout.
                // Note that the resulting absolute Uri must be added to the
                // **Allowed Logout URLs** settings for the app.
                .WithRedirectUri(Url.Action("Index", "Home"))
                .Build();

            await HttpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
        
        // protected async Task<bool> UsuarioAdmin()
        // {
        //     string usuarioLogado = _identityService.ObterEmail();
        //
        //     return await _usuarioRepository.isAdministrator(usuarioLogado);
        // }

        // protected async Task<IActionResult> AdminView(string viewName)
        // {
        //     try
        //     {
        //         if (! await UsuarioAdmin())
        //         {
        //             return base.View("NaoAutorizado");
        //         }
        //
        //         return base.View(viewName);
        //     }
        //     catch (Exception ex)
        //     {
        //         return Json(ex.Message);
        //     }
        // }
        //
        // protected async Task<IActionResult> AdminView(string viewName, object model)
        // {
        //     try
        //     {
        //         if (! await UsuarioAdmin())
        //         {
        //             return base.View("NaoAutorizado");
        //         }
        //
        //         return base.View(viewName, model);
        //     }
        //     catch (Exception ex)
        //     {
        //         return Json(ex.Message);
        //     }
        // }
    }
}
