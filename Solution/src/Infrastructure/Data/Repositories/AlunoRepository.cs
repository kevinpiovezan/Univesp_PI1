﻿using Microsoft.EntityFrameworkCore;
using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.Business;
using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.DTOs;
using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.Interfaces.Repository;
using Univesp.CaminhoDoMar.ProjetoIntegradorInfrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Univesp.CaminhoDoMar.ProjetoIntegradorInfrastructure.Data.Repositories;

namespace Univesp.CaminhoDoMar.ProjetoIntegrador.Infrastructure.Data.Repositories
{
    public class AlunoRepository : Repository<Aluno>, IAlunoRepository
    {
        public AlunoRepository(AppDbContext context) : base(context) { }
        
        public async Task<List<Aluno>> ObtemAlunosPorParametrosDeBuscaNoTracking(SearchFiltersDTO filters)
        {
            return await (from a in _Context.Alunos
                where 
                    (filters.Nome == "" || a.Nome.ToLower().Contains(filters.Nome.ToLower())) &&
                    (filters.Cpf == "" || filters.Cpf == a.Cpf) &&
                    (!filters.Professor || a.Professor == filters.Professor) &&
                    (!filters.Autorizacao_Imagem || a.Autorizacao_Imagem == filters.Autorizacao_Imagem) &&
                    (!filters.Cursou_Faculdade || a.Cursou_Faculdade == filters.Cursou_Faculdade) &&
                    (!filters.EnsinoMedio_Escola_Publica || a.EnsinoMedio_Escola_Publica == filters.EnsinoMedio_Escola_Publica) &&
                    (!filters.Cadastro_SpTrans || a.Cadastro_SpTrans == filters.Cadastro_SpTrans) &&
                    (!filters.Servidor_Publico || a.Servidor_Publico == filters.Servidor_Publico)
                select a).AsNoTracking().Distinct().ToListAsync();
        }

        public async Task<Aluno> ObtemAlunoPorCPF(string cpf)
        {
            return await _Context.Alunos.Where(c => c.Cpf == cpf).FirstOrDefaultAsync();
        }
        
        public async Task<BuscaDTO> ObtemAlunosBusca(int start, int length, DtOrder order, SearchFiltersDTO filters)
        {
            var query = (from a in _Context.Alunos
                where 
                    (filters.Nome == "" || a.Nome.ToLower().Contains(filters.Nome.ToLower())) &&
                    (filters.Cpf == "" || filters.Cpf == a.Cpf) &&
                    (!filters.Professor || a.Professor == filters.Professor) &&
                    (!filters.Autorizacao_Imagem || a.Autorizacao_Imagem == filters.Autorizacao_Imagem) &&
                    (!filters.Cursou_Faculdade || a.Cursou_Faculdade == filters.Cursou_Faculdade) &&
                    (!filters.EnsinoMedio_Escola_Publica || a.EnsinoMedio_Escola_Publica == filters.EnsinoMedio_Escola_Publica) &&
                    (!filters.Cadastro_SpTrans || a.Cadastro_SpTrans == filters.Cadastro_SpTrans) &&
                    (!filters.Servidor_Publico || a.Servidor_Publico == filters.Servidor_Publico)

                select new BuscaRes()
                {
                    DadosAluno = a,
                });

            if(order!=null)
            {
                switch (order.Column)
                {
                    case 0:
                        query = order.Dir == "asc" ? query.OrderBy(c => c.DadosAluno.Cpf) : query.OrderByDescending(a => a.DadosAluno.Cpf);
                        break;
                    case 1:
                        query = order.Dir == "asc" ? query.OrderBy(c => c.DadosAluno.Nome) : query.OrderByDescending(a => a.DadosAluno.Nome);
                        break;
                    default:
                    // code block
                    break;
                }
            }

            return new BuscaDTO()
            {
                Qtd_Total = await query.CountAsync(),
                ResultadosBusca = await query.Skip(start).Take(length).AsNoTracking().ToListAsync()
            };
        }
    }
}
