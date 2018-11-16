using System;

namespace Zodo.Saler.Application
{
    public partial class MonthReportDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 销售ID
        /// </summary>
        public int SalerId { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public int DeptId { get; set; }

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
        /// 交通津贴
        /// </summary>
        public decimal JTJT { get; set; }

        /// <summary>
        /// 餐费津贴
        /// </summary>
        public decimal CFJT { get; set; }

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

    }
}
