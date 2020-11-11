using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hrm.Models
{
    public class LoginModel
    {
        [Display(Name = "Tên đăng nhập")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tài khoản trống")]
        [StringLength(maximumLength: 20, MinimumLength = 4, ErrorMessage = "Độ dài tài khoản phải từ 4 đến 20")]
        public string Username { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Mật khẩu trống")]
        [StringLength(maximumLength: 20, MinimumLength = 4, ErrorMessage = "Độ dài mật khẩu từ 4 đến 20")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Ghi nhớ đăng nhập")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Display(Name = "Tên đăng nhập")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tài khoản trống")]
        [StringLength(maximumLength: 20, MinimumLength = 4, ErrorMessage = "Độ dài tài khoản phải từ 4 đến 20")]
        public string Username { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Mật khẩu trống")]
        [StringLength(maximumLength: 20, MinimumLength = 4, ErrorMessage = "Độ dài mật khẩu từ 4 đến 20")]
        [DataType(DataType.Password)]
        public string Password1 { get; set; }

        [Display(Name = "Nhập lai mật khẩu")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password1", ErrorMessage = "Mật khẩu nhập lại không giống mật khẩu")]
        public string Password2 { get; set; }

        [Display(Name = "Họ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Họ không được để trống")]
        public string LastName { get; set; }

        [Display(Name = "Tên")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tên không được để trống")]
        public string FirstName { get; set; }

        [Display(Name = "Ngày sinh")]
        public DateTime BirthDay { get; set; }

        [Display(Name = "Giới tính")]
        [Required(ErrorMessage = "Chọn giới tính")]
        public string Sex { get; set; }

        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        public IEnumerable<SelectListItem> SelectSex()
        {
            var Male = Enumerable.Repeat(new SelectListItem
            {
                Value = "M",
                Text = "Nam"
            }, count: 1);
            var Female = Enumerable.Repeat(new SelectListItem
            {
                Value = "F",
                Text = "Nữ"
            }, count: 1);
            return Male.Concat(Female);
        }
    }

}