using Newtonsoft.Json;

namespace Foodarc.Model;

public class Food
{

    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    [JsonProperty(PropertyName = "description")]
    public string Description { get; set; }

    [JsonProperty(PropertyName = "price")]
    public int Price { get; set; }

    [JsonProperty(PropertyName = "calories")]
    public int Calories { get; set; }

}
