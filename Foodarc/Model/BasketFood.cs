using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Foodarc.Model;

public class BasketFood
{
    public string Id { get; set; }
    public DateTime AddTime { get; set; }
    public Food OrderedFood { get; set; }
    public string RestaurantUrl { get; set; }
}
