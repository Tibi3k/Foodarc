using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Foodarc.Model;

public class Basket
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    [JsonProperty(PropertyName = "userId")]
    public string UserId { get; set; }

    [JsonProperty(PropertyName = "lastEdited")]
    public DateTime LastEdited { get; set; }

    [JsonProperty(PropertyName = "foods")]
    public List<BasketFood> Foods { get; set; }

    [JsonProperty(PropertyName = "totalCost")]
    public int TotalCost { get; set; }
}
