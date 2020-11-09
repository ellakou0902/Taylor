using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Taylor.Infrastructure.Entity
{
    
    public class JiraTicketResponse
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("fields")]
        public JiraIssueResponse Fields { get; set; }
    }
}
