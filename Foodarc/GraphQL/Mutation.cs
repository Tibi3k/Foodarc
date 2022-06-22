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

    private string? getUserIdFromClaim(ClaimsPrincipal principal)
    {
        var claim = principal.Claims
            .FirstOrDefault(claim =>
             claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier");
        return claim?.Value;
    }
}
