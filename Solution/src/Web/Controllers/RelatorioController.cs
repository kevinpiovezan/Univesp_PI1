using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Univesp.CaminhoDoMar.ProjetoIntegradorApplicationCore.Business;
using Univesp.CaminhoDoMar.ProjetoIntegradorApplicationCore.Interfaces.Repository;
using Univesp.CaminhoDoMar.ProjetoIntegradorApplicationCore.Interfaces.Service;
using Univesp.CaminhoDoMar.ProjetoIntegradorWeb.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Univesp.CaminhoDoMar.ProjetoIntegradorInfrastructure.Services;
using Syngenta.Casa.RenCad.ApplicationCore.Enums;
using AppHub.Syngenta.Common.Extensions;

namespace Univesp.CaminhoDoMar.ProjetoIntegradorWeb.Controllers
{
    public class RelatorioController : ProtectedController
    {
        private readonly ICicloRenovacaoRepository _cicloRepository;
        private readonly IAlunoArquivoRepository _clienteArquivoRepository;
        private readonly ICicloAlunoRepository _cicloAlunoRepository;
        private readonly IAlunoRepository _clienteRepository;
        private readonly ICicloChecklistRepository _cicloChecklistRepository;
        private readonly ITipoArquivoRepository _tipoArquivoRepository;
        private readonly ITipoAlunoRepository _tipoAlunoRepository;
        private readonly IArquivoRepository _arquivoRepository;
        private readonly IContatoRepository _contatoRepository;
        private readonly ILogAtividadeRepository _logRepository;
        private Usuario usuarioLogado; 
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContext Current => _httpContextAccessor.HttpContext;
        public string AppBaseUrl => $"{Current.Request.Scheme}://{Current.Request.Host}{Current.Request.PathBase}";

        public RelatorioController(IUsuarioRepository usuarioRepository,
            IAlunoArquivoRepository clienteArquivoRepository,
            ICicloAlunoRepository cicloAlunoRepository,
            ICicloRenovacaoRepository cicloRepository,
            ICicloChecklistRepository cicloChecklistRepository,
            IAlunoRepository clienteRepository,
            ITipoArquivoRepository tipoArquivoRepository,
            ITipoAlunoRepository tipoAlunoRepository,
            ILogAtividadeRepository logAtividadeRepository,
            IArquivoRepository arquivoRepository,
            IContatoRepository contatoRepository,
            IHttpContextAccessor httpContextAccessor,
            IIdentityService identityService) : base(usuarioRepository, identityService)
        {
            _cicloRepository = cicloRepository;
            _clienteRepository = clienteRepository;
            _cicloAlunoRepository = cicloAlunoRepository;
            _cicloChecklistRepository = cicloChecklistRepository;
            _tipoArquivoRepository = tipoArquivoRepository;
            _tipoAlunoRepository = tipoAlunoRepository;
            _logRepository = logAtividadeRepository;
            _arquivoRepository = arquivoRepository;
            _contatoRepository = contatoRepository;
            _clienteArquivoRepository = clienteArquivoRepository;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<IActionResult> Index()
        {
            Usuario usuarioLogado = await _usuarioRepository.ObterUsuarioOuInserir(_identityService.ObterEmail(), _identityService.ObterNome());
            var ciclos = await _cicloRepository.ObterTodos();
            var model = new RelatorioModel();
            model.Ciclos = ciclos.Where(w => w.Id != 0).ToList();
            return View(model);
        }
        
        [HttpPost]
        [Route("baixar-relatorio/{id}")]
        public async Task<string> BaixarRelatorio(int id)
        {
            var ciclo = await _cicloRepository.ObterPorId(id);
            var clientes = await _clienteRepository.ObtemAlunosPorCicloNoTracking(id);
            var cicloChecklists =await _cicloChecklistRepository.ObterChecklistCicloNaoIncluir(id);
            var tiposArquivos =await _tipoArquivoRepository.ObterTiposArquivosDoCiclo(id);
            var tiposArquivosGeral = await _tipoArquivoRepository.ObterTiposGeral();
            var arquivos = await _arquivoRepository.ObterRevisadosPeloCiclo(id);
            var cicloAlunos = await _cicloAlunoRepository.ObterTodosPorCicloUntracked(id);
            var clienteArquivos = await _clienteArquivoRepository.ObterPeloCiclo(id);
            var arquivosGerais = await _arquivoRepository.ObterArquivosGerais();

            try
            {
                MemoryStream tabelaExcel = await ExcelSpreadsheetService.ExportarRelatorio(ciclo,clientes,tiposArquivos,cicloChecklists,arquivos,cicloAlunos, clienteArquivos, arquivosGerais, tiposArquivosGeral);

                byte[] bytes = tabelaExcel.ToArray();

                return Convert.ToBase64String(bytes);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Erro ao recuperar relatório!");
            }
            
        }

        [HttpPost("baixar-relatorio-custom/{id}")]
        public async Task<string> BaixarRelatorioCustom(int id)
        {
            var ciclo = await _cicloRepository.ObterPorId(id);
            var dados_relatorio = await _contatoRepository.ObterDadosRelatorioContrato();
            try
            {
                MemoryStream tabelaExcel = await ExcelSpreadsheetService.ExportarRelatorioContato(dados_relatorio, AppBaseUrl);

                byte[] bytes = tabelaExcel.ToArray();

                return Convert.ToBase64String(bytes);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Erro ao recuperar relatório!");
            }
            
        }

        [HttpPost("baixar-relatorio-todos-ciclos/{id_cliente}")]
        public async Task<JsonResult> BaixarRelatorioTodosCiclos(int id_cliente)
        {
            var dados_relatorio = await _arquivoRepository.ObterArquivosPorIdAluno(id_cliente);
            var usuarios = await _usuarioRepository.ObterTodos();
            var ciclos = await _cicloRepository.ObterTodos();
            var cliente = await _clienteRepository.ObterPorId(id_cliente);
            var tipos_arquivos = await _tipoArquivoRepository.ObterTodos();

            try
            {
                MemoryStream tabelaExcel = await ExcelSpreadsheetService.RelatorioTodosCiclos(dados_relatorio, usuarios, ciclos, tipos_arquivos);

                byte[] bytes = tabelaExcel.ToArray();

                return Json(new
                {
                    base64 = Convert.ToBase64String(bytes),
                    nome = cliente.Nome.Replace(".", "") + " arquivos " + DateTime.Now.ToBrazilLocalTime().ToString("dd/MM/yyyy")
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Erro ao recuperar relatório!");
            }
            
        }
        
        [HttpPost("baixar-relatorio-logs/{id_cliente}")]
        public async Task<JsonResult> BaixarRelatorioLogs(int id_cliente)
        {
            var dados_relatorio = await _logRepository.ObterPorIdAluno(id_cliente);
            var arqs = await _arquivoRepository.ObterArquivosPorIdAluno(id_cliente);
            var usuarios = await _usuarioRepository.ObterTodos();
            var ciclos = await _cicloRepository.ObterTodos();
            var cliente = await _clienteRepository.ObterPorId(id_cliente);
            var tipos_arquivos = await _tipoArquivoRepository.ObterTodos();

            try
            {
                MemoryStream tabelaExcel = await ExcelSpreadsheetService.RelatorioLogs(dados_relatorio, arqs, usuarios, ciclos, tipos_arquivos);

                byte[] bytes = tabelaExcel.ToArray();

                return Json(new
                {
                    base64 = Convert.ToBase64String(bytes),
                    nome = cliente.Nome.Replace(".", "") + " logs " + DateTime.Now.ToBrazilLocalTime().ToString("dd/MM/yyyy")
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Erro ao recuperar relatório!");
            }
            
        }
    }
}
