namespace Zodo.Saler.Services
{
    /// <summary>
    /// 月报告人事部门视图
    /// </summary>
    public class MonthReportPersonnelViewDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 员工编号
        /// </summary>
        public string EmployeeNumber { get; set; } = "";

        /// <summary>
        /// 销售ID
        /// </summary>
        public string SalerName { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public string DeptName { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        public string Job { get; set; }

        /// <summary>
        /// 年度
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 月份
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// 薪资
        /// </summary>
        public decimal XZ { get; set; }
    }
}
