using HZC.Common.Services;
using HZC.Database;
using System;

namespace Zodo.Saler.Services
{
    public partial class MonthReportSearchParam : ISearchParam
    {
        public string Key { get; set; }

        public int? SalerId { get; set; }

        public int? DeptId { get; set; }

        public string Year { get; set; }

        public string Month { get; set; }

        public MySearchUtil ToSearchUtil()
        {
            MySearchUtil util = MySearchUtil.New().OrderByDesc("Id");

            if (!string.IsNullOrWhiteSpace(Key))
            {
                // util.AndContains(new string[] { "Title", "Name" }, Key.Trim());
            }

            if (SalerId.HasValue)
            {
                util.AndEqual("SalerId", SalerId.Value);
            }

            if (DeptId.HasValue)
            {
                util.AndEqual("DeptId", DeptId.Value);
            }

            if (!string.IsNullOrWhiteSpace(Year))
            {
                util.AndContains("Year", Year.Trim());
            }

            if (!string.IsNullOrWhiteSpace(Month))
            {
                util.AndContains("Month", Month.Trim());
            }

            return util;
        }

        protected string GetFullColumnName(string columnName)
        {
            return "Base_MonthReport." + columnName;
        }
    }
}
