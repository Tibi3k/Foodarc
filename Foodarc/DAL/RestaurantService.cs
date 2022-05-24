using Foodarc.Controllers.DTO;
using Foodarc.Model;
using Microsoft.Azure.Cosmos;

namespace Foodarc.DAL;

public class RestaurantService : IRestaurantService
{
    private Container restaurantContainer;

    public RestaurantService(
        CosmosClient dbClient,
        string databaseName,
        string containerName)
    {
        this.restaurantContainer = dbClient.GetContainer(databaseName, containerName);
    }

    public async Task CreateRestaurant(CreateRestaurant restaurant, string id)
    {
        var newRestaurant = new Restaurant
        {
            Id = id,
            OwnerId = id,
            Address = restaurant.Address,
            ZipCode = restaurant.ZipCode,
            City = restaurant.City,
            Country = restaurant.Country,
            AvailableFoods = new List<Food>(),
            Name = restaurant.Name,
            Description = restaurant.Description,
            ImagePath = restaurant.ImagePath
        };
        await this.restaurantContainer.CreateItemAsync<Restaurant>(newRestaurant, new PartitionKey(id));
    }

    public async Task<List<Restaurant>> GetAllRestaurants() {
        List<Restaurant> allSalesForAccount1 = new List<Restaurant>();
        using (FeedIterator<Restaurant> resultSet = restaurantContainer.GetItemQueryIterator<Restaurant>(
            queryDefinition: null,
            requestOptions: new QueryRequestOptions()))
        {
            while (resultSet.HasMoreResults)
            {
                FeedResponse<Restaurant> response = await resultSet.ReadNextAsync();
                Restaurant food = response.First();
                if (response.Diagnostics != null)
                {
                    Console.WriteLine($" Diagnostics {response.Diagnostics.ToString()}");
                }

                allSalesForAccount1.AddRange(response.Resource);
            }
        }
        return allSalesForAccount1;
    }

    public async Task DeleteRestaurantAsync(string id)
    {
        await this.restaurantContainer.DeleteItemAsync<Restaurant>(id, new PartitionKey(id));
    }

    public async Task<Restaurant?> GetRestaurantById(string id)
    {
        try
        {
            ItemResponse<Restaurant> response = await this.restaurantContainer.ReadItemAsync<Restaurant>(id, new PartitionKey(id));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<Food?> GetFoodById(string restaurantId, string foodId)
    {
        try
        {
            ItemResponse<Restaurant> response = await this.restaurantContainer.ReadItemAsync<Restaurant>(restaurantId, new PartitionKey(restaurantId));
            return response.Resource.AvailableFoods.Where(f => f.Id == foodId).SingleOrDefault();
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task UpdateItemAsync(string id, Restaurant item)
    {
        await this.restaurantContainer.UpsertItemAsync<Restaurant>(item, new PartitionKey(id));
    }

    public async Task UpdateFoodAsync(string id, Food food)
    {
        var restaurant = await this.GetRestaurantById(id);
        var index = restaurant.AvailableFoods.FindIndex(f => f.Id == food.Id);
        restaurant.AvailableFoods[index] = food;
        await this.restaurantContainer.UpsertItemAsync(restaurant, new PartitionKey(restaurant.Id));
    }


    public async Task AddFoodToRestaurant(string id, CreateFood food) {
        var restaurant = await this.GetRestaurantById(id);
        var foodToAdd = new Food
        {
            Id = Guid.NewGuid().ToString(),
            Description = food.Description,
            Calories = food.Calories,
            Name = food.Name,
            Price = food.Price,
            ImagePath = food.ImagePath
        };
        restaurant.AvailableFoods.Add(foodToAdd);
        await this.restaurantContainer.UpsertItemAsync(restaurant, new PartitionKey(restaurant.Id));
    }

    public async Task DeleteFoodFromRestaurant(string id, string foodId)
    {
        var restaurant = await this.GetRestaurantById(id);
        restaurant.AvailableFoods = restaurant.AvailableFoods.Where(f => f.Id != foodId).ToList();
        await this.restaurantContainer.UpsertItemAsync(restaurant, new PartitionKey(restaurant.Id));
    }
}
