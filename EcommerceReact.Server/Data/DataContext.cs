global using Microsoft.EntityFrameworkCore.SqlServer;

using EcommerceReact.Server.Models;

using Microsoft.Extensions.Hosting;

namespace EcommerceReact.Server.Data
{
    public class DataContext: DbContext
    {
        public DataContext():base() { } 
 
        public DataContext(DbContextOptions<DataContext> options):base(options) 
        {
          
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set;}
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set;}

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderLine> OrderLines { get; set; }


    }
}
