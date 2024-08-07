using AutoMapper;
using Dal.Domain;
using Snacks.Api.Dtos;

namespace Snacks.Api.Profiles;

public class AddressProfile : Profile
{
    public AddressProfile()
    {
        CreateMap<AddressForCreationDto, Address>();
        CreateMap<Address, AddressDto>();
    }
}
