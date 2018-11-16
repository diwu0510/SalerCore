using HZC.Common.Services;
using HZC.Database;

namespace Zodo.Saler.Services
{
    public partial class DeptSearchParam : ISearchParam
    {
        public string Key { get; set; }

        public MySearchUtil ToSearchUtil()
        {
            MySearchUtil util = MySearchUtil.New().OrderBy("Sort");

            if (!string.IsNullOrWhiteSpace(Key))
            {
                util.AndContains("Name", Key.Trim());
            }

            return util;
        }

        protected string GetFullColumnName(string columnName)
        {
            return "Base_Dept." + columnName;
        }
    }
}
