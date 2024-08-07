using AutoMapper;
using Dal.Domain;
using Microsoft.AspNetCore.Mvc;
using Snacks.Api.Dtos;
using Snacks.Logic;

namespace Snacks.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenuItemController : Controller
{
    private readonly IMenuItemLogic logic;
    private readonly IMapper mapper;

    public MenuItemController(IMenuItemLogic logic, IMapper mapper)
    {
        this.logic = logic ?? throw new ArgumentNullException(nameof(logic));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet("{menuItemId}")]
    public async Task<ActionResult<MenuItemDto>> GetMenuItemById(int menuItemId)
    {
        var menuItem = await logic.GetMenuItemByIdAsync(menuItemId);
        if (menuItem == null)
        {
            return NotFound(StatusInfo.InvalidMenuItemId(menuItemId));
        }

        return mapper.Map<MenuItemDto>(menuItem);
    }

    [HttpGet("restaurant/{restaurantId}")]
    public async Task<IEnumerable<MenuItemDto>> GetMenuItemsByRestaurantId(int restaurantId)
    {
        IEnumerable<MenuItem> menuItems =
            await logic.GetMenuItemsByRestaurantIdAsync(restaurantId);
        return mapper.Map<IEnumerable<MenuItemDto>>(menuItems);
    }

    [HttpPost("restaurant/{restaurantId}")]
    public async Task<ActionResult<MenuItemDto>> AddMenuItems(int restaurantId, 
                                                              [FromBody] MenuItemForCreationDto[] menuDto)
    {

        foreach (var menuItem in menuDto)
        {
            // add menu item 
            var item = mapper.Map<MenuItem>(menuItem);
            item.RestaurantId = restaurantId;
            await logic.AddMenuItemAsync(item);
        }

        return CreatedAtAction(nameof(GetMenuItemsByRestaurantId), 
                               new { restaurantId }, menuDto);
    }

    
    [HttpPut("restaurant/{restaurantId}")]
    public async Task<ActionResult> UpdateMenuItems(int restaurantId,
                                                    [FromBody] MenuItemForUpdateDto[] itemsToUpdate)
    {
        foreach (var item in itemsToUpdate)
        {
            // update each item
            var menuItem = await logic.GetMenuItemByIdAsync(item.MenuItemId);
            if(menuItem == null)
                return NotFound(StatusInfo.InvalidMenuItemId(item.MenuItemId));
            if (menuItem.RestaurantId != restaurantId)
                return BadRequest(StatusInfo.InvalidMenuItemRestaurantId(restaurantId));
            mapper.Map(item, menuItem);
            var updated = await logic.UpdateMenuItemAsync(menuItem);
            if (!updated)
                return StatusCode(500);
        }

        return NoContent();
    }
    

    [HttpDelete("{restaurantId}/{menuItemId}")]
    public async Task<ActionResult> DeleteMenuItem(int restaurantId, int menuItemId)
    {
        // check if menu item exists
        var menuItem = await logic.GetMenuItemByIdAsync(menuItemId);
        if (menuItem is null)
            return NotFound(StatusInfo.InvalidMenuItemId(menuItemId));
        if (menuItem.RestaurantId != restaurantId)
            return BadRequest(StatusInfo.InvalidMenuItemRestaurantId(menuItem.RestaurantId));

        // delete menu item
        var deleted = await logic.DeleteMenuItemAsync(menuItemId);
        return deleted ? NoContent() : StatusCode(500);
    }
}
