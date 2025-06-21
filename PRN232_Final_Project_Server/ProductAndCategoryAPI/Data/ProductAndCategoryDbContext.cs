using Microsoft.EntityFrameworkCore;
using ProductAndCategoryAPI.Models;

namespace ProductAndCategoryAPI.Data
{
    public class ProductAndCategoryDbContext : DbContext
    {
        public ProductAndCategoryDbContext(DbContextOptions<ProductAndCategoryDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryID = 1, CategoryName = "Bread", Description = "Freshly baked bread, whole wheat bread, baguettes." },
                new Category { CategoryID = 2, CategoryName = "Cake slice", Description = "Sliced cakes such as cheesecake, mousse, tiramisu." },
                new Category { CategoryID = 3, CategoryName = "Savory", Description = "Savory pastries like dumplings, meat pies, and quiches." },
                new Category { CategoryID = 4, CategoryName = "Special", Description = "Limited edition or seasonal special cakes and pastries." },
                new Category { CategoryID = 5, CategoryName = "Sweet", Description = "Sweet treats including cookies, macarons, and cupcakes." }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { ProductID = 1, ProductName = "Croissant", Description = "Classic French croissant with buttery layers.", Price = 2.99M, StockQuantity = 50, CategoryID = 1, ImageURL = "img/product/bread/croissant.png", IsAvailable = true },
                new Product { ProductID = 2, ProductName = "Golden Lava Bun", Description = "Soft bun filled with rich custard lava.", Price = 3.50M, StockQuantity = 40, CategoryID = 1, ImageURL = "img/product/bread/goldenlavabun.png", IsAvailable = true },
                new Product { ProductID = 3, ProductName = "Gourmet Fruit Loaf", Description = "Fruit loaf packed with dried fruits and nuts.", Price = 4.99M, StockQuantity = 30, CategoryID = 1, ImageURL = "img/product/bread/gourmetfruitloaf.png", IsAvailable = true },
                new Product { ProductID = 4, ProductName = "Sausage Standard", Description = "Savory bread with a classic sausage filling.", Price = 3.20M, StockQuantity = 35, CategoryID = 1, ImageURL = "img/product/bread/sausagestandard.png", IsAvailable = true },
                new Product { ProductID = 5, ProductName = "Spring In The City", Description = "Seasonal bread inspired by spring flavors.", Price = 5.00M, StockQuantity = 25, CategoryID = 1, ImageURL = "img/product/bread/springinthecity.png", IsAvailable = true },

                new Product { ProductID = 6, ProductName = "Berry Choco", Description = "Chocolate cake with fresh berries topping.", Price = 4.99M, StockQuantity = 20, CategoryID = 2, ImageURL = "img/product/cakeslice/bearychoco.png", IsAvailable = true },
                new Product { ProductID = 7, ProductName = "Brownie Cheese Sliced", Description = "Creamy cheesecake layered with brownie crust.", Price = 5.50M, StockQuantity = 25, CategoryID = 2, ImageURL = "img/product/cakeslice/browniecheesesliced.png", IsAvailable = true },
                new Product { ProductID = 8, ProductName = "Bunny", Description = "Cute bunny-shaped cake with vanilla flavor.", Price = 6.00M, StockQuantity = 15, CategoryID = 2, ImageURL = "img/product/cakeslice/bunny.png", IsAvailable = true },
                new Product { ProductID = 9, ProductName = "Chocolate Peanut Mousse", Description = "Chocolate mousse cake with peanut crunch.", Price = 6.99M, StockQuantity = 18, CategoryID = 2, ImageURL = "img/product/cakeslice/bearyberry.png", IsAvailable = true },
                new Product { ProductID = 10, ProductName = "Les Opera Sliced", Description = "French Opera cake with coffee and chocolate.", Price = 7.50M, StockQuantity = 22, CategoryID = 2, ImageURL = "img/product/cakeslice/lesopera.png", IsAvailable = true },
                new Product { ProductID = 11, ProductName = "Oreo Chocolate Cheese", Description = "Chocolate cheesecake topped with Oreo.", Price = 5.99M, StockQuantity = 20, CategoryID = 2, ImageURL = "img/product/cakeslice/bearymatcha.png", IsAvailable = true },

                new Product { ProductID = 12, ProductName = "Chocolate Bun", Description = "Soft chocolate-filled bun.", Price = 3.50M, StockQuantity = 30, CategoryID = 3, ImageURL = "img/product/savory/chocolatebun.png", IsAvailable = true },
                new Product { ProductID = 13, ProductName = "Chocolate Croissant", Description = "Flaky croissant filled with rich chocolate.", Price = 4.20M, StockQuantity = 25, CategoryID = 3, ImageURL = "img/product/savory/chocolatecroissant.png", IsAvailable = true },
                new Product { ProductID = 14, ProductName = "Goma Croissant", Description = "Japanese-style croissant with sesame flavor.", Price = 4.50M, StockQuantity = 20, CategoryID = 3, ImageURL = "img/product/savory/gomacroissant.png", IsAvailable = true },
                new Product { ProductID = 15, ProductName = "Matcha Bun", Description = "Green tea-flavored bun with red bean filling.", Price = 4.80M, StockQuantity = 18, CategoryID = 3, ImageURL = "img/product/savory/matchabun.png", IsAvailable = true },
                new Product { ProductID = 16, ProductName = "Ultimate Choco Croissant", Description = "Extra chocolate-filled croissant.", Price = 5.20M, StockQuantity = 22, CategoryID = 3, ImageURL = "img/product/savory/ultimatechococroissant.png", IsAvailable = true },
                new Product { ProductID = 17, ProductName = "White Choco Croissant", Description = "Croissant with smooth white chocolate filling.", Price = 4.90M, StockQuantity = 25, CategoryID = 3, ImageURL = "img/product/savory/whitechococroissant.png", IsAvailable = true },

                new Product { ProductID = 18, ProductName = "Les Opera", Description = "Luxurious French Opera cake with multiple layers.", Price = 8.99M, StockQuantity = 15, CategoryID = 4, ImageURL = "img/product/cakeslice/lesopera.png", IsAvailable = true },
                new Product { ProductID = 19, ProductName = "MangoCoCo Cake", Description = "Exotic mango and coconut-flavored cake.", Price = 7.99M, StockQuantity = 18, CategoryID = 4, ImageURL = "img/product/special/mangocococake.png", IsAvailable = true },
                new Product { ProductID = 20, ProductName = "Party Pink", Description = "Vibrant pink cake perfect for celebrations.", Price = 6.99M, StockQuantity = 20, CategoryID = 4, ImageURL = "img/product/special/partypink.png", IsAvailable = true },
                new Product { ProductID = 21, ProductName = "Passion Cheese", Description = "Tangy passion fruit cheesecake.", Price = 7.50M, StockQuantity = 18, CategoryID = 4, ImageURL = "img/product/special/passioncheese.png", IsAvailable = true },
                new Product { ProductID = 22, ProductName = "Rainbow", Description = "Colorful multi-layered sponge cake.", Price = 8.50M, StockQuantity = 12, CategoryID = 4, ImageURL = "img/product/special/rainbow.png", IsAvailable = true },
                new Product { ProductID = 23, ProductName = "Snowy Fruity", Description = "Winter-inspired cake with fruity layers.", Price = 7.20M, StockQuantity = 16, CategoryID = 4, ImageURL = "img/product/special/snowyfruity.png", IsAvailable = true },
                new Product { ProductID = 24, ProductName = "Tiramisu", Description = "Classic Italian Tiramisu with coffee and mascarpone.", Price = 7.99M, StockQuantity = 14, CategoryID = 4, ImageURL = "img/product/special/tiramisu.png", IsAvailable = true },

                new Product { ProductID = 25, ProductName = "Chocolate Donut", Description = "Soft donut covered in chocolate glaze.", Price = 3.20M, StockQuantity = 30, CategoryID = 5, ImageURL = "img/product/sweet/chocolatedonut.png", IsAvailable = true },
                new Product { ProductID = 26, ProductName = "Croffle Chocolate Black", Description = "Chocolate-infused croffle with crispy texture.", Price = 4.80M, StockQuantity = 25, CategoryID = 5, ImageURL = "img/product/sweet/chocolatedonut.png", IsAvailable = true },
                new Product { ProductID = 27, ProductName = "Croffle Chocolate White", Description = "White chocolate-coated croffle.", Price = 4.50M, StockQuantity = 28, CategoryID = 5, ImageURL = "img/product/sweet/chocolatedonut.png", IsAvailable = true },
                new Product { ProductID = 28, ProductName = "Flan Cake", Description = "Caramel flan cake with creamy texture.", Price = 5.50M, StockQuantity = 22, CategoryID = 5, ImageURL = "img/product/sweet/chocolatedonut.png", IsAvailable = true },
                new Product { ProductID = 29, ProductName = "Japan Light Cheese", Description = "Fluffy Japanese-style cheesecake.", Price = 6.99M, StockQuantity = 18, CategoryID = 5, ImageURL = "img/product/sweet/chocolatedonut.png", IsAvailable = true }
            );
        }
    }
}
