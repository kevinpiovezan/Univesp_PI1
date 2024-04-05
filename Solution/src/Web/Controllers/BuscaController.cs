using Microsoft.AspNetCore.Mvc;
using Univesp.CaminhoDoMar.ProjetoIntegradorWeb.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.Business;
using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.DTOs;
using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.Interfaces.Repository;
using Univesp.CaminhoDoMar.ProjetoIntegradorApplicationCore.Interfaces.Service;
using Univesp.CaminhoDoMar.ProjetoIntegradorInfrastructure.Services;

namespace Univesp.CaminhoDoMar.ProjetoIntegradorWeb.Controllers
{
    [Route("busca")]
    public class BuscaController : BaseController
    {
        private readonly IAlunoRepository _alunoRepository;
        private readonly IIdentityService _identityService;


        public BuscaController(IUsuarioRepository usuarioRepository, IIdentityService identityService,
            
            IAlunoRepository alunoRepository) : base(usuarioRepository, identityService)
        {
           
            _alunoRepository = alunoRepository;
            _identityService = identityService;
        }

        public async Task<IActionResult> Index()
        {
            Usuario usuarioLogado = _usuarioRepository.ObterPorEmail(_identityService.ObterEmail());
            var model = new FiltersBusca();

            return View(model);
        }

        [HttpPost("load_table")]
        public async Task<JsonResult> ObterProcessosPorPagina([FromBody] DtParameters dtParameters)
        {
            var res = await _alunoRepository.ObtemAlunosBusca(dtParameters.Start, dtParameters.Length, dtParameters.Order.FirstOrDefault(), dtParameters.Filters);
            foreach (var rb in res.ResultadosBusca)
            {
                var a = rb.DadosAluno;
            }

            return Json(new
            {
                draw = dtParameters.Draw,
                recordsTotal = res.Qtd_Total,
                recordsFiltered = res.Qtd_Total,
                data = res.ResultadosBusca
            });
        }
        
        [HttpPost("baixar-relatorio-filtrado")]
        public async Task<string> BaixarRelatorio([FromBody] SearchFiltersDTO filtros)
        {
            var alunos = await _alunoRepository.ObtemAlunosPorParametrosDeBuscaNoTracking(filtros);

            try
            {
                MemoryStream tabelaExcel = await ExcelSpreadsheetService.ExportarRelatorio(alunos);

                byte[] bytes = tabelaExcel.ToArray();

                return Convert.ToBase64String(bytes);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Erro ao recuperar relatório!");
            }
            
        }
    }
    
}
