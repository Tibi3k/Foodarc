using Foodarc.Controllers.DTO;
using Foodarc.DAL;
using Foodarc.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Foodarc.Controllers;

[ApiController]
[Route("/api/restaurant")]
public class RestaurantController : ControllerBase
{
    private readonly ILogger<RestaurantController> _logger;
    private readonly IRestaurantService restaurantService;

    public RestaurantController(ILogger<RestaurantController> logger, IRestaurantService cosmosdb)
    {
        _logger = logger;
        restaurantService = cosmosdb;
    }

    [Authorize("Authenticated")]
    [HttpGet(Name = "GetAllFoods")]
    public async Task<ActionResult<IEnumerable<Restaurant>>> GetAll()
    {
        var result = await this.restaurantService.GetAllRestaurants();
        return Ok(result);
    }

    [Authorize("Authenticated")]
    [HttpGet("single", Name = "GetRestaurantById")]
    public async Task<ActionResult<IEnumerable<Restaurant>>> GetRestaurantById()
    {
        var id = getUserIdFromClaim(User);
        var result = await this.restaurantService.GetRestaurantById(id);
        return Ok(result);
    }

    [Authorize("Authenticated")]
    [HttpGet("{restaurantId}/{foodId}", Name = "GetFoodById")]
    public async Task<ActionResult<Food?>> GetFoodById(string restaurantId, string foodId)
    {
        var result = await this.restaurantService.GetFoodById(restaurantId, foodId);
        return Ok(result);
    }

    [Authorize("Owner")]
    [HttpPost(Name = "CreateRestaurant")]
    public async Task<ActionResult> Create([FromBody] CreateRestaurant restaurant)
    {
        var id = getUserIdFromClaim(User);
        await this.restaurantService.CreateRestaurant(restaurant, id);
        return Ok();
    }

    [Authorize("Owner")]
    [HttpDelete(Name = "DeleteRestaurant")]
    public async Task<ActionResult> Delete()
    {
        var id = getUserIdFromClaim(User);
        await this.restaurantService.DeleteRestaurantAsync(id);
        return Ok();
    }

    [Authorize("Owner")]
    [HttpPost("food")]
    public async Task<ActionResult> AddFood([FromBody] CreateFood food) {
        var id = getUserIdFromClaim(User);
        await this.restaurantService.AddFoodToRestaurant(id, food);
        return Ok();
    }

    [Authorize("Owner")]
    [HttpPut]
    public async Task<ActionResult> UpdateRestaurant([FromBody] Restaurant restaurant) {
        var id = getUserIdFromClaim(User);
        await this.restaurantService.UpdateItemAsync(id, restaurant);
        return Ok();
    }
    
    private string getUserIdFromClaim(ClaimsPrincipal principal)
    {
        return principal.Claims
            .FirstOrDefault(claim =>
             claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")!
            .Value;
    }
}
