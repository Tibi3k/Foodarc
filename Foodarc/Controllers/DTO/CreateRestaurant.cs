using Foodarc.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Foodarc.Controllers.DTO;
public class CreateRestaurant 
{
    [JsonProperty(PropertyName = "address")]
    public string Address { get; set; }

    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    [JsonProperty(PropertyName = "description")]
    public string Description { get; set; }

    [JsonProperty(PropertyName = "imagePath")]
    public string ImagePath { get; set; }

    [JsonProperty(PropertyName = "zipCode")]
    public int ZipCode { get; set; }

    [JsonProperty(PropertyName = "city")]
    public string City { get; set; }

    [JsonProperty(PropertyName = "country")]
    public string Country { get; set; }
}
