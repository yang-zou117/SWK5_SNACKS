using AutoMapper;
using Dal.Domain;
using Snacks.Api.Dtos;

namespace Snacks.Api.Profiles;

public class ClosingDayProfile : Profile
{

    public ClosingDayProfile()
    {
        CreateMap<ClosingDayForCreationDto, ClosingDay>();
        CreateMap<ClosingDay, ClosingDayDto>();
    }
}
