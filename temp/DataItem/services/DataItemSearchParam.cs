using HZC.Database;
using System;

namespace Zodo.Saler.Services
{
    public partial class DataItemSearchParam : ISearchParam
    {
        public string Key { get; set; }

        public string K { get; set; }

        public bool? IsDel { get; set; }

        public override MySearchUtil ToSearchUtil()
        {
            MySearchUtil util = MySearchUtil.New().OrderByDesc("Id");

            if (!string.IsNullOrWhiteSpace(Key))
            {
                // util.AndContains(new string[] { "Title", "Name" }, Key.Trim());
            }

            if (!string.IsNullOrWhiteSpace(K))
            {
                util.AndContains("K", K.Trim());
            }

            if (IsDel.HasValue)
            {
                util.AndEqual("IsDel", IsDel.Value);
            }

            return util;
        }

        protected override string GetFullColumnName(string columnName)
        {
            return "Base_DataItem." + columnName;
        }
    }
}
