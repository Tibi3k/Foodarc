using AutoMapper;
using Foodarc.DAL.EfDbContext;
using Foodarc.Model;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;

namespace Foodarc.DAL;

public class OrderService : IOrderServcie
{

    private CosmosDbContext context;
    private readonly IMapper mapper;

    public OrderService(CosmosDbContext db, IMapper mapper)
    {
        this.mapper = mapper;
        context = db;
    }

    public async Task<Order?> GetBasketOfUser(string userId) {

        var order = await context.Orders.WithPartitionKey(userId).SingleOrDefaultAsync();
        if (order == null)
            return null;
        return mapper.Map<Order?>(order);
    }

    public async Task AddOrderToUser(string userId, Basket order) {

        var dbOrder = await context.Orders.WithPartitionKey(userId).SingleOrDefaultAsync();
        if (dbOrder == null) {
            dbOrder = new DbOrder
            {
                Id = Guid.NewGuid().ToString(),
                OrderDate = DateTime.Now,
                UserId = userId,
                Orders = new List<DbOrderBasket>()
            };
            await context.Orders.AddAsync(dbOrder);
        }
        var dbBasket = mapper.Map<DbBasket>(order);
        dbBasket.Id = Guid.NewGuid().ToString();
        dbOrder.Orders.Add(mapper.Map<DbOrderBasket>(dbBasket));
        await context.SaveChangesAsync();
    }
}
