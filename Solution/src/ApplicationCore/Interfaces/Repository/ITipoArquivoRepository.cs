using Syngenta.Casa.RenCad.AppHub.ApplicationCore.Business;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Syngenta.Casa.RenCad.AppHub.ApplicationCore.Interfaces.Repository
{
    public interface ITipoArquivoRepository : IRepository<Tipo_Arquivo>
    {
        public Task<List<Tipo_Arquivo>> ObterTiposArquivosDoCiclo(int id);
        public Task<List<Tipo_Arquivo>> ObterTiposGeral();
    }
}
