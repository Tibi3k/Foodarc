using Foodarc.DAL;
using Foodarc.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Foodarc.Controllers;
[Route("api/order")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly ILogger<OrderController> _logger;
    private readonly IOrderServcie orderService;
    private readonly IRestaurantService restaurantService;

    public OrderController(ILogger<OrderController> logger, IOrderServcie cosmosdb, IRestaurantService restaurantService)
    {
        _logger = logger;
        orderService = cosmosdb;
        this.restaurantService= restaurantService;
    }

    [Authorize("User")]
    [HttpGet]
    public async Task<ActionResult<Order>> GetOrdersOfUser(){
        var id = this.getUserIdFromClaim(User);
        var orders =  await this.orderService.GetBasketOfUser(id);
        return Ok(orders);
    }

    [Authorize("Owner")]
    [HttpGet("restaurant")]
    public async Task<ActionResult<Order>> GetOrdersOfRestaurant()
    {
        var id = this.getUserIdFromClaim(User);
        var orders = await this.restaurantService.GetOrderOfRestaurant(id);
        return Ok(orders);
    }

    [Authorize("User")]
    [HttpPost("{restaurantId}")]
    public async Task<ActionResult> AddOrderToRestaurant([FromBody] Basket order, string restaurantId) {
        await this.restaurantService.AddOrderToRestaurant(restaurantId, order);
        return Ok();
    }

    [Authorize("User")]
    [HttpPost]
    public async Task<ActionResult> AddOrderToUser([FromBody] Basket order)
    {
        var id = this.getUserIdFromClaim(User);
        await this.orderService.AddOrderToUser(id, order);
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
