using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace Taylor.Infrastructure.Entity
{
    public class JiraSearchResponse
    {
        [JsonProperty("issues")]
        public List<JiraTicketResponse> Tickets { get; set; }
    }
}
