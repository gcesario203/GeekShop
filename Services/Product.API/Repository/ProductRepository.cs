using AutoMapper;
using Product.API.Data.ValueObjects;
using Product.API.Model.Context;
using Library.Repositories.Base;

namespace Product.API.Repository
{
    public class ProductRepository : BaseRepository<ProductVO, Model.Product, MySqlContext>
    {

        public ProductRepository(MySqlContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}