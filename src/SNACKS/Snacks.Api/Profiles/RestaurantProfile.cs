using AutoMapper;
using Dal.Domain;
using Snacks.Api.Dtos;

namespace Snacks.Api.Profiles;

public class RestaurantProfile : Profile
{
    public RestaurantProfile()
    {
        CreateMap<Restaurant, RestaurantDto>();
        CreateMap<RestaurantForCreationDto, Restaurant>();
        CreateMap<RestaurantDistanceResult, RestaurantDistanceResultDto>();
    }
}
