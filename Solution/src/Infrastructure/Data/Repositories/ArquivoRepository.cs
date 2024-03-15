using Microsoft.EntityFrameworkCore;
using Univesp.CaminhoDoMar.ProjetoIntegradorApplicationCore.Business;
using Univesp.CaminhoDoMar.ProjetoIntegradorApplicationCore.Interfaces.Repository;
using Univesp.CaminhoDoMar.ProjetoIntegradorInfrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syngenta.Casa.RenCad.ApplicationCore.Enums;

namespace Univesp.CaminhoDoMar.ProjetoIntegradorInfrastructure.Data.Repositories
{
    public class ArquivoRepository : Repository<Arquivo>, IArquivoRepository
    {
        public ArquivoRepository(AppDbContext context) : base(context) { }

        public async Task<List<Arquivo>> ObterArquivosPorIdAluno(int id)
        {
            return _Context.Arquivo.Where(w => w.Id_Aluno == id).AsNoTracking().ToList();
        }

        public async Task<Arquivo> ObterArquivo(int idCiclo, int idArquivo, int idAluno, int ano)
        {
            var arquivo = await _Context.Arquivo.FirstOrDefaultAsync(w =>
                w.Id_Ciclo_Renovacao == idCiclo && w.Id_Tipo_Arquivo == idArquivo && w.Id_Aluno == idAluno && (w.Ano == ano || w.Ano == 0) && !w.Excluido); 
            return arquivo;
        }

        public async Task<List<Arquivo>> ObterArquivoMultiplosDoMesmoTipo(int idCiclo, int idTipo, int idAluno, int ano)
        {
            var arquivos = await _Context.Arquivo.Where(a => a.Id_Ciclo_Renovacao == idCiclo && a.Id_Aluno == idAluno
            && (a.Ano == ano || a.Ano == 0) && a.Id_Tipo_Arquivo == idTipo).ToListAsync();
            return arquivos;
        }

        public async Task<List<Arquivo>> ObterPendentesPeloCiclo(int idCiclo)
        {
            return await (from a in _Context.Arquivo
                join c in _Context.Aluno
                    on a.Id_Aluno equals c.Id
                join cc in _Context.Ciclo_Aluno
                    on c.Id equals cc.Id_Aluno
                where a.Id_Status_Arquivo == (int)EStatusArquivo.AGUARDANDO_REVISAO &&
                      a.Id_Ciclo_Renovacao == idCiclo &&
                      !a.Excluido &&
                      cc.Id_Ciclo_Renovacao == idCiclo &&  
                      cc.Cancelado == false
                select a).AsNoTracking().ToListAsync();
        }

        public async Task<List<Arquivo>> ObterRevisadosPeloCiclo(int idCiclo)
        {
            return await (from a in _Context.Arquivo
                join c in _Context.Aluno
                    on a.Id_Aluno equals c.Id
                join cc in _Context.Ciclo_Aluno
                    on c.Id equals cc.Id_Aluno
                where 
                      a.Id_Ciclo_Renovacao == idCiclo &&
                      !a.Excluido &&
                      cc.Id_Ciclo_Renovacao == idCiclo &&  
                      cc.Cancelado == false
                select a).AsNoTracking().ToListAsync();
        }
        
        public async Task<List<Arquivo>> ObterArquivosDoAlunoPeloTipoEAno(int idCiclo, int idAluno, int idTipoArquivo, int ano)
        {
            return await _Context.Arquivo.Where(w =>
                w.Id_Aluno == idAluno && 
                w.Id_Tipo_Arquivo == idTipoArquivo && 
                w.Ano == ano &&
                !w.Excluido && w.Id_Ciclo_Renovacao == idCiclo
            ).ToListAsync();
        }
        public async Task<List<Arquivo>> ObterArquivosGeraisDoAlunoPorTipo(int idAluno, int idTipoArquivo)
        {
            return await _Context.Arquivo.Where(w =>
                w.Id_Aluno == idAluno && 
                w.Id_Tipo_Arquivo == idTipoArquivo &&
                !w.Excluido && w.Id_Ciclo_Renovacao == 0
            ).ToListAsync();
        }
        public async Task<List<Arquivo>> ObterArquivosGerais()
        {
            return await _Context.Arquivo.Where(w =>
                !w.Excluido && w.Id_Ciclo_Renovacao == 0
            ).ToListAsync();
        }
    }
}
