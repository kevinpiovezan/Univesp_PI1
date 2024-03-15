using Microsoft.EntityFrameworkCore;
using Univesp.CaminhoDoMar.ProjetoIntegradorApplicationCore.Business;
using Univesp.CaminhoDoMar.ProjetoIntegradorApplicationCore.DTOs;
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
    public class CicloAlunoRepository : Repository<Ciclo_Aluno>, ICicloAlunoRepository
    {
        public CicloAlunoRepository(AppDbContext context) : base(context) { }

        public Task<List<Ciclo_Aluno>> ObterTodosPorCiclo(int id_ciclo)
        {
            return _Context.Ciclo_Aluno.Where(cc => cc.Id_Ciclo_Renovacao == id_ciclo).ToListAsync();
        }
        public Task<List<Ciclo_Aluno>> ObterTodosPorCicloUntracked(int id_ciclo)
        {
            return _Context.Ciclo_Aluno.Where(cc => cc.Id_Ciclo_Renovacao == id_ciclo).AsNoTracking().ToListAsync();
        }

        public async Task<UltimaModificacaoCicloDTO> ObterUltimaModificacao(int id_ciclo)
        {
            return await 
                    (from c in _Context.Ciclo_Renovacao
                    join u1 in _Context.Usuarios
                    on c.Id_Usuario_Alteracao_Checklist equals u1.Id
                    join u2 in _Context.Usuarios
                    on c.Id_Usuario_Alteracao_Alunos equals u2.Id
                    where c.Id == id_ciclo
                    select new UltimaModificacaoCicloDTO()
                    {
                        Usuario_Alteracao_Checklist = u1,
                        Usuario_Alteracao_Alunos = u2,
                        Data_Alteracao_Checklist = c.Data_Alteracao_Checklist.ToString("dd/MM/yyyy HH:mm:ss"),
                        Data_Alteracao_Alunos = c.Data_Alteracao_Alunos.ToString("dd/MM/yyyy HH:mm:ss")
                    }).FirstOrDefaultAsync();
                    
        }

        public async Task<List<Ciclo_Aluno>> ObterPorIdAluno(int idAluno)
        {
            return await _Context.Ciclo_Aluno.Where(f => f.Id_Aluno == idAluno).AsNoTracking().ToListAsync();
        }

        public async Task<Ciclo_Aluno> ObterPorIdAlunoECiclo(int idAluno, int idCiclo)
        {
            return await _Context.Ciclo_Aluno.FirstOrDefaultAsync( f => f.Id_Aluno == idAluno && f.Id_Ciclo_Renovacao == idCiclo);
        }

        public async Task<string> AlteraStatusCicloAluno(int idAluno, int idCiclo, bool concluir, bool reabrir, bool arquivoNovo,List<Ciclo_Checklist> checklist, List<Arquivo> arquivos_ciclo, bool excluir)
        {
            var cliente = _Context.Aluno.FirstOrDefault(f => f.Id == idAluno);
            var cicloAluno = await ObterPorIdAlunoECiclo(idAluno, idCiclo);
            var progresso = cliente?.CalculaProgresso(idCiclo, checklist, arquivos_ciclo);

            if (progresso == 0)
            {
                if (concluir)
                {
                    cicloAluno.Id_Status_Aluno_Ciclo = (int)EStatusAlunoCiclo.ENV_P_CREDITO_INCOMPLETO;
                } else if (reabrir)
                {
                    cicloAluno.Id_Status_Aluno_Ciclo = (int)EStatusAlunoCiclo.PENDENTE;
                    cicloAluno.Data_Finalizacao = null;
                } else if (arquivoNovo)
                {
                    cicloAluno.Id_Status_Aluno_Ciclo = (int)EStatusAlunoCiclo.RECEBIDO_INCOMPLETO;
                }
                else if (excluir)
                {
                    cicloAluno.Id_Status_Aluno_Ciclo = (int)EStatusAlunoCiclo.PENDENTE;
                }
                else
                {
                    cicloAluno.Id_Status_Aluno_Ciclo = (int)EStatusAlunoCiclo.PENDENTE;
                }
            }
            else if (progresso > 0 && progresso < 100)
            {
                if (concluir)
                {
                    cicloAluno.Id_Status_Aluno_Ciclo = (int)EStatusAlunoCiclo.ENV_P_CREDITO_INCOMPLETO;
                } else if (reabrir)
                {
                    cicloAluno.Id_Status_Aluno_Ciclo = (int)EStatusAlunoCiclo.RECEBIDO_INCOMPLETO;
                    cicloAluno.Data_Finalizacao = null;
                } else if (arquivoNovo)
                {
                    cicloAluno.Id_Status_Aluno_Ciclo = (int)EStatusAlunoCiclo.RECEBIDO_INCOMPLETO;
                } else if (excluir)
                {
                    cicloAluno.Id_Status_Aluno_Ciclo = (int)EStatusAlunoCiclo.RECEBIDO_INCOMPLETO;
                }
            } 
            else if (progresso >= 100)
            {
                if (concluir)
                {
                    cicloAluno.Id_Status_Aluno_Ciclo = (int)EStatusAlunoCiclo.ENV_P_CREDITO;
                } else if (reabrir)
                {
                    cicloAluno.Id_Status_Aluno_Ciclo = (int)EStatusAlunoCiclo.RECEBIDO_COMPLETO;
                    cicloAluno.Data_Finalizacao = null;

                }
                else if (arquivoNovo)
                {
                    cicloAluno.Id_Status_Aluno_Ciclo = (int)EStatusAlunoCiclo.RECEBIDO_COMPLETO;
                }
                else if (excluir)
                {
                    cicloAluno.Id_Status_Aluno_Ciclo = (int)EStatusAlunoCiclo.RECEBIDO_COMPLETO;
                }
                else
                {
                    cicloAluno.Id_Status_Aluno_Ciclo = (int)EStatusAlunoCiclo.RECEBIDO_COMPLETO;
                }
            }

            return "Ok";
        }
    }
}
