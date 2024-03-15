using Microsoft.AspNetCore.Mvc;
using Univesp.CaminhoDoMar.ProjetoIntegradorApplicationCore.Interfaces.Service;
using System;
using System.Threading.Tasks;
using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.Interfaces.Repository;

namespace Univesp.CaminhoDoMar.ProjetoIntegradorWeb.Controllers
{
// #if !DEBUG
//     [Authorize]
// #endif
    public class ProtectedController : Controller
    {
        protected readonly IIdentityService _identityService;
        protected readonly IUsuarioRepository _usuarioRepository;

        public ProtectedController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
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
