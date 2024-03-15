using Microsoft.AspNetCore.Mvc;
using Univesp.CaminhoDoMar.ProjetoIntegradorApplicationCore.Interfaces.Service;
using Univesp.CaminhoDoMar.ProjetoIntegradorWeb.Models;
using System.Diagnostics;
using System.Threading.Tasks;
using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.Business;
using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.Interfaces.Repository;
using Univesp.CaminhoDoMar.ProjetoIntegrador.Web.Models;

namespace Univesp.CaminhoDoMar.ProjetoIntegradorWeb.Controllers
{
    public class HomeController : ProtectedController
    {

        private readonly IAlunoRepository _alunoRepository;
        private Usuario usuarioLogado;

        public HomeController(IUsuarioRepository usuarioRepository,
            
            IAlunoRepository alunoRepository) : base(usuarioRepository)
        {
           
            _alunoRepository = alunoRepository;
            
        }


        public async Task<IActionResult> Index()
        {
            // Usuario usuarioLogado = await _usuarioRepository.ObterUsuarioOuInserir(_identityService.ObterEmail(), _identityService.ObterNome());

            var model = new HomeModel()
            {
                UsuarioLogado = usuarioLogado,
                Alunos = await _alunoRepository.ObterTodos(),
            };

            model.TodosUsuarios = await _usuarioRepository.ObterTodos();
            return View(model);
        }
        
        [HttpPost("dados")]
        public async Task<ActionResult> ObterDadosDashboard()
        {
            var alunos = await _alunoRepository.ObterTodos();
            DadosDashboard model = new DadosDashboard(alunos);

            return Json(model);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
