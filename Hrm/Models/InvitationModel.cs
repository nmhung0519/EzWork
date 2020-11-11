using Antlr.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Hrm.Models
{
    [Table("invitation")]
    public class InvitationModel
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("orgid")]
        public int OrgID { get; set; }

        [ForeignKey("OrgID")]
        public virtual OrgModel Org { get; set; }

        [Column("user_id")]
        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public virtual UserModel User { get; set; }

        [Column("create_time")]
        public DateTime CreateTime { get; set; }

        [Column("status")]
        public int Status { get; set; }
    }
}