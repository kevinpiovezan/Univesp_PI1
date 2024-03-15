using Syngenta.Casa.RenCad.AppHub.ApplicationCore.Business;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Syngenta.Casa.RenCad.AppHub.ApplicationCore.Interfaces.Repository
{
    public interface ILogAtividadeRepository : IRepository<LogAtividade>
    {
        Task<List<LogAtividade>> ObterPorIdCliente(int id_cliente);
    }
}
