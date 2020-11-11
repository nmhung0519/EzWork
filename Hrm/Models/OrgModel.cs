using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Hrm.Models
{
    [Table("org")]
    public class OrgModel
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("creater_id")]
        public int CreaterID { get; set; }

        [ForeignKey("CreaterID")]
        public virtual UserModel Creater { get; set; }

        [Column("create_time")]
        public DateTime CreaterTime { get; set; }

        public ICollection<WorkModel> Works { get; set; }
        public ICollection<StaffModel> Staffs { get; set; }
        public ICollection<InvitationModel> Invitations { get; set; }
    }
}