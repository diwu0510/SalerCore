using HZC.Common.Services;
using HZC.Database;
using System;

namespace Zodo.Saler.Services
{
    public partial class DeptSalerSearchParam : ISearchParam
    {
        public int? SalerId { get; set; }

        public int? DeptId { get; set; }

        public MySearchUtil ToSearchUtil()
        {
            MySearchUtil util = MySearchUtil.New().OrderByDesc("Id");

            if (SalerId.HasValue)
            {
                util.AndEqual("SalerId", SalerId.Value);
            }

            if (DeptId.HasValue)
            {
                util.AndEqual("DeptId", DeptId.Value);
            }

            return util;
        }

        protected string GetFullColumnName(string columnName)
        {
            return "Base_DeptSaler." + columnName;
        }
    }
}
