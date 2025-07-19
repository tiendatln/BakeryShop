using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OrderAPI.Migrations
{
    /// <inheritdoc />
    public partial class dat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserID = table.Column<int>(type: "integer", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    ShippingAddress = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    OrderStatus = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    PaymentMethod = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PaymentStatus = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderID);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    OrderDetailID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderID = table.Column<int>(type: "integer", nullable: false),
                    ProductID = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.OrderDetailID);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderID", "OrderDate", "OrderStatus", "PaymentMethod", "PaymentStatus", "ShippingAddress", "TotalAmount", "UserID" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pending", "Credit Card", "Paid", "Quận 12 - TP.HCM", 100m, 2 },
                    { 2, new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Shipped", "COD", "Pending", "Quận 11 - TP.HCM", 150m, 2 },
                    { 3, new DateTime(2025, 6, 3, 0, 0, 0, 0, DateTimeKind.Utc), "Pending", "Credit Card", "Pending", "Quận 9 - TP.HCM", 200m, 2 },
                    { 4, new DateTime(2025, 6, 4, 0, 0, 0, 0, DateTimeKind.Utc), "Shipped", "COD", "Paid", "Quận 10 - TP.HCM", 250m, 2 },
                    { 5, new DateTime(2025, 6, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Pending", "Credit Card", "Pending", "Quận 9 - TP.HCM", 300m, 2 },
                    { 6, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Utc), "Shipped", "COD", "Pending", "Quận 1 - TP.HCM", 350m, 2 },
                    { 7, new DateTime(2025, 6, 7, 0, 0, 0, 0, DateTimeKind.Utc), "Pending", "Credit Card", "Paid", "Quận 1 - TP.HCM", 400m, 2 },
                    { 8, new DateTime(2025, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "Shipped", "COD", "Pending", "Quận 2 - TP.HCM", 450m, 2 },
                    { 9, new DateTime(2025, 6, 9, 0, 0, 0, 0, DateTimeKind.Utc), "Pending", "Credit Card", "Pending", "Quận 3 - TP.HCM", 500m, 2 },
                    { 10, new DateTime(2025, 6, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Shipped", "COD", "Paid", "Quận 8 - TP.HCM", 550m, 2 },
                    { 11, new DateTime(2025, 6, 11, 0, 0, 0, 0, DateTimeKind.Utc), "Pending", "Credit Card", "Pending", "Quận 11 - TP.HCM", 600m, 2 },
                    { 12, new DateTime(2025, 6, 12, 0, 0, 0, 0, DateTimeKind.Utc), "Shipped", "COD", "Pending", "Quận 6 - TP.HCM", 650m, 2 },
                    { 13, new DateTime(2025, 6, 13, 0, 0, 0, 0, DateTimeKind.Utc), "Pending", "Credit Card", "Paid", "Quận 3 - TP.HCM", 700m, 2 },
                    { 14, new DateTime(2025, 6, 14, 0, 0, 0, 0, DateTimeKind.Utc), "Shipped", "COD", "Pending", "Quận 6 - TP.HCM", 750m, 2 },
                    { 15, new DateTime(2025, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Pending", "Credit Card", "Pending", "Quận 1 - TP.HCM", 800m, 2 },
                    { 16, new DateTime(2025, 6, 16, 0, 0, 0, 0, DateTimeKind.Utc), "Shipped", "COD", "Paid", "Quận 11 - TP.HCM", 850m, 2 },
                    { 17, new DateTime(2025, 6, 17, 0, 0, 0, 0, DateTimeKind.Utc), "Pending", "Credit Card", "Pending", "Quận 7 - TP.HCM", 900m, 2 },
                    { 18, new DateTime(2025, 6, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Shipped", "COD", "Pending", "Quận 4 - TP.HCM", 950m, 2 },
                    { 19, new DateTime(2025, 6, 19, 0, 0, 0, 0, DateTimeKind.Utc), "Pending", "Credit Card", "Paid", "Quận 1 - TP.HCM", 1000m, 2 },
                    { 20, new DateTime(2025, 6, 20, 0, 0, 0, 0, DateTimeKind.Utc), "Shipped", "COD", "Pending", "Quận 12 - TP.HCM", 1050m, 2 }
                });

            migrationBuilder.InsertData(
                table: "OrderDetails",
                columns: new[] { "OrderDetailID", "OrderID", "ProductID", "Quantity", "UnitPrice" },
                values: new object[,]
                {
                    { 1, 1, 1, 4, 10m },
                    { 2, 2, 19, 3, 90m },
                    { 3, 2, 16, 1, 63m },
                    { 4, 2, 25, 4, 16m },
                    { 5, 3, 3, 4, 28m },
                    { 6, 3, 12, 4, 93m },
                    { 7, 4, 15, 1, 78m },
                    { 8, 4, 4, 1, 44m },
                    { 9, 4, 5, 2, 73m },
                    { 10, 5, 1, 3, 84m },
                    { 11, 6, 4, 3, 15m },
                    { 12, 6, 3, 2, 20m },
                    { 13, 6, 19, 1, 13m },
                    { 14, 7, 22, 1, 88m },
                    { 15, 8, 27, 2, 86m },
                    { 16, 8, 7, 1, 34m },
                    { 17, 8, 16, 3, 32m },
                    { 18, 9, 14, 2, 76m },
                    { 19, 9, 2, 2, 49m },
                    { 20, 10, 7, 1, 66m },
                    { 21, 10, 28, 1, 23m },
                    { 22, 10, 9, 2, 83m },
                    { 23, 11, 13, 1, 72m },
                    { 24, 12, 2, 3, 26m },
                    { 25, 12, 16, 2, 37m },
                    { 26, 13, 16, 3, 48m },
                    { 27, 13, 16, 2, 47m },
                    { 28, 13, 19, 1, 44m },
                    { 29, 14, 18, 3, 29m },
                    { 30, 14, 16, 4, 31m },
                    { 31, 15, 22, 1, 58m },
                    { 32, 16, 6, 1, 18m },
                    { 33, 16, 13, 3, 33m },
                    { 34, 16, 12, 4, 60m },
                    { 35, 17, 3, 1, 13m },
                    { 36, 17, 20, 3, 28m },
                    { 37, 18, 20, 3, 69m },
                    { 38, 18, 11, 2, 70m },
                    { 39, 19, 17, 2, 86m },
                    { 40, 19, 6, 2, 24m },
                    { 41, 19, 24, 1, 78m },
                    { 42, 20, 10, 3, 40m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderID",
                table: "OrderDetails",
                column: "OrderID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
