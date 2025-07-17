using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserAPI.Migrations
{
    /// <inheritdoc />
    public partial class dat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "bytea", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "bytea", nullable: false),
                    Role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    RegistrationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Address", "Email", "FullName", "PasswordHash", "PasswordSalt", "PhoneNumber", "RegistrationDate", "Role" },
                values: new object[,]
                {
                    { 1, "123 Admin St", "admin@example.com", "Admin User", new byte[] { 61, 245, 95, 239, 114, 51, 117, 227, 183, 88, 194, 170, 106, 144, 54, 13, 33, 71, 118, 189, 184, 225, 12, 65, 131, 253, 201, 255, 151, 229, 142, 245, 94, 254, 157, 12, 111, 7, 197, 205, 92, 121, 191, 176, 238, 210, 90, 21, 74, 229, 48, 186, 12, 115, 248, 70, 202, 75, 52, 134, 237, 133, 70, 90 }, new byte[] { 141, 64, 4, 134, 215, 100, 101, 229, 217, 66, 141, 203, 233, 199, 35, 51, 46, 248, 54, 179, 214, 28, 47, 122, 137, 17, 58, 11, 78, 111, 103, 96, 106, 85, 125, 215, 222, 76, 5, 9, 7, 231, 21, 163, 240, 155, 230, 86, 2, 94, 247, 158, 118, 220, 179, 233, 5, 31, 3, 218, 189, 53, 185, 7, 191, 200, 116, 172, 23, 52, 112, 188, 35, 213, 174, 98, 224, 96, 114, 244, 81, 83, 89, 30, 40, 248, 35, 0, 151, 183, 215, 16, 10, 38, 135, 11, 128, 232, 56, 203, 156, 119, 0, 96, 55, 12, 243, 165, 15, 213, 43, 158, 103, 115, 150, 193, 65, 32, 1, 122, 27, 182, 73, 33, 127, 132, 245, 240 }, "0123456789", new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Admin" },
                    { 2, "456 Customer Ave", "customer@example.com", "Customer User", new byte[] { 61, 245, 95, 239, 114, 51, 117, 227, 183, 88, 194, 170, 106, 144, 54, 13, 33, 71, 118, 189, 184, 225, 12, 65, 131, 253, 201, 255, 151, 229, 142, 245, 94, 254, 157, 12, 111, 7, 197, 205, 92, 121, 191, 176, 238, 210, 90, 21, 74, 229, 48, 186, 12, 115, 248, 70, 202, 75, 52, 134, 237, 133, 70, 90 }, new byte[] { 141, 64, 4, 134, 215, 100, 101, 229, 217, 66, 141, 203, 233, 199, 35, 51, 46, 248, 54, 179, 214, 28, 47, 122, 137, 17, 58, 11, 78, 111, 103, 96, 106, 85, 125, 215, 222, 76, 5, 9, 7, 231, 21, 163, 240, 155, 230, 86, 2, 94, 247, 158, 118, 220, 179, 233, 5, 31, 3, 218, 189, 53, 185, 7, 191, 200, 116, 172, 23, 52, 112, 188, 35, 213, 174, 98, 224, 96, 114, 244, 81, 83, 89, 30, 40, 248, 35, 0, 151, 183, 215, 16, 10, 38, 135, 11, 128, 232, 56, 203, 156, 119, 0, 96, 55, 12, 243, 165, 15, 213, 43, 158, 103, 115, 150, 193, 65, 32, 1, 122, 27, 182, 73, 33, 127, 132, 245, 240 }, "0987654321", new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Customer" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
