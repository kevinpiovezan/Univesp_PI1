using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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
using Univesp.CaminhoDoMar.ProjetoIntegradorApplicationCore.Interfaces.Service;

namespace Univesp.CaminhoDoMar.ProjetoIntegradorWeb.Controllers
{
    [Route("Carga")]
    public class CargaController : BaseController
    {
        private readonly IAlunoRepository _alunoRepository;
        private Usuario usuarioLogado;
        public readonly IIdentityService _identityService;


        public CargaController(IUsuarioRepository usuarioRepository, IIdentityService identityService,
            
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
        //
        //     base.OnActionExecuting(filterContext);
        // }

        public async Task<IActionResult> Index()
        {
            AdminModel adminViewModel = new AdminModel();

            return View("Index", adminViewModel);
        }
        

        #region Alunos
        [HttpPost]
        [Route("obter-alunos")]
        public async Task<ActionResult> ObterAlunos()
        {
            return Json(new
            {
                alunos = _alunoRepository.ObterTodos().Result.OrderBy(o => o.Nome).ToList(),
            });
        }

        [HttpPost]
        [Route("baixar-alunos")]
        public async Task<string> AdicionarAlunos()
        {
            List<Aluno> alunos = await _alunoRepository.ObterTodos();

            MemoryStream tabelaExcel = ExcelSpreadsheetService.ExportarAlunos(alunos);

            byte[] bytes = tabelaExcel.ToArray();

            return Convert.ToBase64String(bytes);
        }

        [HttpPost("carga-alunos")]
        public async Task<JsonResult> UploadTabela([FromBody] CargaAlunosDTO dados)
        {
            var bytes = Convert.FromBase64String(dados.ArquivoBase64.Split(new[] { ";base64," }, StringSplitOptions.RemoveEmptyEntries)[1]);

            var res_carga = ExcelSpreadsheetService.CargaExcelAlunos(new MemoryStream(bytes));

            if (res_carga.Criticas_Carga.Count > 0)
                return Json(new { criticas = res_carga.Criticas_Carga });

            List<Aluno> alunos_banco = await _alunoRepository.ObterTodosTracked();

            List<string> criticas = new List<string>();

            foreach (Aluno a in res_carga.Alunos)
            {
                Aluno aluno_banco = alunos_banco.FirstOrDefault(ab => ab.Cpf == a.Cpf);
                if (aluno_banco != null)
                {
                    aluno_banco.Id_Status_Matricula = a.Id_Status_Matricula;
                    aluno_banco.Email = a.Email;
                    aluno_banco.Endereco = a.Endereco;
                    aluno_banco.Cep = a.Cep;
                    aluno_banco.Nome = a.Nome;
                    aluno_banco.Nome_Social = a.Nome_Social;
                    aluno_banco.Celular = a.Celular;
                    aluno_banco.Tel_Fixo = a.Tel_Fixo;
                    aluno_banco.Professor = a.Professor;
                    aluno_banco.Data_Emissao = a.Data_Emissao;
                    aluno_banco.Autorizacao_Imagem = a.Autorizacao_Imagem;
                    aluno_banco.Cadastro_SpTrans = a.Cadastro_SpTrans;
                    aluno_banco.Servidor_Publico = a.Servidor_Publico;
                    aluno_banco.Ultima_Atualizacao = DateTime.Now;

                    if(dados.Confirmado == false)
                        criticas.Add("CPF " + a.Cpf + " já está cadastrado");
                    
                } else
                {
                    _alunoRepository.Add(a);
                }
            }

            if (criticas.Count > 0)
                return Json(new { criticas = new List<CriticasCarga>() { new CriticasCarga() {Criticas = criticas}}});


            if (res_carga.Criticas_Carga.Count > 0)
                return Json(new { criticas = res_carga.Criticas_Carga });

            await _alunoRepository.SaveChanges();
            

            return Json("Ok");
        }
        #endregion
        
    }
}
