using Syngenta.Casa.RenCad.AppHub.ApplicationCore.Business;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Syngenta.Casa.RenCad.AppHub.ApplicationCore.Interfaces.Repository
{
    public interface ICicloChecklistRepository : IRepository<Ciclo_Checklist>
    {
        Task<Ciclo_Checklist> ObterCheck(int id_ciclo, int id_tipo_arquivo, int id_tipo_cliente);
        Task<List<Ciclo_Checklist>> ObterChecklistCliente(int id_ciclo, int id_tipo_cliente);
        Task<List<Ciclo_Checklist>> ObterChecklistCicloNaoIncluir(int id_ciclo);
        Task<List<Ciclo_Checklist>> ObterChecklistCiclo(int id_ciclo);
    }
}
