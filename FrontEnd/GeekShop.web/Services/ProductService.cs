using GeekShop.web.Models;
using Library.Services.Base;

namespace GeekShop.web.Services
{
    public class ProductService : BaseService<ProductModel>
    {
        public ProductService(HttpClient client) : base(client)
        {
            BasePath = "/api/Product";
        }
    }
}