using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.Business;
using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.Interfaces.Repository;
using Univesp.CaminhoDoMar.ProjetoIntegrador.Web.Models;
using Univesp.CaminhoDoMar.ProjetoIntegradorApplicationCore.Interfaces.Service;

namespace Univesp.CaminhoDoMar.ProjetoIntegradorWeb.Controllers
{
    [Route("aluno")]
    public class AlunoController : BaseController
    {
        private readonly IAlunoRepository _alunoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IIdentityService _identityService;

        public AlunoController(IAlunoRepository alunoRepository,IUsuarioRepository usuarioRepository,IIdentityService identityService) : base(usuarioRepository, identityService)
        {
            _alunoRepository = alunoRepository;
            _usuarioRepository = usuarioRepository;
            _identityService = identityService;
        }

        [Route("{cpf}")]
        public async Task<IActionResult> Index(string cpf)
        {
            Usuario usuarioLogado = _usuarioRepository.ObterPorEmail(_identityService.ObterEmail());
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
        
        [Route("novo-aluno")]
        public async Task<IActionResult> Novo()
        {
            // var aluno = await _alunoRepository.ObtemAlunoPorCPF(cpf);
            // if (aluno == null)
            //     return View("NaoEncontrado");
            // var usuarios = await _usuarioRepository.ObterTodos();
            // var model = new AlunoViewModel();
            // model.Aluno = aluno;
            // model.Usuarios = usuarios;
            // model.UsuarioLogado = usuarioLogado;
            return View();
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
        
        [HttpPost("adicionar")]
        public async Task<IActionResult> NovoAluno([FromBody] Aluno dadosAluno)
        {
            var alunoExiste = await _alunoRepository.ObtemAlunoPorCPF(dadosAluno.Cpf);
            if (alunoExiste != null)
            {
                return StatusCode(403, "Já existe um aluno cadastrado com esse CPF, caso queira alterar os dados edite pela página do aluno");
            }
            
            if (dadosAluno.Data_Emissao > DateTime.Today)
            {
                return StatusCode(403, "Data de Emissão não pode ser maior que a data atual");
            }
            
            if (dadosAluno.Data_Nascimento > DateTime.Today)
            {
                return StatusCode(403, "Data de Nascimento não pode ser maior que a data atual");
            }
            
            dadosAluno.Ultima_Atualizacao = DateTime.Now;
            await _alunoRepository.Adicionar(dadosAluno);
            return Json(dadosAluno.Cpf);
        }
    }
}
