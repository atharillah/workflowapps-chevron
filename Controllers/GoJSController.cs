using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WorkFlowAppsChevron.Models;

namespace WorkFlowAppsChevron.Controllers
{
    public class GoJSController : Controller
    {
        // GET: GoJS
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveJSON(Process_Table processData)
        {
            // Initiate and Construction
            NodeDataArray_Table nodetable1 = new NodeDataArray_Table();
            NodeDataArray_Table nodetable2 = new NodeDataArray_Table();
            LinkDataArray_Table linktable1 = new LinkDataArray_Table();
            Workflow_Table wmtable1 = new Workflow_Table();

            DB_WorkflowEntities db = new DB_WorkflowEntities();

            List<NodeDataArray> nodeListTemp = new List<NodeDataArray>();
            List<LinkDataArray> linkListTemp = new List<LinkDataArray>();

            nodeListTemp = processData.NodeDataArray.ToList();
            linkListTemp = processData.LinkDataArray.ToList();

            //Make sure every item in Node Data Array Have a same perspective with table in Database
            var count = 0;
            foreach (var item in nodeListTemp)
            {
                // Initiate variable in order to make a conclusion of statement
                nodetable1 = db.NodeDataArray_Table.Where(x => x.key == item.Key && x.workflowName == item.WorkflowName).FirstOrDefault();
                var check = 0;
                // Condition whether to add or overwrite
                if (nodetable1 != null)
                {
                    check = 1;
                }
                else
                {
                    nodetable1 = new NodeDataArray_Table();
                }
                // make sure the length of input is recording to database
                var checkWM = db.Workflow_Table.Where(x => x.workflow_name == item.WorkflowName && x.workflow_pic == item.Pic && x.workflow_subject == item.SubjectProcess).FirstOrDefault();

                nodetable1.processName = item.processName;
                nodetable1.subjectProcess = item.SubjectProcess;
                nodetable1.statusNode = item.StatusNode;
                nodetable1.workflowName = item.WorkflowName;
                nodetable1.loc = item.Loc;
                nodetable1.pic = item.Pic;
                nodetable1.startProcess = item.StartProcess;
                nodetable1.dueProcess = item.DueProcess;
                nodetable1.cycleProcess = item.CycleProcess;
                nodetable1.comment = item.Comment;
                nodetable1.key = item.Key;
                wmtable1.workflow_name = item.WorkflowName;
                wmtable1.workflow_pic = item.Pic;
                wmtable1.workflow_subject = item.SubjectProcess;

                count++;
                System.Diagnostics.Debug.WriteLine("COUNTING LOOP FOREACH");
                System.Diagnostics.Debug.WriteLine(count);

                // Condition and Saving into SQL Server
                if (check == 1 && checkWM != null)
                {
                    db.SaveChanges();
                }
                else
                {
                    if (nodeListTemp.Count == count)
                    {
                        db.Workflow_Table.Add(wmtable1);
                    }
                    db.NodeDataArray_Table.Add(nodetable1);
                    db.SaveChanges();
                }
            }

            //CASE FOR LINK DATA ARRAY
            //Make sure every item in Link Data Array Have a same perspective with table in Database
            foreach (var item2 in linkListTemp)
            {
                //var checking = new NodeDataArray();
                linktable1 = db.LinkDataArray_Table.Where(a => a.from == item2.From && a.to == item2.To && a.workflowName == nodetable1.workflowName).FirstOrDefault();
                var linkcheck = 0;

                // Condition whether to add or overwrite
                if (linktable1 != null)
                {
                    linkcheck = 1;
                }
                else
                {
                    linktable1 = new LinkDataArray_Table();
                }

                linktable1.workflowName = nodetable1.workflowName;
                linktable1.to = item2.To;
                linktable1.from = item2.From;
                linktable1.fromPort = item2.FromPort;
                linktable1.toPort = item2.ToPort;
                linktable1.points = String.Join("/", item2.Points);

                // Saving into SQL Server
                if (linkcheck == 1)
                {
                    db.SaveChanges();
                }
                else
                {
                    db.LinkDataArray_Table.Add(linktable1);
                    db.SaveChanges();
                }
            }
            return null;
        }

        [HttpPost]
        public ActionResult ImplementSaveJSON(Process_Table processData)
        {
            // Initiate and Construction
            DateTime today = DateTime.Today;
            NodeDataArray_Table nodetable1 = new NodeDataArray_Table();
            NodeDataArray_Table nodetable2 = new NodeDataArray_Table();
            LinkDataArray_Table linktable1 = new LinkDataArray_Table();
            Workflow_Table wmtable1 = new Workflow_Table();

            DB_WorkflowEntities db = new DB_WorkflowEntities();

            List<NodeDataArray> nodeListTemp = new List<NodeDataArray>();
            List<LinkDataArray> linkListTemp = new List<LinkDataArray>();

            nodeListTemp = processData.NodeDataArray.ToList();
            linkListTemp = processData.LinkDataArray.ToList();

            //Make sure every item in Node Data Array Have a same perspective with table in Database
            var count = 0;
            var segmentTemp = (User_Table)Session["userRoleSession"];

            foreach (var item in nodeListTemp)
            {
                // Initiate variable in order to make a conclusion of statement
                nodetable1 = db.NodeDataArray_Table.Where(x => x.key == item.Key && x.workflowName == item.WorkflowName).FirstOrDefault();

                var check = 0;

                // Condition whether to add or overwrite
                if (nodetable1 != null)
                {
                    check = 1;
                }
                else
                {
                    nodetable1 = new NodeDataArray_Table();
                }

                // make sure the length of input is recording to database
                var checkWM = db.Workflow_Table.Where(x => x.workflow_name == item.WorkflowName && x.workflow_pic == item.Pic && x.workflow_subject == item.SubjectProcess).FirstOrDefault();

                nodetable1.processName = item.processName;
                nodetable1.subjectProcess = item.SubjectProcess;
                nodetable1.statusNode = item.StatusNode;
                nodetable1.workflowName = item.WorkflowName;
                nodetable1.loc = item.Loc;
                nodetable1.pic = item.Pic;
                nodetable1.startProcess = item.StartProcess;
                nodetable1.dueProcess = item.DueProcess;
                nodetable1.cycleProcess = item.CycleProcess;
                if (item.Key == -1)
                {
                    nodetable1.startProcess = today.ToString("dd/MM/yyyy");
                }
                nodetable1.comment = item.Comment;
                nodetable1.key = item.Key;
                wmtable1.workflow_name = item.WorkflowName;
                wmtable1.workflow_pic = item.Pic;
                wmtable1.workflow_subject = item.SubjectProcess;

                count++;
                System.Diagnostics.Debug.WriteLine("COUNTING LOOP FOREACH");
                System.Diagnostics.Debug.WriteLine(count);

                // Condition and Saving into SQL Server
                if (check == 1 && checkWM != null)
                {
                    db.SaveChanges();
                }
                else
                {
                    if (nodeListTemp.Count == count)
                    {
                        wmtable1.workflow_wm = segmentTemp.user_name;
                        db.Workflow_Table.Add(wmtable1);
                    }
                    nodetable1.wm = segmentTemp.user_name;
                    db.NodeDataArray_Table.Add(nodetable1);
                    db.SaveChanges();
                }
            }

            //CASE FOR LINK DATA ARRAY
            //Make sure every item in Link Data Array Have a same perspective with table in Database
            //var segmentTemp = (User_Table)Session["userRoleSession"];
            foreach (var item2 in linkListTemp)
            {
                linktable1 = db.LinkDataArray_Table.Where(a => a.from == item2.From && a.to == item2.To && a.workflowName == nodetable1.workflowName).FirstOrDefault();
                var linkcheck = 0;

                // Condition whether to add or overwrite
                if (linktable1 != null)
                {
                    linkcheck = 1;
                }
                else
                {
                    linktable1 = new LinkDataArray_Table();
                }

                linktable1.workflowName = nodetable1.workflowName;
                linktable1.to = item2.To;
                linktable1.from = item2.From;
                linktable1.fromPort = item2.FromPort;
                linktable1.toPort = item2.ToPort;
                linktable1.points = String.Join("/", item2.Points);
                linktable1.wm = segmentTemp.user_name;

                // Saving into SQL Server
                if (linkcheck == 1)
                {
                    db.SaveChanges();
                }
                else
                {
                    db.LinkDataArray_Table.Add(linktable1);
                    db.SaveChanges();
                }
            }
            return null;
        }
        // SAVE JSON IMPLEMENT FROM WM
    }
}