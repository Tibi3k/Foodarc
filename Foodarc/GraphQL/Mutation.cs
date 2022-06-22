using Foodarc.Controllers.DTO;
using Foodarc.DAL;
using Foodarc.Model;
using HotChocolate.AspNetCore.Authorization;
using System.Security.Claims;

namespace Foodarc.GraphQL;

public class Mutation
{

    [Authorize(Policy = "Owner")]
    [GraphQLName("UpdateRestaurant")]
    public async Task<Restaurant> EditRestaurant(ClaimsPrincipal claimsPrincipal, Restaurant editRestaurant, [Service] IRestaurantService restaurantService) { 
        var id = getUserIdFromClaim(claimsPrincipal);
        return await restaurantService.UpdateItemAsync(id, editRestaurant);
    }

    [Authorize(Policy = "Owner")]
    [GraphQLName("CreateRestaurant")]
    public async Task<Restaurant> CreateRestaurant(ClaimsPrincipal claimsPrincipal, CreateRestaurant createRestaurant, [Service] IRestaurantService restaurantService) {
        var id = getUserIdFromClaim(claimsPrincipal);
        return await restaurantService.CreateRestaurant(createRestaurant, id);
    }

    [Authorize(Policy = "Owner")]
    [GraphQLName("DeleteRestaurant")]
    public async Task<Restaurant?> DeleteRestaurant(ClaimsPrincipal claimsPrincipal, [Service] IRestaurantService restaurantService) {
        var id = getUserIdFromClaim(claimsPrincipal);
        return await restaurantService.DeleteRestaurantAsync(id);
    }
    
    private string? getUserIdFromClaim(ClaimsPrincipal principal)
    {
        var claim = principal.Claims
            .FirstOrDefault(claim =>
             claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier");
        return claim?.Value;
    }
}
