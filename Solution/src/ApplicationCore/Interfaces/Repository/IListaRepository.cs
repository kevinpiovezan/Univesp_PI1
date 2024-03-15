using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.Business;
using System;
using System.Collections.Generic;
using System.Text;

namespace Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.Interfaces.Repository
{
    public interface IListaRepository : IRepository<Lista>
    {
        List<string> ObterPorNome(string nome);
        Dictionary<string, List<string>> ObterTodosParsed(bool upper = false);
    }
}
