using Foodarc.Config;
using Foodarc.DAL;
using Foodarc.DAL.EfDbContext;
using Foodarc.GraphQL;
using Foodarc.Model;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Playground;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = builder.Configuration;
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAdB2C"));

builder.Services.AddDbContext<CosmosDbContext>(options =>
{
    var cosmosDB = configuration.GetSection("CosmosDb");
    options.UseCosmos(
        cosmosDB.GetValue<string>("Account"),
        cosmosDB.GetValue<string>("Key"),
        cosmosDB.GetValue<string>("DatabaseName")
    );
});

builder.Services.AddGraphQLServer()
    .AddAuthorization()
    .AddType<FoodType>()
    .AddType<RestaurantType>()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddResolver<FoodResolver>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
        });
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("User", policy => {
        policy.RequireClaim("http://schemas.microsoft.com/identity/claims/objectidentifier");
        policy.RequireClaim("extension_Position", "Customer");
    });
    options.AddPolicy("Owner", policy => {
        policy.RequireClaim("http://schemas.microsoft.com/identity/claims/objectidentifier");
        policy.RequireClaim("extension_Position", "owner");
    });
    options.AddPolicy("Authenticated", policy => {
        policy.RequireClaim("http://schemas.microsoft.com/identity/claims/objectidentifier");
    });
});

builder.Services.AddTransient<IRestaurantService, RestaurantService>();
builder.Services.AddTransient<IOrderServcie, OrderService>();
builder.Services.AddTransient<IBasketService, BasketService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UsePlayground(new PlaygroundOptions
    //{
    //    QueryPath = "/graphql",
    //    Path = "/playground"
    //});
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL();
app.MapControllers();

app.Run();
