using Microsoft.EntityFrameworkCore;
using Univesp.CaminhoDoMar.ProjetoIntegradorApplicationCore.Business;
using Univesp.CaminhoDoMar.ProjetoIntegradorApplicationCore.Interfaces.Repository;
using Univesp.CaminhoDoMar.ProjetoIntegradorInfrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Univesp.CaminhoDoMar.ProjetoIntegradorInfrastructure.Data.Repositories
{
    public class CicloRenovacaoRepository : Repository<Ciclo_Renovacao>, ICicloRenovacaoRepository
    {
        public CicloRenovacaoRepository(AppDbContext context) : base(context) { }

        public async Task DesativaTodosOsCiclos()
        {
            var cs = await _Context.Ciclo_Renovacao.Where(c => c.Ativo == true).ToListAsync();

            foreach(var c in cs)
            {
                c.Ativo = false;
            }

            await _Context.SaveChangesAsync();
        }

        public async Task<Ciclo_Renovacao> ObterCicloPeloAno(int ano)
        {
            return await _Context.Ciclo_Renovacao.FirstOrDefaultAsync(f => Regex.IsMatch(f.Nome, $"{ano}"));
        }

        public async Task<Ciclo_Renovacao> ObterCicloAtivo()
        {
            return await _Context.Ciclo_Renovacao.FirstOrDefaultAsync(f => f.Ativo);
        }
    }
}
