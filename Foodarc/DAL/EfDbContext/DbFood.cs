using Newtonsoft.Json;

namespace Foodarc.DAL.EfDbContext;

public class DbFood
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

    [JsonProperty(PropertyName = "imagePath")]
    public string ImagePath { get; set; }
}
