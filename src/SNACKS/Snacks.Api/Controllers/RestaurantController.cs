using System.Collections;
using System.Globalization;
using System.Transactions;
using AutoMapper;
using Dal.Domain;
using Microsoft.AspNetCore.Mvc;
using Snacks.Api.Dtos;
using Snacks.Logic;

namespace Snacks.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RestaurantController : Controller
{
    private readonly IRestaurantLogic logic;
    private readonly IMapper mapper;
    private readonly IApiKeyLogic apiKeyLogic; 

    public RestaurantController(IRestaurantLogic logic, IApiKeyLogic apiKeyLogic, IMapper mapper)
    {
        this.logic = logic ?? throw new ArgumentNullException(nameof(logic));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this.apiKeyLogic = apiKeyLogic ?? throw new ArgumentNullException(nameof(apiKeyLogic));
    }

    
    [HttpPost("register")]
    public async Task<IActionResult> RegisterRestaurant([FromBody] RegisterDto registerDto)
    {

        // store the the uploaded image
        RestaurantForCreationDto restaurantDto = registerDto.Restaurant; 
        string imagePath = "";
        if (restaurantDto.Image is not null) 
        {
            string fileName = restaurantDto.RestaurantName + 
                              DateTime.Now.ToString("yyyyMMdd_HHmmss_fff") + "." +
                              restaurantDto.Image.FileType;
            string filePath = "./Images/" + fileName;
            try
            {
                // Write the image file
                using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                fs.Write(restaurantDto.Image.ImageData, 0, restaurantDto.Image.ImageData.Length);
                imagePath = fileName;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error writing the image file for new restaurant: " + ex.Message);
            }
        }

        try // start transaction to insert restaurant
        {
			using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled)) {

				// add address
				Address address = mapper.Map<Address>(registerDto.Address);
				address.AddressId = await logic.AddAddressAsync(address);

				// add restaurant
				Restaurant restaurant = new Restaurant(
                    0, 
                    restaurantDto.RestaurantName,
                    restaurantDto.WebhookUrl, 
                    imagePath, 
                    address.AddressId
                ); 
                restaurant.RestaurantId = await logic.AddRestaurantAsync(restaurant);

                // map opening hours
                List<OpeningHours> openingHoursList = registerDto.OpeningHours
                    .Select(mapper.Map<OpeningHours>)
                    .Select(openingHours => { openingHours.RestaurantId = restaurant.RestaurantId; 
                        return openingHours; })
                    .ToList();

                foreach (var openingHours in openingHoursList) 
                        await logic.AddOpeningHoursAsync(openingHours);
                
                // map closing days
                List<ClosingDay> closingDayList = registerDto.ClosingDays
                    .Select(mapper.Map<ClosingDay>)
                    .Select(closingDay => { closingDay.RestaurantId = restaurant.RestaurantId; 
                        return closingDay; })
                    .ToList();

                foreach (var closingDay in closingDayList) await logic.AddClosingDayAsync(closingDay);
        
                // generate API key
                var apiKeyValue = ApiKeyGenerator.GenerateApiKeyValue();
                ApiKey apiKey = new ApiKey(restaurant.RestaurantId, apiKeyValue);

                await apiKeyLogic.InsertApiKeyAsync(apiKey);

                scope.Complete();
                return Ok(new { ApiKeyValue = apiKeyValue, restaurant.RestaurantId });
            }
        } 
        catch(Exception ex)
        {
			return StatusCode(500, "Internal server error");
		}

    }

    [HttpGet("{restaurantId}")]
    public async Task<ActionResult<RestaurantDetailsDto>> GetRestaurantDetails(int restaurantId)
    {
        // get restaurant object first
        var restaurant = await logic.GetRestaurantAsync(restaurantId);
        if (restaurant == null) return NotFound(StatusInfo.RestaurantNotFound(restaurantId));

        // get address
        var address = await logic.GetAddressForRestaurantAsync(restaurantId);
        if (address == null) return NotFound();

        // get opening hours and closing days
        IEnumerable<OpeningHours> openingHours = 
            await logic.GetOpeningHoursForRestaurantAsync(restaurantId);
        IEnumerable<ClosingDay> closingDays = 
            await logic.GetClosingDaysForRestaurantAsync(restaurantId);

        // map to DTO
        var restaurantDetails = new RestaurantDetailsDto
        {
            Restaurant = mapper.Map<RestaurantDto>(restaurant),
            Address = mapper.Map<AddressDto>(address),
            OpeningHours = mapper.Map<IEnumerable<OpeningHoursDto>>(openingHours),
            ClosingDays = mapper.Map<IEnumerable<ClosingDayDto>>(closingDays)
        };

        return restaurantDetails;
    }

    [HttpPost("openingHours/{restaurantId}")]
    public async Task<IActionResult> AddOpeningHours(int restaurantId,
                                                     [FromBody] OpeningHoursForCreationDto dto)
    {
        // check if restaurant exists
        var restaurant = await logic.GetRestaurantAsync(restaurantId);
        if (restaurant == null) return NotFound(StatusInfo.RestaurantNotFound(restaurantId));

        // map to domain object
        var openingHours = mapper.Map<OpeningHours>(dto);
        openingHours.RestaurantId = restaurantId;

        // add opening hours
        await logic.AddOpeningHoursAsync(openingHours);

        return Ok();
    }

    [HttpPost("closingDay/{restaurantId}")]
    public async Task<IActionResult> AddClosingDay(int restaurantId,
                                                   [FromBody] ClosingDayForCreationDto closingDayDto)
    {
        // check if restaurant exists
        var restaurant = await logic.GetRestaurantAsync(restaurantId);
        if (restaurant == null) return NotFound(StatusInfo.RestaurantNotFound(restaurantId));

        // map to domain object
        var closingDay = mapper.Map<ClosingDay>(closingDayDto);
        closingDay.RestaurantId = restaurantId;

        // add closing day
        await logic.AddClosingDayAsync(closingDay);

        return Ok();
    }

    [HttpDelete("openingHours/{restaurantId}/{openingHoursId}")]
    public async Task<IActionResult> DeleteOpeningHours(int openingHoursId, int restaurantId)
    {
        var openingHours = await logic.GetOpeningHoursForRestaurantAsync(restaurantId);

        // check if openingHours contains openingHoursId
        if (openingHours.All(oh => oh.OpeningHoursId != openingHoursId))
            return NotFound(StatusInfo.OpeningHoursNotFound(openingHoursId, restaurantId));

        // delete opening hours
        var deleted = await logic.DeleteOpeningHoursAsync(openingHoursId);
        return deleted ? NoContent() : StatusCode(500);
    }

    [HttpDelete("closingDay/{restaurantId}/{weekDay}")]
    public async Task<IActionResult> DeleteClosingDay(string weekDay, int restaurantId)
    {
        var closingDays = await logic.GetClosingDaysForRestaurantAsync(restaurantId);

        // check if closingDays contains weekDay
        if (closingDays.All(cd => cd.WeekDay != weekDay))
            return NotFound(StatusInfo.ClosingDayNotFound(weekDay, restaurantId));

        // delete closing day
        var deleted = await logic.DeleteClosingDayAsync(weekDay, restaurantId);
        return deleted ? NoContent() : StatusCode(500);
    }
}
