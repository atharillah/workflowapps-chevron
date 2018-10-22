
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkFlowAppsChevron.Models;
using Newtonsoft.Json;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;



namespace WorkFlowAppsChevron.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult AdminDashboard()
        {
            var segmentTemp = (User_Table)Session["userRoleSession"];
            if (segmentTemp.role == "Admin")
            {
                return View();
            }
            else
            {
                return RedirectToAction("LoginPage", "Login");
            }
        }

        public ActionResult AdminWorkflow()
        {
            var segmentTemp = (User_Table)Session["userRoleSession"];
            if (segmentTemp.role == "Admin")
            {
                return View();
            }
            else
            {
                return RedirectToAction("LoginPage", "Login");
            }
        }

        public ActionResult AdminUser()
        {
            var segmentTemp = (User_Table)Session["userRoleSession"];
            if (segmentTemp.role == "Admin")
            {
                return View();
            }
            else
            {
                return RedirectToAction("LoginPage", "Login");
            }
        }

        public ActionResult Workflow_Data([DataSourceRequest]DataSourceRequest request)
        {
            List<NodeDataArray_Table> result = new List<NodeDataArray_Table>();
            DB_WorkflowEntities db = new DB_WorkflowEntities();
            var workflow = db.NodeDataArray_Table.ToArray();

            for (int i = 0; i < db.NodeDataArray_Table.Count(); i++)
            {
                result.Add(workflow.ElementAt(i));
            }
            return Json(result.ToDataSourceResult(request));
        }
        
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditingInline_Create([DataSourceRequest] DataSourceRequest request, NodeDataArray_Table product)
        {
            if (product != null && ModelState.IsValid)
            {
            }

            return Json(new[] { product }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditingInline_Update([DataSourceRequest] DataSourceRequest request, NodeDataArray_Table product)
        {
            if (product != null && ModelState.IsValid)
            {
            }

            return Json(new[] { product }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditingInline_Destroy([DataSourceRequest] DataSourceRequest request, NodeDataArray_Table product)
            {
            if (product != null)
            {
            }

            return Json(new[] { product }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult User_Data([DataSourceRequest]DataSourceRequest request)
        {
            List<User_Table> result = new List<User_Table>();
            DB_WorkflowEntities db = new DB_WorkflowEntities();

            var adminSegment = db.User_Table.ToArray();

            for (int i = 0; i < db.User_Table.Count(); i++)
            {
                result.Add(adminSegment.ElementAt(i));
            }
            return Json(result.ToDataSourceResult(request));
        }
    }
}