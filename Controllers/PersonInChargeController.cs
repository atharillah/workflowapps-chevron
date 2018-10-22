using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using WorkFlowAppsChevron.Models;

using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace WorkFlowAppsChevron.Controllers
{
    public class PersonInChargeController : Controller
    {
        // GET: PersonInCharge
        public ActionResult PICDashboard()
        {
            var segmentTemp = (User_Table)Session["userRoleSession"];
            if (segmentTemp.role == "PIC")
            {
                return View();
            }
            else
            {
                return RedirectToAction("LoginPage", "Login");
            }
        }

        public ActionResult Complete(int id)
        {
            bool res = false;
            DateTime today = DateTime.Today;

            List<NodeDataArray_Table> result = new List<NodeDataArray_Table>();
            DB_WorkflowEntities db = new DB_WorkflowEntities();
            var workflow = db.NodeDataArray_Table.ToArray();
            var link = db.LinkDataArray_Table.ToArray();
            var usr = db.User_Table.ToArray();
            var temp = new Workflow_Table();
            for (int i = 0; i < db.NodeDataArray_Table.Count(); i++)
            {
                if (workflow.ElementAt(i).id == id)
                {
                    workflow.ElementAt(i).statusNode = "Done";
                    workflow.ElementAt(i).dueProcess = today.ToString("dd/MM/yyyy");
                    for (int j = 0; j < db.LinkDataArray_Table.Count(); j++)
                    {
                        if (link.ElementAt(j).from == workflow.ElementAt(i).key)
                        {
                            for (int k = 0; k < db.NodeDataArray_Table.Count(); k++)
                            {
                                if (workflow.ElementAt(k).key == link.ElementAt(j).to && string.Equals(workflow.ElementAt(k).pic, workflow.ElementAt(k).pic) && workflow.ElementAt(k).pic != null)
                                {
                                    workflow.ElementAt(k).startProcess = today.ToString("dd/MM/yyyy");
                                    db.SaveChanges();
                                    for (int l = 0; l < db.User_Table.Count(); l++)
                                    {
                                        if (string.Equals(workflow.ElementAt(k).pic, usr.ElementAt(l).full_name))
                                        {
                                            res = SendEmail(usr.ElementAt(l).email, "test", "<p>Hi,<br />We like to inform you that your process in Workflow Apps has already started.<br />Regards, Workflow Apps</p>");
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                        db.SaveChanges();
                    }
                }
            }
            return RedirectToAction("PICDashboard");
        }

        public bool SendEmail(string toEmail, string subject, string emailBody)
        {
            try
            {
                string senderEmail = System.Configuration.ConfigurationManager.AppSettings["SenderEmail"].ToString();
                string senderPassword = System.Configuration.ConfigurationManager.AppSettings["SenderPassword"].ToString();

                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Timeout = 100000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(senderEmail, senderPassword);

                MailMessage mailMessage = new MailMessage(senderEmail, toEmail, subject, emailBody);
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = UTF8Encoding.UTF8;
                client.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public ActionResult Workflow_Data([DataSourceRequest]DataSourceRequest request)
        {
            List<NodeDataArray_Table> result = new List<NodeDataArray_Table>();
            DB_WorkflowEntities db = new DB_WorkflowEntities();
            var workflow = db.NodeDataArray_Table.ToArray();
            var temp = new Workflow_Table();
            for (int i = 0; i < db.NodeDataArray_Table.Count(); i++)
            {
                var segmentTemp = (User_Table)Session["userRoleSession"];
                if (string.Equals(workflow.ElementAt(i).pic, segmentTemp.full_name) && workflow.ElementAt(i).startProcess != null && string.Equals(workflow.ElementAt(i).statusNode, "Not Done"))
                {
                    result.Add(workflow.ElementAt(i));
                }
            }
            return Json(result.ToDataSourceResult(request));
        }
    }
}