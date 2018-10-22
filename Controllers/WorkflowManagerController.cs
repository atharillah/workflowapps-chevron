using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using WorkFlowAppsChevron.Models;


namespace WorkFlowAppsChevron.Controllers
{
    public class WorkflowManagerController : Controller
    {
        // GET: WorkflowManager
        public ActionResult WMDashboard()
        {
            var segmentTemp = (User_Table)Session["userRoleSession"];
            if (segmentTemp.role == "WM")
            {
                return View();
            }
            else
            {
                return RedirectToAction("LoginPage", "Login");
            }
        }

        public ActionResult WMMyWorkflow()
        {
            var segmentTemp = (User_Table)Session["userRoleSession"];
            if (segmentTemp.role == "WM")
            {
                return View();
            }
            else
            {
                return RedirectToAction("LoginPage", "Login");
            }
        }

        public ActionResult WMReport(String workflowName)
        {
            var segmentTemp = (User_Table)Session["userRoleSession"];
            if (segmentTemp.role == "WM")
            {
                return View();
            }
            else
            {
                return RedirectToAction("LoginPage", "Login");
            }
        }

        public ActionResult Chart_Data([DataSourceRequest]DataSourceRequest request)
        {
            List<NodeDataArray_Table> result = new List<NodeDataArray_Table>();
            DB_WorkflowEntities db = new DB_WorkflowEntities();
            var workflow = db.NodeDataArray_Table.ToArray();
            var compare = db.Workflow_Table.ToArray();

            for (int i = 2; i < db.NodeDataArray_Table.Count(); i++)
            {
                for (int j = 2; j < db.Workflow_Table.Count(); j++)
                {
                    if (string.Equals(compare.ElementAt(j).workflow_name, workflow.ElementAt(i).workflowName) && workflow.ElementAt(i).wm != null)
                    {
                        result.Add(workflow.ElementAt(i));
                        break;
                    }
                }
            }
            return Json(result);
        }

        public ActionResult WMWorkflow(String workflowName)
        {
            var id = 0;
            System.Diagnostics.Debug.WriteLine(workflowName);
            List<NodeDataArray_Table> result = new List<NodeDataArray_Table>();
            DB_WorkflowEntities db = new DB_WorkflowEntities();
            var workflow2 = db.LinkDataArray_Table.ToArray();
            var workflow = db.NodeDataArray_Table.ToArray();
            NodeDataArray_Table coba = new NodeDataArray_Table();

            var temp = new String[10];
            temp[0] = "{ \"class\": \"go.GraphLinksModel\",\"linkFromPortIdProperty\": \"fromPort\",\"linkToPortIdProperty\": \"toPort\",\"nodeDataArray\": [";
            int o = 0;
            var idx = db.NodeDataArray_Table.Where(x => x.id == id).FirstOrDefault();
            var tempName = workflow.ElementAt(o).workflowName;

            for (int i = 0; i < db.NodeDataArray_Table.Count(); i++)
            {
                if (string.Equals(workflow.ElementAt(i).workflowName, workflowName))
                {
                    temp[0] += "{\"processName\": ";
                    temp[0] += "\"" + workflow.ElementAt(i).processName + "\",";
                    temp[0] += "\"subjectProcess\": ";
                    temp[0] += "\"" + workflow.ElementAt(i).subjectProcess + "\",";
                    temp[0] += "\"statusNode\": ";
                    temp[0] += "\"" + workflow.ElementAt(i).statusNode + "\",";
                    temp[0] += "\"workflowName\": ";
                    temp[0] += "\"" + workflow.ElementAt(i).workflowName + "\",";
                    temp[0] += "\"loc\": ";
                    temp[0] += "\"" + workflow.ElementAt(i).loc + "\",";
                    temp[0] += "\"pic\": ";
                    temp[0] += "\"" + workflow.ElementAt(i).pic + "\",";
                    temp[0] += "\"startProcess\": ";
                    temp[0] += "\"" + workflow.ElementAt(i).startProcess + "\",";
                    temp[0] += "\"dueProcess\": ";
                    temp[0] += "\"" + workflow.ElementAt(i).dueProcess + "\",";
                    temp[0] += "\"cycleProcess\": ";
                    temp[0] += "\"" + workflow.ElementAt(i).cycleProcess + "\",";
                    temp[0] += "\"comment\": ";
                    temp[0] += "\"" + workflow.ElementAt(i).comment + "\",";
                    temp[0] += "\"key\": ";
                    temp[0] += "\"" + workflow.ElementAt(i).key + "\"},\n";
                }
            }
            temp[0] = temp[0].Remove(temp[0].Count() - 2);
            temp[0] += "],\n";
            temp[0] += "\"linkDataArray\" : [\n";

            for (int i = 0; i < db.LinkDataArray_Table.Count(); i++)
            {
                if (string.Equals(workflow2.ElementAt(i).workflowName, workflowName))
                {
                    temp[0] += "{\"from\":";
                    temp[0] += "" + workflow2.ElementAt(i).from + ", ";
                    temp[0] += "\"to\":";
                    temp[0] += "" + workflow2.ElementAt(i).to + ", ";
                    temp[0] += "\"fromPort\" :";
                    temp[0] += "\"" + workflow2.ElementAt(i).fromPort + "\", ";
                    temp[0] += "\"toPort\" :";
                    temp[0] += "\"" + workflow2.ElementAt(i).toPort + "\", ";
                    temp[0] += "\"points\" :[";
                    string points = workflow2.ElementAt(i).points;
                    points = points.Replace("/", ",");
                    temp[0] += "" + points + "]},\n";
                }
            }
            temp[0] = temp[0].Remove(temp[0].Count() - 2);
            temp[0] += "]} ";

            ViewData["coba"] = temp[0];
            ViewData["Title"] = workflowName;
            return View();
        }

        public ActionResult ToJson()
        {
            List<NodeDataArray_Table> result = new List<NodeDataArray_Table>();
            DB_WorkflowEntities db = new DB_WorkflowEntities();
            var workflow = db.NodeDataArray_Table.ToArray();
            NodeDataArray_Table coba = new NodeDataArray_Table();

            for (int i = 0; i < db.NodeDataArray_Table.Count(); i++)
            {
                result.Add(workflow.ElementAt(i));
            }
            return View();
        }
        
        public ActionResult Workflow_Data([DataSourceRequest]DataSourceRequest request)
        {
            List<Workflow_Table> result = new List<Workflow_Table>();
            DB_WorkflowEntities db = new DB_WorkflowEntities();
            var workflow = db.Workflow_Table.ToArray();
            var compare = db.NodeDataArray_Table.ToArray();
            var temp = new Workflow_Table();
            var check = true;
            for (int i = 0; i < db.Workflow_Table.Count(); i++)
            {
                for (int j = 0; j < db.NodeDataArray_Table.Count(); j++)
                {
                    if (string.Equals(compare.ElementAt(j).workflowName, workflow.ElementAt(i).workflow_name) && workflow.ElementAt(i).workflow_wm == null)
                    {
                        result.Add(workflow.ElementAt(i));
                        break;
                    }
                }
            }
            return Json(result.ToDataSourceResult(request));
        }

        public ActionResult ImplementWorkflow_Data([DataSourceRequest]DataSourceRequest request)
        {
            List<Workflow_Table> result = new List<Workflow_Table>();
            DB_WorkflowEntities db = new DB_WorkflowEntities();
            var workflow = db.Workflow_Table.ToArray();
            var compare = db.NodeDataArray_Table.ToArray();
            var temp = new Workflow_Table();
            for (int i = 0; i < db.Workflow_Table.Count(); i++)
            {
                for (int j = 0; j < db.NodeDataArray_Table.Count(); j++)
                {
                    if (string.Equals(compare.ElementAt(j).workflowName, workflow.ElementAt(i).workflow_name) && workflow.ElementAt(i).workflow_wm != null)
                    {
                        result.Add(workflow.ElementAt(i));
                        break;
                    }
                }
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