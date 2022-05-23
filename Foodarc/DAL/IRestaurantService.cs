using Foodarc.Controllers.DTO;
using Foodarc.Model;

namespace Foodarc.DAL;

public interface IRestaurantService
{
    public Task<List<Restaurant>> GetAllRestaurants();
    public Task CreateRestaurant(CreateRestaurant restaurant, string id);
    public Task AddFoodToRestaurant(string id, CreateFood food);
    public Task<Restaurant> GetRestaurantById(string id);
    public Task DeleteRestaurantAsync(string id);
    public Task UpdateItemAsync(string id, Restaurant item);
    public Task<Food?> GetFoodById(string restaurantId, string foodId);

}
