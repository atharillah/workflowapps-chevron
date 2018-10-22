
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkFlowAppsChevron.Models;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace WorkFlowAppsChevron.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult LoginPage()
        {
            System.Web.HttpContext.Current.Session["userRoleSession"] = "";
            return View();
        }
        [HttpPost]
        public ActionResult Authorize(WorkFlowAppsChevron.Models.User_Table userModel_)
        {
            using (DB_WorkflowEntities db = new DB_WorkflowEntities())
            {
                var segment = db.User_Table.Where(x => x.user_name == userModel_.user_name && (x.role == "Admin" || x.role == "WM" || x.role == "PIC")).FirstOrDefault();

                if (segment.role.ToString() == "Admin")
                {
                    System.Web.HttpContext.Current.Session["userRoleSession"] = segment;
                    return RedirectToAction("AdminDashboard", "Admin");
                }
                else if (segment.role.ToString() == "WM")
                {
                    System.Web.HttpContext.Current.Session["userRoleSession"] = segment;
                    return RedirectToAction("WMDashboard", "WorkflowManager");
                }
                else if (segment.role.ToString() == "PIC")
                {
                    System.Web.HttpContext.Current.Session["userRoleSession"] = segment;
                    return RedirectToAction("PICDashboard", "PersonInCharge");
                }
                else
                {
                    return RedirectToAction("LoginPage", "Login");
                }
            }
        }

    }
}

