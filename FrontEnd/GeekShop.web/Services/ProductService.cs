using GeekShop.web.Models;
using Library.Services.Base;
using Microsoft.AspNetCore.Http;

namespace GeekShop.web.Services
{
    public class ProductService : BaseService<ProductViewModel>
    {
        public ProductService(HttpClient client, IHttpContextAccessor contextAccessor)
        : base(client, contextAccessor)
        {
            BasePath = "/api/Product";
        }
    }
}