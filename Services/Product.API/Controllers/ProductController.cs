using Microsoft.AspNetCore.Mvc;
using Library.Controllers.Base;
using Product.API.Data.ValueObjects;
using Product.API.Repository;
using Product.API.Model.Context;

namespace Product.API.Controllers
{
    [Route("/api/[controller]/")]
    [ApiController]
    public class ProductController : BaseController<ProductRepository, ProductVO, Model.Product, MySqlContext>
    {
        public ProductController(ProductRepository repository) : base(repository)
        {

        }
    }
}