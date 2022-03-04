using AutoMapper;
using Product.API.Data.ValueObjects;

namespace Product.API.Config
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config => {
                config.CreateMap<ProductVO, Model.Product>();
                config.CreateMap<Model.Product, ProductVO>();
            });

            return mappingConfig;
        }
    }
}