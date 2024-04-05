using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Univesp.CaminhoDoMar.ProjetoIntegradorApplicationCore.Interfaces.Service;
using Univesp.CaminhoDoMar.ProjetoIntegradorInfrastructure.Services;
using Univesp.CaminhoDoMar.ProjetoIntegradorWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.Business;
using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.DTOs;
using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.Interfaces.Repository;

namespace Univesp.CaminhoDoMar.ProjetoIntegradorWeb.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IAlunoRepository _alunoRepository;
        private readonly IIdentityService _identityService;


        public AdminController(IUsuarioRepository usuarioRepository, IIdentityService identityService,
            
            IAlunoRepository alunoRepository) : base(usuarioRepository, identityService)
        {
           
            _alunoRepository = alunoRepository;
            _identityService = identityService;
        }

        // public override void OnActionExecuting(ActionExecutingContext filterContext)
        // {
        //     string emailUsuario = _identityService.ObterEmail();
        //     string nomeUsuario = _identityService.ObterNome();
        //     Usuario usuario = _usuarioRepository.ObterUsuarioOuInserir(emailUsuario, nomeUsuario).Result;
        //     usuarioLogado = usuario;
        //
        //     if (usuario == null)
        //     {
        //         filterContext.Result = View("NaoCadastrado");
        //     }
        //     base.OnActionExecuting(filterContext);
        // }

        public async Task<IActionResult> Index()
        {
            Usuario usuarioLogado = _usuarioRepository.ObterPorEmail(_identityService.ObterEmail());
            AdminModel adminViewModel = new AdminModel();
            adminViewModel.Usuarios =  _usuarioRepository.ObterTodos().Result.OrderBy(o => o.Nome).ToList();

            return View("Admin", adminViewModel);
        }
        
        
        
        [HttpPost("/admin/baixar-usuarios")]
        public async Task<string> BaixarUsuarios()
        {
            var usuarios = await _usuarioRepository.ObterTodos();
            var tabelaDto = new TabelaDTO()
            {
                cabecalho = new List<string>()
                {
                    "Nome",
                    "Email",
                },
                linhas = new List<LinhaTabelaDTO>()
            };
            foreach (var usuario in usuarios)
            {
                tabelaDto.linhas.Add(new LinhaTabelaDTO()
                {
                    colunas = new List<string>
                    {
                        usuario.Nome,
                        usuario.Email,
                    }
                });
            }
            MemoryStream tabelaExcel = ExcelSpreadsheetService.ExportarTabela("Relatório de Usuários",tabelaDto);
            byte[] bytes = tabelaExcel.ToArray();

            return Convert.ToBase64String(bytes);
        }
    }
}
