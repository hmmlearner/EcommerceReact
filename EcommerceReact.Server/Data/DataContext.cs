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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // Seed data for Regions
            var categories = new List<Category>
            {
                new Category
                {
                    Id = 1,
                    Name = "Kitchen",
                    DisplayOrder = 100,
                    CreatedDateTime = DateTime.Now
                },
                new Category
                {
                    Id = 2,
                    Name = "Outdoor",
                    DisplayOrder = 200,
                    CreatedDateTime = DateTime.Now
                },


            };

            modelBuilder.Entity<Category>().HasData(categories);

            var products = new List<Product>()
            {
                new Product()
                {
                    Id = 1,
                    Title="Pan",
                    Description = "Introducing the Shiny Pan – a culinary gem that adds brilliance to your kitchen. Crafted with a gleaming, high-quality stainless steel exterior, this pan not only dazzles with its polished finish but also ensures superior durability and even heat distribution. The Shiny Pan features a non-stick interior for easy cooking and cleaning, making every culinary endeavor a joy. Its ergonomic handle guarantees a comfortable grip, while the radiant design adds a touch of elegance to your cooking space. Elevate your kitchen aesthetics and cooking precision with the Shiny Pan – where style meets functionality for a truly dazzling culinary experience.",
                    SKU= "PAN",
                    Price = 100,
                    SalePrice = 90,
                    WasPrice = 120,
                    ImageUrl="pan.jpg",
                    CategoryId = 1,
                    CreatedDate = DateTime.Now,
                    Inventory = 100,
                },
                new Product()
                {
                    Id = 2,
                    Title="Pot",
                    Description = "Introducing the Shiny Pot – a sleek and versatile addition to your kitchen. Crafted with a lustrous stainless steel exterior, this pot not only radiates elegance but also promises durability and efficient heat distribution. Its capacious design accommodates various culinary creations, from hearty soups to flavorful stews. The Shiny Pot features a non-stick interior for easy cooking and cleaning, while the ergonomic handle ensures a secure grip. Elevate your cooking experience with this stylish yet practical pot, where sophistication meets functionality. Choose the Shiny Pot for a shimmering touch to your kitchen and a reliable partner in crafting delicious meals.",
                    SKU= "POT",
                    Price = 100,
                    SalePrice = 90,
                    WasPrice = 120,
                    ImageUrl="pot.jpg",
                    CategoryId = 1,
                    CreatedDate = DateTime.Now,
                    Inventory = 15,
                },
                new Product()
                {
                    Id = 3,
                    Title="Glasses",
                    Description = "Introducing our Shiny Glasses – a perfect blend of style and sophistication for your beverage experience. Crafted with meticulous attention to detail, these glasses boast a crystal-clear design that enhances the visual appeal of your drinks. The shiny finish adds a touch of elegance, making them a standout choice for any occasion. Whether you're enjoying a refreshing cocktail or a fine wine, the Shiny Glasses elevate your sipping experience. The premium quality ensures durability, while the sleek profile feels comfortable in your hand. Choose Shiny Glasses to make a statement at your table – where aesthetics and functionality seamlessly meet.",
                    SKU= "GLS",
                    Price = 100,
                    SalePrice = 90,
                    WasPrice = 120,
                    ImageUrl="glasses.jpg",
                    CategoryId = 1,
                    CreatedDate = DateTime.Now,
                    Inventory = 100,
                },
                new Product()
                {
                    Id = 4,
                    Title="Potato Planters Set of 3",
                    Description = "Introducing our innovative Potato Planter – no digging required. Save space, avoid harmful pesticides and enjoy tasty, home grown potatoes",
                    SKU= "POTAS",
                    Price = 19.95,
                    SalePrice = 18.95,
                    WasPrice = 24.95,
                    ImageUrl="potato.jpg",
                    CategoryId = 2,
                    CreatedDate = DateTime.Now,
                    Inventory = 10,
                }
            };

            // Seed difficulties to the database
            modelBuilder.Entity<Product>().HasData(products);
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
