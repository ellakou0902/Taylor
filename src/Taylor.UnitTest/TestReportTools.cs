using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Taylor.Core.DapperExtensions;
using Taylor.Infrastructure;
using Taylor.Infrastructure.Entity;
using Xunit;

namespace Taylor.UnitTest.Infrastructure
{
    /// <summary>
    /// 测试报告工具
    /// </summary>
    public class TestReportTools
    {
        private string outputPath { get; set; }
        private string fixVersions { get; set; }
        public TestReportTools()
        {
            outputPath = @"E:\@TestData";
            fixVersions = "'WMS Sprint 119','OMS Sprint 119','TMS Sprint 119'";
        }

        [Fact(DisplayName = "生成需求列表")]
        public void GenerateRequirementList()
        {
            /*
             * 查询当前Sprint 需求并导出             
             */
            var jiraClient = new JiraClient(fixVersions);
            var tickets = jiraClient.SearchTicketsForCurrentRelease();
            var epics = jiraClient.SearchAllEpics();

            var sb = new StringBuilder("需求编号,状态,标题,开发人员,测试人员,Epic,EpicId\n\t");

            foreach (var ticket in tickets)
            {
                var epic = epics.FirstOrDefault(x => x.Key == ticket.EpicId);
                var epicKey = (epic == null) ? "N/A" : epic.Key;
                var epicName = (epic == null) ? "N/A" : epic.Summary;

                sb.Append($"{ticket.DmsId},{ticket.Status},{ticket.Summary.Replace(",", " ")},{ticket.Developer},{ticket.Tester},{epicName},{epicKey}\n\t");
            }

            File.WriteAllText($@"{outputPath}\需求列表-{DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss")}.csv", sb.ToString(), Encoding.UTF8);

        }

        [Fact(DisplayName = "生成测试用例列表")]
        public void GenerateTestCaseList()
        {
            #region 获取需求
            //查询当前Sprint 需求
            var requirements = new List<string>();
            requirements.AddRange(new string[] { "RQ180803013" });
            //var jiraClient = new JiraClient(fixVersions);
            //var tickets = jiraClient.SearchTicketsForCurrentRelease();
            //var requirements = tickets.Select(x => x.DmsId);
            #endregion 获取需求

            #region 获取对应测试用例
            //获取对应测试用例
            var testCaseRepository = new DapperRepositoryBase<TestCase>();
            var testCases = new List<TestCase>(100);

            var allTestCases = testCaseRepository.GetAll().ToList();
            foreach (var req in requirements)
            {
                testCases.AddRange(allTestCases.Where(x => x.RequirementId == req));
            }

            testCases.ForEach(x => x.Description = NoHTML(x.Description));
            #endregion 获取对应测试用例

            //生成报告
            var sb = new StringBuilder("用例编号,标题,描述,需求编号,测试人员,测试结果\n\t");

            foreach (var tc in testCases)
            {
                //sb.Append($"{tc.CaseId},{tc.Title.Replace(",", "，")},{tc.Description},{tc.RequirementId},{tc.CreatedBy},\"测试通过\"\n\t");
                sb.Append($"{tc.CaseId},{tc.Title},\"{tc.Description}\",{tc.RequirementId},{tc.CreatedBy},\"测试通过\"\n\t");
            }

            File.WriteAllText($@"{outputPath}\测试用例-{DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss")}.csv", sb.ToString(), Encoding.UTF8);
        }


        [Fact(DisplayName = "生成Bug分析报告")]
        public void GenerateBugAnalysisReport()
        {
            /*
             *      待办 | 处理中 | 开发完成 | 测试中 | 完成
             *  P1
             *  P2
             *  P3
             *  P4
             *  P5
             */
            var jiraClient = new JiraClient(fixVersions);
            var bugs = jiraClient.SearchSubBugsForCurrentSprint();


            var sbSummay = new StringBuilder("Bug级别,待办,处理中,开发完成,测试中,完成\n\t");


            for (var level = 1; level <= 5; level++)
            {
                var countToDo = bugs.Count(x => x.PriorityLevel == level && x.Status == "待办");
                var countInProcess = bugs.Count(x => x.PriorityLevel == level && x.Status == "处理中");
                var countDevComplete = bugs.Count(x => x.PriorityLevel == level && x.Status == "开发完成");
                var countTesting = bugs.Count(x => x.PriorityLevel == level && x.Status == "测试中");
                var countDone = bugs.Count(x => x.PriorityLevel == level && x.Status == "完成");
                sbSummay.Append($"P{ level },{ countToDo },{ countInProcess },{ countDevComplete },{ countTesting },{ countDone }\n\t");
            }
            File.WriteAllText($@"{outputPath}\Bug分析列表-{DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss")}.csv", sbSummay.ToString(), Encoding.UTF8);

            //P1&P2 Bug 列表
            var bugsForHighLevel = bugs.Where(x => x.PriorityLevel == 1 || x.PriorityLevel == 2).OrderBy(x => x.PriorityLevel);

            var sbBugList = new StringBuilder("Bug编号,标题,状态,级别,Bug产生人,经办人,开发人员,测试人员\n\t");
            foreach (var bug in bugsForHighLevel)
            {
                sbBugList.Append($"{ bug.Key },{ bug.Summary.Replace(",", " ") },{ bug.Status },P{ bug.PriorityLevel },{ bug.DefectBy },{ bug.Assigner },{ bug.Developer },{ bug.Tester }\n\t");
            }
            File.WriteAllText($@"{outputPath}\High Level - Bug列表-{DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss")}.csv", sbBugList.ToString(), Encoding.UTF8);

        }
         

        [Fact(DisplayName = "同步DMS ID")]
        public void SynchronizeDmsId()
        {
            /*
             * 同步DMS ID
             */
            var client = new JiraClient();
            var tickets = client.SearchTicketsForDmsIdIsNull();
            for (var i = 0; i < tickets.Count; i++)
            {
                var dmsId = GetAndMapDmsId(tickets[i].Summary);
                client.EditTaskDmsId(tickets[i].Key, dmsId);
            }
        }

        private string GetAndMapDmsId(string jiraSummay)
        {
            string pattern = @"RQ[0-9]{8,10}";
            var matchs = Regex.Matches(jiraSummay, pattern);
            var str = matchs.Count > 0 ? matchs[0].Value : "N/A";
            return str;
        }

        private string NoHTML(string htmlString)  //替换HTML标记
        {
            //删除脚本
            htmlString = Regex.Replace(htmlString, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);

            //删除HTML
            htmlString = Regex.Replace(htmlString, @"</(.[^>]*)>", "\r\n", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            //Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"-->", "", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"<!--.*", "", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"<img[^>]*>;", "", RegexOptions.IgnoreCase);

            htmlString = htmlString.Replace(",", "，");
            htmlString = htmlString.Replace("\"", "'");
            //htmlString = htmlString.Replace("\r\n", "\r");

            while (htmlString.IndexOf("\r\n\r\n") > 1)
            {
                htmlString = htmlString.Replace("\r\n\r\n", "\r\n");
            }

            return htmlString;
        }
    }
}
