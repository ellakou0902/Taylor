using System;
using System.Collections.Generic;
using System.Text;
using Taylor.Infrastructure.Entity;

namespace Taylor.Infrastructure
{
    /// <summary>
    /// 配置管理功能
    /// </summary>
    public class ConfigKeeper
    {
        #region 实现单例模式
        public static readonly ConfigKeeper Instance;
        private ConfigKeeper() { }
        static ConfigKeeper()
        {
            Instance = new ConfigKeeper();
            Instance.Testers = LoadTesters();
            Instance.Developers = LoadDevelopers();
            
        }

        private static Dictionary<string, Member> LoadTesters()
        {
            var members = new Dictionary<string, Member>(20);

            //Venus Team
            members.Add("daisyliang", new Member() { Code = "daisyliang", Name = "Daisy.Liang", Type = MemberType.Tester, Team = Team.Test });
            members.Add("annehui", new Member() { Code = "annehui", Name = "Anne.Hui", Type = MemberType.Tester, Team = Team.Test });
            members.Add("amygao", new Member() { Code = "amygao", Name = "Amy.Gao", Type = MemberType.Tester, Team = Team.Test });
            members.Add("derekzhang", new Member() { Code = "derekzhang", Name = "Derek.Zhang", Type = MemberType.Tester, Team = Team.Test });
            members.Add("berryzhao", new Member() { Code = "berryzhao", Name = "Berry.Zhao", Type = MemberType.Tester, Team = Team.Test });
            members.Add("yixue", new Member() { Code = "yixue", Name = "Yi.Xue", Type = MemberType.Tester, Team = Team.Test });
            members.Add("josephzhao", new Member() { Code = "josephzhao", Name = "Joseph.Zhao", Type = MemberType.Tester, Team = Team.Test });

            //Pluto Team
            members.Add("allenli", new Member() { Code = "allenli", Name = "Allen.Li", Type = MemberType.Tester, Team = Team.Test });
            members.Add("mggma", new Member() { Code = "mggma", Name = "Mgg.Ma", Type = MemberType.Tester, Team = Team.Test });
            members.Add("sherrywang", new Member() { Code = "sherrywang", Name = "Sherry.Wang", Type = MemberType.Tester, Team = Team.Test });
            members.Add("mandyma", new Member() { Code = "mandyma", Name = "Mandy.Ma", Type = MemberType.Tester, Team = Team.Test });

            //Uranus Team
            members.Add("Amberhuo", new Member() { Code = "Amberhuo", Name = "Amber.Huo", Type = MemberType.Tester, Team = Team.Test });
            members.Add("jiarryjia", new Member() { Code = "jiarryjia", Name = "Jiarry.Jia", Type = MemberType.Tester, Team = Team.Test });
            members.Add("xuszhang", new Member() { Code = "xuszhang", Name = "Xus.Zhang", Type = MemberType.Tester, Team = Team.Test });
            members.Add("keyleenie", new Member() { Code = "keyleenie", Name = "Keylee.Nie", Type = MemberType.Tester, Team = Team.Test });
            members.Add("ailyxia", new Member() { Code = "ailyxia", Name = "Aily.Xia", Type = MemberType.Tester, Team = Team.Test });
            members.Add("ellakou", new Member() { Code = "ellakou", Name = "Ella.Kou", Type = MemberType.Tester, Team = Team.Test });

            return members;
        }

        private static Dictionary<string, Member> LoadDevelopers()
        {
            var members = new Dictionary<string, Member>(20);

            //WMS
            members.Add("andygu", new Member() { Code = "andygu", Name = "Andy.Gu", Type = MemberType.Developer, Team = Team.WMS });
            members.Add("archerjiang", new Member() { Code = "archerjiang", Name = "Archer.Jiang", Type = MemberType.Developer, Team = Team.WMS });
            members.Add("brandonqu", new Member() { Code = "brandonqu", Name = "Brandon.Qu", Type = MemberType.Developer, Team = Team.WMS });
            members.Add("ericyuan", new Member() { Code = "	ericyuan", Name = "Eric.Yuan", Type = MemberType.Developer, Team = Team.WMS });
            members.Add("leoyang", new Member() { Code = "leoyang", Name = "Leo.Yang", Type = MemberType.Developer, Team = Team.WMS });
            members.Add("neiltian", new Member() { Code = "neiltian", Name = "Neil.Tian", Type = MemberType.Developer, Team = Team.WMS });
            members.Add("viczhang", new Member() { Code = "viczhang", Name = "Vic.Zhang", Type = MemberType.Developer, Team = Team.WMS });
            members.Add("vitosu", new Member() { Code = "vitosu", Name = "Vito.Su", Type = MemberType.Developer, Team = Team.WMS });
            members.Add("aaronlu", new Member() { Code = "aaronlu", Name = "Aaron.Lu", Type = MemberType.Developer, Team = Team.WMS });
            members.Add("nickguo", new Member() { Code = "nickguo", Name = "Nick.Guo", Type = MemberType.Developer, Team = Team.WMS });


            //TMS
            members.Add("blackjin", new Member() { Code = "blackjin", Name = "Black.Jin", Type = MemberType.Developer, Team = Team.TMS });
            members.Add("celichen", new Member() { Code = "celichen", Name = "Celi.Chen", Type = MemberType.Developer, Team = Team.TMS });
            members.Add("jacklee", new Member() { Code = "jacklee", Name = "Jack.Li", Type = MemberType.Developer, Team = Team.TMS });
            members.Add("monatian", new Member() { Code = "monatian", Name = "Mona.Tian", Type = MemberType.Developer, Team = Team.TMS });
            members.Add("krisdeng", new Member() { Code = "krisdeng", Name = "Kris.Deng", Type = MemberType.Developer, Team = Team.TMS });
            
            members.Add("payneqin", new Member() { Code = "payneqin", Name = "Payne.Qin", Type = MemberType.Developer, Team = Team.TMS });
            members.Add("rickerzhang", new Member() { Code = "rickerzhang", Name = "Ricker.Zhang", Type = MemberType.Developer, Team = Team.TMS });
            members.Add("timothyyang", new Member() { Code = "timothyyang", Name = "Timothy.Yang", Type = MemberType.Developer, Team = Team.TMS });
            members.Add("jhonwang", new Member() { Code = "jhonwang", Name = "Jhon.Wang", Type = MemberType.Developer, Team = Team.TMS });
            members.Add("tguo", new Member() { Code = "tguo", Name = "T.Guo", Type = MemberType.Developer, Team = Team.TMS });


            //OMS & AMS
            members.Add("tomzhang", new Member() { Code = "tomzhang", Name = "Tom.Zhang", Type = MemberType.Developer, Team = Team.OMS_AMS });
            members.Add("hermanjin", new Member() { Code = "hermanjin", Name = "Herman.Jin", Type = MemberType.Developer, Team = Team.OMS_AMS });
            return members;
        }


        #endregion


        #region 配置对象
        public Dictionary<string, Member> Testers { get; protected set; }
        public Dictionary<string, Member> Developers { get; protected set; }
        #endregion
    }
}
