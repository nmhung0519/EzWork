using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Hrm.Models
{
    public class NewWorkModel
    {
        public int OrgID { get; set; }

        public int StaffID { get; set; }

        [Display(Name = "Tiêu đề")]
        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        public string Title { get; set; }

        [Display(Name = "Nội dung")]
        [Required(ErrorMessage = "Nội dung không được để trống")]
        public string Content { get; set; }
        public int Assignee { get; set; }

        [Display(Name = "Người thực hiện")]
        public int AssignTo { get; set; }

        [Display(Name = "Thời hạn")]
        [DataType(DataType.DateTime)]
        public DateTime Deadline { get; set; }

        public List<StaffModel> Staffs { get; set; }

        public IEnumerable<SelectListItem> ListStaff()
        {
            var Title = Enumerable.Repeat(new SelectListItem
            {
                Text = "Người thực hiện",
                Value = "-1",
                Disabled = true
            }
            , count: 1);
            var staff = Staffs.Select(f => new SelectListItem
            {
                Value = f.StaffCode.ToString(),
                Text = f.StaffCode.ToString() + " - " + f.User.GetName().ToString()
            });
            return Title.Concat(staff);
        }
    }
}