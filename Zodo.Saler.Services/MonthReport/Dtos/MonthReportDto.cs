using System;

namespace Zodo.Saler.Services
{
    /// <summary>
    /// 月度报告视图DTO
    /// </summary>
    public partial class MonthReportDto
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
        /// 半年，1上半年|2下半年
        /// </summary>
        public int HalfYear { get; set; }

        /// <summary>
        /// 季度
        /// </summary>
        public int Quarter { get; set; }

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

        /// <summary>
        /// 薪资
        /// </summary>
        public decimal XZ { get; set; }

        /// <summary>
        /// 业绩
        /// </summary>
        public decimal YJ { get; set; }

        /// <summary>
        /// 服务费
        /// </summary>
        public decimal FWF { get; set; }

        /// <summary>
        /// 账面毛利
        /// </summary>
        public decimal ZMML { get; set; }

        /// <summary>
        /// 毛利
        /// </summary>
        public decimal ML { get; set; }

        /// <summary>
        /// 毛利率
        /// </summary>
        public decimal MLL
        {
            get
            {
                if (YJ == 0)
                {
                    return 0;
                }
                return ZMML / YJ;
            }
        }

        /// <summary>
        /// 销售费用占业绩
        /// </summary>
        public decimal XSFYZYJ
        {
            get
            {
                if (YJ == 0)
                {
                    return 0;
                }
                return HJ / YJ;
            }
        }

        /// <summary>
        /// 合计
        /// </summary>
        public decimal HJ
        {
            get; set;
        }

        /// <summary>
        /// 效益
        /// </summary>
        public decimal XY
        {
            get; set;
        }

        /// <summary>
        /// 上一个月的业绩
        /// </summary>
        public decimal LastMonthXY { get; set; }

        /// <summary>
        /// 上一年度当月的效益
        /// </summary>
        public decimal LastYearXY { get; set; }
    }
}
