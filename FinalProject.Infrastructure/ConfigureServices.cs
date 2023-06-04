using FinalProject.Application.Common.Interfaces;
using FinalProject.Infrastructure.Persistance.Configurations.Context;
using FinalProject.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinalProject.Infrastructure
{
    public static class ConfigureServices
    {
        public static void AddInfratstructureServices(this IServiceCollection services)
        {
            services.AddSingleton<IEmailService,EmailManager>();
        }
    }
}
