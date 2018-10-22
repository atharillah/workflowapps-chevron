using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorkFlowAppsChevron.Models
{
    public class Process_Table
    {
        public string Class { get; set; }
        public string LinkFromPortIdProperty { get; set; }
        public string LinkToPortIdProperty { get; set; }
        public List<NodeDataArray> NodeDataArray { get; set; }
        public List<LinkDataArray> LinkDataArray { get; set; }
    }

    public partial class NodeDataArray
    {
        public int id { get; set; }
        public int Key { get; set; }
        public string processName { get; set; }
        public string StatusNode { get; set; }
        public string WorkflowName { get; set; }
        public string Loc { get; set; }
        public string SubjectProcess { get; set; }
        public string Pic { get; set; }
        public string StartProcess { get; set; }
        public string DueProcess { get; set; }
        public string CycleProcess { get; set; }
        public string Comment { get; set; }
    }

    public partial class LinkDataArray
    {
        public string WorkflowName { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public string FromPort { get; set; }
        public string ToPort { get; set; }
        public string[] Points { get; set; }
    }
}