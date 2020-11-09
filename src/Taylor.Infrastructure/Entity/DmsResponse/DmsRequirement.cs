using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Taylor.Infrastructure.Entity.DmsResponse
{
    public class DmsRequirement
    {
        [JsonProperty("idx")]
        public string SysNo { get; set; }
        [JsonProperty("req_id")]
        public string Id { get; set; }
        [JsonProperty("req_title")]
        public string Title { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        [JsonProperty("project_id")]
        public string ProjectId { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("req_priority")]
        public string Priority { get; set; }

        /// <summary>
        /// 需求提出人
        /// </summary>
        [JsonProperty("submit_by")]
        public string SubmitBy { get; set; }

        [JsonProperty("created_by")]
        public string CreatedBy { get; set; }

        [JsonProperty("check_by")]
        public string CheckBy { get; set; }

        [JsonProperty("dev_confirm_by")]
        public string DevConfirmBy { get; set; }

        /// <summary>
        /// 开发人
        /// </summary>
        [JsonProperty("dev_manager")]
        public string DevManager { get; set; }

        [JsonProperty("test_confirm_by")]
        public string TestConfirmBy { get; set; }

        [JsonProperty("req_type")]
        public string ReqType { get; set; }

        [JsonProperty("created_date")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("expect_end_date")]
        public DateTime ExpectEndDate { get; set; }

        [JsonProperty("check_date")]
        public string CheckDate { get; set; }
        /// <summary>
        /// 是否测试
        /// </summary>
        [JsonProperty("is_need_test")]
        public string IsNeedTest { get; set; }

        [JsonProperty("submit_date")]
        public DateTime SubmitDate { get; set; }
    }
}
