using Syngenta.Casa.RenCad.AppHub.ApplicationCore.Business;
using Syngenta.Casa.RenCad.AppHub.ApplicationCore.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Syngenta.Casa.RenCad.AppHub.ApplicationCore.Interfaces.Repository
{
    public interface IClienteArquivoRepository : IRepository<Cliente_Arquivo>
    {
        public Task<List<Cliente_Arquivo>> ObterPeloCiclo(int id);
    }
}
