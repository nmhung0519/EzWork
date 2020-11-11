using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Hrm.Models
{
    [Table("staff")]
    public class StaffModel
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("user_id")]
        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public virtual UserModel User { get; set; }

        [Column("staffcode")]
        public int StaffCode { get; set; }

        [Column("org_id")]
        public int OrgID { get; set; }

        [ForeignKey("OrgID")]
        public virtual OrgModel Org { get; set; }

        [InverseProperty("Assign")]
        public ICollection<AssignWorkModel> Assigns { get; set; }

        [InverseProperty("AssignTo")]
        public ICollection<AssignWorkModel> AssignTos { get; set; }
    }
}