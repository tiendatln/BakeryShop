using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RegistrationDate",
                table: "Users",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Address", "Email", "FullName", "PasswordHash", "PasswordSalt", "PhoneNumber", "RegistrationDate", "Role" },
                values: new object[,]
                {
                    { 1, "123 Admin St", "admin@example.com", "Admin User", new byte[] { 200, 65, 189, 120, 187, 178, 209, 70, 243, 227, 62, 91, 175, 5, 247, 98, 113, 103, 13, 148, 102, 198, 228, 221, 209, 243, 70, 21, 184, 45, 79, 239, 205, 47, 252, 110, 209, 7, 117, 250, 41, 38, 18, 155, 82, 241, 254, 121, 249, 162, 213, 238, 195, 114, 9, 251, 231, 45, 243, 33, 254, 39, 162, 7 }, new byte[] { 150, 171, 146, 107, 166, 75, 104, 5, 131, 172, 23, 202, 139, 180, 235, 179, 75, 244, 33, 120, 220, 71, 205, 80, 219, 198, 43, 84, 17, 65, 160, 130, 123, 196, 91, 174, 70, 74, 96, 105, 213, 37, 116, 140, 39, 129, 141, 102, 130, 65, 176, 99, 70, 99, 133, 162, 146, 221, 76, 74, 250, 24, 179, 76, 70, 124, 38, 7, 42, 53, 74, 10, 47, 141, 111, 86, 246, 101, 255, 126, 114, 6, 130, 8, 168, 193, 164, 185, 224, 71, 140, 119, 212, 30, 80, 35, 115, 33, 29, 206, 170, 31, 89, 181, 218, 157, 85, 58, 94, 170, 223, 156, 135, 192, 188, 159, 107, 7, 207, 191, 75, 46, 84, 1, 30, 159, 192, 105 }, "0123456789", new DateTime(2025, 6, 3, 15, 20, 10, 343, DateTimeKind.Utc).AddTicks(7616), "Admin" },
                    { 2, "456 Customer Ave", "customer@example.com", "Customer User", new byte[] { 60, 67, 9, 101, 16, 250, 185, 15, 45, 145, 84, 222, 101, 137, 79, 13, 95, 177, 36, 250, 167, 133, 8, 183, 132, 251, 14, 255, 46, 224, 34, 185, 29, 195, 90, 253, 13, 248, 64, 198, 76, 218, 30, 149, 154, 208, 93, 63, 238, 125, 145, 52, 87, 127, 247, 3, 172, 233, 72, 218, 5, 159, 33, 6 }, new byte[] { 60, 168, 78, 250, 212, 142, 12, 126, 7, 234, 232, 30, 115, 107, 68, 237, 54, 234, 244, 70, 102, 150, 128, 3, 217, 180, 178, 224, 13, 160, 219, 84, 254, 134, 16, 178, 246, 30, 197, 39, 45, 69, 125, 66, 253, 167, 42, 22, 3, 172, 4, 105, 160, 24, 120, 241, 203, 186, 248, 21, 203, 26, 110, 103, 228, 192, 175, 167, 59, 80, 8, 113, 253, 169, 121, 133, 160, 36, 245, 233, 103, 159, 197, 153, 46, 24, 159, 21, 133, 67, 162, 1, 222, 250, 176, 164, 15, 158, 222, 211, 88, 241, 169, 176, 46, 161, 133, 77, 198, 148, 247, 223, 19, 79, 226, 88, 3, 218, 9, 22, 95, 191, 45, 210, 98, 218, 119, 170 }, "0987654321", new DateTime(2025, 6, 3, 15, 20, 10, 343, DateTimeKind.Utc).AddTicks(7860), "Customer" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegistrationDate",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
