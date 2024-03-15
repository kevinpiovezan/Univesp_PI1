using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace Univesp.CaminhoDoMar.ProjetoIntegradorInfrastructure.Services.Email
{
    //Referências: https://docs.microsoft.com/en-us/dotnet/architecture/microservices/multi-container-microservice-net-applications/background-tasks-with-ihostedservice
    //
    public class TimedHostedService : BackgroundService
    {
        private readonly IServiceScopeFactory scopeFactory;
        private TimeSpan IntervaloExecucao = TimeSpan.FromMinutes(10);
        public TimedHostedService(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                
            }

        }
    }
}
