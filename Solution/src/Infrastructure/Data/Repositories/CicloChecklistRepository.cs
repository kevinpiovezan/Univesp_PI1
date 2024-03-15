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
    public class CicloChecklistRepository : Repository<Ciclo_Checklist>, ICicloChecklistRepository
    {
        public CicloChecklistRepository(AppDbContext context) : base(context) { }

        public async Task<Ciclo_Checklist> ObterCheck(int id_ciclo, int id_tipo_arquivo, int id_tipo_cliente)
        {
            return await _Context.Ciclo_Checklist.Where(cc => cc.Id_Ciclo == id_ciclo && cc.Id_Tipo_Arquivo == id_tipo_arquivo && cc.Id_Tipo_Aluno == id_tipo_cliente).FirstOrDefaultAsync();
        }

        public async Task<List<Ciclo_Checklist>> ObterChecklistCiclo(int id_ciclo)
        {
            return await _Context.Ciclo_Checklist.Where(cc => cc.Id_Ciclo == id_ciclo).ToListAsync();

        }
        
        public async Task<List<Ciclo_Checklist>> ObterChecklistCicloNaoIncluir(int id_ciclo)
        {
            return await _Context.Ciclo_Checklist.Where(cc => cc.Id_Ciclo == id_ciclo && cc.Valor != "Não Incluir").AsNoTracking().ToListAsync();

        }

        public async Task<List<Ciclo_Checklist>> ObterChecklistAluno(int id_ciclo, int id_tipo_cliente)
        {
            return await _Context.Ciclo_Checklist.Where(cc => cc.Id_Ciclo == id_ciclo && cc.Id_Tipo_Aluno == id_tipo_cliente).ToListAsync();

        }
    }
}
