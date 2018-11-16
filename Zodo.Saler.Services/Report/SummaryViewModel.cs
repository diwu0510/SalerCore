using System;
using System.Collections.Generic;
using System.Text;

namespace Zodo.Saler.Services
{
    public class SummaryViewModel
    {
        /// <summary>
        /// 编组字段ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 编组字段名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 年
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 日期单位，月|季度
        /// </summary>
        public int DateUnit { get; set; }

        /// <summary>
        /// 合计
        /// </summary>
        public decimal HJ { get; set; }

        /// <summary>
        /// 账面毛利
        /// </summary>
        public decimal ZMML { get; set; }

        /// <summary>
        /// 毛利
        /// </summary>
        public decimal ML { get; set; }

        /// <summary>
        /// 业绩
        /// </summary>
        public decimal YJ { get; set; }

        /// <summary>
        /// 效益
        /// </summary>
        public decimal XY { get; set; }

        /// <summary>
        /// 毛利率
        /// </summary>
        public decimal MLL { get; set; }

        /// <summary>
        /// 销售费用占业绩
        /// </summary>
        public decimal XSFYZYJ { get; set; }
    }
}
