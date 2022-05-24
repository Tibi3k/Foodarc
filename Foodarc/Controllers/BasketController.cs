using Foodarc.DAL;
using Foodarc.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Foodarc.Controllers;
[Route("api/basket")]
[ApiController]
public class BasketController : ControllerBase
{
    private readonly ILogger<BasketController> _logger;
    private readonly IBasketService basketService;

    public BasketController(ILogger<BasketController> logger, IBasketService cosmosdb)
    {
        _logger = logger;
        basketService = cosmosdb;
    }

    [Authorize("User")]
    [HttpGet]
    public async Task<ActionResult<Basket?>> GetUserBasket() {
        var id = getUserIdFromClaim(User);
        var basket = await this.basketService.GetBasketById(id);
        return Ok(basket);
    }

    [Authorize("User")]
    [HttpDelete]
    public async Task<ActionResult> DeleteBasket()
    {
        var id = getUserIdFromClaim(User);
        await this.basketService.DeleteBasket(id);
        return NoContent();
    }

    [Authorize("User")]
    [HttpPost]
    public async Task<ActionResult> AddFoodToBasket([FromBody] BasketFood basketFood)
    {
        var id = getUserIdFromClaim(User);
        await this.basketService.AddFoodToBasket(basketFood, id);
        return Ok();
    }

    [Authorize("User")]
    [HttpDelete("{foodId}")]
    public async Task<ActionResult> AddFoodToBasket(string foodId)
    {
        var id = getUserIdFromClaim(User);
        await this.basketService.DeleteFoodFromBasket(id, foodId);
        return NoContent();
    }

    private string getUserIdFromClaim(ClaimsPrincipal principal)
    {
        return principal.Claims
            .FirstOrDefault(claim =>
             claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")!
            .Value;
    }
}
