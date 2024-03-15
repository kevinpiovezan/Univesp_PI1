using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.Business;
using System;
using System.Threading.Tasks;

namespace Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.Interfaces.Repository
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        void AdicionarCasoInexistente(Usuario usuario);
        Task<Usuario> ObterUsuarioOuInserir(string email, string nome);
        Usuario ObterPorEmail(string email);
        Task<int> ObterIdOuInserir(string email, string nome);
    }
}
