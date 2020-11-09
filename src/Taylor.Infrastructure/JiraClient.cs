using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Taylor.Infrastructure.Entity;
using System.Net;
using System.Text.RegularExpressions;

namespace Taylor.Infrastructure
{
    /// <summary>
    /// Client for jira
    /// </summary>
    public class JiraClient
    {
        private string _baseUrl;
        private string _searchUrl;
        private string _account;
        private string _fixVersions;
        private RestClient _client;
        public JiraClient()
        {
            //TODO: move to config
            _baseUrl = "http://192.168.100.128:8080";
            _searchUrl = "rest/api/2/search";
            _account = "ZWxsYWtvdTprb3VqaWU1Njg1NDM";
            _fixVersions = "'WMS Sprint 126','OMS Sprint 126','TMS Sprint 126'";
            _client = new RestClient(_baseUrl);
        }
        public JiraClient(string fixVersions) : this()
        {
            _fixVersions = fixVersions;
        }

        private List<JiraTicket> SearchTickets(string jql, int index = 0)
        {
            var body = new
            {
                jql = jql,
                startAt = index * 1000,
                maxResults = 1000,
                fields = new string[] {
                    "key",
                    "summary",
                    "description",
                    "assignee",
                    "reporter",
                    "status",
                    "customfield_10301",    //Dev
                    "customfield_10302",    //Tester
                    "customfield_10106",    //Story Point
                    "customfield_11101",    //Dms Id
                    "customfield_10101",    //Epic Id
                    "customfield_11000",    //Bug 产生人
                    "labels",
                    "priority"
                }
            };

            var request = InitRequest(_searchUrl);
            request.AddJsonBody(body);

            // execute the request
            var response = _client.Execute(request);
            var jsonObject = (JObject)JsonConvert.DeserializeObject(response.Content);
            string datas = jsonObject["issues"].ToString();
            var jiraTickets = JsonConvert.DeserializeObject<List<JiraTicketResponse>>(datas);

            var tickets = new List<JiraTicket>();
            jiraTickets.ForEach(x => tickets.Add(new JiraTicket(x)));

            return tickets;
        }

        private List<JiraTicket> AmendData(List<JiraTicket> tickets)
        {
            //修正WMS6.5数据 参数35%
            foreach (var ticket in tickets)
            {
                if (ticket.Summary.StartsWith("[WMS6.5]"))
                {
                    // 0.32 ， 0.55
                    ticket.StoryPoint = ticket.StoryPoint * 0.80m;
                }
            }
            return tickets;
        }

        private RestRequest InitRequest(string uri, Method method = Method.POST)
        {
            var request = new RestRequest(uri, method);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Basic {_account}");
            return request;
        }


        public List<JiraTicket> SearchTicketsForCurrentRelease(string userName, bool isTester = true)
        {
            var field = isTester ? "测试人员" : "开发人员";
            var issueType = isTester ? "Bug（缺陷）,Enhancement（优化）,Story（故事）" : "Bug（缺陷）,Enhancement（优化）,Story（故事）,Task（任务）";
            var jql = $"{field} = {userName} AND Sprint in openSprints() AND issuetype in ({issueType}) AND fixVersion  in({_fixVersions})";
            var tickets = SearchTickets(jql);
            return AmendData(tickets);
        }

        public List<JiraTicket> SearchTicketsForCurrentRelease(bool isTester = true)
        {
            var issueType = isTester ? "Bug（缺陷）,Enhancement（优化）,Story（故事）" : "Bug（缺陷）,Enhancement（优化）,Story（故事）,Task（任务）";
            var jql = $"Sprint in openSprints() AND issuetype in ({issueType}) AND fixVersion  in({_fixVersions})";
            var tickets = SearchTickets(jql);
            tickets = tickets.Where(x => (!string.IsNullOrEmpty(x.DmsId) && IsDmsTask(x.DmsId))).ToList();
            return tickets;
        }
        public List<JiraTicket> SearchAllEpics()
        {
            var jql = $"project in(OMS,WMS,TMS,Framework) AND issuetype = Epic（史诗）";
            var tickets = SearchTickets(jql);
            return tickets;
        }

        public List<JiraTicket> SearchSubBugsForCurrentSprint()
        {
            var jql = $"project in(OMS,WMS,TMS,Framework) AND issuetype = Sub-Bug（缺陷） AND Sprint in openSprints()";
            var tickets = SearchTickets(jql);
            return tickets;
        }
        public List<JiraTicket> SearchTicketsForDone(string userName, bool isTester = true)
        {
            var field = isTester ? "测试人员" : "开发人员";
            var issueType = isTester ? "Bug（缺陷）,Enhancement（优化）,Story（故事）" : "Bug（缺陷）,Enhancement（优化）,Story（故事）,Task（任务）";
            var jql = $"{field} = {userName} AND Sprint in openSprints() AND issuetype in ({issueType}) AND status = Done AND fixVersion in({_fixVersions})";
            var tickets = SearchTickets(jql);
            return AmendData(tickets);
        }

        public List<JiraTicket> SearchTickets(string[] reqIds)
        {
            var sbReqIds = new StringBuilder();

            for (var i = 0; i < reqIds.Length; i++)
            {
                sbReqIds.Append($"'{reqIds[i]}'");
                if (i != reqIds.Length - 1)
                {
                    sbReqIds.Append(",");
                }
            }

            var jql = $"'Dms ID' in {sbReqIds.ToString()}";
            var tickets = SearchTickets(jql);
            return tickets;
        }


        public List<JiraTicket> SearchTicketsForDmsIdIsNull()
        {
            var results = new List<JiraTicket>(5000);
            var jql = $"issuetype in( Bug（缺陷）,Story（故事）,Enhancement（优化）,Task（任务）) AND project in (OMS,TMS,WMS,Framework)";
            for (var i = 0; i < 5; i++)
            {
                var tickets = SearchTickets(jql, i);
                tickets = tickets.Where(x => string.IsNullOrEmpty(x.DmsId)).ToList();
                results.AddRange(tickets);
            }
            
            return results;
        }
        public void EditTaskDmsId(string issueKey,string dmsId)
        {
            var body = new
            {
                fields = new
                {
                    customfield_11101 = dmsId
                }
            };
            var request = InitRequest($"/rest/api/2/issue/{issueKey}", Method.PUT);
            request.AddJsonBody(body);          
            
            // execute the request
            var response = _client.Execute(request);
            //var jsonObject = (JObject)JsonConvert.DeserializeObject(response.Content);
            //string datas = jsonObject["issues"].ToString();
            //var jiraTickets = JsonConvert.DeserializeObject<List<JiraTicketResponse>>(datas);

            if (!response.StatusCode.HasFlag(HttpStatusCode.NoContent))
            {
                throw new Exception("编辑失败");
            }
        }


        private bool IsDmsTask(string jiraSummay)
        {
            string pattern = @"RQ[0-9]{8,10}";
            return Regex.IsMatch(jiraSummay, pattern);
        }



        /*******************/

        public List<JiraTicket> SearchTicketsStroyForTime(string start = "2019-05-17", string end = "2019-10-25")
        {
            
            var jql = $"createdDate >=  '{start}' AND createdDate <=  '{end}' AND 测试人员 is not Empty AND issuetype in(Story（故事）) AND  status = Done";

            var tickets = new List<JiraTicket>();
            var tempTickets = new List<JiraTicket>();

            var index = 0;
            do
            {
                tempTickets = SearchTickets(jql, index++);
                tickets.AddRange(tempTickets);

            } while (tempTickets != null && tempTickets.Count != 0);


            return tickets;
        }

        public List<JiraTicket> SearchTicketsBugByDesignForTime(string start = "2019-05-17", string end = "2019-10-25")
        {

            var jql = $"createdDate >=  '{start}' AND createdDate <=  '{end}' AND 测试人员 is not Empty AND issuetype in(Sub-Bug（缺陷）) AND  status = Done AND Bug产生原因 = '设计如此（By Design）' ";

            var tickets = new List<JiraTicket>();
            var tempTickets = new List<JiraTicket>();

            var index = 0;
            do
            {
                tempTickets = SearchTickets(jql, index++);
                tickets.AddRange(tempTickets);

            } while (tempTickets != null && tempTickets.Count != 0);


            return tickets;
        }
        public List<JiraTicket> SearchTicketsBugCodeErrorForTime(string start = "2019-05-17", string end = "2019-10-25")
        {

            var jql = $"createdDate >=  '{start}' AND createdDate <=  '{end}' AND 测试人员 is not Empty AND issuetype in(Sub-Bug（缺陷）) AND  status = Done AND Bug产生原因 != '设计如此（By Design）'  ";

            var tickets = new List<JiraTicket>();
            var tempTickets = new List<JiraTicket>();

            var index = 0;
            do
            {
                tempTickets = SearchTickets(jql, index++);
                tickets.AddRange(tempTickets);

            } while (tempTickets != null && tempTickets.Count != 0);


            return tickets;
        }
    }
}
