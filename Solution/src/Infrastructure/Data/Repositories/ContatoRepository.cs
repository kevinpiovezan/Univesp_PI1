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

namespace Univesp.CaminhoDoMar.ProjetoIntegradorInfrastructure.Data.Repositories
{
    public class ContatoRepository : Repository<Contato>, IContatoRepository
    {
        public ContatoRepository(AppDbContext context) : base(context) { }

        public async Task<List<Contato>> ObterContatosAluno(int idAluno)
        {
            return await _Context.Contato.Where(c => c.Id_Aluno == idAluno).ToListAsync();
        }
        public async Task<List<RelatorioContatoDTO>> ObterDadosRelatorioContrato()
        {
            return await (from c in _Context.Contato
                     join u in _Context.Usuarios
                     on c.Id_Usuario equals u.Id
                     join ciclo in _Context.Ciclo_Renovacao
                     on c.Id_Ciclo equals ciclo.Id
                     join cliente in _Context.Aluno
                     on c.Id_Aluno equals cliente.Id
                     select new RelatorioContatoDTO()
                     {
                         Ciclo = ciclo,
                         Contato = c,
                         Aluno = cliente,
                         Usuario = u,
                     }).AsNoTracking().ToListAsync();
        }
    }
}
