using Microsoft.Extensions.DependencyInjection;
using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.Interfaces.Repository;
using Univesp.CaminhoDoMar.ProjetoIntegrador.Infrastructure.Data.Repositories;
using Univesp.CaminhoDoMar.ProjetoIntegradorApplicationCore.Interfaces.Service;
using Univesp.CaminhoDoMar.ProjetoIntegradorInfrastructure.Data.Context;
using Univesp.CaminhoDoMar.ProjetoIntegradorInfrastructure.Data.Repositories;
using Univesp.CaminhoDoMar.ProjetoIntegradorInfrastructure.Services;

namespace Univesp.CaminhoDoMar.ProjetoIntegrador.Infrastructure.Configurations
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // DbContexts
            services.AddScoped<AppDbContext>();
            services.AddScoped<IAlunoRepository, AlunoRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<ITabelaRepository, TabelaRepository>();
            services.AddScoped<IListaRepository, ListaRepository>();
            // Services
            services.AddTransient<IIdentityService, IdentityService>();
            // services.AddScoped<IAmazonS3StorageService, AmazonS3StorageService>()
            //     .AddOptions<AmazonS3Options>()
            //     .Configure(options => {
            //         options.AppEnvironment = AppSettings.GetEnv().ToLower();
            //         options.AppFolder = AppSettings.Configuration.GetSection("AmazonS3")["FolderName"];
            //     });

            return services;
        }
        
    }
}
