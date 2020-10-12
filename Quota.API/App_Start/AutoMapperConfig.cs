using Application.Quota.Dtos;
using AutoMapper;
using Domain.Quota.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Quota.API.App_Start
{
    public class AutoMapperConfig
    {
        public static void Register(IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<QuotaDataDto, QuotaEntity>().ReverseMap()
                .ForMember(dest => dest.IdQuota, opt => opt.MapFrom(src => src.IdCuota))
                .ForMember(dest => dest.IdCredit, opt => opt.MapFrom(src => src.IdCredito))
                .ForMember(dest => dest.CapitalValue, opt => opt.MapFrom(src => src.ValorCapital))
                .ForMember(dest => dest.TotalValue, opt => opt.MapFrom(src => src.ValorTotal))
                .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.FechaPago))
                .ForAllMembers(opts => opts.PreCondition((src, dest, srcMember)
                  => srcMember != null && !string.IsNullOrWhiteSpace(srcMember?.ToString())));
            CreateMap<QuotaEntity, QuotaDataDto>().ReverseMap()
                .ForMember(dest => dest.IdCuota, opt => opt.MapFrom(src => src.IdQuota))
                .ForMember(dest => dest.IdCredito, opt => opt.MapFrom(src => src.IdCredit))
                .ForMember(dest => dest.ValorCapital, opt => opt.MapFrom(src => src.CapitalValue))
                .ForMember(dest => dest.ValorTotal, opt => opt.MapFrom(src => src.TotalValue))
                .ForMember(dest => dest.FechaPago, opt => opt.MapFrom(src => src.PaymentDate))
                .ForAllMembers(opts => opts.PreCondition((src, dest, srcMember)
                  => srcMember != null && !string.IsNullOrWhiteSpace(srcMember?.ToString())));
        }
    }
}
