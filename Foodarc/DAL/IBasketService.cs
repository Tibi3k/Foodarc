using Foodarc.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Foodarc.DAL;

public interface IBasketService 
{
    public Task<Basket?> GetBasketById(string id);
    public Task AddFoodToBasket(BasketFood basketFood, string id);
    public Task DeleteBasket(string id);
    public Task DeleteFoodFromBasket(string id, string foodId);
}
