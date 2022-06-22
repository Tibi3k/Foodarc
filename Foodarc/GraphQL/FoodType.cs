using Foodarc.Model;

namespace Foodarc.GraphQL;

public class FoodType : ObjectType<Food>
{
    protected override void Configure(IObjectTypeDescriptor<Food> descriptor)
    {
        descriptor.Field(a => a.Id).Type<IdType>();
        descriptor.Field(a => a.Calories).Type<IntType>();
        descriptor.Field(a => a.Description).Type<StringType>();
        descriptor.Field(a => a.ImagePath).Type<StringType>();
        descriptor.Field(a => a.Name).Type<StringType>();
        descriptor.Field(a => a.Price).Type<FloatType>();
    }
}

