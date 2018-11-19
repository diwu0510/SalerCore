namespace Zodo.Saler.Services
{
    /// <summary>
    /// 月报告销售管理视图
    /// </summary>
    public class MonthReportSaleManageViewDto
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
        /// 飞机
        /// </summary>
        public decimal FJ { get; set; }

        /// <summary>
        /// 火车
        /// </summary>
        public decimal HC { get; set; }

        /// <summary>
        /// 巴士
        /// </summary>
        public decimal BS { get; set; }

        /// <summary>
        /// 自驾车
        /// </summary>
        public decimal ZJC { get; set; }

        /// <summary>
        /// 的士费
        /// </summary>
        public decimal DSF { get; set; }

        /// <summary>
        /// 业务招待费
        /// </summary>
        public decimal YWZDF { get; set; }

        /// <summary>
        /// 住宿费
        /// </summary>
        public decimal ZSF { get; set; }

        /// <summary>
        /// 住宿节约奖励
        /// </summary>
        public decimal ZSJYJL { get; set; }

        /// <summary>
        /// 招标费用
        /// </summary>
        public decimal ZBFY { get; set; }

        /// <summary>
        /// 出差补贴
        /// </summary>
        public decimal CCBT { get; set; }

        /// <summary>
        /// 通讯费
        /// </summary>
        public decimal TXF { get; set; }

        /// <summary>
        /// 其他费用
        /// </summary>
        public decimal QTFY { get; set; }
    }
}
