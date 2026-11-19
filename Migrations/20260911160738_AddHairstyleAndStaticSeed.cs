using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BarberShopBooking.Migrations
{
    /// <inheritdoc />
    public partial class AddHairstyleAndStaticSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Hairstyles",
                keyColumn: "HairstyleId",
                keyValue: 1,
                columns: new[] { "Category", "Description", "HairstyleName" },
                values: new object[] { "Kiểu tóc", "Kiểu tóc rẽ ngôi 7/3 lịch lãm, vuốt phồng chuẩn quý ông Việt Nam.", "Side Part 7/3 Hiện Đại" });

            migrationBuilder.UpdateData(
                table: "Hairstyles",
                keyColumn: "HairstyleId",
                keyValue: 2,
                columns: new[] { "Category", "Description", "HairstyleName" },
                values: new object[] { "Kiểu tóc", "Tóc ngắn cắt gọn 2 bên, phần mái vuốt nhè nhẹ cực kỳ mát mẻ, nam tính.", "Short Quiff / Crop Fade" });

            migrationBuilder.UpdateData(
                table: "Hairstyles",
                keyColumn: "HairstyleId",
                keyValue: 3,
                columns: new[] { "Category", "Description", "HairstyleName" },
                values: new object[] { "Kiểu tóc", "Phong cách Mullet tỉa layer trẻ trung, cá tính và sành điệu.", "Mullet Layer Modern" });

            migrationBuilder.InsertData(
                table: "Hairstyles",
                columns: new[] { "HairstyleId", "Category", "CreatedAt", "Description", "HairstyleName", "ImageUrl", "IsActive" },
                values: new object[,]
                {
                    { 4, "Màu tóc", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tông màu xám khói tôn da, thời thượng được giới trẻ Việt cực kỳ yêu thích.", "Nhuộm Xám Khói Thời Trang", "/images/hairstyles/dye_ashgray.jpg", true },
                    { 5, "Màu tóc", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Màu nâu tây trầm ấm, tự nhiên, thích hợp cho dân công sở và học sinh sinh viên.", "Nhuộm Nâu Tây Lịch Lãm", "/images/hairstyles/dye_brown.jpg", true },
                    { 6, "Uốn tóc", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Uốn phồng chân tóc, tạo sóng nhẹ tự nhiên dễ chăm sóc hằng ngày.", "Uốn Texture Preppy Hàn Quốc", "/images/hairstyles/perm_korean.jpg", true }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEO2oX7+0/7lb6DKX1QHNg8cS0RuCEDF/yBICJNpcqjSvWJab4A0CAT87gyhk94jScQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDDMcu8Sz6Os7sHl0afL1q0c1N0BjXnAzLVdJDIM5vF6mb4zVHY3xYImggzWnQ/d4g==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDDMcu8Sz6Os7sHl0afL1q0c1N0BjXnAzLVdJDIM5vF6mb4zVHY3xYImggzWnQ/d4g==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Hairstyles",
                keyColumn: "HairstyleId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Hairstyles",
                keyColumn: "HairstyleId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Hairstyles",
                keyColumn: "HairstyleId",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "Hairstyles",
                keyColumn: "HairstyleId",
                keyValue: 1,
                columns: new[] { "Category", "Description", "HairstyleName" },
                values: new object[] { "Lịch Lãm", "Kiểu tóc lịch lãm, hiện đại phù hợp cho dân công sở.", "Side Part Modern" });

            migrationBuilder.UpdateData(
                table: "Hairstyles",
                keyColumn: "HairstyleId",
                keyValue: 2,
                columns: new[] { "Category", "Description", "HairstyleName" },
                values: new object[] { "Năng Động", "Tóc ngắn cá tính, dễ chăm sóc và tạo kiểu hằng ngày.", "Textured Crop Fade" });

            migrationBuilder.UpdateData(
                table: "Hairstyles",
                keyColumn: "HairstyleId",
                keyValue: 3,
                columns: new[] { "Category", "Description", "HairstyleName" },
                values: new object[] { "Thời Trang", "Phong cách trẻ trung, bụi bặm và thời thượng.", "Mullet Modern" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEL6Vp8qH8xQ7JvY8Z123456789012345678901234567890123");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEL6Vp8qH8xQ7JvY8Z123456789012345678901234567890123");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEL6Vp8qH8xQ7JvY8Z123456789012345678901234567890123");
        }
    }
}
