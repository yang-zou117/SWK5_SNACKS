using AutoMapper;
using Dal.Domain;
using Snacks.Api.Dtos;

namespace Snacks.Api.Profiles;


public class OpeningHoursProfile : Profile
{
    public OpeningHoursProfile()
    {
        CreateMap<OpeningHoursForCreationDto, OpeningHours>();
        CreateMap<OpeningHours, OpeningHoursDto>();
        
    }
}
