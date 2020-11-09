using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Taylor.Infrastructure.Entity.DmsResponse;

namespace Taylor.Infrastructure
{
    public class DmsClient
    {

        private string _baseUrl;
        //private RestClient _client;

        public DmsClient()
        {
            _baseUrl = "http://192.168.100.111:8000/dms.api/";
        }
        private string GetToken()
        {
            var client = new RestClient(_baseUrl);
            var requestPost = new RestRequest("/api/Token?userid=jirabot&password=123qwe", Method.GET);

            var response = client.Execute(requestPost);//发送请求
            var contents = response.Content;
            var jsonObject = (JObject)JsonConvert.DeserializeObject(contents);
            var token = jsonObject["access_token"].ToString();
            return token;
        }


        #region Actions
        public List<DmsRequirement> SearchDmsRequirements(List<SearchParameter> filters, DateTime? startTime = null)
        {
            startTime = startTime ?? DateTime.Parse("2018-05-01");
            var token = GetToken();
            var client = new RestClient(_baseUrl);
            var requestPost = new RestRequest("/api/v_dev_req_query_list/PagedList", Method.POST);
            requestPost.AddHeader("auth", token);
            requestPost.AddHeader("Content-Type", "application/json; charset=UTF-8");

            requestPost.AddJsonBody(new
            {
                filterRules = filters,
                page = 1,
                rows = 999,
                sort = "req_id",
                order = "desc"
            });

            var response = client.Execute(requestPost);
            if (!response.StatusCode.HasFlag(HttpStatusCode.OK))
            {
                throw new Exception("DMS查询失败");
            }

            string contents = response.Content;
            var jsonObject = (JObject)JsonConvert.DeserializeObject(contents);
            string datas = jsonObject["rows"].ToString();
            var objs = JsonConvert.DeserializeObject<List<DmsRequirement>>(datas);

            var queryDatas = objs.Where(x => x.SubmitDate <= DateTime.Now && x.SubmitDate >= startTime.Value).ToList();
            return queryDatas;
        }

        #endregion
    }
}
