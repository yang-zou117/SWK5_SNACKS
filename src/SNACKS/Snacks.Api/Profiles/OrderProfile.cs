using AutoMapper;
using Dal.Domain;
using Snacks.Api.Dtos;
using static Dal.Domain.Order;

namespace Snacks.Api.Profiles;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<OrderForCreationDto, Order>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => OrderStatus.Pending));
    }
}

