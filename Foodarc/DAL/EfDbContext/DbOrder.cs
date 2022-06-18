using Newtonsoft.Json;

namespace Foodarc.DAL.EfDbContext;

public class DbOrder
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    [JsonProperty(PropertyName = "userId")]
    public string UserId { get; set; }

    [JsonProperty(PropertyName = "orderDate")]
    public DateTime OrderDate { get; set; }

    [JsonProperty(PropertyName = "orders")]
    public List<DbOrderBasket> Orders { get; set; }
}
