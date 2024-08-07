using System.Text.Json.Serialization;
using System.Text.Json;
using Snacks.Logic;
using Snacks.Api.HostedServices;
using Snacks.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);
ConfigureServices(builder.Services);
var app = builder.Build();

ConfigureMiddleWare(app);
ConfigureEndPoints(app);

app.Run();

# region Configuration
void ConfigureServices(IServiceCollection services)
{
    services
        .AddControllers(options => options.ReturnHttpNotAcceptable = true)
        .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                
            })
        .AddXmlDataContractSerializerFormatters();

    services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigin",
            builder =>
            {
                builder.WithOrigins("http://localhost:4200")
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });
    });

    services.AddRouting(options => options.LowercaseUrls = true);
    services.AddAutoMapper(typeof(Program));
    services.AddOpenApiDocument(config => config.Title = "Order Management API");

    services.AddCors(builder =>
    builder.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin();
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
    }));

    // add logics as scoped services
    services.AddScoped<IMenuItemLogic, MenuItemLogic>();
    services.AddScoped<IOrderLogic, OrderLogic>();
    services.AddScoped<IDeliveryConditionLogic, DeliveryConditionLogic>();
    services.AddScoped<IRestaurantLogic, RestaurantLogic>();
    services.AddScoped<IApiKeyLogic, ApiKeyLogic>();
    services.AddScoped<SendOrderToWebhookUrlService, SendOrderToWebhookUrlService>();


    services.AddHttpClient();
    services.AddHostedService<SendOrderToWebhookUrlService>();
}

void ConfigureMiddleWare(IApplicationBuilder app)
{
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.UseOpenApi();
    app.UseSwaggerUi3(config => config.Path = "/swagger");
    app.UseCors();
    app.UseWhen(context => ((context.Request.Method != "GET") && // only use api key middleware for delete, put, post (except post new restaurant, and order) and get (except get link)
                            (!context.Request.Path.StartsWithSegments("/api/restaurant/register")) &&
                            (!context.Request.Path.StartsWithSegments("/api/order/placeOrder")) &&
                            (!context.Request.Path.StartsWithSegments("/api/order/calculatePrice"))) || 
                           (context.Request.Path.StartsWithSegments("/api/order/status/link")) || 
                           (context.Request.Path.StartsWithSegments("/api/order/restaurant")), builder =>
    {
        builder.UseMiddleware<ApiKeyMiddleware>();
    });
}

void ConfigureEndPoints(IEndpointRouteBuilder app){
    app.MapControllers(); 
}
#endregion