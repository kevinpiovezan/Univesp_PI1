using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.Business;
using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Syngenta.Casa.RenCad.AppHub.ApplicationCore.Interfaces.Repository
{
    public interface ICicloClienteRepository : IRepository<Ciclo_Cliente>
    {
        Task<List<Ciclo_Cliente>> ObterTodosPorCiclo(int id_ciclo);
        Task<List<Ciclo_Cliente>> ObterTodosPorCicloUntracked(int id_ciclo);
        Task<UltimaModificacaoCicloDTO> ObterUltimaModificacao(int id_ciclo);
        Task<Ciclo_Cliente> ObterPorIdClienteECiclo(int idCliente, int idCiclo);
        Task<List<Ciclo_Cliente>> ObterPorIdCliente(int idCliente);
        Task<string> AlteraStatusCicloCliente(int idCliente, int idCiclo,bool concluir, bool reabrir, bool arquivoNovo,List<Ciclo_Checklist> checklist, List<Arquivo> arquivos_cicloList, bool naoEnvia);
    }
}
