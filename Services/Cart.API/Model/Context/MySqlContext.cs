using Cart.API.Model.DataModel;
using Library.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Cart.API.Model.Context
{
    public class MySqlContext : BaseDBContext<MySqlContext>
    {

        public DbSet<Product> Products { get; set; }
        public DbSet<CartDetail> CartDetails { get; set; }
        public DbSet<CartHeader> CartHeaders { get; set; }

        public MySqlContext()
        {

        }
        public MySqlContext(DbContextOptions<MySqlContext> options) : base(options)
        {

        }
    }
}