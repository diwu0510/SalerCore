using HZC.Database;

namespace HZC.Common.Services
{
    public class DataItemSearchParam : ISearchParam
    {
        public string Key { get; set; }

        public string Cate { get; set; }

        public MySearchUtil ToSearchUtil()
        {
            var util = MySearchUtil.New()
                .AndEqual("IsDel", false).OrderByDesc("Id");

            if (!string.IsNullOrWhiteSpace(Key))
            {
                util.AndContains(new string[] { "K", "Describe" }, Key.Trim());
            }

            if (!string.IsNullOrWhiteSpace(Cate))
            {
                util.AndEqual("Cate", Cate.Trim());
            }

            return util;
        }
    }
}
