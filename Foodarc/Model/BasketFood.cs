using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Foodarc.Model;

public class BasketFood
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    [JsonProperty(PropertyName = "addTime")]
    public DateTime AddTime { get; set; }

    [JsonProperty(PropertyName = "orderedFood")]
    public Food OrderedFood { get; set; }

    [JsonProperty(PropertyName = "restaurantUrl")]
    public string RestaurantUrl { get; set; }
}
