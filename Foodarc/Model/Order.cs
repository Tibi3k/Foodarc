using Newtonsoft.Json;

namespace Foodarc.Model;

public class Order
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    [JsonProperty(PropertyName = "restaurantId")]
    public string RestaurantId { get; set; }


    [JsonProperty(PropertyName = "userId")]
    public string UserId { get; set; }

    [JsonProperty(PropertyName = "orderDate")]
    public DateTime OrderDate { get; set; }

    [JsonProperty(PropertyName = "totalPrice")]
    public int TotalPrice { get; set; }

    [JsonProperty(PropertyName = "calories")]
    public int Calories { get; set; }

    [JsonProperty(PropertyName = "foods")]
    public List<Food> Foods { get; set; }
}
