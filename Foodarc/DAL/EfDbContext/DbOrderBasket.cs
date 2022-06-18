using Newtonsoft.Json;

namespace Foodarc.DAL.EfDbContext;

public class DbOrderBasket
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    [JsonProperty(PropertyName = "userId")]
    public string UserId { get; set; }

    [JsonProperty(PropertyName = "lastEdited")]
    public DateTime LastEdited { get; set; }

    [JsonProperty(PropertyName = "foods")]
    public List<DbBasketFood> Foods { get; set; }

    [JsonProperty(PropertyName = "totalCost")]
    public int TotalCost { get; set; }
}