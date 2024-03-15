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
    public class LogAtividadeRepository : Repository<LogAtividade>, ILogAtividadeRepository
    {
        public LogAtividadeRepository(AppDbContext context) : base(context) { }
        public async Task<List<LogAtividade>> ObterPorIdAluno(int id_cliente)
        {
            return await _Context.Log_Atividades.Where(la => la.Id_Aluno == id_cliente).ToListAsync();
        }
    }
}
