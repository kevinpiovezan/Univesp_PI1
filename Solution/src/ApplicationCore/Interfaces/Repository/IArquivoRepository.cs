using Syngenta.Casa.RenCad.AppHub.ApplicationCore.Business;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Syngenta.Casa.RenCad.AppHub.ApplicationCore.Interfaces.Repository
{
    public interface IArquivoRepository : IRepository<Arquivo>
    {
        public Task<List<Arquivo>> ObterArquivosPorIdCliente(int id);
        public Task<List<Arquivo>> ObterArquivoMultiplosDoMesmoTipo(int idCiclo, int idTipo, int idCliente, int ano);
        public Task<Arquivo> ObterArquivo(int idCiclo, int idArquivo, int idCliente, int ano);
        public Task<List<Arquivo>> ObterPendentesPeloCiclo(int idCiclo);
        
        public Task<List<Arquivo>> ObterRevisadosPeloCiclo(int idCiclo);
        public Task<List<Arquivo>> ObterArquivosDoClientePeloTipoEAno(int idCiclo, int idCliente, int idTipoArquivo, int ano);
        public Task<List<Arquivo>> ObterArquivosGeraisDoClientePorTipo(int idCliente, int idTipoArquivo);
        public Task<List<Arquivo>> ObterArquivosGerais();
    }
}
