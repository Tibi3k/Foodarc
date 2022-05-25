using Foodarc.Model;

namespace Foodarc.DAL;

public interface IOrderServcie
{
    public Task<Order?> GetBasketOfUser(string userId);
    public Task AddOrderToUser(string userId, Basket order);
}
