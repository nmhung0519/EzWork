using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Web;

namespace Hrm.Models
{
    [Table("assignwork")]
    public class AssignWorkModel
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("work_id")]
        public int WorkID { get; set; }

        [ForeignKey("WorkID")]
        public WorkModel Work { get; set; }

        [Column("org_id")]
        public int OrgID { get; set; }

        [Column("assign_id")]
        public int AssignID { get; set; }

        [ForeignKey("AssignID")]
        public virtual StaffModel Assign { get; set; }

        [Column("assign_to_id")]
        public int AssignToID { get; set; }

        [ForeignKey("AssignToID")]
        public virtual StaffModel AssignTo { get; set; }

        [Column("note")]
        public string Note { get; set; }

        [Column("status")]
        public int Status { get; set; }

        [Column("assign_time")]
        public DateTime AssignTime { get; set; }

        [Column("complete_time")]
        public DateTime CompleteTime { get; set; }

        [Column("deadline")]
        public DateTime Deadline { get; set; }

        public string getStatus()
        {
            if (Status == -1)
            {
                return "Đã trả lại";
            }
            if (Deadline < DateTime.Now)
            {
                if (Status == 2)
                {
                    if (CompleteTime.Year != 1 && Deadline < CompleteTime) return "Đã hoàn thành quá hạn";
                }
                else if (Status == 1) return "Đã quá hạn";
                else if (Status == 0) return "Đã giao, Đã quá hạn";
            }
            else
            {
                if (Status == 0) return "Đã giao";
                else if (Status == 1) return "Đang thực hiện";
            }
            return "Đã hoàn thành";
        }
    }

    public class AssignWorkObject
    {
        public int ID { get; set; }
        
        public string Title { get; set; }

        public int AssignID { get; set; }

        public string AssignName { get; set; }

        public int AssignToID { get; set; }

        public string AssignToName { get; set; }

        public string Status { get; set; }
    }
}