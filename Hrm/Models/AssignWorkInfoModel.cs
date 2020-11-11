using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hrm.Models
{
    public class AssignWorkInfoModel
    {
        public int ID { get; set; }

        public int AssignID { get; set; }
        public string AssignName { get; set; }

        public int AssignToID { get; set; }

        public string AssignToName { get; set; } 

        public string WorkTitle { get; set; }

        public string WorkContent { get; set; }

        public DateTime AssignTime { get; set; }

        public DateTime CompleteTime { get; set; }

        public DateTime Deadline { get; set; }
        public int Status { get; set; }

        public AssignWorkInfoModel() { }

        public AssignWorkInfoModel(int id, int assignid, string assignname, int assigntoid, string assigntoname, string worktitle, string workcontent, DateTime assigntime, DateTime completetime, DateTime deadline, int status)
        {
            ID = id;
            AssignID = assignid;
            AssignName = assignname;
            AssignToID = assigntoid;
            AssignToName = assigntoname;
            WorkTitle = worktitle;
            WorkContent = workcontent;
            AssignTime = assigntime;
            CompleteTime = completetime;
            Deadline = deadline;
            Status = status;
        }

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
}