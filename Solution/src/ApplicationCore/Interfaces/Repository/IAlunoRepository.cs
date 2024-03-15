using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.Business;
using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.Interfaces.Repository
{
    public interface IAlunoRepository : IRepository<Aluno>
    {

        public Task<Aluno> ObtemAlunoPorCPF(string cpf_cnpj);
        public Task<List<Aluno>> ObtemAlunosPorParametrosDeBuscaNoTracking(SearchFiltersDTO filters);
        public Task<BuscaDTO> ObtemAlunosBusca(int start, int length, DtOrder order, SearchFiltersDTO filters);
    }
}
