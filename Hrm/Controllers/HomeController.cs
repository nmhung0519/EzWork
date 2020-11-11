using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Data.Entity;
using Hrm.Helpers;
using Hrm.Models;
using System.Web.DynamicData;
using Microsoft.Ajax.Utilities;
using System.Security.Cryptography.X509Certificates;

namespace Hrm.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["UserID"] != null) return RedirectToAction("Work");
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Work()
        {
            return View();
        }

        public ActionResult WorkAll(string page)
        {
            int pageIndex = 1;
            if (page != null)
            {
                pageIndex = int.Parse(page);
            }
            else { page = "1"; }
            if (Session["userid"] == null || Session["orgid"] == null) return RedirectToAction("Login", "Account");
            using (var db = new DBContext())
            {
                var model = db.GetAssignWorkInfoList(int.Parse(Session["staffid"].ToString()), "all", "", "", "", "");
                if (pageIndex * 10 - 1 > model.Count)
                {
                    return PartialView("WorkTable", new WorkBodyTableModel(model.Count, model.GetRange((pageIndex - 1) * 10, model.Count - ((pageIndex - 1) * 10))));
                }
                return PartialView("WorkTable", new WorkBodyTableModel(model.Count, model.GetRange((pageIndex - 1) * 10, 10)));
            }
        }

        public ActionResult WorkAssign(string page)
        {
            int pageIndex = 1;
            if (page != null)
            {
                pageIndex = int.Parse(page);
            }
            else { page = "1"; }
            if (Session["userid"] == null || Session["orgid"] == null) return RedirectToAction("Login", "Account");
            using (var db = new DBContext())
            {
                var model = db.GetAssignWorkInfoList(int.Parse(Session["staffid"].ToString()), "assign", "", "", "", "");
                if (pageIndex * 10 - 1 > model.Count)
                {
                    return PartialView("WorkTable", new WorkBodyTableModel(model.Count, model.GetRange((pageIndex - 1) * 10, model.Count - ((pageIndex - 1) * 10))));
                }
                return PartialView("WorkTable", new WorkBodyTableModel(model.Count, model.GetRange((pageIndex - 1) * 10, 10)));
            }
        }

        public ActionResult WorkAssignTo(string page)
        {
            int pageIndex = 1;
            if (page != null)
            {
                pageIndex = int.Parse(page);
            }
            else { page = "1"; }
            if (Session["userid"] == null || Session["orgid"] == null) return RedirectToAction("Login", "Account");
            using (var db = new DBContext())
            {
                var model = db.GetAssignWorkInfoList(int.Parse(Session["staffid"].ToString()), "assignto", "", "", "", "");
                if (pageIndex * 10 - 1 > model.Count)
                {
                    return PartialView("WorkTable", new WorkBodyTableModel(model.Count, model.GetRange((pageIndex - 1) * 10, model.Count - ((pageIndex - 1) * 10))));
                }
                return PartialView("WorkTable", new WorkBodyTableModel(model.Count, model.GetRange((pageIndex - 1) * 10, 10)));
            }
        }

        public ActionResult WorkDetail(int id)
        {
            if (Session["userid"] == null || Session["orgid"] == null) return RedirectToAction("Login", "Account");
            using (var db = new DBContext())
            {
                var model = db.GetAssignWorkInfo(id);
                return View(model);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateWork(NewWorkModel model)
        {
            if (!ModelState.IsValid) {
                model.OrgID = int.Parse(Session["orgid"].ToString());
                model.StaffID = int.Parse(Session["orgid"].ToString());
                using (var db = new DBContext())
                {
                    var staffs = await (from s in db.Staff
                                        where s.OrgID == model.OrgID && s.ID != model.StaffID
                                        select s).ToListAsync();
                    foreach (var item in staffs)
                    {
                        await (db.Entry(item)
                            .Reference(x => x.User)
                            .LoadAsync());
                    }
                    model.Staffs = staffs;
                }
                return View("NewWork", model);
            }
            WorkModel newwork = new WorkModel();

            AssignWorkModel newassignworkmodel = new AssignWorkModel();
            try
            {
                using (var db = new DBContext())
                {
                    model.OrgID = int.Parse(Session["orgid"].ToString());
                    model.Assignee = int.Parse(Session["staffid"].ToString());
                    var work = await (from w in db.Work
                                      where w.OrgID == model.OrgID
                                      orderby w.ID descending
                                      select w).FirstOrDefaultAsync();
                    newwork.OrgID = model.OrgID;
                    newwork.CreaterID = model.Assignee;
                    newwork.Title = model.Title;
                    newwork.Content = model.Content;
                    newwork.CreateTime = DateTime.Now;
                    db.Work.Add(newwork);
                    newassignworkmodel.AssignID = model.Assignee;
                    newassignworkmodel.AssignTime = DateTime.Now;
                    newassignworkmodel.AssignToID = model.AssignTo;
                    newassignworkmodel.Deadline = model.Deadline;
                    newassignworkmodel.OrgID = model.OrgID;
                    newwork.AssignWorks = new List<AssignWorkModel>();
                    newwork.AssignWorks.Add(newassignworkmodel);
                    db.AssignWork.Add(newassignworkmodel);
                    await db.SaveChangesAsync();
                }
                return Content("<script>window.parent.location.reload()</script>");
            }
            catch(Exception e) { return Json(e); }
        }

        public async Task<ActionResult> NewWork()
        {
            var model = new NewWorkModel();
            model.OrgID = int.Parse(Session["orgid"].ToString());
            model.StaffID = int.Parse(Session["orgid"].ToString());
            using (var db = new DBContext())
            {
                var staffs = await (from s in db.Staff
                                    where s.OrgID == model.OrgID && s.ID != model.StaffID
                                    select s).ToListAsync();
                foreach (var item in staffs)
                {
                    await (db.Entry(item)
                        .Reference(x => x.User)
                        .LoadAsync());
                }
                model.Staffs = staffs;
            }
            return View(model);
        }

        public ActionResult WorkList(string page, string collection, string title, string assignee, string performer, string status)
        {
            int pageIndex = int.Parse(page);
            using (var db = new DBContext())
            {
                var model = db.GetAssignWorkInfoList(int.Parse(Session["staffid"].ToString()), collection, title, assignee, performer, status);
                if (pageIndex * 10 - 1 > model.Count)
                {
                    return PartialView("WorkTableBody", new WorkBodyTableModel(model.Count, model.GetRange((pageIndex - 1) * 10, model.Count - ((pageIndex - 1) * 10))));
                }
                return PartialView("WorkTableBody", new WorkBodyTableModel(model.Count, model.GetRange((pageIndex - 1) * 10, 10)));
            }
        }

        public async Task<JsonResult> ChangeStatus(int id, int status)
        {
            using (var db = new DBContext())
            {
                int orgid = int.Parse(Session["orgid"].ToString());
                int staffid = int.Parse(Session["staffid"].ToString());
                var assignwork = await (from aw in db.AssignWork
                                        where aw.ID == id
                                        select aw).FirstOrDefaultAsync();
                if (orgid != assignwork.OrgID) return null;
                if (status == 1)
                {
                    if (assignwork.AssignToID == staffid)
                    {
                        assignwork.Status = 1;
                        await db.SaveChangesAsync();
                        return Json("Success");
                    }
                }
                else if (status == 2)
                {
                    if (assignwork.AssignToID == staffid)
                    {
                        assignwork.Status = 2;
                        assignwork.CompleteTime = DateTime.Now;
                        await db.SaveChangesAsync();
                        return Json("Success");
                    }
                }
                else if (status == -1)
                {
                    if (assignwork.AssignToID == staffid)
                    {
                        assignwork.Status = -1;
                        assignwork.CompleteTime = DateTime.Now;
                        await db.SaveChangesAsync();
                        return Json("Success");
                    }
                }
                else if (status == -2)
                {
                    if (assignwork.AssignID == staffid)
                    {
                        assignwork.Status = -2;
                        assignwork.CompleteTime = DateTime.Now;
                        await db.SaveChangesAsync();
                        return Json("Success");
                    }
                }
                return null;
            }
        }
    }
}