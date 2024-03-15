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
    public class AlunoArquivoRepository : Repository<Aluno_Arquivo>, IAlunoArquivoRepository
    {
        public AlunoArquivoRepository(AppDbContext context) : base(context) { }

        public async Task<List<Aluno_Arquivo>> ObterPeloCiclo(int id)
        {
            return (await _Context.Aluno_Arquivo.Where(w => w.IdCicloRenovacao == id).AsNoTracking().ToListAsync());
        }
    }
}
