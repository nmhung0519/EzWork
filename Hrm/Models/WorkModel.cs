using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Hrm.Models
{
    [Table("work")]
    public class WorkModel
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("org_id")]
        public int OrgID { get; set; }

        [ForeignKey("OrgID")]
        public virtual OrgModel Org { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("content")]
        public string Content { get; set; }

        [Column("creater")]
        public int CreaterID { get; set; }

        [ForeignKey("CreaterID")]
        public virtual StaffModel Creater { get; set; }

        [Column("createtime")]
        public DateTime CreateTime { get; set; }

        public virtual ICollection<AssignWorkModel> AssignWorks { get; set; }
    }
}