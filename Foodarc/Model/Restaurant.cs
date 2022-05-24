using Newtonsoft.Json;

namespace Foodarc.Model;

public class Restaurant
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    [JsonProperty(PropertyName = "availableFoods")]
    public List<Food> AvailableFoods { get; set; }

    [JsonProperty(PropertyName ="zipCode")]
    public int ZipCode { get; set; }

    [JsonProperty(PropertyName = "city")]
    public string City { get; set; }

    [JsonProperty(PropertyName = "country")]
    public string Country { get; set; }

    [JsonProperty(PropertyName = "address")]
    public string Address { get; set; }

    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    [JsonProperty(PropertyName = "ownerId")]
    public string OwnerId { get; set; }

    [JsonProperty(PropertyName = "description")]
    public string Description { get; set; }

    [JsonProperty(PropertyName = "imagePath")]
    public string ImagePath { get; set; }
}
