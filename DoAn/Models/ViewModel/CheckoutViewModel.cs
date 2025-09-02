using System.ComponentModel.DataAnnotations;

namespace DoAn.Models.ViewModel
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "Họ và tên không được để trống")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Địa chỉ nhận hàng không được để trống")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn Tỉnh/Thành phố")]
        public string ProvinceName { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn Quận/Huyện")]
        public string DistrictName { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn Phường/Xã")]
        public string WardName { get; set; }

        public string? Note { get; set; }
    }
}
