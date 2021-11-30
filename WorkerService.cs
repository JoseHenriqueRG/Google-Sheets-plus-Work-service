using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Jobs.Google.Drive;
namespace Jobs
{
    public class WorkerService : BackgroundService
    {
        private IConfiguration Configuration { get; }


        public WorkerService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await ChangeSpreadsheet();

                var _time = Convert.ToInt32(Configuration["JobTime"]);

                await Task.Delay(_time, stoppingToken);
            }
        }

        private Task ChangeSpreadsheet()
        {
            new PlanilhaClassificacaoTecnica(Configuration).CreateService();

            return Task.CompletedTask;
        }
    }
}
