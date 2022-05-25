using Foodarc.Controllers.DTO;
using Foodarc.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;

namespace Foodarc.DAL;
public class BasketService : IBasketService
{
    private Container basketContainer;

    public BasketService(
        CosmosClient dbClient,
        string databaseName,
        string containerName)
    {
        this.basketContainer = dbClient.GetContainer(databaseName, containerName);
    }

    public async Task<Basket?> GetBasketById(string id)
    {
        try
        {
            ItemResponse<Basket> response = await this.basketContainer.ReadItemAsync<Basket>(id, new PartitionKey(id));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }


    public async Task AddFoodToBasket(CreateBasketFood basketFood, string id) {
        var bFood = new BasketFood
        {
            Id = Guid.NewGuid().ToString(),
            AddTime = DateTime.Now,
            OrderedFood = basketFood.OrderedFood,
            RestaurantUrl = basketFood.RestaurantUrl
        };
        var basket = await this.GetBasketById(id);
        if (basket == null) {
            basket = new Basket
            {
                Id = id,
                UserId = id,
                Foods = new List<BasketFood>(),
                LastEdited = bFood.AddTime,
                TotalCost = 0
            };
        }
        basket.Foods.Add(bFood);
        basket.LastEdited = bFood.AddTime;
        basket.TotalCost += bFood.OrderedFood.Price;
        await this.basketContainer.UpsertItemAsync<Basket>(basket, new PartitionKey(basket.UserId));
    }

    public async Task DeleteBasket(string id) {
        await this.basketContainer.DeleteItemAsync<Basket>(id, new PartitionKey(id));
    }

    public async Task DeleteFoodFromBasket(string id, string foodId)
    {
        var basket = await this.GetBasketById(id);
        var food = basket.Foods.Where(f => f.Id == foodId).SingleOrDefault();
        basket.Foods = basket.Foods.Where(f => f.Id != foodId).ToList();
        basket.LastEdited = DateTime.Now;
        basket.TotalCost -= food.OrderedFood.Price;
        await this.basketContainer.UpsertItemAsync(basket, new PartitionKey(basket.UserId));
    }
}
