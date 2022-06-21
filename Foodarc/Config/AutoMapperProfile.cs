using AutoMapper;
using Foodarc.DAL.EfDbContext;
using Foodarc.Model;

namespace Foodarc.Config;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile() : base()
    {
        CreateMap<DbFood, Food>().ReverseMap();
        CreateMap<DbOrder, Order>().ReverseMap();
        CreateMap<DbBasket, Basket>().ReverseMap();
        CreateMap<DbBasketFood, BasketFood>().ReverseMap();
        CreateMap<DbRestaurant, Restaurant>().ReverseMap();
        CreateMap<DbBasket, DbOrderBasket>().ReverseMap();
        CreateMap<DbOrder, DbRestaurantOrder>().ReverseMap();
        CreateMap<DbRestaurantOrder, Order>().ReverseMap();
        CreateMap<DbOrderBasket, DbBasket>().ReverseMap();
        CreateMap<DbOrderBasket, Basket>().ReverseMap();
    }   
}
