using AutoMapper;
using Foodarc.Controllers.DTO;
using Foodarc.DAL.EfDbContext;
using Foodarc.Model;
using Microsoft.Azure.Cosmos;

namespace Foodarc.DAL;

public class RestaurantService : IRestaurantService
{

    private CosmosDbContext context;
    private readonly IMapper mapper;

    public RestaurantService(CosmosDbContext db, IMapper mapper)
    {
        this.mapper = mapper;
        context = db;
    }

    public async Task CreateRestaurant(CreateRestaurant restaurant, string id)
    {
        Console.WriteLine("Creating restaurant");
        Console.WriteLine(restaurant);
        Console.WriteLine(restaurant.Address);
        var newRestaurant = new DbRestaurant
        {
            Id = id,
            OwnerId = id,
            Address = restaurant.Address,
            ZipCode = restaurant.ZipCode,
            City = restaurant.City,
            Country = restaurant.Country,
            AvailableFoods = new List<DbFood>(),
            Name = restaurant.Name,
            Description = restaurant.Description,
            ImagePath = restaurant.ImagePath,
            Orders = new List<DbRestaurantOrder>{
                new DbRestaurantOrder {
                    Id = Guid.NewGuid().ToString(),
                    UserId = id,
                    OrderDate = DateTime.Now,
                    Orders = new List<DbOrderBasket>()
                }
            }
        };
        Console.WriteLine("Creating new restaurant");
        Console.WriteLine(newRestaurant.Address);
        await context.Restaurant.AddAsync(newRestaurant);
        Console.WriteLine(newRestaurant.Address);
        await context.SaveChangesAsync();
    }

    public async Task<List<Restaurant>> GetAllRestaurants() {
        return context.Restaurant.Where(_ => true).Select(x => mapper.Map<Restaurant>(x)).ToList();  
    }

    public async Task DeleteRestaurantAsync(string id)
    {
        var restaurant = await context.Restaurant.FindAsync(id);
        context.Restaurant.Remove(restaurant);
        await context.SaveChangesAsync();
    }

    public async Task<Restaurant?> GetRestaurantById(string id)
    {
        var restaurant = await context.Restaurant.FindAsync(id);
        if (restaurant == null)
            return null;
        return mapper.Map<Restaurant>(restaurant);
    }

    public async Task<Food?> GetFoodById(string restaurantId, string foodId)
    {
        var restaurant = await context.Restaurant.FindAsync(restaurantId);
        if (restaurant == null)
            return null;
        var food = restaurant.AvailableFoods.FirstOrDefault(x => x.Id == foodId);
        if (food == null)
            return null;
        return mapper.Map<Food>(food);
    }

    public async Task UpdateItemAsync(string id, Restaurant item)
    {
        var restaurant = await context.Restaurant.FindAsync(id);
        if (restaurant == null)
            return;
        context.Restaurant.Update(mapper.Map<DbRestaurant>(item));
        await context.SaveChangesAsync();
    }

    public async Task UpdateFoodAsync(string id, Food food)
    {
        var restaurant = await context.Restaurant.FindAsync(id);
        var index = restaurant.AvailableFoods.FindIndex(f => f.Id == food.Id);
        restaurant.AvailableFoods[index] = mapper.Map<DbFood>(food);
        await context.SaveChangesAsync();
    }


    public async Task AddFoodToRestaurant(string id, CreateFood food) {
        var restaurant = await context.Restaurant.FindAsync(id);
        if (restaurant == null)
            return;
        var foodToAdd = new DbFood
        {
            Id = Guid.NewGuid().ToString(),
            Description = food.Description,
            Calories = food.Calories,
            Name = food.Name,
            Price = food.Price,
            ImagePath = food.ImagePath
        };
        if(restaurant.AvailableFoods == null)
            restaurant.AvailableFoods = new List<DbFood>();
        restaurant.AvailableFoods.Add(foodToAdd);
        await context.SaveChangesAsync();
    }

    public async Task DeleteFoodFromRestaurant(string id, string foodId)
    {
        var restaurant = await context.Restaurant.FindAsync(id);
        restaurant.AvailableFoods = restaurant.AvailableFoods.Where(f => f.Id != foodId).ToList();
        await context.SaveChangesAsync();
    }

    public async Task AddOrderToRestaurant(string id, Basket order) {
        var restaurant = await context.Restaurant.FindAsync(id);
        if (restaurant == null)
            return;
        if (restaurant.Orders == null) {
            restaurant.Orders = new List<DbRestaurantOrder>{ new DbRestaurantOrder
            {
                Id = Guid.NewGuid().ToString(),
                UserId = id,
                OrderDate = DateTime.Now,
                Orders = new List<DbOrderBasket>()
            }
        };
        }
        var dbBasket = mapper.Map<DbBasket>(order);
        dbBasket.Id = Guid.NewGuid().ToString();    
        restaurant.Orders[0].Orders.Add(mapper.Map<DbOrderBasket>(dbBasket));
        await context.SaveChangesAsync();
    }

    public async Task<Order> GetOrderOfRestaurant(string id) {
        var restaurant = await this.GetRestaurantById(id);
        return restaurant.Orders[0];
    }
}
