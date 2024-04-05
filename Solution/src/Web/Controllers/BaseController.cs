using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.Business;
using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.Interfaces.Repository;
using Univesp.CaminhoDoMar.ProjetoIntegradorApplicationCore.Interfaces.Service;

namespace Univesp.CaminhoDoMar.ProjetoIntegradorWeb.Controllers
{
    #if !DEBUG
        [Authorize]
    #endif
    public class BaseController : Controller
    {
        public readonly IIdentityService _identityService;
        public readonly IUsuarioRepository _usuarioRepository;


        public BaseController(IUsuarioRepository usuarioRepository, IIdentityService identityService)
        {
            _usuarioRepository = usuarioRepository;
            _identityService = identityService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _usuarioRepository.AdicionarCasoInexistente(new Usuario()
            {
                Nome = "Kevin Piovezan",
                Email = "kevinpiovezan@gmail.com",
                IsAdmin = true
            });
            _usuarioRepository.AdicionarCasoInexistente(new Usuario()
            {
                Nome = "Deborah Regina Freitas Dantas",
                Email = "deborah.dantas@polo.univesp.br",
                IsAdmin = true
            });
            if (!_identityService.IsAuthenticated())
            {
                context.Result = RedirectToAction("Login","Account");
            } else
            {
                var usuario = _usuarioRepository.ObterUsuarioOuInserir(_identityService.ObterEmail(), _identityService.ObterEmail()).Result;
                if(!usuario.IsAdmin)
                {
                    context.Result = View("NaoAutorizado");
                }
            }


            base.OnActionExecuting(context);
        }
    }
}