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

}
