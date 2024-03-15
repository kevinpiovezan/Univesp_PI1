using Microsoft.EntityFrameworkCore;
using Univesp.CaminhoDoMar.ProjetoIntegradorApplicationCore.Business;
using Univesp.CaminhoDoMar.ProjetoIntegradorApplicationCore.Interfaces.Repository;
using Univesp.CaminhoDoMar.ProjetoIntegradorInfrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Univesp.CaminhoDoMar.ProjetoIntegradorInfrastructure.Data.Repositories
{
    public class TipoArquivoRepository : Repository<Tipo_Arquivo>, ITipoArquivoRepository
    {
        public TipoArquivoRepository(AppDbContext context) : base(context)
        {
            
            
        }

        public async Task<List<Tipo_Arquivo>> ObterTiposArquivosDoCiclo(int id)
        {
            var result = await 
                (from cc in _Context.Ciclo_Checklist
                join ta in _Context.Tipo_Arquivo
                    on cc.Id_Tipo_Arquivo equals ta.Id
                where cc.Valor != "Não Incluir"
                select ta).AsNoTracking().Distinct().ToListAsync();
            return result;
        }
        public async Task<List<Tipo_Arquivo>> ObterTiposGeral()
        {
            var result = await 
                (from ta in _Context.Tipo_Arquivo
                where ta.Fora_De_Ciclo == true
                select ta).AsNoTracking().Distinct().ToListAsync();
            return result;
        }
    }
}
