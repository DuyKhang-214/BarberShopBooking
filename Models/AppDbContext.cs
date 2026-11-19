using Microsoft.EntityFrameworkCore;

namespace BarberShopBooking.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Barber> Barbers { get; set; } = null!;
    public DbSet<Service> Services { get; set; } = null!;
    public DbSet<Appointment> Appointments { get; set; } = null!;
    public DbSet<BarberSchedule> BarberSchedules { get; set; } = null!;
    public DbSet<Contact> Contacts { get; set; } = null!;
    public DbSet<BarberReview> BarberReviews { get; set; } = null!;
    public DbSet<Hairstyle> Hairstyles { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Enum Status conversion mapping for Appointment
        modelBuilder.Entity<Appointment>()
            .Property(a => a.Status)
            .HasConversion(
                status => status.ToDisplayName(),
                text => AppointmentStatusExtensions.ParseDisplayName(text));

        // Decimal Precision
        modelBuilder.Entity<Service>()
            .Property(s => s.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Appointment>()
            .Property(a => a.PriceAtBooking)
            .HasPrecision(18, 2);

        // Unique Indexes
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<BarberReview>()
            .HasIndex(r => r.AppointmentId)
            .IsUnique();

        // Additional Performance Indexes
        modelBuilder.Entity<Appointment>()
            .HasIndex(a => a.UserId);

        modelBuilder.Entity<Appointment>()
            .HasIndex(a => new { a.BarberId, a.AppointmentDate, a.AppointmentTime });

        modelBuilder.Entity<Appointment>()
            .HasIndex(a => a.Status);

        // Relationships & Restrict Cascade Deletes (Preserve History)
        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.User)
            .WithMany(u => u.Appointments)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Barber)
            .WithMany(b => b.Appointments)
            .HasForeignKey(a => a.BarberId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Service)
            .WithMany(s => s.Appointments)
            .HasForeignKey(a => a.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BarberReview>()
            .HasOne(r => r.Appointment)
            .WithOne(a => a.BarberReview)
            .HasForeignKey<BarberReview>(r => r.AppointmentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BarberReview>()
            .HasOne(r => r.User)
            .WithMany(u => u.BarberReviews)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BarberReview>()
            .HasOne(r => r.Barber)
            .WithMany(b => b.BarberReviews)
            .HasForeignKey(r => r.BarberId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BarberSchedule>()
            .HasOne(s => s.Barber)
            .WithMany(b => b.BarberSchedules)
            .HasForeignKey(s => s.BarberId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Contact>()
            .HasOne(c => c.User)
            .WithMany(u => u.Contacts)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        // Seed Initial Data
        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        // Hardcoded static hashes to prevent non-deterministic model changes on EF Core 9 database migrations/updates
        string adminHash = "AQAAAAIAAYagAAAAEO2oX7+0/7lb6DKX1QHNg8cS0RuCEDF/yBICJNpcqjSvWJab4A0CAT87gyhk94jScQ==";
        string customerHash = "AQAAAAIAAYagAAAAEDDMcu8Sz6Os7sHl0afL1q0c1N0BjXnAzLVdJDIM5vF6mb4zVHY3xYImggzWnQ/d4g==";

        // 1. Seed Users (1 Admin, 2 Customers)
        modelBuilder.Entity<User>().HasData(
            new User
            {
                UserId = 1,
                Username = "admin",
                FullName = "Quản Trị Viên",
                Email = "admin@barbershop.com",
                Phone = "0901234567",
                PasswordHash = adminHash,
                Role = "Admin",
                CreatedAt = new DateTime(2026, 1, 1),
                IsActive = true
            },
            new User
            {
                UserId = 2,
                Username = "khachhang1",
                FullName = "Nguyen Van A",
                Email = "nguyenvana@gmail.com",
                Phone = "0987654321",
                PasswordHash = customerHash,
                Role = "Customer",
                CreatedAt = new DateTime(2026, 1, 2),
                IsActive = true
            },
            new User
            {
                UserId = 3,
                Username = "khachhang2",
                FullName = "Tran Thi B",
                Email = "tranthib@gmail.com",
                Phone = "0912345678",
                PasswordHash = customerHash,
                Role = "Customer",
                CreatedAt = new DateTime(2026, 1, 3),
                IsActive = true
            }
        );

        // 2. Seed Barbers (3 Barbers)
        modelBuilder.Entity<Barber>().HasData(
            new Barber
            {
                BarberId = 1,
                FullName = "Trần Văn Hùng",
                Phone = "0933111222",
                SkillDescription = "Chuyên gia Fade & Undercut hàng đầu với 8 năm kinh nghiệm.",
                ImageUrl = "/images/barbers/barber1.jpg",
                ExperienceYears = 8,
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 1)
            },
            new Barber
            {
                BarberId = 2,
                FullName = "Lê Hoàng Nam",
                Phone = "0933222333",
                SkillDescription = "Chuyên gia uốn nhuộm và tạo kiểu Hàn Quốc trẻ trung.",
                ImageUrl = "/images/barbers/barber2.jpg",
                ExperienceYears = 5,
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 1)
            },
            new Barber
            {
                BarberId = 3,
                FullName = "Nguyễn Minh Tuấn",
                Phone = "0933333444",
                SkillDescription = "Chuyên tỉa râu, cắt phong cách cổ điển Gentleman.",
                ImageUrl = "/images/barbers/barber3.jpg",
                ExperienceYears = 6,
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 1)
            }
        );

        // 3. Seed Services (5 Services)
        modelBuilder.Entity<Service>().HasData(
            new Service
            {
                ServiceId = 1,
                ServiceName = "Cắt Tóc Barber Chuyên Nghiệp",
                Description = "Cắt tạo kiểu, tư vấn khuôn mặt, cạo mặt, xả tóc sạch sẽ.",
                Price = 100000m,
                DurationMinutes = 30,
                IsActive = true,
                IsFeatured = true,
                DisplayOrder = 1,
                ImageUrl = "/images/services/service1.jpg"
            },
            new Service
            {
                ServiceId = 2,
                ServiceName = "Combo Cắt + Gội + Massage Đầu",
                Description = "Gói thư giãn toàn diện bao gồm cắt tóc, gội đầu dưỡng sinh và massage đầu vai gáy.",
                Price = 150000m,
                DurationMinutes = 45,
                IsActive = true,
                IsFeatured = true,
                DisplayOrder = 2,
                ImageUrl = "/images/services/service2.jpg"
            },
            new Service
            {
                ServiceId = 3,
                ServiceName = "Uốn Tóc Thời Trang (Preppy/Texture)",
                Description = "Uốn phồng, uốn xoăn nhẹ nhàng giữ nếp tự nhiên từ 2-3 tháng.",
                Price = 300000m,
                DurationMinutes = 60,
                IsActive = true,
                IsFeatured = true,
                DisplayOrder = 3,
                ImageUrl = "/images/services/service3.jpg"
            },
            new Service
            {
                ServiceId = 4,
                ServiceName = "Nhuộm Tóc Tẩy/Không Tẩy",
                Description = "Nhuộm màu thời trang độc đáo với thuốc nhuộm cao cấp bảo vệ da đầu.",
                Price = 350000m,
                DurationMinutes = 60,
                IsActive = true,
                IsFeatured = false,
                DisplayOrder = 4,
                ImageUrl = "/images/services/service4.jpg"
            },
            new Service
            {
                ServiceId = 5,
                ServiceName = "Cạo Râu & Chăm Sóc Khăn Nóng",
                Description = "Dịch vụ cạo râu cổ điển kết hợp ủ khăn nóng thư giãn da mặt.",
                Price = 80000m,
                DurationMinutes = 30,
                IsActive = true,
                IsFeatured = false,
                DisplayOrder = 5,
                ImageUrl = "/images/services/service5.jpg"
            }
        );

        // 4. Seed Barber Schedules (Mon - Sun for each of 3 Barbers)
        var schedules = new List<BarberSchedule>();
        int scheduleId = 1;
        for (int barberId = 1; barberId <= 3; barberId++)
        {
            foreach (DayOfWeek day in Enum.GetValues<DayOfWeek>())
            {
                schedules.Add(new BarberSchedule
                {
                    ScheduleId = scheduleId++,
                    BarberId = barberId,
                    DayOfWeek = day,
                    StartTime = new TimeSpan(8, 0, 0),  // 08:00
                    EndTime = (day == DayOfWeek.Sunday) ? new TimeSpan(17, 0, 0) : new TimeSpan(19, 0, 0),
                    IsWorkingDay = true
                });
            }
        }
        modelBuilder.Entity<BarberSchedule>().HasData(schedules);

        // 5. Seed Sample Hairstyles (Xu hướng Việt Nam)
        modelBuilder.Entity<Hairstyle>().HasData(
            new Hairstyle
            {
                HairstyleId = 1,
                HairstyleName = "Side Part 7/3 Hiện Đại",
                Description = "Kiểu tóc rẽ ngôi 7/3 lịch lãm, vuốt phồng chuẩn quý ông Việt Nam.",
                Category = "Kiểu tóc",
                ImageUrl = "/images/hairstyles/sidepart.jpg",
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 1)
            },
            new Hairstyle
            {
                HairstyleId = 2,
                HairstyleName = "Short Quiff / Crop Fade",
                Description = "Tóc ngắn cắt gọn 2 bên, phần mái vuốt nhè nhẹ cực kỳ mát mẻ, nam tính.",
                Category = "Kiểu tóc",
                ImageUrl = "/images/hairstyles/cropfade.jpg",
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 1)
            },
            new Hairstyle
            {
                HairstyleId = 3,
                HairstyleName = "Mullet Layer Modern",
                Description = "Phong cách Mullet tỉa layer trẻ trung, cá tính và sành điệu.",
                Category = "Kiểu tóc",
                ImageUrl = "/images/hairstyles/mullet.jpg",
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 1)
            },
            new Hairstyle
            {
                HairstyleId = 4,
                HairstyleName = "Nhuộm Xám Khói Thời Trang",
                Description = "Tông màu xám khói tôn da, thời thượng được giới trẻ Việt cực kỳ yêu thích.",
                Category = "Màu tóc",
                ImageUrl = "/images/hairstyles/dye_ashgray.jpg",
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 1)
            },
            new Hairstyle
            {
                HairstyleId = 5,
                HairstyleName = "Nhuộm Nâu Tây Lịch Lãm",
                Description = "Màu nâu tây trầm ấm, tự nhiên, thích hợp cho dân công sở và học sinh sinh viên.",
                Category = "Màu tóc",
                ImageUrl = "/images/hairstyles/dye_brown.jpg",
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 1)
            },
            new Hairstyle
            {
                HairstyleId = 6,
                HairstyleName = "Uốn Texture Preppy Hàn Quốc",
                Description = "Uốn phồng chân tóc, tạo sóng nhẹ tự nhiên dễ chăm sóc hằng ngày.",
                Category = "Uốn tóc",
                ImageUrl = "/images/hairstyles/perm_korean.jpg",
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 1)
            }
        );

        // 6. Seed Sample Appointments
        modelBuilder.Entity<Appointment>().HasData(
            new Appointment
            {
                AppointmentId = 1,
                UserId = 2,
                BarberId = 1,
                ServiceId = 1,
                AppointmentDate = new DateTime(2026, 3, 1),
                AppointmentTime = new TimeSpan(9, 0, 0),
                Note = "Cắt tỉa nhẹ 2 bên",
                PriceAtBooking = 100000m,
                CreatedAt = new DateTime(2026, 2, 28)
            }
        );
    }
}