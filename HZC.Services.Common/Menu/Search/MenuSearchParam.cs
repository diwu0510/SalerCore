using HZC.Database;

namespace HZC.Common.Services
{
    public class MenuSearchParam : ISearchParam
    {
        public string Role { get; set; }

        public MySearchUtil ToSearchUtil()
        {
            var util = MySearchUtil.New().AndEqual("IsDel", false);

            if (!string.IsNullOrWhiteSpace(Role))
            {
                util.AndContains("Roles", Role.Trim());
            }

            return util;
        }
    }
}
