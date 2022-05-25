using Foodarc.Model;

namespace Foodarc.Controllers.DTO;

public class CreateBasketFood
{
    public Food OrderedFood { get; set; }
    public string RestaurantUrl { get; set; }
}

