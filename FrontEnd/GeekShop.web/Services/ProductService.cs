using GeekShop.web.Models;
using Library.Services.Base;

namespace GeekShop.web.Services
{
    public class ProductService : BaseService<ProductModel>
    {
        public ProductService(string basePath, HttpClient client) : base(basePath, client)
        {
        }
    }
}