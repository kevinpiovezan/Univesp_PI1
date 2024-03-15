using AppHub.Syngenta.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AppHub.Syngenta.Common.Services.Storage.AmazonS3;
using Univesp.CaminhoDoMar.ProjetoIntegradorApplicationCore.Business;
using Univesp.CaminhoDoMar.ProjetoIntegradorApplicationCore.DTOs;
using Univesp.CaminhoDoMar.ProjetoIntegradorApplicationCore.Interfaces.Repository;
using Univesp.CaminhoDoMar.ProjetoIntegradorApplicationCore.Interfaces.Service;
using Univesp.CaminhoDoMar.ProjetoIntegradorInfrastructure.Services;
using Univesp.CaminhoDoMar.ProjetoIntegradorWeb.Models;
using Syngenta.Casa.RenCad.ApplicationCore.Enums;

namespace Univesp.CaminhoDoMar.ProjetoIntegradorWeb.Controllers
{
    [Route("arquivo")]
    public class ArquivoController : Controller
    {
        private readonly IAlunoRepository _clienteRepository;
        private readonly IAlunoArquivoRepository _clienteArquivoRepository;
        private readonly ICicloAlunoRepository _cicloAlunoRepository;
        private readonly ICicloChecklistRepository _cicloChecklistRepository;
        private readonly IAmazonS3StorageService _s3Service;
        private readonly IIdentityService _identityService;
        private readonly IArquivoRepository _arquivoRepository;
        private readonly ITipoArquivoRepository _tipoArquivoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ICicloRenovacaoRepository _cicloRepository;
        private readonly ILogAtividadeRepository _logAtividadeRepository;

        public ArquivoController(IAlunoArquivoRepository clienteArquivoRepository,ICicloChecklistRepository cicloChecklistRepository,ICicloAlunoRepository cicloAlunoRepository,IAlunoRepository clienteRepository,ICicloRenovacaoRepository cicloRepository, IAmazonS3StorageService s3Service, IArquivoRepository arquivoRepository, ITipoArquivoRepository tipoArquivoRepository, IUsuarioRepository usuarioRepository, IIdentityService identityService,ILogAtividadeRepository logAtividadeRepository)
        {
            _s3Service = s3Service;
            _arquivoRepository = arquivoRepository;
            _tipoArquivoRepository = tipoArquivoRepository;
            _usuarioRepository = usuarioRepository;
            _identityService = identityService;
            _logAtividadeRepository = logAtividadeRepository;
            _cicloRepository = cicloRepository;
            _clienteRepository = clienteRepository;
            _cicloAlunoRepository = cicloAlunoRepository;
            _cicloChecklistRepository = cicloChecklistRepository;
            _clienteArquivoRepository = clienteArquivoRepository;
        }

        [HttpPost("adicionar-arquivo/{idCiclo}/{idArquivo}/{idAluno}/{obrigatorio}")]
        [RequestSizeLimit(500_000_000)] //500MB
        public async Task<IActionResult> RealizarUpload([FromBody] ArquivoDTO arquivoDto,[FromRoute] string idCiclo,string idArquivo,string idAluno, bool obrigatorio)
        {
            var usuarioLogado = _usuarioRepository.ObterPorEmail(_identityService.ObterEmail());
            var cliente = await _clienteRepository.ObterPorId(Convert.ToInt32(idAluno));
            var tipoArquivo = await _tipoArquivoRepository.ObterPorId(Convert.ToInt32(idArquivo));
            if (arquivoDto.Base64 == null)
                return BadRequest();
            using var stream = new MemoryStream(Convert.FromBase64String(arquivoDto.Base64.Split(new[] { ";base64," }, StringSplitOptions.RemoveEmptyEntries)[1]));

            string arquivoGUID = Guid.NewGuid().ToString();

            var uploadResult = await _s3Service.UploadObject(arquivoGUID, stream);
            try
            {
                if (!(uploadResult.HttpStatusCode == HttpStatusCode.OK))
                    return StatusCode(403, "Erro ao carregar arquivo no s3.");
                var arquivoExistente = await  _arquivoRepository.ObterArquivo(Convert.ToInt32(idCiclo), Convert.ToInt32(idArquivo), Convert.ToInt32(idAluno), arquivoDto.Ano);

                var arquivo = new Arquivo
                {
                    Id_Aluno = Convert.ToInt32(idAluno),
                    Nome = arquivoDto.Descricao.Split(".")[0],
                    Id_Ciclo_Renovacao = Convert.ToInt32(idCiclo),
                    Id_Status_Arquivo = (int)EStatusArquivo.AGUARDANDO_REVISAO,
                    Id_Tipo_Arquivo = Convert.ToInt32(idArquivo),
                    GUID = arquivoGUID,
                    Data_Upload = DateTime.Now.ToBrazilLocalTime(),
                    Id_Usuario = usuarioLogado.Id,
                    Size = stream.ToArray().Length,
                    Obrigatorio = obrigatorio,
                    Ano  = arquivoDto.Ano,
                    Excluido = false,
                    Tipo = arquivoDto.Tipo == "" ? null : arquivoDto.Tipo
                };
                arquivo.Comentario = arquivoDto.Comentario;

                if (!tipoArquivo.Multiplos && (usuarioLogado.Is_Admin || usuarioLogado.Revisor))
                {
                    arquivo.Id_Status_Arquivo = (int)EStatusArquivo.REVISADO;
                    arquivo.Id_Usuario_Revisor = usuarioLogado.Id;
                    arquivo.Data_Revisao = DateTime.Now.ToBrazilLocalTime();
                }
                await _arquivoRepository.Adicionar(arquivo);
                var log = new LogAtividade()
                {
                    Id_Aluno = arquivo.Id_Aluno,
                    Id_Usuario = arquivo.Id_Usuario,
                    Data = DateTime.Now.ToBrazilLocalTime(),
                    Id_Arquivo = arquivo.Id,
                    Id_Ciclo = arquivo.Id_Ciclo_Renovacao,
                    Id_Tipo_Atividade = (int)ETipoAtividade.ARQUIVO_ENVIADO,
                    Descricao = null
                };
                await _logAtividadeRepository.Adicionar(log);
                
                
                if (arquivo.Id_Ciclo_Renovacao != 0)
                {
                    var arquivosAluno = await _arquivoRepository.ObterArquivosPorIdAluno(cliente.Id);
                    var cicloChecklist =
                        await _cicloChecklistRepository.ObterChecklistAluno(Convert.ToInt32(idCiclo), cliente.Id_Tipo_Aluno);
                    await _cicloAlunoRepository.AlteraStatusCicloAluno(cliente.Id,Convert.ToInt32(idCiclo),false,false,true,cicloChecklist,arquivosAluno,false);
                    await _cicloAlunoRepository.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            return Json("Ok");
        }

        [HttpPost("reutilizar-arquivo/{idArquivo}/{idAluno}/{idCiclo}/{ano}")]
        public async Task<IActionResult> ReutilizarArquivo([FromRoute] string idCiclo,string idArquivo,string idAluno,string ano)
        {
            var usuarioLogado = _usuarioRepository.ObterPorEmail(_identityService.ObterEmail());
            var cliente = await _clienteRepository.ObterPorId(Convert.ToInt32(idAluno));
            int anoCiclo;
            Arquivo arquivoExistente = null;
            Arquivo arquivoCicloDiferente = null;
            Arquivo arquivoMesmoCiclo = null;
            if (ano == "0")
            {
                anoCiclo = _cicloRepository.ObterPorId(Convert.ToInt32(idCiclo)).Result.Ano - 1;
                Ciclo_Renovacao cicloAntigo = await _cicloRepository.ObterCicloPeloAno(anoCiclo);
                arquivoCicloDiferente = await  _arquivoRepository.ObterArquivo(cicloAntigo.Id, Convert.ToInt32(idArquivo), Convert.ToInt32(idAluno), Convert.ToInt32(anoCiclo));
            }
            else
            {
                anoCiclo = Convert.ToInt32(ano) - 1;
                arquivoMesmoCiclo = await  _arquivoRepository.ObterArquivo(Convert.ToInt32(idCiclo), Convert.ToInt32(idArquivo), Convert.ToInt32(idAluno), Convert.ToInt32(anoCiclo));
            }
            try
            {
                if (arquivoMesmoCiclo != null)
                {
                    arquivoExistente = arquivoMesmoCiclo;
                }
                else
                {
                    arquivoExistente = arquivoCicloDiferente;
                }
                if (arquivoExistente != null && arquivoExistente.Id_Status_Arquivo == (int)EStatusArquivo.REVISADO)
                {
                    var arquivo = new Arquivo
                    {
                        Data_Upload = DateTime.Now.ToBrazilLocalTime(),
                        Id_Usuario = usuarioLogado.Id,
                        Id_Ciclo_Renovacao = Convert.ToInt32(idCiclo),
                        Id_Status_Arquivo = (int)EStatusArquivo.AGUARDANDO_REVISAO,
                        Id_Aluno = arquivoExistente.Id_Aluno,
                        Id_Tipo_Arquivo = arquivoExistente.Id_Tipo_Arquivo,
                        Ano = Convert.ToInt32(ano),
                        Excluido = false,
                        Obrigatorio = arquivoExistente.Obrigatorio,
                        Nome = arquivoExistente.Nome,
                        Size = arquivoExistente.Size,
                        GUID = arquivoExistente.GUID,
                        Comentario = arquivoExistente.Comentario,
                        Tipo = arquivoExistente.Tipo
                    };
                    if (usuarioLogado.Is_Admin || usuarioLogado.Revisor)
                    {
                        arquivo.Id_Status_Arquivo = (int)EStatusArquivo.REVISADO;
                        arquivo.Id_Usuario_Revisor = usuarioLogado.Id;
                        arquivo.Data_Revisao = DateTime.Now.ToBrazilLocalTime();
                    }
                    await _arquivoRepository.Adicionar(arquivo);
                    var log = new LogAtividade()
                    {
                        Id_Aluno = arquivo.Id_Aluno,
                        Id_Usuario = usuarioLogado.Id,
                        Data = DateTime.Now.ToBrazilLocalTime(),
                        Id_Arquivo = arquivo.Id,
                        Id_Ciclo = arquivo.Id_Ciclo_Renovacao,
                        Id_Tipo_Atividade = (int)ETipoAtividade.ARQUIVO_ENVIADO,
                        Descricao = null
                    };
                    await _logAtividadeRepository.Adicionar(log);
                    
                    
                    var arquivosAluno = await _arquivoRepository.ObterArquivosPorIdAluno(cliente.Id);
                    var cicloChecklist =
                        await _cicloChecklistRepository.ObterChecklistAluno(Convert.ToInt32(idCiclo), cliente.Id_Tipo_Aluno);
                    
                    await _cicloAlunoRepository.AlteraStatusCicloAluno(cliente.Id,Convert.ToInt32(idCiclo),false,false,true,cicloChecklist,arquivosAluno,false);
                    await _cicloAlunoRepository.SaveChanges();
                }
                else
                {
                    return StatusCode(404, "Nenhum arquivo com status aprovado foi encontrado");
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            return Json("Ok");
        }

        [HttpGet("obter-arquivo/{idArquivo}")]
        public async Task<IActionResult> BaixarArquivo([FromRoute] string idArquivo)
        {

            var arquivo = await _arquivoRepository.ObterPorId(Convert.ToInt32(idArquivo));
            var response = await _s3Service.DownloadObjectStream(arquivo.GUID);
            var tipos = await _tipoArquivoRepository.ObterTodos();

            MemoryStream arquivoMemoryStream = new MemoryStream();
            response.CopyTo(arquivoMemoryStream);
            var ciclo = await _cicloRepository.ObterCicloAtivo();
            var tipoArquivos = await _tipoArquivoRepository.ObterTodos();
            var cliente = await _clienteRepository.ObterPorId(arquivo.Id_Aluno);
            string nome = $"{ciclo.Ano} - {cliente.Cod_Sap} - {(arquivo.Comentario != null ? arquivo.Comentario + " -" : "")} {arquivo.ObterNomeParaDownload(tipoArquivos.FirstOrDefault(f => f.Id == arquivo.Id_Tipo_Arquivo), ciclo.Ano)}";
            var arquivoDTO = new ArquivoDTO()
            {
                Base64 = Convert.ToBase64String(arquivoMemoryStream.ToArray()),
                Extensao = "pdf",
                Descricao = tipos.Where(t => t.Id == arquivo.Id_Tipo_Arquivo).FirstOrDefault().Descricao,
            };
            return Json(arquivoDTO);
        }

        [HttpGet("obter-arquivo-preview/{idArquivo}")]
        public async Task<IActionResult> PreviewArquivo([FromRoute] string idArquivo)
        {
            if (string.IsNullOrEmpty(idArquivo))
                return BadRequest();

            var arquivo = await _arquivoRepository.ObterPorId(Convert.ToInt32(idArquivo));
            var tipos = await _tipoArquivoRepository.ObterTodos();
            MemoryStream arquivoMemoryStream = new MemoryStream();
            var response = await _s3Service.DownloadObjectStream(arquivo.GUID);
            response.CopyTo(arquivoMemoryStream);
            response.Dispose();
            string usuarioRevisou = null;
            string dataRevisao = null;
            if (arquivo.Id_Usuario_Revisor != null)
            {
                var usuario = await _usuarioRepository.ObterPorId(arquivo.Id_Usuario_Revisor.Value);
                usuarioRevisou = usuario.Nome;
            }

            if (arquivo.Data_Revisao != null)
            {
                dataRevisao = arquivo.Data_Revisao.Value.ToString("dd/MM/yyyy HH:mm");
            }
            var arquivoDTO = new ArquivoDTO()
            {
                // Base64 = Convert.ToBase64String(arquivoMemoryStream.ToArray()),
                Base64Bytes = arquivoMemoryStream.ToArray(),
                Extensao = "pdf",
                Descricao = tipos.Where(t => t.Id == arquivo.Id_Tipo_Arquivo).FirstOrDefault().Descricao,
                UsuarioEnviou = _usuarioRepository.ObterPorId(arquivo.Id_Usuario).Result.Nome,
                DataEnvio = arquivo.Data_Upload.ToString("dd/MM/yyyy HH:mm"),
                UsuarioRevisou = usuarioRevisou,
                DataRevisao = dataRevisao,
            };
            return Json(arquivoDTO);
        }

        [HttpGet("obter-arquivos-multiplos-preview/{idArquivo}/{idCiclo}/{idAluno}/{ano}")]
        public async Task<IActionResult> ObterArquivosMultiplosPreview([FromRoute] string idArquivo, int idCiclo, string idAluno,int ano = 0)
        {
            if (string.IsNullOrEmpty(idArquivo))
                return BadRequest();
            var result = new List<ArquivoDTO>();
            var usuarios = await _usuarioRepository.ObterTodos();
            var tipos = await _tipoArquivoRepository.ObterTodos();
            var arquivos = await _arquivoRepository.ObterArquivosDoAlunoPeloTipoEAno(idCiclo, Convert.ToInt32(idAluno),Convert.ToInt32(idArquivo), ano);
            if (arquivos.Count == 0)
                arquivos = await _arquivoRepository.ObterArquivosGeraisDoAlunoPorTipo(Convert.ToInt32(idAluno), Convert.ToInt32(idArquivo));

            foreach (var arquivo in arquivos)
            {
                var response = await _s3Service.DownloadObjectStream(arquivo.GUID);
                MemoryStream arquivoMemoryStream = new MemoryStream();
                response.CopyTo(arquivoMemoryStream);
                response.Dispose();
                string usuarioRevisou = null;
                string dataRevisao = null;
                if (arquivo.Id_Usuario_Revisor != null)
                {
                    var usuario = usuarios.FirstOrDefault(f => f.Id == arquivo.Id_Usuario_Revisor.Value);
                    usuarioRevisou = usuario?.Nome;
                }

                if (arquivo.Data_Revisao != null)
                {
                    dataRevisao = arquivo.Data_Revisao.Value.ToBrazilLocalTime().ToString("dd/MM/yyyy HH:mm");
                }
                
                result.Add(new ArquivoDTO()
                {
                    Id = arquivo.Id,
                    // Base64 = Convert.ToBase64String(arquivoMemoryStream.ToArray()),
                    Base64Bytes = arquivoMemoryStream.ToArray(),
                    Extensao = "pdf",
                    Descricao = tipos.FirstOrDefault(t => t.Id == arquivo.Id_Tipo_Arquivo)?.Descricao,
                    UsuarioEnviou = _usuarioRepository.ObterPorId(arquivo.Id_Usuario).Result.Nome,
                    DataEnvio = arquivo.Data_Upload.ToBrazilLocalTime().ToString("dd/MM/yyyy HH:mm"),
                    UsuarioRevisou = usuarioRevisou,
                    DataRevisao = dataRevisao,
                    Nome = arquivo.Nome,
                    Cpf_cnpj = arquivo.Comentario,
                    Tipo = arquivo.Tipo,
                    Id_Ciclo = arquivo.Id_Ciclo_Renovacao,
                    ArquivoGeral = tipos.Any(w => w.Id == arquivo.Id_Tipo_Arquivo && w.Fora_De_Ciclo)
                });
            }
            
            return Json(result);
        }
        
        [HttpGet("obter-arquivos-multiplos/{idArquivo}/{idCiclo}/{idAluno}/{ano}")]
        public async Task<string> ObterArquivosMultiplos([FromRoute] string idArquivo, int idCiclo, string idAluno, int ano)
        {
            if (string.IsNullOrEmpty(idArquivo))
                return String.Empty;
            var result = new List<ArquivoDTO>();
            var arquivos = await _arquivoRepository.ObterArquivosDoAlunoPeloTipoEAno(idCiclo, Convert.ToInt32(idAluno),Convert.ToInt32(idArquivo), ano);
            if (arquivos.Count == 0)
                arquivos = await _arquivoRepository.ObterArquivosGeraisDoAlunoPorTipo(Convert.ToInt32(idAluno), Convert.ToInt32(idArquivo));
            
            var ciclo = await _cicloRepository.ObterPorId(idCiclo);
            var tipoArquivos = await _tipoArquivoRepository.ObterTodos();
            foreach (var arquivo in arquivos)
            {
                var response = await _s3Service.DownloadObjectStream(arquivo.GUID);
                var cliente = await _clienteRepository.ObterPorId(arquivo.Id_Aluno);
                MemoryStream arquivoMemoryStream = new MemoryStream();
                response.CopyTo(arquivoMemoryStream);
                string nome = $"{ciclo.Ano} - {cliente.Cod_Sap} - {(arquivo.Comentario != null ? arquivo.Comentario + " -" : "")} {arquivo.ObterNomeParaDownload(tipoArquivos.FirstOrDefault(f => f.Id == arquivo.Id_Tipo_Arquivo), ciclo.Ano)}";
                var arquivoDTO = new ArquivoDTO()
                {
                    Id = arquivo.Id,
                    Base64 = Convert.ToBase64String(arquivoMemoryStream.ToArray()),
                    Nome = nome+".pdf",
                };
                result.Add(arquivoDTO);
            }
            MemoryStream pacoteZip = CompressorArquivo.Zip(result);

            byte[] bytes = pacoteZip.ToArray();

            return Convert.ToBase64String(bytes);
        }

        [HttpPost("excluir/{id}")]
        public async Task<IActionResult> ExcluirArquivo(int id)
        {
            var usuarioLogado = _usuarioRepository.ObterPorEmail(_identityService.ObterEmail());
            if (usuarioLogado.Is_Admin == false && usuarioLogado.Revisor == false)
            {
                return StatusCode(403, "Usuário não tem permissão para realizar esta ação");
            }
        
            Arquivo a = await _arquivoRepository.ObterPorId(id);
            a.Excluido = true;

            bool tipoEhMultiplo = _tipoArquivoRepository.ObterTodos().Result.Where(x => x.Id == a.Id_Tipo_Arquivo).FirstOrDefault().Multiplos;
            if (tipoEhMultiplo)
            {
                List<Arquivo> arquivosMultiplosDoMesmoTipo = await _arquivoRepository.ObterArquivoMultiplosDoMesmoTipo(a.Id_Ciclo_Renovacao, a.Id_Tipo_Arquivo, a.Id_Aluno, a.Ano ?? 0);
                foreach (var arq in arquivosMultiplosDoMesmoTipo)
                {
                    arq.Id_Status_Arquivo = 1;
                    arq.Id_Usuario_Revisor = null;
                    arq.Data_Revisao = null;
                }
            }

            await _arquivoRepository.SaveChanges();
            var log = new LogAtividade()
            {
                Id_Aluno = a.Id_Aluno,
                Id_Usuario = usuarioLogado.Id,
                Data = DateTime.Now.ToBrazilLocalTime(),
                Id_Arquivo = a.Id,
                Id_Ciclo = a.Id_Ciclo_Renovacao,
                Id_Tipo_Atividade = (int)ETipoAtividade.ARQUIVO_REMOVIDO,
                Descricao = null
            };
            await _logAtividadeRepository.Adicionar(log);
            
            var cliente = await _clienteRepository.ObterPorId(a.Id_Aluno);
            if (a.Id_Ciclo_Renovacao != 0)
            {
                var arquivosAluno = await _arquivoRepository.ObterArquivosPorIdAluno(cliente.Id);
                var cicloChecklist =
                    await _cicloChecklistRepository.ObterChecklistAluno(a.Id_Ciclo_Renovacao, cliente.Id_Tipo_Aluno);
            
                await _cicloAlunoRepository.AlteraStatusCicloAluno(cliente.Id,a.Id_Ciclo_Renovacao,false,false,false,cicloChecklist,arquivosAluno,true);
                await _cicloAlunoRepository.SaveChanges();   
            }
            return Json("ok");
        }
    }
}
