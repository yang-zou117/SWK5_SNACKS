using Dal.Domain;
using Snacks.Logic;

namespace Snacks.Api.Middlewares;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate next;

    public ApiKeyMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context, IApiKeyLogic validator)
    {
        // check if api key is given
        if (!context.Request.Headers.TryGetValue("X-API-KEY", out var apiKeyVal))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API key missing");
            return;
        }

        // read the restaurant_id
        if (!int.TryParse((string)context.Request.RouteValues["restaurantId"]!, out var restaurantId))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("Invalid restaurant ID");
            return;
        }

        // validate the api key
        if (!await validator.ValidateApiKeyAsync(new ApiKey(restaurantId, apiKeyVal)))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Invalid API key or false restaurant ID");
            return;
        }


        await next(context);
        
    }
}