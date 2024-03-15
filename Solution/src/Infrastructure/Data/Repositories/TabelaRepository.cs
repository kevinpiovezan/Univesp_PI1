using Microsoft.EntityFrameworkCore;
using Univesp.CaminhoDoMar.ProjetoIntegradorInfrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.Business;
using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.Interfaces.Repository;

namespace Univesp.CaminhoDoMar.ProjetoIntegradorInfrastructure.Data.Repositories
{
    public class TabelaRepository : Repository<Tabela>, ITabelaRepository
    {
        public TabelaRepository(AppDbContext context) : base(context) { }

        public async Task<Tabela> ObterPorNome(string nome)
        {
            return await _Context.Tabelas.Where(t => t.Nome == nome).FirstOrDefaultAsync();
        }

        public async Task<List<Tabela>> ObterTabelasPorLista(List<string> nomes)
        {
            return await _Context.Tabelas.Where(t => nomes.Contains(t.Nome)).ToListAsync();
        }
    }
}
