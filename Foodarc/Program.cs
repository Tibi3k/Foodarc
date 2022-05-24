using Foodarc.DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.Cosmos;
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

static async Task<RestaurantService> InitializeRestaurantServiceContainer(IConfigurationRoot configurationRoot)
{
    var configuration = configurationRoot.GetSection("CosmosDb");
    string databaseName = configuration.GetSection("DatabaseName").Value;
    string account = configuration.GetSection("Account").Value;
    string key = configuration.GetSection("Key").Value;
    CosmosClient client = new CosmosClient(account, key);
    var containerName = "Restaurants";
    RestaurantService cosmosDbService = new RestaurantService(client, databaseName, containerName);
    DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
    await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");
    return cosmosDbService;
}

static async Task<OrderService> InitializeOrderServiceContainer(IConfigurationRoot configurationRoot)
{
    var configuration = configurationRoot.GetSection("CosmosDb");
    string databaseName = configuration.GetSection("DatabaseName").Value;
    string account = configuration.GetSection("Account").Value;
    string key = configuration.GetSection("Key").Value;
    CosmosClient client = new CosmosClient(account, key);
    var containerName = "Orders";
    OrderService cosmosDbService = new OrderService(client, databaseName, containerName);
    DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
    await database.Database.CreateContainerIfNotExistsAsync(containerName, "/userId");
    return cosmosDbService;
}

static async Task<BasketService> InitializeBasketServiceContainer(IConfigurationRoot configurationRoot)
{
    var configuration = configurationRoot.GetSection("CosmosDb");
    string databaseName = configuration.GetSection("DatabaseName").Value;
    string account = configuration.GetSection("Account").Value;
    string key = configuration.GetSection("Key").Value;
    CosmosClient client = new CosmosClient(account, key);
    var containerName = "Baskets";
    var cosmosDbService = new BasketService(client, databaseName, containerName);
    DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
    await database.Database.CreateContainerIfNotExistsAsync(containerName, "/userId");
    return cosmosDbService;
}


builder.Services.AddSingleton<IRestaurantService>(InitializeRestaurantServiceContainer(configuration).GetAwaiter().GetResult());
builder.Services.AddSingleton<IOrderServcie>(InitializeOrderServiceContainer(configuration).GetAwaiter().GetResult());
builder.Services.AddSingleton<IBasketService>(InitializeBasketServiceContainer(configuration).GetAwaiter().GetResult());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
