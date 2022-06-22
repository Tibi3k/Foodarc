using Foodarc.Model;

namespace Foodarc.GraphQL;

public class RestaurantType : ObjectType<Restaurant>
{
    protected override void Configure(IObjectTypeDescriptor<Restaurant> descriptor)
    {
        descriptor.Field(a => a.Id).Type<IdType>();
        descriptor.Field(a => a.Address).Type<StringType>();
        descriptor.Field(a => a.City).Type<StringType>();
        descriptor.Field(a => a.Country).Type<StringType>();
        descriptor.Field(a => a.Description).Type<StringType>();
        descriptor.Field(a => a.ImagePath).Type<StringType>();
        descriptor.Field(a => a.OwnerId).Type<StringType>();
        descriptor.Field(a => a.ZipCode).Type<IntType>();
        descriptor.Field(a => a.Name).Type<StringType>();
        descriptor.Field<FoodResolver>(a => a.GetFoodsOfRestaurant(default, default)).Type<StringType>();
        //orders
    }
}
