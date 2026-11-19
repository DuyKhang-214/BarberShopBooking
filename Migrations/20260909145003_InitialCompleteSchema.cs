using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BarberShopBooking.Migrations
{
    /// <inheritdoc />
    public partial class InitialCompleteSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Barbers",
                columns: table => new
                {
                    BarberId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SkillDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExperienceYears = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Barbers", x => x.BarberId);
                });

            migrationBuilder.CreateTable(
                name: "Hairstyles",
                columns: table => new
                {
                    HairstyleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HairstyleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hairstyles", x => x.HairstyleId);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    ServiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.ServiceId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "BarberSchedules",
                columns: table => new
                {
                    ScheduleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BarberId = table.Column<int>(type: "int", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsWorkingDay = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BarberSchedules", x => x.ScheduleId);
                    table.ForeignKey(
                        name: "FK_BarberSchedules_Barbers_BarberId",
                        column: x => x.BarberId,
                        principalTable: "Barbers",
                        principalColumn: "BarberId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    AppointmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BarberId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    AppointmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AppointmentTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PriceAtBooking = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.AppointmentId);
                    table.ForeignKey(
                        name: "FK_Appointments_Barbers_BarberId",
                        column: x => x.BarberId,
                        principalTable: "Barbers",
                        principalColumn: "BarberId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointments_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    ContactId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReplyMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RepliedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.ContactId);
                    table.ForeignKey(
                        name: "FK_Contacts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "BarberReviews",
                columns: table => new
                {
                    ReviewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BarberId = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BarberReviews", x => x.ReviewId);
                    table.ForeignKey(
                        name: "FK_BarberReviews_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "AppointmentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BarberReviews_Barbers_BarberId",
                        column: x => x.BarberId,
                        principalTable: "Barbers",
                        principalColumn: "BarberId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BarberReviews_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Barbers",
                columns: new[] { "BarberId", "CreatedAt", "ExperienceYears", "FullName", "ImageUrl", "IsActive", "Phone", "SkillDescription" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, "Trần Văn Hùng", "/images/barbers/barber1.jpg", true, "0933111222", "Chuyên gia Fade & Undercut hàng đầu với 8 năm kinh nghiệm." },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, "Lê Hoàng Nam", "/images/barbers/barber2.jpg", true, "0933222333", "Chuyên gia uốn nhuộm và tạo kiểu Hàn Quốc trẻ trung." },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, "Nguyễn Minh Tuấn", "/images/barbers/barber3.jpg", true, "0933333444", "Chuyên tỉa râu, cắt phong cách cổ điển Gentleman." }
                });

            migrationBuilder.InsertData(
                table: "Hairstyles",
                columns: new[] { "HairstyleId", "Category", "CreatedAt", "Description", "HairstyleName", "ImageUrl", "IsActive" },
                values: new object[,]
                {
                    { 1, "Lịch Lãm", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kiểu tóc lịch lãm, hiện đại phù hợp cho dân công sở.", "Side Part Modern", "/images/hairstyles/sidepart.jpg", true },
                    { 2, "Năng Động", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tóc ngắn cá tính, dễ chăm sóc và tạo kiểu hằng ngày.", "Textured Crop Fade", "/images/hairstyles/cropfade.jpg", true },
                    { 3, "Thời Trang", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Phong cách trẻ trung, bụi bặm và thời thượng.", "Mullet Modern", "/images/hairstyles/mullet.jpg", true }
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "ServiceId", "Description", "DisplayOrder", "DurationMinutes", "ImageUrl", "IsActive", "IsFeatured", "Price", "ServiceName" },
                values: new object[,]
                {
                    { 1, "Cắt tạo kiểu, tư vấn khuôn mặt, cạo mặt, xả tóc sạch sẽ.", 1, 30, "/images/services/service1.jpg", true, true, 100000m, "Cắt Tóc Barber Chuyên Nghiệp" },
                    { 2, "Gói thư giãn toàn diện bao gồm cắt tóc, gội đầu dưỡng sinh và massage đầu vai gáy.", 2, 45, "/images/services/service2.jpg", true, true, 150000m, "Combo Cắt + Gội + Massage Đầu" },
                    { 3, "Uốn phồng, uốn xoăn nhẹ nhàng giữ nếp tự nhiên từ 2-3 tháng.", 3, 60, "/images/services/service3.jpg", true, true, 300000m, "Uốn Tóc Thời Trang (Preppy/Texture)" },
                    { 4, "Nhuộm màu thời trang độc đáo với thuốc nhuộm cao cấp bảo vệ da đầu.", 4, 60, "/images/services/service4.jpg", true, false, 350000m, "Nhuộm Tóc Tẩy/Không Tẩy" },
                    { 5, "Dịch vụ cạo râu cổ điển kết hợp ủ khăn nóng thư giãn da mặt.", 5, 30, "/images/services/service5.jpg", true, false, 80000m, "Cạo Râu & Chăm Sóc Khăn Nóng" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "Email", "FullName", "IsActive", "PasswordHash", "Phone", "Role", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@barbershop.com", "Quản Trị Viên", true, "AQAAAAIAAYagAAAAEL6Vp8qH8xQ7JvY8Z123456789012345678901234567890123", "0901234567", "Admin", "admin" },
                    { 2, new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "nguyenvana@gmail.com", "Nguyen Van A", true, "AQAAAAIAAYagAAAAEL6Vp8qH8xQ7JvY8Z123456789012345678901234567890123", "0987654321", "Customer", "khachhang1" },
                    { 3, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "tranthib@gmail.com", "Tran Thi B", true, "AQAAAAIAAYagAAAAEL6Vp8qH8xQ7JvY8Z123456789012345678901234567890123", "0912345678", "Customer", "khachhang2" }
                });

            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "AppointmentId", "AppointmentDate", "AppointmentTime", "BarberId", "CreatedAt", "Note", "PriceAtBooking", "ServiceId", "Status", "UserId" },
                values: new object[] { 1, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 9, 0, 0, 0), 1, new DateTime(2026, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cắt tỉa nhẹ 2 bên", 100000m, 1, "Chờ xác nhận", 2 });

            migrationBuilder.InsertData(
                table: "BarberSchedules",
                columns: new[] { "ScheduleId", "BarberId", "DayOfWeek", "EndTime", "IsWorkingDay", "StartTime" },
                values: new object[,]
                {
                    { 1, 1, 0, new TimeSpan(0, 17, 0, 0, 0), true, new TimeSpan(0, 8, 0, 0, 0) },
                    { 2, 1, 1, new TimeSpan(0, 19, 0, 0, 0), true, new TimeSpan(0, 8, 0, 0, 0) },
                    { 3, 1, 2, new TimeSpan(0, 19, 0, 0, 0), true, new TimeSpan(0, 8, 0, 0, 0) },
                    { 4, 1, 3, new TimeSpan(0, 19, 0, 0, 0), true, new TimeSpan(0, 8, 0, 0, 0) },
                    { 5, 1, 4, new TimeSpan(0, 19, 0, 0, 0), true, new TimeSpan(0, 8, 0, 0, 0) },
                    { 6, 1, 5, new TimeSpan(0, 19, 0, 0, 0), true, new TimeSpan(0, 8, 0, 0, 0) },
                    { 7, 1, 6, new TimeSpan(0, 19, 0, 0, 0), true, new TimeSpan(0, 8, 0, 0, 0) },
                    { 8, 2, 0, new TimeSpan(0, 17, 0, 0, 0), true, new TimeSpan(0, 8, 0, 0, 0) },
                    { 9, 2, 1, new TimeSpan(0, 19, 0, 0, 0), true, new TimeSpan(0, 8, 0, 0, 0) },
                    { 10, 2, 2, new TimeSpan(0, 19, 0, 0, 0), true, new TimeSpan(0, 8, 0, 0, 0) },
                    { 11, 2, 3, new TimeSpan(0, 19, 0, 0, 0), true, new TimeSpan(0, 8, 0, 0, 0) },
                    { 12, 2, 4, new TimeSpan(0, 19, 0, 0, 0), true, new TimeSpan(0, 8, 0, 0, 0) },
                    { 13, 2, 5, new TimeSpan(0, 19, 0, 0, 0), true, new TimeSpan(0, 8, 0, 0, 0) },
                    { 14, 2, 6, new TimeSpan(0, 19, 0, 0, 0), true, new TimeSpan(0, 8, 0, 0, 0) },
                    { 15, 3, 0, new TimeSpan(0, 17, 0, 0, 0), true, new TimeSpan(0, 8, 0, 0, 0) },
                    { 16, 3, 1, new TimeSpan(0, 19, 0, 0, 0), true, new TimeSpan(0, 8, 0, 0, 0) },
                    { 17, 3, 2, new TimeSpan(0, 19, 0, 0, 0), true, new TimeSpan(0, 8, 0, 0, 0) },
                    { 18, 3, 3, new TimeSpan(0, 19, 0, 0, 0), true, new TimeSpan(0, 8, 0, 0, 0) },
                    { 19, 3, 4, new TimeSpan(0, 19, 0, 0, 0), true, new TimeSpan(0, 8, 0, 0, 0) },
                    { 20, 3, 5, new TimeSpan(0, 19, 0, 0, 0), true, new TimeSpan(0, 8, 0, 0, 0) },
                    { 21, 3, 6, new TimeSpan(0, 19, 0, 0, 0), true, new TimeSpan(0, 8, 0, 0, 0) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_BarberId_AppointmentDate_AppointmentTime",
                table: "Appointments",
                columns: new[] { "BarberId", "AppointmentDate", "AppointmentTime" });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ServiceId",
                table: "Appointments",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_Status",
                table: "Appointments",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_UserId",
                table: "Appointments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BarberReviews_AppointmentId",
                table: "BarberReviews",
                column: "AppointmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BarberReviews_BarberId",
                table: "BarberReviews",
                column: "BarberId");

            migrationBuilder.CreateIndex(
                name: "IX_BarberReviews_UserId",
                table: "BarberReviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BarberSchedules_BarberId",
                table: "BarberSchedules",
                column: "BarberId");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_UserId",
                table: "Contacts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BarberReviews");

            migrationBuilder.DropTable(
                name: "BarberSchedules");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "Hairstyles");

            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "Barbers");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
