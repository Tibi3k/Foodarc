using Foodarc.DAL;
using Foodarc.Model;
using HotChocolate.AspNetCore.Authorization;
using System.Security.Claims;

namespace Foodarc.GraphQL;

public class Query
{
    public Food GetFood() =>
        new Food
        {
            Id = "asdf sad fasd f sad",
            Calories = 1111,
            Description = "asdfasdf",
            ImagePath = "asdfsadf",
            Name = "Food",
            Price = 123

        };

    [Authorize]
    [GraphQLName("GetAllRestaurants")]
    public async Task<List<Restaurant>> GetAllRestaurant([Service] IRestaurantService restaurantService) {
        return await restaurantService.GetAllRestaurants();
    }

    [Authorize]
    [GraphQLName("GetDetailsOfRestaurant")]
    public async Task<Restaurant?> GetDetailsOfRestaurant([Service] IRestaurantService restaurantService, string restaurantId) {
        return await restaurantService.GetRestaurantById(restaurantId);
    }


}
