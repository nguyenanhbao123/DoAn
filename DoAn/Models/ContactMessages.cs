using System.ComponentModel.DataAnnotations;

namespace DoAn.Models
{
    public partial class ContactMessages
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Họ tên không được bỏ trống")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Email không được bỏ trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Số điện thoại không được bỏ trống")]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "Chủ đề không được bỏ trống")]
        public string Subject { get; set; } = null!;

        [Required(ErrorMessage = "Nội dung không được bỏ trống")]
        public string Message { get; set; } = null!;

        public DateTime SentAt { get; set; } = DateTime.Now;
        public bool IsRead { get; set; } = false;
    }
}
