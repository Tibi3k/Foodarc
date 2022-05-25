using Newtonsoft.Json;

namespace Foodarc.Model;

public class Order
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    [JsonProperty(PropertyName = "userId")]
    public string UserId { get; set; }

    [JsonProperty(PropertyName = "orderDate")]
    public DateTime OrderDate { get; set; }

    [JsonProperty(PropertyName = "orders")]
    public List<Basket> Orders { get; set; }
}
