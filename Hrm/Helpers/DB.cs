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

        public List<AssignWorkInfoModel> GetAssignWorkInfoList(int page, int staffid, string collection, string title, string assignee, string performer, string status)
        {
            var conn = new MySqlConnection();
            conn.ConnectionString = "server=localhost;port=3306;database=hrm;uid=root;password=root;Convert Zero Datetime=True";
            var cmd = new MySqlCommand();
            if (title == null) title = "";
            if (assignee == null) assignee = "";
            if (performer == null) performer = "";
            if (status == null) status = "";
            try
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "get_assignwork_list";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@staffid", staffid);
                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@collection", collection);
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@assignee", assignee);
                cmd.Parameters.AddWithValue("@performer", performer);
                MySqlDataReader reader = cmd.ExecuteReader();
                List<AssignWorkInfoModel> result = new List<AssignWorkInfoModel>();
                DataTable dt = new DataTable();
                dt.Load(reader);
                var count = dt.Rows.Count;
                List<AssignWorkInfoModel> model = new List<AssignWorkInfoModel>();
                for (int i = 0; i < 10 && i < count; i++)
                {
                    var item = dt.Rows[i].ItemArray;
                    model.Add(new AssignWorkInfoModel(int.Parse(item[0].ToString()), int.Parse(item[1].ToString()), item[2].ToString(), int.Parse(item[3].ToString()), item[4].ToString(), item[5].ToString(), item[6].ToString(), (DateTime)item[7], (DateTime)item[8], (DateTime)item[9], int.Parse(item[10].ToString())));
                } 
                conn.Close();
                return model;
            }
            catch (Exception) { return null; }
        }

        public int CountAssignWork(int staffid, string collection, string title, string assignee, string performer, string status)
        {
            var conn = new MySqlConnection();
            conn.ConnectionString = "server=localhost;port=3306;database=hrm;uid=root;password=root;Convert Zero Datetime=True";
            conn.Open();
            var cmd = new MySqlCommand();
            var cmd2 = new MySqlCommand();
            if (title == null) title = "";
            if (assignee == null) assignee = "";
            if (performer == null) performer = "";
            if (status == null) status = "";
            try
            {
                cmd2.Connection = conn;
                cmd2.CommandText = "count_assignwork";
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@staffid", staffid);
                cmd2.Parameters.AddWithValue("@collection", collection);
                cmd2.Parameters.AddWithValue("@title", title);
                cmd2.Parameters.AddWithValue("@assignee", assignee);
                cmd2.Parameters.AddWithValue("@performer", performer);
                cmd2.Parameters.AddWithValue("?coun", MySqlDbType.Int32);
                cmd2.Parameters["?coun"].Direction = ParameterDirection.Output;
                cmd2.ExecuteNonQuery();
                conn.Close();
                return int.Parse(cmd2.Parameters["?coun"].Value.ToString());
            }
            catch (Exception )
            {
                conn.Close();
                return -1;
            }
        }
    }

}