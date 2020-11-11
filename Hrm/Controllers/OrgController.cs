using Hrm.Helpers;
using Hrm.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Hrm.Controllers
{
    public class OrgController : Controller
    {
        // GET: Org
        public async Task<ActionResult> Index()
        {
            using (var db = new DBContext())
            {
                int orgid = int.Parse(Session["orgid"].ToString());
                var org = await(from o in db.Org
                                where o.ID == orgid
                                select o).FirstOrDefaultAsync();
                await (db.Entry(org)
                    .Collection(x => x.Staffs)
                    .LoadAsync());
                foreach(var item in org.Staffs)
                {
                    await (db.Entry(item)
                        .Reference(x => x.User)
                        .LoadAsync());
                }
                return View(org.Staffs.ToList());
            };
        }

        [HttpGet]
        public async Task<ActionResult> AddStaff()
        {
            using (var db = new DBContext())
            {
                int orgid = int.Parse(Session["orgid"].ToString());
                var invitations = await (from i in db.Invitation
                                         where i.OrgID == orgid && i.Status != 1
                                         select i).ToListAsync();
                foreach (var item in invitations)
                {
                    await (db.Entry(item)
                        .Reference(x => x.User)
                        .LoadAsync());
                }
                return View(invitations);
            }
        }
        [HttpPost]
        public async Task<JsonResult> AddStaff(int id)
        {
            using (var db = new DBContext())
            {
                try
                {
                    InvitationModel invitation = new InvitationModel();
                    invitation.OrgID = int.Parse(Session["orgid"].ToString());
                    invitation.CreateTime = DateTime.Now;
                    invitation.Status = 0;
                    invitation.UserID = id;
                    var staff = await (from s in db.Staff
                                       where s.OrgID == invitation.OrgID && s.UserID == invitation.UserID
                                       select s).FirstOrDefaultAsync();
                    if (staff != null) return Json("StfInOrg");
                    var inv = await (from i in db.Invitation
                                     where i.UserID == invitation.UserID && i.OrgID == i.OrgID && i.Status == 0
                                     select i).FirstOrDefaultAsync();
                    if (inv != null) return Json("InvExst");
                    db.Invitation.Add(invitation);
                    await db.SaveChangesAsync();
                }
                catch (Exception e) { return Json(e); }
                return Json("Success");
            }
        }

        [HttpGet]
        public async Task<JsonResult> SearchStaff(string username)
        {
            using (var db = new DBContext())
            {
                int orgid = int.Parse(Session["orgid"].ToString());
                var user = await (from u in db.User
                                  where u.Username == username
                                  select u).FirstOrDefaultAsync();
                if (user != null)
                {
                    var staff = await (from s in db.Staff
                                        where s.OrgID == orgid && s.UserID == user.ID
                                        select s).FirstOrDefaultAsync();
                    if (staff == null) return Json(new KeyNamePair(user.ID, user.GetName()), JsonRequestBehavior.AllowGet);
                }
                return Json("No result", JsonRequestBehavior.AllowGet);
            }
        }
        
        [HttpPost]
        public async Task<JsonResult> Invitation(int id, bool accept)
        {
            try
            {
                if (accept == false)
                {
                    int userid = int.Parse(Session["userid"].ToString());
                    using (var db = new DBContext())
                    {
                        var invitation = await (from i in db.Invitation
                                                where i.ID == id
                                                select i).FirstOrDefaultAsync();
                        if (invitation.UserID == userid)
                        {
                            invitation.Status = -1;
                            await db.SaveChangesAsync();
                            return Json("Success");
                        }
                    }
                }
                else if (accept == true)
                {
                    int userid = int.Parse(Session["userid"].ToString());
                    using (var db = new DBContext())
                    {
                        var invitation = await (from i in db.Invitation
                                                where i.ID == id
                                                select i).FirstOrDefaultAsync();
                        if (invitation.UserID == userid)
                        {
                            invitation.Status = 1;
                            StaffModel staff = new StaffModel();
                            staff.UserID = userid;
                            staff.StaffCode = 1;
                            staff.OrgID = invitation.OrgID;
                            db.Staff.Add(staff);
                            await db.SaveChangesAsync();
                            Session["orgid"] = invitation.OrgID;
                            staff = await (from s in db.Staff
                                           where s.OrgID == invitation.OrgID && s.UserID == userid
                                           select s).FirstOrDefaultAsync();
                            Session["staffid"] = staff.ID;
                            return Json("Success");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return Json(e);
            }
            return null;
        }
    }
}