using Foodarc.Model;
using Microsoft.Azure.Cosmos;

namespace Foodarc.DAL;

public class OrderService : IOrderServcie
{

    private Container ordersContainer;

    public OrderService(
        CosmosClient dbClient,
        string databaseName,
        string containerName)
    {
        this.ordersContainer = dbClient.GetContainer(databaseName, containerName);
    }

    public async Task<Order?> GetBasketOfUser(string userId) {
        try
        {
            ItemResponse<Order> response = await this.ordersContainer.ReadItemAsync<Order>(userId, new PartitionKey(userId));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task AddOrderToUser(string userId, Basket order) {
        try
        {
            ItemResponse<Order> response = await this.ordersContainer.ReadItemAsync<Order>(userId, new PartitionKey(userId));
            response.Resource.Orders.Add(order);
            await this.ordersContainer.UpsertItemAsync(response.Resource.UserId, new PartitionKey(response.Resource.UserId));
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            var newOrder = new Order {
                Id = userId,
                OrderDate = DateTime.Now,
                UserId = userId,
                Orders = new List<Basket>()
            };
            newOrder.Orders.Add(order);
            await this.ordersContainer.UpsertItemAsync(newOrder, new PartitionKey(newOrder.UserId));
        }
    }
}
