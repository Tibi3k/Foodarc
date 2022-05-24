using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Foodarc.Model;

public class Basket
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public DateTime LastEdited { get; set; }
    public List<BasketFood> Foods { get; set; }
    public int TotalCost { get; set; }
}
