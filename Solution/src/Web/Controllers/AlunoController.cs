using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.Business;
using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.Interfaces.Repository;
using Univesp.CaminhoDoMar.ProjetoIntegrador.Web.Models;

namespace Univesp.CaminhoDoMar.ProjetoIntegradorWeb.Controllers
{
    [Route("aluno")]
    public class AlunoController : Controller
    {
        private readonly IAlunoRepository _alunoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        public AlunoController(IAlunoRepository alunoRepository,IUsuarioRepository usuarioRepository)
        {
            _alunoRepository = alunoRepository;
            _usuarioRepository = usuarioRepository;
        }

        [Route("{cpf}")]
        public async Task<IActionResult> Index(string cpf)
        {
            var aluno = await _alunoRepository.ObtemAlunoPorCPF(cpf);
            if (aluno == null)
                return View("NaoEncontrado");
            var usuarios = await _usuarioRepository.ObterTodos();
            var model = new AlunoViewModel();
            model.Aluno = aluno;
            model.Usuarios = usuarios;
            // model.UsuarioLogado = usuarioLogado;
            return View(model);
        }
        
        [HttpPost("editar/{idAluno}")]
        public async Task<IActionResult> EditarAluno([FromRoute] int idAluno, [FromBody] Aluno dadosAluno)
        {
            var aluno = await _alunoRepository.ObterPorId(idAluno);

            aluno.Email = dadosAluno.Email;
            aluno.Endereco = dadosAluno.Endereco;
            aluno.Cep = dadosAluno.Cep;
            aluno.Nome = dadosAluno.Nome;
            aluno.Celular = dadosAluno.Celular;
            aluno.Tel_Fixo = dadosAluno.Tel_Fixo;
            aluno.Professor = dadosAluno.Professor;
            aluno.Autorizacao_Imagem = dadosAluno.Autorizacao_Imagem;
            aluno.Cadastro_SpTrans = dadosAluno.Cadastro_SpTrans;
            aluno.Servidor_Publico = dadosAluno.Servidor_Publico;
            aluno.Ultima_Atualizacao = DateTime.Now;

            await _alunoRepository.Atualizar(aluno);
            return Json("Ok");
        }
        
    }
}
