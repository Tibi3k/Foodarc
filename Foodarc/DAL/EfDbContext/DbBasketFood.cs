using Newtonsoft.Json;

namespace Foodarc.DAL.EfDbContext;

public class DbBasketFood
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    [JsonProperty(PropertyName = "addTime")]
    public DateTime AddTime { get; set; }

    [JsonProperty(PropertyName = "orderedFood")]
    public DbFood OrderedFood { get; set; }

    [JsonProperty(PropertyName = "restaurantUrl")]
    public string RestaurantUrl { get; set; }
}
