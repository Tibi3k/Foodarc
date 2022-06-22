using Foodarc.DAL;
using Foodarc.Model;
using HotChocolate.Resolvers;

namespace Foodarc.GraphQL;

public class FoodResolver
{

    public readonly IRestaurantService restaurantService;

    public FoodResolver(IRestaurantService restaurantService)
    {
        this.restaurantService = restaurantService;
    }

    public IEnumerable<Food> GetFoodsOfRestaurant(Restaurant restaurant, IResolverContext context) {
        return restaurant.AvailableFoods;
    }
    
}

