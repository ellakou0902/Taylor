using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Taylor.Infrastructure.Entity
{
    public class JiraIssueResponse
    {
        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("assignee")]
        public dynamic OrgAssigner { get; set; }

        [JsonProperty("status")]
        public dynamic Status { get; set; }

        [JsonProperty("customfield_10302")]
        public dynamic Customfield_10302 { get; set; }

        [JsonProperty("customfield_10301")]
        public dynamic Customfield_10301 { get; set; }

        [JsonProperty("customfield_10106")]
        public dynamic Customfield_10106 { get; set; }


        [JsonProperty("customfield_11101")]
        public string DmsId { get; set; }

        [JsonProperty("customfield_10101")]
        public string EpicId { get; set; }

        /// <summary>
        /// Bug 产生人
        /// </summary>
        [JsonProperty("customfield_11000")]
        public dynamic Customfield_11000 { get; set; }
        

        [JsonProperty("labels")]
        public string[] Labels { get; set; }

        [JsonProperty("priority")]
        public dynamic Priority { get; set; }

        [JsonIgnore]
        public string Assigner
        {
            get
            {
                if (OrgAssigner == null || OrgAssigner.name == null) { return string.Empty; }
                return OrgAssigner.name;
            }
        }

        [JsonProperty("reporter")]
        public dynamic OrgReporter { get; set; }

        [JsonIgnore]
        public string Reporter
        {
            get
            {
                if (OrgReporter == null || OrgReporter.name == null) { return string.Empty; }
                return OrgReporter.name;
            }
        }

        /// <summary>
        /// 测试人员
        /// </summary>
        [JsonIgnore]
        public string Tester
        {
            get
            {
                if (Customfield_10302 == null || Customfield_10302.name == null) { return string.Empty; }
                return Customfield_10302.name;
            }
        }
        /// <summary>
        /// 开发人员
        /// </summary>
        [JsonIgnore]
        public string Developer
        {
            get
            {
                if (Customfield_10301 == null || Customfield_10301.name == null) { return string.Empty; }
                return Customfield_10301.name;
            }
        }

        /// <summary>
        /// Bug 产生人
        /// </summary>
        [JsonIgnore]
        public string DefectBy
        {
            get
            {
                if (Customfield_11000 == null || Customfield_11000.name == null) { return string.Empty; }
                return Customfield_11000.name;
            }
        }

        [JsonIgnore]
        public int PriorityLevel
        {
            get
            {
                return (int)Priority.id;
            }
        }
        [JsonIgnore]
        public string PriorityName
        {
            get
            {
                return Priority.name;
            }
        }


        /// <summary>
        /// Story Point
        /// </summary>
        [JsonIgnore]
        public decimal StoryPoint
        {
            get
            {
                return (decimal)(Customfield_10106 ?? 0);
            }
        }
        
        [JsonIgnore]
        public string StatusName
        {
            get
            {
                if (Status == null || Status.name == null) { return string.Empty; }
                return Status.name;
            }
        }
    }
}
