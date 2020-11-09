using System;
using System.Collections.Generic;
using System.Text;
using Taylor.Infrastructure;
using Xunit;
using System.Linq;
using Taylor.Infrastructure.Entity;
using System.IO;

namespace Taylor.UnitTest.Infrastructure
{
    public class JiraClient_Test
    {
        [Fact(DisplayName = "Tester OKR")]
        public void SearchTesterReport_Test()
        {
            var client = new JiraClient();
            var members = ConfigKeeper.Instance.Testers;
            var sb = new StringBuilder("Name,Done,Total,Percent\n\t");

            foreach (var (key, member) in members)
            {
                List<JiraTicket> tickets = null;

                tickets = client.SearchTicketsForCurrentRelease(key);
                var spTotal = tickets.Sum(x => x.StoryPoint);

                tickets = client.SearchTicketsForDone(key);
                var spDone = tickets.Sum(x => x.StoryPoint);
                var percent = string.Empty;
                if (spTotal > 0)
                {
                    percent = (spDone / spTotal).ToString("#0.##%");
                }
                sb.Append($"{key},{spDone},{spTotal},{percent}\n\t");
            }

            File.WriteAllText($@"E:\@TestData-{DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss")}.csv", sb.ToString(), Encoding.UTF8);
        }

        [Fact]
        public void SearchDeveloperReport_Test()
        {
            var client = new JiraClient();
            var members = ConfigKeeper.Instance.Developers;
            var sb = new StringBuilder("Team,Name,Done,Total,Percent\n\t");

            foreach (var (key, member) in members)
            {
                List<JiraTicket> tickets = null;

                tickets = client.SearchTicketsForCurrentRelease(key, false);
                var spTotal = tickets.Sum(x => x.StoryPoint);

                tickets = client.SearchTicketsForDone(key, false);
                var spDone = tickets.Sum(x => x.StoryPoint);
                var percent = string.Empty;
                if (spTotal > 0)
                {
                    percent = (spDone / spTotal).ToString("#0.##%");
                }
                sb.Append($"{member.Team},{key},{spDone},{spTotal},{percent}\n\t");
            }

            File.WriteAllText($@"E:\@TestData-{DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss")}.csv", sb.ToString(), Encoding.UTF8);
        }


        [Fact(DisplayName = "Tester OKR -New")]
        public void SearchTesterReportNew_Test()
        {
            var client = new JiraClient();
            var members = ConfigKeeper.Instance.Testers;
            var sb = new StringBuilder("Name,StroyPoint,Bug-Error,Bug-Design\n\t");
            var start = "2019-07-17";
            var end = "2019-10-25";
            var stories = client.SearchTicketsStroyForTime(start, end);
            var bugsByDesign = client.SearchTicketsBugByDesignForTime(start, end);
            var bugsCodeError = client.SearchTicketsBugCodeErrorForTime(start, end);

            foreach (var (name, m) in members)
            {
                var tempTickets = stories.Where(x => x.Tester == name);
                var tempBugCodeErrors = bugsCodeError.Where(x => x.Tester == name);
                var tempBugByDesigns = bugsByDesign.Where(x => x.Tester == name);

                var storyPoint = tempTickets.Sum(x => x.StoryPoint);

                var bugCodeErrorCount = tempBugCodeErrors.Count();
                var bugByDesignCount = tempBugByDesigns.Count();
                

                sb.Append($"{name},{storyPoint},{bugCodeErrorCount},{bugByDesignCount}\n\t");
            }




            File.WriteAllText($@"E:\@TestData-{DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss")}.csv", sb.ToString(), Encoding.UTF8);
        }

    }
}
