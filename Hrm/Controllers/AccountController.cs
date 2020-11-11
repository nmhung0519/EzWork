using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hrm.Models;
using Hrm.Helpers;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Hrm.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Login()
        {
            if (Session["UserID"] == null) return View();
            else return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid) return View(model);
            else
            {
                using (var db = new DBContext())
                {
                    var user = await (from u in db.User
                                      where u.Username == model.Username
                                      select u).FirstOrDefaultAsync();
                    if (user != null && user.Password == model.Password)
                    {
                        Session["userid"] = user.ID;
                        Session["fullname"] = user.GetName();
                        return RedirectToAction("SelectOrg");
                    }
                    return View(model);
                }
            }
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View(new RegisterModel());
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            using (var db = new DBContext())
            {
                var user = await (from u in db.User
                                  where u.Username == model.Username
                                  select u).FirstOrDefaultAsync();
                if (user != null) { return View(model); }
                else
                {
                    UserModel newUser = new UserModel(model);
                    db.User.Add(newUser);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Login");
                }
            }
        }

        [HttpGet]
        public async Task<ActionResult> SelectOrg()
        {
            if (Session["userid"] != null)
            {
                using (var db = new DBContext())
                {
                    int userid = int.Parse(Session["userid"].ToString());
                    var staffs = await (from s in db.Staff
                                       where (s.UserID == userid)
                                       select s).ToListAsync();
                    List<KeyNamePair> ListOrg = new List<KeyNamePair>();
                    foreach (var item in staffs) {
                        await (db.Entry(item)
                            .Reference(x => x.Org)
                            .LoadAsync());
                        ListOrg.Add(new KeyNamePair(item.Org.ID, item.Org.Name));
                    }
                    return View(ListOrg);
                }
            }
            else return RedirectToAction("Login", "Account");
        }

        public ActionResult Login2(int id)
        {
            Session["orgid"] = id;
            using (var db = new DBContext())
            {
                int userid = int.Parse(Session["userid"].ToString());
                var staff = from s in db.Staff
                            where s.UserID == userid && s.OrgID == id
                            select s;
                var org = from o in db.Org
                          where o.ID == id
                          select o;
                Session["staffid"] = staff.FirstOrDefault().ID;
                Session["orgname"] = org.FirstOrDefault().Name;
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            Session["userid"] = null;
            Session["fullname"] = null;
            Session["orgid"] = null;
            Session["staffid"] = null;
            return RedirectToAction("Login");
        }


        [HttpGet]
        public ActionResult OrgInvitationNotice()
        {
            int userid = int.Parse(Session["userid"].ToString());
            using (var db = new DBContext())
            {
                var invitation = (from i in db.Invitation
                                  where i.UserID == userid
                                  select i).ToList();
                foreach (var item in invitation)
                {
                    db.Entry(item)
                        .Reference(x => x.Org)
                        .Load();
                }
                return View(invitation);
            }
        }
    }
}