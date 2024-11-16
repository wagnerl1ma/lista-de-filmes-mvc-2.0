using ListaDeFilmes.App.Extensions;
using ListaDeFilmes.Business.Interfaces;
using ListaDeFilmes.Business.Notificacoes;
using ListaDeFilmes.Business.Services;
using ListaDeFilmes.Data.Context;
using ListaDeFilmes.Data.Repository;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;

namespace ListaDeFilmes.App.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<ListaDeFilmesContext>();

            services.AddScoped<IFilmeRepository, FilmeRepository>();
            services.AddScoped<IGeneroRepository, GeneroRepository>();

            services.AddSingleton<IValidationAttributeAdapterProvider, MoedaValidationAttributeAdapterProvider>();

            services.AddScoped<INotificador, Notificador>();

            services.AddScoped<IFilmeService, FilmeService>();
            services.AddScoped<IGeneroService, GeneroService>();

            return services;
        }
    }
}
