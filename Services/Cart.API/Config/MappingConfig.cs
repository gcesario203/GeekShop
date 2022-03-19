using AutoMapper;
using Cart.API.Data.ValueObjects;
using Cart.API.Model.DataModel;

namespace Cart.API.Config
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config => {
                config.CreateMap<ProductVO, Product>().ReverseMap();
                config.CreateMap<CartHeaderVO, CartHeader>().ReverseMap();
                config.CreateMap<CartDetailVO, CartDetail>().ReverseMap();
                config.CreateMap<CartVO, Model.Entity.Cart>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}