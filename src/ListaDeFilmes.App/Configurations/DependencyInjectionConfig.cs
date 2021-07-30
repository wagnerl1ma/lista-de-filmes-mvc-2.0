using ListaDeFilmes.App.Extensions;
using ListaDeFilmes.Business.Interfaces;
using ListaDeFilmes.Data.Context;
using ListaDeFilmes.Data.Repository;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            return services;
        }
    }
}
