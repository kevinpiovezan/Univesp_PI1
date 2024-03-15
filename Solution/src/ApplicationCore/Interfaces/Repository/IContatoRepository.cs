using Syngenta.Casa.RenCad.AppHub.ApplicationCore.Business;
using Syngenta.Casa.RenCad.AppHub.ApplicationCore.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Syngenta.Casa.RenCad.AppHub.ApplicationCore.Interfaces.Repository
{
    public interface IContatoRepository : IRepository<Contato>
    {
        public Task<List<Contato>> ObterContatosCliente(int idCliente);
        public Task<List<RelatorioContatoDTO>> ObterDadosRelatorioContrato();
    }
}
