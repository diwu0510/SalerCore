using HZC.Common.Services;
using HZC.Database;
using System;

namespace Zodo.Saler.Services
{
    public partial class DeptSalerSearchParam : ISearchParam
    {
        public string Key { get; set; }

        public MySearchUtil ToSearchUtil()
        {
            MySearchUtil util = MySearchUtil.New().OrderByDesc("Id");

            if (!string.IsNullOrWhiteSpace(Key))
            {
                // util.AndContains(new string[] { "Title", "Name" }, Key.Trim());
            }

            return util;
        }

        protected string GetFullColumnName(string columnName)
        {
            return "Base_DeptSaler." + columnName;
        }
    }
}
