using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.Business;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.Interfaces.Repository
{
    public interface ITabelaRepository : IRepository<Tabela>
    {
        Task<Tabela> ObterPorNome(string nome);
        Task<List<Tabela>> ObterTabelasPorLista(List<string> nomes);
    }
}
