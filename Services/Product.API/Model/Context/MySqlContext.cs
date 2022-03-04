using Library.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Product.API.Model.Context
{
    public class MySqlContext : BaseDBContext<MySqlContext>
    {

        public DbSet<Product> Products {get;set;}
        public MySqlContext()
        {
            
        }
        public MySqlContext(DbContextOptions<MySqlContext> options) : base(options)
        {
            
        }
    }
}