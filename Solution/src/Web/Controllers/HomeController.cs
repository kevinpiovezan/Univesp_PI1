using System;
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
    public class HomeController : BaseController
    {

        private readonly IAlunoRepository _alunoRepository;
        protected readonly IUsuarioRepository _usuarioRepository;
        private readonly IIdentityService _identityService;



        public HomeController(IUsuarioRepository usuarioRepository, IIdentityService identityService,
            
            IAlunoRepository alunoRepository) : base(usuarioRepository,identityService)
        {
           
            _alunoRepository = alunoRepository;
            _usuarioRepository = usuarioRepository;
            _identityService = identityService;
        }


        public async Task<IActionResult> Index()
        {
            var usuarios = await _usuarioRepository.ObterTodos();
            Console.WriteLine("TO AQUI PORRA");
            Console.WriteLine(_identityService.ObterEmail());
            Usuario usuarioLogado = _usuarioRepository.ObterPorEmail(_identityService.ObterEmail());

            var model = new HomeModel()
            {
                UsuarioLogado = usuarioLogado,
                Alunos = await _alunoRepository.ObterTodos(),
            };

            model.TodosUsuarios = usuarios;
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
