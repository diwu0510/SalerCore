using System;
using System.Collections.Generic;
using System.Text;

namespace Zodo.Saler.Services
{
    /// <summary>
    /// 月报告创建DTO
    /// </summary>
    public class MonthReportCreateDto
    {
        /// <summary>
        /// 业务员ID
        /// </summary>
        public int SalerId { get; set; }

        /// <summary>
        /// 年度
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 月份
        /// </summary>
        public int Month { get; set; }
    }
}
