using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hrm.Models
{
    public class WorkBodyTableModel
    {
        public int Count { get; set; }
        public List<AssignWorkInfoModel> ListItem { get; set; }

        public WorkBodyTableModel() { }

        public WorkBodyTableModel(int count, List<AssignWorkInfoModel> listitem)
        {
            Count = count;
            ListItem = listitem;
        }
    }
}