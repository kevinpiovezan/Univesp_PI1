using Microsoft.Extensions.Configuration;

namespace Univesp.CaminhoDoMar.ProjetoIntegradorInfrastructure
{
    public static class AppSettings
    {
        public static IConfiguration Configuration;

        public static string GetEnv() => AppSettings.Configuration.GetConnectionString("DeployedEnvironment");
        public static bool IsProduction() => AppSettings.GetEnv() == "Prod";
        public static bool IsDevelopment() => AppSettings.GetEnv() == "Dev";
    }
}
