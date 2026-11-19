namespace BarberShopBooking.Models;

/// <summary>
/// Thực thể Tài Khoản Người Dùng (User).
/// Bao gồm 2 Vai trò chính (Actors):
/// - "Customer": Khách hàng đăng ký tài khoản để đặt lịch & viết đánh giá.
/// - "Admin": Quản trị viên toàn quyền quản lý hệ thống BarberShop.
/// </summary>
public class User
{
    public int UserId { get; set; }

    // Tên đăng nhập duy nhất
    public string Username { get; set; } = "";

    // Họ và tên người dùng
    public string FullName { get; set; } = "";

    // Email liên hệ duy nhất
    public string Email { get; set; } = "";

    // Số điện thoại liên lạc
    public string? Phone { get; set; }

    // Mật khẩu đã mã hóa bằng PasswordHasher<User>
    public string PasswordHash { get; set; } = "";

    // Vai trò hệ thống: "Admin" hoặc "Customer"
    public string Role { get; set; } = "Customer";

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Trạng thái tài khoản (true = Hoạt động, false = Khóa)
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<BarberReview> BarberReviews { get; set; } = new List<BarberReview>();
    public ICollection<Contact> Contacts { get; set; } = new List<Contact>();
}