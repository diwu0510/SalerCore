using HZC.Database;

namespace HZC.Common.Services
{
    public class AppUserSearchParam : ISearchParam
    {
        public string Key { get; set; }

        public string Role { get; set; }

        public MySearchUtil ToSearchUtil()
        {
            var util = MySearchUtil.New().AndEqual("IsDel", false).AndNotEqual("Name", "admin").OrderBy("Id");

            if (!string.IsNullOrWhiteSpace(Key))
            {
                util.AndContains("Name", Key.Trim());
            }

            if (!string.IsNullOrWhiteSpace(Role))
            {
                util.AndEqual("Role", Role.Trim());
            }

            return util;
        }
    }
}
