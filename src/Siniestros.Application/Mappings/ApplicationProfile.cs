using AutoMapper;
using Siniestros.Contracts.DTOs;
using Siniestros.Domain.Aggregates;

namespace Siniestros.Application.Mappings
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<Siniestro, SiniestroDto>()
             .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.Tipo.ToString()));
        }
    }
}
