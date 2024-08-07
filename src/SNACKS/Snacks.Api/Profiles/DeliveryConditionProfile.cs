using AutoMapper;
using Dal.Domain;
using Snacks.Api.Dtos;

namespace Snacks.Api.Profiles;

public class DeliveryConditionProfile : Profile
{
    public DeliveryConditionProfile()
    {
        CreateMap<DeliveryCondition, DeliveryConditionDto>();
        CreateMap<DeliveryConditionForCreationDto, DeliveryCondition>();
        CreateMap<DeliveryConditionForUpdateDto, DeliveryCondition>();
    }
}
