using Application.DTOs.Pedido;
using Application.UseCase.Pedidos;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Application
{
    [ExcludeFromCodeCoverage]
    public static class ServiceApplicationExtensions
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddScoped<IPedidoUseCase, PedidoUseCase>();

            var config = new MapperConfiguration(cfg =>
            {          
                cfg.CreateMap<PedidoDto, Pedido>().ReverseMap()
                .ForMember(x => x.Status, opt => opt.MapFrom(u => u.Status.GetEnumDescription()));                     
            });

            IMapper mapper = config.CreateMapper();

            services.AddSingleton(mapper);

            return services;
        }

        public static string GetEnumDescription(this Enum value)
        {
            if (value == null) { return ""; }

            DescriptionAttribute? attribute = value.GetType()
                    .GetField(value.ToString())
                    ?.GetCustomAttributes(typeof(DescriptionAttribute), false)
                    .SingleOrDefault() as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}

