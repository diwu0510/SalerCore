using HZC.Database;
using System;

namespace Zodo.Saler.Services
{
    public partial class DeptSearchParam : ISearchParam
    {
        public string Key { get; set; }

        public override MySearchUtil ToSearchUtil()
        {
            MySearchUtil util = MySearchUtil.New().OrderByDesc("Id");

            if (!string.IsNullOrWhiteSpace(Key))
            {
                // util.AndContains(new string[] { "Title", "Name" }, Key.Trim());
            }

            return util;
        }

        protected override string GetFullColumnName(string columnName)
        {
            return "Base_Dept." + columnName;
        }
    }
}
