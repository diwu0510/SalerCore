namespace Zodo.Saler.Services
{
    /// <summary>
    /// 月报告销售服务视图
    /// </summary>
    public class MonthReportSaleServiceViewDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

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
        /// 业绩
        /// </summary>
        public decimal YJ { get; set; }

        /// <summary>
        /// 账面毛利
        /// </summary>
        public decimal ZMML { get; set; }

        /// <summary>
        /// 毛利
        /// </summary>
        //public decimal ML { get; set; }
    }
}
