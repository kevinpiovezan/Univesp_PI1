namespace Univesp.CaminhoDoMar.ProjetoIntegradorApplicationCore.Interfaces.Service
{
    public interface IIdentityService
    {
        string ObterNome();
        string ObterEmail();
        bool IsAuthenticated();
    }
}
