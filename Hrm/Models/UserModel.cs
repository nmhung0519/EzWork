using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Hrm.Models
{   
    [Table("user")]
    public class UserModel
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("username")]
        public string Username { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("firstname")]
        public string Firstname { get; set; }

        [Column("lastname")]
        public string Lastname { get; set; }

        [Column("sex")]
        public char Sex { get; set; }

        [Column("birthday")]
        public DateTime Birthday { get; set; }

        [Column("phone")]
        public string Phone { get; set; }

        [Column("email")]
        public string Email { get; set; }

        public ICollection<OrgModel> Orgs { get; set; }
        public ICollection<StaffModel> Staffs { get; set; }
        public ICollection<InvitationModel> Invitations { get; set; }

        public UserModel() { }

        public UserModel(RegisterModel model)
        {
            Username = model.Username;
            Password = model.Password1;
            Firstname = model.FirstName;
            Lastname = model.LastName;
            Sex = model.Sex[0];
            Phone = model.Phone;
            Email = model.Email;
            Birthday = model.BirthDay;
        }
        public string GetName()
        {
            return Firstname + " " + Lastname;
        }
    }
}