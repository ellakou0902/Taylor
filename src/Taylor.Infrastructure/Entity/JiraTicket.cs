using System;
using System.Collections.Generic;
using System.Text;

namespace Taylor.Infrastructure.Entity
{
    /// <summary>
    /// Jira ticket entity
    /// </summary>
    public class JiraTicket
    {
        public string Key { get; set; }
        public string Assigner { get; set; }
        
        public string Tester { get; set; }
        public string Developer { get; set; }
        public decimal StoryPoint { get; set; }
        public string Summary { get; set; }
        public string Status { get; set; }
        public string DmsId { get; set; }

        public string EpicId { get; set; }
        public string DefectBy { get; set; }

        public int PriorityLevel { get; set; }
        public string PriorityName { get; set; }


        public JiraTicket() { }
        public JiraTicket(JiraTicketResponse response)
        {
            Key = response.Key;
            Tester = response.Fields.Tester;
            Developer = response.Fields.Developer;
            StoryPoint = response.Fields.StoryPoint;
            Summary = response.Fields.Summary;
            Status = response.Fields.StatusName;
            DmsId = response.Fields.DmsId;
            EpicId = response.Fields.EpicId;
            DefectBy = response.Fields.DefectBy;
            PriorityLevel = response.Fields.PriorityLevel;
            PriorityName = response.Fields.PriorityName;
            Assigner = response.Fields.Assigner;

        }
    }
}
