using Microsoft.EntityFrameworkCore;

namespace Library.Contexts
{
    public class BaseDBContext<G> : DbContext where G : DbContext
    {
        public BaseDBContext()
        {}

        public BaseDBContext(DbContextOptions<G> options) :base(options)
        {
        }

        public DbSet<T> GetDbSetByType<T>() where T : class
        {
            return (DbSet<T>)this.GetType()
                            .GetProperties()
                            .Where(x => x.PropertyType.AssemblyQualifiedName == typeof(DbSet<T>).AssemblyQualifiedName)
                            .FirstOrDefault()
                            .GetValue(this);
        }
    }
}