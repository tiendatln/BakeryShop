using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProductAndCategoryAPI.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    StockQuantity = table.Column<int>(type: "int", nullable: false),
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryID", "CategoryName", "Description" },
                values: new object[,]
                {
                    { 1, "Bread", "Freshly baked bread, whole wheat bread, baguettes." },
                    { 2, "Cake slice", "Sliced cakes such as cheesecake, mousse, tiramisu." },
                    { 3, "Savory", "Savory pastries like dumplings, meat pies, and quiches." },
                    { 4, "Special", "Limited edition or seasonal special cakes and pastries." },
                    { 5, "Sweet", "Sweet treats including cookies, macarons, and cupcakes." }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductID", "CategoryID", "CreatedDate", "Description", "ImageURL", "IsAvailable", "Price", "ProductName", "StockQuantity" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 6, 5, 0, 0, 0, 0, DateTimeKind.Local), "Classic French croissant with buttery layers.", "img/PRODUCT/Bread/Croissant.png", true, 2.99m, "Croissant", 50 },
                    { 2, 1, new DateTime(2025, 6, 5, 0, 0, 0, 0, DateTimeKind.Local), "Soft bun filled with rich custard lava.", "img/PRODUCT/Bread/Golden Lava Bun.png", true, 3.50m, "Golden Lava Bun", 40 },
                    { 3, 1, new DateTime(2025, 6, 5, 0, 0, 0, 0, DateTimeKind.Local), "Fruit loaf packed with dried fruits and nuts.", "img/PRODUCT/Bread/Gourmet Fruit Loaf.png", true, 4.99m, "Gourmet Fruit Loaf", 30 },
                    { 4, 1, new DateTime(2025, 6, 5, 0, 0, 0, 0, DateTimeKind.Local), "Savory bread with a classic sausage filling.", "img/PRODUCT/Bread/Sausage Standard.png", true, 3.20m, "Sausage Standard", 35 },
                    { 5, 1, new DateTime(2025, 6, 5, 0, 0, 0, 0, DateTimeKind.Local), "Seasonal bread inspired by spring flavors.", "img/PRODUCT/Bread/Spring In The City.png", true, 5.00m, "Spring In The City", 25 },
                    { 6, 2, new DateTime(2025, 6, 5, 0, 0, 0, 0, DateTimeKind.Local), "Chocolate cake with fresh berries topping.", "img/PRODUCT/Cake slice/Berry Choco.png", true, 4.99m, "Berry Choco", 20 },
                    { 7, 2, new DateTime(2025, 6, 5, 0, 0, 0, 0, DateTimeKind.Local), "Creamy cheesecake layered with brownie crust.", "img/PRODUCT/Cake slice/Brownie Cheese Sliced.jpg", true, 5.50m, "Brownie Cheese Sliced", 25 },
                    { 8, 2, new DateTime(2025, 6, 5, 0, 0, 0, 0, DateTimeKind.Local), "Cute bunny-shaped cake with vanilla flavor.", "img/PRODUCT/Cake slice/Bunny.png", true, 6.00m, "Bunny", 15 },
                    { 9, 2, new DateTime(2025, 6, 5, 0, 0, 0, 0, DateTimeKind.Local), "Chocolate mousse cake with peanut crunch.", "img/PRODUCT/Cake slice/Chocolate Peanut Mousse.png", true, 6.99m, "Chocolate Peanut Mousse", 18 },
                    { 10, 2, new DateTime(2025, 6, 5, 0, 0, 0, 0, DateTimeKind.Local), "French Opera cake with coffee and chocolate.", "img/PRODUCT/Cake slice/Les Opera Sliced.png", true, 7.50m, "Les Opera Sliced", 22 },
                    { 11, 2, new DateTime(2025, 6, 5, 0, 0, 0, 0, DateTimeKind.Local), "Chocolate cheesecake topped with Oreo.", "img/PRODUCT/Cake slice/Oreo Chocolate Cheese.png", true, 5.99m, "Oreo Chocolate Cheese", 20 },
                    { 12, 3, new DateTime(2025, 6, 5, 0, 0, 0, 0, DateTimeKind.Local), "Soft chocolate-filled bun.", "img/PRODUCT/Savory/Chocolate Bun.png", true, 3.50m, "Chocolate Bun", 30 },
                    { 13, 3, new DateTime(2025, 6, 5, 0, 0, 0, 0, DateTimeKind.Local), "Flaky croissant filled with rich chocolate.", "img/PRODUCT/Savory/Chocolate Croissant.png", true, 4.20m, "Chocolate Croissant", 25 },
                    { 14, 3, new DateTime(2025, 6, 5, 0, 0, 0, 0, DateTimeKind.Local), "Japanese-style croissant with sesame flavor.", "img/PRODUCT/Savory/Goma Croissant.png", true, 4.50m, "Goma Croissant", 20 },
                    { 15, 3, new DateTime(2025, 6, 5, 0, 0, 0, 0, DateTimeKind.Local), "Green tea-flavored bun with red bean filling.", "img/PRODUCT/Savory/Matcha Bun.png", true, 4.80m, "Matcha Bun", 18 },
                    { 16, 3, new DateTime(2025, 6, 5, 0, 0, 0, 0, DateTimeKind.Local), "Extra chocolate-filled croissant.", "img/PRODUCT/Savory/Ultimate Choco Croissant.png", true, 5.20m, "Ultimate Choco Croissant", 22 },
                    { 17, 3, new DateTime(2025, 6, 5, 0, 0, 0, 0, DateTimeKind.Local), "Croissant with smooth white chocolate filling.", "img/PRODUCT/Savory/White Choco Croissant.png", true, 4.90m, "White Choco Croissant", 25 },
                    { 18, 4, new DateTime(2025, 6, 5, 0, 0, 0, 0, DateTimeKind.Local), "Luxurious French Opera cake with multiple layers.", "img/PRODUCT/Special/Les Opera.png", true, 8.99m, "Les Opera", 15 },
                    { 19, 4, new DateTime(2025, 6, 5, 0, 0, 0, 0, DateTimeKind.Local), "Exotic mango and coconut-flavored cake.", "img/PRODUCT/Special/MangoCoCo Cake.png", true, 7.99m, "MangoCoCo Cake", 18 },
                    { 20, 4, new DateTime(2025, 6, 5, 0, 0, 0, 0, DateTimeKind.Local), "Vibrant pink cake perfect for celebrations.", "img/PRODUCT/Special/Party Pink.png", true, 6.99m, "Party Pink", 20 },
                    { 21, 4, new DateTime(2025, 6, 5, 0, 0, 0, 0, DateTimeKind.Local), "Tangy passion fruit cheesecake.", "img/PRODUCT/Special/Passion Cheese.png", true, 7.50m, "Passion Cheese", 18 },
                    { 22, 4, new DateTime(2025, 6, 5, 0, 0, 0, 0, DateTimeKind.Local), "Colorful multi-layered sponge cake.", "img/PRODUCT/Special/Rainbow.png", true, 8.50m, "Rainbow", 12 },
                    { 23, 4, new DateTime(2025, 6, 5, 0, 0, 0, 0, DateTimeKind.Local), "Winter-inspired cake with fruity layers.", "img/PRODUCT/Special/Snowy Fruity.png", true, 7.20m, "Snowy Fruity", 16 },
                    { 24, 4, new DateTime(2025, 6, 5, 0, 0, 0, 0, DateTimeKind.Local), "Classic Italian Tiramisu with coffee and mascarpone.", "img/PRODUCT/Special/Tiramisu.png", true, 7.99m, "Tiramisu", 14 },
                    { 25, 5, new DateTime(2025, 6, 5, 0, 0, 0, 0, DateTimeKind.Local), "Soft donut covered in chocolate glaze.", "img/PRODUCT/Sweet/Chocolate Donut.png", true, 3.20m, "Chocolate Donut", 30 },
                    { 26, 5, new DateTime(2025, 6, 5, 0, 0, 0, 0, DateTimeKind.Local), "Chocolate-infused croffle with crispy texture.", "img/PRODUCT/Sweet/Croffle Chocolate Black.png", true, 4.80m, "Croffle Chocolate Black", 25 },
                    { 27, 5, new DateTime(2025, 6, 5, 0, 0, 0, 0, DateTimeKind.Local), "White chocolate-coated croffle.", "img/PRODUCT/Sweet/Croffle Chocolate White.png", true, 4.50m, "Croffle Chocolate White", 28 },
                    { 28, 5, new DateTime(2025, 6, 5, 0, 0, 0, 0, DateTimeKind.Local), "Caramel flan cake with creamy texture.", "img/PRODUCT/Sweet/Flan Cake.png", true, 5.50m, "Flan Cake", 22 },
                    { 29, 5, new DateTime(2025, 6, 5, 0, 0, 0, 0, DateTimeKind.Local), "Fluffy Japanese-style cheesecake.", "img/PRODUCT/Sweet/Japan Light Cheese.png", true, 6.99m, "Japan Light Cheese", 18 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryID",
                table: "Products",
                column: "CategoryID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
