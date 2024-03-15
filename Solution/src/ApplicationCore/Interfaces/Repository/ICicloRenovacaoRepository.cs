using Syngenta.Casa.RenCad.AppHub.ApplicationCore.Business;
using System;
using System.Threading.Tasks;

namespace Syngenta.Casa.RenCad.AppHub.ApplicationCore.Interfaces.Repository
{
    public interface ICicloRenovacaoRepository : IRepository<Ciclo_Renovacao>
    {
        Task DesativaTodosOsCiclos();
        Task<Ciclo_Renovacao> ObterCicloPeloAno(int ano);

        Task<Ciclo_Renovacao> ObterCicloAtivo();
    }
}
