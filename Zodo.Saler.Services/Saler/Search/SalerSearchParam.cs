using HZC.Common.Services;
using HZC.Database;

namespace Zodo.Saler.Services
{
    public partial class SalerSearchParam : ISearchParam
    {
        /// <summary>
        /// 
        /// </summary>
        public string EmployeeNumber { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        public string Job { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public int? DeptId { get; set; }

        public MySearchUtil ToSearchUtil()
        {
            MySearchUtil util = MySearchUtil.New().OrderByDesc("Id");

            if (!string.IsNullOrWhiteSpace(Key))
            {
                util.AndContains("Name", Key.Trim());
            }

            if (!string.IsNullOrWhiteSpace(EmployeeNumber))
            {
                util.AndContains("EmployeeNumber", EmployeeNumber.Trim());
            }

            if (!string.IsNullOrWhiteSpace(Job))
            {
                util.AndEqual("Job", Job.Trim());
            }

            if (DeptId.HasValue)
            {
                util.AndEqual("DeptId", DeptId.Value);
            }

            return util;
        }

        protected string GetFullColumnName(string columnName)
        {
            return "Base_Saler." + columnName;
        }
    }
}
