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

        public int? Year { get; set; }

        public int? Month { get; set; }

        public int? Quarter { get; set; }

        public MySearchUtil ToSearchUtil()
        {
            MySearchUtil util = MySearchUtil.New().OrderBy("Year DESC, Month DESC, Sort, SalerName");

            if (!string.IsNullOrWhiteSpace(Key))
            {
                util.AndContains(new string[] { "SalerName", "DeptName" }, Key.Trim());
            }

            if (SalerId.HasValue)
            {
                util.AndEqual("SalerId", SalerId.Value);
            }

            if (DeptId.HasValue)
            {
                util.AndEqual("DeptId", DeptId.Value);
            }

            if (Year.HasValue)
            {
                util.AndEqual("Year", Year.Value);
            }

            if (Month.HasValue)
            {
                util.AndEqual("Month", Month.Value);
            }

            if (Quarter.HasValue)
            {
                util.AndEqual("Quarter", Quarter.Value);
            }

            return util;
        }

        protected string GetFullColumnName(string columnName)
        {
            return "Base_MonthReport." + columnName;
        }
    }
}
