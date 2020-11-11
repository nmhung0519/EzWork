using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Hrm.Models;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data.Entity.Core.Objects;
using System.CodeDom;
using System.Data.Entity.Infrastructure;
using Renci.SshNet.Security.Cryptography;
using System.Data;
using Microsoft.Extensions.Logging.Abstractions;

namespace Hrm.Helpers
{
    public partial class DBContext : DbContext
    {
        public DBContext() : base("name=Default") { }

        public DbSet<UserModel> User { get; set; }

        public DbSet<OrgModel> Org { get; set; }

        public DbSet<WorkModel> Work { get; set; }

        public DbSet<StaffModel> Staff { get; set; }

        public DbSet<AssignWorkModel> AssignWork { get; set; }
        public DbSet<InvitationModel> Invitation { get; set; }

        public AssignWorkInfoModel GetAssignWorkInfo(int staffid)
        {
            var conn = new MySqlConnection();
            conn.ConnectionString = "server=localhost;port=3306;database=hrm;uid=root;password=root;Convert Zero Datetime=True";
            var cmd = new MySqlCommand();
            try
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "get_assignwork_info";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@p_assignwork_id", staffid);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read()) {
                    AssignWorkInfoModel result = new AssignWorkInfoModel(int.Parse(reader[0].ToString()), int.Parse(reader[1].ToString()), reader[2].ToString(), int.Parse(reader[3].ToString()), reader[4].ToString(), reader[5].ToString(), reader[6].ToString(), (DateTime)reader[7], (DateTime)reader[8], (DateTime)reader[9], int.Parse(reader[10].ToString()));
                    conn.Close();
                    return result;
                }
                return null;
            }
            catch (Exception) { return null; }
        }

        public List<AssignWorkInfoModel> GetAssignWorkInfoList(int staffid, string collection, string title, string assignee, string performer, string status)
        {
            var conn = new MySqlConnection();
            conn.ConnectionString = "server=localhost;port=3306;database=hrm;uid=root;password=root;Convert Zero Datetime=True";
            var cmd = new MySqlCommand();
            try
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "get_assignwork_list";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@staffid", staffid);
                cmd.Parameters.AddWithValue("@page", 1);
                cmd.Parameters.AddWithValue("@collection", collection);
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@assignee", assignee);
                cmd.Parameters.AddWithValue("@performer", performer);
                MySqlDataReader reader = cmd.ExecuteReader();
                List<AssignWorkInfoModel> result = new List<AssignWorkInfoModel>();
                while (reader.Read())
                {
                    AssignWorkInfoModel model = new AssignWorkInfoModel(int.Parse(reader[0].ToString()), int.Parse(reader[1].ToString()), reader[2].ToString(), int.Parse(reader[3].ToString()), reader[4].ToString(), reader[5].ToString(), reader[6].ToString(), (DateTime)reader[7], (DateTime)reader[8], (DateTime)reader[9], int.Parse(reader[10].ToString()));
                    result.Add(model);
                }
                conn.Close();
                return result;
            }
            catch (Exception) { return null; }
        }
    }

}