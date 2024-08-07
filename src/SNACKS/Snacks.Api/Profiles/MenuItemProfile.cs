using AutoMapper;
using Dal.Domain;
using Snacks.Api.Dtos;

namespace Snacks.Api.Profiles;

public class MenuItemProfile: Profile
{
    public MenuItemProfile()
    {
        CreateMap<MenuItem, MenuItemDto>();
        CreateMap<MenuItemForCreationDto, MenuItem>();
        CreateMap<MenuItemForUpdateDto, MenuItem>();
    }
}
