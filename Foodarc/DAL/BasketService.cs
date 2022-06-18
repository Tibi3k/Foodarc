using Foodarc.Controllers.DTO;
using Foodarc.DAL.EfDbContext;
using Foodarc.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Foodarc.DAL;
public class BasketService : IBasketService
{
    private CosmosDbContext context;
    private readonly IMapper mapper;

    public BasketService(CosmosDbContext db, IMapper mapper)
    {
        this.mapper = mapper; 
        context = db;
    }

    public async Task<Basket?> GetBasketById(string id)
    {
        var basket = await context.Baskets.WithPartitionKey(id).SingleOrDefaultAsync();
        if (basket == null)
            return null;
        return mapper.Map<Basket?>(basket);
    }


    public async Task AddFoodToBasket(CreateBasketFood basketFood, string id) {
        var bFood = new BasketFood
        {
            Id = Guid.NewGuid().ToString(),
            AddTime = DateTime.Now,
            OrderedFood = basketFood.OrderedFood,
            RestaurantUrl = basketFood.RestaurantUrl
        };
        var basket = await context.Baskets.WithPartitionKey(id).SingleOrDefaultAsync();
        if (basket == null) {
            basket = new DbBasket
            {
                Id = id,
                UserId = id,
                Foods = new List<DbBasketFood>(),
                LastEdited = bFood.AddTime,
                TotalCost = 0
            };
        }
        basket.LastEdited = bFood.AddTime;
        basket.TotalCost += bFood.OrderedFood.Price;
        basket.Foods.Add(mapper.Map<DbBasketFood>(bFood));
        await context.SaveChangesAsync();
    }

    public async Task DeleteBasket(string id) {
        var basket = await context.Baskets.WithPartitionKey(id).SingleAsync();
        this.context.Baskets.Remove(basket);
        await this.context.SaveChangesAsync();
    }

    public async Task DeleteFoodFromBasket(string id, string foodId)
    {
        var basket = await context.Baskets.WithPartitionKey(id).SingleAsync();
        var food = basket.Foods.Where(f => f.Id == foodId).SingleOrDefault();
        basket.Foods = basket.Foods.Where(f => f.Id != foodId).ToList();
        basket.LastEdited = DateTime.Now;
        basket.TotalCost -= food.OrderedFood.Price;
        await context.SaveChangesAsync();
    }
}
